#include "json.hpp"
#include <cstdio>
#include <cstdlib>
#include <cstdint>
#include <cassert>
#include <string>
#include <iostream>

struct CCITFontIndex {
    uint32_t ch_raw;  // big endian, no leading zeros
    uint32_t index;

    std::string get_char() {
        uint32_t data = ch_raw;
        std::string ret;
        while (data & 0xFF) {
            unsigned char ch = data & 0xFF;
            ret.insert(ret.begin(), ch);
            data >>= 8;
        }
        return ret;
    }
};

struct CCITFile {
    uint32_t total;
    uint32_t aUn;
    // = { 0xF9, 0x04, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00 };
    CCITFontIndex fonts[];
};


int main(int argc, char *argv[]) {
    if (argc != 2) {
        puts("usage: ./ccit ccitfile");
        return 1;
    }

    FILE *fp = fopen(argv[1], "rb");
    if (fp == NULL) {
        puts("cannot open file");
        return 1;
    }

    fseek(fp, 0, SEEK_END);
    size_t fsize = ftell(fp);
    fseek(fp, 0, SEEK_SET);

    void* buf = new unsigned char[fsize];

    size_t nsize = fread(buf, fsize, 1, fp);

    CCITFile* ptr = (CCITFile*)buf;

    printf("total: %d\n", ptr->total);
    printf("aUn: %x\n", ptr->aUn);

    for (auto i = 0; i < ptr->total; i++) {
        printf("i: %d, index: %d0\n", i, ptr->fonts[i].index);
        std::cout << ptr->fonts[i].get_char() << std::endl;
    }

    return 0;
}