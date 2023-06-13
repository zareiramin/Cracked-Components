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
//  Filename    : ServerAeAddressSpaceElement.h                               |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC AE Address space elements definition classes            |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _SERVERAEADDRESSSPACEELEMENT_H_
#define _SERVERAEADDRESSSPACEELEMENT_H_

#include "../../Enums.h"
#include "../ServerEnums.h"
#include "../../ValueQT.h"
#include "../ServerAddressSpaceElement.h"

#include <vector>

#pragma pack(push,4)

namespace SoftingOPCToolboxServer
{

// AddressSpaceElement Class
class TBC_EXPORT AeAddressSpaceElement : public AddressSpaceElement
{

	friend class Creator;
	friend class AeAddressSpaceRoot;
protected:
	AeAddressSpaceElement();

	AeAddressSpaceElement(
		tstring aName,
		unsigned long anUserData,
		unsigned long anObjectHandle,
		unsigned long aParentHandle);
public:

	virtual ~AeAddressSpaceElement();

	virtual BOOL addChild(AddressSpaceElement* aChild);
	virtual BOOL removeChild(AddressSpaceElement* aChild);
	virtual std::vector<AddressSpaceElement*> getChildren(void);

	virtual AddressSpaceElement* getParent(void);

	virtual long queryConditions(
		tstring& aSourcePath,
		std::vector<tstring>& aConditionNames);

};  //  end class AeAddressSpaceElement


// AddressSpaceRoot: Represents the namespaces root
class TBC_EXPORT AeAddressSpaceRoot : public AddressSpaceRoot
{

	friend class AeAddressSpaceElement;
	friend class Creator;

protected:
	AeAddressSpaceRoot(EnumAddressSpaceType anAddressSpaceType);
	AeAddressSpaceRoot(void);
public:

	virtual ~AeAddressSpaceRoot(void);

	virtual long queryAddressSpaceElementData(
		const tstring& anElementId,
		AddressSpaceElement*& anElement);

	virtual long queryAddressSpaceElementChildren(
		const tstring& anElementId,
		std::vector<AddressSpaceElement*>& aChildrenList);

	virtual long queryConditions(
		unsigned long anElementUserData,
		tstring& aSourcePath,
		std::vector<tstring>& aConditionNames);

};  //  end class AeAddressSpaceRoot

};  //  end namespace SoftingOPCToolboxServer

#pragma pack(pop)
#endif  //  _AEADDRESSSPACEELEMENT_H_
