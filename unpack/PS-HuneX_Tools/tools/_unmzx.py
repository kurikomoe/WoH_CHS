#!/usr/bin/env python

import os
import shutil
from sys import stderr, argv
from struct import unpack
from pathlib import Path
from mzx.decomp_mzx0 import mzx0_decompress

if __name__ == '__main__':
    folder_path = Path(argv[1] if len(argv) > 1 else '.')
    for file_path in folder_path.glob('**/*.[Mm][Zz][Xx]'):
        out_path = file_path.with_suffix('.ini')
        with file_path.open('rb') as data:
            offset = 7 if(data.read(2) == b'LV') else 0
            data.seek(offset)
            sig, size = unpack('<LL', data.read(0x8))
                        # def mzx0_decompress(f, inlen, exlen, xorff=False) -> [str, BytesIO]:
            status, dec_buf = mzx0_decompress(data, os.path.getsize(file_path) - 8 - offset, size, True)
            if status != "OK":
                print("[{0}] {1}".format(file_path, status), file=stderr)
            with out_path.open('wb') as dbg:
                shutil.copyfileobj(dec_buf, dbg)
