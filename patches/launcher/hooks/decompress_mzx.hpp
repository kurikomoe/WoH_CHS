//
// Created by Kuriko on 2023/9/19.
//

#pragma once

#include <windows.h>
#include <format>
#include <fstream>
#include <filesystem>

#include "utils/log.h"
#include "hooks/hookbase.h"
#include "hooks/data/data.h"

namespace fs = std::filesystem;


namespace Game::HookDecompressMZX
{
    using namespace data;

    const int64_t SIZE = 1024*1024;

    static auto logger = Logger::GetLogger("Game::HookDecompressMZX");

    using TFnType = int64_t (__fastcall *)(void** ppBuf, int32_t* dwSize, int32_t* a3, int64_t* pSrcBuf);

    int64_t __fastcall DetourFunction(void** ppBuf, int32_t* dwSize, int32_t* a3, int64_t* pSrcBuf);

    using HFnType = decltype(DetourFunction);

    static class DecompressMZXHook : public HookAddressBase<TFnType, HFnType>
    {
    public:
        DecompressMZXHook() : HookAddressBase(nullptr, 0x15EE40) {
            fs::path fonts ("fonts");

            for (int i = 1; i <= 53; i++) {
                auto filename = std::format("font_replace-{}.bin", i);
                logger.Debug(std::format("Loading {}", filename));

                fs::path font_file(filename);
                if (std::filesystem::exists(fonts / font_file)) {
                    auto file = std::fstream(fonts / font_file, std::ios::binary | std::ios::in);
                    font_data_[i] = new char[SIZE];
                    file.read(font_data_[i], SIZE);
                }
            }
        }

        void InitHook() override
        {
            logger.Debug(L"InitHook called");
            BaseInitHook(DetourFunction);
            logger.Debug(std::format("InitHook: Address: 0x{:x}", (intptr_t)this->GetOrigFnPtr()));
        }

        int dump_file_cnt_ = 0;

        char* font_data_[255] = {0};
    } g_obj;

    int64_t __fastcall DetourFunction(void** ppBuf, int32_t* dwSrcSize, int32_t* a3, int64_t* pSrcBuf)
    {

        auto ret = g_obj.GetOrigFnPtr()(ppBuf, dwSrcSize, a3, pSrcBuf);

        char* pBuf = (char*)*ppBuf;
        uint64_t nSize = *(uint64_t*)dwSrcSize;

        auto idx = g_obj.dump_file_cnt_;
        if (nSize == SIZE && g_obj.dump_file_cnt_ != -1) {
            auto* ptr = g_obj.font_data_[idx];
            if (idx >= 1 && idx <= 53 && ptr != nullptr) {
                memcpy(pBuf, ptr, SIZE);
            } else if (idx > 53) {
                g_obj.dump_file_cnt_ = -1;
            }
            g_obj.dump_file_cnt_++;
            // auto filename = std::format("dump-{}.bin", g_obj.dump_file_cnt++);
            // logger.Debug(filename);
            // std::fstream file(filename, std::ios::out);
            // file.write(pBuf, nSize);
            // file.flush();
            // file.close();
        }
        return ret;
    }
}
