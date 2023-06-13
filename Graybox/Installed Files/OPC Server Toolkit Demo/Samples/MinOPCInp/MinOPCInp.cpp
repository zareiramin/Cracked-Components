/*****************************************************************

 ProgID: Graybox.Sample.MinOPCInproc
 CLSID:  {9188AF0A-4E88-4a37-815D-FDB391F41C3D}

 Compiler options:
 1. Add additional include directory that contains GB_OPCDA.h
 2. Set 'Treat wchar_t as a built in type' to 'no'
 3. Set 'Create/Use precompiled headeers' to 'not use'
 4. Turn off 'Buffer Security Check' (for some platforms only).
 
 Linker options:
 1. Add additional library directory that contains gbda3.lib for
    your target platform.
 2. Set 'Module Definition File' to minopcinp.def.

 Dependencies:
    gbda3.dll

*****************************************************************/

#if !defined(_WIN32_DCOM) && !defined(_WIN32_WCE)
#define _WIN32_DCOM
#endif

#ifdef _WIN32_WCE
#pragma comment (lib, "gbda3.lib")
#pragma comment (linker, "/nodefaultlib:libc.lib")
#pragma comment (linker, "/nodefaultlib:libcd.lib")
#endif


#include <windows.h>
#include "resource.h"
#include <GB_OPCDA.h>

// {9188AF0A-4E88-4a37-815D-FDB391F41C3D}
static const GUID guid = 
{ 0x9188af0a, 0x4e88, 0x4a37, { 0x81, 0x5d, 0xfd, 0xb3, 0x91, 0xf4, 0x1c, 0x3d } };

class CMinimalServer : public GBDataAccessInproc
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
};


HMODULE hModule;
long nInitialized = 0;
CMinimalServer* srv = NULL;

// Module entry point.
// Note that we can't initialize/uninitialize OPC Server here.
BOOL APIENTRY DllMain( HANDLE module, 
                       DWORD ul_reason_for_call, 
                       LPVOID reserved )
{
	if (ul_reason_for_call == DLL_PROCESS_ATTACH)
	{
		DisableThreadLibraryCalls(hModule);
		hModule = (HMODULE)module;
		GBSetResourceModule((HMODULE)module);
	}
	return TRUE;
}

// Used to determine whether the DLL can be unloaded by OLE
STDAPI DllCanUnloadNow(void)
{
	if ( !InterlockedExchangeAdd((long*)&nInitialized, 0) ) return S_OK;
	HRESULT hr = srv->GBCanUnloadNow();
	if (hr == S_OK)
	{
		InterlockedExchange((long*)&nInitialized, 0);
		delete srv;
		srv = NULL;
	}
	return hr;
}

// Returns a class factory to create an object of the requested type
STDAPI DllGetClassObject(REFCLSID rclsid, REFIID riid, LPVOID* ppv)
{
	if ( InterlockedIncrement((long*)&nInitialized)==1 )	// need initialization, the OPC Server instance is being created for the first time
	{
		srv = new CMinimalServer();
		srv->GBInitialize((GUID*)&guid, 125, 0, GB_SRV_NOACCESSPATH, L'.', 100, 1, 0, 0, MAKEINTRESOURCE(IDS_VENDOR));
		DWORD ids[10], i;
		VARIANT val;
		val.vt = VT_UI4;
		for (i = 0; i<10; i++)
		{
			wchar_t opcid[32];
			wsprintfW(opcid, L"Folder.Item%02i", i);
			val.ulVal = i;
			srv->GBCreateItem(&ids[i], i, opcid, OPC_READABLE|OPC_WRITEABLE, 0, &val);
		}
		srv->GBBeginUpdate();
		srv->GBEndUpdate(TRUE);
	}
	return srv->GBGetClassObject(rclsid, riid, ppv);
}

// DllRegisterServer - Adds entries to the system registry
STDAPI DllRegisterServer(void)
{
	return GBDataAccessInproc::GBRegisterServer(
		hModule,
		(GUID*)&guid,
		MAKEINTRESOURCE(IDS_VENDOR),
		MAKEINTRESOURCE(IDS_DESCR),
		MAKEINTRESOURCE(IDS_VIPROGID),
		MAKEINTRESOURCE(IDS_VERSION));
}

// DllUnregisterServer - Removes entries from the system registry
STDAPI DllUnregisterServer(void)
{
	return GBDataAccessInproc::GBUnregisterServer(hModule, (GUID*)&guid);
}
