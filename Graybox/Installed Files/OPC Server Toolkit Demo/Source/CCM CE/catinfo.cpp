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

	CatInfo.cpp

Abstract:

	This file contains the implementation for the ICatInformation interface.

Environment:

	Windows CE

*/


#include "ccm.h"



/*++

Routine Name: 

	CComCat::EnumCategories

Routine Description:

	Returns an enumerator for the component categories registered on the system.

Arguments:

	lcid:					Requested locale for returned szDescription of 
							enumerated CATEGORYINFOs
	ppenumCategoryInfo:		Location in which to return an IEnumCATEGORYINFO
							interface

Return Value:

	HRESULT to indicate return status

--*/

HRESULT STDMETHODCALLTYPE CComCat::EnumCategories(LCID lcid, IEnumCATEGORYINFO** ppenumCategoryInfo)
{
	HRESULT 			hr 				= S_OK;
	WCHAR				lpGuidStr[MAX_GUID_STR_LEN];
	DWORD				cdwSubKeys 	= 0;
	DWORD				cdwCats 		= 0;
	DWORD				i 				= 0;
	CATEGORYINFO*		pciList			= NULL;
	HKEY				hKey;
	CEnumCatInfoImpl*	pEnumCatInfo 	= NULL;
	
	// Check parameter
	if(ppenumCategoryInfo == NULL)
	{
		hr = E_INVALIDARG;
		goto exit;
	}
	
	// Open the reg key
	hr = RegOpenKeyEx(HKEY_CLASSES_ROOT,
					CATID_KEY_HEADER,
					0,
					0,
					&hKey);
	if(hr != S_OK)
	{
		hr = E_INVALIDARG;
		goto exit;
	}

	// Get the count of subkeys
	hr = GetNumSubKeys(hKey, &cdwSubKeys);
	if(hr != S_OK)
	{
		RegCloseKey(hKey);
		goto exit;
	}

	// Allocate memory
	pciList = new CATEGORYINFO[cdwSubKeys];
	pEnumCatInfo = new CEnumCatInfoImpl;
	if(pciList == NULL || pEnumCatInfo == NULL)
	{
		hr = E_OUTOFMEMORY;
		RegCloseKey(hKey);

		// If pciList failed then we should delete pEnumCatInfo
		if(pEnumCatInfo)
		{
			delete pEnumCatInfo;
		}
		
		goto exit;
	}
	
	// Enumerate the 'Component Categories' key in the registry
	for(i=0;i<cdwSubKeys;i++)
	{
		DWORD	cdwNameBuf = MAX_GUID_STR_LEN;
	
		// Enumerate the ith key
		hr = RegEnumKeyEx(hKey,
						i,
						lpGuidStr,
						&cdwNameBuf,
						NULL,
						NULL,
						NULL,
						NULL);
		if(hr == S_OK)
		{
			HKEY	hSubKey;
				
			// Open the reg key
			hr = RegOpenKeyEx(hKey,
							lpGuidStr,
							0,
							0,
							&hSubKey);
			if(hr == S_OK)
			{
		
				WCHAR	lpName[MAX_LOCALE_LEN]; 

				hr = CLSIDFromString(lpGuidStr, &(pciList[i].catid));
				if(hr == S_OK)
				{
					DWORD	cdwData = sizeof(pciList[i].szDescription);
					pciList[i].lcid = lcid;
					
					// The name for the reg key is the locale id
					wsprintf(lpName, TEXT("%X"), lcid);

					// Get the reg key data
					hr = RegQueryValueEx(hSubKey,
									lpName,
									NULL,
									NULL,
									(LPBYTE) pciList[i].szDescription,
									&cdwData);
					if(hr == S_OK)
					{
						cdwCats++;
					}
				}

				RegCloseKey(hSubKey);
			}
		}
		else if(hr == ERROR_NO_MORE_ITEMS) // This should not happen since we get the count, but do it anyway
		{
			hr = S_OK;
			break;
		}
	}

	RegCloseKey(hKey);

	// Initialize the Enumeration object
	if(cdwCats > 0)
	{
		pEnumCatInfo->Init(cdwCats, pciList);
	}
	
	// Set the out parameter to point to the enumeration interface
	*ppenumCategoryInfo = (IEnumCATEGORYINFO *) pEnumCatInfo;

exit:
	
	if(pciList)
	{
		delete[] pciList;
	}
	
	return hr;

} // CComCat::EnumCategories



