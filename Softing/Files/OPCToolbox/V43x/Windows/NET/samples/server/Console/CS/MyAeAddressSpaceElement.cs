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
//  Filename    : MyAeAddressSpaceElement.cs                                  |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's specific Alarms and Events OPC Server               |
//                address space element definition                            |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using System.Text;
using Softing.OPCToolbox;
using Softing.OPCToolbox.Server;

namespace Console
{
	public class MyAeAddressSpaceElement : AeAddressSpaceElement
	{
		#region Constructors
		//-------------------

		public MyAeAddressSpaceElement(
			string aName,
			uint anUserData,
			IntPtr anObjectHandle,
			IntPtr aParentHandle)
			:
			base(
			aName,
			anUserData,
			anObjectHandle,
			aParentHandle)
		{
		}	//	end constructor

		public MyAeAddressSpaceElement()
		{
		}	//	end constructor

		//--
		#endregion

		#region Public Methods
		//-------------------

		public override int QueryConditions(
			string aSourcePath, 
			out string[] aConditionNames) 
		{
			
			aConditionNames = new string[2]{OpcServer.COND_NAME_BETWEEN_MULTIPLE, OpcServer.COND_NAME_BETWEEN_SINGLE};
			return (int)EnumResultCode.S_OK;
		}	//	end QueryConditions

		//--
		#endregion		

	}	//	end class MyAeAddressSpaceElemen
}
