//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//
//
// Use of this source code is subject to the terms of the Microsoft end-user
// license agreement (EULA) under which you licensed this SOFTWARE PRODUCT.
// If you did not accept the terms of the EULA, you are not authorized to use
// this source code. For a copy of the EULA, please see the LICENSE.RTF on your
// install media.
//
/*


Module Name:

	ccm.cpp

Abstract:

	DLL entry point for the Component Category Manager dll

Environment:

	Windows CE

*/


#include "ccm.h"
#include "registry.h"


LONG 		g_cServerLocks = 0;
LONG 		g_cComponents = 0;
HINSTANCE	g_hInstance;


/*++

Routine Name: 

	DllEntry

Routine Description:

	Entry point of the DLL.

Arguments:

	hInstDll:		Instance variable
	dwReason:		Reason for calling DllEntry
	lpReserved:		

Return Value:

	BOOL

--*/

BOOL WINAPI DllEntry(HINSTANCE hInstDll, DWORD dwReason, LPVOID lpReserved)
{
	g_hInstance = hInstDll;
	
	return TRUE;
	
} // DllEntry



HRESULT __stdcall DllCanUnloadNow()
{
	if(g_cServerLocks == 0 && g_cComponents == 0)
		return S_OK;
	else
		return S_FALSE;
}



HRESULT __stdcall DllGetClassObject(REFCLSID clsid, REFIID riid, void** ppv)
{
	if(clsid != CLSID_StdComponentCategoriesMgr)
		return CLASS_E_CLASSNOTAVAILABLE;

	CFactory* pFactory = new CFactory;
	if(pFactory == NULL)
		return E_OUTOFMEMORY;

	// QueryInterface probably for IClassFactory
	HRESULT hr = pFactory->QueryInterface(riid, ppv);
	pFactory->Release();
	return hr;
}



HRESULT __stdcall DllRegisterServer()
{
	return RegisterServer(L"ccm.dll", CLSID_StdComponentCategoriesMgr, L"Component Categories Manager", L"Comcat.COM", L"Comcat.COM.1", L"Both");
}



HRESULT __stdcall DllUnregisterServer()
{
	return UnregisterServer(CLSID_StdComponentCategoriesMgr, L"Comcat.COM", L"Comcat.COM.1");
}


