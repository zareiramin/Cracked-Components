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
//  Filename    : ClientAeCategory.h                                          |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Represents the OPC AE Category class                        |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _CLIENTAECATEGORY_H_
#define _CLIENTAECATEGORY_H_

#include "../../Enums.h"
#include "../ClientEnums.h"

#include <vector>

#pragma pack(push,4)

namespace SoftingOPCToolboxClient
{

class AeAttribute;
class AeSession;

class TBC_EXPORT AeCategory
{

	friend class OTCGlobals;
	friend class AeSession;

private:

	unsigned long m_index;
	std::vector<tstring> m_conditionNameList;

	AeCategory(
		EnumEventType anEventType,
		unsigned long anId,
		tstring aDescription,
		AeSession* aSession);

	static unsigned long CategoryIndex;

	void onQueryAeConditionNames(
		unsigned long executionContext,
		unsigned long categoryId,
		std::vector<tstring>& conditionNames,
		long result);

	void onQueryAeSubConditionNames(
		unsigned long executionContext,
		tstring& conditionName,
		std::vector<tstring>& subConditionNames,
		long result);

	void onQueryAeAttributes(
		unsigned long executionContext,
		unsigned long categoryId,
		std::vector<AeAttribute*>& attributes,
		long result);

protected:

	EnumEventType m_eventType;
	unsigned long m_id;
	tstring m_description;
	AeSession* m_aeSession;

public:

	AeCategory();

	virtual ~AeCategory();

	unsigned long getCategoryCode();

	EnumEventType getEventType();
	void setEventType(EnumEventType anEventType);

	unsigned long getId();
	void setId(unsigned long id);

	tstring& getDescription();
	void setDescription(tstring description);

	AeSession* getSession();
	void setSession(AeSession* anAeSession);

	virtual long queryAeAttributes(
		std::vector<AeAttribute*>& attributes,
		ExecutionOptions* someExecutionOptions);

	virtual long queryAeConditionNames(
		std::vector<tstring>& conditionNames,
		ExecutionOptions* someExecutionOptions);

	virtual long queryAeSubConditionsNames(
		tstring conditionName,
		std::vector<tstring>& subConditionNames,
		ExecutionOptions* someExecutionOptions);

	virtual void handleQueryAeConditionNamesCompleted(
		unsigned long executionContext,
		unsigned long categoryId,
		std::vector<tstring>& conditionNames,
		long result);

	virtual void handleQueryAeSubConditionNamesCompleted(
		unsigned long executionContext,
		tstring& conditionName,
		std::vector<tstring>& subConditionNames,
		long result);

	void handleQueryAeAttributesCompleted(
		unsigned long executionContext,
		unsigned long categoryId,
		std::vector<AeAttribute*>& attributes,
		long result);

}; // end class AeCategory

typedef std::pair<unsigned long, AeCategory*> AeCategoryPair;

}; //end namespace OPC_Client


#pragma pack(pop)
#endif
