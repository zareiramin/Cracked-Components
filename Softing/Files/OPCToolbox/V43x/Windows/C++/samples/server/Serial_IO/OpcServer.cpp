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
//  Filename    : OpcServer.cpp                                               |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC Server template class implementation                    |
//                                                                            |
//-----------------------------------------------------------------------------

#include "stdafx.h"
#include "OpcServer.h"

#include "MyTransaction.h"
#include "MyCreator.h"
#include "ServerCommon.h"

OpcServer* instance = NULL;


OpcServer* getOpcServer(void)
{
	return instance;
}   //  end getOpcServer


void createOpcServer(void)
{
	if (instance == NULL)
	{
		instance = new OpcServer();
	}   //  end if
}   //  end createOpcServer


void destroyOpcServer(void)
{
	if (instance != NULL)
	{
		delete instance;
		instance = NULL;
	}   //  end if
}   //  end destroyOpcServer


long API_CALL HandleShutdownRequest(void)
{
	return S_OK;
}   //end HandleShutdownRequest


OpcServer::OpcServer(void)
{
	m_pDaSimulationElement1 = NULL;
	m_pDaSimulationElement2 = NULL;
}   //  end constructor


OpcServer::~OpcServer(void)
{
}   //  end destructor


long OpcServer::initialize(void)
{
	getApp()->setVersionOtb(431);
	getApp()->setAppType(EnumApplicationType_EXECUTABLE);
	getApp()->setClsidDa(_T("{4BF8410A-AC1B-4016-BA89-FC622A31AE27}"));
	getApp()->setProgIdDa(_T("Softing.OPCToolbox.C++.Sample.SerialIO.DA.1"));
	getApp()->setVerIndProgIdDa(_T("Softing.OPCToolbox.C++.Sample.SerialIO.DA"));
	getApp()->setDescription(_T("Softing OPC Toolbox C++ Serial IO Sample Server"));
	getApp()->setMajorVersion(4);
	getApp()->setMinorVersion(31);
	getApp()->setPatchVersion(1);
	getApp()->setBuildNumber(0);
	getApp()->setVendorInfo(_T("Softing Industrial Automation GmbH"));
	getApp()->setMinUpdateRateDa(100);
	getApp()->setClientCheckPeriod(30000);
	getApp()->setAddressSpaceDelimiter(_T('.'));
	getApp()->setPropertyDelimiter(_T('/'));
	getApp()->setIpPortHTTP(8079);
	getApp()->setUrlDa(_T("/OPC/DA"));
	return S_OK;
}   //  end initialize

void OpcServer::setServiceName(tstring serviceName)
{
	getApp()->setServiceName(serviceName);
}   //  end setServiceName

long OpcServer::prepare(MyCreator* creator)
{
	long result = S_OK;
	//  TODO - design time license activation
	//  Fill in your design time license activation keys here
	//
	//  NOTE: you can activate one or all of the features at the same time
	//  firstly activate the COM-DA Server feature
	//result = getApp()->activate(EnumFeature_DA_SERVER, _T("XXXX-XXXX-XXXX-XXXX-XXXX"));

	if (!SUCCEEDED(result))
	{
		return result;
	}

	//  activate the XML-DA Server Feature
	//result = getApp()->activate(EnumFeature_XMLDA_SERVER, _T("XXXX-XXXX-XXXX-XXXX-XXXX"));

	if (!SUCCEEDED(result))
	{
		return result;
	}

	//  END TODO - design time license activation
	result = getApp()->initialize(creator);

	if (SUCCEEDED(result))
	{
		getApp()->enableTracing(
			EnumTraceGroup_ALL,
			EnumTraceGroup_ALL,
			EnumTraceGroup_SERVER,
			EnumTraceGroup_SERVER,
			_T("Trace1.txt"),
			1000000,
			0);
	}

	return result;
}   //  end prepare

long OpcServer::start(void)
{
	return getApp()->start();
}   //  end start

long OpcServer::ready(void)
{
	return getApp()->ready();
}   //  end ready

long OpcServer::stop(void)
{
	return getApp()->stop();
}   //  end stop

long OpcServer::terminate(void)
{
	long result = getApp()->terminate();
	releaseApp();
	return result;
}   //  end terminate

long OpcServer::processCommandLine(tstring command)
{
	return getApp()->processCommandLine(command);
}   //  end processCommandLine


