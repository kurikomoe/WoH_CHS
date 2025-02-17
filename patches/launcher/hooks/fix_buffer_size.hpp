//
// Created by Kuriko on 2023/9/19.
//

#pragma once

#include <windows.h>
#include <format>

#include "utils/log.h"
#include "hooks/hookbase.h"
#include "hooks/data/data.h"
#include "hooks/load_text_script.hpp"
#include <intrin.h>


namespace Game::HookFixBufferSize {
    using namespace data;

    static auto logger = Logger::GetLogger("Game::HookFixBufferSize");

    using TFnType = int64_t (__fastcall *)(int32_t* pOut, void* buf, int offset, unsigned int a4, int a5);

    int64_t __fastcall DetourFunction(int32_t* pOut, void* buf, int offset, unsigned int a4, int a5);

    using HFnType = decltype(DetourFunction);

    static class FindEntryHook : public HookAddressBase<TFnType, HFnType> {
    public:
        FindEntryHook() : HookAddressBase(nullptr, 0x175110) { }

        void InitHook() override {
            logger.Debug(L"HookFindEntry::InitHook called");
            BaseInitHook(DetourFunction);
            logger.Debug(std::format("HookFindEntry::InitHook: Address: 0x{:x}", (intptr_t) this->GetOrigFnPtr()));
        }

        void addData(const std::vector<char>& data) { data_ = data; }

        std::vector<char> data_;

        void* new_buffer = nullptr;

    } g_obj;

    int64_t __fastcall DetourFunction(int32_t* pOut, void* buf, int offset, unsigned int a4, int a5) {

        auto ret = g_obj.GetOrigFnPtr()(pOut, buf, offset, a4, a5);

        int64_t* src_buf = (int64_t*)(*((int64_t*)buf + 0));
        int32_t src_size = *((int32_t*)buf + 2);

        // script_text_zc.ctd, size: 0xc4db5
        if (src_size == 0xc4db5) {
            logger.Debug(std::format("Found get size: {:x}", *pOut));
            // *pOut = Game::HookTextLoad::g_obj.data_.size() + 3;
        }

        return ret;
    }
}

