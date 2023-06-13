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
//  Filename    : MyDaAddressSpaceElement.cs                                  |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : User's specific DataAccess OPC Server                      |
//                address space element definition                            |
//                                                                            |
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using System.Text;
using Softing.OPCToolbox;
using Softing.OPCToolbox.Server;

namespace Softing.OPCToolbox.DllSample
{
	public class MyDaAddressSpaceElement : DaAddressSpaceElement
	{

		#region Constructors
		//------------------------------

		public MyDaAddressSpaceElement(
			string anItemID,
			string aName,
			uint anUserData,
			IntPtr anObjectHandle,
			IntPtr aParentHandle)
			:
			base(
			anItemID,
			aName,
			anUserData,
			anObjectHandle,
			aParentHandle)
		{
		}	//	end constructor

		public MyDaAddressSpaceElement()
		{
		}	//	end constructor

		//--
		#endregion

		#region Public Static Attributes
		//-------------------------------

		static public byte TYPE_UNKNOWN = 0x0;
		static public byte TYPE_NODEMATH = 0x80;
		static public byte TYPE_SINE = 0x04;
		static public byte TYPE_ACCEPT = 0x08;

		//--
		#endregion

		#region Private Attributes
		//-------------------------------

		private byte m_Type = TYPE_UNKNOWN;
		private Hashtable m_properties = new Hashtable();

		//--
		#endregion

		#region Public Property
		//---------------------

		public virtual byte Type
		{
			get { return m_Type; }
			set { m_Type = value; }
		}	//	Attribute Type

		//--
		#endregion

		#region Public Methods
		//---------------------

		/// <summary>
		/// Get elements property value data
		/// </summary>
		public void GetPropertyValue(DaRequest aRequest)
		{
			if (aRequest.PropertyId == 101)
			{
				aRequest.Value = new ValueQT(
					"description",
					EnumQuality.GOOD,
					DateTime.Now);

				aRequest.Result = EnumResultCode.S_OK;
			}
			else
			{
				aRequest.Result = EnumResultCode.E_NOTFOUND;
			}	//	end if ... else
		}	//	end GetPropertyValue

		public override int QueryProperties(out ArrayList aPropertyList)
		{
			if (m_properties.Count > 0)
			{
				aPropertyList = new ArrayList();
				aPropertyList.AddRange(m_properties.Values);
			}
			else
			{
				aPropertyList = null;
			}	//	end if ... else
			return (int)(EnumResultCode.S_OK);
		}	//	end QueryProperties


		public int AddProperty(DaProperty aProperty)
		{
			if ( aProperty != null)
			{
				m_properties.Add(
					aProperty.Id,
					aProperty);
				return (int)EnumResultCode.S_OK;
			}	//	end if
			else
			{
				return (int)EnumResultCode.S_FALSE;
			}	//	end if...else
		}	//	end AddProperty

		//--
		#endregion

	}	//	end class MyAddressSpaceElement
}	//	end namespace
