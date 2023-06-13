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

	CatRegister.cpp

Abstract:

	This file contains the implementation for the ICatRegister interface.

Environment:

	Windows CE

*/


#include "ccm.h"



/*++

Routine Name: 

	CComCat::QueryInterface

Routine Description:

	Query interface method from IUnknown interface.

Arguments:

	riid:	Interface Id
	ppv:	Object to be queried

Return Value:

	HRESULT to indicate return status

--*/

HRESULT CComCat::QueryInterface(REFIID riid, void** ppv)
{
	HRESULT hr = S_OK;

	if(riid == IID_IUnknown)
	{
		*ppv = (IUnknown*)(ICatInformation*) this;
	}
	else if(riid == IID_ICatInformation)
	{
		*ppv = (ICatInformation*) this;
	}
	else if(riid == IID_ICatRegister)
	{
		*ppv = (ICatRegister*) this;
	}
	else
	{
		*ppv = NULL;
		hr = E_NOINTERFACE;
		goto exit;
	}
	static_cast<IUnknown*>(*ppv)->AddRef();
	
exit:
	return hr;
	
} // CComCat::QueryInterface



/*++

Routine Name: 

	CComCat::AddRef

Routine Description:

	Increment the reference count.

Return Value:

	ULONG to indicate return status

--*/

ULONG CComCat::AddRef(void)
{
	return InterlockedIncrement(&m_cRef);
}



/*++

Routine Name: 

	CComCat::Release

Routine Description:

	Decrement the reference count and delete the current object if
	the ref count is zero.

Return Value:

	ULONG to indicate return status

--*/

ULONG CComCat::Release(void)
{
	unsigned cRef = InterlockedDecrement(&m_cRef);
	if(cRef == 0)
	{
		delete this;
	}

	return cRef;
}



/*++

Routine Name: 

	CComCat::RegisterCategories

Routine Description:

	Registers one or more component categories. Each component category 
	consists of a CATID and a list of locale-dependent description strings.

Arguments:

	cCategories:	Number of component categories
	rgCategoryInfo: Array of cCategories CATEGORYINFO structures

Return Value:

	HRESULT to indicate return status

--*/

STDMETHODIMP CComCat::RegisterCategories(ULONG cCategories, CATEGORYINFO rgCategoryInfo[])
{
	HRESULT 	hr = S_OK;
	HRESULT		RetHr = S_OK;
	WCHAR 		lpSubKey[MAX_REG_KEY_LEN];
	WCHAR		lpLocale[MAX_LOCALE_LEN];
	OLECHAR		lpCatid[MAX_GUID_STR_LEN];
	HKEY		hKey;
	DWORD		dwDisposition;
	DWORD		cdwCh;

	// For each category in the array
	for(ULONG i=0;i<cCategories;i++)
	{
		// Convert CATID to a string
		cdwCh = StringFromGUID2(rgCategoryInfo[i].catid, lpCatid, MAX_GUID_STR_LEN);
		if(cdwCh != 0)
		{
			// Create the string representing the key
			wcscpy(lpSubKey, CATID_KEY_HEADER);
			wcscat(lpSubKey, L"\\");
			wcscat(lpSubKey, lpCatid);

			// Create reg key
			hr = RegCreateKeyEx(HKEY_CLASSES_ROOT,
						lpSubKey,
						0,
						NULL,
						0,
						0,
						NULL,
						&hKey,
						&dwDisposition);
			if(hr == S_OK && dwDisposition == REG_CREATED_NEW_KEY)
			{
				// Get locale string
				wsprintf(lpLocale, TEXT("%X"), rgCategoryInfo[i].lcid);
			
				// Set reg value
				hr = RegSetValueEx(hKey,
								lpLocale,
								0,
								REG_SZ,
								(BYTE *)rgCategoryInfo[i].szDescription,
								128);

				RegCloseKey(hKey);
			}

			// If one of the categories failed, continue but make sure the return 
			// value is E_INVALIDARG to let the caller know that there was a problem
			if(hr != ERROR_SUCCESS)
			{
				RetHr = E_INVALIDARG;
			}
		}
	}

	return RetHr;
	
} // CComCat::RegisterCategories



