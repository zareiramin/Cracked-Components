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
//  Filename    : ServerCreator.h                                             |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC Server Object instance factory handler                  |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _SERVERCREATOR_H_
#define _SERVERCREATOR_H_

#include "../Enums.h"
#include "ServerEnums.h"

#include <vector>

#pragma pack(push,4)

namespace SoftingOPCToolboxServer
{

class DaAddressSpaceRoot;
class DaAddressSpaceElement;
class DaRequest;
class DaTransaction;
class DaSession;

class AeAddressSpaceRoot;
class AeAddressSpaceElement;
class WebTemplate;
class AeAttribute;

class Application;

class TBC_EXPORT Creator
{

public:

	Creator() {}
	virtual ~Creator() {}

	virtual DaAddressSpaceElement* createDaAddressSpaceElement();
	virtual AeAddressSpaceElement* createAeAddressSpaceElement();

	// Creates a singleton DaAddressSpaceRoot
	virtual DaAddressSpaceRoot* createDaAddressSpaceRoot(void);

	// Creates a new instance of DaAddressSpaceElement
	virtual DaAddressSpaceElement* createInternalDaAddressSpaceElement(
		tstring& anItemId,
		tstring& aName,
		unsigned long anUserData,
		unsigned long anObjectHandle,
		unsigned long aParentHandle);

	// Creates a new instance of "Request"
	virtual DaRequest* createRequest(
		EnumTransactionType aTransactionType,
		unsigned long aSessionHandle,
		DaAddressSpaceElement* anElement,
		int aPropertyId,
		unsigned long aRequestHandle);

	// Creates a new instance of "Transaction"
	virtual DaTransaction* createTransaction(
		EnumTransactionType aTransactionType,
		std::vector<DaRequest*>& aRequestList,
		unsigned long aSessionHandle);

	// Creates a new instance of "Session"
	// NOTE: please be note thet the session can be also an AE Session.
	//  The name was not changed due to backwords compatibility issues
	virtual DaSession* createSession(
		EnumSessionType aType,
		unsigned long aHandle);

	// Creates a singleton AeAddressSpaceRoot
	virtual AeAddressSpaceRoot* createAeAddressSpaceRoot(void);

	// Creates a new instance of AddressSpaceElement
	virtual AeAddressSpaceElement* createInternalAeAddressSpaceElement(
		tstring& aName,
		unsigned long anUserData,
		unsigned long anObjectHandle,
		unsigned long aParentHandle);

	virtual WebTemplate* createWebTemplate(void);

};  //  end  class Creator

}   //  end namespace

#pragma pack(pop)
#endif  //  end _CREATOR_H_
