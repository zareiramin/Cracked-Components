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
//  Filename    : MyDaSession.h                                               |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic DataAccess OPC Client                      |
//                session definition                                          |
//                                                                            |
//  This code and information is provided "as is" without warranty of         |
//  any kind, either expressed or implied, including but not limited          |
//  to the implied warranties of merchantability and/or fitness for a         |
//  particular purpose.                                                       |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _MYDASESSION_H_
#define _MYDASESSION_H_

#include "ClientApplication.h"

#ifdef TBC_OS_WINDOWS
#include "Da\ClientDaGetPropertiesOptions.h"
#include "Da\ClientDaProperty.h"
#include "Da\ClientDaSession.h"
#endif

#ifdef TBC_OS_LINUX
#include "ClientDaGetPropertiesOptions.h"
#include "ClientDaProperty.h"
#include "ClientDaSession.h"
#endif



using namespace SoftingOPCToolboxClient;


class MyDaSession :
	public SoftingOPCToolboxClient::DaSession
{
public:

	MyDaSession(const tstring& url) : DaSession(url) {}

	unsigned char handleShutdownRequest(const tstring& reason)
	{
		_tprintf(_T("Session shutdown - reason: %s\n"), reason.c_str());
		return TRUE; // reconnect automatically
	}

	void handleGetDaPropertiesCompleted(
		unsigned long executionContext,
		unsigned long addressSpaceElementHandle,
		tstring& addressSpaceElementId,
		tstring& addressSpaceElementPath,
		std::vector<DaProperty*>& someDaProperties,
		long result)
	{
		unsigned long i;

		if (SUCCEEDED(result))
		{
			size_t count = someDaProperties.size();

			for (i = 0; i < count; i++)
			{
				_tprintf(_T("%s%-30.30s\n"), _T("Property Name: "), someDaProperties[i]->getName().c_str());
				_tprintf(_T("%s%5.5d\n"), _T("Property Id: "), (int)someDaProperties[i]->getId());
				_tprintf(_T("%s%-30.30s\n"), _T("Property Item Id: "), someDaProperties[i]->getItemId().c_str());
				_tprintf(_T("%s%5.5d\n"), _T("Property DataType: "), someDaProperties[i]->getDataType());
				_tprintf(_T("%s%-30.30s\n"), _T("Property Description: "), someDaProperties[i]->getDescription().c_str());
				_tprintf(_T("%s%s\n"), _T("Property Value: "), someDaProperties[i]->getValueQT().toString().c_str());
				_tprintf(_T("\n"));
			} //end for
		} //end if
		else
		{
			_tprintf(_T(" Failed to asynchronously get properties of address space element: %s"), addressSpaceElementId.c_str());
		}//end if ... else
	} //end handleGetDaPropertiesCompleted

	virtual ~MyDaSession()
	{} // end dtor

}; // end class MyDaSession

#endif
