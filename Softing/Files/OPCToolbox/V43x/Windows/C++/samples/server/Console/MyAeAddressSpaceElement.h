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
//  Description :                                                             |
//                                                                            |
//-----------------------------------------------------------------------------
#ifndef _MYAEADDRESSSPACEELEMENT_H_
#define _MYAEADDRESSSPACEELEMENT_H_


#include "Ae/ServerAeAddressSpaceElement.h"
#include "Ae/ServerAeCategory.h"
#include "Ae/ServerAeCondition.h"
#include "Ae/ServerAeEvent.h"

using namespace SoftingOPCToolboxServer;

extern tstring COND_NAME_BETWEEN_SINGLE;
extern tstring COND_NAME_BETWEEN_MULTIPLE;



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
		AeAddressSpaceElement(aName, anUserData, anObjectHandle, aParentHandle)
	{
	}   //  end ctr

	MyAeAddressSpaceElement(void)
	{
	}   //  end ctr

	long queryConditions(
		tstring& aSourcePath,
		std::vector<tstring>& aConditionNameList)
	{
		aConditionNameList.push_back(COND_NAME_BETWEEN_MULTIPLE);
		aConditionNameList.push_back(COND_NAME_BETWEEN_SINGLE);
		return S_OK;
	}   //  end QueryConditions

};  //  end class MyAeAddressSpaceElement

#endif
