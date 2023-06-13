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
//  Filename    : MyCreator.h                                                 |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description :                                                             |
//                                                                            |
//-----------------------------------------------------------------------------
#ifndef _MYCREATOR_H_
#define _MYCREATOR_H_

#include "ServerCreator.h"

#include "MyDaAddressSpaceElement.h"
#include "MyAeAddressSpaceElement.h"
#include "MyRequest.h"
#include "MyTransaction.h"
#include "MyDaAddressSpaceRoot.h"
#include "MyWebTemplate.h"

using namespace SoftingOPCToolboxServer;

//-----------------------------------------------------------------------------
// MyCreator
//-----------------------------------------------------------------------------
class MyCreator : public Creator
{

protected:
	virtual DaAddressSpaceElement* createInternalDaAddressSpaceElement(
		tstring& anItemId,
		tstring& aName,
		unsigned long anUserData,
		unsigned long anObjectHandle,
		unsigned long aParentHandle)
	{
		return (DaAddressSpaceElement*) new MyDaAddressSpaceElement(anItemId, aName, anUserData, anObjectHandle, aParentHandle);
	}   //  end createInternalDaAddressSpaceElement

	virtual AeAddressSpaceElement* createInternalAeAddressSpaceElement(
		tstring& aName,
		unsigned long anUserData,
		unsigned long anObjectHandle,
		unsigned long aParentHandle)
	{
		return new MyAeAddressSpaceElement(aName, anUserData, anObjectHandle, aParentHandle);
	}   //  end createInternalAeAddressSpaceElement

public:
	virtual DaAddressSpaceRoot* createDaAddressSpaceRoot(void)
	{
		return (DaAddressSpaceRoot*) new MyDaAddressSpaceRoot();
	}   //  end createDaAddressSpaceRoot

	virtual DaTransaction* createTransaction(
		EnumTransactionType aTransactionType,
		std::vector<DaRequest*>& aRequestList,
		unsigned long aSessionKey)
	{
		return (DaTransaction*) new MyTransaction(aTransactionType, aRequestList, aSessionKey);
	}   //  end createTransaction

	virtual DaAddressSpaceElement* createMyDaAddressSpaceElement(void)
	{
		return (DaAddressSpaceElement*) new MyDaAddressSpaceElement();
	}   //  end DaAddressSpaceElement

	virtual DaRequest* createRequest(
		EnumTransactionType aTransactionType,
		unsigned long aSessionHandle,
		DaAddressSpaceElement* anElement,
		int aPropertyId,
		unsigned long aRequestHandle)
	{
		return new MyRequest(aTransactionType, aSessionHandle, anElement, aPropertyId, aRequestHandle);
	}   //  end createRequest

	WebTemplate* createWebTemplate(void)
	{
		return (WebTemplate*) new MyWebTemplate();
	}   //  end createWebTemplate

};  //  end class MyCreator

#endif