/*++

Routine Name: 

	CComCat::UnRegisterCategories

Routine Description:

	Removes the registration of one or more component categories. Each component 
	category consists of a CATID and a list of locale-dependent description strings.

Arguments:

	cCategories:	Number of cCategories CATIDs to be removed
	rgcatid:		Array of cCategories CATIDs

Return Value:

	HRESULT to indicate return status

--*/

STDMETHODIMP CComCat::UnRegisterCategories(ULONG cCategories, CATID rgcatid[])
{
	HRESULT 	hr = S_OK;
	HRESULT		RetHr = S_OK;
	WCHAR 		lpSubKey[MAX_REG_KEY_LEN];
	OLECHAR		lpCatid[MAX_GUID_STR_LEN];
	DWORD		cdwCh;

	// For each category in the array
	for(ULONG i=0;i<cCategories;i++)
	{	
		// Convert CATID to a string
		cdwCh = StringFromGUID2(rgcatid[i], lpCatid, MAX_GUID_STR_LEN);
		if(cdwCh != 0)
		{
			// Create the string representing the key
			wcscpy(lpSubKey, CATID_KEY_HEADER);
			wcscat(lpSubKey, L"\\");
			wcscat(lpSubKey, lpCatid);

			// Create reg key
			hr = RegDeleteKey(HKEY_CLASSES_ROOT, lpSubKey);
		}

		// If one of the categories failed, continue but make sure the return 
		// value is E_INVALIDARG to let the caller know that there was a problem
		if(hr != ERROR_SUCCESS)
		{
			RetHr = E_INVALIDARG;
		}
	}

	return RetHr;	

} // CComCat::UnRegisterCategories



/*++

Routine Name: 

	CComCat::RegisterClassImplCategories

Routine Description:

	Registers the class as implementing one or more component categories.

Arguments:

	rclsid:			Class ID of the relevent class
	cCategories:	Number of category CATIDs
	rgcatid:		Array of cCategories CATID

Return Value:

	HRESULT to indicate return status

--*/

HRESULT CComCat::RegisterClassImplCategories(REFCLSID rclsid, ULONG cCategories, CATID rgcatid[])
{
	HRESULT 	hr 				= S_OK;
	HRESULT		RetHr 			= S_OK;
	WCHAR		lpClsid[MAX_GUID_STR_LEN];
	WCHAR		lpSubKey[MAX_REG_KEY_LEN];
	WCHAR* 		lpImplSubKey 	= NULL;
	HKEY		hKey;
	DWORD		dwDisposition;
	DWORD		cdwCh;
	ULONG 		i;

	// Convert CLSID to a string
	cdwCh = StringFromGUID2(rclsid, lpClsid, MAX_GUID_STR_LEN);
	if(cdwCh == 0)
	{
		RetHr = E_INVALIDARG;
		goto exit;
	}

	// Create subkey string
	wcscpy(lpSubKey, CLSID_KEY_HEADER);
	wcscat(lpSubKey, L"\\");
	wcscat(lpSubKey, lpClsid);
	wcscat(lpSubKey, L"\\");
	wcscat(lpSubKey, IMPL_CAT_KEY_HEADER);
	wcscat(lpSubKey, L"\\");

	lpImplSubKey = lpSubKey + wcslen(lpSubKey);
		
	for(i=0;i<cCategories;i++)
	{
		cdwCh = StringFromGUID2(rgcatid[i], lpImplSubKey, MAX_GUID_STR_LEN);
		if(cdwCh != 0)
		{
			// Create reg key
			hr = RegCreateKeyEx(HKEY_CLASSES_ROOT,
						lpSubKey,
						0,
						NULL,
						0,
						0,
						NULL,
						&hKey,
						&dwDisposition);						
			if(hr != STATUS_SUCCESS)
			{
				RetHr = E_INVALIDARG;
			}

			RegCloseKey(hKey);
		}
	}

exit:
	return RetHr;

} // CComCat::RegisterClassImplCategories



/*++

Routine Name: 

	CComCat::UnRegisterClassImplCategories

Routine Description:

	Removes one or more implemented category identifiers from a class.

Arguments:

	rclsid:			Class ID of the relevent class
	cCategories:	Number of category CATIDs
	rgcatid:		Array of cCategories CATID

Return Value:

	HRESULT to indicate return status

--*/

