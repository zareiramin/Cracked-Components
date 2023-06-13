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

#include "Da\ClientDaSubscription.h"

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
	{}  //  end dtor

};  //  end class MyDaSubscription

#endif
