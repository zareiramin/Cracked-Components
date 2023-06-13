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
//  Filename    : ClientObjectSpaceElement.h                                  |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Client generic class from which all OPC classes are derived |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _CLIENTOBJECTSPACEELEMENT_H_
#define _CLIENTOBJECTSPACEELEMENT_H_

#include "../Enums.h"
#include "ClientEnums.h"

#include "../Trace.h"
#include <vector>

#pragma pack(push,4)

namespace SoftingOPCToolboxClient
{

//The process values read from the server will be taken directly
//from the data source(Device).
const unsigned long MAX_AGE_DEVICE = 0x00000000;
//The process values read from the server will be taken from the
//server's internal cache of the process data(Cache).
const unsigned long MAX_AGE_CACHE  = 0xFFFFFFFF;

class TBC_EXPORT ObjectSpaceElement
{

	friend class AeSession;
	friend class AeSubscription;
	friend class DaSession;
	friend class DaSubscription;
	friend class DaItem;
	friend class Application;
	friend class AeCategory;
	friend class DaAddressSpaceElement;
	friend class OTCGlobals;

private:

	static unsigned long m_objectIndex;
	unsigned long m_index;
	unsigned long m_objectHandle;

	virtual unsigned long getHandle()
	{
		return m_objectHandle;
	} // end getHandle

	virtual void setHandle(unsigned long aHandle)
	{
		m_objectHandle = aHandle;
	} // end setHandle

	void onStateChange(EnumObjectState newState);

	void onPerformStateTransition(
		unsigned long executionContext,
		long result);

	void onGetAttributesFromServer(
		unsigned long executionContext,
		std::vector<EnumObjectAttribute>& whatAttributes,
		std::vector<long>& results,
		long result);

	void onSetAttributesToServer(
		unsigned long executionContext,
		std::vector<EnumObjectAttribute>& whatAttributes,
		std::vector<long>& results,
		long result);

public:

	//In c# the equivalent is GetHashCode()
	virtual unsigned long getUserData(void)
	{
		return m_index;
	} // end getUserData;

	ObjectSpaceElement();
	virtual ~ObjectSpaceElement();

	virtual BOOL getValid();
	virtual EnumObjectState getCurrentState();

	virtual EnumObjectState getTargetState();
	virtual void setTargetState(EnumObjectState aState);

	virtual long performStateTransition(
		BOOL deep,
		ExecutionOptions* someExecutionOptions);

	virtual long connect(
		BOOL deep,
		BOOL active,
		ExecutionOptions* someExecutionOptions);

	virtual long disconnect(ExecutionOptions* someExecutionOptions);

	virtual long getAttributesFromServer(
		std::vector<EnumObjectAttribute> whatAttributes,
		std::vector<long> results,
		ExecutionOptions* someExecutionOptions);

	virtual long setAttributesToServer(
		std::vector<EnumObjectAttribute> whatAttributes,
		std::vector<long> results,
		ExecutionOptions* someExecutionOptions);

	virtual void handleStateChangeCompleted(
		EnumObjectState newState);

	virtual void handlePerformStateTransitionCompleted(
		unsigned long executionContext,
		long result);

	virtual void handleGetAttributesFromServerCompleted(
		unsigned long executionContext,
		std::vector<EnumObjectAttribute>& whatAttributes,
		std::vector<long>& results,
		long result);

	virtual void handleSetAttributesToServerCompleted(
		unsigned long executionContext,
		std::vector<EnumObjectAttribute>& whatAttributes,
		std::vector<long>& results,
		long result);

}; // end ObjectSpaceElement
};// end SoftingOPCToolboxClient

#pragma pack(pop)
#endif
