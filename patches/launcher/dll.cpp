#include <windows.h>
#include <iterator>
#include <fstream>

#include "utils/log.h"
// #include "hooks/media.hpp"
// #include "hooks/readfile.hpp"
// #include "hooks/getfilesizeex.hpp"
// #include "hooks/findentry.hpp"
// #include "hooks/readdisk.hpp"
// #include "hooks/movieplay.hpp"
#include "hooks/createwindowexw.hpp"
#include "hooks/load_text_script.hpp"
#include "hooks/load_text_script_wrapper.hpp"

#include "hooks/decompress_mzx.hpp"


// #include "anonymouscode_data/src/lib.rs.h"

void Init()
{
    // bool is_debug = true;
    bool is_debug = false;

    const auto* LOG_FILE = L"log.txt";
    const auto LOG_LEVEL = is_debug ? LogLevel::Debug : LogLevel::Silent;

    Logger::GetInstance().init(LOG_FILE, LOG_LEVEL);

    Logger::Info("----------- WoH CHS Started -------------");

    if (!is_debug)
        Window::HookCreateWindowExW::g_obj.InitHook();

    if (std::filesystem::exists("fonts/remapped.txt")) {
        Game::HookTextLoad::g_obj.AddData("fonts/remapped.txt");
    } else {
        // Use internal data
        Game::HookTextLoad::g_obj.AddData("");
    }
    Game::HookTextLoad::g_obj.InitHook();

    Game::HookTextLoadWrapper::g_obj.InitHook();

    Game::HookDecompressMZX::g_obj.InitHook();

    // MessageBoxA(nullptr, "HookTextLoadWrapper Hooked", "WoH CHS", MB_OK);

    Logger::Info("WoH CHS All Hooks Loaded");
}

void Destroy()
{
    // kdata::release_resource();
}


[[maybe_unused]]
BOOL WINAPI
DllMain(HINSTANCE hWnd, DWORD reason, LPVOID lpReserved)
{
    switch (reason)
    {
    case DLL_PROCESS_ATTACH:
        Init();
        break;
    case DLL_PROCESS_DETACH:
        Destroy();
        break;
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    default:
        break;
    }

    return TRUE;
}

// Detour needs at least on exported function
extern "C"
__declspec(dllexport)
int WINAPI KurikoPatches() { return 0; }