HRESULT CComCat::UnRegisterClassImplCategories(REFCLSID rclsid, ULONG cCategories, CATID rgcatid[])
{
	HRESULT 	hr 				= S_OK;
	HRESULT		RetHr 			= S_OK;
	WCHAR		lpClsid[MAX_GUID_STR_LEN];
	WCHAR		lpSubKey[MAX_REG_KEY_LEN];
	WCHAR* 		lpImplSubKey 	= NULL;
	ULONG 		i;
	DWORD		cdwCh;
	DWORD		cdwSubKeys;
	HKEY		hKey;

	// Convert CLSID to a string
	cdwCh = StringFromGUID2(rclsid, lpClsid, MAX_GUID_STR_LEN);
	if(cdwCh == 0)
	{
		RetHr = E_INVALIDARG;
		goto exit;
	}

	// Create subkey string
	wcscpy(lpSubKey, CLSID_KEY_HEADER);
	wcscat(lpSubKey, L"\\");
	wcscat(lpSubKey, lpClsid);
	wcscat(lpSubKey, L"\\");
	wcscat(lpSubKey, IMPL_CAT_KEY_HEADER);
	wcscat(lpSubKey, L"\\");

	lpImplSubKey = lpSubKey + wcslen(lpSubKey);

	// Loop through the list of CATIDs and delete them from the registry
	for(i=0;i<cCategories;i++)
	{
		cdwCh = StringFromGUID2(rgcatid[i], lpImplSubKey, MAX_GUID_STR_LEN);
		if(cdwCh != 0)
		{
			// Create reg key
			hr = RegDeleteKey(HKEY_CLASSES_ROOT, lpSubKey);
			if(hr != STATUS_SUCCESS)
			{
				RetHr = E_INVALIDARG;
			}
		}
	}

	// Set the Subkey back to IMPL_CAT_KEY_HEADER
	*(lpImplSubKey-1) = NULL;

	// Open the reg key which is a parent to all implemented catids for a class
	hr = RegOpenKeyEx(HKEY_CLASSES_ROOT,
					lpSubKey,
					0,
					0,
					&hKey);
	if(hr != S_OK)
	{
		RetHr = E_INVALIDARG;
		goto exit;
	}

	// Get the number of subkeys
	hr = GetNumSubKeys(hKey, &cdwSubKeys);
	if(hr != S_OK)
	{
		RetHr = E_INVALIDARG;
		RegCloseKey(hKey);
		goto exit;
	}

	RegCloseKey(hKey);

	// If all the keys underneath this key have been deleted, then delete this key
	if(cdwSubKeys == 0)
	{
		RegDeleteKey(HKEY_CLASSES_ROOT, lpSubKey);
	}

exit:
	return RetHr;

} // CComCat::UnRegisterClassImplCategories



/*++

Routine Name: 

	CComCat::RegisterClassReqCategories

Routine Description:

	Registers the class as requiring one or more component categories.

Arguments:

	rclsid:			Class ID of the relevent class
	cCategories:	Number of category CATIDs
	rgcatid:		Array of cCategories CATID

Return Value:

	HRESULT to indicate return status

--*/

