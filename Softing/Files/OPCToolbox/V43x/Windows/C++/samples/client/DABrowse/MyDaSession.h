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

#include "ClientServerStatus.h"
#include "resource.h"

#ifdef TBC_OS_WINDOWS
#include "Da\ClientDaSession.h"
#endif

#ifdef TBC_OS_LINUX
#include "ClientDaSession.h"
#endif

using namespace SoftingOPCToolboxClient;

class CDABrowseDlg;

class MyDaSession :
	public SoftingOPCToolboxClient::DaSession
{
public:

	MyDaSession(const tstring& url, CDABrowseDlg* dialog);
	bool isConnected() const;
	void handleStateChangeCompleted(EnumObjectState state);

private:
	bool m_connected;
	CDABrowseDlg* m_dialog;

};

#endif // _MYDASESSION_H_