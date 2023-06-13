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
//  Filename    : MySession.cs                                                |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's specific DataAccess OPC server                      |
//                DaSession definition                                        |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using System.Text;
using Softing.OPCToolbox;
using Softing.OPCToolbox.Server;

namespace Session
{
	class MySession : DaSession
	{

		public static MySession s_controlingSession = null;
		private static  Int32 s_clientCount = 0;
		private ValueQT m_clientSpecValue = null;

		public MySession(EnumSessionType aType,  System.IntPtr aHandle):base(aType, aHandle)
		{
			m_clientSpecValue = new ValueQT(s_clientCount, EnumQuality.GOOD, DateTime.Now);
			s_clientCount++;		
		}	//	end ctr
		
		public IntPtr Handle
		{
			get
			{
				return base.ObjectHandle;
			}	//	end get
		}	//	end Handle

		public override void ConnectSession ()
		{
			
			base.ConnectSession();

			ArrayList sessions = ArrayList.Synchronized(Console.m_sessions);
			sessions.Add(this);
			Console.m_clientChanged.Set();			
			
		}	//	end ConnectSession

		public override void DisconnectSession ()
		{
			
			base.ConnectSession();

			ArrayList sessions = ArrayList.Synchronized(Console.m_sessions);
			sessions.Remove(this);
			Console.m_clientChanged.Set();
			
		}	//	end DisconnectSession
		
		public override int Logoff (  )
		{

			if (this.Equals(s_controlingSession))
			{
				s_controlingSession = null;
			}	//	end if

			Console.m_clientChanged.Set();
			return (int)EnumResultCode.S_OK;

		}	//	end Logoff

		public override int Logon(System.String UserName, System.String Password )
		{

			int ret = (int)EnumResultCode.E_ACCESSDENIED;

			if (UserName == "OPC" && Password == "opc")
			{

				if (this.Equals(s_controlingSession) || s_controlingSession == null)
				{
					s_controlingSession = this;
					ret = (int)EnumResultCode.S_OK;
				}	//	end if
			}	//	end if

			Console.m_clientChanged.Set();
			return ret;
		}	//	end Logon

		public int GetCacheValue(ref ValueQT aCacheValue)
		{
			
			aCacheValue = new ValueQT(m_clientSpecValue.Data, m_clientSpecValue.Quality, m_clientSpecValue.TimeStamp);
			return (int)EnumResultCode.S_OK; 
		}	//	end GetCacheValue

	}	//	end class MySession
}
