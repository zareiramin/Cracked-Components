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
//  Filename    : OpcClient.h                                                 |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC Client template class definition                        |
//                                                                            |
//  This code and information is provided "as is" without warranty of         |
//  any kind, either expressed or implied, including but not limited          |
//  to the implied warranties of merchantability and/or fitness for a         |
//  particular purpose.                                                       |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _CLIENT_H_
#define _CLIENT_H_

#include "Enums.h"
#include "ClientApplication.h"

using namespace SoftingOPCToolboxClient;

#include "MyAeSession.h"
#include "MyAeSubscription.h"
#include "Ae\ClientAeCondition.h"
#include "ClientServerStatus.h"
#include "Ae\ClientAeEvent.h"

class OpcClient
{

private:

	MyAeSubscription* subscription;
	MyAeSession* session;
	tstring getStateToString(EnumConditionState conditionState);
	ExecutionOptions executionOptions;
	unsigned long executionContext;
	unsigned long count;
	ServerStatus serverStatus;
	std::vector<tstring> areas;
	std::vector<tstring> sources;
	std::vector<unsigned long> categoryIds;
	std::vector<AeReturnedAttributes*>aeReturnedAttributes;
	std::vector<unsigned long> attributesIds;

	SoftingOPCToolboxClient::Application* getApp();

public:

	OpcClient();
	virtual ~OpcClient() {}

	long initialize();
	void terminate();
	long processCommandLine(IN tstring commandLine);
	void setServiceName(tstring serviceName);

	void activateSyncObjects();
	void activateAsyncObjects();
	void connectSyncObjects();
	void connectAsyncObjects();
	void disconnectSyncObjects();
	void disconnectAsyncObjects();
	void getSyncServerStatus();
	void getAsyncServerStatus();
	void activateConnectionMonitor();
	void deactivateConnectionMonitor();

	long initializeAeObjects();
	tstring getConditionState();

	void trace(
		EnumTraceLevel aLevel,
		EnumTraceGroup aMask,
		const TCHAR* anObjectID,
		const TCHAR* aMessage,
		...);

};  // end class OpcClient

// public OpcClient handlers
OpcClient* getOpcClient(void);
void createOpcClient(void);
void destroyOpcClient(void);

#endif
