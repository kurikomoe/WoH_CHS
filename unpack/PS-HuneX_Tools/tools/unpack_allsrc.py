#!/usr/bin/env python
#
# MRG Extractor
# comes with ABSOLUTELY NO WARRANTY.
#
# Copyright (C) 2016, 2019 Hintay <hintay@me.com>
#
# Portions Copyright (C) 2016 Quibi
#
# MRG files extraction utility
# For more information, see Specifications/mzp_format.md

import argparse
import sys
import struct
from pathlib import Path

INPUT_FILE_NAME = Path('allscr.mrg')
MODE = 'fate'


def parse_args():
    if len(sys.argv) > 1:
        args = Path(sys.argv[1])
        if args.is_dir():
            return args
        elif args.is_file():
            return args.parent
    else:
        return Path('.')


class ArchiveEntry:
    def __init__(self, sector_offset, offset, sector_size_upper_boundary, size, number_of_entries):
        self.sector_offset = sector_offset
        self.offset = offset
        self.sector_size_upper_boundary = sector_size_upper_boundary
        self.size = size
        self.real_size = (sector_size_upper_boundary - 1) // 0x20 * 0x10000 + size
        data_start_offset = 6 + 2 + number_of_entries * 8
        self.real_offset = data_start_offset + self.sector_offset * 0x800 + self.offset


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawTextHelpFormatter)
    parser.add_argument('input', metavar='input_files', help='Input file or folder.', nargs='?')
    args = parser.parse_args()

    if args.input:
        input_path = Path(args.input)
        if input_path.is_file():
            input_path = input_path.parent
    else:
        input_path = Path('.')

    file_path = input_path.joinpath(INPUT_FILE_NAME)
    if file_path.is_file():
        input_file = open(file_path, 'rb')
    else:
        print("allscr.mrg not found. Please pass the path to the folder it is located in.")
        sys.exit(1)

    header = input_file.read(6)
    print('header: {0}'.format(header.decode('ASCII')))

    number_of_entries, = struct.unpack('<H', input_file.read(2))

    print('found {0} entries'.format(number_of_entries))
    entries_descriptors = []
    for i in range(number_of_entries):
        sector_offset, offset, sector_size_upper_boundary, size = struct.unpack('<HHHH', input_file.read(8))
        entries_descriptors.append(
            ArchiveEntry(sector_offset=sector_offset, offset=offset, sector_size_upper_boundary=sector_size_upper_boundary,
                        size=size, number_of_entries=number_of_entries))

    file_names = ['allscr.nam', 'unknownX.mrg', 'unknownX2.mrg']
    for i in range(number_of_entries):
        file_name = ''
        if i * 32 < entries_descriptors[0].real_size:
            # Fix code for RN
            if MODE == 'fate':
                if 101 <= i <= 202:
                    file_name = 'セイバールート十'
                elif i == 240:
                    file_name = 'ラストエピソ'
                elif 338 <= i <= 483:
                    file_name = '桜ルート十'
                elif 604 <= i <= 705:
                    file_name = '凛ルート十'

            file_name_bytes, = struct.unpack('<32s', input_file.read(32))
            file_name_bytes = file_name_bytes.replace(b'\x01', b'')
            file_name = file_name + file_name_bytes[0:file_name_bytes.index(b'\x00')].decode('932', 'ignore')
        if not file_name:
            file_name = 'unknown' + str(i)
        file_names.append(file_name + '.mzx')

    output_dir = input_path.joinpath(INPUT_FILE_NAME.stem + '-unpacked')
    if not output_dir.is_dir():
        output_dir.mkdir()

    for index, entry in enumerate(entries_descriptors):
        input_file.seek(entry.real_offset)
        data = input_file.read(entry.real_size)
        output_file_name = output_dir.joinpath(file_names[index])
        print(output_file_name.name, file=sys.stderr)
        output_file = open(output_file_name, 'wb')
        output_file.write(data)
        output_file.close()

    input_file.close()
