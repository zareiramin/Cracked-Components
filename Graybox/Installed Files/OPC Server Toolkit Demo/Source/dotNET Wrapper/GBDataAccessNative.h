#pragma once
#include <vcclr.h>
#include <gb_opcda.h>
#include "Events.h"

using namespace System::Runtime::InteropServices;
using namespace Graybox::OPC::ServerToolkit::CLRWrapper;

template <class T> class GBDataAccessNativeDecendant :
	public T
{
protected:
	gcroot<OPCDAServerEvents^> m_events;
	gcroot<System::Object^> m_gbda_clr;
	LONG m_event_flags[EventIdx_Count];
	inline bool GetEventFlag(Int32 event_idx)
	{
		return (InterlockedExchangeAdd(&m_event_flags[event_idx], 0) != 0);
	}
public:
	GBDataAccessNativeDecendant()
	{
	}
	GBDataAccessNativeDecendant(System::Object^ gbda_clr)
	{
		m_gbda_clr = gbda_clr;
		memset(m_event_flags, 0, sizeof(m_event_flags));
		m_events = gcnew OPCDAServerEvents(IntPtr(m_event_flags));
	}
	OPCDAServerEvents^ GetEvents()
	{
		return m_events;
	}

	////////////////////////////////////////////////////////////////////////////////////
	// GBDataAccessBase implementation
	////////////////////////////////////////////////////////////////////////////////////

	HRESULT __stdcall GBOnBeforeCreateInstance(BOOL bAggregating, DWORD* pdwClientId)
	{
		if (!GetEventFlag(EventIdx_BeforeCreateInstance))
			return __super::GBOnBeforeCreateInstance(bAggregating, pdwClientId);
		BeforeCreateInstanceArgs^ e = gcnew BeforeCreateInstanceArgs(bAggregating == TRUE, (Int32)*pdwClientId);
		m_events->BeforeCreateInstance(m_gbda_clr, e);
		*pdwClientId = (DWORD)e->ClientId;
		return (HRESULT)e->EventHandlingError;
	}

	void __stdcall GBOnCreateInstance(DWORD dwClientId)
	{
		if (!GetEventFlag(EventIdx_CreateInstance)) return;
		m_events->CreateInstance(m_gbda_clr, gcnew CreateInstanceArgs((Int32)dwClientId));
	}

	void __stdcall GBOnDestroyInstance(DWORD dwClientId)
	{
		if (!GetEventFlag(EventIdx_DestroyInstance)) return;
		m_events->DestroyInstance(m_gbda_clr, gcnew DestroyInstanceArgs((Int32)dwClientId));
	}

	void __stdcall GBOnLock()
	{
		if (!GetEventFlag(EventIdx_Lock)) return;
		m_events->Lock(m_gbda_clr, gcnew LockArgs());
	}

	void __stdcall GBOnUnLock()
	{
		if (!GetEventFlag(EventIdx_Unlock)) return;
		m_events->Unlock(m_gbda_clr, gcnew UnlockArgs());
	}

	HRESULT __stdcall GBOnServerReleased()
	{
		if (!GetEventFlag(EventIdx_ServerReleased))
			return __super::GBOnServerReleased();
		ServerReleasedArgs^ e = gcnew ServerReleasedArgs(true);
		m_events->ServerReleased(m_gbda_clr, e);
		return e->Suspend? S_FALSE : S_OK;
	}

	DWORD __stdcall GBOnWriteItems(
		DWORD dwCount,
		GBItemID* pTags,
		VARIANT* pvValues,
		WORD* pwQualities,
		::FILETIME* pftTimestamps,
		HRESULT* pErrors,
		HRESULT* pMasterError,
		LCID dwLcid,
		DWORD dwClientID)
	{
		if (!GetEventFlag(EventIdx_WriteItems))
			return __super::GBOnWriteItems(dwCount, pTags, pvValues,
				pwQualities, pftTimestamps, pErrors, pMasterError, dwLcid, dwClientID);
		
		array<ItemId>^ tags = gcnew array<ItemId>(dwCount);
		pin_ptr<ItemId> p_tags = &tags[0];
		memcpy(p_tags, pTags, sizeof(GBItemID)*dwCount);

		array<Object^>^ values = Marshal::GetObjectsForNativeVariants(IntPtr(pvValues), dwCount);

		array<Quality>^ qualities = nullptr;
		if (pwQualities)
		{
			qualities = gcnew array<Quality>(dwCount);
			pin_ptr<Quality> p_qualities = &qualities[0];
			memcpy(p_qualities, pwQualities, sizeof(WORD)*dwCount);
		}

		array<FileTime>^ timestamps = nullptr;
		if (pftTimestamps)
		{
			timestamps = gcnew array<FileTime>(dwCount);
			pin_ptr<FileTime> p_timestamps = &timestamps[0];
			memcpy(p_timestamps, pftTimestamps, sizeof(::FILETIME)*dwCount);
		}

		WriteItemsArgs^ e = gcnew WriteItemsArgs(
			tags, values, qualities, timestamps, dwLcid, dwClientID);
		m_events->WriteItems(m_gbda_clr, e);
		*pMasterError = (HRESULT)e->MasterError;

		memcpy(pTags, p_tags, sizeof(GBItemID)*dwCount);

		pin_ptr<ErrorCodes> p_errors = &e->Errors[0];
		memcpy(pErrors, p_errors, sizeof(HRESULT)*dwCount);
		return e->CopyToCache ? GB_RET_CACHE : GB_RET_NOP;
	}

   void __stdcall GBOnDataUpdate(
		DWORD dwCount,
		GBItemID* pTags,
		VARIANT* pvValues,
		WORD* pwQualities,
		::FILETIME* pftTimestamps,
		HRESULT* pErrors,
		HRESULT dwClientResult,
		DWORD dwClientID)
   {
		if (!GetEventFlag(EventIdx_WriteItems)) return;
		
		array<ItemId>^ tags = gcnew array<ItemId>(dwCount);
		pin_ptr<ItemId> p_tags = &tags[0];
		memcpy(p_tags, pTags, sizeof(GBItemID)*dwCount);

		array<Object^>^ values = Marshal::GetObjectsForNativeVariants(IntPtr(pvValues), dwCount);

		array<Quality>^ qualities = gcnew array<Quality>(dwCount);
		pin_ptr<Quality> p_qualities = &qualities[0];
		memcpy(p_qualities, pwQualities, sizeof(WORD)*dwCount);

		array<FileTime>^ timestamps = gcnew array<FileTime>(dwCount);
		pin_ptr<FileTime> p_timestamps = &timestamps[0];
		memcpy(p_timestamps, pftTimestamps, sizeof(::FILETIME)*dwCount);

		array<ErrorCodes>^ errors = gcnew array<ErrorCodes>(dwCount);
		pin_ptr<ErrorCodes> p_errors = &errors[0];
		memcpy(p_errors, pErrors, sizeof(HRESULT)*dwCount);

		DataUpdateArgs^ e = gcnew DataUpdateArgs(
			tags, values, qualities, timestamps, errors, dwClientResult, dwClientID);
		m_events->DataUpdate(m_gbda_clr, e);
   }

	DWORD __stdcall GBOnUpdateItems(
		DWORD dwCount,
		GBItemID* pTags,
		VARIANT* pvValues,
		WORD* pwQualities,
		::FILETIME* pftTimestamps,
		HRESULT* pErrors,
		HRESULT* pMasterError,
		LCID dwLcid,
		DWORD dwClientID)
	{
		if (!GetEventFlag(EventIdx_WriteItems))
			return __super::GBOnWriteItems(dwCount, pTags, pvValues,
				pwQualities, pftTimestamps, pErrors, pMasterError, dwLcid, dwClientID);
		
		array<ItemId>^ tags = gcnew array<ItemId>(dwCount);
		pin_ptr<ItemId> p_tags = &tags[0];
		memcpy(p_tags, pTags, sizeof(GBItemID)*dwCount);

		array<Object^>^ values = Marshal::GetObjectsForNativeVariants(IntPtr(pvValues), dwCount);

		array<Quality>^ qualities = nullptr;
		if (pwQualities)
		{
			qualities = gcnew array<Quality>(dwCount);
			pin_ptr<Quality> p_qualities = &qualities[0];
			memcpy(p_qualities, pwQualities, sizeof(WORD)*dwCount);
		}

		array<FileTime>^ timestamps = nullptr;
		if (pftTimestamps)
		{
			timestamps = gcnew array<FileTime>(dwCount);
			pin_ptr<FileTime> p_timestamps = &timestamps[0];
			memcpy(p_timestamps, pftTimestamps, sizeof(::FILETIME)*dwCount);
		}

		WriteItemsArgs^ e = gcnew WriteItemsArgs(
			tags, values, qualities, timestamps, dwLcid, dwClientID);
		m_events->WriteItems(m_gbda_clr, e);
		*pMasterError = (HRESULT)e->MasterError;

		memcpy(pTags, p_tags, sizeof(GBItemID)*dwCount);

		pin_ptr<ErrorCodes> p_errors = &e->Errors[0];
		memcpy(pErrors, p_errors, sizeof(HRESULT)*dwCount);
		return e->CopyToCache ? GB_RET_CACHE : GB_RET_NOP;
	}

	DWORD __stdcall GBOnReadItems(
		DWORD dwCount,
		GBItemID* pTags,
		VARIANT* pValues,
		WORD* pQualities,
		::FILETIME* pTimestamps,
		HRESULT* pErrors,
		HRESULT* pMasterError,
		HRESULT* pMasterQuality,
		const VARTYPE* pRequestedTypes,
		LCID dwLcid,
		DWORD dwClientID)
	{
		if (!GetEventFlag(EventIdx_ReadItems))
			return __super::GBOnReadItems(dwCount, pTags, pValues,
				pQualities, pTimestamps, pErrors, pMasterError,
				pMasterQuality, pRequestedTypes, dwLcid, dwClientID);
		
		array<ItemId>^ tags = gcnew array<ItemId>(dwCount);
		pin_ptr<ItemId> p_tags = &tags[0];
		memcpy(p_tags, pTags, sizeof(GBItemID)*dwCount);

		ReadItemsArgs^ e = gcnew ReadItemsArgs(
			tags, (pValues != NULL), dwLcid, dwClientID);
		m_events->ReadItems(m_gbda_clr, e);
		if (pValues && e->ValuesReturned)
		{
			*pMasterError = (HRESULT)e->MasterError;
			*pMasterQuality = (HRESULT)e->MasterQuality;
			pin_ptr<Quality> p_q = &e->Qualities[0];
			pin_ptr<FileTime> p_t = &e->Timestamps[0];
			pin_ptr<ErrorCodes> p_e = &e->Errors[0];

			if (e->ValuesReturnedPartial)
			{
				memcpy(pTags, p_tags, sizeof(GBItemID)*dwCount);
				for (unsigned i = 0; i<dwCount; i++)
				{
					if (pTags[i].dwTagID != 0 || e->Values[i] == nullptr) continue;
					pQualities[i] = ((WORD*)p_q)[i];
					pTimestamps[i] = ((::FILETIME*)p_t)[i];
					pErrors[i] = ((HRESULT*)p_e)[i];
					if FAILED(pErrors[i]) continue;
					Marshal::GetNativeVariantForObject(e->Values[i], IntPtr((int)&pValues[i]));
					VARTYPE vt = pRequestedTypes[i];
					if (vt == pValues[i].vt) continue;
					HRESULT hr = VariantChangeTypeEx(&pValues[i], &pValues[i], dwLcid, 0, vt);
					if FAILED(hr)
					{
						pErrors[i] = hr;
						*pMasterError = S_FALSE;
					}
				}
				return GB_RET_CACHE;
			}
			else
			{
				memcpy(pQualities, p_q, sizeof(WORD) * dwCount);
				memcpy(pTimestamps, p_t, sizeof(::FILETIME) * dwCount);
				memcpy(pErrors, p_e, sizeof(HRESULT) * dwCount);
				for (unsigned i = 0; i<dwCount; i++)
				{
					if (!pTags[i].dwTagID) continue;
					if FAILED(pErrors[i]) continue;
					Marshal::GetNativeVariantForObject(e->Values[i], IntPtr((int)&pValues[i]));
					VARTYPE vt = pRequestedTypes[i];
					if (vt == pValues[i].vt) continue;
					HRESULT hr = VariantChangeTypeEx(&pValues[i], &pValues[i], dwLcid, 0, vt);
					if FAILED(hr)
					{
						pErrors[i] = hr;
						*pMasterError = S_FALSE;
					}
				}
				return GB_RET_ARG;
			}
		}
		return GB_RET_CACHE;
	}

	HRESULT __stdcall GBOnBrowseAccessPath(
		LPCWSTR szItemID,
		DWORD* pdwAccessPathCount,
		LPWSTR** ppszAccessPaths,
		DWORD dwClientID)
	{
		if (!GetEventFlag(EventIdx_BrowseAccessPaths))
			return __super::GBOnBrowseAccessPath(szItemID, pdwAccessPathCount, ppszAccessPaths, dwClientID);
		
		BrowseAccessPathsArgs^ e = gcnew BrowseAccessPathsArgs(
			Marshal::PtrToStringUni(IntPtr((void*)szItemID)), dwClientID);
		m_events->BrowseAccessPaths(m_gbda_clr, e);
		HRESULT hr = (HRESULT)e->EventHandlingError;
		if FAILED(hr) return hr;
		int count = e->AccessPaths == nullptr ? 0 : e->AccessPaths->Length;
		*pdwAccessPathCount = (DWORD)count;
		if (!count) return S_FALSE;
		LPWSTR* paths = (LPWSTR*)LocalAlloc(0, sizeof(LPWSTR) * count);
		for (int i = 0; i < count; i++)
			paths[i] = (LPWSTR)Marshal::StringToHGlobalUni(e->AccessPaths[i]).ToPointer();
		*ppszAccessPaths = paths;
		return hr;
	}

	HRESULT __stdcall GBOnQueryItem(
		LPCWSTR szItemID,
		LPCWSTR szAccessPath,
		VARTYPE wDataType,
		BOOL bAddItem,
		DWORD* pdwTagID,
		DWORD* pdwAccessPathID,
		DWORD dwClientID)
	{
		if (!GetEventFlag(EventIdx_QueryItem))
			return __super::GBOnQueryItem(szItemID, szAccessPath, wDataType,
				bAddItem, pdwTagID, pdwAccessPathID, dwClientID);
		
		QueryItemArgs^ e = gcnew QueryItemArgs(
			Marshal::PtrToStringUni(IntPtr((void*)szItemID)),
			Marshal::PtrToStringUni(IntPtr((void*)szAccessPath)),
			wDataType,
			bAddItem == TRUE? true : false,
			dwClientID);
		m_events->QueryItem(m_gbda_clr, e);
		*pdwTagID = e->TagId;
		if (pdwAccessPathID) *pdwAccessPathID = e->AccessPathId;
		return (HRESULT)e->EventHandlingError;
	}

	HRESULT __stdcall GBOnGetProperties(
		GBItemID* pTag,
		DWORD dwCount,
		DWORD *pdwPropIDs,
		VARIANT *pvValues,
		HRESULT *pdwErrors,
		LCID dwLcid,
		DWORD dwClientID)
	{
		if (!GetEventFlag(EventIdx_ReadProperties))
			return __super::GBOnGetProperties(pTag, dwCount, pdwPropIDs,
				pvValues, pdwErrors, dwLcid, dwClientID);
		
		ItemId tag;
		pin_ptr<ItemId> p_tag = &tag;
		memcpy(p_tag, pTag, sizeof(GBItemID));

		array<Int32>^ props = gcnew array<Int32>(dwCount);
		pin_ptr<Int32> p_props = &props[0];
		memcpy(p_props, pdwPropIDs, sizeof(DWORD) * dwCount);

		array<ErrorCodes>^ err = gcnew array<ErrorCodes>(dwCount);
		pin_ptr<ErrorCodes> p_err = &err[0];
		memcpy(p_err, pdwErrors, sizeof(HRESULT) * dwCount);

		ReadPropertiesArgs^ e = gcnew ReadPropertiesArgs(
			tag,
			props,
			err,
			dwLcid,
			dwClientID);
		m_events->ReadProperties(m_gbda_clr, e);
		HRESULT hr = (HRESULT)e->EventHandlingError;
		if FAILED(hr) return hr;

		memcpy(pdwErrors, p_err, sizeof(HRESULT) * dwCount);

		for (unsigned i = 0; i<dwCount; i++)
		{
			if FAILED(pdwErrors[i]) continue;
			Marshal::GetNativeVariantForObject(e->Values[i], IntPtr(&pvValues[i]));
		}

		return hr;
	}

	void __stdcall GBOnActivate(DWORD dwCount, GBItemID* pTags)
	{
		if (!GetEventFlag(EventIdx_ActivateItems)) return;
		array<ItemId, 1>^ tags = gcnew array<ItemId, 1>(dwCount);
		pin_ptr<ItemId> p = &tags[0];
		memcpy(p, pTags, sizeof(GBItemID)*dwCount);
		ActivateItemsArgs^ e = gcnew ActivateItemsArgs(tags);
		m_events->ActivateItems(m_gbda_clr, e);
	}

	void __stdcall GBOnDeactivate(DWORD dwCount, GBItemID* pTags)
	{
		if (!GetEventFlag(EventIdx_DeactivateItems)) return;
		array<ItemId, 1>^ tags = gcnew array<ItemId, 1>(dwCount);
		pin_ptr<ItemId> p = &tags[0];
		memcpy(p, pTags, sizeof(GBItemID)*dwCount);
		DeactivateItemsArgs^ e = gcnew DeactivateItemsArgs(tags);
		m_events->DeactivateItems(m_gbda_clr, e);
	}

	HRESULT __stdcall GBOnGetErrorString(
		HRESULT dwError,
		LCID dwLcid,
		LPWSTR* pszErrorString,
		DWORD dwCallerID)
	{
		if (!GetEventFlag(EventIdx_GetErrorString))
			return __super::GBOnGetErrorString(dwError, dwLcid, pszErrorString, dwCallerID);
		GetErrorStringArgs^ e = gcnew GetErrorStringArgs(
			(Int32)dwError,
			(Int32)dwLcid,
			(Int32)dwCallerID);
		m_events->GetErrorString(m_gbda_clr, e);
		if SUCCEEDED((int)e->EventHandlingError)
		{
			if (e->RequestedErrorString == nullptr)
			{
				return E_FAIL;
			}
			IntPtr ptr = Marshal::StringToHGlobalUni(e->RequestedErrorString);
			*pszErrorString = (LPWSTR)ptr.ToPointer();
		}
		return (HRESULT)e->EventHandlingError;
	}

	HRESULT __stdcall GBOnQueryLocales(DWORD* pdwCount, LCID** ppdwLcid)
	{
		if (!GetEventFlag(EventIdx_QueryLocales))
			return __super::GBOnQueryLocales(pdwCount, ppdwLcid);
		QueryLocalesArgs^ e = gcnew QueryLocalesArgs(nullptr);
		m_events->QueryLocales(m_gbda_clr, e);
		if ( SUCCEEDED((int)e->EventHandlingError) && e->Cultures != nullptr )
		{
			*pdwCount = (DWORD)e->Cultures->Count + 3;
			*ppdwLcid = (LCID*)e->Cultures->Unmanaged.ToPointer();
		}
		else
		{
			*pdwCount = 0;
			*ppdwLcid = NULL;
		}
		return (HRESULT)e->EventHandlingError;
	}


	HRESULT __stdcall GBGetItems_Caller(
		DWORD dwCount,
		DWORD* pdwTagIDs,
		VARIANT* pvValues,
		WORD* pwQualities,
		::FILETIME* pftTimeStamps,
		HRESULT* pdwErrors)
	{
		memset(pvValues, 0, sizeof(VARIANT)*dwCount);
		return GBGetItems(dwCount, pdwTagIDs, pvValues, pwQualities, pftTimeStamps, pdwErrors);
	}

	void __stdcall GBGetItems_Cleanup(
		DWORD dwCount,
		VARIANT* pvValues)
	{
		for (unsigned i = 0; i<dwCount; i++) VariantClear(pvValues + i);
		LocalFree(pvValues);
	}


	HRESULT __stdcall GBUpdateItems_Caller(
		DWORD dwCount,
		DWORD* pdwItemID,
		VARIANT* pvValues,
		WORD* pwQualites,
		::FILETIME* pftTimestamps,
		HRESULT* pdwErrors,
		BOOL bWait)
	{
		HRESULT hr;
		if (pvValues)
		{
			hr = GBUpdateItems(dwCount, pdwItemID, pvValues, pwQualites, pftTimestamps, pdwErrors, bWait);
			for (dwCount--; (Int32)dwCount >= 0; dwCount--, pvValues++) VariantClear(pvValues);
		}
		else
		{
			pvValues = (VARIANT*)LocalAlloc(LMEM_ZEROINIT, sizeof(VARIANT)*dwCount);
			hr = GBUpdateItems(dwCount, pdwItemID, pvValues, pwQualites, pftTimestamps, pdwErrors, bWait);
		}
		LocalFree(pvValues);
		return hr;
	}

	HRESULT __stdcall GBSetItem_Caller(
		DWORD dwTagID,
		VARIANT* pvValue,
		WORD wQuality,
		::FILETIME* pftTimestamp,
		HRESULT dwError)
	{
		HRESULT hr = GBSetItem(dwTagID, pvValue, wQuality, pftTimestamp, dwError);
		VariantClear(pvValue);
		return hr;
	}

	HRESULT __stdcall GBAddProperty_Caller(
		DWORD dwTagID,
		DWORD dwPropID,
		VARIANT* pvValue,
		LPCWSTR szDescription,
		LPCWSTR szOpcItemID,
		DWORD dwFlags)
	{
		HRESULT hr = GBAddProperty(dwTagID, dwPropID, pvValue, szDescription, szOpcItemID, dwFlags);
		VariantClear(pvValue);
		return hr;
	}

	HRESULT __stdcall GBSetProperty_Caller(
		DWORD dwTagID,
		DWORD dwPropID,
		VARIANT* pvValue)
	{
		HRESULT hr = GBSetProperty(dwTagID, dwPropID, pvValue);
		VariantClear(pvValue);
		return hr;
	}
};

/// <summary>
/// GBDataAccess descendant. Native toolkit out-of-proc OPC server class.
/// </summary>
class GBDataAccessNative :
	public GBDataAccessNativeDecendant<GBDataAccess>
{
public:
	GBDataAccessNative(System::Object^ gbda_clr) : GBDataAccessNativeDecendant<GBDataAccess>(gbda_clr)
	{
	}
};

/// <summary>
/// GBDataAccessInproc descendant. Native toolkit in-proc OPC server class.
/// </summary>
class GBDataAccessInprocNative :
	public GBDataAccessNativeDecendant<GBDataAccessInproc>
{
public:
	GBDataAccessInprocNative(System::Object^ gbda_clr) : GBDataAccessNativeDecendant<GBDataAccessInproc>(gbda_clr)
	{
	}
};

