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
//  Filename    : Console.cpp                                                 |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Console application main implementation                     |
//                                                                            |
//  This code and information is provided "as is" without warranty of         |
//  any kind, either expressed or implied, including but not limited          |
//  to the implied warranties of merchantability and/or fitness for a         |
//  particular purpose.                                                       |
//                                                                            |
//-----------------------------------------------------------------------------

#include "stdafx.h"

#include "OpcClient.h"

#include <stdio.h>
#include <iostream>

#ifdef TBC_OS_WINDOWS
#include <conio.h>
#endif

#ifdef TBC_OS_LINUX
#include <pthread.h>
#include <semaphore.h>
#include <errno.h>
#endif

using namespace std;
using namespace SoftingOPCToolboxClient;

#ifdef TBC_OS_WINDOWS
HANDLE g_endEvent = 0;
#endif
#ifdef TBC_OS_LINUX
sem_t g_endSemaphor;
#endif

#ifdef TBC_OS_WINDOWS

BOOL WINAPI ControlHandler(IN unsigned long crtlType)   // control type
{
	switch (crtlType)
	{
	case CTRL_BREAK_EVENT:  // use Ctrl+C or Ctrl+Break to end server
	case CTRL_C_EVENT:
	case CTRL_CLOSE_EVENT:
		if (g_endEvent)
		{
			SetEvent(g_endEvent);
		}

		return TRUE;
	}

	return FALSE;
} // end controlHandler
#endif


void usage(void)
{
	_tprintf(_T("\nUsage:\n"));
	_tprintf(_T("Press \'a\' or \'A\' to asynchronously get address space element properties \n"));
	_tprintf(_T("Press \'s\' or \'S\' to synchronously get the address space element properties \n"));
	_tprintf(_T("Press \'?\' or \'u\' to display this usage information\n"));
	_tprintf(_T("Press \'e\' or \'q\' or Ctrl-C to exit\n\n"));
}

int _tmain(int argc, _TCHAR* argv[])
{
	long result = S_OK;
#ifdef TBC_OS_WINDOWS
	SetConsoleCtrlHandler(ControlHandler, TRUE);
#endif
	// create the OpcClient instance
	createOpcClient();
	OpcClient* pClient = getOpcClient();
#ifdef TBC_OS_WINDOWS
	unsigned long waitRet = 0;
	g_endEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
#endif

	// initialize the client instance
	if (!SUCCEEDED(pClient->initialize()))
	{
		pClient->terminate();
		destroyOpcClient();
#ifdef TBC_OS_WINDOWS
		CloseHandle(g_endEvent);
#endif
		return 1;
	}

	// initialize the DA client simulation
	result |= pClient->initializeDaObjects();
	usage();
	unsigned long waitTime = 100;
#ifdef TBC_OS_WINDOWS
	bool end = FALSE;

	while (!end)
	{
		waitRet = WaitForSingleObject(g_endEvent, waitTime);

		if (waitRet == WAIT_OBJECT_0)
		{
			end = TRUE;
			CloseHandle(g_endEvent);
		}
		else
		{
#endif
#ifdef TBC_OS_LINUX
	unsigned long timeout = 0;

	while (timeout < waitTime)
	{
		timespec delay;
		delay.tv_sec = 0;
		delay.tv_nsec = 100000000;  // 100 milli sec
		// Wait for the event be signaled
		int retCode = sem_trywait(&g_endSemaphor); // event semaphore handle

		if (!retCode)
		{
			// Event is signaled
			break;
		}
		else
		{
			// check whether somebody else has the mutex
			if (retCode == EPERM)
			{
				// sleep for delay time
				nanosleep(&delay, NULL);
				timeout++ ;
			}

#endif
#ifdef TBC_OS_WINDOWS

			if (_kbhit())
			{
				int input = _getch();
#endif
#ifdef TBC_OS_LINUX
			char input = 0;

			if (1)
			{
				std::cin.get(input);
#endif

				if (input == _T('a') || (input == _T('A')))
				{
					pClient->getAsyncAddressSpaceElementProps();
				} //end if

				if (input == _T('s') || (input == _T('S')))
				{
					pClient->getSyncAddressSpaceElementProps();
				} //end if

				if (input == _T('u') || (input == _T('U')) || (input == _T('?')))
				{
					usage();
				} //end if

				if (input == _T('E') || (input == _T('e')) || (input == _T('q')) || (input == _T('Q')))
				{
#ifdef TBC_OS_WINDOWS
					end = TRUE;
					CloseHandle(g_endEvent);
#endif
#ifdef TBC_OS_LINUX
					sem_post(&g_endSemaphor);
#endif
				}
			} // end if
		} // end else

#ifdef TBC_OS_WINDOWS
		Sleep(1000);
#endif
	} // end while

	pClient->terminate();
	destroyOpcClient();
#ifdef TBC_OS_WINDOWS
	CloseHandle(g_endEvent);
#endif
	return 0;
} // end main
