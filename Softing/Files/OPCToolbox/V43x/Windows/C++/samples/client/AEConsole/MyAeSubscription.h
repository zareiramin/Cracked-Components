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
//                          OPC TOOLBOX C++ - Samples                         |
//                                                                            |
//  Filename    : MyAeSubscription.h                                          |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic Alarms and Events OPC Client               |
//                subscription definition                                     |
//                                                                            |
//  This code and information is provided "as is" without warranty of         |
//  any kind, either expressed or implied, including but not limited          |
//  to the implied warranties of merchantability and/or fitness for a         |
//  particular purpose.                                                       |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _MYAESUBSCRIPTION_H_
#define _MYAESUBSCRIPTION_H_

#include "Ae\ClientAeSubscription.h"
#include "Ae\ClientAeCondition.h"

class MyAeSubscription :
	public SoftingOPCToolboxClient::AeSubscription
{
public:

	MyAeSubscription(SoftingOPCToolboxClient::AeSession* parentSession) :
		SoftingOPCToolboxClient::AeSubscription(parentSession)
	{}

	virtual ~MyAeSubscription()
	{} // dtor

	void handleStateChangeCompleted(EnumObjectState state)
	{
		tstring stateToString;

		switch (state)
		{
		case EnumObjectState_DISCONNECTED:
			stateToString = _T("DISCONNECTED");
			break;

		case EnumObjectState_CONNECTED:
			stateToString = _T("CONNECTED");
			break;

		case EnumObjectState_ACTIVATED:
			stateToString = _T("ACTIVATED");
		} //end switch

		_tprintf(_T("%s state changed - %s\n"), _T("Subscription"), stateToString.c_str());
	} //end handleStateChangeCompleted


	void handlePerformStateTransitionCompleted(
		unsigned long executionContext,
		long result)
	{
		_tprintf(_T("\n"));

		if (SUCCEEDED(result))
		{
			_tprintf(_T("%s performed state transition - context: %lu\n"), _T("Subscription"), executionContext);
		} //end if
		else
		{
			_tprintf(_T(" performed state transition failed"));
		}
	} //handlePerformStateTransitionCompleted

	tstring eventTypeToString(EnumEventType eventType)
	{
		tstring eventTypeToString(_T(""));

		if ((eventType & EnumEventType_SIMPLE) == EnumEventType_SIMPLE)
		{
			eventTypeToString += _T("SIMPLE");
		}//end if

		if ((eventType & EnumEventType_CONDITION) == EnumEventType_CONDITION)
		{
			eventTypeToString += _T("CONDITION");
		}//end if

		if ((eventType & EnumEventType_TRACKING) == EnumEventType_TRACKING)
		{
			eventTypeToString += _T("TRACKING");
		}//end if

		return eventTypeToString;
	}

	void handleAeEventsReceived(
		BOOL isRefresh,
		BOOL lastRefresh,
		const std::vector<AeEvent*>& events)
	{
		size_t count = events.size();
		size_t attributesCount;
		unsigned long i, j;
		_tprintf(_T("\n%s received events - %lu\n"), _T(" Subscription"), count);

		for (i = 0; i < count; i++)
		{
			if ((events[i]->getEventType() == EnumEventType_SIMPLE) || (events[i]->getEventType() == EnumEventType_TRACKING))
			{
				tstring toString = eventTypeToString(events[i]->getEventType());
				_tprintf(_T("[%3.3lu]  Event type: %s  Event category: %lu\n"), i, toString.c_str(), events[i]->getCategory());
				_tprintf(_T("       Source path: %s\n"), events[i]->getSourcePath().c_str());
				_tprintf(_T("       Message: %s\n"), events[i]->getMessage().c_str());
				_tprintf(_T("       Occurence time: %s\n"), events[i]->getOcurrenceTime().toString().c_str());
				_tprintf(_T("       Severity: %lu\n"), events[i]->getSeverity());
				_tprintf(_T("       Actor id: %s\n"), events[i]->getActorId().c_str());
				std::vector<Variant*>attributes = events[i]->getAttributes();
				attributesCount = attributes.size();
				_tprintf(_T("       Attributes: %lu\n"), attributesCount);

				for (j = 0; j < attributesCount; j++)
				{
					_tprintf(_T("         [%3.3lu] %s"), j, attributes[j]->toString().c_str());
				} //end for
			} //end if

			if (events[i]->getEventType() == EnumEventType_CONDITION)
			{
				tstring toString = eventTypeToString(events[i]->getEventType());
				_tprintf(_T("[%3.3lu]  Event type: %s  Event category: %lu\n"), i, toString.c_str(), events[i]->getCategory());
				_tprintf(_T("       Source path: %s\n"), events[i]->getSourcePath().c_str());
				_tprintf(_T("       Message: %s\n"), events[i]->getMessage().c_str());
				_tprintf(_T("       Occurence time: %s\n"), events[i]->getOcurrenceTime().toString().c_str());
				_tprintf(_T("       Severity: %lu\n"), events[i]->getSeverity());
				_tprintf(_T("       Actor id: %s\n"), events[i]->getActorId().c_str());
				_tprintf(_T("       Change mask: %u\n"), ((AeCondition*)events[i])->getChangeMask());
				_tprintf(_T("       New state: %u\n"), (WORD)((AeCondition*)events[i])->getState());
				_tprintf(_T("       Ack Required: %u\n"), ((AeCondition*)events[i])->getAckRequired());
				_tprintf(_T("       Quality: %u\n"), ((AeCondition*)events[i])->getQuality());
				_tprintf(_T("       Condition name: %s\n"), ((AeCondition*)events[i])->getConditionName().c_str());
				_tprintf(_T("       Sub condition name: %s\n"), ((AeCondition*)events[i])->getSubConditionName().c_str());
				_tprintf(_T("       Active time: %s\n"), (((AeCondition*)events[i])->getActiveTime()).toString().c_str());
				std::vector<Variant*>attributes = events[i]->getAttributes();
				attributesCount = attributes.size();
				_tprintf(_T("       Attributes: %lu\n"), attributesCount);

				for (j = 0; j < attributesCount; j++)
				{
					_tprintf(_T("         [%3.3lu] %s"), j, attributes[j]->toString().c_str());
				} //end for
			} //end if
		} //end for
	} //end handleAeEventsReceived

};  // end class MyAeSubscription

#endif
