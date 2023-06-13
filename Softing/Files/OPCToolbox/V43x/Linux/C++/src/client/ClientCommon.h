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
//  Filename    : ClientCommon.h                                              |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Declares the Application instance                           |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _CLIENT_COMMON_H_
#define _CLIENT_COMMON_H_

using namespace SoftingOPCToolboxClient;
#ifndef TBC_USING_LIB
Application* Application::m_instance = NULL;
#endif
#endif
