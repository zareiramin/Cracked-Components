// OPCServerList.cpp : Implementation of COPCServerList
#include "stdafx.h"
#include "Opcenum.h"
#include "OPCServerList.h"

/////////////////////////////////////////////////////////////////////////////
// COPCServerList

static const GUID CATID_OPCDAServer10 = 
{ 0x63D5F430, 0xCFE4, 0x11d1, { 0xB2, 0xC8, 0x00, 0x60, 0x08, 0x3B, 0xA1, 0xFB } };

// AddOldstyleServers
void COPCServerList::AddOldstyleServers(std::vector<GUID>* guids)
{
	USES_CONVERSION;

	_TCHAR szKey[200];
	DWORD dwKeyClassLen;
	_TCHAR szKeyClass[200];
	for (DWORD dwIndex = 0 ;
		dwKeyClassLen = 200, RegEnumKeyEx(HKEY_CLASSES_ROOT, dwIndex, szKey, &dwKeyClassLen, NULL, szKeyClass, &dwKeyClassLen, NULL) == ERROR_SUCCESS ;
		++dwIndex )	{

		// Open each subkey (the ProgIDs)
		HKEY hProgID;
		_TCHAR szDummy[2048];

		if (RegOpenKeyEx( HKEY_CLASSES_ROOT, szKey, 0, KEY_READ, &hProgID) == ERROR_SUCCESS) {

			// and for that subkey/ProgID - try to read the OPC Subkey
			unsigned long lVsize = 2048;
			if(RegQueryValueEx( hProgID, _T("OPC"), NULL, NULL, (LPBYTE)szDummy, &lVsize) == ERROR_SUCCESS) {

				// Check to see if this is already in the list.
				CLSID clsid;
				CLSIDFromString( T2OLE(szKey), &clsid);

				unsigned int i = 0;

				for (i = 0; i < guids->size(); i++) {

					if ( IsEqualGUID((*guids)[i], clsid) )
						break;
				}

				// If we did not find the GUID in the list, i will equal
				// guids->size().
				if (i == guids->size())
					guids->push_back(clsid);
			}

			RegCloseKey( hProgID ) ;
		}
		// Enum the next one...
	}
}

// VersionIndependentProgIDFromCLSID
HRESULT COPCServerList::VersionIndependentProgIDFromCLSID(REFCLSID clsid, LPOLESTR * lplpszProgID)
{
	USES_CONVERSION;

	LPOLESTR id;
	StringFromCLSID (clsid, &id);

	_TCHAR verId[2048] = _T("CLSID\\");
	_tcscat( verId, W2T(id) );

	CoTaskMemFree (id);

	_tcscat( verId, _T("\\VersionIndependentProgID") );

	CRegKey key;
	if (key.Open (HKEY_CLASSES_ROOT, verId, KEY_READ) != ERROR_SUCCESS)
		return REGDB_E_CLASSNOTREG;

	_TCHAR buf[1024];
	DWORD bufSize = sizeof(buf);
	key.QueryValue(buf, NULL, &bufSize);//  .QueryStringValue(NULL, buf, &bufSize);

	if (bufSize > 0) {
		// bufSize contains the size of the data string including the NULL
		// terminator. Multiply by two if we are not compiled for UNICODE.
#ifndef _UNICODE
		bufSize *= 2;
#endif
		*lplpszProgID = (LPOLESTR)CoTaskMemAlloc (bufSize);
		CopyMemory (*lplpszProgID, T2W (buf), bufSize);
		return S_OK;
	}
	else
		return E_FAIL;
}



// IOPCServerList
STDMETHODIMP COPCServerList::EnumClassesOfCategories(ULONG cImplemented, GUID rgcatidImpl[], ULONG cRequired, GUID rgcatidReq[], IEnumGUID ** ppenumClsid)
{
	ICatInformation* pCatInfo;
	HRESULT hr = CoCreateInstance(CLSID_StdComponentCategoriesMgr, NULL, CLSCTX_ALL, __uuidof(ICatInformation), (void**)&pCatInfo);
	if (FAILED(hr))
		return hr;

	if (!cImplemented)
	{
		cImplemented = -1;
		rgcatidImpl = NULL;
	}
	if (!cRequired)
	{
		cRequired = -1;
		rgcatidReq = NULL;
	}
	hr = pCatInfo->EnumClassesOfCategories(cImplemented, rgcatidImpl, 
		cRequired, rgcatidReq, ppenumClsid);
	return hr;
}

