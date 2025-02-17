#!/usr/bin/env python

from struct import unpack, pack


def mzx0_compress(f, inlen, xorff=False):
    """Compress a block of data.
    """
    dout = bytearray(b'MZX0')
    dout.extend(pack('<L', inlen))

    key = 0xFFFFFFFF if xorff else 0
    while inlen >= 0x80:
        # 0xFF literal, write plaintext input 0x80 bytes // 0xFF = 0xFC+0x03 // (0xFC >> 2 + 1) => 0x40 times 2-byte pairs
        dout.extend(b'\xFF')
        for i in range(0x20):
            dout.extend(pack('<L', unpack('<L', f.read(0x4))[0] ^ key))
        inlen -= 0x80

    key = 0xFF if xorff else 0
    if inlen >= 0x02: # 0x02-0x7F remainder
        dout.extend(bytes([ ((inlen >> 1) - 1) * 4 + 3 ]))
        for i in range((inlen >> 1) * 2):
            dout.extend(bytes([ ord(f.read(1)) ^ key ]))
        inlen -= (inlen >> 1) * 2

    if inlen == 0x01: # pad with useless character
        dout.extend(b'\x03')
        dout.extend(bytes([ ord(f.read(1)) ^ key ]))
        dout.extend(b'\x00')

    return dout
