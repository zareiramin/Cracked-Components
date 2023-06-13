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

namespace Softing.OPCToolbox.SerialIO
{
	public class MyTransaction : DaTransaction
	{
		#region Public Methods
		//--------------------

		public MyTransaction(
			EnumTransactionType aTransactionType,
			DaRequest[]requestList,
			IntPtr aSessionKey):
			base(
			aTransactionType,
			requestList,
			aSessionKey)
		{
		}	//	end constructor

		public override int HandleReadRequests()
		{
            //  Handle reads asynchrousouly
            Console.OpcServer.AddRequsts(this.m_requestList);
            return (int)EnumResultCode.S_OK;
            
		}	//	end HandleReadRequests

		public override int HandleWriteRequests()
		{
            //  Handle writesasynchrousouly

            Console.OpcServer.AddRequsts(this.m_requestList);
            return (int)EnumResultCode.S_OK;
            
		}	//	end HandleWriteRequests

		//--
		#endregion

	}	//	end class MyTransaction

}	//	end namespace
