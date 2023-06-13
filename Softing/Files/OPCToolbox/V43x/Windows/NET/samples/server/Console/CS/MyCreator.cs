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

namespace Console
{
	public class MyCreator : Creator
	{		
		#region Public Methods
		//---------------------

		public override AeAddressSpaceElement CreateInternalAeAddressSpaceElement(
			string aName,
			uint anUserData,
			System.IntPtr anObjectHandle,
			System.IntPtr aParentHandle)
		{
			return new MyAeAddressSpaceElement(
				aName,
				anUserData,
				anObjectHandle,
				aParentHandle);
		}	//	end CreateAeAddressSpaceElement

		public override DaAddressSpaceElement CreateInternalDaAddressSpaceElement(
			string anItemId,
			string aName,
			uint anUserData,
			IntPtr anObjectHandle,
			IntPtr aParentHandle)
		{
			return 
				(DaAddressSpaceElement)new MyDaAddressSpaceElement(
					anItemId,
					aName,
					anUserData,
					anObjectHandle,
					aParentHandle);
		}	//	end DaAddressSpaceElement

		
		public override DaAddressSpaceRoot CreateDaAddressSpaceRoot()
		{
			return (DaAddressSpaceRoot)new MyDaAddressSpaceRoot();
		}	//	end CreateDaAddressSpaceRoot

		public override DaTransaction CreateTransaction(
			EnumTransactionType transactionType,
			DaRequest[] requestList,
			IntPtr sessionKey)
		{
			return (DaTransaction)new MyTransaction(
									transactionType,
									requestList,
									sessionKey);
		}	//	end CreateTransaction

		public virtual DaAddressSpaceElement CreateMyDaAddressSpaceElement()
		{
			return (DaAddressSpaceElement)new MyDaAddressSpaceElement();
		}	//	end CreateDaAddressSpaceRoot

		public override DaRequest CreateRequest(
			EnumTransactionType aTransactionType,
			System.IntPtr aSessionHandle,
			DaAddressSpaceElement anElement,
			int aPropertyId,
			System.IntPtr aRequestHandle)
		{
			return new MyRequest(
				aTransactionType,
				aSessionHandle,
				anElement,
				aPropertyId,
				aRequestHandle);
		}	//	end CreateRequest

		public override WebTemplate CreateWebTemplate() 
		{
			return new MyWebTemplate();
		}	//	end CreateWebTemplate

		//--
		#endregion

	}	//	end class MyCreator
}
