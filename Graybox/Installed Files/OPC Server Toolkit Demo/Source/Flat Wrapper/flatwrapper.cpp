// MinOPCInp.cpp : Defines the entry point for the DLL application.
//

#ifdef _WIN32_WCE

	#pragma comment(linker, "/nodefaultlib:libc.lib")
	#pragma comment(linker, "/nodefaultlib:libcd.lib")

	// NOTE - this value is not strongly correlated to the Windows CE OS version being targeted
	#define WINVER _WIN32_WCE

	#include <ceconfig.h>
	#if defined(WIN32_PLATFORM_PSPC) || defined(WIN32_PLATFORM_WFSP)
	#define SHELL_AYGSHELL
	#endif

	#ifdef SHELL_AYGSHELL
	#include <aygshell.h>
	#pragma comment(lib, "aygshell.lib") 
	#endif // SHELL_AYGSHELL

	// Windows Header Files:
	#include <windows.h>

	#if defined(WIN32_PLATFORM_PSPC) || defined(WIN32_PLATFORM_WFSP)
	#ifndef _DEVICE_RESOLUTION_AWARE
	#define _DEVICE_RESOLUTION_AWARE
	#endif
	#endif

	#if _WIN32_WCE < 0x500 && ( defined(WIN32_PLATFORM_PSPC) || defined(WIN32_PLATFORM_WFSP) )
		#pragma comment(lib, "ccrtrtti.lib")
		#ifdef _X86_	
			#if defined(_DEBUG)
				#pragma comment(lib, "libcmtx86d.lib")
			#else
				#pragma comment(lib, "libcmtx86.lib")
			#endif
		#endif
	#endif


#else

	#define _WIN32_DCOM
	#include <windows.h>

#endif

#include <GB_OPCDA.h>
#pragma comment(lib, "gbda3.lib")

// Defines the function pointer types for GBDataAccess events
typedef HRESULT (_cdecl *BeforeCreateInstanceFunction)(BOOL, DWORD*);
typedef	void (_cdecl *CreateInstanceFunction)(DWORD);
typedef void (_cdecl *DestroyInstanceFunction)(DWORD);
typedef void (_cdecl *LockFunction)();
typedef void (_cdecl *UnlockFunction)();
typedef HRESULT (_cdecl *ServerReleasedFunction)();
typedef	DWORD (_cdecl *WriteItemsFunction)(DWORD, GBItemID*, VARIANT*, WORD*, FILETIME*, HRESULT*,	HRESULT*, LCID,	DWORD);
typedef DWORD (_cdecl *ReadItemsFunction)(DWORD, GBItemID*, VARIANT*, WORD*, FILETIME*, HRESULT*, HRESULT*, HRESULT*, const VARTYPE*, LCID, DWORD);
typedef void (_cdecl *DataUpdateFunction)(DWORD, GBItemID*, VARIANT*, WORD*, FILETIME*,	HRESULT*, HRESULT, DWORD);
typedef void (_cdecl *ActivateItemsFunction)(DWORD, GBItemID*);
typedef void (_cdecl *DeactivateItemsFunction)(DWORD, GBItemID*);
typedef	HRESULT (_cdecl *GetErrorStringFunction)(HRESULT, LCID, LPWSTR*, DWORD);
typedef	HRESULT (_cdecl *QueryLocalesFunction)(DWORD*, LCID**);
typedef	HRESULT (_cdecl *BrowseAccessPathsFunction)(LPCWSTR, DWORD*, LPWSTR**, DWORD);
typedef	HRESULT (_cdecl *QueryItemFunction)(LPCWSTR, LPCWSTR, VARTYPE, BOOL, DWORD*, DWORD*, DWORD);
typedef	HRESULT (_cdecl *ReadPropertiesFunction)(GBItemID*,	DWORD, DWORD*, VARIANT*, HRESULT*, LCID, DWORD);

