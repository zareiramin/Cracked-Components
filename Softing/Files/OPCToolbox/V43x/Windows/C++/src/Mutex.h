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
//  Filename    : Mutex.h                                                     |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description :   Thread safety mutex class declaration                     |
//                                                                            |
//-----------------------------------------------------------------------------

#ifndef _MUTEX_H_
#define _MUTEX_H_

#ifdef TBC_OS_LINUX
#include "pthread.h"
#ifndef INFINITE
#define INFINITE ((unsigned long)0xFFFFFFFF)
#endif
#endif

#pragma pack(push,4)

//-----------------------------------------------------------------------------
// Mutex
//
class TBC_EXPORT Mutex
{

public:
	Mutex();
	virtual ~Mutex();

	// gain access to the mutex object
	BOOL lock(unsigned long timeout = INFINITE);

	// release access to the mutex object
	BOOL unlock(void);

protected:

#ifdef TBC_OS_LINUX
	pthread_mutex_t m_mutex;
#endif  //  end TBC_OS_LINUX

#ifdef TBC_OS_WINDOWS
	HANDLE m_mutex; // handle of system mutex
#endif  //  end TBC_OS_WINDOWS

}; // SOCmnMutex


#pragma pack(pop)
#endif
