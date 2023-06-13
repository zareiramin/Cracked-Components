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
//  Filename    : ServerDaRequest.h                                           |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC DA client generated request handler class               |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _SERVERDAREQUEST_H_
#define _SERVERDAREQUEST_H_

#include "../../Enums.h"
#include "../ServerEnums.h"
#include "../../ValueQT.h"

#pragma pack(push,4)

namespace SoftingOPCToolboxServer
{

class DaAddressSpaceElement;
class DaTransaction;

// Stores data concerning a read- or write-request.
class TBC_EXPORT DaRequest
{

	friend class OTSGlobals;
	friend class DaTransaction;

private:

	unsigned long m_sessionHandle;
	unsigned long m_requestHandle;
	unsigned long m_transactionKey;
	EnumTransactionType m_transactionType;

	int m_propertyId;

	DaAddressSpaceElement* m_addressSpaceElement;

	long m_result;
	EnumRequestState m_requestState;
	ValueQT m_value;

protected:

	unsigned long getRequestHandle(void)
	{
		return m_requestHandle;
	}
	unsigned long getSessionHandle(void)
	{
		return m_sessionHandle;
	}

public:

	DaRequest(EnumTransactionType aTransactionType,
			  unsigned long aSessionHandle,
			  DaAddressSpaceElement* anElement,
			  int aPropertyId,
			  unsigned long aRequestHandle);

	virtual ~DaRequest();

	// The property ID the request is about (0 means no property request)
	int getPropertyId(void)
	{
		return m_propertyId;
	}

	// The namespace element associated to the request
	DaAddressSpaceElement* getAddressSpaceElement(void)
	{
		return m_addressSpaceElement;
	}

	//  the getTransactionType
	EnumTransactionType getTransactionType(void)
	{
		return m_transactionType;
	}

	// The result of the request
	long getResult(void)
	{
		return m_result;
	}
	void setResult(long aValue)
	{
		m_result = aValue;
	}

	// the state of the request
	EnumRequestState getRequestState(void)
	{
		return m_requestState;
	}
	void setRequestState(EnumRequestState aValue)
	{
		m_requestState = aValue;
	}

	// the transaction the request is contained in
	unsigned long getTransactionKey(void)
	{
		return m_transactionKey;
	}
	void setTransactionKey(unsigned long aValue)
	{
		m_transactionKey = aValue;
	}

	// This requests value with quality and timestamp.
	ValueQT* getValue(void)
	{
		return &m_value;
	}
	void setValue(ValueQT& aValue)
	{
		m_value = aValue;
	}

	// Complete only this request
	long complete(void);

};  //  end DaRequest

}   //  end namespace SoftingOPCToolboxServer

#pragma pack(pop)
#endif  //  _DAREQUEST_H_