/*++

Routine Name: 

	CComCat::GetCategoryDesc

Routine Description:

	Retrieves the localized description string for a specific category ID.

Arguments:

	rcatid:		Category for which the string is to be returned
	lcid:		Locale in which the resulting string is returned
	pszDesc:	Pointer to the string pointer that contains the description

Return Value:

	HRESULT to indicate return status

--*/

HRESULT STDMETHODCALLTYPE CComCat::GetCategoryDesc(REFCATID rcatid, LCID lcid, LPWSTR* pszDesc)
{
	HRESULT 	hr 			= S_OK;
	WCHAR		lpSubKey[MAX_REG_KEY_LEN];
	WCHAR		lpCatid[MAX_GUID_STR_LEN];
	WCHAR		lpName[MAX_LOCALE_LEN];
	DWORD		cdwData 	= sizeof(pszDesc);
	DWORD		cdwCh		= 0;
	HKEY		hKey;

	// Check parameter
	if(pszDesc == NULL)
	{
		hr = E_INVALIDARG;
		goto exit;
	}

	// Convert CLSID to a string
	cdwCh = StringFromGUID2(rcatid, lpCatid, MAX_GUID_STR_LEN);
	if(cdwCh == 0)
	{
		hr = E_INVALIDARG;
		goto exit;
	}

	// Create the string representing the key
	wcscpy(lpSubKey, CATID_KEY_HEADER);
	wcscat(lpSubKey, L"\\");
	wcscat(lpSubKey, lpCatid);

	// Open the reg key
	hr = RegOpenKeyEx(HKEY_CLASSES_ROOT,
					lpSubKey,
					0,
					0,
					&hKey);
	if(hr == S_OK)
	{
		// The name for the reg key is the locale id
		wsprintf(lpName, TEXT("%X"), lcid);

		// Get the reg key data
		hr = RegQueryValueEx(hKey,
					lpName,
					NULL,
					NULL,
					(LPBYTE) *pszDesc,
					&cdwData);
		if(hr != S_OK)
		{
			// If we failed it is probably due to an invalid 'lcid'.
			hr = CAT_E_NODESCRIPTION;
		}

		RegCloseKey(hKey);
	}
	else
	{
		hr = CAT_E_CATIDNOEXIST;
	}
	
exit:
	return hr;

} // CComCat::GetCategoryDesc



/*++

Routine Name: 

	CComCat::EnumClassesOfCategories

Routine Description:

	Returns an enumerator over the classes that implement one or more of 
	rgcatidImpl. If a class requires a category not listed in rgcatidReq, 
	it is not included in the enumeration.

Arguments:

	cImplemented:	Number of category IDs in the rgcatidImpl array
	rgcatidImpl:	Array of category identifiers
	cRequired:		Number of category IDs in the rgcatidReq array
	rgcatidReq:		Array of category identifiers
	ppenumCatid:	Location in which to return an IEnumCLSID interface

Return Value:

	HRESULT to indicate return status

--*/

