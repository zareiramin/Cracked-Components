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
//  Filename    : MyDaSession.cpp                                             |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic DataAccess OPC Client                      |
//                session implementation                                      |
//                                                                            |
//  This code and information is provided "as is" without warranty of         |
//  any kind, either expressed or implied, including but not limited          |
//  to the implied warranties of merchantability and/or fitness for a         |
//  particular purpose.                                                       |
//                                                                            |
//-----------------------------------------------------------------------------
#include "stdafx.h"
#include "MyDaSession.h"
#include "DABrowseDlg.h"

MyDaSession::MyDaSession(const tstring& url, CDABrowseDlg* dialog): DaSession(url)
{
	m_connected = false;
	m_dialog = dialog;
}

bool MyDaSession::isConnected() const
{
	return m_connected;
}

void MyDaSession::handleStateChangeCompleted(EnumObjectState state)
{
	if (state == EnumObjectState_CONNECTED)
	{
		m_connected = true;
		if ((m_dialog != NULL) && (m_dialog->m_ready))
		{
			m_dialog->m_label.SetWindowText(_T("Connected"));
		}
	}
	else
	{
		m_connected = false;
		if ((m_dialog != NULL) && (m_dialog->m_ready))
		{
			m_dialog->m_label.SetWindowText(_T("Disconnected"));
		}
	}
}

