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
//  Filename    : ClientAddressSpaceElementBrowseOptions.h                    |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : - AddressSpaceElementBrowseOptions                          |
//                   Options class for browsing an OPC Server's address space |
//                 - DaAddressSpaceElementBrowseOptions                       |
//                    Options class for browsing an DA Server's address space |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _CLIENTADDRESSSPACEELEMENTBROWSEOPTIONS_
#define _CLIENTADDRESSSPACEELEMENTBROWSEOPTIONS_

#include "../Enums.h"
#include "ClientEnums.h"

#pragma pack(push,4)


namespace SoftingOPCToolboxClient
{

class TBC_EXPORT AddressSpaceElementBrowseOptions
{

protected:

	EnumAddressSpaceElementType m_elementTypeFilter;
	tstring m_elementNameFilter;
	BOOL m_forceBrowseUp;

public:

	AddressSpaceElementBrowseOptions();
	virtual ~AddressSpaceElementBrowseOptions();

	EnumAddressSpaceElementType getElementTypeFilter();
	void setElementTypeFilter(EnumAddressSpaceElementType);

	tstring& getElementNameFilter();
	void setElementNameFilter(tstring aNameFilter);

	BOOL getForceBrowseUp();
	void setForceBrowseUp(BOOL forceBrowseUp);
};

class TBC_EXPORT DaAddressSpaceElementBrowseOptions: public AddressSpaceElementBrowseOptions
{

private:

	unsigned long m_maxElements;
	BOOL m_returnProperties;
	BOOL m_returnPropertyValues;
	BOOL m_retrieveItemID;
	tstring m_vendorFilter;
	VARTYPE m_dataTypeFilter;
	EnumAccessRights m_accessRightsFilter;


protected:



public:

	DaAddressSpaceElementBrowseOptions();
	virtual ~DaAddressSpaceElementBrowseOptions();

	unsigned long getMaxElements();
	void setMaxElements(unsigned long nElements);

	tstring& getVendorFilter();
	void setVendorFilter(tstring aVendorFilter);

	EnumAccessRights getAccessRightsFilter();
	void setAccessRightsFilter(EnumAccessRights someAccessRights);

	VARTYPE getDataTypeFilter();
	void setDataTypeFilter(VARTYPE aDataTypeFilter);

	BOOL getReturnPropertyValues();
	void setReturnPropertyValues(BOOL returnPropertyValues);

	BOOL getReturnProperties();
	void setReturnProperties(BOOL returnProperties);

	BOOL getRetrieveItemId();
	void setRetrieveItemId(BOOL retrieveItemId);


}; //end DaAddressSpaceElementBrowseOptions

} //end SoftingOPCToolboxClient

#pragma pack(pop)
#endif
