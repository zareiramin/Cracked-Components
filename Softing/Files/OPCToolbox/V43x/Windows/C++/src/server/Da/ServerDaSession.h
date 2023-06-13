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
//  Filename    : ServerDaSession.h                                           |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC DA Session opened by client handler class               |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _SERVERDASESSION_H_
#define _SERVERDASESSION_H_
#include "../../Enums.h"
#include "../ServerEnums.h"

#pragma pack(push,4)

namespace SoftingOPCToolboxServer
{

// Handles data of a client connected.
class TBC_EXPORT DaSession
{
	friend class OTSGlobals;
private:
	// Handle of the session
	unsigned long m_objectHandle;
	EnumSessionType m_type;

	tstring m_clientName;
protected:

	unsigned long getObjectHandle(void)
	{
		return m_objectHandle;
	}

public:

	DaSession(EnumSessionType aType, unsigned long aHandle);
	virtual ~DaSession();

	//  Events
	virtual void handleConnected(void) {};
	virtual void handleDisconnected(void) {};

	// Type of session
	EnumSessionType getType(void)
	{
		return m_type;
	}
	// ClientName
	tstring getClientName(void)
	{
		return m_clientName;
	}

	virtual void connectSession(void);
	virtual void disconnectSession(void);

	virtual long logon(
		tstring& aUserName,
		tstring& aPassword);

	virtual long logoff(void);

};  //  end class Session

}   //  end namespace

#pragma pack(pop)
#endif  //  _DASESSION_H_
