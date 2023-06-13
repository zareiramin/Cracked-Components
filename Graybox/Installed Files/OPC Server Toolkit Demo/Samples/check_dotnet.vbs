' Returns 1 if .NET2.0 is installed, returns 0 otherwise
Option Explicit
Dim oShell
Dim value
On Error Resume Next
Set oShell = CreateObject("WScript.Shell")
value = oShell.RegRead("HKLM\SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727\Install")
If Err.Number = 0 Then
	' .NET 2.0 installed
	WScript.Quit 1
Else
	' .NET 2.0 not installed
	WScript.Quit 0
End If
