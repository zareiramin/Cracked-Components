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
//  Description :                                                             |
//                                                                            |
//-----------------------------------------------------------------------------
#ifndef _MYTRANSACTION_H_
#define _MYTRANSACTION_H_

#include "Da/ServerDaTransaction.h"

#include <math.h>

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
					ValueQT cacheValue;
					element->getCacheValue(cacheValue);
					m_requestList[i]->setValue(cacheValue);
					m_requestList[i]->setResult(S_OK);
				}
				else
				{
					//  the element will handle this request
					element->getPropertyValue(m_requestList[i]);
				}   //  end if ... else
			}   //  end if ... else
		}   //  enmd for

		return completeRequests();
	}   //  end HandleReadRequests


	virtual long handleWriteRequests(void)
	{
		size_t count = m_requestList.size();

		for (size_t i = 0; i < count; i++)
		{
			MyDaAddressSpaceElement* element = (MyDaAddressSpaceElement*)m_requestList[i]->getAddressSpaceElement();

			if (element == NULL)
			{
				m_requestList[i]->setResult(E_FAIL);
			}
			else if (element->getType() == MyDaAddressSpaceElement::TYPE_ACCEPT)
			{
				// accept the written value
				ValueQT* writeValue = m_requestList[i]->getValue();

				if (m_requestList[i]->getValue() != NULL)
				{
					long result = element->valueChanged(*writeValue);
					m_requestList[i]->setResult(result);
				}
			}
			else if ((m_requestList[i]->getPropertyId() == 6020) &&
					 (element->getType() == MyDaAddressSpaceElement::TYPE_SINE))
			{
				// change the angle of the sin
				ValueQT* writeValue = m_requestList[i]->getValue();

				if (writeValue != NULL && writeValue->getData().iVal < 359)
				{
					g_angle = writeValue->getData().iVal;
					double pi = 3.1415926535;
					double radianAngle = (2 * pi) * ((double)g_angle / 360.0);
					Variant var;
					var.setR8(sin(radianAngle));
					DateTime now;
					now.now();
					ValueQT elementValue(var, EnumQuality_GOOD, now);
					long result = element->valueChanged(elementValue);
					m_requestList[i]->setResult(result);
				}
				else
				{
					m_requestList[i]->setResult(EnumResultCode_E_OPC_RANGE);
				}   //  end if
			}
			else
			{
				// unknown write item - should never get here !
				m_requestList[i]->setResult(E_FAIL);
			}   //  end else ... if
		}   //  end  for

		return completeRequests();
	}   //  end handleWriteRequests

};  //  end class MyTransaction

#endif