class GBDataAccessNative : public GBDataAccess
{
private:
   enum GBDataAccessEventIdx
   {
        EventIdx_BeforeCreateInstance = 0,
        EventIdx_CreateInstance,
        EventIdx_DestroyInstance,
        EventIdx_Lock,
        EventIdx_Unlock,
        EventIdx_ServerReleased,
        EventIdx_WriteItems,
        EventIdx_ReadItems,
        EventIdx_DataUpdate,
        EventIdx_ActivateItems,
        EventIdx_DeactivateItems,
        EventIdx_GetErrorString,
        EventIdx_QueryLocales,
        EventIdx_BrowseAccessPaths,
        EventIdx_QueryItem,
        EventIdx_ReadProperties,
        EventIdx_Count
    };
	BeforeCreateInstanceFunction m_pOnBeforeCreateInstance;
	CreateInstanceFunction m_pOnCreateInstance;
	DestroyInstanceFunction m_pOnDestroyInstance;
	LockFunction m_pOnLock;
	UnlockFunction m_pOnUnlock;
	ServerReleasedFunction m_pOnServerReleased;
	WriteItemsFunction m_pOnWriteItems;
	ReadItemsFunction m_pOnReadItems;
	DataUpdateFunction m_pOnDataUpdate;
    ActivateItemsFunction m_pOnActivateItems;
    DeactivateItemsFunction m_pOnDeactivateItems;
    GetErrorStringFunction m_pOnGetErrorString;
    QueryLocalesFunction m_pOnQueryLocales;
    BrowseAccessPathsFunction m_pOnBrowseAccessPaths;
    QueryItemFunction m_pOnQueryItem;
    ReadPropertiesFunction m_pOnReadProperties;
	LONG m_EventAdvised[EventIdx_Count];
	bool m_ThreadSafeAdvise;
private:
	inline bool CheckAdvised(int idx)
	{
		if (m_ThreadSafeAdvise) return (InterlockedExchangeAdd(&m_EventAdvised[idx], 0) > 0);
		return (m_EventAdvised[idx] > 0);
	}

public:

	void SetEvents(
		bool bThreadSafeAdvise,
		BeforeCreateInstanceFunction pOnBeforeCreateInstance,
		CreateInstanceFunction pOnCreateInstance,
		DestroyInstanceFunction pOnDestroyInstance,
		LockFunction pOnLock,
		UnlockFunction pOnUnlock,
		ServerReleasedFunction pOnServerReleased,
		WriteItemsFunction pOnWriteItems,
		ReadItemsFunction pOnReadItems,
		DataUpdateFunction pOnDataUpdate,
		ActivateItemsFunction pOnActivateItems,
		DeactivateItemsFunction pOnDeactivateItems,
		GetErrorStringFunction pOnGetErrorString,
		QueryLocalesFunction pOnQueryLocales,
		BrowseAccessPathsFunction pOnBrowseAccessPaths,
		QueryItemFunction pOnQueryItem,
		ReadPropertiesFunction pOnReadProperties)
	{
		m_ThreadSafeAdvise = bThreadSafeAdvise;
		memset(m_EventAdvised, 0, sizeof(m_EventAdvised));
		m_pOnBeforeCreateInstance = pOnBeforeCreateInstance;
		m_pOnCreateInstance = pOnCreateInstance;
		m_pOnDestroyInstance = pOnDestroyInstance;
		m_pOnLock = pOnLock;
		m_pOnUnlock = pOnUnlock;
		m_pOnServerReleased = pOnServerReleased;
		m_pOnWriteItems = pOnWriteItems;
		m_pOnReadItems = pOnReadItems;
		m_pOnDataUpdate = pOnDataUpdate;
		m_pOnActivateItems = pOnActivateItems;
		m_pOnDeactivateItems = pOnDeactivateItems;
		m_pOnGetErrorString = pOnGetErrorString;
		m_pOnQueryLocales = pOnQueryLocales;
		m_pOnBrowseAccessPaths = pOnBrowseAccessPaths;
		m_pOnQueryItem = pOnQueryItem;
		m_pOnReadProperties = pOnReadProperties;
	}

