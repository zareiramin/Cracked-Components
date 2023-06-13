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

#include <fstream>
typedef std::basic_ofstream<TCHAR, std::char_traits<TCHAR> > tofstream;

#include "Da\ClientDaItem.h"

class MyDaItem :
	public SoftingOPCToolboxClient::DaItem
{

private:
	tofstream m_file;
public:

	MyDaItem(
		tstring itemId,
		SoftingOPCToolboxClient::DaSubscription* parentSubscription) :
		DaItem(itemId, parentSubscription)
	{}

	virtual ~MyDaItem()
	{} // end dtor

	void handleStateChangeCompleted(SoftingOPCToolboxClient::EnumObjectState state)
	{
		tstring stateToString;

		switch (state)
		{
		case SoftingOPCToolboxClient::EnumObjectState_DISCONNECTED:
			stateToString = _T("DISCONNECTED");
			break;

		case SoftingOPCToolboxClient::EnumObjectState_CONNECTED:
			stateToString = _T("CONNECTED");
			break;

		case SoftingOPCToolboxClient::EnumObjectState_ACTIVATED:
			stateToString = _T("ACTIVATED");
		} //end switch

		// TODO change the name of the file where to write the results
		m_file.open("C:\\Service_TraceData.txt", tofstream::in  | tofstream::out  | tofstream::app);
		m_file << "\n" << _T("Item ") << this->getId().c_str() << _T(" state changed - ") << stateToString.c_str();
		m_file.close();
	} //end handleStateChangeCompleted

};  //  end class MyDaItem

#endif
