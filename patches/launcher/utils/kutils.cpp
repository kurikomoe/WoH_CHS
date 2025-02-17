#include <cassert>
#include <cstdio>
#include <cstdlib>
#include <format>
#include <iostream>
#include <map>

#include <windows.h>
#include <winternl.h>

#include "kutils.h"
#include "log.h"

namespace kutils {

    namespace data {

        bool Cache::IsSame(HANDLE hProcess)
        {
            return this->hProcess == hProcess;
        }

        DWORD64 Cache::GetImageBase(HANDLE hProcess)
        {
            if (IsSame(hProcess))
                return data::g_cache.dwAddressBase;
            else
                return GetProcessImageBaseX64(hProcess);
        }

        void Cache::AddModule(const std::string& name, HMODULE hModule)
        {
            std::cout << "AddModule: " << name << std::endl;
            assert(hMoudleList.find(name) == hMoudleList.end());
            hMoudleList[name] = hModule;
        }

        HMODULE Cache::GetModule(const std::string& name)
        {
            std::cout << "GetModule: " << name << std::endl;
            if (hMoudleList.count(name) == 0)
                return nullptr;
            else
                return hMoudleList.at(name);
        }

        Cache::~Cache()
        {
            for (auto& [libname, hModule] : this->hMoudleList) {
                std::cout << "FreeLibrary: " << libname << std::endl;
                FreeLibrary(hModule);
            }
        }

        Cache g_cache = { 0 };
    }

    void ShowErrorReasonMsgbox(DWORD errCode)
    {
        // WINBASEAPI
        // _Success_(return != 0)
        // DWORD
        // WINAPI
        // FormatMessageA(
        //     _In_     DWORD dwFlags,
        //     _In_opt_ LPCVOID lpSource,
        //     _In_     DWORD dwMessageId,
        //     _In_     DWORD dwLanguageId,
        //     _When_((dwFlags & FORMAT_MESSAGE_ALLOCATE_BUFFER) != 0, _At_((LPSTR*)lpBuffer, _Outptr_result_z_))
        //     _When_((dwFlags & FORMAT_MESSAGE_ALLOCATE_BUFFER) == 0, _Out_writes_z_(nSize))
        //              LPSTR lpBuffer,
        //     _In_     DWORD nSize,
        //     _In_opt_ va_list *Arguments
        //     );
        LPSTR msgBuf = nullptr; // NULL terminated
        size_t size = FormatMessageA(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
                                     NULL,
                                     errCode, // dwMessageId
                                     MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), // dwLanguageId
                                     (LPSTR)&msgBuf,
                                     0,
                                     NULL);

        auto msg = std::format("Call failed, ErrCode: {}\nErr Msg: {}", errCode, msgBuf);
        MessageBoxA(nullptr, msg.c_str(), NULL, MB_OK);

