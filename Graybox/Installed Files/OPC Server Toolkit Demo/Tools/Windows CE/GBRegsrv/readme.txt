DESCRIPTION
----------
gbregsrv.exe is a command line tool used to register COM servers located in a DLL-module.
This tool is similar to Microsoft regsvr32 but it works on Windows CE.
This tool loads the specified DLL-module and calls DllRegisterServer or DllUnregisterServer
export function. You may run this tool from the command line or from within your application
in silent mode. If tool succeeds to register/unregister COM-servers than it returns zero.


SUPPORTED OPERATING SYSTEMS
---------------------------
Windows CE 1.0 and later


SUPPORTED PLATFORMS
-------------------
ARMV4
ARMV4I
MIPSII
MIPSII_FP
MIPSIV
MIPSIV_FP
SH3
SH4
x86
Connectix Windows CE Device Emulator x86


USAGE
-----
Command line syntax:
gbregsrv.exe DLLNAME [-u] [-s]
DLLNAME - name of the DLL with or without path to it
-u - if this key is specified than unregistration will take place instead of registration
-s - silent mode, no message boxes


LICENSE
-------
This tool is completely free. You may use and redistribute it without any limitations.
