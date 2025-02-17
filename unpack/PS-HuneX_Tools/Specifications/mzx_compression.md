 The MZX compression algorithm
===============================
> Revision 1, maintainer: Waku_Waku

> Reverse-engineered by anonymous in http://mimizun.com/log/2ch/leaf/1141063964/#191 (Mzx.cpp)

All fields are little-endian unless specified otherwise.


 Introduction
==============

The MZX compression algorithm is a Lempel-Ziv (LZ77) class compression algorithm. It is used in consumer titles in Playstation 2-era and early PSP lifetime, in containers carrying .mrg, .mzp and .mzx extensions.

The .mzx container is the most straightforward one and it will be described in this document.


 I) .mzx structure (header: 0x8 bytes)
=======================================
	4 bytes - 'MZX0' signature (0x4D 0x5A 0x58 0x30)
	4 bytes - uncompressed length
	{ MZX datablock }

The output of the decompressed datablock may need to be truncated, so that its length matches the value found in the .mzx header.


 II) MZX datablock
===================

This LZ-class algorithm uses a combination of RLE codes, a 128-byte ring buffer and a sliding window technique.

Each command byte is composed of
- The 2 Least Significant Bits (4 possible values) indicate the command type: RLE (0), backreference (1), ring buffer (2), literal (3)
- The 6 remaining bits are used to compute LENGTH
An extra input byte is read, for backreference, to compute POSITION (distance)


```
counter = 4096
while remaining_output {
    LENGTH, COMMAND = read_byte
    test(COMMAND):
    case RLE: write2(B1B2, 1+LENGTH)
    case BACKREF: POSITION = (1+read_byte) * 2; write2(current-POSITION, 1+LENGTH); set B1 B2
    case RINGBUF: write2(ringbuf[2*LENGTH]ringbuf[2*LENGTH+1], 1); set B1 B2
    case LITERAL: 1+LENGTH times { read_byte, read_byte; set ringbuf[ofs:ofs+1]; set B1 B2; write2(B1B2, 1) }

    decrement counter if RINGBUF otherwise counter = counter - (1+LENGTH)
    when counter <=0 reset counter,B1,B2
}

```