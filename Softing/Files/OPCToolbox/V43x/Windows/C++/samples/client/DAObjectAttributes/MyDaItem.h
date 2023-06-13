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
//  Filename    : MyDaItem.h                                                  |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic DataAccess OPC Client                      |
//                item definition                                             |
//                                                                            |
//  This code and information is provided "as is" without warranty of         |
//  any kind, either expressed or implied, including but not limited          |
//  to the implied warranties of merchantability and/or fitness for a         |
//  particular purpose.                                                       |
//                                                                            |
//-----------------------------------------------------------------------------
#ifndef _MYDAITEM_H_
#define _MYDAITEM_H_

#ifdef TBC_OS_WINDOWS
#include "Da\ClientDaItem.h"
#endif

#ifdef TBC_OS_LINUX
#include "ClientDaItem.h"
#endif
class MyDaItem :
	public SoftingOPCToolboxClient::DaItem
{
public:

	MyDaItem(
		tstring itemId,
		SoftingOPCToolboxClient::DaSubscription* parentSubscription) :
		DaItem(itemId, parentSubscription)
	{}

	virtual ~MyDaItem()
	{} // end dtor

	void print()
	{
		_tprintf(_T("\n%s\n"), getPath().c_str());
		_tprintf(_T("  ID: %s\n"), getId().c_str());
		_tprintf(_T("  Path: %s\n"), getPath().c_str());
		_tprintf(_T("  Native Datatype: %u\n"), getNativeDataType());
		_tprintf(_T("  Access Rights: %d\n"), getAccessRights());
		_tprintf(_T("  Requested Datatype: %u\n"), getRequestedDataType());
		_tprintf(_T("  Deadband: %g\n"), getDeadband());
		_tprintf(_T("  EU Type: %u\n"), getEUType());
		_tprintf(_T("  EU Info: %s\n"), getEUInfo().toString().c_str());
	}// end print

};  // end class MyDaItem

#endif
