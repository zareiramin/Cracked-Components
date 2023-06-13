//////////////////////////////////////////////////////////
//
//  This file is a part of Graybox OPC Server Toolkit.
//  Do not redistribute.
//  Copyright (C) 2002-2012 Graybox Software
//
//////////////////////////////////////////////////////////

#ifndef _GB_OPCDA
#define _GB_OPCDA

#pragma once

#include <windows.h>
#include <unknwn.h>
#include <objbase.h>
#include <oaidl.h>

#include "opcda.h"
#include "opccomn.h"
#include "opcerror.h"

#define GBAPI __declspec(dllimport)

#ifdef _WIN32_WCE
#define GBCALL __cdecl
#else
#define GBCALL __stdcall
#endif

#pragma pack(push, 8)

typedef struct GBItemID
{
	DWORD dwTagID;
	DWORD dwUserID;
	DWORD dwAccessPathID;
} GBItemID;

#define	GB_RET_NOP			(0x00000000L)
#define	GB_RET_CACHE		(0x00000001L)
#define	GB_RET_ARG			(0x00000002L)

#define GB_SRV_DONTFINDCHLD	(0x00000001L)
#define GB_SRV_CHILDLESS	(0x00000002L)
#define GB_SRV_NOACCESSPATH	(0x00000008L)
#define GB_SRV_FREEERRSTR	(0x00001000L)		// 08.03.2008
#define GB_SRV_RETANYEUINFO	(0x00002000L)		// 04.12.2008
#define GB_SRV_GRPCHECKLCID	(0x00004000L)		// 21.01.2011
#define GB_SRV_USEOVRDQUAL	(0x01000000L)		// 06.03.2012
#define GB_SRV_ALWAYSDEVICE	(0x02000000L)		// 06.03.2012
#define GB_SRV_DATANOTIFY	(0x04000000L)		// 22.03.2012

#define GB_TAG_NOVALCMP		(0x00000200L)
#define GB_TAG_CANONONLY	(0x00000100L)
#define GB_TAG_DONTCOPYSTR	(0x00000001L)

#define GB_PROP_DONTCOPYSTR	(0x00000001L)

typedef unsigned __int64	GRAYSTUB8;
typedef unsigned int		GRAYSTUB4;
typedef unsigned short		GRAYSTUB2;
typedef unsigned char		GRAYSTUB1;

class GBClassFactory : public IClassFactory
{
public:
	GBAPI STDMETHODIMP_(ULONG) AddRef(void);
	GBAPI STDMETHODIMP_(ULONG) Release(void);
	GBAPI STDMETHODIMP QueryInterface(REFIID iid, LPVOID *ppInterface);
	virtual STDMETHODIMP LockServer(BOOL fLock) = 0;
	virtual STDMETHODIMP CreateInstance(LPUNKNOWN pUnkOuter, REFIID riid, LPVOID *ppvObject) = 0;
	virtual void GBCALL GBClassObjectAdd(DWORD dwClientId) = 0;
	virtual void GBCALL GBClassObjectRemove(DWORD dwClientId) = 0;
protected:
	CLSID m_GBClassId;
	LONG m_GBClassObjectUsed;
	LONG m_GBInstanceCount;
	LONG m_GBLockCount;
	CRITICAL_SECTION m_critGBClassFactory;
};

