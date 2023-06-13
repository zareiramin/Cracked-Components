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

#include <conio.h>

int _tmain(int argc, _TCHAR* argv[])
{
	long result = S_OK;
	// create the OpcClient instance
	createOpcClient();
	OpcClient* pClient = getOpcClient();

	// initialize the client instance
	if (!SUCCEEDED(pClient->initialize()))
	{
		pClient->terminate();
		destroyOpcClient();
		return 1;
	}

	// initialize the AE client simulation
	result |= pClient->initializeAeObjects();
	pClient->printInitialObjectAttributes();
	pClient->setSessionAttributes();
	pClient->setSubscriptionAttributes();
	pClient->terminate();
	destroyOpcClient();
	_tprintf(_T("\nPress any key to finish..."));
	_getch();
	return 0;
} // end main