	int Advise(int nEventIdx)
	{
		if (m_ThreadSafeAdvise) return InterlockedIncrement(&m_EventAdvised[nEventIdx]);
		return ++m_EventAdvised[nEventIdx];
	}

	int Unadvise(int nEventIdx)
	{
		if (m_ThreadSafeAdvise) return InterlockedDecrement(&m_EventAdvised[nEventIdx]);
		return --m_EventAdvised[nEventIdx];
	}


private:

	// GBDataAccess callback handlers
	HRESULT GBCALL GBOnBeforeCreateInstance(BOOL bAggregating, DWORD* pdwClientId)
	{
		if (!CheckAdvised(EventIdx_BeforeCreateInstance))
			return GBDataAccess::GBOnBeforeCreateInstance(bAggregating, pdwClientId);
		return m_pOnBeforeCreateInstance(bAggregating, pdwClientId);
	}

	void GBCALL GBOnCreateInstance(DWORD dwClientId)
	{
		if (!CheckAdvised(EventIdx_CreateInstance))
		{
			GBDataAccess::GBOnCreateInstance(dwClientId);
			return;
		}
		m_pOnCreateInstance(dwClientId);
	}

	void GBCALL GBOnDestroyInstance(DWORD dwClientId)
	{
		if (!CheckAdvised(EventIdx_DestroyInstance))
			GBDataAccess::GBOnDestroyInstance(dwClientId);
		else
			m_pOnDestroyInstance(dwClientId);
	}

	void GBCALL GBOnLock()
	{
		if (!CheckAdvised(EventIdx_Lock))
			GBDataAccess::GBOnLock();
		else
			m_pOnLock();
	}

	void GBCALL GBOnUnLock()
	{
		if (!CheckAdvised(EventIdx_Unlock))
			GBDataAccess::GBOnUnLock();
		else
			m_pOnUnlock();
	}

	HRESULT GBCALL GBOnServerReleased()
	{
		if (!CheckAdvised(EventIdx_ServerReleased))
			return GBDataAccess::GBOnServerReleased();
		return m_pOnServerReleased();
	}

	DWORD GBCALL GBOnWriteItems(
		DWORD dwCount,
		GBItemID* pTags,
		VARIANT* pvValues,
		WORD* pwQualities,
		FILETIME* pftTimestamps,
		HRESULT* pErrors,
		HRESULT* pMasterError,
		LCID dwLcid,
		DWORD dwClientID)
	{
		if (!CheckAdvised(EventIdx_WriteItems))
			return GBDataAccess::GBOnWriteItems(dwCount, pTags, pvValues, pwQualities, pftTimestamps,
			pErrors, pMasterError, dwLcid, dwClientID);
		return m_pOnWriteItems(dwCount, pTags, pvValues, pwQualities, pftTimestamps,
			pErrors, pMasterError, dwLcid, dwClientID);
	}

	DWORD GBCALL GBOnReadItems(
		DWORD dwCount,
		GBItemID* pTags,
		VARIANT* pValues,
		WORD* pQualities,
		FILETIME* pTimestamps,
		HRESULT* pErrors,
		HRESULT* pMasterError,
		HRESULT* pMasterQuality,
		const VARTYPE* pRequestedTypes,
		LCID dwLcid,
		DWORD dwClientID)
	{
		if (!CheckAdvised(EventIdx_ReadItems))
			return GBDataAccess::GBOnReadItems(dwCount, pTags, pValues, pQualities, pTimestamps, pErrors,
			pMasterError, pMasterQuality, pRequestedTypes, dwLcid, dwClientID);
		return m_pOnReadItems(dwCount, pTags, pValues, pQualities, pTimestamps, pErrors,
			pMasterError, pMasterQuality, pRequestedTypes, dwLcid, dwClientID);
	}

