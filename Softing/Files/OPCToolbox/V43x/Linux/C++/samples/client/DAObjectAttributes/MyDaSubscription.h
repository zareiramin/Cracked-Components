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

	MyDaSubscription(
		unsigned long updateRate,
		SoftingOPCToolboxClient::DaSession* parentSession) :
		SoftingOPCToolboxClient::DaSubscription(updateRate, parentSession)
	{}

	virtual ~MyDaSubscription()
	{} // end dtor

	void print()
	{
		_tprintf(_T("\n%s\n"), _T("Subscription"));
		_tprintf(_T("  Name: %s\n"), getName().c_str());
		_tprintf(_T("  Requested Update Rate: %lu ms\n"), getRequestedUpdateRate());
		_tprintf(_T("  Revised Update Rate: %lu ms\n"), getRevisedUpdateRate());
		_tprintf(_T("  Deadband: %g\n"), getDeadband());
		_tprintf(_T("  Time Bias: %ld\n"), getTimeBias());
		_tprintf(_T("  Requested Keep Alive Time: %lu ms\n"), getRequestedKeepAliveTime());
		_tprintf(_T("  Revised Keep Alive Time: %lu ms\n"), getRevisedKeepAliveTime());
		_tprintf(_T("  LCID: %s\n"), getLocaleId().c_str());
	}// end print

};  // end class MyDaSubscription

#endif
