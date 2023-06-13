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
//  Filename    : MyAeSession.h                                               |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic Alarms and Events OPC Client               |
//                session definition                                          |
//                                                                            |
//  This code and information is provided "as is" without warranty of         |
//  any kind, either expressed or implied, including but not limited          |
//  to the implied warranties of merchantability and/or fitness for a         |
//  particular purpose.                                                       |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _MYAESESSION_H_
#define _MYAESESSION_H_

#include "Ae\ClientAeSession.h"

class MyAeSession :
	public SoftingOPCToolboxClient::AeSession
{

public:

	MyAeSession(const tstring& url) :
		SoftingOPCToolboxClient::AeSession(url) {}

	unsigned char handleShutdownRequest(const tstring& reason)
	{
		_tprintf(_T("Session shutdown - reason: %s\n"), reason.c_str());
		return TRUE; // reconnect automatically
	}

	void handleQueryAeSourceConditionsCompleted(
		unsigned long executioncontext,
		tstring& sourceName,
		std::vector<tstring>& conditionNames,
		long result)
	{
		unsigned long i;

		if (SUCCEEDED(result))
		{
			for (i = 0; i < conditionNames.size(); i++)
			{
				_tprintf(_T("	[%i]%s"), i, conditionNames[i].c_str());
				_tprintf(_T("\n"));
			} //end for
		} //end if
		else
		{
			_tprintf(_T(" Failed to asynchronously query source conditions for source: %s"), sourceName.c_str());
		}//end if ... else
	} //end handleGetDaPropertiesCompleted

	virtual ~MyAeSession()
	{} // dtor

}; // end MySession

#endif
