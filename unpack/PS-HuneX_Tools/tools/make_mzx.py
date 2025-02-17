#!/usr/bin/env python

"""make_mzx version 1.0, Copyright (C) 2014 Nanashi3.

make_mzx comes with ABSOLUTELY NO WARRANTY.

Usage:
make_mzx.py 30insertedscript
"""

import os, errno, io
import sys
from sys import stderr
from struct import unpack, pack
import glob
import re
from mzx.comp_mzx0 import mzx0_compress


class CustomException(Exception):
    pass


def makedir(dirname):
    try:
        os.makedirs(dirname)
    except OSError as exc:  # Python >2.5
        if exc.errno == errno.EEXIST and os.path.isdir(dirname):
            pass
        else:
            raise


def process_directory(sourcedirpath, args, mask='*.txt'):
    successful = failed = 0
    for filepath in glob.iglob(os.path.join(sourcedirpath, mask)):
        s = process_path(filepath, args)
        if s != "OK":
            failed += 1
        else:
            successful += 1
    return [successful, failed]


def process_path(sourcepath, args):
    infile = None
    outpath = None
    try:
        basename_src = os.path.basename(sourcepath)

        # using split() b/c source basename generally has multiple dots
        basename_inter = basename_src.split('.', 2)[0] + '.pre-comp.sjs'
        basename_mzx = basename_src.split('.', 2)[0] + '.MZX'  # using split() b/c it generally has multiple dots
        print("* {0} => {1}: ".format(basename_src, basename_mzx), end="")
        if str.upper(os.path.splitext(basename_src)[1]) == '.MZX':
            raise CustomException("'{0}' is already a .MZX file".format(basename_src))
        outpath = os.path.join(args.outputdir, basename_mzx)

        infile = open(sourcepath, 'rt', encoding="utf-8-sig")
        processed_lines = []

        # now, revert operation from prep_tpl
        lnum = 1
        fulwidcomma = chr(0xff0c)
        for l in infile.readlines():
            # processed_lines.append(l.rstrip('\r\n'))
            l = l.rstrip('\r\n')
            m = re.search(r'^<[0-9]+>(.+)', l)
            if m is not None:
                # unescape contents between bracket, avoid problematic characters
                parts = m.group(1).split('=', 2)
                line = parts[0] if len(parts) < 2 else parts[1]
                expr = re.search(r'^([^(]+)\((.+)\)', line)
                if expr is not None:
                    befo = expr.group(1) + "("
                    subj = expr.group(2)
                    afte = ")"
                else:
                    befo = afte = ""
                    subj = line
                line = befo + subj.replace(", ", fulwidcomma).replace(",", fulwidcomma).replace(";/", ",").replace("(",
                                                                                                                   chr(
                                                                                                                       0xff08)).replace(
                    ")", chr(0xff09)).replace("_n", "@n").replace("_r", "^") + afte
                # TODO: process actor name translation here
                processed_lines.append(line)
            elif len(l) > 1 and l[0] == '!':
                processed_lines.append(l[1:])
            else:
                if len(l) > 0:
                    m = re.search(r'^~(.*)~$', l)
                    if m is None:
                        print("WRN: \"{0}\" line {1} - {2}".format(sourcepath, lnum,
                                                                   "text should be enclosed in ~~ " + l), file=stderr)
                    else:
                        processed_lines.append(m.group(1))
            lnum += 1

        resultbytes = ';'.join(processed_lines).encode('CP932')
        inlen = len(resultbytes)
        with open(os.path.join(args.tempdir, basename_inter), 'wb') as interfile:
            interfile.write(resultbytes)

        outdata = mzx0_compress(io.BytesIO(resultbytes), inlen, xorff=True)
        outlen = len(outdata)
        with open(outpath, 'wb') as outfile:
            outfile.write(outdata)

        print("{0}b {1}b [PASSED]".format(inlen, outlen))
        return "OK"
    except CustomException as exc:
        print("[FAILED]")
        nfailed += 1
        print("ERR: [{1}] failed to process \"{0}\" - {2}".format(sourcepath, type(exc).__name__, str(exc)),
              file=stderr)
        if outpath is not None and os.path.isfile(outpath):
            try:
                os.remove(outpath)
            except Exception:
                pass  # swallow
        return "ERR"
    finally:
        if infile is not None:
            infile.close()


############
# __main__ #
############

if __name__ == '__main__':

    import argparse
    from sys import stderr

    parser = argparse.ArgumentParser(description='Compress one or several files as MZX')
    parser.add_argument('inputs', metavar='input_file', nargs='+', help='Input file(s)')
    parser.add_argument('-o', '--output-dir',
                        default=None, dest='outputdir',
                        help='Output directory (default: 40buildedscript)')
    parser.add_argument('-t', '--temp-dir',
                        default=None, dest='tempdir',
                        help='Temporary directory (default: 35precompscript)')
    args = parser.parse_args()
    if args.outputdir is None:
        args.outputdir = "40buildedscript"
    if args.tempdir is None:
        args.tempdir = "35precompscript"

    dir = ""
    try:
        for dir in [args.outputdir, args.tempdir]:
            makedir(dir)
    except Exception as exc:
        print("ERR: [{1}] failed to create specified output directory \"{0}\" - {2}".format(dir, type(exc).__name__,
                                                                                            str(exc)), file=stderr)
        sys.exit(1)

    npassed = nfailed = 0

    for inpath in args.inputs:
        if os.path.isdir(inpath):
            successful, failed = process_directory(inpath, args)
        else:
            s = process_path(inpath, args)
            if s != "OK":
                successful = 0
                failed = 1
            else:
                successful = 1
                failed = 0

        npassed += successful
        nfailed += failed

    print("Passed = {0}\nFailed = {1}".format(npassed, nfailed))