	void GBCALL GBOnDataUpdate(
		DWORD dwCount,
		GBItemID* pTags,
		VARIANT* pValues,
		WORD* pQualities,
		FILETIME* pTimestamps,
		HRESULT* pErrors,
		HRESULT dwClientResult,
		DWORD dwClientID)
	{
		if (!CheckAdvised(EventIdx_DataUpdate)) return;
		m_pOnDataUpdate(dwCount, pTags, pValues, pQualities, pTimestamps, pErrors, dwClientResult, dwClientID);
	}

    void GBCALL GBOnActivate(
		DWORD dwCount,
		GBItemID* pTags)
	{
		if (!CheckAdvised(EventIdx_ActivateItems))
		{
			GBDataAccess::GBOnActivate(dwCount, pTags);
			return;
		}
		m_pOnActivateItems(dwCount, pTags);
	}

    void GBCALL GBOnDeactivate(
		DWORD dwCount,
		GBItemID* pTags)
	{
		if (!CheckAdvised(EventIdx_DeactivateItems))
		{
			GBDataAccess::GBOnDeactivate(dwCount, pTags);
			return;
		}
		m_pOnDeactivateItems(dwCount, pTags);
	}

	HRESULT GBCALL GBOnGetErrorString(
		HRESULT dwError,
		LCID dwLcid,
		LPWSTR* pszErrorString,
		DWORD dwCallerID)
	{
		if (!CheckAdvised(EventIdx_GetErrorString))
			return GBDataAccess::GBOnGetErrorString(dwError, dwLcid, pszErrorString, dwCallerID);
		return m_pOnGetErrorString(dwError, dwLcid, pszErrorString, dwCallerID);
	}

	HRESULT GBCALL GBOnQueryLocales(
		DWORD* pdwCount,
		LCID** ppdwLcid)
	{
		if (!CheckAdvised(EventIdx_QueryLocales))
			return GBDataAccess::GBOnQueryLocales(pdwCount, ppdwLcid);
		return m_pOnQueryLocales(pdwCount, ppdwLcid);
	}

	HRESULT GBCALL GBOnBrowseAccessPath(
		LPCWSTR szItemID,
		DWORD* pdwAccessPathCount,
		LPWSTR** ppszAccessPaths,
		DWORD dwClientID)
	{
		if (!CheckAdvised(EventIdx_BrowseAccessPaths))
			return GBDataAccess::GBOnBrowseAccessPath(szItemID, pdwAccessPathCount, ppszAccessPaths, dwClientID);
		return m_pOnBrowseAccessPaths(szItemID, pdwAccessPathCount, ppszAccessPaths, dwClientID);
	}

	HRESULT GBCALL GBOnQueryItem(
		LPCWSTR szItemID,
		LPCWSTR szAccessPath,
		VARTYPE wDataType,
		BOOL bAddItem,
		DWORD* pdwTagID,
		DWORD* pdwAccessPathID,
		DWORD dwClientID)
	{
		if (!CheckAdvised(EventIdx_QueryItem))
			return GBDataAccess::GBOnQueryItem(szItemID, szAccessPath, wDataType, bAddItem, pdwTagID, pdwAccessPathID, dwClientID);
		return m_pOnQueryItem(szItemID, szAccessPath, wDataType, bAddItem, pdwTagID, pdwAccessPathID, dwClientID);
	}

	HRESULT GBCALL GBOnGetProperties(
		GBItemID* pTag,
		DWORD dwCount,
		DWORD *pdwPropIDs,
		VARIANT *pvValues,
		HRESULT *pdwErrors,
		LCID dwLcid,
		DWORD dwClientID)
	{
		if (!CheckAdvised(EventIdx_ReadProperties))
			return GBDataAccess::GBOnGetProperties(pTag, dwCount, pdwPropIDs, pvValues, pdwErrors, dwLcid, dwClientID);
		return m_pOnReadProperties(pTag, dwCount, pdwPropIDs, pvValues, pdwErrors, dwLcid, dwClientID);
	}
};

