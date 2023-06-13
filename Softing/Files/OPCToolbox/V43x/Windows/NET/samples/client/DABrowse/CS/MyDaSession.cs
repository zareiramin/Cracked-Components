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
//  Filename    : MyDaSession.cs		                                      |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC DA Session template class definition                    |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using Softing.OPCToolbox.Client;
using Softing.OPCToolbox;
using System.Threading;

namespace DABrowse
{
	public class MyDaSession : DaSession
	{
	
		#region Constructor
		//-----------------

		public MyDaSession (string url) : base (url) 
		{
			StateChangeCompleted += new StateChangeEventHandler(HandleStateChanged);
		}

		//--
		#endregion

		#region Private Members
		//---------------------
			bool m_connected = false;

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
		public bool isConnected()
		{
			return m_connected;
		}
       
		//--
		#endregion

		#region Handles
		//---------------------
		public void HandleStateChanged(ObjectSpaceElement sender, EnumObjectState state)
		{
			if (state == EnumObjectState.CONNECTED)
			{
				m_connected = true;
			}
			else
			{
				m_connected = false;
			}
		}
	

		//--
		#endregion	
        	
	}	//	end class MyDaSession

}	//	end namespace
