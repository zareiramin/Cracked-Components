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
//                            OPC TOOLBOX - C++                               |
//                                                                            |
//  Filename    : ClientAddressSpaceElement.h                                 |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Generic object that represents AE and DA address space      |
//                 elements                                                   |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _CLIENTADDRESSSPACEELEMENT_H_
#define _CLIENTADDRESSSPACEELEMENT_H_

#include "../Enums.h"

#include "ClientEnums.h"
#include <vector>

#pragma pack(push,4)

namespace SoftingOPCToolboxClient
{

class TBC_EXPORT AddressSpaceElement
{
	friend class AeSession;
	friend class OTCGlobals;

protected:
	EnumAddressSpaceElementType m_type;
	tstring m_name;
	tstring m_qName;
	unsigned long m_objectElementHandle;

public:
	AddressSpaceElement(
		EnumAddressSpaceElementType aType,
		tstring aName,
		tstring aQName,
		unsigned long anObjectHandle);

public:
	virtual ~AddressSpaceElement();
	unsigned long getHandle();
	tstring& getName();
	tstring& getQualifiedName();
	bool isBranch(void);
	bool isLeaf(void);

};  //  end class AddressSpaceElement

}   // end SoftingOPCToolboxClient

#pragma pack(pop)
#endif  //  _CLIENTADDRESSSPACEELEMENT_H_
