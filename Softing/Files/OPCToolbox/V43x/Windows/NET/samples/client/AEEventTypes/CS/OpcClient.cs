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

namespace AEEventTypes
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

		
		//--
		#endregion

		#region Private Methods
		//---------------------


		//--
		#endregion

		//--
		#endregion
		
		#region Public Methods
		//---------------------

		#region Public Attributes
		//------------------------

		public MyAeSession m_aeSession = null;

		//--
		#endregion

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

			//	activate the COM-AE Client Feature
			//	result = Application.Instance.Activate(EnumFeature.AE_CLIENT, "XXXX-XXXX-XXXX-XXXX-XXXX");
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

			if (m_aeSession.CurrentState != EnumObjectState.DISCONNECTED)
			{
				m_aeSession.Disconnect(new ExecutionOptions());
			}

			GetApplication().RemoveAeSession(m_aeSession);
			
			GetApplication().Terminate();
			m_aeSession = null;
		}	//	end Terminate


		public int InitializeAeObjects()
		{
			int connectResult = (int)EnumResultCode.E_FAIL;

			try{
				
				m_aeSession = new MyAeSession("opcae:///Softing.OPCToolboxDemo_ServerAE.1/{2E565243-B238-11D3-842D-0008C779D775}");		

				connectResult = m_aeSession.Connect(
					true,
					true,
					null);

			}
			catch(Exception exc)
			{
				GetApplication().Trace(
					EnumTraceLevel.ERR,
					EnumTraceGroup.USER,
					"OpcClient::InitializeAeObjects",
					exc.ToString());
			}	//	end try...catch

			return connectResult;
		}	//	end InitializeAeObjects

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
