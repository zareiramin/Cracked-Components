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
//  Filename    : MyTransaction.cs                                            |
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

namespace Softing.OPCToolbox.AddressSpace
{
	public class MyTransaction : DaTransaction
	{

		public MyTransaction(
			EnumTransactionType aTransactionType, 
			DaRequest[] aRequests,
			IntPtr aSessionKey) : 
			base(aTransactionType, aRequests, aSessionKey)
		{

		}	//	end constructor

		public override int HandleReadRequests()
		{

			int count = Requests.Count;
			for(int i=0; i < count; i++)
			{
				
				DaRequest request = Requests[i] as DaRequest;
				if (request.PropertyId == 0) 
				{
					// get address space element value
					// take the toolbox cache value
					ValueQT valueQT = null;
					request.AddressSpaceElement.GetCacheValue(ref valueQT);
					request.Value = valueQT;
					request.Result = EnumResultCode.S_OK;
				}
				else 
				{
					// get property value
					// get the value from the address space element
					MyDaAddressSpaceElement element = request.AddressSpaceElement as MyDaAddressSpaceElement;
					request.Value = new ValueQT(element.ObjectType, EnumQuality.GOOD, DateTime.Now);
					request.Result = EnumResultCode.S_OK;
				}	//	end if ... else

			}	//	end for
			
			return CompleteRequests();

		}	//	end HandleReadRequests

		public override int HandleWriteRequests()
		{
	
			int result = ValuesChanged();
			if (ResultCode.FAILED(result))
			{
				return result;
			}	//	end if

			return CompleteRequests();

		}	//	end HandleWriteRequests
	}	//	end class MyTransaction
}	//	end namespace
