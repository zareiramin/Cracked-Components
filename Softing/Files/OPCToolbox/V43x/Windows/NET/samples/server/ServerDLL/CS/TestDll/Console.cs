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
//  Filename    : Console.cs                                                  |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Console application main implementation                     |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using Softing.OPCToolbox;

//	Add the dll sample namespace
using Softing.OPCToolbox.DllSample;

namespace TestDll
{
	public class Console
	{
		
		public static void Main(String []args)
		{
			try{
				//create an instance of the generated DLL
				Dll usedDll = new Dll();
				
				// call the DLL's Start method; server will be initialized, 
				// namespaces will be created and simulation thread will be started
				int result  = usedDll.Start();

				if (result != (int)EnumResultCode.S_OK)
				{					
					return;
				}

				System.Console.WriteLine("Press \'e\' or \'q\' and then \'ENTER\'to exit");

				bool end = false;

				while (!end)
				{
					String read = System.Console.ReadLine();
					switch (read)
					{
						case "E":
						case "e":
						case "Q":
						case "q":
							end = true;							
							break;

						default: 						
							break;
					}					
				}// while

				//calls the DLL's Stop method which terminates the server application and closes the simulation thread
				usedDll.Stop();

			}
			catch(Exception exc)
			{
				System.Console.WriteLine(exc.ToString());
			}
		}	//	end Main
	
	}	//	end class Console
}	//	end namespace
