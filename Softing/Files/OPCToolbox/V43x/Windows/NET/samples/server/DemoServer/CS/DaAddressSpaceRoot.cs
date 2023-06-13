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
//  Filename    : DaAddressSpaceRoot.cs                                       |
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

namespace Softing.OPCToolbox.DemoServer
{
	public class DemoDaAddressSpaceRoot : DaAddressSpaceRoot
	{
		#region Public Methods
		//--------------------

		public override int QueryAddressSpaceElementData(
			string elementId,
			out AddressSpaceElement anAddressSpaceElement)
		{
			//	TODO: add string based address space validations
			anAddressSpaceElement = null;
			return (int)EnumResultCode.E_NOTIMPL;
		}	//	end QueryAddressSpaceElementData

		//--
		#endregion

	}	//	end MyDaAddressSpaceRoot
}	//	end namespace
