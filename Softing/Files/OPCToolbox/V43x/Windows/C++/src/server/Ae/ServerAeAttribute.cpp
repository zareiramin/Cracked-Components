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
//  Filename    : ServerAeAttribute.cpp                                       |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Defines the OPC AE Attribute class                          |
//                                                                            |
//-----------------------------------------------------------------------------

#include "OSCompat.h"
#include "ServerAeAttribute.h"

using namespace SoftingOPCToolboxServer;

//-----------------------------------------------------------------------------
// Constructor
//
AeAttribute::AeAttribute(
	unsigned long anId,
	tstring& aName,
	VARENUM aDataType):
	m_id(anId),
	m_name(aName),
	m_dataType(aDataType)
{
}   //  end ctr
