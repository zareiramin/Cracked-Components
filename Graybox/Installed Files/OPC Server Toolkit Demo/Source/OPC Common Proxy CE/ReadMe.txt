========================================================================
         Creating the Windows CE Proxy Stub DLL for OPC Common
========================================================================

1.	Create a new Visual C++ project for Smart Device. Select the
	'Win32 Smart Device Project' template.

2.	Name the DLL project 'opccomn_ps'

3.	On the 'Platforms' page select platform SDKs for which you are going
	to build the proxy/stub DLL. Note, that SDKs must include DCOM.

4.	On the 'Application Settings' page select DLL for 'Application type'
	and check 'Empty project' in the Additional settings.
	Now click 'Finish'.

5.	Add the following files to the proxy/stub DLL project.
	·	dlldata.c
	·	opccomn.h
	·	opccomn_i.c
	·	opccomn_p.c
	·	opccomn_ps.def

6.	Open the project settings dialog for the DLL project
	and select the 'Configuration Properties -> C/C++ -> Preprocessor' page.
	Add WIN32 and REGISTER_PROXY_DLL to the 'Preprocessor Definitions' edit box.
	
7.	Now go to the 'Configuration Properties -> Linker -> Input' page.
	Add the following LIB files to the 'Additional Dependancies' edit box.
	·	rpcrt4.lib
	·	uuid.lib
	·	oleaut32.lib
	·	corelibc.lib
	·	coredll.lib

8.	On the same page enter 'opccomn_ps.def' to the 'Module Definition File'
	edit box.

9.	Now you can build the project.
