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

	EnumGuid.cpp

Abstract:

	This file contains the implementation for the IEnumGUID interface.

Environment:

	Windows CE

*/


#include "ccm.h"



/*++

Routine Name: 

	CEnumGUIDImpl::Init

Routine Description:

	Initialize the enumeration with data.

Arguments:

	cdwList:	Number of elements
	rgelt:		Array containing the elements

Return Value:

	HRESULT to indicate return status

--*/

HRESULT CEnumGUIDImpl::Init(DWORD cdwList, GUID* rgelt)
{
	HRESULT	hr = S_OK;
	DWORD 	i;
	
	m_cdwGuidList = cdwList;

	// Allocate memory for the GUID array
	m_pGuidList = new GUID[cdwList];
	if(m_pGuidList == NULL)
	{
		hr = E_OUTOFMEMORY;
		goto exit;
	}

	// Fill the GUID array with data
	for(i=0;i<cdwList;i++)
	{
		m_pGuidList[i] = rgelt[i];
	}

exit:
	return hr;
	
} // CEnumGUIDImpl::Init



/*++

Routine Name: 

	CEnumGUIDImpl::~CEnumGUIDImpl (destructor)

Routine Description:

	Class destructor

--*/

CEnumGUIDImpl::~CEnumGUIDImpl(void)
{
	if(m_pGuidList)
	{
		delete[] m_pGuidList;
	}
}



/*++

Routine Name: 

	CEnumGUIDImpl::QueryInterface

Routine Description:

	Query interface method from IUnknown interface.

Arguments:

	riid:	Interface Id
	ppv:	Object to be queried

Return Value:

	HRESULT to indicate return status

--*/

HRESULT CEnumGUIDImpl::QueryInterface(REFIID riid, void** ppv)
{
	HRESULT hr = S_OK;

	if(riid == IID_IUnknown)
	{
		*ppv = (IUnknown*) this;
	}
	else if(riid == IID_IEnumGUID)
	{
		*ppv = (IEnumGUID*) this;
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
	
} // CEnumGUIDImpl::QueryInterface



/*++

Routine Name: 

	CEnumGUIDImpl::AddRef

Routine Description:

	Increment the reference count.

Return Value:

	ULONG to indicate return status

--*/

ULONG CEnumGUIDImpl::AddRef(void)
{
	return InterlockedIncrement(&m_cRef);
}



/*++

Routine Name: 

	CEnumGUIDImpl::Release

Routine Description:

	Decrement the reference count and delete the current object if
	the ref count is zero.

Return Value:

	ULONG to indicate return status

--*/

ULONG CEnumGUIDImpl::Release(void)
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

	CEnumGUIDImpl::Next

Routine Description:

	Obtain the next 'celt' elements in the enumeration.

Arguments:

	celt:			Number of elements to get
	rgelt:			Array out parameter to store elements
	pceltFetched:	Number of elements actally fetched.

Return Value:

	S_OK:		If succeeded
	S_FALSE:	If the demanded number of elements were not available

--*/

HRESULT CEnumGUIDImpl::Next(ULONG celt, GUID* rgelt, ULONG* pceltFetched)
{
	HRESULT hr = S_OK;
	DWORD	dwMaxFetcheable = m_cdwGuidList - m_dwCurrElem;
	DWORD	dwFetched;
	DWORD 	i;

	if(celt <= 0)
	{
		hr = E_INVALIDARG;
		goto exit;
	}

	// Determine how many elements will be fetched
	if(dwMaxFetcheable >= celt)
	{
		dwFetched = celt;
	}
	else
	{
		dwFetched = dwMaxFetcheable;
		hr = S_FALSE;
	}

	// Copy the category info for the number of items to fetch
	for(i=0;i<dwFetched;i++)
	{
		memcpy((LPVOID)&rgelt[i], &m_pGuidList[i+m_dwCurrElem], sizeof(GUID));
	}

	m_dwCurrElem += dwFetched;

	// Set the out parameter if it is not null
	if(pceltFetched != NULL)
	{
		*pceltFetched = dwFetched;
	}

exit:
	return hr;

} // CEnumGUIDImpl::Next



/*++

Routine Name: 

	CEnumGUIDImpl::Clone

Routine Description:

	Clone the enumeration and return it.

Arguments:

	penum: Out parameter pointing to the new enumeration

Return Value:

	HRESULT to indicate return status.

--*/

HRESULT CEnumGUIDImpl::Clone(IEnumGUID **penum)
{
	HRESULT 			hr 			= S_OK;
	CEnumGUIDImpl*		pGuidEnum	= NULL;

	// Allocate memory for the new enumeration
	pGuidEnum = new CEnumGUIDImpl;
	if(pGuidEnum == NULL)
	{
		hr = E_OUTOFMEMORY;
		goto exit;
	}

	// Initialize the new object to the state of the current object
	hr = pGuidEnum->Init(this->m_cdwGuidList, this->m_pGuidList);
	if(hr != S_OK)
	{
		goto exit;
	}

	// Ignore return value because this should always work
	// since a failure would indicate a problem with the current object
	pGuidEnum->Skip(this->m_dwCurrElem);

	// Set the out parameter to point to the enumeration interface
	*penum = (IEnumGUID *) pGuidEnum;
	
exit:
	return hr;
	
} // CEnumGUIDImpl::Clone



/*++

Routine Name: 

	CEnumGUIDImpl::Reset

Routine Description:

	Reset the pointer to the current element in the enumeration to the start.

Return Value:

	HRESULT to indicate return status

--*/

HRESULT CEnumGUIDImpl::Reset(void)
{
	HRESULT hr = S_OK;

	m_dwCurrElem = 0;

	return hr;
	
} // CEnumGUIDImpl::Reset



/*++

Routine Name: 

	CEnumGUIDImpl::Skip

Routine Description:

	Skip 'celt' number of elements in the enumeration.

Arguments:

	celt:	Number of elements to skip in the enumeration

Return Value:

	S_OK:		Skip successful
	S_FALSE:	Could not skip 'celt' elements

--*/

HRESULT CEnumGUIDImpl::Skip(ULONG celt)
{
	HRESULT hr = S_OK;

	// Verify that it is possible to skip 'celt' elements and if not return S_FALSE
	if((m_cdwGuidList - m_dwCurrElem) < celt)
	{
		hr = S_FALSE;
	}
	else
	{
		m_dwCurrElem += celt;
	}

	return hr;
	
} // CEnumGUIDImpl::Skip


