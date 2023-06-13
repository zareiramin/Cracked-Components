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
//  Filename    : MyDaAddressSpaceRoot.h                                      |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description :                                                             |
//                                                                            |
//-----------------------------------------------------------------------------
#ifndef _MYDAADDRESSSPACEROOT_H_
#define _MYDAADDRESSSPACEROOT_H_

#include "Da/ServerDaAddressSpaceElement.h"
#include "MyDaAddressSpaceElement.h"

using namespace SoftingOPCToolboxServer;

//-----------------------------------------------------------------------------
// MyDaAddressSpaceRoot
//-----------------------------------------------------------------------------
class MyDaAddressSpaceRoot : public DaAddressSpaceRoot
{

private:
	DaProperty propType1;
	DaProperty propType2;

public:

	MyDaAddressSpaceRoot()
	{
		propType1.setId(5555);
		propType1.setAccessRights(EnumAccessRights_READABLE);
		propType1.setDatatype(VT_I2);
		tstring propertyName(_T("Type"));
		propType1.setName(propertyName);
		tstring propertyDescription(_T("Object Type"));
		propType1.setDescription(propertyDescription);
		propType2.setId(5556);
		propType2.setAccessRights(EnumAccessRights_READABLE);
		propType2.setDatatype(VT_I2);
		propertyName = _T("Type2");
		propType2.setName(propertyName);
		propertyDescription = _T("Object Type2");
		propType2.setDescription(propertyDescription);
	}

	virtual ~MyDaAddressSpaceRoot()
	{
	}

	long queryAddressSpaceElementData(
		const tstring& anElementID,
		AddressSpaceElement*& anElement)
	{
		MyDaAddressSpaceElement* element = new MyDaAddressSpaceElement();
		tstring name;

		if (anElementID.find(_T("stringBased")) == 0)
		{
			//starts with stringBased
			if (anElementID == _T("stringBased"))
			{
				//equals with stringBased
				name = _T("stringBased");
				element->setName(name);
				element->setIoMode(EnumIoMode_NONE);
				element->setHasChildren(TRUE);
			} // end if

			if (anElementID == _T("stringBased.N1"))
			{
				//equals with stringBased.N1
				name = _T("N1");
				element->setName(name);
				element->setAccessRights(EnumAccessRights_READWRITEABLE);
				element->setDatatype(VT_BSTR);
				element->setHasChildren(TRUE);
				element->setIoMode(EnumIoMode_REPORT);
			} // end if

			if (anElementID == _T("stringBased.N2"))
			{
				//equals with stringBased.N2
				name = _T("N2");
				element->setName(name);
				element->setAccessRights(EnumAccessRights_READWRITEABLE);
				element->setDatatype(VT_BSTR);
				element->setHasChildren(TRUE);
				element->setIoMode(EnumIoMode_REPORT);
			} // end if

			if (anElementID == _T("stringBased.T1"))
			{
				//equals with stringBased.T1
				name = _T("T1");
				element->setName(name);
				element->setAccessRights(EnumAccessRights_READWRITEABLE);
				element->setDatatype(VT_I1);
				element->setHasChildren(false);
				element->setIoMode(EnumIoMode_REPORT);
			} // end if

			if (anElementID == _T("stringBased.N1.T1"))
			{
				//equals with stringBased.N1.T1
				name = _T("T1");
				element->setName(name);
				element->setAccessRights(EnumAccessRights_READWRITEABLE);
				element->setDatatype(VT_I1);
				element->setHasChildren(FALSE);
				element->setIoMode(EnumIoMode_REPORT);
			} // end if

			if (anElementID == _T("stringBased.N2.T1"))
			{
				//equals with stringBased.N2.T1
				name = _T("T1");
				element->setName(name);
				element->setAccessRights(EnumAccessRights_READWRITEABLE);
				element->setDatatype(VT_I1);
				element->setHasChildren(FALSE);
				element->setIoMode(EnumIoMode_REPORT);
			} // end if

			if (anElementID == _T("stringBased.N2.T2"))
			{
				//equals with stringBased.N2.T2
				name = _T("T2");
				element->setName(name);
				element->setAccessRights(EnumAccessRights_READWRITEABLE);
				element->setDatatype(VT_I1);
				element->setHasChildren(FALSE);
				element->setIoMode(EnumIoMode_REPORT);
			} // end if
		}
		else
		{
			if (anElementID.find(_T("syntaxBased")) == 0)
			{
				//starts with syntaxBased
				if (anElementID == _T("syntaxBased"))
				{
					name = _T("stringBased");
					element->setName(name);
					element->setIoMode(EnumIoMode_NONE);
					element->setHasChildren(true);
				} // end if

				size_t index = anElementID.rfind(_T(".T"));

				if (index != tstring::npos)
				{
					name = anElementID.substr(index + 1);
					element->setName(name);

					if (element->getName().length() == 2)
					{
						element->setHasChildren(FALSE);
						element->setIoMode(EnumIoMode_POLL);
						element->setAccessRights(EnumAccessRights_READWRITEABLE);
						element->setDatatype(VT_I4);
					}
					else
					{
						name = _T("");
						element->setName(name);
					} // end if .. else
				}
				else
				{
					index = anElementID.rfind(_T(".N"));

					if (index != tstring::npos)
					{
						name = anElementID.substr(index + 1);
						element->setName(name);

						if (element->getName().length() == 2)
						{
							element->setHasChildren(TRUE);
							element->setIoMode(EnumIoMode_NONE);
						}
						else
						{
							name = _T("");
							element->setName(name);
						} // end if .. else
					} // end if
				} // end if .. else
			} // end if
		} // end if .. else

		//  check if an element was succeeded
		if (element->getName() == _T(""))
		{
			delete element;
			element = NULL;
			anElement = NULL;
			return EnumResultCode_E_OPC_BADTYPE;
		}   //  end if

		anElement = element;
		return EnumResultCode_S_OK;
	}   //  end QueryAddressSpaceElementData

