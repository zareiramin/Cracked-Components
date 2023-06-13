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
//  Filename    : MyDaAddressSpaceRoot.cs                                     |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's specific DataAccess OPC Server                       |
//                address space root element definition                       |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using System.Text;
using Softing.OPCToolbox;
using Softing.OPCToolbox.Server;

namespace Console
{
	public class MyDaAddressSpaceRoot : DaAddressSpaceRoot
	{
		#region Public Methods
		//--------------------
		
		public override int QueryAddressSpaceElementData(string anElementId, out AddressSpaceElement anElement) 
		{
			
			MyDaAddressSpaceElement element = new MyDaAddressSpaceElement();
			element.ItemId = anElementId;
			anElement = element;

			return (int)EnumResultCode.S_OK;

		}	//	end QueryAddressSpaceElementData

		//--
		#endregion

	}	//	end MyDaAddressSpaceRoot
}	//	end namespace
