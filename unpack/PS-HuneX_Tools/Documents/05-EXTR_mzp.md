.mzp Extraction
==============================================

 Used Tools
------------
- `_extract_mzp.py` [in-house dev / Python 3]

 About
-----------

allpac.mrg, extracted using `hedutil`, contains *.MZP files, which are compressed files with an 'mrgd00' sig.

You can see more information about .MZP in `specs` folder.

 Command
-----------

    python _extract_mzp.py input.mzp

 Source(s)
-----------
* .\*.MZP ('mrgd00') [from .mrg]

 Product(s)
-----------
* items in current folder (``*.png ``)

 Expected Output
-----------
	H:\155\image\allpac-unpacked>python _extract_mzp.py -h
	usage: _extract_mzp.py [-h] [-b] [-n] input.mzp

	positional arguments:
	  input.mzp     input .mzp file

	optional arguments:
	  -h, --help    show this help message and exit
	  -b, --bin     just extract MZP to bin and not output PNGs
	  -n, --notmzx  do not extract MZX that extracted from MZP

	H:\155\image\allpac-unpacked>python _extract_mzp.py @VOICE_01(OBJECT).MZP
	header: mrgd00
	found 36 entries