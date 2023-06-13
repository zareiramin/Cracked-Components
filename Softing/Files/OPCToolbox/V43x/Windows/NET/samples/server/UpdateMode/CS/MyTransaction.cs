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

namespace Softing.OPCToolbox.UpdateMode
{
	public class MyTransaction : DaTransaction
	{
		#region Public Methods
		//--------------------

		public MyTransaction(
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
				DaRequest request = (DaRequest)Requests[i];
				MyDaAddressSpaceElement element = request.AddressSpaceElement as MyDaAddressSpaceElement;

				if (element == null)
				{
					request.Result = EnumResultCode.E_FAIL;
				}	
				else
				{
					if (request.PropertyId == 0)
					{
						if (element.Type == MyDaAddressSpaceElement.TYPE_POLL_SEC)
						{							
							// get address space element value
							DateTime now = DateTime.Now;
							ValueQT cacheValue = new ValueQT(now.Second, EnumQuality.GOOD, now);							
							request.Value = cacheValue;
							request.Result = EnumResultCode.S_OK;
						}					
						else if (element.Type == MyDaAddressSpaceElement.TYPE_POLL_MIN)
						{
							// get address space element value
							DateTime now = DateTime.Now;
							ValueQT cacheValue = new ValueQT(now.Minute, EnumQuality.GOOD, now);							
							request.Value = cacheValue;
							request.Result = EnumResultCode.S_OK;	
						}
						else if (element.Type == MyDaAddressSpaceElement.TYPE_REPORT_MIN)
						{
							//	this is a direct device read request
							DateTime now = DateTime.Now;
							ValueQT cacheValue = new ValueQT(now.Minute, EnumQuality.GOOD, now);							
							request.Value = cacheValue;
							request.Result = EnumResultCode.S_OK;
						}
						else if (element.Type == MyDaAddressSpaceElement.TYPE_REPORT_SEC)
						{
							//	this is a direct device read request
							DateTime now = DateTime.Now;
							ValueQT cacheValue = new ValueQT(now.Second, EnumQuality.GOOD, now);							
							request.Value = cacheValue;
							request.Result = EnumResultCode.S_OK;
						}	//	end if ... else
					}
					else
					{
						// get property value
						// get the value from the address space element
						element.GetPropertyValue(request);							
					}	//	end if ... else
				}			
			}	//	end for
			return CompleteRequests();
		}	//	end HandleReadRequests
	

		//--
		#endregion
	}	//	end class MyTransaction
}	//	end namespace
