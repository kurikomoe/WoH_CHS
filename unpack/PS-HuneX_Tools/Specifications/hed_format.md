 The .hed/.nam/.mrg format
===========================
> Revision 1, maintainer: Waku_Waku

> Reverse-engineered by Waku_Waku

All fields are little-endian unless specified otherwise.


 Introduction
==============

This document refers to the top-level MRG/HED/NAM structure, an archive format mostly used in consumer titles in Playstation 2-era and early PSP lifetime.

They are commonly found at top-level of disc filesystem for PS2 titles, or in the USRDIR folder otherwise.

NOTE: encrypted allpac.cpk has not been reversed yet. I know chinese hackers managed to do it for Mashiro Iro Symphony PSP, though.


These containers typically contain the following file types:

- *.MRG, *.MZP ('mrgd00')
> generic container, group of pictures (described in ``mzp_format.md``)

- *.MZX ('MZX0')
> Compressed data stream (described in ``mzx_compression.md``)

- *.ahx, *.at3
> CRI Middleware MPEG-2 audio file or ATRAC3-in-RIFF  audio file (generally easily decodable to .wav for people who want to create voice patches or whatever)


 I) .hed structure 
===================

> For the general purpose 'allpac.hed':

    { n times (0x8 bytes): Generic Entry Descriptor }

> For the specific 'voice*.hed':

    { n times (0x4 bytes): Voice Entry Descriptor }


A series of sixteen 0xFF bytes marks EOF.



 I.1)  Generic Entry Descriptor (allpac)
-----------------------------------------

	2 bytes - offset, low Word
	2 bytes - offset, high Word
	2 bytes - size upper bound, sector count (0x800 bytes)
	2 bytes - size, low Word

Entry offset = 0x800 * (((.ofsHigh & 0xF000) << 4) | .ofsLow)

If .sizeLow is zero:
Entry size = 0x800 * .sizeSect

Otherwise:
Entry size = (0x800 * (.sizeSect - 1) & 0xFFFF0000) | .sizeLow


I.2)  Voice Entry Descriptor (voice*)
--------------------------------------

	2 bytes - offset, low Word
	2 bytes - offset and size, high Word

Entry offset = 0x800 * (((.ofsSzHigh & 0xF000) << 4) | .ofsLow)

Entry size = 0x800 * (.ofsSzHigh & 0x0FFF)


