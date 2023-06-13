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
//  Filename    : ClientServerBrowser.h                                       |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : - ServerBrowser                                             |
//                  Object for browsing the OPC servers installed on a        |
//                  machine                                                   |
//                - ServerBrowserData                                         |
//                  Object for storing information about OPC servers          |
//                  installed on a machine                                    |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _CLIENTSERVERBROWSER_H_
#define _CLIENTSERVERBROWSER_H_

#include "../Enums.h"
#include "ClientEnums.h"
#include "../ValueQT.h"
#include <vector>

#pragma pack(push,4)

namespace SoftingOPCToolboxClient
{

class ServerBrowserData;

class TBC_EXPORT ServerBrowser
{

	//friend class Application;
	friend class OTCGlobals;

private:

	unsigned long m_index;
	static unsigned long m_objectIndex;

	void onBrowseServers(
		unsigned long executionContext,
		tstring& ipAddress,
		std::vector<ServerBrowserData*>& serverData,
		long result);

protected:

	tstring m_ipAddress;

public:

	ServerBrowser(tstring ipAddress);

	virtual ~ServerBrowser();

	virtual unsigned long getUserData(void);

	tstring& getIpAddress();
	void setIpAddress(tstring anIpAddress);

	virtual long browse(
		EnumOPCSpecification whatOPCSpecification,
		EnumServerBrowserData whatServerData,
		std::vector<ServerBrowserData*>& serverData,
		ExecutionOptions* someExecutionOptions);

	virtual void handleBrowseServersCompleted(
		unsigned long executionContext,
		tstring& ipAddress,
		std::vector<ServerBrowserData*>& serverData,
		long result);

}; // end ServerBrowser

typedef std::pair<unsigned long, ServerBrowser*> ServerBrowserPair;

class TBC_EXPORT ServerBrowserData
{

	friend class ServerBrowser;
	//friend class Application;
	friend class OTCGlobals;

private:

	tstring m_clsId;
	tstring m_progId;
	tstring m_description;
	tstring m_progIdVersionIndependent;
	EnumOPCSpecification m_opcSpecification;
	tstring m_url;

	ServerBrowserData(
		tstring clsId,
		tstring progId,
		tstring description,
		tstring progIdVersionIndependent,
		EnumOPCSpecification opcSpecification,
		tstring url);

protected:

public:

	ServerBrowserData();

	virtual ~ServerBrowserData();

	tstring& getClsId();

	tstring& getDescription();

	EnumOPCSpecification getOpcSpecification();

	tstring& getProgId();

	tstring& getProgIdVersionIndependent();

	tstring& getUrl();

}; // end ServerBrowser
};// end SoftingOPCToolboxClient

#pragma pack(pop)
#endif
