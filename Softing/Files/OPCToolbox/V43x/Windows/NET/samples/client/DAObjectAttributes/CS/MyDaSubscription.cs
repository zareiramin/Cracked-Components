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
//  Filename    : MyDaSubscription.cs		                                  |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC DA Subscription template class definition               |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using Softing.OPCToolbox.Client;
using Softing.OPCToolbox;
using System.Threading;

namespace DAObjectAttributes
{
	public class MyDaSubscription : DaSubscription 
	{
	
		#region Constructor
		//-----------------

		public MyDaSubscription(uint updateRate, MyDaSession parentSession) : base (updateRate, parentSession)
		{
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


		//--
		#endregion

	}	//	end class MyDaItem

}	//	end namespace
