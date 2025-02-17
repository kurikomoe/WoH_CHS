import os
import json
import logging
from coloredlogs import install
from pprint import pprint

install(level=logging.DEBUG)
logger = logging.getLogger(__name__)


with open("orig_ccit.json", "r") as f:
    data = json.load(f)

charset = set()

dup_cnt = 0
for file, chars in data.items():
    for ch in chars:
        if ch in charset:
            dup_cnt += 1
            logger.warning(f"found duplication: {ch}")
            for file in data.keys():
                if ch in data[file]:
                    logger.warning(f"\t in: {file}")

        charset.add(ch)

# pprint(charset)
# print(len(charset))
logger.info(f"duplications: {dup_cnt}")

# check only existed in italic
it_set = set()
for ch in data['italic.ccit.bin']:
    it_set.add(ch)

other_set = set()
for file, chars in data.items():
    if file == "italic.ccit.bin": continue
    for ch in chars:
        other_set.add(ch)

diff = it_set.difference(other_set)
pprint(diff)
