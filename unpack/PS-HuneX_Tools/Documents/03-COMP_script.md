 Script Localization - Modding
===============================

 Used Tools
------------
- `mzx/comp_mzx0.py` [in-house dev / Python 3]
- `make_mzx.py` [in-house dev / Python 3]
- Sakuraume's Text Editor -- modded

#### Working Directory ####

To get started, create a copy of folder `.\1.prep_mzx_to_tpl\20decodedscript` and name it `".\2.build_mzx\30insertedscript"`.


## About ##

Files where designed to work with a modified copy of [Sakuraume's Text Editor](http://vn.i-forge.net/tools/#text+files+editor).

Encoding is UTF-8 and translated text using characters outside ASCII is untested and not guaranteed to work.


## Guidelines for modifications:

- Extraction has generated `*.tpl.txt` files containing game script, including localizable text.  

- I recommend leaving names alone when starting localization, those can be substituted at later stages.

- Limitations:  
  (to be defined)

_Sample translated line:_

	`<0040>_ZM00118(　飯塚薫はため息をついた。_r)=_ZM00118(　A sigh escaped from Iizuka Kaoru's lips._r)`


- - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	
 Command
-----------
	python make_mzx.py
-or-

	python make_mzx.py 30insertedscript\CO0101.tpl.txt

 Source(s)
-----------
1. .\30insertedscript\~scriptname~.tpl.txt

 Product(s)
-----------

* Folder:`35precompscript`
* .\35precompscript\~scriptname~.pre-comp.sjs  => raw buffer before compression. CP932 encoding

* Folder:`40buildedscript`
* .\40buildedscript\~scriptname~.MZX  => pseudo-compressed script

All MZX should be repacked into allpac.mrg/nam/hed by modifying the filelist using `hedutil`.


 Expected Output
-----------

	C:\work\_TL_\psp_aya\2.build_mzx>python make_mzx.py 30insertedscript
	* CO0101.tpl.txt => CO0101.MZX: 21260b 21435b [PASSED]
	(...omitted...)
	Passed = 193
	Failed = 0
	C:\work\_TL_\psp_aya\2.build_mzx>python make_mzx.py 30insertedscript\CO0204A.tpl.txt
	* CO0204A.tpl.txt => CO0204A.MZX: 6014b 6069b [PASSED]
	Passed = 1
	Failed = 0
	C:\work\_TL_\psp_aya\2.build_mzx>
	