GBDataAccessNative* _cdecl GBDataAccess_Constructor(
		bool bThreadSafeAdvise,
		BeforeCreateInstanceFunction pOnBeforeCreateInstance,
		CreateInstanceFunction pOnCreateInstance,
		DestroyInstanceFunction pOnDestroyInstance,
		LockFunction pOnLock,
		UnlockFunction pOnUnlock,
		ServerReleasedFunction pOnServerReleased,
		WriteItemsFunction pOnWriteItems,
		ReadItemsFunction pOnReadItems,
		DataUpdateFunction pOnDataUpdate,
		ActivateItemsFunction pOnActivateItems,
		DeactivateItemsFunction pOnDeactivateItems,
		GetErrorStringFunction pOnGetErrorString,
		QueryLocalesFunction pOnQueryLocales,
		BrowseAccessPathsFunction pOnBrowseAccessPaths,
		QueryItemFunction pOnQueryItem,
		ReadPropertiesFunction pOnReadProperties)
{
	GBDataAccessNative* pNewInstace = new GBDataAccessNative();
	pNewInstace->SetEvents(
		bThreadSafeAdvise,
		pOnBeforeCreateInstance,
		pOnCreateInstance,
		pOnDestroyInstance,
		pOnLock,
		pOnUnlock,
		pOnServerReleased,
		pOnWriteItems,
		pOnReadItems,
		pOnDataUpdate,
		pOnActivateItems,
		pOnDeactivateItems,
		pOnGetErrorString,
		pOnQueryLocales,
		pOnBrowseAccessPaths,
		pOnQueryItem,
		pOnReadProperties);
	return pNewInstace;
}

void _cdecl GBDataAccess_Destructor(GBDataAccessNative* pNativeInstance)
{
	delete pNativeInstance;
}

int _cdecl GBDataAccess_Advise(GBDataAccessNative* pNativeInstance, int nEventIdx)
{
	return pNativeInstance->Advise(nEventIdx);
}

int _cdecl GBDataAccess_Unadvise(GBDataAccessNative* pNativeInstance, int nEventIdx)
{
	return pNativeInstance->Unadvise(nEventIdx);
}

HRESULT _cdecl GBDataAccess_GBInitialize(
		GBDataAccessNative* pNativeInstance,
		CLSID* pClassId,
		DWORD dwTimeBase,
		DWORD dwMinUpdateRate,
		DWORD dwFlags,
		WCHAR cSeparator,
		DWORD dwTagMax,
		DWORD dwVersionMajor,
		DWORD dwVersionMinor,
		DWORD dwVersionBuild,
		LPCWSTR szVendorName)
{
	return pNativeInstance->GBInitialize(pClassId, dwTimeBase, dwMinUpdateRate, dwFlags,
		cSeparator, dwTagMax, dwVersionMajor, dwVersionMinor, dwVersionBuild, szVendorName);
}

HRESULT _cdecl GBDataAccess_GBRegisterClassObject(
		GBDataAccessNative* pNativeInstance)
{
	return pNativeInstance->GBRegisterClassObject();
}

HRESULT _cdecl GBDataAccess_GBRevokeClassObject(
		GBDataAccessNative* pNativeInstance)
{
	return pNativeInstance->GBRevokeClassObject();
}

HRESULT _cdecl GBDataAccess_GBRegisterServer(
		CLSID* pClassID,
		LPCWSTR szVendorName,
		LPCWSTR szDescription,
		LPCWSTR szVersionIndipProgID,
		LPCWSTR szCurVersion,
		LPCWSTR szServiceName)
{
	return GBDataAccess::GBRegisterServer(pClassID, szVendorName, szDescription, szVersionIndipProgID, szCurVersion, szServiceName);
}

HRESULT _cdecl GBDataAccess_GBUnregisterServer(
		CLSID* pClassID)
{
	return GBDataAccess::GBUnregisterServer(pClassID);
}

