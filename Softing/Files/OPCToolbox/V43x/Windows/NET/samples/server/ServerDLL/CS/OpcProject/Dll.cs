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
//  Filename    : Dll.cs                                                      |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC Library main class implementation                       |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Threading;
using System.IO;

namespace Softing.OPCToolbox.DllSample
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Dll
	{
		#region Private Attributes
		//-------------------------
		private bool m_simulationEnd = false;
		private static Mutex m_mutex = new Mutex();
		private static OpcServer m_opcServer = null;
		
		//--
		#endregion

		#region Private Methods
		//----------------------

		private void Simulate()
		{
			// Wait until it is safe to enter.
			m_mutex.WaitOne();

			while (!m_simulationEnd)
			{
				//Simulate the AE Server events generation
				m_opcServer.FireSimulationEvents();
				//Simulate the DA Server data changes generation
				m_opcServer.ChangeSimulationValues();
				Thread.Sleep(1000);
			}	//	end while
			// Release the Mutex
			m_mutex.ReleaseMutex();

		}	//	end Simulate

		private void StartSimulationThread()
		{
			Thread simulationThread = new Thread(new ThreadStart(Simulate));
			simulationThread.Start();			
		}	//	end StartSimulationThread		
		//--
		#endregion

		#region Public Methods
		//--------------------

		public int Start()
		{
			try{
				int result = (int)EnumResultCode.S_OK;
                int registrationResult = (int)EnumResultCode.S_OK;
				//	create and initialize the OpcServer instance
				m_opcServer = new OpcServer();
				m_opcServer.Initialize();
			
				MyCreator creator = new MyCreator();
				if (!ResultCode.SUCCEEDED(m_opcServer.Prepare(creator)))
				{
					m_opcServer.Terminate();					
					m_opcServer = null;	
					return (int)EnumResultCode.E_FAIL;
				}	//	end if

				//	handle the command line arguments (register/unregister, etc)
				string commandline = Environment.CommandLine;
				registrationResult = m_opcServer.ProcessCommandLine(commandline);

				if (registrationResult != (int)EnumResultCode.S_OK)
				{
					if (registrationResult == (int)EnumResultCode.S_FALSE)
					{
						//registration operation succesful
						m_opcServer.Trace(
							EnumTraceLevel.INF,
							EnumTraceGroup.USER1,
							"Dll::Main",
							"Registration succeeded");
					}
					else
					{
						m_opcServer.Trace(
							EnumTraceLevel.INF,
							EnumTraceGroup.USER1,
							"Console::Main",
							"Registration failed");
					}	//	end if...else

					//	no matter what close the application if
					//processCommandLine returned something different of S_OK				
					m_opcServer.Terminate();
					m_opcServer = null;	
					return registrationResult;
				}	//	end if
				//	start the OPC server's I/O internal mechanism 
				if (ResultCode.SUCCEEDED(m_opcServer.Start()))
				{	
					//	build the namespace
					m_opcServer.BuildAddressSpace();
					//	Build the event categories				
					m_opcServer.BuildEventCategories();
					//	declare the namespaces built and the server ready for clients to connect
					m_opcServer.Ready();
				}	//	end if
				//	start the simulation thread 
				if (ResultCode.SUCCEEDED(result))
				{
					StartSimulationThread();
				}	//	end if	
	            return registrationResult;			
			}
			catch(Exception exc)
			{				
				Console.WriteLine(exc.ToString());
                return (int)EnumResultCode.E_FAIL;
			}	//	end try...catch
		}	//	end Start

		public void Stop()
		{
			//	terminate the simulation
			m_simulationEnd = true;
			m_mutex.WaitOne();
			m_mutex.ReleaseMutex();
			m_opcServer.Stop();	
			m_opcServer.Terminate();
			m_opcServer = null;
		}	//	end Stop

		//--
		#endregion

	}	//	end class Dll
}	//	end namespace
