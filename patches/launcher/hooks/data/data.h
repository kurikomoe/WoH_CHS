//
// Created by Kuriko on 2023/9/20.
//

#ifndef HOOKTEST_DATA_H
#define HOOKTEST_DATA_H

#include <cstdint>
#include <string>

namespace data {
#pragma pack(push, 1)

    struct StdString {
        char aStr[16]{0};
        uint32_t uiStrLen = 0;
        uint32_t uiReserved = 0;

        StdString(const std::string &str) {
            uiStrLen = static_cast<uint32_t>(str.size());
            if (uiStrLen <= 16) {
                memcpy(aStr, str.c_str(), uiStrLen);
            } else {
                auto *pTemp = new char[uiStrLen];
                memcpy(pTemp, str.c_str(), uiStrLen);
                ((char **) aStr)[0] = pTemp;
            }
        }
    };

#pragma pack(pop)

#pragma pack(push, 1)
    struct MAG_FileRead_Entry {
        uint32_t uiID;
        StdString msPackName;
        StdString msFileName;
        StdString msFolderName;
        uint32_t aUn0[5];
        uint32_t uiOffsetLow;
        uint32_t uiOffsetHigh;
        uint32_t uiSizeLow;
        uint32_t uiSizeHigh;
        uint32_t uiUn0;
        uint32_t uiUn1;
    };
#pragma pack(pop)
}


#endif //HOOKTEST_DATA_H
