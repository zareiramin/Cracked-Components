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

Header file for Component Category Manager implementation


*/

#ifndef _CCM_H_
#define _CCM_H_

#include "comcat.h"
#include "objbase.h"

#define MAX_REG_KEY_LEN		256
#define CATID_KEY_HEADER		L"Component Categories"
#define CLSID_KEY_HEADER		L"CLSID"
#define IMPL_CAT_KEY_HEADER	L"Implemented Categories"
#define REQ_CAT_KEY_HEADER	L"Required Categories"
#define MAX_GUID_STR_LEN		60
#define MAX_LOCALE_LEN			12


extern LONG 			g_cServerLocks;
extern LONG 			g_cComponents;
extern HINSTANCE		g_hInstance;

class CFactory : public IClassFactory
{
public:
	// IUnknown
	ULONG __stdcall AddRef();
	ULONG __stdcall Release();
	HRESULT __stdcall QueryInterface(REFIID riid, void** ppv);

	// IClassFactory
	HRESULT __stdcall CreateInstance(IUnknown *pUnknownOuter, REFIID riid, void** ppv);
	HRESULT __stdcall LockServer(BOOL bLock);

	CFactory() : m_cRef(1) { }
	~CFactory() { }

private:
	LONG m_cRef;
};


class CEnumCatInfoImpl : public IEnumCATEGORYINFO
{
public:
	CEnumCatInfoImpl(void) : m_cRef(1), m_dwCurrElem(0), m_pCATList(NULL), m_cdwCATList(0) { }
	~CEnumCatInfoImpl(void);

	HRESULT Init(DWORD cdwList, CATEGORYINFO* rgelt);

	// Methods of IEnumCATEGORYINFO interface
	HRESULT STDMETHODCALLTYPE QueryInterface(REFIID iid, void** ppvObject);
	ULONG STDMETHODCALLTYPE AddRef(void);
	ULONG STDMETHODCALLTYPE Release(void);
	HRESULT STDMETHODCALLTYPE Next(ULONG celt, CATEGORYINFO* rgelt, ULONG* pceltFetched);
	HRESULT STDMETHODCALLTYPE Clone(IEnumCATEGORYINFO **penum);
	HRESULT STDMETHODCALLTYPE Reset(void);
	HRESULT STDMETHODCALLTYPE Skip(ULONG celt);

private:
	LONG 			m_cRef;
	CATEGORYINFO*	m_pCATList;
	DWORD			m_cdwCATList;
	DWORD			m_dwCurrElem;
};


class CEnumGUIDImpl : public IEnumGUID
{
public:
	CEnumGUIDImpl(void) : m_cRef(1), m_dwCurrElem(0), m_pGuidList(NULL), m_cdwGuidList(0) { }
	~CEnumGUIDImpl(void);

	HRESULT Init(DWORD cdwList, GUID* rgelt);

	// Methods of IEnumGUID interface
	HRESULT STDMETHODCALLTYPE QueryInterface(REFIID iid, void** ppvObject);
	ULONG STDMETHODCALLTYPE AddRef(void);
	ULONG STDMETHODCALLTYPE Release(void);
	HRESULT STDMETHODCALLTYPE Next(ULONG celt, GUID* rgelt, ULONG* pceltFetched);
	HRESULT STDMETHODCALLTYPE Clone(IEnumGUID **penum);
	HRESULT STDMETHODCALLTYPE Reset(void);
	HRESULT STDMETHODCALLTYPE Skip(ULONG celt);

private:
	LONG 	m_cRef;
	GUID*	m_pGuidList;
	DWORD	m_cdwGuidList;
	DWORD	m_dwCurrElem;
};


class CComCat : public ICatRegister, public ICatInformation
{
public:
	CComCat(void) : m_cRef(1) { }
	~CComCat(void) { }

	// IUnknown Methods
	HRESULT __stdcall QueryInterface(REFIID iid, void** ppvObject);
	ULONG __stdcall AddRef(void);
	ULONG __stdcall Release(void);

	// Methods of ICatRegister interface
	virtual HRESULT STDMETHODCALLTYPE RegisterCategories(ULONG cCategories, CATEGORYINFO rgCategoryInfo[]);
	virtual HRESULT STDMETHODCALLTYPE UnRegisterCategories(ULONG cCategories, CATID rgcatid[]);
	virtual HRESULT STDMETHODCALLTYPE RegisterClassImplCategories(REFCLSID rclsid, ULONG cCategories, CATID rgcatid[]);
	virtual HRESULT STDMETHODCALLTYPE UnRegisterClassImplCategories(REFCLSID rclsid, ULONG cCategories, CATID rgcatid[]);
	virtual HRESULT STDMETHODCALLTYPE RegisterClassReqCategories(REFCLSID rclsid, ULONG cCategories, CATID rgcatid[]);
	virtual HRESULT STDMETHODCALLTYPE UnRegisterClassReqCategories(REFCLSID rclsid, ULONG cCategories, CATID rgcatid[]);

	// Methods of ICatInformation interface
	virtual HRESULT STDMETHODCALLTYPE EnumCategories(LCID lcid, IEnumCATEGORYINFO** ppenumCategoryInfo);
	virtual HRESULT STDMETHODCALLTYPE GetCategoryDesc(REFCATID rcatid, LCID lcid, LPWSTR *pszDesc);
	virtual HRESULT STDMETHODCALLTYPE EnumClassesOfCategories(ULONG cImplemented, CATID rgcatidImpl[], ULONG cRequired, CATID rgcatidReq[], IEnumCLSID** ppenumCLSID);
	virtual HRESULT STDMETHODCALLTYPE IsClassOfCategories(REFCLSID rclsid, ULONG cImplemented, CATID rgcatidImpl[], ULONG cRequired, CATID rgcatidReq[]);
	virtual HRESULT STDMETHODCALLTYPE EnumImplCategoriesOfClass(REFCLSID rclsid, IEnumCATID** ppenumCatid);
	virtual HRESULT STDMETHODCALLTYPE EnumReqCategoriesOfClass(REFCLSID rclsid, IEnumCATID** ppenumCatid);

	// Methods not part of any interface
	HRESULT EnumXXXCategoriesOfClass (REFCLSID rclsid, IEnumCATID** ppenumCatid, WCHAR* lpSubKeyName);
	BOOL IsCatidInList(REFCATID rcatid, ULONG ulArraySize, CATID rgcatid[]);
	HRESULT GetNumSubKeys(HKEY hKey, DWORD *cdwSubKeys);

private:
	LONG m_cRef;
};





#endif

