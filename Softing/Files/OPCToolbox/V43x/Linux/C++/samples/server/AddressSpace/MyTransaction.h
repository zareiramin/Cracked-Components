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



//-----------------------------------------------------------------------------
// MyTransaction
//-----------------------------------------------------------------------------
class MyTransaction : public DaTransaction
{
public:

	MyTransaction(
		EnumTransactionType aTransactionType,
		std::vector<DaRequest*>& aRequestList,
		unsigned long aSessionKey) : DaTransaction(aTransactionType, aRequestList, aSessionKey)
	{
	}   //  enc ctr

	virtual long handleReadRequests(void)
	{
		size_t count = m_requestList.size();

		for (size_t i = 0; i < count; i++)
		{
			MyDaAddressSpaceElement* element = (MyDaAddressSpaceElement*)m_requestList[i]->getAddressSpaceElement();

			if (element == NULL)
			{
				m_requestList[i]->setResult(E_FAIL);
			}
			else
			{
				if (m_requestList[i]->getPropertyId() == 0)
				{
					// get address space element value take the toolbox cache value
					ValueQT aValue;
					element->getCacheValue(aValue);
					m_requestList[i]->setValue(aValue);
					m_requestList[i]->setResult(S_OK);
				}
				else
				{
					//  for all properties return the element type
					DateTime now;
					Variant aVariant(element->getType());
					ValueQT aValue(aVariant, EnumQuality_GOOD, now);
					m_requestList[i]->setValue(aValue);
					m_requestList[i]->setResult(S_OK);
				}   //  end if ... else
			}   //  end if ... else
		}   //  enmd for

		return completeRequests();
	}   //  end HandleReadRequests


	virtual long handleWriteRequests(void)
	{
		long result = valuesChanged();

		if (FAILED(result))
		{
			return result;
		} // end if

		return completeRequests();
	}   //  end handleWriteRequests

};  //  end class MyTransaction

#endif
