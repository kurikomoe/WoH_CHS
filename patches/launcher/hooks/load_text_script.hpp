//
// Created by Kuriko on 2023/9/19.
//

#pragma once

#include <windows.h>
#include <format>
#include <map>

#include <nlohmann/json.hpp>

#include "utils/log.h"
#include "hooks/hookbase.h"
#include "hooks/data/data.h"

// #include "data/mapping.h"
// #include "data/woh_chs_fixed.h"
#include "data/remapped.h"

using json = nlohmann::json;

namespace Game::HookTextLoad {
    using namespace data;

    static auto logger = Logger::GetLogger("Game::HookLoadText");

    using TFnType = int64_t (__fastcall *)(void* dst, void* src);

    int64_t __fastcall DetourFunction(void* dst, void* src);

    using HFnType = decltype(DetourFunction);

    static class TextLoadHook : public HookAddressBase<TFnType, HFnType> {
    public:
        TextLoadHook() : HookAddressBase(nullptr, 0x174c50) {
            // std::string json_data;
            // if (std::filesystem::exists(L"fonts/mapping.json")) {
            //     auto file = std::fstream(L"fonts/mapping.json", std::ios::in);
            //     file >> json_data;
            // } else {
            //     const bin2cpp::File& data_file = bin2cpp::getMappingJsonFile();
            //     json_data = const_cast<char*>(data_file.getBuffer());
            // }

            // auto j = json::parse(json_data);
            // for (auto& it : j.items()) {
            //     std::string ch_chengkong = it.key();
            //     std::string ch_orig = it.value()["ch_orig"];

            //     mapping_[ch_chengkong] = ch_orig;
            // }
        }

        void InitHook() override {
            logger.Debug(L"TextLoadHook::InitHook called");
            BaseInitHook(DetourFunction);
            logger.Debug(std::format("TextLoadHook::InitHook: Address: 0x{:x}", (intptr_t) this->GetOrigFnPtr()));
        }

        void AddData(std::string filename) {
            if (filename != "") {
                auto file = std::fstream(filename, std::ios::binary | std::ios::in);
                file.seekg(0, std::ios::end);
                size_ = (size_t)file.tellg();
                file.seekg(0, std::ios::beg);
                data_ = new char[size_];
                file.read(data_, size_);
            } else {
                const bin2cpp::File& data_file = bin2cpp::getRemappedTxtFile();
                data_ = const_cast<char*>(data_file.getBuffer());
                size_ = data_file.getSize();
            }
        }

        char* data_;
        size_t size_;

        void* new_buffer = nullptr;

        std::map<std::string, std::string> mapping_;
    } g_obj;

    int64_t __fastcall DetourFunction(void* dst, void* src) {
        // logger.Debug("TextLoad called");

        auto ret = g_obj.GetOrigFnPtr()(dst, src);

        int64_t* src_buf = (int64_t*)(*((int64_t*)src + 0));
        int32_t src_size = *((int32_t*)src + 2);

        int64_t* dst_buf = (int64_t*)(*((int64_t*)dst + 0));

        // script_text_zc.ctd, size: 0xc4db5
        if (src_size == 0xc4db5) {
            // MessageBoxA(NULL, "Debug", "", MB_OK);

            // logger.Debug(std::format("size: 0x{:x}, found script_text_zc.ctd", src_size));

            auto new_buf_size = g_obj.size_ + 100; // add \00 incase that overflow
            g_obj.new_buffer = new char[new_buf_size];
            memset(g_obj.new_buffer, 0, sizeof(char) * new_buf_size);
            char* buf = (char*)g_obj.new_buffer;

            memcpy(buf, g_obj.data_, g_obj.size_);

            // auto wsize = MultiByteToWideChar(CP_UTF8, 0, buf, 400, NULL, 0);
            // wchar_t* wbuf = new wchar_t[wsize];
            // MultiByteToWideChar(CP_UTF8, 0, buf, 400, wbuf, wsize);
            // MessageBoxW(NULL, wbuf, NULL, MB_OK);

            // logger.Debug("TextLoad hook finished");
            return new_buf_size;
        }

        // logger.Debug("TextLoad finished");
        return ret;
    }
}

