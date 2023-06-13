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
//                             OPC Toolbox C++                                |
//                                                                            |
//  Filename    : TimeVariable.h                                              |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic DataAccess Server OPC Server               |
//                address space element definition                            |
//                                                                            |
//-----------------------------------------------------------------------------
#ifndef _TIME_VARIABLE_H_
#define _TIME_VARIABLE_H_

#include "DaAddressSpaceElement.h"
#include "Da/ServerDaAddressSpaceElement.h"
#include "Da/ServerDaProperty.h"

using namespace SoftingOPCToolboxServer;

class TimeVariable : public DemoDaAddressSpaceElement
{
public:

	enum TimeType
	{
		second,
		minute,
		hour,
		string,
		array,
		date,
		limitSecond
	};

	enum TimeZone
	{
		local,
		GMT,
		none
	};

	TimeVariable(IN tstring varName);
	TimeVariable(IN tstring varName, IN enum TimeType type, IN enum TimeZone timeZone);

	// request handling
	virtual void handleReadRequest(DaRequest* pRequest);

	virtual void handleWriteRequest(DaRequest* pRequest);

protected:
	enum TimeType m_timeType;
	enum TimeZone m_timeZone;

}; // SODmSTagTime

#endif
