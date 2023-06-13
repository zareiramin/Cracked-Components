//-----------------------------------------------------------------------------
//                                                                            |
//                   Softing Industrial Automation GmbH                       |
//                        Richard-Reitzner-Allee 6                            |
//                           85540 Haar, Germany                              |
//                                                                            |
//                 This is a part of the Softing OPC Toolbox                  |
//       Copyright (c) 1998 - 2012 Softing Industrial Automation GmbH         |
//                           All Rights Reserved                              |
//                                                                            |
//-----------------------------------------------------------------------------
//-----------------------------------------------------------------------------
//                          OPC TOOLBOX C++ - Samples                         |
//                                                                            |
//  Filename    : AEQuerySourceConditions.cpp                                 |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Defines the entry point for the console application         |
//                                                                            |
//  This code and information is provided "as is" without warranty of         |
//  any kind, either expressed or implied, including but not limited          |
//  to the implied warranties of merchantability and/or fitness for a         |
//  particular purpose.                                                       |
//                                                                            |
//-----------------------------------------------------------------------------

#include "ClientApplication.h"
#include "ClientCommon.h"

#include "ClientAeSession.h"
#include <conio.h>
#include <stdio.h>

using namespace std;
using namespace SoftingOPCToolboxClient;

HANDLE g_endEvent;
//-----------------------------------------------------------------------------
// controlHandler
//
// - handle console control events
//
// returns:
//		TRUE  - handled event
//		FALSE - event not handled
//
BOOL WINAPI controlHandler(
	IN unsigned long crtlType)  // control type
{
	switch (crtlType)
	{
	case CTRL_BREAK_EVENT:  // use Ctrl+C or Ctrl+Break to end server
	case CTRL_C_EVENT:
	{
		if (g_endEvent)
		{
			SetEvent(g_endEvent);
		}

		return TRUE;
	}
	break;
	}

	return FALSE;
} // controlHandler

void usage(void)
{
	_tprintf(_T("\nUsage:\n"));
	_tprintf(_T("Press \'a\' or \'A\' to asynchronously query source conditions \n"));
	_tprintf(_T("Press \'s\' or \'S\' to synchronously query source conditions \n"));
	_tprintf(_T("Press \'?\' or \'u\' to display this usage information\n"));
	_tprintf(_T("Press \'e\' or \'q\' or Ctrl-C to exit\n\n"));
}
class MyAeSession : public AeSession
{

public:

	MyAeSession(const tstring& url) : AeSession(url) {}
	unsigned char handleShutdownRequest(const tstring& reason)
	{
		_tprintf(_T("Session shutdown - reason: %s\n"), reason.c_str());
		return TRUE; // reconnect automatically
	}
	void handleQueryAeSourceConditionsCompleted(
		unsigned long executioncontext,
		tstring& sourceName,
		std::vector<tstring>& conditionNames,
		long result)
	{
		unsigned long i;

		if (SUCCEEDED(result))
		{
			for (i = 0; i < conditionNames.size(); i++)
			{
				_tprintf(_T("	[%i]%s"), i, conditionNames[i].c_str());
				_tprintf(_T("\n"));
			} //end for
		} //end if
		else
		{
			_tprintf(_T(" Failed to asynchronously query source conditions for source: %s"), sourceName.c_str());
		}//end if ... else
	} //end handleGetDaPropertiesCompleted


}; //end MySession

int _tmain(int argc, _TCHAR* argv[])
{
	long result = S_OK;
	bool end = FALSE;
	unsigned long waitRet, i;
	ExecutionOptions executionOptions;
	unsigned long executionContext;
	std::vector<tstring> conditionNames;
	tstring sourcePath = _T("computer.clock.time slot 1");
	executionOptions.setExecutionType(EnumExecutionType_SYNCHRONOUS);
	executionOptions.setExecutionContext(0);
	executionContext = executionOptions.getExecutionContext();
	Application* application = getApplication();

	//  TODO - design time license activation
	//  Fill in your design time license activation keys here
	//
	//  NOTE: you can activate one or all of the features at the same time

	//  activate the COM-AE Client Feature
	//result = getApplication()->activate(EnumFeature_AE_CLIENT, _T("XXXX-XXXX-XXXX-XXXX-XXXX"));

	if (!SUCCEEDED(result))
	{
		return FALSE;
	}

	//  END TODO - design time license activation

	if (SUCCEEDED(application->initialize()))
	{
		application->enableTracing(
			EnumTraceGroup_ALL,
			EnumTraceGroup_ALL,
			EnumTraceGroup_CLIENT,
			EnumTraceGroup_CLIENT,
			_T("Trace.txt"),
			1000000,
			2);
		MyAeSession* session = new MyAeSession(_T("opcae:///Softing.OPCToolboxDemo_ServerAE.1/{2E565243-B238-11D3-842D-0008C779D775}"));
		session->connect(TRUE, FALSE, NULL);
		usage();
		bool end = false;

		while (!end)
		{
			waitRet = WaitForSingleObject(g_endEvent, 100);

			if (waitRet == WAIT_OBJECT_0)
			{
				end = TRUE;
			}
			else
			{
				if (_kbhit())
				{
					int input;
					input = _getch();

					switch (input)
					{
					case _T('A'):
					case _T('a'):
						executionOptions.setExecutionType(EnumExecutionType_ASYNCHRONOUS);
						executionOptions.setExecutionContext(++executionContext);
						_tprintf(_T("Conditions of the source  %s are:\n"), sourcePath.c_str());
						result = session->queryAeSourceConditions(
									 sourcePath,
									 conditionNames,
									 &executionOptions);
						break;

					case _T('S'):
					case _T('s'):
						executionOptions.setExecutionType(EnumExecutionType_SYNCHRONOUS);
						_tprintf(_T("Conditions of the source  %s are:\n"), sourcePath.c_str());
						result = session->queryAeSourceConditions(
									 sourcePath,
									 conditionNames,
									 &executionOptions);

						if (SUCCEEDED(result))
						{
							for (i = 0; i < conditionNames.size(); i++)
							{
								_tprintf(_T("	[%i]%s"), i, conditionNames[i].c_str());
								_tprintf(_T("\n"));
							} //end for
						} //end if
						else
						{
							_tprintf(_T(" Failed to synchronously query the conditions of source: %s"), sourcePath.c_str());
						}

						break;

					case _T('?'):
					case _T('U'):
					case _T('u'):
						usage();
						break;

					case _T('E'):
					case _T('e'):
					case _T('Q'):
					case _T('q'):
						end = TRUE;
						break;
					} // end switch
				} // end if
			} // end else
		} // end while

		if (session->getCurrentState() != EnumObjectState_DISCONNECTED)
		{
			session->disconnect(NULL);
		}

		application->removeAeSession(session);
		application->terminate();
		releaseApplication();
		delete session;
	}

	return 0;
}

