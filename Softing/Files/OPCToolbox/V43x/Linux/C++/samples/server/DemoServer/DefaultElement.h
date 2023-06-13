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
//  Filename    : DefaultElement.h                                            |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic DataAccess Server OPC Server               |
//                address space element definition                            |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _DEFAULT_ELEMENT_
#define _DEFAULT_ELEMENT_

#include "DaAddressSpaceElement.h"
#include "Da/ServerDaRequest.h"

using namespace SoftingOPCToolboxServer;

class BasicElement : public DemoDaAddressSpaceElement
{
public:
	BasicElement(tstring name);

	BasicElement(VARENUM varType);

	void init();

protected:
	virtual void handleReadRequest(IN DaRequest* pRequest);

	virtual void handleWriteRequest(IN DaRequest* pRequest);

	VARENUM m_varType;
};

class BasicStaticElement : public BasicElement
{
public:
	BasicStaticElement(tstring name);

	BasicStaticElement(VARENUM varType);

};

class BasicDymanicElement : public BasicElement
{
public:
	BasicDymanicElement(tstring name);

	BasicDymanicElement(VARENUM varType);

protected:

	virtual void simulation(void);

private:
	void incrementWString(IN BSTR w);

	void incrementDate(IN DATE* pDate);
};

#endif
