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
//                             OPC Toolbox .NET                               |
//                                                                            |
//  Filename    : ClientCreator.cs                                            |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : Generic Client Creator                                      |
//                                                                            |
//-----------------------------------------------------------------------------

using System;
using Softing.OPCToolbox.Client.DA;
using System.Runtime.InteropServices;

namespace Softing.OPCToolbox.Client
{
	/// <summary>
	/// Creator class : its members are called when a certain instance of a class is created in the toolbox
	/// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public class Creator
	{	
		#region //Public Methods
		/// <summary>
		/// Creates a new instance of ServerBrowser
		/// </summary>		
		virtual public ServerBrowser CreateServerBrowser(string anIpAddress){

			return new OPCToolbox.Client.ServerBrowser(anIpAddress);
		}

		/// <summary>
		/// Creates a new instance of Server
		/// </summary>
		/// <param name="x"></param>
		virtual public Session CreateDASession(){

			return new Softing.OPCToolbox.Client.DA.Session();			
		}

		/// <summary>
		/// Creates a new instance of Subscription
		/// </summary>
		/// <returns></returns>
		virtual public Subscription CreateDASubscription(){

			return new Softing.OPCToolbox.Client.DA.Subscription();
		}
		
		/// <summary>
		/// Creates a new instance of Item
		/// </summary>
		/// <returns></returns>
		virtual public Item CreateDAItem(){

			return new Softing.OPCToolbox.Client.DA.Item();
		}
		#endregion

	}
}
