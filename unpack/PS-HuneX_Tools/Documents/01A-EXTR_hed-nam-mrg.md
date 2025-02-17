 allpac.hed/.nam/.mrg Extraction
=================================

 Used Tools
------------
- `hedutil.py` [in-house dev / Python 3]


 About
-----------

.hed/.nam/.mrg triples are commonly found at top-level of disc filesystem for PS2 titles, or in the USRDIR folder otherwise.

These containers typically contain the following file types:

- *.MRG, *.MZP ('mrgd00')
> generic container, group of pictures

- *.MZX ('MZX0')
> Compressed data stream.

- *.ahx, *.at3
> CRI Middleware MPEG-2 audio file or ATRAC3-in-RIFF  audio file

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

 Command
-----------
	python hedutil.py unpack --filelist allpac.list allpac.hed


 Source(s)
-----------
***When unpacking***:

1. allpac.hed
2. allpac.nam (optional)
3. allpac.mrg

***When repacking***:

1. allpac.list + files referenced inside


 Product(s)
-----------
***When unpacking***:

* items in subfolder (``allpac-unpacked``)
* Ordered file list (``allpac.list``)

***When repacking***:

* HED/NAM/MRG treble (``newpac.hed``, etc.) 



 Expected Output
-----------

	C:\work\_TL_\ayakashibito_py\lab\01A-EXTR-hed>python hedutil.py
	usage: hedutil.py [-h] {unpack,replace,repack} ...


	C:\work\_TL_\ayakashibito_py\lab\01A-EXTR-hed>python hedutil.py unpack -h
	usage: hedutil.py unpack [-h] [-f FILELIST] input.hed
	
	positional arguments:
	  input.hed             Input .hed file
	
	optional arguments:
	  -h, --help            show this help message and exit
	  -f FILELIST, --filelist FILELIST
	                        Output filelist path (default: none -- only unpack
	                        files)


	C:\work\_TL_\ayakashibito_py\lab\01A-EXTR-hed>python hedutil.py unpack --filelist allpac.list allpac.hed
	----------------------------------------------------------------------------------------
	| Archive count: 6184 entries
	----------------------------------------------------------------------------------------
	|- KCUR00.MZP - 344 b
	|- KCUR01.MZP - 344 b
	|- ACURSOR01.MZP - 1416 b	
	(...)
	========================================================================================
	Filelist: allpac.list
	Output Directory: allpac-unpacked


	