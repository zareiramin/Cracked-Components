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
//  Filename    : MyAeAddressSpaceElement.h                                   |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic Alarms and Events OPC Server               |
//                address space element definition                            |
//                                                                            |
//-----------------------------------------------------------------------------
#ifndef _MYAEADDRESSSPACEELEMENT_H_
#define _MYAEADDRESSSPACEELEMENT_H_

#include "Ae/ServerAeAddressSpaceElement.h"
#include "Ae/ServerAeCategory.h"
#include "Ae/ServerAeCondition.h"
#include "Ae/ServerAeEvent.h"

using namespace SoftingOPCToolboxServer;

//-----------------------------------------------------------------------------
// MyAeAddressSpaceElement
//-----------------------------------------------------------------------------
class MyAeAddressSpaceElement : public AeAddressSpaceElement
{

public:
	MyAeAddressSpaceElement(
		tstring& aName,
		unsigned long anUserData,
		unsigned long anObjectHandle,
		unsigned long aParentHandle) :
		AeAddressSpaceElement(
			aName,
			anUserData,
			anObjectHandle,
			aParentHandle)
	{}  //  end cotr

	MyAeAddressSpaceElement(void)
	{}  //  end cotr

	virtual ~MyAeAddressSpaceElement(void)
	{}  //  end dtor

};  //  end class MyAeAddressSpaceElement

#endif
