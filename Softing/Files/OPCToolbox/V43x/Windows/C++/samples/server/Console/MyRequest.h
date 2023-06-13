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
//  Filename    : MyRequest.h                                                 |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description :                                                             |
//                                                                            |
//-----------------------------------------------------------------------------
#ifndef _MYREQUEST_H_
#define _MYREQUEST_H_

#include "Da/ServerDaRequest.h"

using namespace SoftingOPCToolboxServer;

//-----------------------------------------------------------------------------
// MyRequest
//-----------------------------------------------------------------------------
class MyRequest : public DaRequest
{
public:
	MyRequest(
		EnumTransactionType aTransactionType,
		unsigned long aSessionHandle,
		DaAddressSpaceElement* anElement,
		int aPropertyId,
		unsigned long aRequestHandle):
		DaRequest(
			aTransactionType,
			aSessionHandle,
			anElement,
			aPropertyId,
			aRequestHandle)
	{}

	virtual ~MyRequest(void)
	{}

};  //  end class MyRequest

#endif
