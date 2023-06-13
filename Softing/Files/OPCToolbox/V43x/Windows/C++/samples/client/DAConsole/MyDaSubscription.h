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
//  Filename    : MyDaSubscription.h                                          |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic DataAccess OPC Client                      |
//                subscription definition                                     |
//                                                                            |
//  This code and information is provided "as is" without warranty of         |
//  any kind, either expressed or implied, including but not limited          |
//  to the implied warranties of merchantability and/or fitness for a         |
//  particular purpose.                                                       |
//                                                                            |
//-----------------------------------------------------------------------------
#ifndef _MYDASUBSCRIPTION_H_
#define _MYDASUBSCRIPTION_H_

#ifdef TBC_OS_WINDOWS
#include "Da\ClientDaSubscription.h"
#endif

#ifdef TBC_OS_LINUX
#include "ClientDaSubscription.h"
#endif

class MyDaSubscription :
	public SoftingOPCToolboxClient::DaSubscription
{
public:

	MyDaSubscription(unsigned long updateRate, MyDaSession* parentSession)
		: DaSubscription(updateRate, parentSession) {}

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


	void handleDataChanged(
		const std::vector<DaItem*>& items,
		const std::vector<ValueQT*>& values,
		const std::vector<long>& results)
	{
		unsigned long i;
		size_t count = items.size();
		_tprintf(_T("\n%s - Data changed\n"), _T("Subscription"));

		for (i = 0; i < count; i++)
		{
			_tprintf(_T("%-23.23s - %s\n"), items[i]->getId().c_str(), values[i]->toString().c_str());
		} //end for
	}

	void handleReadCompleted(
		unsigned long executionContext,
		const std::vector<DaItem*>& items,
		const std::vector<ValueQT*>& values,
		const std::vector<long>& results,
		long result)
	{
		unsigned long i;
		size_t itemCount = items.size();
		_tprintf(_T("\nSubscription - Read complete\n"));

		if (SUCCEEDED(result))
		{
			for (i = 0; i < itemCount; i++)
			{
				tstring id = items[i]->getId();

				if (SUCCEEDED(results[i]))
				{
					_tprintf(_T("%-23.23s - %s\n"), id.c_str(), values[i]->toString().c_str());
				}
				else
				{
					_tprintf(_T("Read failed - %s [0x%8.8X]\n"), id.c_str(), (unsigned int)results[i]);
				}//end if...else
			}//end for
		}
		else
		{
			_tprintf(_T("Read failed [0x%8.8X]\n"), (unsigned int)result);
		}//end if...else
	} //end handleReadCompleted

	void  handleWriteCompleted(
		unsigned long executionContext,
		const std::vector<DaItem*>& items,
		const std::vector<ValueQT*>& values,
		const std::vector<long>& results,
		long result)
	{
		_tprintf(_T("\nSubscription write complete\n"));

		if (SUCCEEDED(result))
		{
			unsigned long i;
			size_t itemCount = items.size();

			for (i = 0; i < itemCount; i++)
			{
				if (SUCCEEDED(results[i]))
				{
					_tprintf(_T("%-23.23s - Write successfull\n"), items[i]->getId().c_str());
				}
				else
				{
					_tprintf(_T("Write failed - %s [0x%8.8X]\n"), items[i]->getId().c_str(), (unsigned int)results[i]);
				}
			}
		}
		else
		{
			_tprintf(_T("Write failed [0x%8.8X]\n"), (unsigned int)result);
		}
	} //end handleWriteCompleted

	virtual ~MyDaSubscription()
	{} // end dtor

};  // end class MyDaSubscription

#endif
