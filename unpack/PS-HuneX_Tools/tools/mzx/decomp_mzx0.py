#!/usr/bin/env python3

# MZX Decompress Library
# Pure Python. Based on Mzx.cpp from ExtractData.
#
# FOR INTERNAL USE ONLY.
#
# Copyright (c) 2018 <hintay@me.com>

from io import BytesIO


def mzx0_decompress(f, inlen, exlen, xorff=False) -> [str, BytesIO]:
    """
    Decompress a block of data.
    """
    key = 0xFF

    out_data = BytesIO()  # slightly overprovision for writes past end of buffer
    ring_buf = [b'\xFF\xFF'] * 64 if xorff else [b'\x00\x00'] * 64
    ring_wpos = 0

    clear_count = 0
    max = f.tell() + inlen
    last = b'\xFF\xFF' if xorff else b'\x00\x00'

    while out_data.tell() < exlen:
        if f.tell() >= max:
            break
        if clear_count <= 0:
            clear_count = 0x1000
            last = b'\xFF\xFF' if xorff else b'\x00\x00'
        flags = ord(f.read(1))
        # print("+ %X %X %X" % (flags, f.tell(), out_data.tell()))

        clear_count -= 1 if (flags & 0x03) == 2 else flags // 4 + 1

        if flags & 0x03 == 0:
            out_data.write(last * ((flags // 4) + 1))

        elif flags & 0x03 == 1:
            k = 2 * (ord(f.read(1)) + 1)
            for i in range(flags // 4 + 1):
                out_data.seek(-k, 1)
                last = out_data.read(2)
                out_data.seek(0, 2)
                out_data.write(last)

        elif flags & 0x03 == 2:
            last = ring_buf[flags // 4]
            out_data.write(last)

        else:
            for i in range(flags // 4 + 1):
                last = ring_buf[ring_wpos] = bytes([byte ^ key for byte in f.read(2)]) if xorff else f.read(2)
                out_data.write(last)

                ring_wpos += 1
                ring_wpos %= 64
    status = "OK"

    out_data.truncate(exlen)  # Resize stream to decompress size
    out_data.seek(0)
    return [status, out_data]
