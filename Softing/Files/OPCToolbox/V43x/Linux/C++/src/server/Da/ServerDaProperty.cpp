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
//  Filename    : ServerDaProperty.cpp                                        |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC DA Property handler class                               |
//                                                                            |
//-----------------------------------------------------------------------------
#include "OSCompat.h"
#include "ServerDaProperty.h"
#include "../ServerApplication.h"

using namespace SoftingOPCToolboxServer;

//-----------------------------------------------------------------------------
// Constructor
//
DaProperty::DaProperty():
	m_id(0),
	m_datatype(VT_EMPTY),
	m_accessRights(EnumAccessRights_READABLE)
{
}   //  end default constructor


//-----------------------------------------------------------------------------
// Destructor
//
DaProperty::~DaProperty()
{
}   //  end destructor


//-----------------------------------------------------------------------------
// handleReadRequest
//
void DaProperty::handleReadRequest(DaRequest& aRequest)
{
	if (aRequest.getPropertyId() != m_id)
	{
		Application::Instance()->trace(EnumTraceLevel_WRN, EnumTraceGroup_OPCSERVER,
									   _T("DaProperty::handleReadRequest"),
									   _T("handleReadRequest called with illegal request"));
	}   //  end if

	//  Meant to be obverriden, just return failed
	aRequest.setResult(E_FAIL);
	ValueQT aValue;
	aRequest.setValue(aValue);
	aRequest.complete();
}   //  end handleReadRequest


//-----------------------------------------------------------------------------
// handleWriteRequest
//
void DaProperty::handleWriteRequest(DaRequest& aRequest)
{
	if (aRequest.getPropertyId() != m_id)
	{
		Application::Instance()->trace(EnumTraceLevel_WRN, EnumTraceGroup_OPCSERVER,
									   _T("DaProperty::handleWriteRequest"),
									   _T("handleWriteRequest called with illegal request"));
	}   //  end if

	//  Meant to be obverriden, just return failed
	aRequest.setResult(E_FAIL);
	ValueQT aValue;
	aRequest.setValue(aValue);
	aRequest.complete();
}   //  end handleWriteRequest


//-----------------------------------------------------------------------------
// DaConstantProperty - constructor
//
DaConstantProperty::DaConstantProperty(
	int anId,
	tstring& aName,
	tstring& anItemId,
	ValueQT& aValue):
	m_value(aValue)
{
	m_id = anId;
	m_name = aName;
	m_itemId = anItemId;
	m_accessRights = EnumAccessRights_READABLE;
	m_description = aName;
	m_datatype = (VARENUM)(aValue.getData().vt);
}   //  end default constructor


//-----------------------------------------------------------------------------
// handleWriteRequest
//
void DaConstantProperty::handleReadRequest(DaRequest& aRequest)
{
	aRequest.setResult(S_OK);
	aRequest.setValue(m_value);
	aRequest.complete();
}   //  end handleReadRequest

