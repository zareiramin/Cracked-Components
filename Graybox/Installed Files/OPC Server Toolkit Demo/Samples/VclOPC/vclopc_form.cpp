//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "vclopc_form.h"
#include <GB_OPCDA.h>
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"
TForm1 *Form1;

// {4B4F633E-B176-4ddf-A243-62651756F2B8}
GUID guid =
{ 0x4b4f633e, 0xb176, 0x4ddf, { 0xa2, 0x43, 0x62, 0x65, 0x17, 0x56, 0xf2, 0xb8 } };

//---------------------------------------------------------------------------
class COPCServer : public GBDataAccess
{
public:
	COPCServer()
	{
		m_Locks = 0;
		m_Clients = 0;
	}
	HRESULT __stdcall GBOnServerReleased()
	{
		Form1->ListBoxLog->Items->Insert(0, "GBOnServerReleased");
		return S_OK;
	}
	HRESULT __stdcall GBOnBeforeCreateInstance(BOOL bAggregating, DWORD* pdwClientId)
	{
		Form1->ListBoxLog->Items->Insert(0, "GBOnBeforeCreateInstance");
		return S_OK;
	}
	void __stdcall GBOnCreateInstance(DWORD dwClientId)
	{
		Form1->ListBoxLog->Items->Insert(0, "GBOnCreateInstance");
		LONG l = InterlockedIncrement(&m_Clients);
		Form1->StaticTextClients->Caption = IntToStr(l);
	}
	void __stdcall GBOnDestroyInstance(DWORD dwClientId)
	{
		Form1->ListBoxLog->Items->Insert(0, "GBOnDestroyInstance");
		LONG l = InterlockedDecrement(&m_Clients);
		Form1->StaticTextClients->Caption = IntToStr(l);
	}
	void __stdcall GBOnLock()
	{
		Form1->ListBoxLog->Items->Insert(0, "GBOnLock");
		LONG l = InterlockedIncrement(&m_Locks);
		Form1->StaticTextLocks->Caption = IntToStr(l);
	}
	void __stdcall GBOnUnLock()
	{
		Form1->ListBoxLog->Items->Insert(0, "GBOnUnLock");
		LONG l = InterlockedDecrement(&m_Locks);
		Form1->StaticTextLocks->Caption = IntToStr(l);
	}
	void __stdcall GBOnActivate(DWORD dwCount, GBItemID* pTags)
	{
		Form1->ListBoxLog->Items->Insert(0, "GBOnActivate");
		Activate(dwCount, pTags, "yes");
	}
	void __stdcall GBOnDeactivate(DWORD dwCount, GBItemID* pTags)
	{
		Form1->ListBoxLog->Items->Insert(0, "GBOnDeactivate");
		Activate(dwCount, pTags, "no");
	}
	HRESULT __stdcall GBOnQueryLocales(DWORD* pdwCount, LCID** ppdwLcid)
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
	void Activate(DWORD dwCount, GBItemID* pTags, char* newstatus)
	{
		for (unsigned i = 0; i < dwCount; i++)
		{
			if (!pTags[i].dwTagID) continue;
			Form1->StringGridTags->Cells[1][pTags[i].dwUserID+1] = newstatus;
		}
	}
	DWORD __stdcall GBOnReadItems(
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
		Form1->ListBoxLog->Items->Insert(0, "GBOnReadItems");
		if (!pValues)	// if pValues is NULL then GBOnReadItems was invoked in Refresh operation. must return GR_RET_CACHE
		{
			GBBeginUpdate();
			VARIANT val;
			FILETIME ft;
			val.vt = VT_BSTR;
			for (unsigned i = 0; i < dwCount; i++)
			{
				if (!pTags[i].dwTagID) continue;
				val.bstrVal = SysAllocString( (wchar_t*)WideString(Form1->StringGridTags->Cells[2][pTags[i].dwUserID+1]) );
				GetSystemTimeAsFileTime(&ft);
				GBSetItem(pTags[i].dwTagID, &val, OPC_QUALITY_GOOD, &ft, S_OK);
				SysFreeString(val.bstrVal);
			}
			GBEndUpdate(true);   // bWait is true - must wait for update to complete
			return GB_RET_CACHE;
		}
		// non-Refresh. we can return GB_RET_ARG
		for (unsigned i = 0; i < dwCount; i++)
		{
			if (!pTags[i].dwTagID) continue;
			pQualities[i] = OPC_QUALITY_GOOD;
			GetSystemTimeAsFileTime(&pTimestamps[i]);
			pErrors[i] = S_OK;
			pValues[i].vt = VT_BSTR;
			pValues[i].bstrVal = SysAllocString( (wchar_t*)WideString(Form1->StringGridTags->Cells[2][pTags[i].dwUserID+1]) );
		}
		return GB_RET_ARG;
	}
	DWORD __stdcall GBOnWriteItems(
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
		Form1->ListBoxLog->Items->Insert(0, "GBOnWriteItems");
		for (unsigned i = 0; i < dwCount; i++)
		{
			if (!pTags[i].dwTagID) continue;
			if SUCCEEDED(VariantChangeType(&pvValues[i], &pvValues[i], 0, VT_BSTR))
			{
				Form1->StringGridTags->Cells[2][pTags[i].dwUserID+1] = AnsiString(pvValues[i].bstrVal);
				Form1->StringGridTags->Cells[3][pTags[i].dwUserID+1] = AnsiString(pvValues[i].bstrVal);
			}
		}
		return GB_RET_CACHE;
	}
	DWORD ids[10];
	volatile LONG m_Locks;
	volatile LONG m_Clients;
} srv;

