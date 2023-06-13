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
//  Filename    : OpcServer.cs		                                          |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC Server class                                            |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using Softing.OPCToolbox;
using Softing.OPCToolbox.Server;

namespace Session
{
	public class OpcServer
	{

		#region Constructor
		//-----------------
		public OpcServer()
		{
			
		}	//	end constructor
		//--
		#endregion

		#region Protected Attributes
		//--------------------------

		protected ShutdownHandler m_shutdownRequest = null;
		protected MyDaAddressSpaceElement m_daSimulationElement1 = null;
		protected MyDaAddressSpaceElement m_daSimulationElement2 = null;
		protected Random m_random = new Random(1);
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

		#region Public Methods
		//-----------------------

		public static int Shutdown()
		{
			return (int)EnumResultCode.S_OK;
		}	//	end Shutdown

		public int Initialize()
		{
			try
			{
				Application.Instance.VersionOtb = 431;
				Application.Instance.AppType = EnumApplicationType.EXECUTABLE;

				Application.Instance.ClsidDa = "{9FEAA5EF-B361-4D7C-A28C-519F073F61A7}";
				Application.Instance.ProgIdDa = "Softing.Session.DA.1";
				Application.Instance.VerIndProgIdDa = "Softing.Session.DA";
				Application.Instance.Description = "Softing Session OPC Server";
				Application.Instance.MajorVersion = 4;
				Application.Instance.MinorVersion = 31;
				Application.Instance.BuildNumber = 0;
				Application.Instance.VendorInfo = "Softing Industrial Automation GmbH";
				Application.Instance.MinUpdateRateDa = 1000;
				Application.Instance.ClientCheckPeriod = 30000;
				Application.Instance.AddressSpaceDelimiter = '.';
				Application.Instance.PropertyDelimiter = '/';
				
				Application.Instance.IpPortHTTP = 8070;
				Application.Instance.UrlDa = "/OPC/DA";
				Application.Instance.ShutdownRequest += new Softing.OPCToolbox.Server.ShutdownHandler(Shutdown);

			}
			catch(Exception exc)
			{
				Trace(
					EnumTraceLevel.ERR,
					EnumTraceGroup.USER1,
					"OpcServer::Initialize",
					exc.ToString());

				return (int)EnumResultCode.E_FAIL;
			}	//	end try...catch
			return (int)EnumResultCode.S_OK;
		}	//	end Initialize

		public int Start()
		{
			return Application.Instance.Start();
		}	//	end Start

		public int Stop()
		{
			return Application.Instance.Stop();
		}	//	end Stop

		public int Ready()
		{
			return Application.Instance.Ready();
		}	//	end Ready

		public int Terminate()
		{
			return Application.Instance.Terminate();
		}	//	end Terminate

		public int Prepare(MyCreator aMyCreator)
		{
			int result = (int)EnumResultCode.S_OK;
			//	TODO - design time license activation
			//	Fill in your design time license activation keys here
			//
			//	NOTE: you can activate one or all of the features at the same time

			//	activate the COM-DA Server feature
			//	result = Application.Instance.Activate(EnumFeature.DA_SERVER, "XXXX-XXXX-XXXX-XXXX-XXXX");
			if (!ResultCode.SUCCEEDED(result))
			{
				return result;
			}

			//	activate the XML-DA Server Feature
			//	result = Application.Instance.Activate(EnumFeature.XMLDA_SERVER, "XXXX-XXXX-XXXX-XXXX-XXXX");
			if (!ResultCode.SUCCEEDED(result))
			{
				return result;
			}
			//	END TODO - design time license activation

			result = Application.Instance.Initialize(aMyCreator);

			if (ResultCode.SUCCEEDED(result))
			{
				Application.Instance.EnableTracing(					
					EnumTraceGroup.ALL,
					EnumTraceGroup.ALL,
					EnumTraceGroup.SERVER,
					EnumTraceGroup.SERVER,
					"Trace.txt",					
					1000000,					
					0);
			}	//	end if

			return result;
		}	//	end Prepare

		public int ProcessCommandLine(string commandLine)
		{
			return Application.Instance.ProcessCommandLine(commandLine);
		}	//	end ProcessCommandLine
	
		public int BuildAddressSpace()
		{
			try
			{
				MyCreator creator = (MyCreator)Application.Instance.Creator;

				//	DA				
				MyDaAddressSpaceRoot daRoot =(MyDaAddressSpaceRoot)Application.Instance.DaAddressSpaceRoot;
				            
				m_daSimulationElement1 = (MyDaAddressSpaceElement)creator.CreateMyDaAddressSpaceElement();
				m_daSimulationElement1.Name = "client specific";
				m_daSimulationElement1.AccessRights = EnumAccessRights.READABLE;
				m_daSimulationElement1.Datatype = typeof(System.Int32);				
				m_daSimulationElement1.IoMode = EnumIoMode.POLL_OWNCACHE;				
				daRoot.AddChild(m_daSimulationElement1);

				m_daSimulationElement2 = (MyDaAddressSpaceElement)creator.CreateMyDaAddressSpaceElement();
				m_daSimulationElement2.Name = "secured write";
				m_daSimulationElement2.AccessRights = EnumAccessRights.READWRITEABLE;
				m_daSimulationElement2.Datatype =  typeof(System.Int32);
				m_daSimulationElement2.IoMode = EnumIoMode.POLL;
				daRoot.AddChild(m_daSimulationElement2);			
				m_daSimulationElement2.ValueChanged(new ValueQT(2, EnumQuality.GOOD, DateTime.Now));

				m_daSimulationElement2.ValueChanged(
					new ValueQT(
						2,
						EnumQuality.GOOD,
						DateTime.Now));				
			}
			catch(Exception exc)
			{
				Trace(
					EnumTraceLevel.ERR,
					EnumTraceGroup.USER1,
					"OpcServer:BuildAddressSpace",
					exc.ToString());
				return (int)EnumResultCode.E_FAIL;
			}	//	end try...catch

			return (int)EnumResultCode.S_OK;
		}	//	end BuildAddressSpace

		public void ChangeSimulationValues()
		{
			if (m_daSimulationElement2 != null)
			{
				m_daSimulationElement2.ValueChanged(
					new ValueQT(
						m_random.Next(5000),
						EnumQuality.GOOD,
						DateTime.Now));
						
			}	//	end if
		}	//	end ChangeSimulationValues
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

	}	//	end OpcServer
}	//	end namespace