class GBOPCDataAccessBase : public GBClassFactory
{
public:
	// toolbox user API
	GBAPI GBOPCDataAccessBase();
	GBAPI ~GBOPCDataAccessBase();
	GBAPI HRESULT GBCALL GBGetItems(
		DWORD dwCount,
		DWORD* pdwTagIDs,
		VARIANT* pvValues,
		WORD* pwQualities,
		FILETIME* pftTimeStamps,
		HRESULT* pdwErrors);
	GBAPI HRESULT GBCALL GBUpdateItems(
		DWORD dwCount,
		DWORD* pdwItemID,
		VARIANT* pvValues,
		WORD* pwQualites,
		FILETIME* pftTimestamps,
		HRESULT* pdwErrors,
		BOOL bWait);
	GBAPI HRESULT GBCALL GBBeginUpdate();
	GBAPI HRESULT GBCALL GBSetItem(
		DWORD dwTagID,
		VARIANT* pvValue,
		WORD wQuality,
		FILETIME* pftTimestamp,
		HRESULT dwError);
	GBAPI HRESULT GBCALL GBEndUpdate(BOOL bWait);
	GBAPI HRESULT GBCALL GBCreateItem(
		DWORD* pdwItemID,
		DWORD dwUserID,
		LPCWSTR szOpcItemID,	// string or resource id
		DWORD dwAccessRights,
		DWORD dwFlags,
		VARIANT* pvValue,
		OPCEUTYPE dwEUType = OPC_NOENUM,
		VARIANT* pvEUInfo = NULL);
	GBAPI HRESULT GBCALL GBCreateItemAnalog(
		DWORD* pdwItemID,
		DWORD dwUserID,
		LPCWSTR szOpcItemID,	// string or resource id
		DWORD dwAccessRights,
		DWORD dwFlags,
		VARIANT* pvValue,
		double dLowRange,
		double dHighRange);
	GBAPI HRESULT GBCALL GBCreateItemEnum(
		DWORD* pdwTagIDs,
		DWORD dwUserID,
		LPCWSTR szOpcItemID,	// string or resource id
		DWORD dwAccessRights,
		DWORD dwFlags,
		VARIANT* pvValue,
		DWORD dwEnumCount,
		LPCWSTR* pszEnumStrings);
	GBAPI HRESULT GBCALL GBAddProperty(
		DWORD dwTagID,
		DWORD dwPropID,
		VARIANT* pvValue,
		LPCWSTR szDescription,		// string or resource id, if NULL or empty common OPC prop def will be used
		LPCWSTR szOpcItemID,		// string or resource id, if NULL or empty common OPC prop def will be used
		DWORD dwFlags);
	GBAPI HRESULT GBCALL GBSetProperty(
		DWORD dwTagID,
		DWORD dwPropID,
		VARIANT* pvValue);
	GBAPI HRESULT GBCALL GBGetProperty(
		DWORD dwTagID,
		DWORD dwPropID,
		VARIANT* pvValue);
	GBAPI HRESULT GBCALL GBRemoveProperty(
		DWORD dwTagID,
		DWORD dwPropID);
	GBAPI HRESULT GBCALL GBGetBandwidth(DWORD* pdwBandwidth);
	GBAPI HRESULT GBCALL GBSetState(OPCSERVERSTATE dwServerState);
	GBAPI HRESULT GBCALL GBSuspend();
	GBAPI HRESULT GBCALL GBResume();
	GBAPI HRESULT GBCALL GBShutdown(LPCWSTR szReason = NULL);
	GBAPI HRESULT GBCALL GBDisconnectClients();
	GBAPI VARTYPE GBCALL GBGetItemCanonicalType(DWORD dwTagId);		//08.03.2008
	GBAPI VARIANT* GBCALL GBGetItemDefaultValue(DWORD dwTagId);		// 08.03.2008
	GBAPI DWORD GBCALL GBGetItemEUType(DWORD dwTagId);		// 08.03.2008
	GBAPI VARIANT* GBCALL GBGetItemEUInfo(DWORD dwTagId);		// 08.03.2008


protected:
	// GBClassFactory implementation
	GBAPI void GBCALL GBClassObjectAdd(DWORD dwClientId);
	GBAPI void GBCALL GBClassObjectRemove(DWORD dwClientId);
	GBAPI STDMETHODIMP LockServer(BOOL fLock);
	GBAPI STDMETHODIMP CreateInstance(LPUNKNOWN pUnkOuter, REFIID riid, LPVOID *ppvObject);

	// class factory callbacks
	GBAPI virtual HRESULT GBCALL GBOnBeforeCreateInstance(BOOL bAggregating, DWORD* pdwClientId);  // return error code to restrict object creation
	GBAPI virtual void GBCALL GBOnCreateInstance(DWORD dwClientId);
	GBAPI virtual void GBCALL GBOnDestroyInstance(DWORD dwClientId);		
	GBAPI virtual void GBCALL GBOnLock();
	GBAPI virtual void GBCALL GBOnUnLock();
	GBAPI virtual HRESULT GBCALL GBOnServerReleased(); // if returns S_FALSE - class object suspends

protected:
	// internal-to-user callbacks
	GBAPI virtual DWORD GBCALL GBOnWriteItems(
		DWORD dwCount,
		GBItemID* pTags,
		VARIANT* pvValues,
		WORD* pwQualities,
		FILETIME* pftTimestamps,
		HRESULT* pErrors,
		HRESULT* pMasterError,
		LCID dwLcid,
		DWORD dwClientID);
   GBAPI virtual DWORD GBCALL GBOnReadItems(
		DWORD dwCount,
		GBItemID* pTags,
		VARIANT* pvValues,
		WORD* pwQualities,
		FILETIME* pTimestamps,
		HRESULT* pErrors,
		HRESULT* pMasterError,
		HRESULT* pMasterQuality,
		const VARTYPE* pRequestedTypes,
		LCID dwLcid,
		DWORD dwClientID);
   GBAPI virtual void GBCALL GBOnDataUpdate(
		DWORD dwCount,
		GBItemID* pTags,
		VARIANT* pValues,
		WORD* pwQualities,
		FILETIME* pftTimestamps,
		HRESULT* pErrors,
		HRESULT dwClientResult,
		DWORD dwClientID);
   GBAPI virtual void GBCALL GBOnActivate(
		DWORD dwCount,
		GBItemID* pTags);
    GBAPI virtual void GBCALL GBOnDeactivate(
		DWORD dwCount,
		GBItemID* pTags);
	GBAPI virtual HRESULT GBCALL GBOnGetErrorString(
		HRESULT dwError,
		LCID dwLcid,
		LPWSTR* pszErrorString,
		DWORD dwCallerID);
	GBAPI virtual HRESULT GBCALL GBOnQueryLocales(
		DWORD* pdwCount,
		LCID** ppdwLcid);
	GBAPI virtual HRESULT GBCALL GBOnBrowseAccessPath(
		LPCWSTR szItemID,
		DWORD* pdwAccessPathCount,
		LPWSTR** ppszAccessPaths,
		DWORD dwClientID);
	GBAPI virtual HRESULT GBCALL GBOnQueryItem(
		LPCWSTR szItemID,
		LPCWSTR szAccessPath,
		VARTYPE wDataType,
		BOOL bAddItem,
		DWORD* pdwTagID,
		DWORD* pdwAccessPathID,
		DWORD dwClientID);
	GBAPI virtual HRESULT GBCALL GBOnGetProperties(
		GBItemID* pTag,
		DWORD dwCount,
		DWORD *pdwPropIDs,
		VARIANT *pvValues,
		HRESULT *pdwErrors,
		LCID dwLcid,
		DWORD dwClientID);

protected:
	void* m_void1;
	void* m_void2;
	wchar_t m_GBVendorName[255];
	DWORD m_GBVersionMajor;
	DWORD m_GBVersionMinor;
	DWORD m_GBVersionBuild;
};

