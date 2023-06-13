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
//  Filename    : ServerCreator.cs                                            |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC Server Object instance factory handler                  |
//                                                                            |
//-----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Softing.OPCToolbox.Server{

	/// <summary>
	/// Helper class used as factory for various object instances of 
	/// the OPC server components. This class is used by the Toolbox internal
	/// mechanisms to instantiate user specific objects. To enable this,
	/// the user must extend the Creator class and override the create methods.
	/// </summary>
	/// <include 
	///		file='TBNS.doc.xml' 
	///		path='//class[@name="Creator"]/doc/*' 
	///	/>
	public class Creator{
		
		#region //	Public overridable methods
		//------------------------------------
		
		/// <summary>
		/// Creates the DaAddressSpaceRoot instance. This method should be overriden when a custom root is wanted.
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///		path='//class[@name="Creator"]/
		///		method[@name="CreateDaAddressSpaceRoot"]/doc/*'
		///	/>
		public virtual DaAddressSpaceRoot CreateDaAddressSpaceRoot(){

			return new DaAddressSpaceRoot();

		}	//	end CreateDaAddressSpaceRoot
		
		/// <summary>
		/// Creates the AeAddressSpaceRoot instance
		/// </summary>	
		/// <include
		///		file='TBNS.doc.xml' 
		///		path='//class[@name="Creator"]/
		///		method[@name="CreateAeAddressSpaceRoot"]/doc/*'
		///	/>
		public virtual AeAddressSpaceRoot CreateAeAddressSpaceRoot(){
			
			return new AeAddressSpaceRoot();
			
		}	//	end CreateAeAddressSpaceRoot

		/// <summary>
		/// Creates the DaAddressSpaceElement instance. This method should be overriden when a custom root is wanted.
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///		path='//class[@name="Creator"]/
		///		method[@name="CreateDaAddressSpaceElement"]/doc/*'
		///	/>
		public virtual DaAddressSpaceElement CreateDaAddressSpaceElement()
		{

			return new DaAddressSpaceElement();

		}	//	end CreateDaAddressSpaceRoot

		/// <summary>
		/// Creates the AeAddressSpaceElement instance. This method should be overriden when a custom root is wanted.
		/// </summary>	
		/// <include
		///		file='TBNS.doc.xml' 
		///		path='//class[@name="Creator"]/
		///		method[@name="CreateAeAddressSpaceElement"]/doc/*'
		///	/>
		public virtual AeAddressSpaceElement CreateAeAddressSpaceElement()
		{

			return new AeAddressSpaceElement();

		}	//	end CreateAeAddressSpaceRoot

		/// <summary>
		/// Creates a new instance of DaAddressSpaceElement for string based address space only
		/// </summary>
		/// <param name="anItemId"></param>
		/// <param name="aName"></param>
		/// <param name="anUserData"></param>
		/// <param name="anObjectHandle"></param>
		/// <param name="aParentHandle"></param>
		/// <returns></returns>		
		/// <include
		///		file='TBNS.doc.xml' 
		///		path='//class[@name="Creator"]/
		///		method[@name="CreateInternalDaAddressSpaceElement"]/doc/*'
		///	/>
		public virtual DaAddressSpaceElement CreateInternalDaAddressSpaceElement(
			string anItemId,
			string aName,
			uint anUserData,
			IntPtr anObjectHandle,
			IntPtr aParentHandle){

			return new DaAddressSpaceElement(anItemId, aName, anUserData, anObjectHandle, aParentHandle);

		}	//	end CreateInternalDaAddressSpaceElement
		
		
		/// <summary>
		/// Creates a new instance of AddressSpaceElement for string based address space only
		/// </summary>
		/// <param name="aName"></param>
		/// <param name="anUserData"></param>
		/// <param name="anObjectHandle"></param>
		/// <param name="aParentHandle"></param>
		/// <returns></returns>
		/// <include
		///		file='TBNS.doc.xml' 
		///		path='//class[@name="Creator"]/
		///		method[@name="CreateInternalAeAddressSpaceElement"]/doc/*'
		///	/>
		public virtual AeAddressSpaceElement CreateInternalAeAddressSpaceElement(
			string aName,
			uint anUserData,
			IntPtr anObjectHandle,
			IntPtr aParentHandle){

			return new AeAddressSpaceElement(aName, anUserData, anObjectHandle, aParentHandle);

		}	//	end CreateInternalAeAddressSpaceElement
		

		/// <summary>
		/// Toolbox internal creator of a <see cref="DaRequest"/>. By overloading this method, the user can determine creation of custom Request objects.
		/// </summary>
		/// <returns></returns>
		/// <include
		///		file='TBNS.doc.xml' 
		///		path='//class[@name="Creator"]/
		///		method[@name="CreateRequest"]/doc/*'
		///	/>
		public virtual DaRequest CreateRequest(
			EnumTransactionType aTransactionType,
			IntPtr aSessionHandle,
			DaAddressSpaceElement anElement,
			int aPropertyId,
			IntPtr aRequestHandle){
			return new DaRequest(aTransactionType, aSessionHandle, anElement, aPropertyId, aRequestHandle);
		}	//	end CreateRequest
		
		/// <summary>
		/// Toolbox internal creator of a <see cref="DaTransaction"/>. By overloading this method, the user can determine creation of custom Transaction objects.
		/// All parameters should be forwarded directly to the custom Transaction class.
		/// </summary>
		/// <param name="aTransactionType"></param>
		/// <param name="aRequests"></param>
		/// <param name="aSessionKey"></param>
		/// <returns></returns>
		/// <include
		///		file='TBNS.doc.xml' 
		///		path='//class[@name="Creator"]/
		///		method[@name="CreateTransaction"]/doc/*'
		///	/>
		public virtual DaTransaction CreateTransaction(
			EnumTransactionType aTransactionType, 
			DaRequest[] aRequests, 
			IntPtr aSessionKey){

			return new DaTransaction(aTransactionType, aRequests, aSessionKey);
		
		}	//	end CreateTransaction
		
		
		/// <summary>
		/// Toolbox internal creator of a <see cref="DaSession"/>. By overloading this method, the user can determine creation of custom Session objects.
		/// The number of connected clients can be limited. This can be achieved by returning null instead of a new instance of the DaSession class if the
		/// number of clients exceeds a desired number.
		/// </summary>
		/// <param name="aType"></param>
		/// <param name="aHandle"></param>
		/// <returns></returns>
		/// <include
		///		file='TBNS.doc.xml' 
		///		path='//class[@name="Creator"]/
		///		method[@name="CreateSession"]/doc/*'
		///	/>		
		public virtual DaSession CreateSession(
			EnumSessionType aType, 
			IntPtr aHandle){

			return new DaSession(aType, aHandle);

		}	//	end CreateSession
		

		/// <summary>
		/// Toolbox internal creator of a <see cref="WebTemplate"/>. By overloading this method, the user can determine creation of a custom WebTemplate handler instance.
		///	If web sever is used, this method shuld be overloaded
		/// </summary>
		/// <returns></returns>
		/// <include
		///		file='TBNS.doc.xml' 
		///		path='//class[@name="Creator"]/
		///		method[@name="CreateWebTemplate"]/doc/*'
		///	/>
		public virtual WebTemplate CreateWebTemplate(){
			
			return new WebTemplate();

		}	//	end CreateWebTemplate

		//-
		#endregion
		
 	}	//	end  class Creator
	
}	//	end namespace Softing.OPCToolbon.Server
