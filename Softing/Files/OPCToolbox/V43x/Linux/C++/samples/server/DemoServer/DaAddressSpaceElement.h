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
//  Filename    : DAAddressSpaceElement.h                                     |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic DataAccess Server OPC Server               |
//                address space element definition                            |
//                                                                            |
//-----------------------------------------------------------------------------
#ifndef _DEMODAADDRESSSPACEELEMENT_H_
#define _DEMODAADDRESSSPACEELEMENT_H_

#include "Da/ServerDaAddressSpaceElement.h"
#include "Da/ServerDaProperty.h"

using namespace SoftingOPCToolboxServer;

class DemoDaAddressSpaceElement : public DaAddressSpaceElement
{

public:

	DemoDaAddressSpaceElement(
		tstring& anItemID,
		tstring& aName,
		unsigned long anUserData,
		unsigned long anObjectHandle,
		unsigned long aParentHandle);

	DemoDaAddressSpaceElement(void);

	virtual ~DemoDaAddressSpaceElement(void);

	virtual void addedToAddressSpace(void);

	virtual void removedFromAddressSpace(void);

	void getPropertyValue(DaRequest* pRequest);

	long queryProperties(std::vector<DaProperty*>& aPropertyList);

	long addProperty(DaProperty* pAProperty);

	virtual void simulation(void);

	virtual void handleReadRequest(IN DaRequest* pRequest) = 0;

	virtual void handleWriteRequest(IN DaRequest* pRequest) = 0;

	virtual void init();

private:
	std::vector<DaProperty*> m_properties;

};  //  end class DemoDaAddressSpaceElement

#endif
