import os

#  for file in ["chs", "cht", "en", "jp"]:
#      with open(f"dump_{file}.bin", "r", encoding="UTF8") as f:
#          data = f.read()
#      with open(f"woh_{file}.txt", "w", encoding="UTF8") as f:
#          f.write(data)

with open(f"woh_chs_fixed.bin", "r", encoding="UTF8") as f:
    data = f.read()
with open(f"woh_chs_fixed.txt", "w", encoding="UTF8") as f:
    f.write(data)
