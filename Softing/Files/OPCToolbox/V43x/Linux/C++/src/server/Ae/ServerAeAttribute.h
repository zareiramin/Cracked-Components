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
//  Filename    : ServerAeAttribute.h                                         |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Defines the OPC AE Attribute class                          |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _SERVERAEATTRIBUTE_H_
#define _SERVERAEATTRIBUTE_H_

#include "../../Enums.h"

#pragma pack(push,4)

namespace SoftingOPCToolboxServer
{

class TBC_EXPORT AeAttribute
{

private:
	unsigned long m_id;
	tstring m_name;
	VARENUM m_dataType;

public:
	AeAttribute(
		unsigned long anId,
		tstring& aName,
		VARENUM aDataType);

	unsigned long getId(void)
	{
		return m_id;
	}
	void setId(unsigned long aValue)
	{
		m_id = aValue;
	}

	tstring& getName(void)
	{
		return m_name;
	}
	void setName(tstring& aValue)
	{
		m_name = aValue;
	}

	VARENUM getDataType(void)
	{
		return m_dataType;
	}
	void setDataType(VARENUM  aValue)
	{
		m_dataType = aValue;
	}

};  //  end class Attribute

}   //  end namespace SoftingOPCToolboxServer

#pragma pack(pop)
#endif  //  _AEATTRIBUTE_H_
