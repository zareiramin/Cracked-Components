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
//  Filename    : Service.cs                                                  |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC Service main class implementation                       |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.IO;
using Softing.OPCToolbox;
using Softing.OPCToolbox.Client;

namespace DaConsole_Service
{
	public class OpcService : System.ServiceProcess.ServiceBase
	{

		#region Private Attributes
		//-------------------------

		private static OpcClient m_opcClient = null;
		//	The following constant holds the name of the Windows NT service that 
		//	runs the OPC application
		//		TODO : change your service name here		
		private const string defaultServiceName = "DaConsole_Service OpcService";

		//--
		#endregion

		#region Public Properties
		//------------------------

		public OpcClient OpcClient
		{
			get
			{				
				return m_opcClient;
			}	//	end get
		}	//	end OpcClient
		
		//--
		#endregion

		#region Public Methods
		//--------------------

		public void CreateOpcClient()
		{
			if (m_opcClient == null)
			{
				m_opcClient = new OpcClient();
			}	//	end if
		}	//	end CreateOpcClient
		//--
		#endregion

		public OpcService()
		{
		}

		// The main entry point for the process
		static void Main()
		{
			int result = (int)EnumResultCode.S_OK;
			bool commandLineProcessed = false;
			string commandline = Environment.CommandLine;

			System.ServiceProcess.ServiceBase[] ServicesToRun;
			OpcService opcService = new OpcService();
			ServicesToRun = new System.ServiceProcess.ServiceBase[] {opcService};
			
			opcService.CreateOpcClient();
			m_opcClient.ServiceName = defaultServiceName;
			//	initialize the client instance
			if (!ResultCode.SUCCEEDED(m_opcClient.Initialize()))
			{								
				m_opcClient = null;	
				return;
			}	//	end if
			
			if (!commandLineProcessed)
			{
				result = m_opcClient.ProcessCommandLine(commandline);
				commandLineProcessed = true;
				if (result != (int)EnumResultCode.S_OK)
				{
					if (result == (int)EnumResultCode.S_FALSE)
					{
						//registration operation succesful
						m_opcClient.Trace(
							EnumTraceLevel.INF,
							EnumTraceGroup.USER1,
							"Service::Main",
							"Registration succeeded");
					}
					else
					{
						m_opcClient.Trace(
							EnumTraceLevel.INF,
							EnumTraceGroup.USER1,
							"Service::Main",
							"Registration failed");
					}	//	end if...else

					//	no matter what close the application if
					//processCommandLine returned something different of S_OK				
					m_opcClient.Terminate();
					m_opcClient = null;
					
					return;
				}	//	end if
			}
			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}

		/// <summary>
		/// Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			int result = (int)EnumResultCode.S_OK;
			//	initialize the DA client simulation
			result |= m_opcClient.InitializeDaObjects();

		}	//	end Start
 
		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			
			try{
				m_opcClient.Terminate();							
				m_opcClient = null;
			}
			catch(Exception exc)
			{				
				FileStream fs =  new FileStream("C:\\ClientService.txt" , FileMode.OpenOrCreate,    FileAccess.Write);
				StreamWriter streamWriter = new StreamWriter(fs);
				streamWriter.BaseStream.Seek(0, SeekOrigin.End);
				streamWriter.WriteLine();
				streamWriter.WriteLine(exc.Message + exc.Source + exc.StackTrace + exc.TargetSite);
				streamWriter.Flush();
				streamWriter.Close();
			}	//	end try...catch
		}	//	end Stop
	}
}
