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
/*++
THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
PARTICULAR PURPOSE.

Sample code derived from "Inside Distributed COM" by Guy and Henry Eddon (© 1998,
Guy and Henry Eddon, published by Microsoft Press, ISBN 1-57231-849-X). 
Modified with permission.
*/

// Registry.cpp
#include <objbase.h>
#include "registry.h"

#if defined (UNDER_CE)
#define assert ASSERT
#else
#include <assert.h>
#endif

// Set the given key and its value.
BOOL setKeyAndValue(const WCHAR * pszPath, const WCHAR * szSubkey, const WCHAR * szValue);

// Open a key and set a value.
BOOL setValueInKey(const WCHAR * szKey, const WCHAR * szNamedValue, const WCHAR * szValue);

// Convert a CLSID into a char string.
void CLSIDtochar(REFCLSID clsid, WCHAR * szCLSID, int length);

// Delete szKeyChild and all of its descendents.
LONG recursiveDeleteKey(HKEY hKeyParent, const WCHAR * szKeyChild);

// Size of a CLSID as a string
const int CLSID_STRING_SIZE = 39;

// Register the component in the registry.
HRESULT RegisterServer(const WCHAR * szModuleName,     // DLL module handle
                       REFCLSID clsid,               // Class ID
                       const WCHAR * szFriendlyName,   // Friendly Name
                       const WCHAR * szVerIndProgID,   // Programmatic
                       const WCHAR * szProgID,         // IDs
					   const WCHAR * szThreadingModel) // ThreadingModel
{
	// Get server location.
	WCHAR szModule[512];
	HMODULE hModule = GetModuleHandle(szModuleName);
	DWORD dwResult = GetModuleFileName(hModule, szModule, sizeof(szModule)/sizeof(WCHAR));
	assert(dwResult != 0);

	// Convert the CLSID into a char.
	WCHAR szCLSID[CLSID_STRING_SIZE];
	CLSIDtochar(clsid, szCLSID, sizeof(szCLSID) / sizeof(WCHAR));

	// Build the key CLSID\\{...}
	WCHAR szKey[64];
	wcscpy(szKey, L"CLSID\\");
	wcscat(szKey, szCLSID);
  
	// Add the CLSID to the registry.
	setKeyAndValue(szKey, NULL, szFriendlyName);

	// Add the server filename subkey under the CLSID key.
	if(wcsstr(szModuleName, L".exe") == NULL)
	{
		setKeyAndValue(szKey, L"InprocServer32", szModule);
		WCHAR szInproc[64];
		wcscpy(szInproc, szKey);
		wcscat(szInproc, L"\\InprocServer32");
		setValueInKey(szInproc, L"ThreadingModel", szThreadingModel);
	}
	else
		setKeyAndValue(szKey, L"LocalServer32", szModule);

	// Add the ProgID subkey under the CLSID key.
	setKeyAndValue(szKey, L"ProgID", szProgID);

	// Add the version-independent ProgID subkey under CLSID key.
	setKeyAndValue(szKey, L"VersionIndependentProgID", szVerIndProgID);

	// Add the version-independent ProgID subkey under HKEY_CLASSES_ROOT.
	setKeyAndValue(szVerIndProgID, NULL, szFriendlyName); 
	setKeyAndValue(szVerIndProgID, L"CLSID", szCLSID);
	setKeyAndValue(szVerIndProgID, L"CurVer", szProgID);

	// Add the versioned ProgID subkey under HKEY_CLASSES_ROOT.
	setKeyAndValue(szProgID, NULL, szFriendlyName); 
	setKeyAndValue(szProgID, L"CLSID", szCLSID);

	return S_OK;
}

// Remove the component from the registry.
LONG UnregisterServer(REFCLSID clsid,             // Class ID
                      const WCHAR * szVerIndProgID, // Programmatic
                      const WCHAR * szProgID)       // IDs
{
	// Convert the CLSID into a char.
	WCHAR szCLSID[CLSID_STRING_SIZE];
	CLSIDtochar(clsid, szCLSID, sizeof(szCLSID)/sizeof(WCHAR));

	// Build the key CLSID\\{...}
	WCHAR szKey[64];
	wcscpy(szKey, L"CLSID\\");
	wcscat(szKey, szCLSID);

	// Delete the CLSID Key - CLSID\{...}
	LONG lResult = recursiveDeleteKey(HKEY_CLASSES_ROOT, szKey);
	assert((lResult == ERROR_SUCCESS) || (lResult == ERROR_FILE_NOT_FOUND)); // Subkey may not exist.

	// Delete the version-independent ProgID Key.
	lResult = recursiveDeleteKey(HKEY_CLASSES_ROOT, szVerIndProgID);
	assert((lResult == ERROR_SUCCESS) || (lResult == ERROR_FILE_NOT_FOUND)); // Subkey may not exist.

	// Delete the ProgID key.
	lResult = recursiveDeleteKey(HKEY_CLASSES_ROOT, szProgID);
	assert((lResult == ERROR_SUCCESS) || (lResult == ERROR_FILE_NOT_FOUND)); // Subkey may not exist.

	return S_OK;
}

