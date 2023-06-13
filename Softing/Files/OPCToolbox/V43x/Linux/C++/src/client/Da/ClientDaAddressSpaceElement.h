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
//  Filename    : ClientDaAddressSpaceElement.h                               |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Address space element in a DA Server address space.         |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _CLIENTDAADDRESSSPACEELEMENT_H_
#define _CLIENTDAADDRESSSPACEELEMENT_H_

#include "../../Enums.h"
#include "../ClientAddressSpaceElement.h"

#include <vector>

#pragma pack(push,4)

namespace SoftingOPCToolboxClient
{

class DaSession;
class DaAddressSpaceElementBrowseOptions;
class DaGetPropertiesOptions;
class DaProperty;

class TBC_EXPORT DaAddressSpaceElement : public AddressSpaceElement
{

	friend class DaSession;
	friend class OTCGlobals;

protected:
	DaSession* m_session;
	tstring m_itemPath;

public:
	DaAddressSpaceElement(
		EnumAddressSpaceElementType aType,
		tstring aName,
		tstring anItemId,
		unsigned long anObjectHandle,
		tstring anItemPath,
		DaSession* aSession);

	virtual ~DaAddressSpaceElement();

	tstring& getItemId();
	tstring& getItemPath();
	DaSession* getSession();
	bool isItem(void);

	virtual long browse(
		DaAddressSpaceElementBrowseOptions* browseOptions,
		std::vector<DaAddressSpaceElement*>& addressSpaceElements,
		ExecutionOptions* someExecutionOptions);

	virtual long getDaProperties(
		DaGetPropertiesOptions* aGetPropertiesOptions,
		std::vector<DaProperty*>& someDaProperty,
		ExecutionOptions* someExecutionOptions);

	virtual long getDaProperties(
		std::vector<DaProperty*>& someDaProperty,
		ExecutionOptions* someExecutionOptions);

}; // end DaAddressSpaceElement
} // end SoftingOPCToolboxClient

#pragma pack(pop)
#endif
