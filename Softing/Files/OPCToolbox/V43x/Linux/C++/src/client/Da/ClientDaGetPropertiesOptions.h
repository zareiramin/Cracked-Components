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
//  Filename    : ClientDaGetPropertiesOptions.h                              |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Options class for browsing an OPC Item's properties         |
//                                                                            |
//-----------------------------------------------------------------------------


#ifndef _CLIENTDAGETPROPERTIESOPTIONS_H_
#define _CLIENTDAGETPROPERTIESOPTIONS_H_

#include "../../Enums.h"
#include "../ClientEnums.h"

#include <vector>
#pragma pack(push,4)

namespace SoftingOPCToolboxClient
{

class TBC_EXPORT  DaGetPropertiesOptions
{

private:
protected:

	EnumPropertyData m_whatPropertyData;
	std::vector<unsigned long> m_pPropertyIds;
	std::vector<tstring> m_pPropertyNames;

public:

	DaGetPropertiesOptions();
	virtual ~DaGetPropertiesOptions();

	EnumPropertyData getWhatPropertyData();
	void setWhatPropertyData(EnumPropertyData somePropertyData);

	std::vector<unsigned long>& getPropertyIds();
	void setPropertyIds(std::vector<unsigned long>& propertyIds);

	std::vector<tstring>& getPropertyNames();
	void setPropertyNames(std::vector<tstring>& propertyNames);

};  //end class DaGetPropertiesOptions
};// end SoftingOPCToolboxClient

#pragma pack(pop)
#endif