long OpcServer::buildAddressSpace(void)
{
	MyCreator* creator = (MyCreator*)getApp()->getCreator();
	tstring aName;
	DateTime now;
	Variant aVariant(rand());
	ValueQT value(aVariant, EnumQuality_GOOD, now);
	//  DA
	DaAddressSpaceRoot* daRoot = getApp()->getDaAddressSpaceRoot();
	m_pDaSimulationElement1 = (MyDaAddressSpaceElement*)creator->createMyDaAddressSpaceElement();
	aName = tstring(_T("Simulation1"));
	m_pDaSimulationElement1->setName(aName);
	m_pDaSimulationElement1->setAccessRights(EnumAccessRights_READWRITEABLE);
	m_pDaSimulationElement1->setDatatype(VT_I4);
	m_pDaSimulationElement1->setIoMode(EnumIoMode_POLL);
	daRoot->addChild(m_pDaSimulationElement1);
	m_pDaSimulationElement1->valueChanged(value);
	m_pDaSimulationElement2 = (MyDaAddressSpaceElement*)creator->createMyDaAddressSpaceElement();
	aName = tstring(_T("Simulation2"));
	m_pDaSimulationElement2->setName(aName);
	m_pDaSimulationElement2->setAccessRights(EnumAccessRights_READWRITEABLE);
	m_pDaSimulationElement2->setDatatype(VT_I4);
	m_pDaSimulationElement2->setIoMode(EnumIoMode_POLL);
	daRoot->addChild(m_pDaSimulationElement2);
	//  start with same value
	m_pDaSimulationElement2->valueChanged(value);
	return EnumResultCode_S_OK;
}   //  end buildAddressSpace


void OpcServer::changeSimulationValues(void)
{
	//  TODO:
	//  this should be triggered by the serial communication handling mechanism
	//  which feeds the value
	if (m_pDaSimulationElement1 != NULL)
	{
		DateTime now;
		Variant aVariant(::rand());
		ValueQT value(aVariant, EnumQuality_GOOD, now);
		//  TODO: if the value is not good return a BAD Quality
		m_pDaSimulationElement1->valueChanged(value);
		//  TODO: go through the requests list and complete the requests
		//  refferencing simulationElement1
	}   //  end if

	if (m_pDaSimulationElement2 != NULL)
	{
		DateTime now;
		Variant aVariant(::rand());
		ValueQT value(aVariant, EnumQuality_GOOD, now);
		m_pDaSimulationElement2->valueChanged(value);
		//  TODO : go through the requests list and complete the requests
		//  refferencing simulationElement1
	}   //  end if
}   //  end changeSimulationValues


void OpcServer::addRequests(std::vector<DaRequest*>& requests)
{
	m_requestsJanitor.lock();
	{
		size_t count = requests.size();

		for (size_t i = 0; i < count; i++)
		{
			requests[i]->setRequestState(EnumRequestState_PENDING);
			m_requests.push_back(requests[i]);
		}   //  end for
	}
	m_requestsJanitor.unlock();
}   //  end addRequests

void OpcServer::handleRequests(void)
{
	m_requestsJanitor.lock();
	{
		size_t count = m_requests.size();

		for (size_t i = 0; i < count; i++)
		{
			MyDaAddressSpaceElement* element =
				(MyDaAddressSpaceElement*)m_requests[i]->getAddressSpaceElement();

			if (element == NULL)
			{
				m_requests[i]->setResult(E_FAIL);
			}
			else
			{
				if (m_requests[i]->getTransactionType() == EnumTransactionType_READ)
				{
					if (m_requests[i]->getPropertyId() == 0)
					{
						// get address space element value take the toolbox cache value
						ValueQT cacheValue;
						element->getCacheValue(cacheValue);
						m_requests[i]->setValue(cacheValue);
						m_requests[i]->setResult(S_OK);
					}
					else
					{
						//  the element's property will handle this request
						element->getPropertyValue(m_requests[i]);
					}   //  end if ... else
				}
				else
				{
					//  EnumTransactionType_WRITE
					ValueQT* pValue = m_requests[i]->getValue();
					long result = element->valueChanged(*pValue);
					m_requests[i]->setResult(result);
				}   //  end if ... else
			}   //  end if ... else

			m_requests[i]->complete();
		}   //  end for

		//  release all requests
		m_requests.clear();
	}
	m_requestsJanitor.unlock();
}   //  end handleRequests


void OpcServer::trace(
	EnumTraceLevel aLevel,
	EnumTraceGroup aMask,
	const TCHAR* anObjectID,
	const TCHAR* aMessage,
	...)
{
	getApp()->trace(aLevel, aMask, anObjectID, aMessage);
}   //  end trace
