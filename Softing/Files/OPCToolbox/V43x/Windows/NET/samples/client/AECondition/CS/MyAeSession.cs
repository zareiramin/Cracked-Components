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
//  Filename    : MyAeSession.cs		                                      |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC AE Session template class definition                    |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using Softing.OPCToolbox.Client;
using Softing.OPCToolbox;
using System.Threading;

namespace AECondition
{
	public class MyAeSession : AeSession
	{
	
		#region Constructor
		//-----------------

		public MyAeSession(string url) : base (url) 
		{
			EnableAeConditionsBySourceCompleted += new EnableAeConditionsBySourceEventHandler(HandleEnableAeConditionsBySourceCompleted);

		}

		//--
		#endregion

		#region Private Members
		//---------------------

		#region Private Attributes
		//------------------------

		private OpcForm m_opcForm = null;

		//--
		#endregion

		//--
		#endregion

		#region Public Methods
		//---------------------

		public void SetForm(OpcForm form)
		{
			m_opcForm = form;
		}

		//--
		#endregion

		#region Public Properties
		//-----------------------


		//--
		#endregion

		#region Handles
		//---------------------

		private void HandleEnableAeConditionsBySourceCompleted(
			AeSession anAeSession,
			uint executionContext, 
			bool enable, 
			string path, 
			int result)
		{
			if (ResultCode.SUCCEEDED(result))
			{
				Console.WriteLine("Enabling conditions succeeded!");
			}
		}

		//--
		#endregion	
		
	}	//	end class MyAeSession

}	//	end namespace
