/*****************************************************************
 
 ProgID: Graybox.Sample.MinOPC
 CLSID:  {0233D094-F7AE-48fe-9334-6691E9D22042}

 Compiler options:
 1. Add additional include directory that contains GB_OPCDA.h
 2. Set 'Treat wchar_t as a built in type' to 'no'
 3. Set 'Create/Use precompiled headeers' to 'not use'
 4. Turn off 'Buffer Security Check' (for some platforms only).

 Linker options:
 1. Add additional library directory that contains gbda3.lib for
    your target platform.

 Dependencies:
    gbda3.dll
	opcproxy.dll
	opccomn_ps.dll

 Windows CE:
 1. Your OS image must include DCOM.
 2. You will need proxy/stub libraries for OPC Data Access and
    OPC Common, built for your target platform (opcproxy.dll and
	opccomn_ps.dll).
 3. To be able to browse your CE device for the list of the
    installed OPC servers with a remote OPC browser, you will
    also need: a proxy/stub for IEnumGUID (opcceproxy.dll),
	components categories manager (ccm.dll), OPC.ServerList object
	(opcenum.exe). Also you will have to setup the DCOM permissions.
You can find all these modules in
    Toolkit/Source.
The binaries for several platforms are available in
    Toolkit/Bin/Windows CE

*****************************************************************/

#if !defined(_WIN32_DCOM) && !defined(_WIN32_WCE)
#define _WIN32_DCOM
#endif

#include <windows.h>
#include <GB_OPCDA.h>

#ifdef __BORLANDC__
#pragma comment (lib, "gbda3_omf.lib")
#include <vcl.h>
#endif

#ifdef _WIN32_WCE
#pragma comment (lib, "gbda3.lib")
#pragma comment (linker, "/nodefaultlib:libc.lib")
#pragma comment (linker, "/nodefaultlib:libcd.lib")
#endif

// {0233D094-F7AE-48fe-9334-6691E9D22042}
static GUID guid = { 0x0233d094, 0xf7ae, 0x48fe, { 0x93, 0x34, 0x66, 0x91, 0xe9, 0xd2, 0x20, 0x42 } };

class CMinimalServer : public GBDataAccess
{
public:
	HRESULT __stdcall GBOnQueryLocales(
			DWORD* pdwCount,
			LCID** ppdwLcid)
	{
		static const LCID deflcid[7] = {
			MAKELCID( MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL), SORT_DEFAULT ),
			MAKELCID( MAKELANGID(LANG_ENGLISH, SUBLANG_NEUTRAL), SORT_DEFAULT ),
			LOCALE_SYSTEM_DEFAULT,
			MAKELCID( MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), SORT_DEFAULT ),	//1024
			MAKELCID( MAKELANGID(LANG_ENGLISH, SUBLANG_DEFAULT), SORT_DEFAULT ),
			MAKELCID( MAKELANGID(LANG_RUSSIAN, SUBLANG_DEFAULT), SORT_DEFAULT ),	//1049
			MAKELCID( MAKELANGID(LANG_RUSSIAN, SUBLANG_NEUTRAL), SORT_DEFAULT )
		};
		*ppdwLcid = (LCID*)&deflcid;
		*pdwCount = 7;
		return S_OK;
	}
	HRESULT __stdcall GBOnServerReleased()
	{
		PostThreadMessage( m_threadid, WM_QUIT, 0, 0 );
		return S_FALSE;
	}
	DWORD m_threadid;
};

#ifdef _WIN32_WCE
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPWSTR lpCmdLine, int nShowCmd)
#else
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd)
#endif
{
	CMinimalServer srv;
	wchar_t *cmd_line = GetCommandLine();
	wchar_t *token;
	if ( (token = wcschr(cmd_line, L'-')) == NULL ) token = wcschr(cmd_line, L'/');
	if (token)
	{
		token++;
		if ( !lstrcmpi(token, L"regserver") || !lstrcmpi(token, L"r") )
		{
			srv.GBRegisterServer(&guid, L"Graybox", L"Graybox OPC Server Toolkit Sample Server", L"Graybox.Sample.MinOPC", L"1.0");
			return 0;
		}
		if ( !lstrcmpi(token, L"unregserver") || !lstrcmpi(token, L"u") )
		{
			srv.GBUnregisterServer(&guid);
			return 0;
		}
	}

	CoInitializeEx(0,0);

	srv.m_threadid = GetCurrentThreadId();
	srv.GBInitialize(&guid, 125, 0, GB_SRV_NOACCESSPATH, L'.', 100, 1, 2, 3, L"Graybox Software");

	DWORD ids[10], i;
	VARIANT val;
	val.vt = VT_UI4;
	for (i = 0; i<10; i++)
	{
		wchar_t opcid[32];
		wsprintfW(opcid, L"Folder.Item%02i", i);
		val.ulVal = i;
		srv.GBCreateItem(&ids[i], i, opcid, OPC_READABLE|OPC_WRITEABLE, 0, &val);
	}
	srv.GBBeginUpdate();
	srv.GBEndUpdate(TRUE);

	srv.GBRegisterClassObject();

	MSG msg;
	do
	{
		GetMessage(&msg, NULL, 0, 0);
		TranslateMessage(&msg); 
		DispatchMessage(&msg);
	}
	while (msg.message != WM_QUIT);

	srv.GBRevokeClassObject();
	CoUninitialize();
	return 0;
}

