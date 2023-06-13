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
//  Filename    : EUEngineeringUnits.h                                        |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic DataAccess Server OPC Server               |
//                address space element definition                            |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _H_EU_ENGINEERING_H
#define _H_EU_ENGINEERING_H

#include "Da/ServerDaRequest.h"
#include "Da/ServerDaProperty.h"

using namespace SoftingOPCToolboxServer;

// Engineering value property
class EUEngineeringUnits : public DaProperty
{

protected:
	ValueQT m_value;

public:
	EUEngineeringUnits(
		int anId,
		tstring& aName,
		tstring& anItemId,
		ValueQT& aValue);
	virtual ~EUEngineeringUnits();

	ValueQT& getValue();
	void setValue(ValueQT value);

};  //  end EUEngineeringUnits

#endif
