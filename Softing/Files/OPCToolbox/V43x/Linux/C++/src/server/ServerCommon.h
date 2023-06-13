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
//  Filename    : ServerCommon.h                                              |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Declares the Application instance                           |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _SERVERCOMMON_H_
#define _SERVERCOMMON_H_

#ifndef TBC_USING_LIB
SoftingOPCToolboxServer::Application* SoftingOPCToolboxServer::Application::m_instance = NULL;
#endif

#endif
