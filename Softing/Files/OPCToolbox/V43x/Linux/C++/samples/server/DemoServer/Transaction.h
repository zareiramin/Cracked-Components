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
//  Filename    : Transaction.h                                               |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic DataAccess OPC server                      |
//                DaTransaction definition                                    |
//                                                                            |
//-----------------------------------------------------------------------------
#ifndef _TRANSACTION_H_
#define _TRANSACTION_H_

#include "DaAddressSpaceElement.h"
#include "DaAddressSpaceRoot.h"
#include "Da/ServerDaTransaction.h"


using namespace SoftingOPCToolboxServer;

class Transaction : public DaTransaction
{

public:

	Transaction(
		EnumTransactionType aTransactionType,
		std::vector<DaRequest*>& aRequestList,
		unsigned long aSessionKey);

	virtual ~Transaction();

	virtual long handleReadRequests(void);

	virtual long handleWriteRequests(void);

};  //  end class Transaction

#endif  //  _TRANSACTION_H_
