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
//  Filename    : Transaction.cpp                                             |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description :                                                             |
//                                                                            |
//-----------------------------------------------------------------------------

#include "stdafx.h"
#include "Transaction.h"
#include "Function.h"

Transaction::Transaction(
	EnumTransactionType aTransactionType,
	std::vector<DaRequest*>& aRequestList,
	unsigned long aSessionKey) :
	DaTransaction(
		aTransactionType,
		aRequestList,
		aSessionKey)
{
}   //  end constructor

Transaction::~Transaction()
{
}   //  end destructor

long Transaction::handleReadRequests(void)
{
	size_t count = m_requestList.size();

	for (size_t i = 0; i < count; i++)
	{
		DemoDaAddressSpaceElement* pElement = (DemoDaAddressSpaceElement*)m_requestList[i]->getAddressSpaceElement();

		if (pElement == NULL)
		{
			m_requestList[i]->setResult(E_FAIL);
		}
		else
		{
			pElement->handleReadRequest(m_requestList[i]);
		}   //  end if ... else
	}   //  end for

	return completeRequests();
}   //  end HandleReadRequests

long Transaction::handleWriteRequests(void)
{
	size_t count = m_requestList.size();

	for (size_t i = 0; i < count; i++)
	{
		DemoDaAddressSpaceElement* pElement = (DemoDaAddressSpaceElement*)m_requestList[i]->getAddressSpaceElement();

		if (pElement == NULL)
		{
			m_requestList[i]->setResult(E_FAIL);
		}
		else
		{
			pElement->handleWriteRequest(m_requestList[i]);
		}
	}   //  end  for

	return completeRequests();
}   //  end handleWriteRequests
