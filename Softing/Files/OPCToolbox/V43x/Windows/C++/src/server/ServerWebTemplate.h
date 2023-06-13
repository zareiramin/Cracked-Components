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
//  Filename    : ServerWebTemplate.h                                         |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC Server's web interface handler class                    |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _SERVERWEBTEMPLATE_H_
#define _SERVERWEBTEMPLATE_H_

#include "../Enums.h"

#pragma pack(push,4)

namespace SoftingOPCToolboxServer
{

class TBC_EXPORT WebTemplate
{

public:

	WebTemplate() {};

	virtual ~WebTemplate() {};

	virtual long handleWebTemplate(
		IN TCHAR* aTemplateName,
		IN unsigned long aNumArgs,
		IN TCHAR* *anArgs,
		tstring& aResult)
	{
		return E_NOTIMPL;
	};  //  end handleWebTemplate

};  //  end WebTemplate

}   //  ens namespace SoftingOPCToolboxServer

#pragma pack(pop)
#endif  // _WEBTEMPLATE_H_
