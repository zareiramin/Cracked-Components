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
//                            OPC TOOLBOX - C++                               |
//                                                                            |
//  Filename    : ServerDaSession.cpp                                         |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC DA Session opened by client handler class               |
//                                                                            |
//-----------------------------------------------------------------------------
#include "OSCompat.h"
#include "ServerDaSession.h"

using namespace SoftingOPCToolboxServer;

//-----------------------------------------------------------------------------
//	constructor
//
DaSession::DaSession(
	EnumSessionType aType,
	unsigned long aHandle)
{
	m_type = aType;
	m_objectHandle = aHandle;
	tstring m_clientName = _T("");
}   //  end constructor


//-----------------------------------------------------------------------------
//	destructor
//
DaSession::~DaSession()
{
}   //  end descructor


//-----------------------------------------------------------------------------
// Notify that this session is to be destroyed
//
void DaSession::connectSession(void)
{
	handleConnected();
}   //  end connectSession


//-----------------------------------------------------------------------------
// Notify that this client was created
//
void DaSession::disconnectSession(void)
{
	handleDisconnected();
}   //  end disconnectSession


//-----------------------------------------------------------------------------
// Notify that this client tried to log on
//
long DaSession::logon(tstring& aUserName, tstring& aPassword)
{
	return E_NOTIMPL;
}   //  end logon


//-----------------------------------------------------------------------------
// Notify that this client logged off
//
long DaSession::logoff(void)
{
	return E_NOTIMPL;
}   //  end logoff
