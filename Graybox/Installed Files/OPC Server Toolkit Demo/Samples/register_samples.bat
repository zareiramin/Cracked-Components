@echo off
check_dotnet.vbs
set CLR=%ErrorLevel%

echo Registering native OPC Server Samples
@echo on
regsvr32 bin\minopcinp.dll -s
bin\graysim.exe -regserver
bin\minopc.exe -regserver
bin\vclopc.exe -regserver
@echo off

if %CLR%==0 (
  echo .NET2.0 is not installed - .NET Samples skipped
  goto ret
  )

echo Registering .NET OPC Server Samples
@echo on
bin\clrminopc.exe -regserver
bin\clrlifetime.exe -regserver
bin\clrtagpolling -regserver
bin\clrcreatetags -regserver
bin\clropcproperties -regserver
@echo off

:ret
