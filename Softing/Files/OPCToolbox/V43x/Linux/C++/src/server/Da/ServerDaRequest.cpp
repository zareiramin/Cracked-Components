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
//  Filename    : ServerDaRequest.cpp                                         |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC DA client generated request handler class               |
//                                                                            |
//-----------------------------------------------------------------------------

#include "OSCompat.h"
#include "ServerDaRequest.h"
#include "ServerDaAddressSpaceElement.h"
#include "ServerDaTransaction.h"
#include "../ServerApplication.h"

#include "OTServer.h"

using namespace SoftingOPCToolboxServer;

//-----------------------------------------------------------------------------
//	Constructor
//
DaRequest::DaRequest(
	EnumTransactionType aTransactionType,
	unsigned long aSessionHandle,
	DaAddressSpaceElement* anElement,
	int aPropertyId,
	unsigned long aRequestHandle):
	m_sessionHandle(aSessionHandle),
	m_requestHandle(aRequestHandle),
	m_transactionKey(0),
	m_transactionType(aTransactionType),
	m_propertyId(aPropertyId),
	m_addressSpaceElement(anElement),
	m_result(E_NOTIMPL),
	m_requestState(EnumRequestState_CREATED)
{
}   //  end constructor


//-----------------------------------------------------------------------------
//	Destructor
//
DaRequest::~DaRequest()
{
	if (m_requestState != EnumRequestState_COMPLETED)
	{
		//  this is not supposed to ever happen
		complete();
	}   //  end if
}   //  end destructor


//-----------------------------------------------------------------------------
// Complete this request
//
long DaRequest::complete(void)
{
	long result = S_FALSE;
	DaTransaction* transaction = Application::Instance()->findTransaction(m_transactionKey);

	if (transaction != NULL)
	{
		result = transaction->completeRequest(this);
	}   //  end if

	return result;
}   //  end complete
