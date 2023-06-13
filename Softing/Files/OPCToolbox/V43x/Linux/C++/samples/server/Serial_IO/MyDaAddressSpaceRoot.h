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
//  Filename    : MyDaAddressSpaceRoot.h                                      |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic DataAccess Server OPC Server               |
//                address space root element definition                       |
//                                                                            |
//-----------------------------------------------------------------------------
#ifndef _MYDAADDRESSSPACEROOT_H_
#define _MYDAADDRESSSPACEROOT_H_

#include "Da/ServerDaAddressSpaceElement.h"

using namespace SoftingOPCToolboxServer;

//-----------------------------------------------------------------------------
// MyDaAddressSpaceRoot
//-----------------------------------------------------------------------------
class MyDaAddressSpaceRoot : public DaAddressSpaceRoot
{

public:
	long queryAddressSpaceElementData(
		tstring& anElementID,
		AddressSpaceElement* anElement)
	{
		//  TODO: add string based address space validations
		return E_NOTIMPL;
	}   //  end QueryAddressSpaceElementData

};  //  end class MyDaAddressSpaceRoot

#endif
