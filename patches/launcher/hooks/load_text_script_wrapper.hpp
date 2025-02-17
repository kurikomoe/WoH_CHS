//
// Created by Kuriko on 2023/9/19.
//

#pragma once

#include <windows.h>
#include <format>

#include "utils/log.h"
#include "hooks/hookbase.h"
#include "hooks/data/data.h"
#include "load_text_script.hpp"

namespace Game::HookTextLoadWrapper {
    using namespace data;

    static auto logger = Logger::GetLogger("Game::HookLoadTextWrapper");

    using TFnType = int64_t (__fastcall *)(void** dst, void* a2, void* buffer, uint32_t nSize, int64_t a5, int64_t a6);

    int64_t __fastcall DetourFunction(void** dst, void* a2, void* buffer, uint32_t nSize, int64_t a5, int64_t a6);

    using HFnType = decltype(DetourFunction);

    static class TextLoadWrapperHook : public HookAddressBase<TFnType, HFnType> {
    public:
        TextLoadWrapperHook() : HookAddressBase(nullptr, 0xc5670) {
        }

        void InitHook() override {
            logger.Debug(L"TextLoadWrapperHook::InitHook called");
            BaseInitHook(DetourFunction);
            logger.Debug(std::format("TextLoadWrapperHook::InitHook: Address: 0x{:x}", (intptr_t) this->GetOrigFnPtr()));
        }
    } g_obj;

    int64_t __fastcall DetourFunction(void** dst, void* a2, void* buffer, uint32_t nSize, int64_t a5, int64_t a6) {
        // logger.Debug("Enter TextLoad wrapper");
        // logger.Debug("TextLoad wrapper called");

        auto ret = g_obj.GetOrigFnPtr()(dst, a2, buffer, nSize, a5, a6);

        // script_text_zc.ctd, size: 0xc4db5
        if (nSize == 0xc4db5) {
            logger.Debug(std::format("size: 0x{:x}, found script_text_zc.ctd in wrapper", nSize));
            if (Game::HookTextLoad::g_obj.new_buffer == nullptr) {
                logger.Debug("Invalid new_buffer address");
            } else {
                *dst = Game::HookTextLoad::g_obj.new_buffer;
            }
        }

        // logger.Debug("TextLoad wrapper Finished");
        return ret;
    }
}

