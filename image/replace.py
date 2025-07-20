import os
import sys
from concurrent.futures import ThreadPoolExecutor
from mzp import MzpImage

mzp_image1 = MzpImage("C:/Users/user/Desktop/WITCH ON THE HOLY NIGHT/data02000/0416_img0416.mzp")
mzp_image1.img_read("C:/Users/user/Desktop/copy/0416_img0416.png", compression_level = 2)
mzp_image1.mzp_write("C:/Users/user/Desktop/copy/0416_img0416.mzp")


mzp_image2 = MzpImage("C:/Users/user/Desktop/WITCH ON THE HOLY NIGHT/data02000/0417_img0417.mzp")
mzp_image2.img_read("C:/Users/user/Desktop/copy/0417_img0417.png", compression_level = 2)
mzp_image2.mzp_write("C:/Users/user/Desktop/copy/0417_img0417.mzp")

mzp_image3 = MzpImage("C:/Users/user/Desktop/WITCH ON THE HOLY NIGHT/data02000/0420_img0420.mzp")
mzp_image3.img_read("C:/Users/user/Desktop/copy/0420_img0420.png", compression_level = 2)
mzp_image3.mzp_write("C:/Users/user/Desktop/copy/0420_img0420.mzp")

mzp_image4 = MzpImage("C:/Users/user/Desktop/WITCH ON THE HOLY NIGHT/data02000/0421_img0421.mzp")
mzp_image4.img_read("C:/Users/user/Desktop/copy/0421_img0421.png", compression_level = 2)
mzp_image4.mzp_write("C:/Users/user/Desktop/copy/0421_img0421.mzp")