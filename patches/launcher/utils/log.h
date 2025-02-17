//
// Created by Kuriko on 2023/9/15.
//

#ifndef HOOKTEST_LOG_H
#define HOOKTEST_LOG_H

#include <iostream>
#include <fstream>
#include <chrono>
#include <mutex>


enum class LogLevel: int {
    Debug = 0,
    Trace,
    Info,
    Warn,
    Error,

    // Suspend log output.
    Silent = 255,
};

static const wchar_t* LogLevelToString(LogLevel level) {
    switch (level) {
        case LogLevel::Debug:
            return L"Debug";
        case LogLevel::Trace:
            return L"Trace";
        case LogLevel::Info:
            return L"Info";
        case LogLevel::Warn:
            return L"Warn";
        case LogLevel::Error:
            return L"Error";
        case LogLevel::Silent:
            return L"Silent";
        default:
            return L"Unknown";
    }
}

class Logger {
public:
    static Logger& GetInstance() {
        static Logger instance;
        return instance;
    }

    class TaggedLogger {
    public:
        template<typename T, std::enable_if<std::is_convertible_v<T, std::wstring>, bool> = true>
        explicit TaggedLogger(const T& TAG): tag_(TAG) { };

        explicit TaggedLogger(const char* TAG) {
            std::string_view tag(TAG);
            tag_ = { tag.begin(), tag.end() };
        };

        template<typename T>
        void Debug(const T& msg) { GetInstance().debug(msg, tag_); }

        template<typename T>
        void Trace(const T& msg) { GetInstance().trace(msg, tag_); }

        template<typename T>
        void Info(const T& msg) { GetInstance().info(msg, tag_); }

        template<typename T>
        void Error(const T& msg) { GetInstance().error(msg, tag_); }

        template<typename T>
        void Warn(const T& msg) { GetInstance().warn(msg, tag_); }

    private:
        std::wstring tag_;
    };

    template<typename T>
    static TaggedLogger GetLogger(const T& TAG) {
        return TaggedLogger(TAG);
    }


    template<typename T>
    static void Debug(const T& msg) { GetInstance().debug(msg); }

    template<typename T>
    static void Trace(const T& msg) { GetInstance().trace(msg); }

    template<typename T>
    static void Info(const T& msg) { GetInstance().info(msg); }

    template<typename T>
    static void Error(const T& msg) { GetInstance().error(msg); }

    template<typename T>
    static void Warn(const T& msg) { GetInstance().warn(msg); }

public:
    Logger(): isInitialized(false), level_(LogLevel::Debug) {}

    void init(const std::wstring& filename, LogLevel level = LogLevel::Silent) {
        isInitialized = true;
        level_ = level;
        if (level != LogLevel::Silent) {
            log_file_.open(filename, std::ios::out);
        }
    }

    template<typename T>
    void debug(const T& msg, std::wstring_view tag = L"") { write_line(tag, msg, LogLevel::Debug); }

    template<typename T>
    void trace(const T& msg, std::wstring_view tag = L"") { write_line(tag, msg, LogLevel::Trace); }

    template<typename T>
    void info(const T& msg, std::wstring_view tag = L"") { write_line(tag, msg, LogLevel::Info); }

    template<typename T>
    void error(const T& msg, std::wstring_view tag = L"") { write_line(tag, msg, LogLevel::Error); }

    template<typename T>
    void warn(const T& msg, std::wstring_view tag = L"") { write_line(tag, msg, LogLevel::Warn); }

private:
    template<typename T>
    void write_line(std::wstring_view tag, const T& msg, LogLevel level) {
        if (!isInitialized || level < level_) { return; }

        std::wstring _msg;
        if constexpr (std::is_convertible_v<T, std::string_view>) {
            auto tmp = std::string_view(msg);
            _msg = std::wstring(tmp.begin(), tmp.end());
        } else {
            _msg = std::wstring_view(msg);
        }

        std::lock_guard<std::mutex> guard(log_mutex_);

        using namespace std::chrono;
        auto local = std::chrono::zoned_time{current_zone(), system_clock::now()};
        log_file_ << std::format(L"[{}][{:%T}]", LogLevelToString(level), local);
        if (!tag.empty()) {
            log_file_ << std::format(L"[{}]", tag);
        }
        log_file_ << std::format(L": {}", _msg) << std::endl;
        // do flush so that we can see logs immediately
        log_file_.flush();
    }

private:
    bool isInitialized = false;

    std::wofstream log_file_;
    LogLevel level_;

    std::mutex log_mutex_;
};

// exported functions for ffi

// namespace kdata {
//     void debug(rust::Str msg);

//     void trace(rust::Str msg);

//     void info(rust::Str msg);

//     void error(rust::Str msg);

//     void warn(rust::Str msg);
// }


#endif //HOOKTEST_LOG_H
