#!/usr/bin/env python

"""prep_tpl version 1.0, Copyright (C) 2014 Nanashi3.
comes with ABSOLUTELY NO WARRANTY.
"""

import shutil
from pathlib import Path
from sys import stderr
from struct import unpack
import re
import argparse
from mzx.decomp_mzx0 import mzx0_decompress


raw_script_path = Path("10rawscript")
decoded_script_path = Path("20decodedscript")


def process_directory(source_dir_path: Path):
    successful = failed = 0
    for filepath in source_dir_path.glob('*.[Mm][Zz][Xx]'):
        if process_path(filepath) != "OK":
            failed += 1
        else:
            successful += 1
    return [successful, failed]


def process_path(source_path: Path):
    base_stem = source_path.stem
    out_txt = base_stem + '.ini'
    out_tpl = base_stem + '.tpl.txt'
    with source_path.open('rb') as data:
        sig, size = unpack('<LL', data.read(0x8))
        status, decbuf = mzx0_decompress(data, source_path.stat().st_size - 8, size, xorff=True)
        if status != "OK":
            print("[{0}] {1}".format(status, source_path), file=stderr)
        with raw_script_path.joinpath(out_txt).open('wb') as dbg:
            shutil.copyfileobj(decbuf, dbg)

        outcoll = []
        decbuf.seek(0)
        for index, instr in enumerate(decbuf.read().split(b';')):
            instrtext = instr.decode('cp932', 'surrogateescape')
            if re.search(r'_LVSV|_STTI|_MSAD|_ZM|SEL[R]', instrtext) is not None:
                outcoll.append(
                    "<{0:04d}>".format(index) + instrtext.replace("^", "_r")
                    .replace("@n", "_n").replace(",", ";/"))  # replace order significant
            elif len(re.sub('[ -~]', '', instrtext)) > 0:
                outcoll.append(u"!" + instrtext)  # flag missing matches containing non-ASCII characters
            else:
                outcoll.append(u"~" + instrtext + u"~")  # non-localizable

        if outcoll:
            with decoded_script_path.joinpath(out_tpl).open('wt', encoding="cp932", errors='surrogateescape') as outfile:
                outfile.write('\n'.join(outcoll))
    return status


"""
debugging:
    try:
        suite
    except Exception as exc:
        print("ERR: [{0}] - {1}".format(type(exc).__name__, str(exc)), file=stderr)
        sys.exit(1)
"""

############
# __main__ #
############

if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawTextHelpFormatter)
    parser.add_argument('input', metavar='input_files', help='Input file or folder.')
    args = parser.parse_args()

    raw_script_path.mkdir(exist_ok=True, parents=True)
    decoded_script_path.mkdir(exist_ok=True, parents=True)

    input_path = Path(args.input)
    successful = failed = 0
    if input_path.is_dir():
        successful, failed = process_directory(input_path)
    elif str.lower(input_path.suffix) == '.mzx':
        if process_path(input_path) != "OK":
            failed = 1
        else:
            successful = 1
    print("{0} scripts, {1} SUCCESS, {2} FAILURE".format(successful + failed, successful, failed), file=stderr)