//---------------------------------------------------------------------------
__fastcall TForm1::TForm1(TComponent* Owner)
	: TForm(Owner)
{
}
//---------------------------------------------------------------------------
void __fastcall TForm1::FormCreate(TObject *Sender)
{
	StringGridTags->Cells[0][0] = "Tag";
	StringGridTags->Cells[1][0] = "Active";
	StringGridTags->Cells[2][0] = "DEVICE Value"; 
	StringGridTags->Cells[3][0] = "CACHE Value"; 
}
//---------------------------------------------------------------------------

void __fastcall TForm1::FormClose(TObject *Sender, TCloseAction &Action)
{
	if ( InterlockedExchangeAdd(&srv.m_Clients,0) ||
		 InterlockedExchangeAdd(&srv.m_Locks,0) )
	{
		Action = caNone;
		MessageBox(NULL, "VclOPC is still in use.", "VclOPC", 0);
		return;
	}
	srv.GBRevokeClassObject();
}
//---------------------------------------------------------------------------

void __fastcall TForm1::InitializeOPC()
{
	srv.GBInitialize(&guid, 125, 0, GB_SRV_NOACCESSPATH, L'\\', 100, 1, 2, 3, L"Graybox Software");

	unsigned i;
	VARIANT def_val;
	wchar_t* def_val_str = L"empty";
	def_val.vt = VT_BSTR;
	def_val.bstrVal = def_val_str;
	for (i = 0; i<10; i++)
	{
		wchar_t opcid[32];
		wsprintfW(opcid, L"Folder\\item%02i", i);
		srv.GBCreateItem(&srv.ids[i], i, opcid, OPC_READABLE|OPC_WRITEABLE, 0, &def_val);
		StringGridTags->Cells[0][i+1] = opcid;
		StringGridTags->Cells[1][i+1] = "no";
		StringGridTags->Cells[2][i+1] = def_val_str;
		StringGridTags->Cells[3][i+1] = def_val_str;
	}
	srv.GBBeginUpdate();
	srv.GBEndUpdate(TRUE);

	srv.GBRegisterClassObject();
}
//---------------------------------------------------------------------------

void __fastcall TForm1::StringGridTagsSetEditText(TObject *Sender, int ACol,
	  int ARow, const AnsiString Value)
{
	if (ACol!=3) return;
	srv.GBBeginUpdate();
	VARIANT val;
	FILETIME ft;
	val.vt = VT_BSTR;
	val.bstrVal = SysAllocString( (wchar_t*)WideString(Form1->StringGridTags->Cells[3][ARow]) );
	Form1->StringGridTags->Cells[2][ARow] = Form1->StringGridTags->Cells[3][ARow];
	GetSystemTimeAsFileTime(&ft);
	srv.GBSetItem(srv.ids[ARow-1], &val, OPC_QUALITY_GOOD, &ft, S_OK);
	srv.GBEndUpdate(false);   // bWait is false - there is no client waiting for this operation to complete
	SysFreeString(val.bstrVal);
}
//---------------------------------------------------------------------------

void __fastcall TForm1::ApplicationEvents1Idle(TObject *Sender, bool &Done)
{
	unsigned long bw;
	srv.GBGetBandwidth(&bw);
	StaticTextBand->Caption = IntToStr(bw);
}
//---------------------------------------------------------------------------

void __fastcall TForm1::ButtonShutdownClick(TObject *Sender)
{
	Form1->ListBoxLog->Items->Insert(0, "Shutdown");
	srv.GBShutdown(L"Terminated by GUI.");
	StringGridTags->Enabled = false;
}
//---------------------------------------------------------------------------

