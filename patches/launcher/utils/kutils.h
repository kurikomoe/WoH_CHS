#pragma once

#include <cassert>
#include <cstdio>
#include <cstdlib>
#include <format>
#include <iostream>
#include <map>

#include <windows.h>
#include <winternl.h>

namespace kutils {

#ifndef IN
#define IN
#endif

#ifndef OUT
#define OUT
#endif

#ifndef IN_OPT
#define IN_OPT
#endif

#ifndef OUT_OPT
#define OUT_OPT
#endif

    const uint32_t UID_MARK = 1 << 31;

    namespace data {
        struct Cache {
            HANDLE hProcess;
            HANDLE hThread;

            DWORD64 dwAddressBase;

            std::map<std::string, HMODULE> hMoudleList;

            bool IsSame(HANDLE hProcess);

            DWORD64 GetImageBase(HANDLE hProcess);

            void AddModule(const std::string& name, HMODULE hModule);

            HMODULE GetModule(const std::string& name);

            ~Cache();
        };
        extern Cache g_cache;
    }

    void ShowErrorReasonMsgbox(DWORD errCode);

    template <bool isCritical = 1, typename F>
    bool MSAPICaller(F f)
    {
        ::SetLastError(0);
        if (f())
            return true;

        auto errCode = ::GetLastError();
        if (errCode == 0) {
            MessageBoxW(nullptr, L"Error when calling function but no last error id", L"Error", MB_ICONERROR | MB_OK);
            return false;
        }

        ShowErrorReasonMsgbox(errCode);

        if constexpr (isCritical) {
            exit(-1);
        }
        return false;
    }

    template <typename Fnew, typename Fdel>
    class ScopeGuard {
    public:
        ScopeGuard(const Fnew& f_new, const Fdel& f_del)
                : f_new_(f_new)
                , f_del_(f_del)
        {
            f_new_();
        }
        ScopeGuard(Fnew&& f_new, Fdel&& f_del)
                : f_new_(std::move(f_new))
                , f_del_(std::move(f_del))
        {
            f_new_();
        }
        ~ScopeGuard() { f_del_(); }

    private:
        Fnew f_new_;
        Fdel f_del_;
    };

// Helper functions

// Dynamic laod a function from a specific dll
    template <typename F, typename lpF = F*>
    lpF MSGetAPIFuncPtr(const char* DllName, const char* FuncName)
    {
        lpF func = nullptr;
        MSAPICaller([&] {
//            HMODULE hwnd = data::g_cache.GetModule(DllName);
//            if (hwnd == nullptr) {
//                hwnd = LoadLibrary(DllName);
//                if (hwnd == nullptr)
//                    return false;
//                data::g_cache.AddModule(std::string(DllName), hwnd);
//            }
            HMODULE hwnd = LoadLibrary(DllName);

            func = (lpF)(GetProcAddress(hwnd, FuncName));
            if (func == nullptr)
                return false;
            return true;
        });

        return func;
    }

    DWORD64 GetProcessImageBaseX64(HANDLE pProcess);

    DWORD32 GetProcessImageBaseX86(HANDLE pProcess);

    void CreateProcessSuspended(
            IN LPCWSTR lpApplicationPath,
            OUT LPPROCESS_INFORMATION lpProcessInformation,
            OUT LPSTARTUPINFOW lpStartupInformation);

    void ResumeProcessAndCleanup(const PROCESS_INFORMATION& pi);

    void WriteProcessMemory(HANDLE pProcess, DWORD64 offset, LPCVOID buf, size_t size);

    void ReadProcessMemory(HANDLE pProcess, DWORD64 offset, SIZE_T size,
                           OUT LPVOID pBuffer, OUT PSIZE_T lpNumberOfBytesRead);

    LPVOID SearchProcessMemory(HANDLE pProcess, LPCVOID lpBuffer, SIZE_T nSize);

    std::wstring GetFileNameByHandle(HANDLE hFile);

    void Assert(bool condition, const std::wstring& msg);
}
