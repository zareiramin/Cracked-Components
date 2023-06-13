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
//  Filename    : Trace.cpp                                                   |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description :   Implementation of the Trace logger class                  |
//                                                                            |
//-----------------------------------------------------------------------------

#include "OSCompat.h"
#include "Trace.h"
#include "OTcommon.h"

typedef enum tagTraceData
{

	TRACEDATA_ALL           = 0x000000FF,
	TRACEDATA_LEVEL         = 0x0000000F,
	TRACEDATA_LEVEL_ERR     = 0x00000008,
	TRACEDATA_LEVEL_WRN     = 0x00000004,
	TRACEDATA_LEVEL_INF     = 0x00000002,
	TRACEDATA_LEVEL_DEB     = 0x00000001,
	TRACEDATA_FILE          = 0x000000F0,
	TRACEDATA_FILE_ENABLE   = 0x00000010,
	TRACEDATA_FILE_NAME     = 0x00000020,
	TRACEDATA_MAX_BACKUPS   = 0x00000040,
	TRACEDATA_FILE_SIZE     = 0x00000080

}   EnumTraceData;

unsigned long Trace::m_errorLevelMask = 0;
unsigned long Trace::m_warningLevelMask = 0;
unsigned long Trace::m_infoLevelMask = 0;
unsigned long Trace::m_debugLevelMask = 0;
bool Trace::m_enableTraceToFile = false;
tstring Trace::m_fileName;
unsigned long Trace::m_maxFileSize = 0;
unsigned long Trace::m_maxBackups = 0;

unsigned long Trace::getDebugLevelMask(void)
{
	return m_debugLevelMask;
}// end GetDebugLevelMask

void Trace::setDebugLevelMask(unsigned long aMask)
{
	m_debugLevelMask = aMask;
}// end SetDebugLevelMask

unsigned long Trace::getInfoLevelMask(void)
{
	return m_infoLevelMask;
}// end GetInfoLevelMask

void Trace::setInfoLevelMask(unsigned long aMask)
{
	m_infoLevelMask = aMask;
}// end SetInfoLevelMask

// Level of warnings traced
unsigned long Trace::getWarningLevelMask(void)
{
	return m_warningLevelMask;
}// end GetWarningLevelMask

void Trace::setWarningLevelMask(unsigned long aMask)
{
	m_warningLevelMask = aMask;
}// end SetWarningLevelMask

unsigned long Trace::getErrorLevelMask(void)
{
	return m_errorLevelMask;
}// end GetErrorLevelMask

void Trace::setErrorLevelMask(unsigned long aMask)
{
	m_errorLevelMask = aMask;
}// end SetErrorLevelMask

bool Trace::getEnableTraceToFile(void)
{
	return m_enableTraceToFile;
}// end GetEnableTraceToFile

void Trace::setEnableTraceToFile(bool isEnabled)
{
	m_enableTraceToFile = isEnabled;
	setTraceOptions(TRACEDATA_ALL);
}// end setEnableTraceToFile

unsigned long Trace::getFileMaxSize(void)
{
	return m_maxFileSize;
}// end getFileMaxSize

void Trace::setFileMaxSize(unsigned long aMaxSize)
{
	m_maxFileSize = aMaxSize;
}// end setFileMaxSize

tstring& Trace::getFileName(void)
{
	return m_fileName;
}   //  end getFileName

void Trace::setFileName(tstring& aFileName)
{
	m_fileName = aFileName;
}   //  end setFileName

unsigned long Trace::getMaxBackups()
{
	return m_maxBackups;
}   //  end getMaxBackups

void Trace::setMaxBackups(unsigned long maxBackups)
{
	m_maxBackups = maxBackups;
}   //  end setMaxBackups

void Trace::setTraceOptions(unsigned long aWhatDataFlag)
{
	OTTraceData traceData;
	traceData.m_debugLevelMask = m_debugLevelMask;
	traceData.m_infoLevelMask = m_infoLevelMask;
	traceData.m_warningLevelMask = m_warningLevelMask;
	traceData.m_errorLevelMask = m_errorLevelMask;
	traceData.m_enableTraceToFile = (unsigned char)m_enableTraceToFile;
	traceData.m_maxFileSize = m_maxFileSize;
	traceData.m_maxBackups  =   m_maxBackups;

	if (m_fileName.empty())
	{
		traceData.m_fileName = NULL;
	}
	else
	{
		traceData.m_fileName = (OTChar*) m_fileName.c_str();
	}   //  end if ... else

	OTEnableTracing(aWhatDataFlag, &traceData);
}   //  end setTraceOptions


void Trace::writeline(
	unsigned char aLevel,
	unsigned long aMask,
	tstring& anObjectID,
	tstring& aMessage)
{
	OTTrace(aLevel, aMask, (OTChar*)anObjectID.c_str(), (OTChar*)aMessage.c_str());
}   //  end writeline
