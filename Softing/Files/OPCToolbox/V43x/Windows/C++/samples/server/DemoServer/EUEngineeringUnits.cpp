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
//  Filename    : EUEngineeringUnits.cpp                                      |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description :                                                             |
//                                                                            |
//-----------------------------------------------------------------------------

#include "stdafx.h"
#include "EUEngineeringUnits.h"

EUEngineeringUnits::EUEngineeringUnits(
	int anId,
	tstring& aName,
	tstring& anItemId,
	ValueQT& aValue) : DaProperty()
{
	m_id        = anId;
	m_name  = aName;
	m_itemId    = anItemId;
	m_value = aValue;
}

EUEngineeringUnits::~EUEngineeringUnits()
{
}

ValueQT& EUEngineeringUnits::getValue()
{
	return m_value;
}

void EUEngineeringUnits::setValue(ValueQT value)
{
	m_value = value;
}
