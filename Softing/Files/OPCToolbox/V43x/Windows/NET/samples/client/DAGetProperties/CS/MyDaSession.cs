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
//  Filename    : MyDaSession.cs		                                      |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC DA Session Client template class definition             |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using Softing.OPCToolbox.Client;
using Softing.OPCToolbox;
using System.Threading;

namespace DAGetProperties
{
	public class MyDaSession : DaSession
	{
	
		#region Constructor
		//-----------------

		public MyDaSession (string url) : base (url) 
		{
			GetDaPropertiesCompleted += new GetDaPropertiesEventHandler(HandleGetDaPropertiesCompleted);	
		}

		//--
		#endregion

		#region Private Members
		//---------------------

		#region Private Attributes
		//------------------------


		//--
		#endregion

		//--
		#endregion

		#region Public Methods
		//---------------------

		//--
		#endregion

		#region Public Properties
		//-----------------------


		//--
		#endregion

		#region Handles
		//---------------------


		public static void HandleGetDaPropertiesCompleted(
			DaSession aDaSession,
			uint executionContext,
			System.IntPtr addressSpaceElementHandle,
			string addressSpaceElementId, 
			string addressSpaceElementPath,
			DaProperty[] properties,
			int result)
		{

			
			if(ResultCode.SUCCEEDED(result))
			{
								
				System.Console.WriteLine("Get properties of address space element:  " + addressSpaceElementId);								
				for (int i = 0; i < properties.Length; i++)
				{
									
					System.Console.WriteLine("	Property Name: " + properties[i].Name);
					System.Console.WriteLine("	Property Id: " + properties[i].Id);
					System.Console.WriteLine("	Property Item Id: " + properties[i].ItemId);
					System.Console.WriteLine("	Property DataType: " + properties[i].DataType);									
					System.Console.WriteLine("	Property description: " + properties[i].Description);
					System.Console.WriteLine("	Property value: " + properties[i].ValueQT.Data.ToString()+ "	");										
					System.Console.WriteLine(" ");

				}
			} //end if
			else
			{

				System.Console.WriteLine("Failed to asynchronously get properties of address space element: " + addressSpaceElementId);
			}

		}// end HandleGetDaPropertiesCompleted

		//--
		#endregion	
		
	}	//	end class MyDaSession

}	//	end namespace
