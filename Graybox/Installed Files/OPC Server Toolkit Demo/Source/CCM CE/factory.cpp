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


Module Name:

	factory.cpp

Abstract:

	This file contains the implementation for the Component Categories Manager class factory.

Environment:

	Windows CE

--*/


#include "ccm.h"



ULONG CFactory::AddRef()
{
	return InterlockedIncrement(&m_cRef);
}



ULONG CFactory::Release()
{
	unsigned cRef = InterlockedDecrement(&m_cRef);
	if(cRef != 0)
		return cRef;
	delete this;
	return 0;
}



HRESULT CFactory::QueryInterface(REFIID riid, void** ppv)
{
	if(riid == IID_IUnknown || riid == IID_IClassFactory)
	{
		*ppv = (IClassFactory *)this;
	}
	else
	{
		*ppv = NULL;
		return E_NOINTERFACE;
	}
	AddRef();
	return S_OK;
}



HRESULT CFactory::CreateInstance(IUnknown *pUnknownOuter, REFIID riid, void** ppv)
{
	if(pUnknownOuter != NULL)
		return CLASS_E_NOAGGREGATION;

	CComCat *pComCat = new CComCat;

	if(pComCat == NULL)
		return E_OUTOFMEMORY;

	// QueryInterface probably for IID_IUnknown
	HRESULT hr = pComCat->QueryInterface(riid, ppv);
	pComCat->Release();
	return hr;
}



HRESULT CFactory::LockServer(BOOL bLock)
{
	if(bLock)
		g_cServerLocks++;
	else
		g_cServerLocks--;
	
	return S_OK;
}

