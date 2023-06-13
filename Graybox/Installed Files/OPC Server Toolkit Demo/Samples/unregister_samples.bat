@echo off
check_dotnet.vbs
set CLR=%ErrorLevel%

echo Registering native OPC Server Samples
@echo on
regsvr32 bin\minopcinp.dll -u -s
bin\graysim.exe -unregserver
bin\minopc.exe -unregserver
bin\vclopc.exe -unregserver
@echo off

if %CLR%==0 (
  echo .NET2.0 is not installed - .NET Samples skipped
  goto ret
  )

echo Registering .NET OPC Server Samples
@echo on
bin\clrminopc.exe -unregserver
bin\clrlifetime.exe -unregserver
bin\clrtagpolling -unregserver
bin\clrcreatetags -unregserver
bin\clropcproperties -unregserver
@echo off

:ret
