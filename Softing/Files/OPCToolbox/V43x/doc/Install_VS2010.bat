@ECHO OFF
setlocal
set ACTION_TYPE=%1
set OS_TYPE=%2


@ECHO.
@ECHO Install_VS2010.bat %ACTION_TYPE% %OS_TYPE%
@ECHO -------------------------------------------------------------------------


if "%ACTION_TYPE%" == "u" (
	if "%OS_TYPE%" == "x64" (
	echo "%ProgramFiles%\Microsoft Help Viewer\v1.0\HelpLibManager.exe" /product VS /version 100 /locale en-us /silent /uninstall /vendor "Softing Industrial Automation Gmbh" /mediaBookList "Softing OPC Toolbox V4.3x" /productName "Softing OPC Toolbox V4.3x"
		"%ProgramFiles%\Microsoft Help Viewer\v1.0\HelpLibManager.exe" /product VS /version 100 /locale en-us /silent /uninstall /vendor "Softing Industrial Automation Gmbh" /mediaBookList "Softing OPC Toolbox V4.3x" /productName "Softing OPC Toolbox V4.3x"
	)
	if "%OS_TYPE%" == "x86" (
	echo "%ProgramFiles%\Microsoft Help Viewer\v1.0\HelpLibManager.exe" /product VS /version 100 /locale en-us /silent /uninstall /vendor "Softing Industrial Automation Gmbh" /mediaBookList "Softing OPC Toolbox V4.3x" /productName "Softing OPC Toolbox V4.3x"
		"%ProgramFiles%\Microsoft Help Viewer\v1.0\HelpLibManager.exe" /product VS /version 100 /locale en-us /silent /uninstall /vendor "Softing Industrial Automation Gmbh" /mediaBookList "Softing OPC Toolbox V4.3x" /productName "Softing OPC Toolbox V4.3x"
	)
)

if "%ACTION_TYPE%" == "r" (
	if "%OS_TYPE%" == "x64" (
	echo "%ProgramFiles%\Microsoft Help Viewer\v1.0\HelpLibManager.exe" /product VS /version 100 /locale en-us /silent -content "%AppData%\Microsoft\HelpLibrary" /sourceMedia "%ProgramFiles(x86)%\Softing\OPCToolbox\V43x\doc\SoftingOPCToolboxV43x.msha"
		"%ProgramFiles%\Microsoft Help Viewer\v1.0\HelpLibManager.exe" /product VS /version 100 /locale en-us /silent -content "%AppData%\Microsoft\HelpLibrary" /sourceMedia "%ProgramFiles(x86)%\Softing\OPCToolbox\V43x\doc\SoftingOPCToolboxV43x.msha"
	)

	if "%OS_TYPE%" == "x86" (
	echo "%ProgramFiles%\Microsoft Help Viewer\v1.0\HelpLibManager.exe" /product VS /version 100 /locale en-us /silent -content "%AppData%\Microsoft\HelpLibrary" /sourceMedia "%ProgramFiles%\Softing\OPCToolbox\V43x\doc\SoftingOPCToolboxV43x.msha"
		"%ProgramFiles%\Microsoft Help Viewer\v1.0\HelpLibManager.exe" /product VS /version 100 /locale en-us /silent -content "%AppData%\Microsoft\HelpLibrary" /sourceMedia "%ProgramFiles%\Softing\OPCToolbox\V43x\doc\SoftingOPCToolboxV43x.msha"
	)
)
