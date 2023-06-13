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
//  Filename    : SOModule.h                                                  |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Module defines                                              |
//                                                                            |
//-----------------------------------------------------------------------------

#define FILE_DESCRIPTION  "Softing OPC Toolbox C++"
#define PRODUCT_NAME      "Softing OPC Toolbox C++"

#ifdef _DEBUG
#ifdef UNICODE
#define ORIGINAL_FILENAME "TBCVS20xxuD.dll"
#else
#define ORIGINAL_FILENAME "TBCVS20xxD.dll"
#endif //   UNICODE
#else
#ifdef UNICODE
#define ORIGINAL_FILENAME "TBCVS20xxu.dll"
#else
#define ORIGINAL_FILENAME "TBCVS20xx.dll"
#endif  //  UNICODE
#endif  //  _DEBUG

#define INTERNAL_NAME     "TBC"
