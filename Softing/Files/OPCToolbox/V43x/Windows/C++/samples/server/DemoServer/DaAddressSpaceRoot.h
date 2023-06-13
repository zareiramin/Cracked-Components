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
//  Filename    : DaAddressSpaceRoot.h                                        |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic DataAccess Server OPC Server               |
//                address space root element definition                       |
//                                                                            |
//-----------------------------------------------------------------------------
#ifndef _DEMODAADDRESSSPACEROOT_H_
#define _DEMODAADDRESSSPACEROOT_H_

#include "Da/ServerDaAddressSpaceElement.h"

using namespace SoftingOPCToolboxServer;

class DemoDaAddressSpaceRoot : public DaAddressSpaceRoot
{

public:

	long queryAddressSpaceElementData(
		tstring& anElementID,
		AddressSpaceElement* pAnElement);

};  //  end class MyDaAddressSpaceRoot

#endif
