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
//  Filename    : ClientAeAttribute.h                                         |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Defines the OPC AE Attribute class                          |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _CLIENTAEATTRIBUTE_H_
#define _CLIENTAEATTRIBUTE_H_

#include "../ClientEnums.h"
#include "../../Enums.h"

#pragma pack(push,4)

namespace SoftingOPCToolboxClient
{

class TBC_EXPORT AeAttribute
{

private:

protected:

	unsigned long m_id;
	tstring m_description;
	VARTYPE m_dataType;

public:

	AeAttribute()
	{
		m_id          = 0;
		m_description = _T("");
		m_dataType    = 0;
	} //end constructor

	AeAttribute(
		unsigned long id,
		tstring description,
		unsigned short type)
	{
		m_id = id;
		m_description = description;
		m_dataType = type;
	} //end constructor

	virtual ~AeAttribute() {} //end destructor

	unsigned long getId()
	{
		return m_id;
	} //end getId
	void setId(unsigned long id)
	{
		m_id = id;
	} //end setId

	tstring& getDescription()
	{
		return m_description;
	} //end getDescription
	void setDescription(tstring description)
	{
		m_description = description;
	} //end setDescription

	VARTYPE getDataType()
	{
		return m_dataType;
	} //end getDataType
	void setDataType(VARTYPE dataType)
	{
		m_dataType = dataType;
	} //end setDataType

}; // end class AeAttribute

}; //end namespace OPC_Client


#pragma pack(pop)
#endif