HRESULT CComCat::EnumClassesOfCategories(ULONG cImplemented, CATID rgcatidImpl[], ULONG cRequired, CATID rgcatidReq[], IEnumCLSID** ppenumCLSID)
{
	HRESULT 		hr 				= S_OK;
	ULONG			i;
	DWORD			cdwSubKeys 	= 0;
	HKEY			hClsidKey;
	CLSID*			pClsidList 		= NULL;
	DWORD			cdwClsid 		= 0;
	CEnumGUIDImpl*	pEnumGuid 		= NULL;

	// Check parameter
	if(ppenumCLSID == NULL)
	{
		hr = E_INVALIDARG;
		goto exit;
	}

	// Open CLSID key
	hr = RegOpenKeyEx(HKEY_CLASSES_ROOT,
					CLSID_KEY_HEADER,
					0,
					0,
					&hClsidKey);
	if(hr != S_OK)
	{
		hr = E_INVALIDARG;
		goto exit;
	}

	// Get the count of subkeys
	hr = GetNumSubKeys(hClsidKey, &cdwSubKeys);
	if(hr != S_OK)
	{
		RegCloseKey(hClsidKey);
		goto exit;
	}

	// Allocate memory (array needs to be MAX cdwSubKeys.. but probably much smaller)
	pClsidList = new CATID[cdwSubKeys];
	pEnumGuid = new CEnumGUIDImpl;
	if(pClsidList == NULL || pEnumGuid == NULL)
	{
		hr = E_OUTOFMEMORY;
		RegCloseKey(hClsidKey);

		// If pClsidList failed then we should delete pEnumGuid
		if(pEnumGuid)
		{
			delete pEnumGuid;
		}

		goto exit;
	}
	
	// For each CLSID
	for(i=0;i<cdwSubKeys;i++)
	{
		WCHAR	lpClsidStr[MAX_GUID_STR_LEN];
		DWORD	cdwNameBuf 	= MAX_GUID_STR_LEN;
		
		// Enumerate the ith key
		RegEnumKeyEx(hClsidKey,
					i,
					lpClsidStr,
					&cdwNameBuf,
					NULL,
					NULL,
					NULL,
					NULL);

		if ((NOERROR == CLSIDFromString(lpClsidStr, &pClsidList[cdwClsid])) &&
			(S_OK == IsClassOfCategories(pClsidList[cdwClsid], cImplemented, rgcatidImpl, cRequired, rgcatidReq)))
			cdwClsid++;
	}	

	RegCloseKey(hClsidKey);

	// Initialize the Enumeration object.  If there are no elements to put in the enumeration
	// then nothing has to be done since the constructor for this object automatically sets everything
	// to null.
	if(cdwClsid > 0)
	{
		pEnumGuid->Init(cdwClsid, pClsidList);
	}

	// Set the out parameter to point to the enumeration interface
	*ppenumCLSID= (IEnumCLSID *) pEnumGuid;

exit:
	
	if(pClsidList)
	{
		delete[] pClsidList;
	}
	return hr;
	
} // CComCat::EnumClassesOfCategories



/*++

Routine Name: 

	CComCat::IsClassOfCategories

Routine Description:

	Determines if a class implements one or more categories. If the class 
	requires a category not listed in rgcatidReq, it is not included in 
	the enumeration.

Arguments:

	rclsid:			Class ID of the class to query
	cImplemented:	Number of category IDs in the rgcatidImpl array
	rgcatidImpl:		Array of category identifiers
	cRequired:		Number of category IDs in the rgcatidReq array
	rgcatidReq:		Array of category identifiers

Return Value:

	HRESULT to indicate return status

--*/

