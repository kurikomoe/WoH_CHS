# SPDX-License-Identifier: MIT

import io
import sys
import struct
import os

def main():
    out_dir = sys.argv[1] + "__"
    if len(sys.argv) > 2:
        out_dir = sys.argv[2]
    try:
        os.makedirs(name=out_dir)
    except FileExistsError as e:
        pass
    with open(sys.argv[1], "rb") as f:
        if f.read(12) != b"HUNEXGGEFA10":
            raise Exception("Invalid input file")
        archive_file_entries = {}
        total_file_entries, = struct.unpack("<I", f.read(4))
        for i in range(total_file_entries):
            file_entry_name, file_entry_offset, file_entry_size = struct.unpack("<96sII24x", f.read(96+4+4+24))
            archive_file_entries[file_entry_name.rstrip(b"\x00")] = [file_entry_offset, file_entry_size]
        data_start = f.tell()
        for file_entry_name in sorted(archive_file_entries.keys()):
            file_entry = archive_file_entries[file_entry_name]
            f.seek(data_start + file_entry[0])
            output_data = f.read(file_entry[1])
            with open(out_dir + "/" + file_entry_name.decode("ASCII"), "wb") as wf:
                wf.write(output_data)

if __name__ == "__main__":
    main()