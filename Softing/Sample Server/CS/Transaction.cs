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
//  Filename    : Transaction.cs                                            |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's specific DataAccess OPC server                      |
//                DaTransaction definition                                    |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using System.Text;
using Softing.OPCToolbox;
using Softing.OPCToolbox.Server;

namespace Softing.OPCToolbox.DemoServer
{
	public class Transaction : DaTransaction
	{
		#region Public Methods
		//--------------------

		public Transaction(
			EnumTransactionType aTransactionType,
			DaRequest[]requestList,
			IntPtr aSessionKey)
			:
			base(
			aTransactionType,
			requestList,
			aSessionKey)
		{
		}	//	end constructor

		public override int HandleReadRequests()
		{
            int count = Requests.Count;
            for (int i = 0; i < count; i++)
            {
                DaRequest request = Requests[i] as DaRequest;
                if (request != null)
                {
                    DemoDaAddressSpaceElement element = (DemoDaAddressSpaceElement)request.AddressSpaceElement;
                    if (element != null)
                    {
                        element.HandleReadRequest(request);
                    }
                    else
                    {
                        request.Result = EnumResultCode.E_FAIL;
                    }
                }	//	end if
            }	//	end for
            return CompleteRequests();
		}	//	end HandleReadRequests

		public override int HandleWriteRequests()
		{
			int count = Requests.Count;
			for (int i = 0; i < count; i++)
			{
				DaRequest request = Requests[i] as DaRequest;
				if ( request != null)
				{
					DemoDaAddressSpaceElement element = (DemoDaAddressSpaceElement)request.AddressSpaceElement;
                    if (element != null)
                    {
                        element.HandleWriteRequest(request);
                    }
                    else
                    {
                        request.Result = EnumResultCode.E_FAIL;
                    }
				}	//	end if
			}	//	end for
			return CompleteRequests();
		}	//	end HandleWriteRequests

		//--
		#endregion
	}	//	end class MyTransaction
}	//	end namespace