        LocalFree(msgBuf);
        return;
    }

    DWORD64 GetProcessImageBaseX64(HANDLE hProcess)
    {
        // Using cache
        PROCESS_BASIC_INFORMATION pbi {};
        DWORD64 dwImageBase = 0;

        MSAPICaller<1>([&] {
            auto func = MSGetAPIFuncPtr<decltype(NtQueryInformationProcess)>("ntdll.dll", "NtQueryInformationProcess");

            if (0 != func(hProcess, ProcessBasicInformation, &pbi, sizeof(pbi), nullptr)) {
                Logger::Debug(L"Calling NtQueryInformationProcess Failed") ;
                return false;
            }

            // https://snoozy.hatenablog.com/entry/2019/12/23/212835
#ifdef WIN32
            auto imageBaseAddr = (DWORD64)pbi.PebBaseAddress + 0x8;
#else
            auto imageBaseAddr = (DWORD64)pbi.PebBaseAddress + 0x10;
#endif
            Logger::Debug(std::format(L"PEB: {:X}", (DWORD64)pbi.PebBaseAddress));

            SIZE_T bytesRead = 0;
            auto ret = ::ReadProcessMemory(hProcess,
                                           (LPCVOID)imageBaseAddr,
                                           &dwImageBase,
                                           sizeof(dwImageBase),
                                           &bytesRead);
            return ret != 0 && bytesRead == sizeof(dwImageBase);
        });

        data::g_cache = {
            .hProcess = hProcess,
            .dwAddressBase = dwImageBase,
        };

        return dwImageBase;
    }

    DWORD32
    GetProcessImageBaseX86(HANDLE hProcess)
    {
        // Old for win32
        WOW64_CONTEXT regs {};
        // CONTEXT regs {};
        regs.ContextFlags = CONTEXT_ALL;

        DWORD32 dwImageBase = 0;

        MSAPICaller([&] {
            // WINBASEAPI
            // BOOL
            // WINAPI
            // GetThreadContext(
            //     _In_ HANDLE hThread,
            //     _Inout_ LPCONTEXT lpContext
            //     );
            if (0 != GetThreadContext(hProcess, (LPCONTEXT)(&regs)))
                return false;

            // BOOL ReadProcessMemory(
            //     [in]  HANDLE  hProcess,
            //     [in]  LPCVOID lpBaseAddress,
            //     [out] LPVOID  lpBuffer,
            //     [in]  SIZE_T  nSize,
            //     [out] SIZE_T  *lpNumberOfBytesRead
            // );
            auto ret = ::ReadProcessMemory(hProcess,
                                           LPVOID((DWORD64)regs.Ebx + 0x8),
                                           &dwImageBase,
                                           sizeof(dwImageBase),
                                           NULL);
            return ret != 0;
        });

        data::g_cache = {
                .hProcess = hProcess,
                .dwAddressBase = dwImageBase,
        };

        return dwImageBase;
    }

    void CreateProcessSuspended(
            IN LPCWSTR lpApplicationPath,
            OUT LPPROCESS_INFORMATION lpProcessInformation,
            OUT LPSTARTUPINFOW lpStartupInformation)
    {
        ZeroMemory(lpProcessInformation, sizeof(PROCESS_INFORMATION));
        ZeroMemory(lpStartupInformation, sizeof(STARTUPINFOW));

        lpStartupInformation->cb = sizeof(STARTUPINFOW);

        // Create Process and suspend
        // Use Scope Guard to ensure resource is processed.
        MSAPICaller<1>([&] {
            // WINBASEAPI
            // BOOL
            // WINAPI
            // CreateProcessW(
            //     _In_opt_ LPCWSTR lpApplicationName,
            //     _Inout_opt_ LPWSTR lpCommandLine,
            //     _In_opt_ LPSECURITY_ATTRIBUTES lpProcessAttributes,
            //     _In_opt_ LPSECURITY_ATTRIBUTES lpThreadAttributes,
            //     _In_ BOOL bInheritHandles,
            //     _In_ DWORD dwCreationFlags,
            //     _In_opt_ LPVOID lpEnvironment,
            //     _In_opt_ LPCWSTR lpCurrentDirectory,
            //     _In_ LPSTARTUPINFOW lpStartupInfo,
            //     _Out_ LPPROCESS_INFORMATION lpProcessInformation
            //     );
            auto ret = CreateProcessW(lpApplicationPath,
                                      NULL,
                                      NULL,
                                      NULL,
                                      FALSE,
                                      CREATE_SUSPENDED,
                                      NULL,
                                      NULL,
                                      lpStartupInformation,
                                      lpProcessInformation);
            return 0 != ret;
        });

        GetProcessImageBaseX64(lpProcessInformation->hProcess);

        return;
    }

    void ResumeProcessAndCleanup(const PROCESS_INFORMATION& pi)
    {
        ResumeThread(pi.hThread);
        CloseHandle(pi.hProcess);
        CloseHandle(pi.hThread);
    }

    void WriteProcessMemory(HANDLE hProcess, DWORD64 offset, LPCVOID buf, size_t size)
    {
        MSAPICaller<1>([&] {
            DWORD oldProtect = 0;

            DWORD64 dwImageBase = data::g_cache.GetImageBase(hProcess);

            DWORD64 addr = dwImageBase + offset;

            // Remove the page protection.
            // WINBASEAPI
            // _Success_(return != FALSE)
            // BOOL
            // WINAPI
            // VirtualProtectEx(
            //     _In_ HANDLE hProcess,
            //     _In_ LPVOID lpAddress,
            //     _In_ SIZE_T dwSize,
            //     _In_ DWORD flNewProtect,
            //     _Out_ PDWORD lpflOldProtect
            // );
            if (0 == ::VirtualProtectEx(hProcess, (LPVOID)addr, size, PAGE_EXECUTE_READWRITE, &oldProtect))
                return false;

            // WINBASEAPI
            // _Success_(return != FALSE)
            // BOOL
            // WINAPI
            // WriteProcessMemory(
            //     _In_ HANDLE hProcess,
            //     _In_ LPVOID lpBaseAddress,
            //     _In_reads_bytes_(nSize) LPCVOID lpBuffer,
            //     _In_ SIZE_T nSize,
            //     _Out_opt_ SIZE_T* lpNumberOfBytesWritten
            // );
            SIZE_T bytesWritten = 0;
            auto ret = ::WriteProcessMemory(hProcess, (LPVOID)addr, buf, size, &bytesWritten);
            return ret != 0 && bytesWritten == size;
        });

        return;
    }

    void ReadProcessMemory(HANDLE hProcess, DWORD64 offset, SIZE_T size,
                           OUT LPVOID pBuffer, OUT PSIZE_T lpNumberOfBytesRead)
    {
        DWORD64 dwImageBase = data::g_cache.GetImageBase(hProcess);

        MSAPICaller<1>([&] {
            DWORD oldProtect = 0;
            DWORD64 addr = dwImageBase + offset;

            // Remove the page protection.
            // WINBASEAPI
            // _Success_(return != FALSE)
            // BOOL
            // WINAPI
            // VirtualProtectEx(
            //     _In_ HANDLE hProcess,
            //     _In_ LPVOID lpAddress,
            //     _In_ SIZE_T dwSize,
            //     _In_ DWORD flNewProtect,
            //     _Out_ PDWORD lpflOldProtect
            // );
            if (0 == ::VirtualProtectEx(hProcess, (LPVOID)addr, size, PAGE_EXECUTE_READWRITE, &oldProtect))
                return false;

            // WINBASEAPI
            // _Success_(return != FALSE)
            // BOOL
            // WINAPI
            // ReadProcessMemory(
            //     _In_ HANDLE hProcess,
            //     _In_ LPCVOID lpBaseAddress,
            //     _Out_writes_bytes_to_(nSize,*lpNumberOfBytesRead) LPVOID lpBuffer,
            //     _In_ SIZE_T nSize,
            //     _Out_opt_ SIZE_T* lpNumberOfBytesRead
            //     );
            auto ret = ::ReadProcessMemory(hProcess, (LPCVOID)addr, pBuffer, size, lpNumberOfBytesRead);
            return ret != 0;
        });

        return;
    }

    LPVOID SearchProcessMemory(HANDLE hProcess, LPCVOID lpBuffer, SIZE_T nSize)
    {
        // TODO(kuriko): search the process memory by specific pattern, return the address;
        DWORD64 dwImageBase = data::g_cache.GetImageBase(hProcess);

        return nullptr;
    }

    std::wstring GetFileNameByHandle(HANDLE hFile) {
        WCHAR buffer[255];
        GetFinalPathNameByHandleW(hFile, (LPWSTR)buffer, sizeof(buffer), NULL);
        // FIXME(kuriko): Remove the leading '\\?\' in path, Dangerous!
        return { buffer+4 };
    }

    void Assert(bool condition, const std::wstring& msg) {
        if (condition) return;

        MessageBoxW(nullptr, msg.c_str(), L"Assert", MB_OK);

        exit(-1);
    }
}
