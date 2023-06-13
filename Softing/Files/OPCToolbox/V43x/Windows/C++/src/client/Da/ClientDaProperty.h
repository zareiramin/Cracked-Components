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
//  Filename    : ClientDaProperty.h                                          |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Defines a DA Item's property                                |
//                                                                            |
//-----------------------------------------------------------------------------


#ifndef _CLIENTDAPROPERTY_H_
#define _CLIENTDAPROPERTY_H_

#include "../../Enums.h"
#include "../ClientEnums.h"
#include "../../ValueQT.h"

#pragma pack(push,4)

namespace SoftingOPCToolboxClient
{

class TBC_EXPORT DaProperty
{

	friend class DaSession;
	friend class OTCGlobals;

private:

	DaProperty(
		unsigned long anId,
		tstring aName,
		tstring anItemId,
		tstring anItemPath,
		tstring aDescription,
		unsigned short aDataType,
		ValueQT* aValue,
		long aResult);

protected:

	unsigned long m_id;
	tstring m_name;
	tstring m_itemId;
	tstring m_itemPath;
	tstring m_description;
	VARTYPE m_dataType;
	ValueQT* m_value;
	long m_result;

public:

	DaProperty();
	virtual ~DaProperty();

	unsigned long getId();

	tstring& getName();

	tstring& getItemId();

	tstring& getItemPath();

	tstring& getDescription();

	VARTYPE getDataType();

	ValueQT& getValueQT();

	long getResult();

}; // end DaProperty
};// end SoftingOPCToolboxClient

#pragma pack(pop)
#endif
