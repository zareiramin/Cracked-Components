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
//  Filename    : ServerCreator.cpp                                           |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC Server Object instance factory handler                  |
//                                                                            |
//-----------------------------------------------------------------------------

#include "../OSCompat.h"
#include "ServerCreator.h"

#include "Da/ServerDaAddressSpaceElement.h"
#include "Da/ServerDaRequest.h"
#include "Da/ServerDaTransaction.h"
#include "Da/ServerDaSession.h"

#include "Ae/ServerAeAddressSpaceElement.h"
#include "Ae/ServerAeAttribute.h"

#include "ServerWebTemplate.h"

using namespace SoftingOPCToolboxServer;

//-----------------------------------------------------------------------------
// Creates a singleton DaAddressSpaceRoot
//
DaAddressSpaceRoot* Creator::createDaAddressSpaceRoot(void)
{
	return new DaAddressSpaceRoot();
}   //  end createDaAddressSpaceRoot


//-----------------------------------------------------------------------------
// Creates a new instance of DaAddressSpaceElement
//
DaAddressSpaceElement* Creator::createInternalDaAddressSpaceElement(
	tstring& anItemId,
	tstring& aName,
	unsigned long anUserData,
	unsigned long anObjectHandle,
	unsigned long aParentHandle)
{
	return new DaAddressSpaceElement(anItemId, aName, anUserData, anObjectHandle, aParentHandle);
}   //  end createDaAddressSpaceElement


//-----------------------------------------------------------------------------
// Creates a new instance of DaAddressSpaceElement
//
DaAddressSpaceElement* Creator::createDaAddressSpaceElement()
{
	return new DaAddressSpaceElement();
}   //  end createDaAddressSpaceElement


//-----------------------------------------------------------------------------
// Creates a new instance of "Request"
//
DaRequest* Creator::createRequest(
	EnumTransactionType aTransactionType,
	unsigned long aSessionHandle,
	DaAddressSpaceElement* anElement,
	int aPropertyId,
	unsigned long m_requestHandle)
{
	return new DaRequest(
			   aTransactionType,
			   aSessionHandle,
			   anElement,
			   aPropertyId,
			   m_requestHandle);
}   //  end createRequest


//-----------------------------------------------------------------------------
// Creates a new instance of "Transaction"
//
DaTransaction* Creator::createTransaction(
	EnumTransactionType aTransactionType,
	std::vector<DaRequest*>& aRequestList,
	unsigned long aSessionHandle)
{
	return new DaTransaction(aTransactionType, aRequestList, aSessionHandle);
}   //  end createTransaction


//-----------------------------------------------------------------------------
// Creates a new instance of Session
//
DaSession* Creator::createSession(
	EnumSessionType aType,
	unsigned long aHandle)
{
	return new DaSession(aType, aHandle);
}   //  end createSession


//-----------------------------------------------------------------------------
// Creates a singleton AeAddressSpaceRoot
//
AeAddressSpaceRoot* Creator::createAeAddressSpaceRoot(void)
{
	return new AeAddressSpaceRoot();
}   //  end createAeAddressSpaceRoot


//-----------------------------------------------------------------------------
// Creates a new instance of AddressSpaceElement
//
AeAddressSpaceElement* Creator::createInternalAeAddressSpaceElement(
	tstring& aName,
	unsigned long anUserData,
	unsigned long anObjectHandle,
	unsigned long aParentHandle)
{
	return new AeAddressSpaceElement(aName, anUserData, anObjectHandle, aParentHandle);
}   //  end createAeAddressSpaceElement


//-----------------------------------------------------------------------------
// Creates a new instance of AddressSpaceElement
//
AeAddressSpaceElement* Creator::createAeAddressSpaceElement()
{
	return new AeAddressSpaceElement();
}   //  end createAeAddressSpaceElement


//-----------------------------------------------------------------------------
// Creates a new instance of the WebTemplate
//
WebTemplate* Creator::createWebTemplate(void)
{
	return new WebTemplate();
}   //  end createWebTemplate