HRESULT CComCat::IsClassOfCategories(REFCLSID rclsid, ULONG cImplemented, CATID rgcatidImpl[], ULONG cRequired, CATID rgcatidReq[])
{
	HRESULT 	hr 				= S_FALSE;
	WCHAR		lpSubKey[MAX_REG_KEY_LEN];
	WCHAR		lpGuidStr[MAX_GUID_STR_LEN];
	WCHAR*		lpImplSubKey 	= NULL;
	HKEY		hClsidKey;
	HKEY		hImplCatidKey;
	HKEY		hReqCatidKey;
	BOOL		fClsidMatch 		= FALSE;
	BOOL		fReqMissing 		= FALSE;
	DWORD		j, k;
	DWORD		cdwCh;

	// Convert rclsid to string
	cdwCh = StringFromGUID2(rclsid, lpGuidStr, MAX_GUID_STR_LEN);
	if(cdwCh == 0)
	{
		hr = E_INVALIDARG;
		goto exit;
	}

	// Create subkey string
	wcscpy(lpSubKey, CLSID_KEY_HEADER);
	wcscat(lpSubKey, L"\\");
	wcscat(lpSubKey, lpGuidStr);

	// Open CLSID key
	hr = RegOpenKeyEx(HKEY_CLASSES_ROOT,
					lpSubKey,
					0,
					0,
					&hClsidKey);
	if(hr != S_OK)
	{
		hr = E_INVALIDARG;
		goto exit;
	}

	// Initialize Implemented CATID key string
	wcscpy(lpSubKey, IMPL_CAT_KEY_HEADER);
	wcscat(lpSubKey, L"\\");
	lpImplSubKey = lpSubKey + wcslen(lpSubKey);

	// Loop through the implemented list checking for an implemented CATID
	for(j=0;j<cImplemented;j++)
	{
		// Convert CATID to a string
		cdwCh = StringFromGUID2(rgcatidImpl[j], lpImplSubKey, MAX_GUID_STR_LEN);
		if(cdwCh != 0)
		{
			// Attempt to open Impl Catid key.  If it fails then the CATID does not exist
			// and we can proceed to checking the other CATIDs in the list.
			hr = RegOpenKeyEx(hClsidKey,
							lpSubKey,
							0,
							0,
							&hImplCatidKey);
			if(hr == S_OK)
			{
				fClsidMatch = TRUE;
				RegCloseKey(hImplCatidKey);
				break;
			}
		}
	}

	// If the class contained on of the matching CATIDs then continue
	// to check the required catids constraint.
	if(fClsidMatch)
	{
		// Open required catids key
		hr = RegOpenKeyEx(hClsidKey,
						REQ_CAT_KEY_HEADER,
						0,
						0,
						&hReqCatidKey);
		// If succeeded then there are required catids
		if(hr == S_OK)
		{
			DWORD	cdwReqSubKeys = 0;

			// Get the count of subkeys
			hr = GetNumSubKeys(hReqCatidKey, &cdwReqSubKeys);
			if(hr == S_OK)
			{
				// Enum required catids
				for(k=0;k<cdwReqSubKeys;k++)
				{
					CATID	catid;
					DWORD	cdwNameBuf = MAX_GUID_STR_LEN;
					
					// Enumerate the ith key
					RegEnumKeyEx(hReqCatidKey,
								k,
								lpGuidStr,
								&cdwNameBuf,
								NULL,
								NULL,
								NULL,
								NULL);

					CLSIDFromString(lpGuidStr, &catid);
					
					// Verify the catid is in the list
					if(!IsCatidInList(catid, cRequired, rgcatidReq))
					{
						fReqMissing = TRUE;
						break;
					}
					
				}
			}
		}
		
		RegCloseKey(hReqCatidKey);

		if(fReqMissing == FALSE)
		{
			hr = S_OK;
		}
		else
		{
			hr = S_FALSE;
		}
	}
	else
	{
		hr = S_FALSE;
	}

	RegCloseKey(hClsidKey);

exit:
	return hr;

} // CComCat::IsClassOfCategories



/*++

Routine Name: 

	CComCat::EnumImplCategoriesOfClass

Routine Description:

	Returns an enumerator for the CATIDs implemented by the specified class.

Arguments:

	rclsid:			Class ID
	ppenumCatid:	Location in which to return an IEnumCATID interface

Return Value:

	HRESULT to indicate return status

--*/

HRESULT CComCat::EnumImplCategoriesOfClass(REFCLSID rclsid, IEnumCATID** ppenumCatid)
{
	return EnumXXXCategoriesOfClass(rclsid, ppenumCatid, IMPL_CAT_KEY_HEADER);
}

/*++

Routine Name: 

	CComCat::EnumReqCategoriesOfClass

Routine Description:

	Returns an enumerator for the CATIDs required by the specified class.

Arguments:

	rclsid:			Class ID
	ppenumCatid:	Location in which to return an IEnumCATID interface

Return Value:

	HRESULT to indicate return status

--*/

HRESULT CComCat::EnumReqCategoriesOfClass(REFCLSID rclsid, IEnumCATID** ppenumCatid)
{
	return EnumXXXCategoriesOfClass(rclsid, ppenumCatid, REQ_CAT_KEY_HEADER);
}

/*++

Routine Name: 

	CComCat::EnumXXXCategoriesOfClass

Routine Description:

	Returns an enumerator for the CATIDs of the specified class.

Arguments:

	rclsid:			Class ID
	ppenumCatid:		Location in which to return an IEnumCATID interface

Return Value:

	HRESULT to indicate return status

--*/