STDMETHODIMP COPCServerList::GetClassDetails(REFCLSID clsid, LPWSTR * ppszProgID, LPWSTR * ppszUserType)
{
	USES_CONVERSION;

	LPOLESTR id;
	StringFromCLSID (clsid, &id);

	_TCHAR sz_key[2048] = _T("CLSID\\");
	_tcscat( sz_key, W2T(id) );

	CoTaskMemFree (id);

	CRegKey key;
	if (key.Open (HKEY_CLASSES_ROOT, sz_key, KEY_READ) != ERROR_SUCCESS)
		return REGDB_E_CLASSNOTREG;

	_TCHAR progid_buf[1024];
	_TCHAR user_buf[1024];
	DWORD progid_buf_size = sizeof(progid_buf);
	DWORD user_buf_size = sizeof(user_buf);

	if ( key.QueryValue(user_buf, NULL, &user_buf_size) != ERROR_SUCCESS ) return E_FAIL;

	_tcscat( sz_key, _T("\\VersionIndependentProgID") );
	if (key.Open(HKEY_CLASSES_ROOT, sz_key, KEY_READ) != ERROR_SUCCESS) return REGDB_E_CLASSNOTREG;
	if (key.QueryValue(progid_buf, NULL, &progid_buf_size) != ERROR_SUCCESS) return E_FAIL;

	if (user_buf_size > 0)
	{
#ifndef _UNICODE
		user_buf_size *= 2;
#endif
		*ppszUserType = (LPOLESTR)CoTaskMemAlloc (user_buf_size);
		CopyMemory (*ppszUserType, T2W (user_buf), user_buf_size);
	}
	else return E_FAIL;

	if (progid_buf_size > 0)
	{
#ifndef _UNICODE
		progid_buf_size *= 2;
#endif
		*ppszProgID = (LPOLESTR)CoTaskMemAlloc (progid_buf_size);
		CopyMemory (*ppszProgID, T2W (progid_buf), progid_buf_size);
	}
	else
	{
		CoTaskMemFree(*ppszUserType);
		*ppszUserType = NULL;
		return E_FAIL;
	}
	return S_OK;
}

STDMETHODIMP COPCServerList::CLSIDFromProgID(LPCOLESTR szProgId, LPCLSID clsid)
{
	return ::CLSIDFromProgID(szProgId, clsid);
}

// IOPCServerList2
STDMETHODIMP COPCServerList::EnumClassesOfCategories(ULONG cImplemented, GUID rgcatidImpl[], ULONG cRequired, GUID rgcatidReq[], IOPCEnumGUID ** ppenumClsid)
{
	*ppenumClsid = 0;

	// Get the enum object from comcat.
	CComPtr<IEnumGUID> enumGuid;

	HRESULT hr = EnumClassesOfCategories(cImplemented, rgcatidImpl,
		cRequired, rgcatidReq, &enumGuid);

	if (FAILED(hr))
		return hr;

	std::vector<GUID>* guids = new std::vector<GUID>;

	// Add all of the returned GUIDs to the vector.
	unsigned long count;	// Count
	GUID guid;
	while ((hr = enumGuid->Next(1, &guid, &count)) == S_OK)
	{
		guids->push_back(guid);
	}

	if (FAILED(hr)) {
		delete guids;
		return hr;
	}

	// Do we need to enumerate the old-style DA 1.0 servers?
	if (cImplemented != -1) {
		for (ULONG i = 0; i < cImplemented; i++) {
			if ( IsEqualGUID(rgcatidImpl[i], CATID_OPCDAServer10) ) {
				AddOldstyleServers(guids);
				break;
			}
		}
	}

	EnumGUID* newEnum = new EnumGUID;

	if (guids->size() > 0)
	{
		newEnum->Init(&(guids->front()), &(guids->back())+1, NULL, AtlFlagTakeOwnership);
	}
	else
	{
		newEnum->Init(NULL, NULL, NULL, AtlFlagTakeOwnership);
	}

	//
	//newEnum->Init(guids->begin(), guids->end(), NULL, AtlFlagTakeOwnership);
    //

	hr = newEnum->QueryInterface(ppenumClsid);
	if (FAILED(hr))
		delete newEnum;

	return hr;
}

STDMETHODIMP COPCServerList::GetClassDetails(REFCLSID clsid, LPWSTR * ppszProgID, LPWSTR * ppszUserType, LPWSTR * ppszVerIndProgID)
{
	*ppszVerIndProgID = 0;

	// Get everything but the version independent prog ID.
	HRESULT hr = GetClassDetails(clsid, ppszProgID, ppszUserType);

	if (FAILED(hr))
		return hr;

	// Get the version independent prog ID.
	hr = VersionIndependentProgIDFromCLSID(clsid, ppszVerIndProgID);

	// If we could not get the version independent prog ID then just copy
	// the stanadard prog ID.
	if (FAILED(hr)) {

		// Allocate enough space for the unicode string with
		// the NULL terminator.
		long size = (long)wcslen(*ppszProgID)*2 + 2;
		*ppszVerIndProgID = (LPOLESTR)CoTaskMemAlloc(size);
		CopyMemory (*ppszVerIndProgID, *ppszProgID, size);
	}

	return S_OK;
}

