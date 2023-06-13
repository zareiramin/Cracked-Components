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
//  Filename    : MyCreator.cs                                                |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's specific OPC Server's objects creator class         |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
using Softing.OPCToolbox;
using Softing.OPCToolbox.Server;

namespace Softing.OPCToolbox.AddressSpace
{
	public class MyCreator : Creator
	{
		
		public override DaAddressSpaceRoot CreateDaAddressSpaceRoot() 
		{
			return (DaAddressSpaceRoot) new MyDaAddressSpaceRoot();
		}	//	end CreateDaAddressSpaceRoot
		
		public override DaTransaction CreateTransaction(
			EnumTransactionType aTransactionType, 
			DaRequest[] aRequests,
			IntPtr aSessionKey)
		{

			return (DaTransaction) new MyTransaction(aTransactionType, aRequests, aSessionKey);
		}	//	end ConsoleTransaction

		public override DaAddressSpaceElement CreateInternalDaAddressSpaceElement(
			string anItemId,
			string aName,
			uint anUserData,
			IntPtr anObjectHandle,
			IntPtr aParentHandle)
		{

			return (DaAddressSpaceElement) new MyDaAddressSpaceElement(anItemId, aName, anUserData, anObjectHandle, aParentHandle);
		
		}	//	end DaAddressSpaceElement
	
	}	//	end class MyCreator
	
}	//	end namespace 
