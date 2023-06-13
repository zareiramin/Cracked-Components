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
//  Filename    : ClientDaGetPropertiesOptions.cpp                            |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Options class for browsing an OPC Item's properties         |
//                                                                            |
//-----------------------------------------------------------------------------

#include "OSCompat.h"
#include "ClientDaGetPropertiesOptions.h"

using namespace SoftingOPCToolboxClient;

DaGetPropertiesOptions::DaGetPropertiesOptions()
{
	m_whatPropertyData = (EnumPropertyData)0;
} //end constructor

DaGetPropertiesOptions::~DaGetPropertiesOptions()
{
}//end destructor

EnumPropertyData DaGetPropertiesOptions::getWhatPropertyData()
{
	return m_whatPropertyData;
} //end getWhatPropertyData
void DaGetPropertiesOptions::setWhatPropertyData(EnumPropertyData somePropertyData)
{
	m_whatPropertyData = somePropertyData;
} //end
std::vector<unsigned long>& DaGetPropertiesOptions::getPropertyIds()
{
	return m_pPropertyIds;
} //end
void DaGetPropertiesOptions::setPropertyIds(std::vector<unsigned long>& propertyIds)
{
	m_pPropertyIds = propertyIds;
} //end
std::vector<tstring>& DaGetPropertiesOptions::getPropertyNames()
{
	return m_pPropertyNames;
} //end
void DaGetPropertiesOptions::setPropertyNames(std::vector<tstring>& propertyNames)
{
	m_pPropertyNames = propertyNames;
} //end