HRESULT _cdecl GBDataAccess_GBGetItems(
		GBDataAccessNative* pNativeInstance,
		DWORD dwCount,
		DWORD* pdwTagIDs,
		VARIANT* pvValues,
		WORD* pwQualities,
		FILETIME* pftTimeStamps,
		HRESULT* pdwErrors)
{
	// It is necessary to pass a pointer to the array of valid empty VARIANTs.
	memset(pvValues, 0, sizeof(VARIANT)*dwCount);
	return pNativeInstance->GBGetItems(dwCount, pdwTagIDs, pvValues, pwQualities, pftTimeStamps, pdwErrors);
}

void _cdecl GBDataAccess_GBGetItems_Cleanup(int nCount, VARIANT* pVars)
{
	for (int i = 0; i < nCount; i++) VariantClear(&pVars[i]);
	LocalFree(pVars);
}


HRESULT _cdecl GBDataAccess_GBUpdateItems(
		GBDataAccessNative* pNativeInstance,
		DWORD dwCount,
		DWORD* pdwItemID,
		VARIANT* pvValues,
		WORD* pwQualities,
		FILETIME* pftTimestamps,
		HRESULT* pdwErrors,
		BOOL bWait)
{
	return pNativeInstance->GBUpdateItems(dwCount, pdwItemID, pvValues, pwQualities, pftTimestamps, pdwErrors, bWait);
}

HRESULT _cdecl GBDataAccess_GBBeginUpdate(GBDataAccessNative* pNativeInstance)
{
	return pNativeInstance->GBBeginUpdate();
}

HRESULT _cdecl GBDataAccess_GBSetItem(
		GBDataAccessNative* pNativeInstance,
		DWORD dwTagID,
		VARIANT* pvValue,
		WORD wQuality,
		FILETIME* pftTimestamp,
		HRESULT dwError)
{
	return pNativeInstance->GBSetItem(dwTagID, pvValue, wQuality, pftTimestamp, dwError);
}

HRESULT _cdecl GBDataAccess_GBEndUpdate(
		GBDataAccessNative* pNativeInstance,
		BOOL bWait)
{
	return pNativeInstance->GBEndUpdate(bWait);
}

HRESULT _cdecl GBDataAccess_GBCreateItem(
		GBDataAccessNative* pNativeInstance,
		DWORD* pdwItemID,
		DWORD dwUserID,
		LPCWSTR szOpcItemID,
		DWORD dwAccessRights,
		DWORD dwFlags,
		VARIANT* pvValue,
		OPCEUTYPE dwEUType,
		VARIANT* pvEUInfo)
{
	return pNativeInstance->GBCreateItem(pdwItemID, dwUserID, szOpcItemID, dwAccessRights, dwFlags,
		pvValue, dwEUType, pvEUInfo);
}

HRESULT _cdecl GBDataAccess_GBCreateItemAnalog(
		GBDataAccessNative* pNativeInstance,
		DWORD* pdwItemID,
		DWORD dwUserID,
		LPCWSTR szOpcItemID,	// string or resource id
		DWORD dwAccessRights,
		DWORD dwFlags,
		VARIANT* pvValue,
		double dLowRange,
		double dHighRange)
{
	return pNativeInstance->GBCreateItemAnalog(pdwItemID, dwUserID, szOpcItemID, dwAccessRights, dwFlags,
		pvValue, dLowRange, dHighRange);
}

HRESULT _cdecl GBDataAccess_GBCreateItemEnum(
		GBDataAccessNative* pNativeInstance,
		DWORD* pdwTagIDs,
		DWORD dwUserID,
		LPCWSTR szOpcItemID,	// string or resource id
		DWORD dwAccessRights,
		DWORD dwFlags,
		VARIANT* pvValue,
		DWORD dwEnumCount,
		LPCWSTR* pszEnumStrings)
{
	return pNativeInstance->GBCreateItemEnum(pdwTagIDs, dwUserID, szOpcItemID, dwAccessRights,
		dwFlags, pvValue, dwEnumCount, pszEnumStrings);
}

