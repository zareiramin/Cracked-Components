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
//  Filename    : Service.cpp                                                 |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Service appication template main functions                  |
//                                                                            |
//  This code and information is provided "as is" without warranty of         |
//  any kind, either expressed or implied, including but not limited          |
//  to the implied warranties of merchantability and/or fitness for a         |
//  particular purpose.                                                       |
//                                                                            |
//-----------------------------------------------------------------------------
#include "stdafx.h"


#include "OpcClient.h"

#include "Mutex.h"

#include <fstream>

typedef std::basic_ofstream<TCHAR, std::char_traits<TCHAR> > tofstream;

using namespace std;

// The following global constant holds the name of the Windows NT service that
// runs the OPC application
//  TODO : change your service name here
const tstring g_defaultServiceName = _T("DAConsole_Service OpcService");

SERVICE_STATUS g_serviceStatus = {0};
SERVICE_STATUS_HANDLE g_serviceStatusHandle =   0;
HANDLE g_serviceStopEvent   =   0;

// instance handle of application
HINSTANCE g_instance = 0;


//-----------------------------------------------------------------------------
// reportStatusToSCMgr  - service manager
//
//-----------------------------------------------------------------------------
long reportStatusToSCMgr(
	IN DWORD currentState,
	IN DWORD waitHint)
{
	static DWORD checkPoint = 1;
	g_serviceStatus.dwControlsAccepted = SERVICE_ACCEPT_STOP | SERVICE_ACCEPT_SHUTDOWN;
	g_serviceStatus.dwCurrentState = currentState;
	g_serviceStatus.dwWaitHint = waitHint;

	if ((currentState == SERVICE_RUNNING) || (currentState == SERVICE_STOPPED))
	{
		g_serviceStatus.dwCheckPoint = 0;
	}
	else
	{
		g_serviceStatus.dwCheckPoint = checkPoint++;
	} // end if...else

	// report the status of the service to the service control manager.
	return SetServiceStatus(g_serviceStatusHandle, &g_serviceStatus);
} // end reportStatusToSCMgr


//-----------------------------------------------------------------------------
// serviceControlHandler    - service manager
//
//-----------------------------------------------------------------------------
void WINAPI serviceControlHandler(IN DWORD ctrlCode)
{
	switch (ctrlCode)
	{
	case SERVICE_CONTROL_STOP:
	case SERVICE_CONTROL_SHUTDOWN:
	{
		reportStatusToSCMgr(SERVICE_STOP_PENDING, 0);

		if (g_serviceStopEvent)
		{
			::SetEvent(g_serviceStopEvent);
		} // end if
	} // end case
	break;
	} // end switch
} // end serviceControlHandler

void WINAPI ServiceMain(
	IN DWORD argc,
	IN LPTSTR* argv)
{
	g_serviceStatusHandle = ::RegisterServiceCtrlHandler(g_defaultServiceName.c_str(), serviceControlHandler);

	if (g_serviceStatusHandle)
	{
		g_serviceStatus.dwServiceType = SERVICE_WIN32_OWN_PROCESS;
		g_serviceStatus.dwServiceSpecificExitCode = 0;
		g_serviceStatus.dwWin32ExitCode = 0;
		reportStatusToSCMgr(SERVICE_START_PENDING, 1000);
		g_serviceStopEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
		BOOL started = FALSE;
		long result = S_OK;
		OpcClient* pClient = getOpcClient();
		// initialize the DA client simulation
		result |= pClient->initializeDaObjects();
		reportStatusToSCMgr(SERVICE_RUNNING, 0);
		WaitForSingleObject(g_serviceStopEvent, INFINITE);
		CloseHandle(g_serviceStopEvent);
		reportStatusToSCMgr(SERVICE_STOPPED, 0);
	} // end if
} // end ServiceMain


int APIENTRY _tWinMain(
	HINSTANCE instance,
	HINSTANCE prevInstance,
	LPTSTR cmdLine,
	int cmdShow)
{
	long result = S_OK;
	g_instance = instance;
	// create the OpcClient instance
	createOpcClient();
	OpcClient* pClient = getOpcClient();
	pClient->setServiceName(g_defaultServiceName);
	::CoInitializeEx(NULL, COINIT_MULTITHREADED);
	::CoInitializeSecurity(0, -1, 0, 0, RPC_C_AUTHN_LEVEL_NONE,
						   RPC_C_IMP_LEVEL_IDENTIFY, 0, EOAC_NONE, 0);
	BOOL commandLineProcessed = FALSE;
	tstring commandLine(GetCommandLine());
	BOOL end = FALSE;
	int ret = 0;
	SERVICE_TABLE_ENTRY dispatchTable[] =
	{
		{ (LPTSTR)g_defaultServiceName.c_str(), (LPSERVICE_MAIN_FUNCTION)ServiceMain },
		{ NULL, NULL }
	};

	// initialize the client instance
	if (!SUCCEEDED(pClient->initialize()))
	{
		pClient->terminate();
		destroyOpcClient();
		::CoUninitialize();
		return 1;
	}

	// handle the command line arguments (register/unregister)
	if (!commandLineProcessed)
	{
		long result = pClient->processCommandLine(commandLine);
		commandLineProcessed = TRUE;

		if (result != S_OK)
		{
			long returnResult = 0;

			if (result == S_FALSE)
			{
				// registration operation successful
				pClient->trace(EnumTraceLevel_INF, EnumTraceGroup_USER1, _T("_tWinMain"),
							   _T("Registration succeeded"));
				returnResult = 0;
			}
			else
			{
				// registration operation has failed
				pClient->trace(EnumTraceLevel_INF, EnumTraceGroup_USER1, _T("_tWinMain"),
							   _T("Registration failed"));
				returnResult = 1;
			}

			// no matter what close the application if processCommandLine returned
			// something different of S_OK
			pClient->terminate();
			destroyOpcClient();
			::CoUninitialize();
			return returnResult;
		} // end if
	}

	// start the service
	::StartServiceCtrlDispatcher(dispatchTable);
	pClient->terminate();
	destroyOpcClient();
	::CoUninitialize();
	return 0;
} // end _tWinMain

