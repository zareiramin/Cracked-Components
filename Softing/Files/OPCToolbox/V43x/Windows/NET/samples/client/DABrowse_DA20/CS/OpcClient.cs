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
//                             OPC Toolbox C#                                 |
//                                                                            |
//  Filename    : OpcClient.cs		                                          |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC Client template class definition                        |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using Softing.OPCToolbox.Client;
using Softing.OPCToolbox;
using System.Threading;

namespace DABrowse_DA20
{
	public class OpcClient
	{
	
		#region Constructor
		//-----------------

		public OpcClient(){}

		//--
		#endregion

		#region Private Members
		//---------------------

		#region Private Attributes
		//------------------------

		private MyDaSession m_daSession = null;
		private ExecutionOptions m_executionOptions = null;
		//--
		#endregion


		//--
		#endregion
		
		#region Public Methods
		//---------------------

		public Application GetApplication()
		{
			return Application.Instance;
		}	//	end GetApplication

		public int Initialize()
		{

			int result = (int)EnumResultCode.S_OK;
			//	TODO - design time license activation
			//	Fill in your design time license activation keys here
			//
			//	NOTE: you can activate one or all of the features at the same time

			//	activate the COM-DA Client feature
			//	result = Application.Instance.Activate(EnumFeature.DA_CLIENT, "XXXX-XXXX-XXXX-XXXX-XXXX");
			if (!ResultCode.SUCCEEDED(result))
			{
				return result;
			}

			//	activate the XML-DA Client Feature
			//	result = Application.Instance.Activate(EnumFeature.XMLDA_CLIENT, "XXXX-XXXX-XXXX-XXXX-XXXX");
			if (!ResultCode.SUCCEEDED(result))
			{
				return result;
			}
			//	END TODO - design time license activation

			//	proceed with the OPC Toolbox core initialization
			result = GetApplication().Initialize();
			
			if (ResultCode.SUCCEEDED(result))
			{
				//	enable toolbox internal initialization
				GetApplication().EnableTracing(					
					EnumTraceGroup.ALL,
					EnumTraceGroup.ALL,
					EnumTraceGroup.CLIENT,
					EnumTraceGroup.CLIENT,
					"Trace.txt",					
					1000000,
					0);
			}	//	end if
			return result;
		}	//	end Initialize

		public int ProcessCommandLine(string commandLine)
		{
			//	forward the command line arguments to the OPC Toolbox core internals
			return Application.Instance.ProcessCommandLine(commandLine);
		}	//	end ProcessCommandLine


		public void Terminate()
		{
			if (m_daSession.CurrentState != EnumObjectState.DISCONNECTED)
			{
				m_daSession.Disconnect(new ExecutionOptions());
			}
			GetApplication().RemoveDaSession(m_daSession);
			
			GetApplication().Terminate();
			m_daSession = null;
			m_executionOptions = null;
		}	//	end Terminate


		public int InitializeDaObjects()
		{
			int connectResult = (int)EnumResultCode.E_FAIL;
			m_executionOptions = new ExecutionOptions();

			try{
				
				//	TODO add your server URL here
				//	this is the server url for Softing OPCToolbox Demo Server
				//	first choice is COM-DA 
				string url = "opcda:///Softing.OPCToolboxDemo_ServerDA.1/{2E565242-B238-11D3-842D-0008C779D775}";
								
				//uncomment if you need an XML-DA access
				//	string url = "http://localhost:8079/OPC/DA";

				m_daSession = new MyDaSession(url);

				connectResult = m_daSession.Connect(true, false, m_executionOptions);
			}
			catch(Exception exc)
			{
				GetApplication().Trace(
					EnumTraceLevel.ERR,
					EnumTraceGroup.USER,
					"OpcClient::InitializaDaObjects",
					exc.ToString());
			}	//	end try...catch

			return connectResult;
		}	//	end InitializeDaObjects


		public MyDaSession GetSession()
		{
			return m_daSession;
		}// end GetSession

		public ExecutionOptions GetExecutionOptions()
		{
			return m_executionOptions;
		}// end GetExecutionOptions


		public void Trace(
			EnumTraceLevel traceLevel,
			EnumTraceGroup traceGroup,
			string objectID,
			string message) 
		{
			Application.Instance.Trace(
				traceLevel,
				traceGroup,
				objectID,
				message);
		}	//	end Trace

		//--
		#endregion
		
		#region Public Properties
		//-----------------------

		public string ServiceName
		{
			get
			{
				return Application.Instance.ServiceName;
			}
			set
			{
				Application.Instance.ServiceName = value;
			}
		}	//	end SetServiceName

		//--
		#endregion

	}	//	end class OpcClient

}	//	end namespace
