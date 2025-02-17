from typing import *
import json
import logging
import argparse
from coloredlogs import install
from pprint import pprint
from collections import OrderedDict

install(level=logging.DEBUG)
logger = logging.getLogger(__name__)


# build font mapping from orig_ccit.json and woh_chs_fixed.txt
parser = argparse.ArgumentParser()

parser.add_argument("--orig_ccit", default="orig_ccit.json")
parser.add_argument("--text", default="woh_chs_fixed.txt")
parser.add_argument("--text_remapped", default="font_rebuild/remapped.txt")


args = parser.parse_args()

logger.info(args)

with open(args.orig_ccit, 'r') as f:
    orig_ccit: Dict = json.load(f)

with open(args.text, 'r') as f:
    text = f.read()
    text = text.replace("\n", "")
    text = text.replace("\r", "")

# 澄空文本
ck_charset = set(text)
ck_charset.add('\u0020')
ck_charset.add('\u00a0')
ck_charset_set = ck_charset
ck_charset = sorted(ck_charset)
print(len(ck_charset))

orig_charset = set()

# keep italic not change, hopefully there is not italic words
orig_ccit.pop("italic.ccit.bin")
orig_ccit.pop("H.ccit.bin")

# Quick refer
orig_char_acc = OrderedDict()
for file, chars in orig_ccit.items():
    for ch in chars:
        orig_charset.add(ch)
        tmp = orig_char_acc.get(ch, [])
        tmp.append(file)
        orig_char_acc[ch] = tmp

orig_char_acc_tosort = list(orig_char_acc.items())
orig_char_acc_tosort = sorted(orig_char_acc_tosort, key=lambda x: (len(x[1]), x[0]), reverse=False)

orig_char_acc = OrderedDict(orig_char_acc_tosort)
# pprint(orig_char_acc)

diff = ck_charset_set.difference(orig_charset)
same = ck_charset_set.intersection(orig_charset)
ck_charset = list(diff)

print(len(ck_charset))
print(len(orig_char_acc))

idx = 0
idy = 0
mapping_chengkong_2_orig = {}
mapping_orig_2_chengkong = {}
# for ch_orig_item, ch_chengkong in zip(orig_char_acc.items(), ck_charset):
BANNED_CHARS = [
    "旋转的七天·结束游戏返回标题界面设置环境正在存档是否存档是否覆盖正在读档是否读档No.无数据确定要结束游戏吗？",
    ".?<|>@^",
    "设置声音语音文本操作说明",
    "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ",
] + ['\u0020', '\u00a0', '\u2002', '\u2004', '\u2005', '\u2007', '\u2008', '\u202f']
# Fix \u0020 problem

BANNED_CHARS = "".join(BANNED_CHARS) + "".join(same)
# BANNED_CHARS = "确定要结束游戏吗？,.?<|>@^0123456789"
orig_char_acc_items = list(orig_char_acc.items())

for ch in BANNED_CHARS:
    if ch in ck_charset:
        ck_charset.remove(ch)

while idy < len(ck_charset):
    print(f"orig {idx} / {len(orig_char_acc_items)}")
    print(f"ck   {idy} / {len(ck_charset)}")
    ch_orig_item = orig_char_acc_items[idx]
    ch_orig = ch_orig_item[0]
    ch_orig_files = ch_orig_item[1]

    if ch_orig in BANNED_CHARS:
        idx += 1
        continue

    ch_chengkong = ck_charset[idy]

    mapping_chengkong_2_orig[ch_chengkong] = {
        "ch_orig": ch_orig,
        "files": ch_orig_files,
    }
    mapping_orig_2_chengkong[ch_orig] = {
        "ch_chengkong": ch_chengkong,
    }

    idx += 1
    idy += 1

    assert idy < len(orig_char_acc_items)

# pprint(mapping_chengkong_2_orig)
# pprint(mapping_orig_2_chengkong)

json.dump(mapping_orig_2_chengkong, open("mapping_orig_2_chengkong.json", "w"), ensure_ascii=False, indent=2)
json.dump(mapping_chengkong_2_orig, open("mapping_chengkong_2_orig.json", "w"), ensure_ascii=False, indent=2)

with open(args.text, 'r') as f:
    text = f.read()

remapped_text = ""
for ch in text:
    if ch in ['\u0020', '\u00a0', '\u2002', '\u2004', '\u2005', '\u2007', '\u2008', '\u202f']:
        ch = '\u00a0'
    if ch in mapping_chengkong_2_orig:
        ch = mapping_chengkong_2_orig[ch]["ch_orig"]
    remapped_text += ch

with open(args.text_remapped, 'w') as f:
    f.write(remapped_text)

# json.dump(mapping_chengkong_2_orig, open("font_rebuild/mapping.json", "w"), ensure_ascii=False, indent=2)