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
//  Filename    : MyDaAddressSpaceElement.h                                   |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's speciffic DataAccess Server OPC Server               |
//                address space element definition                            |
//                                                                            |
//-----------------------------------------------------------------------------
#ifndef _MYDAADDRESSSPACEELEMENT_H_
#define _MYDAADDRESSSPACEELEMENT_H_

#include "Da/ServerDaAddressSpaceElement.h"
#include "Da/ServerDaProperty.h"
#include "OpcServer.h"

using namespace SoftingOPCToolboxServer;

extern BOOL g_holdSignalChange;
extern Mutex g_syncDisplay;

#ifdef TBC_OS_WINDOWS
extern HANDLE g_changedEvent;
#endif

#ifdef TBC_OS_LINUX
extern PosixEvents g_events;
#endif

//-----------------------------------------------------------------------------
// MyDaAddressSpaceElement
//-----------------------------------------------------------------------------
class MyDaAddressSpaceElement : public DaAddressSpaceElement
{

public:
	MyDaAddressSpaceElement(
		tstring& anItemID,
		tstring& aName,
		unsigned long anUserData,
		unsigned long anObjectHandle,
		unsigned long aParentHandle) : DaAddressSpaceElement(anItemID, aName, anUserData, anObjectHandle, aParentHandle)
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
	}   //  end ctr

	MyDaAddressSpaceElement(void)
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
	}   //  end ctr



	virtual ~MyDaAddressSpaceElement()
	{
	}   //  end destructor

	virtual void addedToAddressSpace(void)
	{
		g_syncDisplay.lock();

		if (getItemId() == _T("stringBased"))
		{
			setType(20);
		}
		else
		{
			if ((getItemId() == _T("stringBased.N1")) ||
				(getItemId() == _T("stringBased.N2")))
			{
				setType(21);
			}
			else
			{
				if (getItemId() == _T("stringBased.T1"))
				{
					setType(22);
				}
				else
				{
					if ((getItemId() == _T("stringBased.N1.T1")) ||
						(getItemId() == _T("stringBased.N2.T1")) ||
						(getItemId() == _T("stringBased.N2.T2")))
					{
						setType(23);
					} // end if
				} // end if .. else
			} // end if .. else
		} // end if .. else

		if (getItemId() == _T("syntaxBased"))
		{
			setType(40);
		}
		else
		{
			if (getItemId().find(_T("syntaxBased")) == 0)
			{
				size_t index = getItemId().rfind(_T(".T"));

				if (index != tstring::npos)
				{
					setType(42); // T
				}
				else
				{
					setType(41); // N
				} // end if .. else
			} // end if
		} // end if .. else

		//  the initial value for the elements cache value
		if (getIoMode() != EnumIoMode_NONE)
		{
			DateTime now;
			Variant defaultValue(0);
			ValueQT initialValue(defaultValue, EnumQuality_GOOD, now);
			valueChanged(initialValue);
		}   //  end if

		if (!g_holdSignalChange)
		{
#ifdef TBC_OS_WINDOWS
			::SetEvent(g_changedEvent);
#endif
#ifdef TBC_OS_LINUX
			g_events.signal(1);
#endif
		}   //  end if

		g_syncDisplay.unlock();
	} // end addedToAddressSpace


	virtual void removedFromAddressSpace(void)
	{
		g_syncDisplay.lock();

		if (!g_holdSignalChange)
		{
#ifdef TBC_OS_WINDOWS
			::SetEvent(g_changedEvent);
#endif
#ifdef TBC_OS_LINUX
			g_events.signal(1);
#endif
		}   //  end if

		g_syncDisplay.unlock();
	} // end removedFromAddressSpace

	unsigned char getType(void)
	{
		return m_Type;
	}
	void setType(unsigned char aValue)
	{
		m_Type = aValue;
	}

	long queryProperties(std::vector<DaProperty*>& aPropertyList)
	{
		tstring propertyItemId(_T("objectType"));

		if (getItemId().empty())
		{
			propType1.setItemId(propertyItemId);
		}
		else
		{
			tstring itemId = getItemId() + getApp()->getPropertyDelimiter() +  propertyItemId;
			propType1.setItemId(itemId);
		}   //  end if .. else

		aPropertyList.push_back(&propType1);
		propertyItemId = _T("objectType2");

		if (getItemId().empty())
		{
			propType2.setItemId(propertyItemId);
		}
		else
		{
			tstring itemId = getItemId() + getApp()->getPropertyDelimiter() +  propertyItemId;
			propType2.setItemId(itemId);
		}   //  end if .. else

		aPropertyList.push_back(&propType2);
		return EnumResultCode_S_OK;
	}   //  end QueryProperties

private:
	unsigned char m_Type;
	DaProperty propType1;
	DaProperty propType2;

};  //  end class MyAddressSpaceElement

#endif