HRESULT CComCat::EnumXXXCategoriesOfClass(REFCLSID rclsid, IEnumCATID** ppenumCatid, WCHAR* lpSubKeyName)
{
	HRESULT 		hr 					= S_OK;
	HKEY			hClsidKey;
	HKEY			hImplKey;
	WCHAR			lpSubKey[MAX_REG_KEY_LEN];
	CATID*			pCatidList			= NULL;
	DWORD			i;
	DWORD			cdwImplSubKeys 	= 0;
	DWORD			cdwCh;
	CEnumGUIDImpl*	pEnumGuid			= NULL;
		

	// Create subkey string
	wcscpy(lpSubKey, CLSID_KEY_HEADER);
	wcscat(lpSubKey, L"\\");

	// Convert rclsid to string
	cdwCh = StringFromGUID2(rclsid, lpSubKey + wcslen (lpSubKey), MAX_GUID_STR_LEN);
	if(cdwCh == 0)
	{
		hr = E_INVALIDARG;
		goto exit;
	}

	// Open CLSID key
	hr = RegOpenKeyEx(HKEY_CLASSES_ROOT,
					lpSubKey,
					0,
					0,
					&hClsidKey);
	if(hr != S_OK)
	{
		hr = E_INVALIDARG;
		goto exit;
	}	

	// Allocate memory for Enumeration class
	pEnumGuid = new CEnumGUIDImpl;
	if(pEnumGuid == NULL)
	{
		hr = E_OUTOFMEMORY;
		RegCloseKey(hClsidKey);
		goto exit;
	}

	hr = RegOpenKeyEx(hClsidKey,
					lpSubKeyName,
					0,
					0,
					&hImplKey);

	RegCloseKey(hClsidKey);

	if(hr == S_OK)
	{
		// Get the count of subkeys
		hr = GetNumSubKeys(hImplKey, &cdwImplSubKeys);
		if(hr == S_OK)
		{
			pCatidList = new CATID[cdwImplSubKeys];
			if(pCatidList == NULL)
			{
				hr = E_OUTOFMEMORY;

				RegCloseKey(hImplKey);

				// If pCatidList failed then we should delete pEnumGuid
				delete pEnumGuid;

				goto exit;
			}
		
			// Enum required catids
			for(i=0;i<cdwImplSubKeys;i++)
			{
				DWORD	cdwNameBuf = MAX_GUID_STR_LEN;
				WCHAR	lpGuidStr[MAX_GUID_STR_LEN];
				
				// Enumerate the ith key
				RegEnumKeyEx(hImplKey,
							i,
							lpGuidStr,
							&cdwNameBuf,
							NULL,
							NULL,
							NULL,
							NULL);

				CLSIDFromString(lpGuidStr, &(pCatidList[i]));
			}
		}
	}
	else
	{
		// If we get to this point it was not really in error.  Just that there were no categories.
		hr = S_OK;
	}

	RegCloseKey(hImplKey);

	// Initialize the Enumeration object.  If there are no elements to put in the enumeration
	// then nothing has to be done since the constructor for this object automatically sets everything
	// to null.
	if(cdwImplSubKeys > 0)
	{
		pEnumGuid->Init(cdwImplSubKeys, pCatidList);
	}

	// Set the out parameter to point to the enumeration interface
	*ppenumCatid= (IEnumCATID *) pEnumGuid;

exit:

	if(pCatidList)
	{
		delete[] pCatidList;
	}
	
	return hr;

} // CComCat::EnumXXXCategoriesOfClass


/*++

Routine Name: 

	CComCat::IsCatidInList

Routine Description:

	Returns a boolean whether the specified CATID is in the specified list.

Arguments:

	rcatid:			Category ID to search for
	ulArraySize:		Size of the array to search through
	rgcatid:			Array to search through

Return Value:

	TRUE:	CATID is in the list
	FALSE:	CATID is not in the list

--*/

BOOL CComCat::IsCatidInList(REFCATID rcatid, ULONG ulArraySize, CATID rgcatid[])
{
	BOOL fCatidInList = FALSE;

	// Loop through the array looking for the specified CATID
	for(ULONG i=0;i<ulArraySize; i++)
	{
		if(!memcmp((PVOID) &rcatid, (PVOID) &rgcatid[i], sizeof(CATID)))
		{
			fCatidInList = TRUE;
			break;
		}
	}

	return fCatidInList;
	
} // CComCat::IsCatidInList



HRESULT CComCat::GetNumSubKeys(HKEY hKey, DWORD* cdwSubKeys)
{
	HRESULT	hr = S_OK;
	
	// Get the count of subkeys
	hr = RegQueryInfoKey(hKey,
					NULL,
					NULL,
					NULL,
					cdwSubKeys,
					NULL,
					NULL,
					NULL,
					NULL,
					NULL,
					NULL,
					NULL);
	if(hr != S_OK)
	{
		hr = E_INVALIDARG;
		goto exit;
	}

exit:
	return hr;
	
} // CComCat::GetNumSubKeys
