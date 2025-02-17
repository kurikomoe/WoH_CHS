 Script Reinsertion into allpac.hed/.nam/.mrg
==============================================

 Used Tools
------------
- `hedutil.py` [in-house dev / Python 3]

 About
-----------

After extracting from allpac.mrg/nam/hed at the very beginning, a filelist destination was specified.

We will modify this filelist to point to the new path (`hedutil`'s "replace" action verb)


#### Working Directory ####

Create a folder named `".\3.reinsert_hed"` and copy into it:

- allpac.* (including filelist)
- allpac-unpacked
- `".\2.build_mzx\40buildedscript"` (from a previous step)
- hedutil.py


- - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	
 Command(s)
-----------
	python hedutil.py replace --filelist allpac.list --source 40buildedscript allpac.hed
	-or-
	
	python hedutil.py replace --filelist allpac.list --source 40buildedscript\CO0101.MZX allpac.hed

 Source(s)
-----------
1. `.\3.reinsert_hed\allpac.list`
2. `.\3.reinsert_hed\allpac.hed`/nam/mrg
3. `.\3.reinsert_hed\40buildedscript\*.MZX`

 Product(s)
-----------

1. allpac.list
2. allpac.hed
2. allpac.mrg


 Expected Output
-----------

	C:\work\_TL_\psp_aya\3.reinsert_hed>python hedutil.py replace -h
	usage: hedutil.py replace [-h] -f FILELIST -s sourcepath [-i INDEX | -n NAME]
	                          existing.hed
	
	positional arguments:
	  existing.hed          Subject .hed file, modified in-place. Same basename is
	                        used for .nam/.mrg
	
	optional arguments:
	  -h, --help            show this help message and exit
	  -f FILELIST, --filelist FILELIST
	                        Subject filelist path, modified in-place [REQUIRED]
	  -s sourcepath, --source-file sourcepath
	                        File path to inserted file [REQUIRED]
	  -i INDEX, --index INDEX
	                        Refer to replaced entry by index
	  -n NAME, --name NAME  Refer to replaced entry by name
	
	C:\work\_TL_\psp_aya\3.reinsert_hed>python _hedutil.py replace --filelist allpac.list --source 40buildedscript allpac.hed
	Loaded Filelist: allpac.hed >> allpac-unpacked
	Replacing: idx=40 40buildedscript\CO0101.MZX - orgOfs-Sz:0C76B000-40104b
	Replacing: idx=41 40buildedscript\CO0102.MZX - orgOfs-Sz:00398000-1063b
	(...)
	Success = 193
	Failed = 0
	Filelist: allpac.list
	
	C:\work\_TL_\psp_aya\3.reinsert_hed>

