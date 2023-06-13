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
//  Filename    : OutProc.cs                                                  |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC OutProc main class implementation                       |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Threading;
using System.IO;

namespace AEBrowse
{
	/// <summary>
	/// Summary description for OutProc.
	/// </summary>
	public class OutProc
	{
		#region Private Attributes
		//-------------------------
		private static OpcClient m_opcClient = null;
			
		//--
		#endregion

		#region Public Properties
		//------------------------

		public OpcClient OpcClient
		{
			get
			{				
				return m_opcClient;
			}	//	end get
		}	//	end OpcClient
		//--
		#endregion

		#region Public Methods
		//--------------------------
		public void CreateOpcClient()
		{
			if (m_opcClient == null)
			{
				m_opcClient = new OpcClient();
			}	//	end if
		}	//	end CreateOpcClient
		//--
		#endregion

	}	//	end class Outproc
}	//	end namespace