	long queryAddressSpaceElementChildren(
		const tstring& anElementId,
		std::vector<AddressSpaceElement*>& aChildrenList)
	{
		MyDaAddressSpaceElement* child = NULL;
		tstring name;

		if (anElementId.length() == 0)
		{
			child = new MyDaAddressSpaceElement();
			name = _T("stringBased");
			child->setName(name);
			child->setIoMode(EnumIoMode_NONE);
			child->setItemId(name);
			child->setHasChildren(TRUE);
			aChildrenList.push_back(child);
		}
		else
		{
			if (anElementId == _T("stringBased"))
			{
				child = new MyDaAddressSpaceElement();
				name = _T("N1");
				child->setName(name);
				name = anElementId + getApp()->getAddressSpaceDelimiter() + child->getName();
				child->setItemId(name);
				child->setAccessRights(EnumAccessRights_READWRITEABLE);
				child->setDatatype(VT_BSTR);
				child->setIoMode(EnumIoMode_REPORT);
				child->setHasChildren(TRUE);
				aChildrenList.push_back(child);
				child = new MyDaAddressSpaceElement();
				name = _T("N2");
				child->setName(name);
				name = anElementId + getApp()->getAddressSpaceDelimiter() + child->getName();
				child->setItemId(name);
				child->setAccessRights(EnumAccessRights_READWRITEABLE);
				child->setDatatype(VT_BSTR);
				child->setIoMode(EnumIoMode_REPORT);
				child->setHasChildren(TRUE);
				aChildrenList.push_back(child);
				child = new MyDaAddressSpaceElement();
				name = _T("T1");
				child->setName(name);
				name = anElementId + getApp()->getAddressSpaceDelimiter() + child->getName();
				child->setItemId(name);
				child->setAccessRights(EnumAccessRights_READWRITEABLE);
				child->setDatatype(VT_I1);
				child->setIoMode(EnumIoMode_REPORT);
				child->setHasChildren(FALSE);
				aChildrenList.push_back(child);
			} // end if

			if (anElementId == _T("stringBased.N1"))
			{
				child = new MyDaAddressSpaceElement();
				name = _T("T1");
				child->setName(name);
				name = anElementId + getApp()->getAddressSpaceDelimiter() + child->getName();
				child->setItemId(name);
				child->setAccessRights(EnumAccessRights_READWRITEABLE);
				child->setDatatype(VT_I1);
				child->setIoMode(EnumIoMode_REPORT);
				child->setHasChildren(FALSE);
				aChildrenList.push_back(child);
			} // end if

			if (anElementId == _T("stringBased.N2"))
			{
				child = new MyDaAddressSpaceElement();
				name = _T("T1");
				child->setName(name);
				name = anElementId + getApp()->getAddressSpaceDelimiter() + child->getName();
				child->setItemId(name);
				child->setAccessRights(EnumAccessRights_READWRITEABLE);
				child->setDatatype(VT_I1);
				child->setIoMode(EnumIoMode_REPORT);
				child->setHasChildren(FALSE);
				aChildrenList.push_back(child);
				child = new MyDaAddressSpaceElement();
				name = _T("T2");
				child->setName(name);
				name = anElementId + getApp()->getAddressSpaceDelimiter() + child->getName();
				child->setItemId(name);
				child->setAccessRights(EnumAccessRights_READWRITEABLE);
				child->setDatatype(VT_I1);
				child->setIoMode(EnumIoMode_REPORT);
				child->setHasChildren(FALSE);
				aChildrenList.push_back(child);
			} // end if
		} // end if .. else

		return EnumResultCode_S_OK;
	} // end queryAddressSpaceElementChildren


	long queryAddressSpaceElementProperties(
		const tstring& anElementID,
		std::vector<DaProperty*>& aPropertyList)
	{
		if (anElementID.find(_T("stringBased")) == 0)   // starts with stringBased
		{
			tstring propertyId;
			propertyId = anElementID + getApp()->getPropertyDelimiter() + _T("objectType");
			propType1.setItemId(propertyId);
			aPropertyList.push_back(&propType1);
			propertyId = anElementID + getApp()->getPropertyDelimiter() + _T("objectType2");
			propType2.setItemId(propertyId);
			aPropertyList.push_back(&propType2);
		} // end if

		return EnumResultCode_S_OK;
	} // end queryAddressSpaceElementProperties

};  //  end MyDaAddressSpaceRoot

#endif
