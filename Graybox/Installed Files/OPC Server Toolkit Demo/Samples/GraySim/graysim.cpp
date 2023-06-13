// Gray Simulator.cpp : Implementation of WinMain

#include "stdafx.h"
#include "resource.h"
#include "tagdescr.h"
#include "graysim.h"
#include <GB_OPCDA.h>
#include <stdio.h>
#define _USE_MATH_DEFINES
#include <cmath>

#define SCANRATE 50
#define TAGDESCRCOUNT (sizeof(TagDescr)/sizeof(TagDescr[0]))

class CGraySimulatorModule :
	public CAtlServiceModuleT< CGraySimulatorModule, IDS_SERVICENAME >,
	public GBDataAccess
{
private:
	unsigned m_uTagCount;
	DWORD m_puTagIds[TAGDESCRCOUNT];
	bool m_pbActive[TAGDESCRCOUNT];
	int m_iActive;
	bool m_bPaused;
	double m_fSawFreq;
	double m_fSinFreq;
	double m_fTriFreq;
	double m_fSqrFreq;
	CRITICAL_SECTION m_crit;

public:
	DECLARE_LIBID(LIBID_GRAYSIMLIB)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_GRAYSIMULATOR, "{E0FECBC8-8E39-4b37-8629-A01E471FC5BB}")
	HRESULT InitializeSecurity() throw()
	{
		return CoInitializeSecurity(NULL, -1, NULL, NULL,
			RPC_C_AUTHN_LEVEL_PKT, RPC_C_IMP_LEVEL_IDENTIFY, NULL, EOAC_NONE, NULL);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	static HRESULT InitializeCom( ) throw( )
	{
		return CoInitializeEx(NULL, COINIT_MULTITHREADED);		// COM must be initialized with COINIT_MULTITHREADED
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	HRESULT PreMessageLoop(int nShowCmd) throw( )
	{
		HRESULT ret;
		CLSID clsid;
		if FAILED( ret = CAtlServiceModuleT::PreMessageLoop(nShowCmd) ) return ret;
		CLSIDFromString((LPOLESTR)GetAppId(), &clsid);
		if FAILED( ret = GBInitialize(&clsid, SCANRATE, SCANRATE, GB_SRV_NOACCESSPATH, L'.',100) ) return ret;
		if FAILED( ret = Initialize() ) return ret;
		if FAILED( ret = GBRegisterClassObject() ) return ret;
		if (m_bService)
		{
			m_status.dwControlsAccepted = SERVICE_ACCEPT_STOP | SERVICE_ACCEPT_PAUSE_CONTINUE;
			SetServiceStatus(SERVICE_RUNNING);
		}
		return S_OK;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void RunMessageLoop( ) throw( )
	{
		MSG msg;
		BOOL bMsg;
		while (1)
		{
			MsgWaitForMultipleObjects(0, NULL, FALSE, SCANRATE, QS_ALLINPUT);
			bMsg = PeekMessage(&msg, NULL, 0, 0, PM_REMOVE);
			if (bMsg)
			{
				if (msg.message == WM_QUIT) break;
				TranslateMessage(&msg); 
				DispatchMessage(&msg);
			}
			else
			{
				Update();
			}
		};
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void OnPause( ) throw( )
	{
		SetServiceStatus(SERVICE_PAUSE_PENDING);
		EnterCriticalSection(&m_crit);
		m_bPaused = true;
		LeaveCriticalSection(&m_crit);
		GBSetState(OPC_STATUS_SUSPENDED);
		SetServiceStatus(SERVICE_PAUSED);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void OnContinue( ) throw( )
	{
		SetServiceStatus(SERVICE_CONTINUE_PENDING);
		EnterCriticalSection(&m_crit);
		m_bPaused = false;
		LeaveCriticalSection(&m_crit);
		GBSetState(OPC_STATUS_RUNNING);
		SetServiceStatus(SERVICE_RUNNING);
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	HRESULT PostMessageLoop( ) throw( )
	{
		GBRevokeClassObject();
		return CAtlServiceModuleT::PostMessageLoop();
	}


private:			
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	inline HRESULT Initialize()
	{
		unsigned i;
		m_fSawFreq = 0.05;
		m_fSinFreq = 0.05;
		m_fTriFreq = 0.05;
		m_fSqrFreq = 0.05;
		m_bPaused = false;
		m_iActive = 0;
		memset(m_pbActive, 0, sizeof(m_pbActive));
		InitializeCriticalSection(&m_crit);

		// Create tags
		for (i = 0; i<TAGDESCRCOUNT; i++)
		{
			VARIANT v;
			memset(&v, 0, sizeof(v));
			v.vt = TagDescr[i].Type;
			if (v.vt == VT_BSTR) v.bstrVal = SysAllocString(L"");
			if ((TagDescr[i].Flags & GBS_ALG_MASK) == GBS_ECOLOR)
			{
				GBCreateItemEnum(&m_puTagIds[i], i, TagDescr[i].Name, TagDescr[i].Rights, GB_TAG_DONTCOPYSTR,
					&v, sizeof(TagColor)/sizeof(TagColor[0]), TagColor);
			}
			else if ((TagDescr[i].Flags & GBS_ALG_MASK) == GBS_ENUMBER)
			{
				GBCreateItemEnum(&m_puTagIds[i], i, TagDescr[i].Name, TagDescr[i].Rights, GB_TAG_DONTCOPYSTR,
					&v, sizeof(TagNumber)/sizeof(TagNumber[0]), TagNumber);
			}
			else if ((TagDescr[i].Flags & GBS_ALG_MASK) == GBS_EWEEK)
			{
				GBCreateItemEnum(&m_puTagIds[i], i, TagDescr[i].Name, TagDescr[i].Rights, GB_TAG_DONTCOPYSTR,
					&v, sizeof(TagWeekday)/sizeof(TagWeekday[0]), TagWeekday);
			}
			else if (TagDescr[i].Min != TagDescr[i].Max)
			{
				GBCreateItemAnalog(&m_puTagIds[i], i, TagDescr[i].Name, TagDescr[i].Rights, GB_TAG_DONTCOPYSTR,
					&v, TagDescr[i].Min, TagDescr[i].Max);
				v.vt = VT_R8;
				v.dblVal = TagDescr[i].Min;
				GBAddProperty(m_puTagIds[i], OPC_PROPERTY_LO_LIMIT, &v, NULL, NULL, 0);
				v.dblVal = TagDescr[i].Max;
				GBAddProperty(m_puTagIds[i], OPC_PROPERTY_HI_LIMIT, &v, NULL, NULL, 0);
			}
			else
				GBCreateItem(&m_puTagIds[i], i, TagDescr[i].Name, TagDescr[i].Rights, GB_TAG_DONTCOPYSTR, &v);
			VariantClear(&v);
			v.vt = VT_BSTR;
			v.bstrVal = SysAllocString(TagDescr[i].Descr);
			GBAddProperty(m_puTagIds[i], OPC_PROPERTY_DESCRIPTION, &v, NULL, NULL, 0);
			VariantClear(&v);

			switch (TagDescr[i].Flags & GBS_PROP_MASK)
			{
			case GBS_FREQ: 
				switch (TagDescr[i].Flags & GBS_ALG_MASK)
				{
				case GBS_SAW:
					v.vt = VT_R8;
					v.dblVal = m_fSawFreq;
					GBAddProperty(m_puTagIds[i], 5000, &v, L"Frequency", L"options.sawfreq", 0);
					break;
				case GBS_SIN:
					v.vt = VT_R8;
					v.dblVal = m_fSinFreq;
					GBAddProperty(m_puTagIds[i], 5000, &v, L"Frequency", L"options.sinfreq", 0);
					break;
				case GBS_TRI:
					v.vt = VT_R8;
					v.dblVal = m_fTriFreq;
					GBAddProperty(m_puTagIds[i], 5000, &v, L"Frequency", L"options.trianglefreq", 0);
					break;
				case GBS_SQR:
					v.vt = VT_R8;
					v.dblVal = m_fSqrFreq;
					GBAddProperty(m_puTagIds[i], 5000, &v, L"Frequency", L"options.squarefreq", 0);
					break;
				}
				break;
			case GBS_EU:
				v.vt = VT_BSTR;
				v.bstrVal = SysAllocString(L"Hertz");
				GBAddProperty(m_puTagIds[i], OPC_PROPERTY_EU_UNITS, &v, NULL, NULL, 0);
				VariantClear(&v);
				break;
			}
		}
		return S_OK;
	}
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	inline void Update()
	{
		unsigned i, c = 0;
		int k, l;
		VARIANT Value[TAGDESCRCOUNT];
		WORD Quality[TAGDESCRCOUNT];
		FILETIME Timestamp[TAGDESCRCOUNT], ft;
		HRESULT Error[TAGDESCRCOUNT];
		DWORD Id[TAGDESCRCOUNT];
		wchar_t rndtxt[32];
		static wchar_t rndchars[] = L"0123456789QWERTYUIOPASDFGHJKLZXCVBNM";
		double t, A, n, T;
		bool clear = false;
		EnterCriticalSection(&m_crit);
		if (m_iActive <= 0 || m_bPaused) goto Skip;
		GetSystemTimeAsFileTime(&ft);
		t = (double)GetTickCount() / 1000.0;
		for (i = 0; i<TAGDESCRCOUNT; i++)
		{
			if (!m_pbActive[i]) continue;
			switch (TagDescr[i].Flags & GBS_FUNC_MASK)
			{
			case GBS_SAW:
				A = abs(TagDescr[i].Max - TagDescr[i].Min);
				Value[c].dblVal = fmod(A*m_fSawFreq*t, A) - abs(TagDescr[i].Min);
				Value[c].vt = VT_R8;
				VariantChangeType(&Value[c], &Value[c], 0, TagDescr[i].Type);
				break;
			case GBS_SIN:
				A = (double)abs(TagDescr[i].Max - TagDescr[i].Min) / 2.0;
				Value[c].dblVal = sin(2*M_PI*m_fSinFreq*t)*A + TagDescr[i].Max - A;
				Value[c].vt = VT_R8;
				VariantChangeType(&Value[c], &Value[c], 0, TagDescr[i].Type);
				break;
			case GBS_TRI:
				A = abs(TagDescr[i].Max - TagDescr[i].Min);
				n = fmod(A*m_fTriFreq*t, A);
				T = 1/m_fTriFreq;
				Value[c].dblVal = fmod(t, T) > T ? n - abs(TagDescr[i].Min) : TagDescr[i].Max - n;
				Value[c].vt = VT_R8;
				VariantChangeType(&Value[c], &Value[c], 0, TagDescr[i].Type);
				break;
			case GBS_SQR:
				T = 1/m_fSqrFreq;
				if (TagDescr[i].Type == VT_BOOL)
				{
					Value[c].boolVal = fmod(t, T) > T ? VARIANT_FALSE : VARIANT_TRUE;
					Value[c].vt = VT_BOOL;
				}
				else
				{
					Value[c].dblVal = fmod(t, 2*T) > T ? TagDescr[i].Min : TagDescr[i].Max;
					Value[c].vt = VT_R8;
					VariantChangeType(&Value[c], &Value[c], 0, TagDescr[i].Type);
				}
				break;
			case GBS_RND:
				Value[c].vt = VT_R8;
				Value[c].dblVal = (double)rand()/10.0;
				VariantChangeType(&Value[c], &Value[c], 0, TagDescr[i].Type);
				break;
			case GBS_TXTRND:
				l = (rand() & 0xf) + 10;
				for (k = 0; k<l; k++) rndtxt[k] = rndchars[rand() % (sizeof(rndchars)/sizeof(rndchars[0]) - 1)];
				rndtxt[k] = 0;
				Value[c].vt = VT_BSTR;
				Value[c].bstrVal = SysAllocString(rndtxt);
				clear = true;
				break;
			case GBS_TXTCOLOR:
				Value[c].vt = VT_BSTR;
				Value[c].bstrVal = SysAllocString( TagColor[ rand() % (sizeof(TagColor)/sizeof(TagColor[0])) ] );
				clear = true;
				break;
			case GBS_TXTWEEK:
				Value[c].vt = VT_BSTR;
				Value[c].bstrVal = SysAllocString( TagWeekday[ rand() % (sizeof(TagWeekday)/sizeof(TagWeekday[0])) ] );
				clear = true;
				break;
			case GBS_TXTNUMBER:
				Value[c].vt = VT_BSTR;
				Value[c].bstrVal = SysAllocString( TagNumber[ rand() % (sizeof(TagNumber)/sizeof(TagNumber[0])) ] );
				clear = true;
				break;
			case GBS_CURTIME:
				Value[c].vt = VT_DATE;
				Value[c].date = (-109205. * (24. * 3600. * 1e7)	+ ft.dwHighDateTime * 4294967296. + ft.dwLowDateTime) / (24. * 3600. * 1e7);
				break;
			case GBS_RNDTIME:
				Value[c].vt = VT_DATE;
				Value[c].date = (double)rand() + (1.0/RAND_MAX)*(double)rand();
				break;
			case GBS_BAND:
				if FAILED(GBGetBandwidth(&Value[c].ulVal)) Value[c].ulVal = 1000;
				Value[c].vt = VT_UI4;
				break;
			case GBS_ECOLOR:
				Value[c].vt = VT_I4;
				Value[c].lVal = rand() % (sizeof(TagColor)/sizeof(TagColor[0]));
				break;
			case GBS_ENUMBER:
				Value[c].vt = VT_I4;
				Value[c].lVal = rand() % (sizeof(TagNumber)/sizeof(TagNumber[0]));
				break;
			case GBS_EWEEK:
				Value[c].vt = VT_I4;
				Value[c].lVal = rand() % (sizeof(TagWeekday)/sizeof(TagWeekday[0]));
				break;
			case GBS_SAW|GBS_OPTFREQ:
				Value[c].vt = VT_R8;
				Value[c].dblVal = m_fSawFreq;
				break;
			case GBS_SIN|GBS_OPTFREQ:
				Value[c].vt = VT_R8;
				Value[c].dblVal = m_fSinFreq;
				break;
			case GBS_SQR|GBS_OPTFREQ:
				Value[c].vt = VT_R8;
				Value[c].dblVal = m_fSqrFreq;
				break;
			case GBS_TRI|GBS_OPTFREQ:
				Value[c].vt = VT_R8;
				Value[c].dblVal = m_fTriFreq;
				break;
			default: continue;
			}
			Id[c] = m_puTagIds[i];
			Error[c] = S_OK;
			Timestamp[c] = ft;
			Quality[c] = OPC_QUALITY_GOOD;
			c++;
		}
Skip:
		LeaveCriticalSection(&m_crit);
		if (c)
		{
			GBUpdateItems(c, Id, Value, Quality, Timestamp, Error, FALSE);
			if (clear) for (i = 0; i<c; i++) VariantClear(&Value[i]);
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Graybox Events
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	// Following four handlers tie together ATL-module and Graybox OPC-server module.
	void __stdcall GBOnCreateInstance(DWORD dwClientId) { Lock(); }
	void __stdcall GBOnDestroyInstance(DWORD dwClientId) { Unlock(); }
	void __stdcall GBOnLock() { Lock(); }
	void __stdcall GBOnUnLock() { Unlock(); }
	HRESULT __stdcall GBOnQueryLocales(
			DWORD* pdwCount,
			LCID** ppdwLcid)
	{
		static const LCID deflcid[7] = {
			MAKELCID( MAKELANGID(LANG_NEUTRAL, SUBLANG_NEUTRAL), SORT_DEFAULT ),
			MAKELCID( MAKELANGID(LANG_ENGLISH, SUBLANG_NEUTRAL), SORT_DEFAULT ),
			LOCALE_SYSTEM_DEFAULT,
			MAKELCID( MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), SORT_DEFAULT ),	//1024
			MAKELCID( MAKELANGID(LANG_ENGLISH, SUBLANG_DEFAULT), SORT_DEFAULT ),
			MAKELCID( MAKELANGID(LANG_RUSSIAN, SUBLANG_DEFAULT), SORT_DEFAULT ),	//1049
			MAKELCID( MAKELANGID(LANG_RUSSIAN, SUBLANG_NEUTRAL), SORT_DEFAULT )
		};
		*ppdwLcid = (LCID*)&deflcid;
		*pdwCount = 7;
		return S_OK;
	}
	HRESULT __stdcall GBOnServerReleased()
	{
		if (m_bService) PostThreadMessage(m_dwThreadID, WM_QUIT, 0, 0);
		else PostThreadMessage(m_dwMainThreadID, WM_QUIT, 0, 0);
		return S_FALSE;
	}

	HRESULT __stdcall GBOnGetProperties(
		GBItemID* pTag,
		DWORD dwCount,
		DWORD *pdwPropertyIDs,
		VARIANT *ppvData,
		HRESULT *ppErrors,
		LCID dwLcid,
		DWORD dwClientID)
	{
		if (!pTag->dwTagID) return S_FALSE;
		for (unsigned i = 0; i<dwCount; i++)
		{
			switch (pdwPropertyIDs[i])
			{
			case 5000:	// frequency property
				EnterCriticalSection(&m_crit);
				switch (TagDescr[pTag->dwUserID].Flags & GBS_ALG_MASK)
				{
				case GBS_SAW:
					ppvData[i].vt = VT_R8;
					ppvData[i].dblVal = m_fSawFreq;
					ppErrors[i] = S_OK;
					break;
				case GBS_SIN:
					ppvData[i].vt = VT_R8;
					ppvData[i].dblVal = m_fSinFreq;
					ppErrors[i] = S_OK;
					break;
				case GBS_TRI:
					ppvData[i].vt = VT_R8;
					ppvData[i].dblVal = m_fTriFreq;
					ppErrors[i] = S_OK;
					break;
				case GBS_SQR:
					ppvData[i].vt = VT_R8;
					ppvData[i].dblVal = m_fSqrFreq;
					ppErrors[i] = S_OK;
					break;
				}
				LeaveCriticalSection(&m_crit);
				break;
			}
		}
		return S_FALSE;
	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	DWORD __stdcall GBOnWriteItems(
		DWORD dwCount,
		GBItemID* pTags,
		VARIANT* pValues,
		WORD* pwQualities,
		FILETIME* pftTimestamps,
		HRESULT* pErrors,
		HRESULT* pMasterError,
		LCID dwLcid,
		DWORD dwClientID)
	{
		VARIANT v;
		HRESULT hr;
		for (unsigned i = 0; i<dwCount; i++)
		{
			if (!pTags[i].dwTagID) continue;
			switch (TagDescr[pTags[i].dwUserID].Flags & GBS_ATTR_MASK)
			{
			case GBS_OPTFREQ:
				v.vt = VT_EMPTY;
				hr = VariantChangeTypeEx(&v, &pValues[i], dwLcid, 0, VT_R8);
				if SUCCEEDED(hr)
				{
					if (v.dblVal>0)
					{
						EnterCriticalSection(&m_crit);
						switch (TagDescr[pTags[i].dwUserID].Flags & GBS_ALG_MASK)
						{
						case GBS_SAW: m_fSawFreq = v.dblVal; break;
						case GBS_SIN: m_fSinFreq = v.dblVal; break;
						case GBS_TRI: m_fTriFreq = v.dblVal; break;
						case GBS_SQR: m_fSqrFreq = v.dblVal; break;
						}
						LeaveCriticalSection(&m_crit);
					}
					else hr = E_FAIL;
				}
				if FAILED(hr)
				{
					pErrors[i] = hr;
					*pMasterError = S_FALSE;
				}
			}
		}
		return GB_RET_CACHE;
	}

	void Activate(DWORD dwCount, GBItemID* pTags, bool bActive)
	{
		EnterCriticalSection(&m_crit);
		if (bActive) m_iActive++; else m_iActive--;
		for (unsigned i = 0; i<dwCount; i++) m_pbActive[pTags[i].dwUserID] = bActive;
		LeaveCriticalSection(&m_crit);
	}
	void __stdcall GBOnActivate(DWORD dwCount, GBItemID* pTags)
	{
		Activate(dwCount, pTags, true);
	}
    void __stdcall GBOnDeactivate(DWORD dwCount, GBItemID* pTags)
	{
		Activate(dwCount, pTags, false);
	}
};

CGraySimulatorModule _AtlModule;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
extern "C" int WINAPI _tWinMain(HINSTANCE /*hInstance*/, HINSTANCE /*hPrevInstance*/, 
                                LPTSTR /*lpCmdLine*/, int nShowCmd)
{
    return _AtlModule.WinMain(nShowCmd);
}

