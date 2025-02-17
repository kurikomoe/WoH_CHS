import re
import struct
import math
import os
import json
import itertools
import more_itertools
import argparse
import glob
from threading import Thread
from multiprocessing import Pool

parser = argparse.ArgumentParser()

# parser.add_argument("--font_regular", help="regular font", default="FZFWZhuZiMinchoR.TTF")
# parser.add_argument("--font_regular", help="regular font", default="SourceHanSerifSC-Regular.otf")
parser.add_argument("--font_regular", help="regular font", default="SourceHanSerifSC-Bold.otf")
# parser.add_argument("--font_bold", help="bold font", default="SourceHanSerifSC-Bold.otf")

parser.add_argument("--font_png_folder", help="used to get hash of each raw image", default="font_pngs")
parser.add_argument("--output", help="output folder to store new font.bin", default="font_rebuild")

args = parser.parse_args()


# load paletteblob.bin
normal_pat = re.compile(r"FONT_zc_(\d+).mzp.+")
h_pat = re.compile(r"FONT_zc_H(\d+).mzp")
italic_pat = re.compile(r"FONT_zc_italic_(\d+).mzp")
font3_pat = re.compile(r"FONT_zc_font3_(\d+).mzp")

paletteblob = [[] for i in range(255)]
for file in glob.glob(args.font_png_folder + "/*.paletteblob.bin"):
    print(f"loading paletteblob.bin: {file}")
    with open(file, 'rb') as f:
        data = f.read()

    if (matches := h_pat.findall(file)):
        idx = int(matches[0]) + 1
    elif (matches := normal_pat.findall(file)):
        idx = int(matches[0]) + 3
    elif (matches := italic_pat.findall(file)):
        idx = int(matches[0]) + 38
    elif (matches := font3_pat.findall(file)):
        idx = int(matches[0]) + 41
    else:
        print(f"Invalid filename: {file}")
        exit(-1)

    assert len(data) % 4 == 0
    i = 0
    while i < len(data):
        b = data[i]
        g = data[i+1]
        r = data[i+2]
        a = data[i+3]

        paletteblob[idx].append((r, g, b, a))
        i += 4

'''
字符映射表        字库集合    数量
20000.ccit  zc  H          2   0,1
20001.ccit  zc  Normal     35  2-36
20100.ccit  zc  Italic     3   37,38,39
20200.ccit  zc  Font3      13  40-52
'''

order = [
    "H.ccit.bin",
    "normal.ccit.bin",
    "italic.ccit.bin",
    "font3.ccit.bin",
]

with open("mapping_orig_2_chengkong.json", "r") as f:
    mapping_orig_2_chengkong = json.load(f)

with open("mapping_chengkong_2_orig.json", "r") as f:
    mapping_chengkong_2_orig = json.load(f)

with open("orig_ccit.json", 'r') as f:
    orig_ccit = json.load(f)

orig_ccit.pop("italic.ccit.bin")
orig_ccit.pop("H.ccit.bin")

# 14 * 7 = 98

orig_chunks = {}

for file, chars in orig_ccit.items():
    orig_chunks[file] = list(more_itertools.batched(chars, 14*7))

# do the fxxking drawing
from PIL import Image, ImageFont, ImageDraw

font_regular = ImageFont.truetype(args.font_regular, size=60)
# font_bold = ImageFont.truetype(args.font_bold, size=64)

M = 1024/14

def draw_char(draw, font, pos, text, outline=False):
    # if text in ['\u0020', '\u00a0', '\u2002', '\u2004', '\u2005', '\u2007', '\u2008', '\u202f']:
    #     print("Found")
    #     text = '\u00a0'
    #     # text = '　'

    x, y = pos
    # print(f"Drawing @ ({x}, {y})")
    _, _, w, h = draw.textbbox((0, 0), text, font=font)
    dy = -16
    # dy = 5

    if text in r"0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()_+=-[]\;',./<>?:\"{}|":
        font = ImageFont.truetype(args.font_regular, size=60*0.8)

    if text in "，":
        dy -= 5

    if text in "'\"0123456789":
        dy += 5


    # if text in "gjpqy":
    #     # dy = 0
    #     dy = -20

    if not outline:
        draw.text((x * M + (M - w)//2, y * M - (M-h)//2 + dy), text, font=font, align="left", fill=(255, 255, 255, 255))
    else:
        draw.text((x * M + (M - w)//2, y * M - (M-h)//2 + dy), text, font=font, align="left", fill=(255, 255, 255, 255),
                stroke_width=3, stroke_fill=(255, 255, 255, 255))
    x += 1
    if x == 14:
        x = 0
        y += 1
    return (x, y)


def find_best_matching(paletteblob, pixel):
    mx =  1145141919819
    ret = None
    for idx, (r, g, b, a) in enumerate(paletteblob):
        (rr, gg, bb, aa) = pixel
        cur = (r - rr)**2 + (g - gg)**2 + (b-bb)**2 + (a-aa)**2
        if cur < mx:
            mx = cur
            ret = idx
    return ret


def dump_rgba8_binary(img: Image, index, filename):
    data = img.getdata()
    output = []
    mapping = {}
    for pixel in data:
        if pixel == (0, 0, 0, 0):
            output.append(0xCC)
        elif pixel == (255, 255, 255, 255):
            output.append(0xFF)
        else:
            if pixel not in mapping:
                idx = find_best_matching(paletteblob[index], pixel)
                mapping[pixel] = idx
            else:
                idx = mapping[pixel]
            output.append(idx)

    with open(filename, "wb") as f:
        f.write(bytes(output))

    return

suffix = 1  # 0 is special for FONT_GAIJI_00.mzp

def worker(chars_chunk, suffix):
    img = Image.new(mode="RGBA", size=(1024, 1024), color = (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)

    cur_pos = (0, 0)
    output_file = os.path.join(args.output, f"font_replace-{suffix}.png")
    # first draw normal text
    for orig_ch in chars_chunk:
        if orig_ch not in mapping_orig_2_chengkong:
            chengkong_ch = orig_ch
        else:
            chengkong_ch = mapping_orig_2_chengkong[orig_ch]['ch_chengkong']

        if "M" == chengkong_ch:
            print("error:", suffix)

        cur_pos = draw_char(draw, font_regular, cur_pos, chengkong_ch)

    # print(chars_chunk)
    # print(list(map(lambda x: mapping_orig_2_chengkong[x]['ch_chengkong'], chars_chunk)))
    # input()

    # Force half page
    cur_pos = (0, 7)

    # second draw bold text
    for orig_ch in chars_chunk:
        if orig_ch not in mapping_orig_2_chengkong:
            chengkong_ch = orig_ch
        else:
            chengkong_ch = mapping_orig_2_chengkong[orig_ch]['ch_chengkong']
        cur_pos = draw_char(draw, font_regular, cur_pos, chengkong_ch, outline=True)

    # img.show()
    # exit(-1)
    img.save(output_file)
    output_file = os.path.join(args.output, f"font_replace-{suffix}.bin")
    dump_rgba8_binary(img, suffix, output_file)
    return


with Pool(24) as pool:
    for file in order:
        if file == "H.ccit.bin":
            print(f"Ignore {file} and suffix: {suffix} += 2")
            suffix += 2
            continue
        if file == "italic.ccit.bin":
            print(f"Ignore {file} and suffix: {suffix} += 3")
            suffix += 3
            continue
        print(f"Processing {suffix-1} @{file}")

        for chars_chunk in orig_chunks[file]:
            pool.apply_async(worker, args=(chars_chunk, suffix, ))
            suffix += 1

    pool.close()
    pool.join()