HRESULT CComCat::RegisterClassReqCategories(REFCLSID rclsid, ULONG cCategories, CATID rgcatid[])
{
	HRESULT 	hr 				= S_OK;
	HRESULT		RetHr 			= S_OK;
	WCHAR		lpClsid[MAX_GUID_STR_LEN];
	WCHAR		lpSubKey[MAX_REG_KEY_LEN];
	WCHAR*		lpReqSubKey 	= NULL;
	HKEY		hKey;
	DWORD		dwDisposition;
	DWORD		cdwCh;
	ULONG 		i;

	// Convert CLSID to a string
	cdwCh = StringFromGUID2(rclsid, lpClsid, MAX_GUID_STR_LEN);
	if(cdwCh == 0)
	{
		RetHr = E_INVALIDARG;
		goto exit;
	}

	// Create subkey string
	wcscpy(lpSubKey, CLSID_KEY_HEADER);
	wcscat(lpSubKey, L"\\");
	wcscat(lpSubKey, lpClsid);
	wcscat(lpSubKey, L"\\");
	wcscat(lpSubKey, REQ_CAT_KEY_HEADER);
	wcscat(lpSubKey, L"\\");

	lpReqSubKey = lpSubKey + wcslen(lpSubKey);
		
	for(i=0;i<cCategories;i++)
	{
		cdwCh = StringFromGUID2(rgcatid[i], lpReqSubKey, MAX_GUID_STR_LEN);
		if(cdwCh != 0)
		{
			// Create reg key
			hr = RegCreateKeyEx(HKEY_CLASSES_ROOT,
						lpSubKey,
						0,
						NULL,
						0,
						0,
						NULL,
						&hKey,
						&dwDisposition);			
			if(hr != STATUS_SUCCESS)
			{
				RetHr = E_INVALIDARG;
			}

			RegCloseKey(hKey);
		}
	}

exit:
	return RetHr;
	
} // CComCat::RegisterClassReqCategories



/*++

Routine Name: 

	CComCat::UnRegisterClassReqCategories

Routine Description:

	Removes one or more required category identifiers from a class.

Arguments:

	rclsid:			Class ID of the relevent class
	cCategories:	Number of category CATIDs
	rgcatid:		Array of cCategories CATID

Return Value:

	HRESULT to indicate return status

--*/

HRESULT CComCat::UnRegisterClassReqCategories(REFCLSID rclsid, ULONG cCategories, CATID rgcatid[])
{
	HRESULT 	hr 				= S_OK;
	HRESULT		RetHr 			= S_OK;
	WCHAR		lpClsid[MAX_GUID_STR_LEN];
	WCHAR		lpSubKey[MAX_REG_KEY_LEN];
	WCHAR* 		lpReqSubKey 	= NULL;
	ULONG 		i;
	DWORD		cdwCh;
	DWORD		cdwSubKeys 	= 0;
	HKEY		hKey;

	// Convert CLSID to a string
	cdwCh = StringFromGUID2(rclsid, lpClsid, MAX_GUID_STR_LEN);
	if(cdwCh == 0)
	{
		RetHr = E_INVALIDARG;
		goto exit;
	}

	// Create subkey string
	wcscpy(lpSubKey, CLSID_KEY_HEADER);
	wcscat(lpSubKey, L"\\");
	wcscat(lpSubKey, lpClsid);
	wcscat(lpSubKey, L"\\");
	wcscat(lpSubKey, REQ_CAT_KEY_HEADER);
	wcscat(lpSubKey, L"\\");

	lpReqSubKey = lpSubKey + wcslen(lpSubKey);

	// Loop through the list of CATIDs and delete them from the registry		
	for(i=0;i<cCategories;i++)
	{
		cdwCh = StringFromGUID2(rgcatid[i], lpReqSubKey, MAX_GUID_STR_LEN);
		if(cdwCh != 0)
		{
			// Create reg key
			hr = RegDeleteKey(HKEY_CLASSES_ROOT, lpReqSubKey);
			if(hr != STATUS_SUCCESS)
			{
				RetHr = E_INVALIDARG;
			}
		}
	}

	// Set the Subkey back to IMPL_CAT_KEY_HEADER
	*(lpReqSubKey-1) = NULL;

	// Open the reg key which is a parent to all required catids for a class
	hr = RegOpenKeyEx(HKEY_CLASSES_ROOT,
					lpSubKey,
					0,
					0,
					&hKey);
	if(hr != S_OK)
	{
		RetHr = E_INVALIDARG;
		goto exit;
	}

	// Get the number of subkeys
	hr = GetNumSubKeys(hKey, &cdwSubKeys);
	if(hr != S_OK)
	{
		RetHr = E_INVALIDARG;
		RegCloseKey(hKey);
		goto exit;
	}

	RegCloseKey(hKey);

	// If all the keys underneath this key have been deleted, then delete this key
	if(cdwSubKeys == 0)
	{
		RegDeleteKey(HKEY_CLASSES_ROOT, lpSubKey);
	}

exit:
	return RetHr;

} // CComCat::UnRegisterClassReqCategories



