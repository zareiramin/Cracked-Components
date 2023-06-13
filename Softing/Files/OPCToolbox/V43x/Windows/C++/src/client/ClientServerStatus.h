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
//  Filename    : ClientServerStatus.h                                        |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description :  Object for storing information about an OPC server status  |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _CLIENTSERVERSTATUS_H_
#define _CLIENTSERVERSTATUS_H_

#include "../Enums.h"
#include "ClientEnums.h"
#include "../ValueQT.h"
#include <vector>

#pragma pack(push,4)

namespace SoftingOPCToolboxClient
{

class TBC_EXPORT ServerStatus
{

	friend class AeSession;
	friend class OTCGlobals;
	friend class DaSession;

protected:

	unsigned long m_state;
	DateTime m_startTime;
	DateTime m_currentTime;
	tstring m_vendorInfo;
	tstring m_productVersion;
	std::vector<tstring> m_supportedLcIds;
	DateTime m_lastUpdateTime;
	long m_groupCount;
	long m_bandwidth;
	tstring m_statusInfo;

	ServerStatus(
		unsigned long state,
		DateTime startTime,
		DateTime currentTime,
		tstring vendorInfo,
		tstring productVersion,
		std::vector<tstring> supportedLcIds,
		DateTime lastUpdateTime,
		long groupCount,
		long bandwidth,
		tstring statusInfo);

public:

	ServerStatus();

	virtual ~ServerStatus();

	unsigned long getState();
	void setState(unsigned long state);

	DateTime getStartTime();


	DateTime getCurrentTime();


	tstring& getVendorInfo();


	tstring& getProductVersion();


	std::vector<tstring>& getSupportedLcIds();


	DateTime getLastUpdateTime();


	long getGroupCount();


	long getBandwidth();


	tstring& getStatusInfo();



}; // end ServerStatus
};// end SoftingOPCToolboxClient

#pragma pack(pop)
#endif
