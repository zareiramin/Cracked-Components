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
//  Filename    : ServerDaProperty.h                                          |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC DA Property handler class                               |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _SERVERDAPROPERTY_H_
#define _SERVERDAPROPERTY_H_

#include "../../Enums.h"
#include "../ServerEnums.h"

#include "ServerDaRequest.h"

#pragma pack(push,4)

namespace SoftingOPCToolboxServer
{

// Represents one NON standard property of an AddressSpace Element
class TBC_EXPORT DaProperty
{

	friend class Application;
protected:

	int m_id;
	tstring m_name;
	tstring m_itemId;
	tstring m_description;
	VARENUM m_datatype;
	EnumAccessRights m_accessRights;

public:
	DaProperty();
	virtual ~DaProperty();

	// The access rights for the clients (read and/or write)
	EnumAccessRights getAccessRights(void)
	{
		return m_accessRights;
	}
	void setAccessRights(EnumAccessRights aValue)
	{
		m_accessRights = aValue;
	}

	// The datatype of the address space element's property e.g. System.String
	VARENUM getDatatype(void)
	{
		return m_datatype;
	}
	void setDatatype(VARENUM aValue)
	{
		m_datatype = aValue;
	}

	// The description what the property is about
	tstring& getDescription(void)
	{
		return m_description;
	}
	void setDescription(tstring& aValue)
	{
		m_description = aValue;
	}

	// Numerical property id as described in the OPC specification
	int getId(void)
	{
		return m_id;
	}
	void setId(int aValue)
	{
		m_id = aValue;
	}

	//  Human readable short string describing the ID
	tstring& getItemId(void)
	{
		return m_itemId;
	}
	void setItemId(tstring& aValue)
	{
		m_itemId = aValue;
	}

	// Name of this property
	tstring& getName(void)
	{
		return m_name;
	}
	void setName(tstring& aValue)
	{
		m_name = aValue;
	}

	virtual void handleReadRequest(DaRequest& aRequest);
	virtual void handleWriteRequest(DaRequest& aRequest);

};  //  end DaProperty

// constant value property
class TBC_EXPORT DaConstantProperty : public DaProperty
{

protected:
	ValueQT m_value;

public:
	DaConstantProperty(
		int anId,
		tstring& aName,
		tstring& anItemId,
		ValueQT& aValue);
	virtual ~DaConstantProperty() {};

	ValueQT& getValue()
	{
		return m_value;
	}

	virtual void handleReadRequest(DaRequest& aRequest);

};  //  end DaConstantProperty

}   //  end namespace SoftingOPCToolboxServer

#pragma pack(pop)
#endif  //  _DAPROPERTY_H_
