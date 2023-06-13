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
//  Filename    : MyTransaction.h                                             |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic DataAccess OPC server                      |
//                DaTransaction definition                                    |
//                                                                            |
//-----------------------------------------------------------------------------
#ifndef _MYTRANSACTION_H_
#define _MYTRANSACTION_H_

#include "Da/ServerDaTransaction.h"
#include "MyDaAddressSpaceElement.h"

using namespace SoftingOPCToolboxServer;

class MyTransaction : public DaTransaction
{

public:

	MyTransaction(
		EnumTransactionType aTransactionType,
		std::vector<DaRequest*>& aRequestList,
		unsigned long aSessionKey) :
		DaTransaction(
			aTransactionType,
			aRequestList,
			aSessionKey)
	{}  //  end ctor

	virtual ~MyTransaction()
	{}  //  end dtor

	virtual long handleReadRequests(void)
	{
		size_t count = m_requestList.size();

		for (size_t i = 0; i < count; i++)
		{
			MyDaAddressSpaceElement* element =
				(MyDaAddressSpaceElement*)m_requestList[i]->getAddressSpaceElement();

			if (element == NULL)
			{
				m_requestList[i]->setResult(E_FAIL);
			}
			else
			{
				if (m_requestList[i]->getPropertyId() == 0)
				{
					// get address space element value take the toolbox cache value
					ValueQT cacheValue;
					element->getCacheValue(cacheValue);
					m_requestList[i]->setValue(cacheValue);
					m_requestList[i]->setResult(S_OK);
				}
				else
				{
					//  the element's property will handle this request
					element->getPropertyValue(m_requestList[i]);
				}   //  end if ... else
			}   //  end if ... else
		}   //  end for

		return completeRequests();
	}   //  end HandleReadRequests

	virtual long handleWriteRequests(void)
	{
		size_t count = m_requestList.size();

		for (size_t i = 0; i < count; i++)
		{
			DaRequest* pRequest = m_requestList[i];

			if (pRequest != NULL)
			{
				MyDaAddressSpaceElement* pElement =
					(MyDaAddressSpaceElement*)pRequest->getAddressSpaceElement();

				if (pElement != NULL)
				{
					ValueQT* pValue = pRequest->getValue();
					pRequest->setResult(pElement->valueChanged(*pValue));
				} // end if
			}   // end if
		}   //  end  for

		return completeRequests();
	}   //  end handleWriteRequests

};  //  end class MyTransaction

#endif  //  _MYTRANSACTION_H_