HRESULT _cdecl GBDataAccess_GBAddProperty(
		GBDataAccessNative* pNativeInstance,
		DWORD dwTagID,
		DWORD dwPropID,
		VARIANT* pvValue,
		LPCWSTR szDescription,		// string or resource id, if NULL or empty common OPC prop def will be used
		LPCWSTR szOpcItemID,		// string or resource id, if NULL or empty common OPC prop def will be used
		DWORD dwFlags)
{
	return pNativeInstance->GBAddProperty(dwTagID, dwPropID, pvValue, szDescription, szOpcItemID, dwFlags);
}

HRESULT _cdecl GBDataAccess_GBSetProperty(
		GBDataAccessNative* pNativeInstance,
		DWORD dwTagID,
		DWORD dwPropID,
		VARIANT* pvValue)
{
	return pNativeInstance->GBSetProperty(dwTagID, dwPropID, pvValue);
}

HRESULT _cdecl GBDataAccess_GBGetProperty(
		GBDataAccessNative* pNativeInstance,
		DWORD dwTagID,
		DWORD dwPropID,
		VARIANT* pvValue)
{
	pvValue->vt = VT_EMPTY;
	return pNativeInstance->GBGetProperty(dwTagID, dwPropID, pvValue);
}

HRESULT _cdecl GBDataAccess_GBRemoveProperty(
		GBDataAccessNative* pNativeInstance,
		DWORD dwTagID,
		DWORD dwPropID)
{
	return pNativeInstance->GBRemoveProperty(dwTagID, dwPropID);
}

HRESULT _cdecl GBDataAccess_GBGetBandwidth(
		GBDataAccessNative* pNativeInstance,
		DWORD* pdwBandwidth)
{
	return pNativeInstance->GBGetBandwidth(pdwBandwidth);
}

HRESULT _cdecl GBDataAccess_GBSetState(
		GBDataAccessNative* pNativeInstance,
		OPCSERVERSTATE dwServerState)
{
	return pNativeInstance->GBSetState(dwServerState);
}

HRESULT _cdecl GBDataAccess_GBSuspend(GBDataAccessNative* pNativeInstance)
{
	return pNativeInstance->GBSuspend();
}

HRESULT _cdecl GBDataAccess_GBResume(GBDataAccessNative* pNativeInstance)
{
	return pNativeInstance->GBResume();
}

HRESULT _cdecl GBDataAccess_GBShutdown(GBDataAccessNative* pNativeInstance, LPCWSTR szReason)
{
	return pNativeInstance->GBShutdown(szReason);
}

HRESULT _cdecl GBDataAccess_GBDisconnectClients(GBDataAccessNative* pNativeInstance)
{
	return pNativeInstance->GBDisconnectClients();
}

VARTYPE _cdecl GBDataAccess_GBGetItemCanonicalType(GBDataAccessNative* pNativeInstance, DWORD dwTagId)
{
	return pNativeInstance->GBGetItemCanonicalType(dwTagId);
}

VARIANT* _cdecl GBDataAccess_GBGetItemDefaultValue(GBDataAccessNative* pNativeInstance, DWORD dwTagId)
{
	return pNativeInstance->GBGetItemDefaultValue(dwTagId);
}

DWORD _cdecl GBDataAccess_GBGetItemEUType(GBDataAccessNative* pNativeInstance, DWORD dwTagId)
{
	return pNativeInstance->GBGetItemEUType(dwTagId);
}

VARIANT* _cdecl GBDataAccess_GBGetItemEUInfo(GBDataAccessNative* pNativeInstance, DWORD dwTagId)
{
	return pNativeInstance->GBGetItemEUInfo(dwTagId);
}


// Module entry point.
BOOL APIENTRY DllMain( HANDLE module, 
                       DWORD ul_reason_for_call, 
                       LPVOID reserved )
{
	if (ul_reason_for_call == DLL_PROCESS_ATTACH)
	{
		DisableThreadLibraryCalls((HMODULE)module);
	}
	return TRUE;
}

