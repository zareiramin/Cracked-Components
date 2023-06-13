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
#include "MySession.h"

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
	{} // end ctor

	virtual ~MyTransaction()
	{} // end dtor

	virtual long handleReadRequests(void)
	{
		MySession* pSession = (MySession*)getSession();
		std::vector<DaRequest*> requests = getRequests();

		for (size_t i = 0; i < requests.size(); i++)
		{
			ValueQT value;
			requests[i]->getAddressSpaceElement()->queryCacheValue(pSession, value);
			requests[i]->setValue(value);
			requests[i]->setResult(EnumResultCode_S_OK);
		} // end for

		return completeRequests();
	}   //  end HandleReadRequests

	virtual long handleWriteRequests(void)
	{
		std::vector<DaRequest*> requests = getRequests();
		size_t count = requests.size();
		MySession* pClient = (MySession*)getSession();

		for (size_t i = 0 ; i < count; i++)
		{
			if (requests[i]->getPropertyId() == 0)
			{
				if (pClient != MySession::s_controlingSession)
				{
					requests[i]->setResult(EnumResultCode_E_ACCESSDENIED);
					requests[i]->complete();
				}
				else
				{
					ValueQT* value = requests[i]->getValue();
					requests[i]->getAddressSpaceElement()->valueChanged(*value);
					requests[i]->setResult(EnumResultCode_S_OK);
					requests[i]->complete();
				}
			} // end if
		} // end for

		return completeRequests();
	} // end handleWriteRequests
};  // end class MyTransaction

#endif  // _MYTRANSACTION_H_
