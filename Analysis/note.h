#include <cstdint>
#include <windows.h>

struct FontHInfo {
    char* filelist[3];
};

struct FontNormalInfo {
    char* filelist[36];
};

struct FontItalicInfo {
    char* filelist[4];
};

struct FontFont3Info {
    char* filelist[14];
};

struct FontInfo {
    FontHInfo* pFontH;
    FontNormalInfo* pFontNormal;
    FontItalicInfo* pFontItalic;
    FontFont3Info* pFontFont3;
};


/*
fontxxxxx.ccit  :

H Normal Italic Font3
jp en zc zt

字符映射表        字库集合    数量
20000.ccit  zc  H          2   1,2
20001.ccit  zc  Normal     35  3-37
20100.ccit  zc  Italic     3   38-40
20200.ccit  zc  Font3      13  41-53

*/

struct CCITFile {
    unsigned char hexData[8] = { 0xF9, 0x04, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00 };

    CCITFontIndex fonts[];
};

struct CCITFontIndex {
    int32_t ch;  // big endian, no leading zeros
    int32_t index;
};

/*
变 b'\xe5\x8f\x98'  0xDE0
她 b'\xe5\xa5\xb9'  0x1898

*/

struct A2 {
    BYTE  aUn1[24];
    struct {
        BYTE  aUn2[8];       // 24
        DWORD dwUn3;        // 32
        DWORD dwUn4;        // 36
        BYTE  aUn5[4];      // 40
        DWORD dwV48;       // 44
        DWORD dwUn6;        // 48
        DWORD dwBmpType;   // 52
        DWORD dwUn7;        // 56
        DWORD dwUn8;        // 60
        BYTE  dwUn9;        // 64  tile_corp
    } v8;
};