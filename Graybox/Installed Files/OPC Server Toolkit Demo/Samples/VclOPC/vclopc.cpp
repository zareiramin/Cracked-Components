//---------------------------------------------------------------------------
#pragma comment (lib, "gbda3_omf.lib")
#include <vcl.h>
#pragma hdrstop
#include <GB_OPCDA.h>
#include "vclopc_form.h"
#include "vclopc_init.h"
//---------------------------------------------------------------------------
USEFORM("vclopc_form.cpp", Form1);
//---------------------------------------------------------------------------
extern GUID guid;
TInitThread* init;
//---------------------------------------------------------------------------
WINAPI WinMain(HINSTANCE, HINSTANCE, LPSTR lpCmdLine, int)
{
	char *token;
	if ( (token = strchr(lpCmdLine, '-')) == NULL ) token = strchr(lpCmdLine, '/');
	if (token)
	{
		token++;
		if ( !lstrcmpiA(token, "regserver") || !lstrcmpiA(token, "r") )
		{
			GBDataAccess::GBRegisterServer(&guid, L"Graybox",
				L"Graybox OPC Server Toolkit Sample Server",
				L"Graybox.Sample.VclOPC", L"1.0");
			return 0;
		}
		if ( !lstrcmpiA(token, "unregserver") || !lstrcmpiA(token, "u") )
		{
			GBDataAccess::GBUnregisterServer(&guid);
			return 0;
		}
	}

	CoInitializeEx(0,0);
	Application->Initialize();
	Application->Title = "VclOpc";
                 Application->CreateForm(__classid(TForm1), &Form1);
        init = new TInitThread(false);
	Application->Run();
	CoUninitialize();
	return 0;
}
//---------------------------------------------------------------------------