class GBDataAccess : public GBOPCDataAccessBase
{
public:
	GBAPI GBDataAccess();
	GBAPI ~GBDataAccess();
	GBAPI HRESULT GBCALL GBInitialize(
		CLSID* pClassId,
		DWORD dwTimeBase,
		DWORD dwMinUpdateRate,
		DWORD dwFlags,
		DWORD dwTagMax,
		DWORD dwVersionMajor = 1,
		DWORD dwVersionMinor = 0,
		DWORD dwVersionBuild = 0,
		LPCWSTR szVendorName = NULL);
	GBAPI HRESULT GBCALL GBInitialize(
		CLSID* pClassId,
		DWORD dwTimeBase,
		DWORD dwMinUpdateRate,
		DWORD dwFlags,
		WCHAR cSeparator,
		DWORD dwTagMax,
		DWORD dwVersionMajor = 1,
		DWORD dwVersionMinor = 0,
		DWORD dwVersionBuild = 0,
		LPCWSTR szVendorName = NULL);
	GBAPI HRESULT GBCALL GBRegisterClassObject();
	GBAPI HRESULT GBCALL GBRevokeClassObject();
	static GBAPI HRESULT GBCALL GBRegisterServer(
		CLSID* pClassID,
		LPCWSTR szVendorName,
		LPCWSTR szDescription,
		LPCWSTR szVersionIndipProgID,
		LPCWSTR szCurVersion,
		LPCWSTR szServiceName = NULL);
	static GBAPI HRESULT GBCALL GBUnregisterServer(CLSID* pClassID);

private:
	DWORD m_GBObjectId;
};

class GBDataAccessInproc : public GBOPCDataAccessBase
{
public:
	GBAPI GBDataAccessInproc();
	GBAPI ~GBDataAccessInproc();
	GBAPI HRESULT GBCALL GBInitialize(
		CLSID* pClassId,
		DWORD dwTimeBase,
		DWORD dwMinUpdateRate,
		DWORD dwFlags,
		DWORD dwTagMax,
		DWORD dwVersionMajor = 1,
		DWORD dwVersionMinor = 0,
		DWORD dwVersionBuild = 0,
		LPCWSTR szVendorName = NULL);
	GBAPI HRESULT GBCALL GBInitialize(
		CLSID* pClassId,
		DWORD dwTimeBase,
		DWORD dwMinUpdateRate,
		DWORD dwFlags,
		WCHAR cSeparator,
		DWORD dwTagMax,
		DWORD dwVersionMajor = 1,
		DWORD dwVersionMinor = 0,
		DWORD dwVersionBuild = 0,
		LPCWSTR szVendorName = NULL);
	static GBAPI HRESULT GBCALL GBRegisterServer(
		HMODULE hModule,
		CLSID* pClassID,
		LPCWSTR szVendorName,
		LPCWSTR szDescription,
		LPCWSTR szVersionIndipProgID,
		LPCWSTR szCurVersion);
	static GBAPI HRESULT GBCALL GBUnregisterServer(
		HMODULE hModule,
		CLSID* pClassID);
	GBAPI HRESULT GBCALL GBCanUnloadNow( );
	GBAPI HRESULT GBCALL GBGetClassObject(
		REFCLSID rClassID,
		REFIID rIID,
		LPVOID* pInterface);
	GBAPI virtual HRESULT GBCALL GBOnServerReleased(); // if returns S_FALSE - class object suspends
};

GBAPI void GBCALL GBSetResourceModule(HMODULE hModule);

#pragma pack(pop)

#endif