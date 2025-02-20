//
// Created by Kuriko on 2023/9/15.
//

#pragma once

#include <windows.h>
#include <filesystem>
#include <tchar.h>

#include "utils/kutils.h"
#include "utils/log.h"
#include "hooks/hookbase.h"
#include "kutils.h"

/*
HWND CreateWindowExW(
  [in]           DWORD     dwExStyle,
  [in, optional] LPCWSTR   lpClassName,
  [in, optional] LPCWSTR   lpWindowName,
  [in]           DWORD     dwStyle,
  [in]           int       X,
  [in]           int       Y,
  [in]           int       nWidth,
  [in]           int       nHeight,
  [in, optional] HWND      hWndParent,
  [in, optional] HMENU     hMenu,
  [in, optional] HINSTANCE hInstance,
  [in, optional] LPVOID    lpParam
);
 */

namespace Window::HookCreateWindowExW {

    HWND  WINAPI
    DetourFunction(
            IN DWORD dwExStyle,
            IN OPTIONAL LPCWSTR lpClassName,
            IN OPTIONAL LPCWSTR lpWindowName,
            IN DWORD dwStyle,
            IN int X,
            IN int Y,
            IN int nWidth,
            IN int nHeight,
            IN OPTIONAL HWND hWndParent,
            IN OPTIONAL HMENU hMenu,
            IN OPTIONAL HINSTANCE hInstance,
            IN OPTIONAL LPVOID lpParam
    );

    using FnType = decltype(&CreateWindowExW);

    static auto logger = Logger::GetLogger("File::HookCreateWindowExW");

    static class CreateWindowExWHook : public HookBase<FnType> {
    public:
        CreateWindowExWHook() : HookBase("User32.dll", "CreateWindowExW") {}

        void InitHook() override { BaseInitHook(DetourFunction); }


    private:
        std::wstring game_path_;

    } g_obj;


    HWND  WINAPI
    DetourFunction(
            IN DWORD dwExStyle,
            IN OPTIONAL LPCWSTR lpClassName,
            IN OPTIONAL LPCWSTR lpWindowName,
            IN DWORD dwStyle,
            IN int X,
            IN int Y,
            IN int nWidth,
            IN int nHeight,
            IN OPTIONAL HWND hWndParent,
            IN OPTIONAL HMENU hMenu,
            IN OPTIONAL HINSTANCE hInstance,
            IN OPTIONAL LPVOID lpParam
    ) {
        logger.Debug("CreateWindowExW called");
        auto orig_fn = g_obj.GetOrigFnPtr();

        // std::wstring append = L" KurikoMoe 测试用汉化（文本来自澄空）";

        HWND hwnd;
        std::wstring window_name;
        if (lpWindowName != nullptr) {
            window_name = lpWindowName;
            if (window_name.find(L"WITCH ON THE HOLY NIGHT") != std::wstring::npos) {
                std::wstring append = L"　澄空汉化／KurikoMoe 移植 (1.0.4)";
                // auto wsize = MultiByteToWideChar(CP_UTF8, 0, append.c_str(), append.length(), NULL, 0);
                // wchar_t* wbuf = new wchar_t[wsize];
                // MultiByteToWideChar(CP_UTF8, 0, append.c_str(), append.length(), wbuf, wsize);
                // window_name += std::wstring(wbuf);

                window_name += append;
                logger.Debug(window_name);
            }
            hwnd = orig_fn(dwExStyle, lpClassName, window_name.c_str(), dwStyle, X, Y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);
        } else {
            hwnd = orig_fn(dwExStyle, lpClassName, lpWindowName, dwStyle, X, Y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);
        }

        // SetWindowTextW(hwnd, window_name.c_str());
        logger.Debug("CreateWindowExW Finished");
        return hwnd;
    }
}