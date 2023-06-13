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
//  Filename    : stdafx.h                                                    |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : precompiled header definitions                              |
//                                                                            |
//-----------------------------------------------------------------------------

#include "OSCompat.h"

#ifdef TBC_OS_WINDOWS
#define WIN32_LEAN_AND_MEAN     // Exclude rarely-used stuff from Windows headers
// Windows Header Files:
#define _CRT_SECURE_NO_DEPRECATE
#include <windows.h>
#include <conio.h>
#include <tchar.h>
#endif

// C RunTime Header Files
#include <stdlib.h>
#include <malloc.h>
#include <memory.h>


// TODO: reference additional headers your program requires here
