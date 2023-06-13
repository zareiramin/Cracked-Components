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
//  Filename    : ClientDaItem.h                                              |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Defines the OPC DA Item class.                              |
//                                                                            |
//-----------------------------------------------------------------------------


#ifndef _CLIENTDAITEM_H_
#define _CLIENTDAITEM_H_

#include "../../Enums.h"
#include "../ClientEnums.h"
#include "../ClientObjectSpaceElement.h"
#include "../../Trace.h"
#include "../../ValueQT.h"

#pragma pack(push,4)

namespace SoftingOPCToolboxClient
{

class DaSubscription;
class TBC_EXPORT DaItem : virtual public ObjectSpaceElement
{

	friend class DaSubscription;

private:

	void onValueChange(
		ValueQT& aValueQT,
		long result);

protected:

	DaSubscription* m_daSubscription;
	ValueQT* m_valueQT;

public:

	DaItem(
		tstring itemId,
		DaSubscription* parentSubscription);

	virtual ~DaItem();

	DaSubscription* getDaSubscription();
	void setDaSubscription(DaSubscription* aDaSubscription);

	ValueQT* getValueQT();
	void setValueQT(ValueQT* aValueQT);

	const Variant& getValue() const;

	tstring getId();
	void setId(tstring id);

	tstring getPath();
	void setPath(tstring path);

	VARTYPE getNativeDataType();

	VARTYPE getRequestedDataType();
	void setRequestedDataType(VARTYPE requestedDataType);

	EnumAccessRights getAccessRights();

	float getDeadband();
	void setDeadband(float deadband);

	EnumEUType getEUType();

	Variant getEUInfo();

	virtual long read(
		unsigned long maxAge,
		ValueQT& itemValue,
		long& result,
		ExecutionOptions* someExecutionOptions);

	virtual long write(
		ValueQT* itemValue,
		long& result,
		ExecutionOptions* someExecutionOptions);

	virtual void handleValueChanged(
		const ValueQT& aValueQT,
		long result);

}; // end class DaItem

typedef std::pair <unsigned long, DaItem*> DaItemPair;

}; //end namespace OPC_Client


#pragma pack(pop)
#endif