// Convert a CLSID to a char string.
void CLSIDtochar(REFCLSID clsid, WCHAR * szCLSID, int length)
{
	assert(length >= CLSID_STRING_SIZE);
	// Get CLSID
	LPOLESTR wszCLSID = NULL;
	HRESULT hr = StringFromCLSID(clsid, &wszCLSID);
	assert(SUCCEEDED(hr));

	// Covert from wide characters to non-wide.
	wcsncpy (szCLSID, wszCLSID, length);

	// Free memory.
	CoTaskMemFree(wszCLSID);
}

// Delete a key and all of its descendents.
LONG recursiveDeleteKey(HKEY hKeyParent,           // Parent of key to delete
                        const WCHAR* lpszKeyChild)  // Key to delete
{
	// Open the child.
	HKEY hKeyChild;
	LONG lRes = RegOpenKeyEx(hKeyParent, lpszKeyChild, 0, KEY_ALL_ACCESS, &hKeyChild);
	if(lRes != ERROR_SUCCESS)
		return lRes;

	// Enumerate all of the decendents of this child.
	FILETIME time;
	WCHAR szBuffer[256];
	DWORD dwSize = 256;
	while(RegEnumKeyEx(hKeyChild, 0, szBuffer, &dwSize, NULL, NULL, NULL, &time) == S_OK)
	{
		// Delete the decendents of this child.
		lRes = recursiveDeleteKey(hKeyChild, szBuffer);
		if(lRes != ERROR_SUCCESS)
		{
			// Cleanup before exiting.
			RegCloseKey(hKeyChild);
			return lRes;
		}
		dwSize = 256;
	}

	// Close the child.
	RegCloseKey(hKeyChild);

	// Delete this child.
	return RegDeleteKey(hKeyParent, lpszKeyChild);
}

// Create a key and set its value.
BOOL setKeyAndValue(const WCHAR* szKey, const WCHAR* szSubkey, const WCHAR* szValue)
{
	HKEY hKey;
	WCHAR szKeyBuf[1024];

	// Copy keyname into buffer.
	wcscpy(szKeyBuf, szKey);

	// Add subkey name to buffer.
	if(szSubkey != NULL)
	{
		wcscat(szKeyBuf, L"\\");
		wcscat(szKeyBuf, szSubkey );
	}

	// Create and open key and subkey.
	long lResult = RegCreateKeyEx(HKEY_CLASSES_ROOT, szKeyBuf, 0, NULL, REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, NULL, &hKey, NULL);
	if(lResult != ERROR_SUCCESS)
		return FALSE;

	// Set the Value.
	if(szValue != NULL)
		RegSetValueEx(hKey, NULL, 0, REG_SZ, (BYTE *)szValue, (wcslen(szValue)+1) * sizeof(WCHAR));

	RegCloseKey(hKey);
	return TRUE;
}

// Open a key and set a value.
BOOL setValueInKey(const WCHAR* szKey, const WCHAR* szNamedValue, const WCHAR* szValue)
{
	HKEY hKey;
	WCHAR szKeyBuf[1024];

	// Copy keyname into buffer.
	wcscpy(szKeyBuf, szKey);

	// Create and open key and subkey.
	long lResult = RegOpenKeyEx(HKEY_CLASSES_ROOT, szKeyBuf, 0, KEY_SET_VALUE, &hKey);
	if(lResult != ERROR_SUCCESS)
		return FALSE;

    // Set the Value.
	if(szValue != NULL)
		RegSetValueEx(hKey, szNamedValue, 0, REG_SZ, (BYTE*)szValue, (wcslen(szValue)+1) * sizeof(WCHAR));

	RegCloseKey(hKey);
	return TRUE;
}
