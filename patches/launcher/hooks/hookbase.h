//
// Created by Kuriko on 2023/9/15.
//

#ifndef HOOKTEST_HOOKBASE_H
#define HOOKTEST_HOOKBASE_H

#include <windows.h>
#include <detours/detours.h>

#include "log.h"
#include "kutils.h"

template<typename HookFnType>
class HookBase {
public:
    using FnType = HookFnType;

public:
    HookBase(const char* module_name, const char* func_name):
        module_name_(module_name), func_name_(func_name) {
    }


    virtual void InitHook() = 0;

    void BaseInitHook(FnType detour_fn) {
        HMODULE hModule = GetModuleHandle(module_name_);
        orig_fn_ = (FnType)GetProcAddress(hModule, func_name_);

        DetourRestoreAfterWith();
        DetourTransactionBegin();
        DetourUpdateThread(GetCurrentThread());
        DetourAttach(&(PVOID&)orig_fn_, (PVOID) detour_fn);
        DetourTransactionCommit();
    };

    FnType GetOrigFnPtr() { return orig_fn_; }

private:
    const char* module_name_;
    const char* func_name_;

protected:
    FnType orig_fn_ = nullptr;
};


//typedef char ( __thiscall * tPlayerJoin )( void *, int, const void );
//tPlayerJoin oPlayerJoin;
//
//typedef char ( __thiscall * tPlayerQuit )( void *, int, int );
//tPlayerQuit oPlayerQuit;
//
//char __fastcall hPlayerJoin( void * This, void * _EDX, int playerIndex, const void * a3 )
//{
//    // ...
//}
//
//char __fastcall hPlayerQuit( void * This, void * _EDX, int playerIndex, int a3 )
//{
//    // ...
//}

template<typename TargetFnType, typename HookFnType>
class HookAddressBase {
public:
    HookAddressBase(const char* module_name, intptr_t func_address):
        module_name_(module_name), func_address_(func_address) {
    }


    virtual void InitHook() = 0;

    void BaseInitHook(HookFnType detour_fn) {
        HMODULE hModule = GetModuleHandle(module_name_);
        auto dwImageBase = reinterpret_cast<intptr_t>(hModule);
        kutils::Assert(dwImageBase!= 0, L"Invalid image base 0");
        Logger::Debug(std::format(L"dwImageBase: 0x{:x}", dwImageBase));
        orig_fn_ = (TargetFnType)(dwImageBase + func_address_);

        DetourRestoreAfterWith();
        DetourTransactionBegin();
        DetourUpdateThread(GetCurrentThread());
        DetourAttach(&(PVOID&)orig_fn_, (PVOID) detour_fn);
        DetourTransactionCommit();
    };

    TargetFnType GetOrigFnPtr() { return orig_fn_; }

private:
    const char* module_name_;
    intptr_t func_address_;

protected:
    TargetFnType orig_fn_ = nullptr;
};

#endif //HOOKTEST_HOOKBASE_H
