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
//  Filename    : OpcClient.cpp                                               |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC Client template implementation                          |
//                                                                            |
//  This code and information is provided "as is" without warranty of         |
//  any kind, either expressed or implied, including but not limited          |
//  to the implied warranties of merchantability and/or fitness for a         |
//  particular purpose.                                                       |
//                                                                            |
//-----------------------------------------------------------------------------
#include "stdafx.h"
#include "OpcClient.h"
#include "ClientCommon.h"

using namespace std;
using namespace SoftingOPCToolboxClient;

OpcClient* instance = NULL;

OpcClient* getOpcClient(void)
{
	return instance;
} // end getOpcClient

void createOpcClient(void)
{
	if (instance == NULL)
	{
		instance = new OpcClient();
	}
} // end createOpcClient

void destroyOpcClient(void)
{
	if (instance != NULL)
	{
		delete instance;
		instance = NULL;
	}
} // end destroyOpcClient

OpcClient::OpcClient()
{
	session = NULL;
} // end constructor

Application* OpcClient::getApp()
{
	return getApplication();
} // end getApp

long OpcClient::initialize()
{
	long result = EnumResultCode_S_OK;

	// TODO - design time license activation
	// Fill in your design time license activation keys here
	//
	// NOTE: you can activate one or all of the features at the same time

	// activate the COM-AE Client Feature
	// result = getApplication()->activate(EnumFeature_AE_CLIENT, _T("XXXX-XXXX-XXXX-XXXX-XXXX"));
	if (!SUCCEEDED(result))
	{
		return FALSE;
	}

	// END TODO - design time license activation
	result = getApp()->initialize();

	if (SUCCEEDED(result))
	{
		getApp()->enableTracing(
			EnumTraceGroup_ALL,
			EnumTraceGroup_ALL,
			EnumTraceGroup_CLIENT,
			EnumTraceGroup_CLIENT,
			_T("Trace.txt"),
			1000000,
			0);
	}

	return result;
} // end initialize

long OpcClient::processCommandLine(tstring command)
{
	return getApp()->processCommandLine(command);
} // end processCommandLine

void OpcClient::terminate()
{
	if (session->getCurrentState() != EnumObjectState_DISCONNECTED)
	{
		session->disconnect(NULL);
	}

	getApp()->removeAeSession(session);
	getApp()->terminate();

	if (session != NULL)
	{
		delete session;
		session = NULL;
	}

	releaseApplication();
} // end terminate

void OpcClient::setServiceName(tstring serviceName)
{
	getApp()->setServiceName(serviceName);
} // end setServiceName

void OpcClient::trace(
	EnumTraceLevel aLevel,
	EnumTraceGroup aMask,
	const TCHAR* anObjectID,
	const TCHAR* aMessage,
	...)
{
	getApp()->trace(aLevel, aMask, anObjectID, aMessage);
} // end trace


long OpcClient::initializeAeObjects()
{
	sourcePath = _T("computer.clock.time slot 1");
	executionOptions.setExecutionType(EnumExecutionType_SYNCHRONOUS);
	executionOptions.setExecutionContext(0);
	executionContext = executionOptions.getExecutionContext();
	session = new MyAeSession(_T("opcae:///Softing.OPCToolboxDemo_ServerAE.1/{2E565243-B238-11D3-842D-0008C779D775}"));
	long connectResult = session->connect(TRUE, FALSE, NULL);
	return connectResult;
} // end initializeAeObjects


void OpcClient::getAsyncSourceConditions()
{
	unsigned int result;
	executionOptions.setExecutionType(EnumExecutionType_ASYNCHRONOUS);
	executionOptions.setExecutionContext(++executionContext);
	_tprintf(_T("Conditions of the source  %s are:\n"), sourcePath.c_str());
	result = session->queryAeSourceConditions(
				 sourcePath,
				 conditionNames,
				 &executionOptions);
}// end getAsyncSourceConditions

void OpcClient::getSyncSourceConditions()
{
	unsigned int result;
	executionOptions.setExecutionType(EnumExecutionType_SYNCHRONOUS);
	_tprintf(_T("Conditions of the source  %s are:\n"), sourcePath.c_str());
	result = session->queryAeSourceConditions(
				 sourcePath,
				 conditionNames,
				 &executionOptions);

	if (SUCCEEDED(result))
	{
		for (unsigned int i = 0; i < conditionNames.size(); i++)
		{
			_tprintf(_T("	[%i]%s"), i, conditionNames[i].c_str());
			_tprintf(_T("\n"));
		} //end for
	} //end if
	else
	{
		_tprintf(_T(" Failed to synchronously query the conditions of source: %s"), sourcePath.c_str());
	}
}// end getSyncSourceConditions
