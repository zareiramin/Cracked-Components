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

#ifdef TBC_OS_WINDOWS
#include "Da\ClientDaSession.h"
#endif

#ifdef TBC_OS_LINUX
#include "ClientDaSession.h"
#endif

class MyDaSession :
	public SoftingOPCToolboxClient::DaSession
{
public:

	MyDaSession(const tstring& url) :
		SoftingOPCToolboxClient::DaSession(url) {}

	virtual ~MyDaSession()
	{} // end dtor

	unsigned char handleShutdownRequest(const tstring& reason)
	{
		// Log the result
		// reconnect automatically
		return TRUE;
	} // end handleShutdownRequest

	void print()
	{
		_tprintf(_T("\n%s\n"), _T("Session"));
		_tprintf(_T("  URL: %s\n"), getUrl().c_str());
		_tprintf(_T("  Client Name: %s\n"), getClientName().c_str());
		_tprintf(_T("  LCID: %s\n"), getLocaleId().c_str());
		tstring supportedSpec = GetOPCSpecificationString(getSupportedOpcSpecification());
		tstring forcedSpec = GetOPCSpecificationString(getForcedOpcSpecification());
		_tprintf(_T("  Supported OPC Specification: %s\n"), supportedSpec.c_str());
		_tprintf(_T("  Forced OPC Specification: %s\n"), forcedSpec.c_str());
	}// end print

	tstring GetOPCSpecificationString(EnumOPCSpecification specification)
	{
		tstring spec;

		switch (specification)
		{
		case EnumOPCSpecification_DEFAULT:
			spec = _T("default");
			break;

		case EnumOPCSpecification_DA10:
			spec = _T("DA1");
			break;

		case EnumOPCSpecification_DA20:
			spec = _T("DA2");
			break;

		case EnumOPCSpecification_DA30 :
			spec = _T("DA3");
			break;

		case EnumOPCSpecification_XMLDA10:
			spec = _T("XML-DA");
			break;

		case EnumOPCSpecification_AE10:
			//nothing to do
			break;
		} //end switch

		return spec;
	}

}; // end class MyDaSession

#endif
