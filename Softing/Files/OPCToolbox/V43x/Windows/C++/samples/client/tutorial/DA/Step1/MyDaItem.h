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
//                             OPC Toolbox C++                                |
//                                                                            |
//  Filename    : MyDaItem.h                                                  |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic DataAccess OPC Client                      |
//                item definition                                             |
//                                                                            |
//-----------------------------------------------------------------------------
#ifndef _MYDAITEM_H_
#define _MYDAITEM_H_

#include "Da\ClientDaItem.h"

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
	{}  //  end dtor

};  //  end class MyDaItem

#endif
