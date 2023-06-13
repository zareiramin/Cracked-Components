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
//  Filename    : ServerDaTransaction.h                                       |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC DA transaction handling class                           |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _SERVERDATRANSACTION_H_
#define _SERVERDATRANSACTION_H_

#include "../../Enums.h"
#include "../ServerEnums.h"

#include <vector>

#pragma pack(push,4)

namespace SoftingOPCToolboxServer
{

class DaRequest;
class DaSession;

// Transaction class: Used for managing the transaction requests
class TBC_EXPORT DaTransaction
{

private:
	EnumTransactionType m_type;
	unsigned long m_sessionHandle;
	static unsigned long KeyBuilder;
	unsigned long m_key;

public:
	DaTransaction(
		EnumTransactionType aTransactionType,
		std::vector<DaRequest*>& aRequestList,
		unsigned long aSessionHandle);

	virtual ~DaTransaction();

protected:
	std::vector<DaRequest*> m_requestList;

public:

	virtual long handleReadRequests(void)
	{
		return E_NOTIMPL;
	}   //  end handleReadRequests

	virtual long handleWriteRequests(void)
	{
		return E_NOTIMPL;
	}   //  end handleWriteRequests

	//  Determines whether this is a read or a write transaction
	EnumTransactionType getType(void)
	{
		return m_type;
	}

	//  retrieves the key
	unsigned long getKey()
	{
		return m_key;
	}

	// Requests contained in this transaction
	std::vector<DaRequest*>& getRequests(void)
	{
		return m_requestList;
	}

	//  no requests pending
	BOOL isEmpty(void)
	{
		return m_requestList.empty();
	}

	// Function to get the session from which  the Transaction came from
	DaSession* getSession(void);

	long completeRequests(void);
	long completeRequest(DaRequest* aRequest);
	long valuesChanged(void);

	void removeRequest(DaRequest* aRequest);

};  //  end class Transaction

}   //  end namespace SoftingOPCToolboxServer

#pragma pack(pop)
#endif  //  _DATRANSACTION_H_

