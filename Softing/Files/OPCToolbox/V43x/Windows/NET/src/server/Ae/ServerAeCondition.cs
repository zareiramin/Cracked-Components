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
//  Filename    : ServerAeCondition.cs                                        |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC AE condition class definition                           |
//                                                                            |
//-----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Runtime.InteropServices;
using Softing.OPCToolbox.OTB;
using Softing.OPCToolbox.Server;

namespace Softing.OPCToolbox.Server{
	
	/// <summary>
	/// <para>
	///	An object of the AeCondition class represents a condition monitored by the OPC AE Server. 
	///	These objects connect the condition class AeCondition with the attribute values ( <see cref="AeAttribute"/> ).
	///	</para>
	///	<para>
	///	The SOAeSCondition class implements the state machine described in the OPC AE specification. 
	///	During state transitions, the corresponding messages are generated and sent to the OPC Clients according to the specification. 
	///	</para>
	///	<para>
	///	The condition objects are stored in the <see cref="Application"/> class. 
	///	</para>
	///	<para>	
	///	The AeCondition class stores the current attribute values <see cref="AeAttribute"/> of the condition in the EventAttributes. 
	///	If this list is not implemented, no attribute values can be supplied to the OPC Client for this condition. 
	///	</para>	
	/// </summary>
	/// <include 
	///		file='TBNS.doc.xml' 
	///		path='//class[@name="AeCondition"]/doc/*'
	///	/>
	public class AeCondition {
			
		#region //	Public Constructors
		//-----------------------------
		
		/// <summary>
		/// creates a default object of AeCondition
		/// </summary>
		/// <include 
		///		file='TBNS.doc.xml'		
		///			path='//class[@name="AeCondition"]/
		///			ctor[@name="AeCondition"]/doc/*'
		///	/>
		public AeCondition(){
			
		}	//	end Constructor
		
		//-
		#endregion

		#region //	Public Destructors
		//----------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <include 
		///		file='TBNS.doc.xml'
		///			path='//class[@name="AeCondition"]/
		///			dtor[@name="AeCondition"]/doc/*'
		///	/>
		~AeCondition(){
			if (m_handle != 0){		
				//	This should never happen!
				Application.Instance.ReleaseCondition(m_handle);
			}	//	end if
		}	//	end Destructor

		//-
		#endregion
	
		#region //	Public Properties 
		//--------------------------
		
		/// <summary>
		/// The current condition state
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="StateChange"]/doc/*'
		///	/>
		public EnumConditionState StateChange{
			get{return this.m_stateChange;}
			set{this.m_stateChange = value;}
		}	//	end StateChange

		/// <summary>
		/// The current change mask
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="ChangeMask"]/doc/*'
		///	/>
		public ushort ChangeMask{
			get{return this.m_changeMask;}
			set{this.m_changeMask = value;}
		}	//	end ChangeMask

		/// <summary>
		/// The category associated to this condition. Must be set before using the condition
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="Category"]/doc/*'
		///	/>
		public AeCategory Category{
			get{return this.m_eventCategory;}
			set{this.m_eventCategory = value;}
		}	//	end Category

		/// <summary>
		/// Severity of the events generated by this condition
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="Severity"]/doc/*'
		///	/>
		public uint Severity{
			get{return this.m_severity;}
			set{this.m_severity = value;}
		}	//	end Severity
		
		/// <summary>
		/// The source path of the address space element associated 
		/// to this condition
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="SourcePath"]/doc/*'
		///	/>
		public string SourcePath{
			get{return this.m_sourcePath;}
			set{this.m_sourcePath = value;}
		}	//	end SourcePath

		/// <summary>
		///	The condition current message
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="Message"]/doc/*'
		///	/>
		public string Message{
			get{return this.m_message;}
			set{this.m_message = value;}
		}	//	end Message

		/// <summary>
		///	The timestamp of the condition occurance
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="OccurenceTime"]/doc/*'
		///	/>
		public DateTime OccurenceTime{
			get{return this.m_occurenceTime;}
			set{this.m_occurenceTime = value;}
		}	//	end OccurenceTime
		

		/// <summary>
		///	Name of the condition
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="Name"]/doc/*'
		///	/>
		public string Name{
			get{return this.m_name;}
			set{this.m_name = value;}
		}	//	end Name
		

		/// <summary>
		///	The name of the active sub condition.
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="ActiveSubConditionName"]/doc/*'
		///	/>
		public string ActiveSubConditionName{
			get{return this.m_activeSubConditionName;}
			set{this.m_activeSubConditionName = value;}
		}	//	end ActiveSubConditionName
		

		/// <summary>
		///	current Quality of the condition 
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="Quality"]/doc/*'
		///	/>
		public EnumQuality Quality{
			get{return this.m_quality;}
			set{this.m_quality = value;}
		}	//	end Quality
		
		/// <summary>
		///	Flag to request ACK from the client for this condition
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="AckRequired"]/doc/*'
		///	/>
		public bool AckRequired{
			get{return this.m_ackRequired;}
			set{this.m_ackRequired = value;}
		}	//	end AckRequired
		
		/// <summary>
		///	The ACK identifier received from the client
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="AckId"]/doc/*'
		///	/>
		public string AckId{
			get{return this.m_ackId;}
			set{this.m_ackId = value;}
		}	//	end AckId
				
		/// <summary>
		///	the ACK comment received from the client
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="AckComment"]/doc/*'
		///	/>
		public string AckComment{
			get{return this.m_ackComment;}
			set{this.m_ackComment = value;}
		}	//	end AckComment

		/// <summary>
		///	Timestamp of the client's acknowledgement
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="AckTime"]/doc/*'
		///	/>
		public DateTime AckTime{
			get{return this.m_ackTime;}
			set{this.m_ackTime = value;}
		}	//	end AckTime
		
		/// <summary>
		///	An array containing the current attribures of the event generated by the condition
		/// </summary>
		/// <value>
		/// The array must contain variant objects for each attribute of this condition
		/// </value>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="EventAttributes"]/doc/*'
		///	/>
		public ArrayList EventAttributes{
			get{
				ArrayList syncEventAttributes = ArrayList.Synchronized(m_eventAttributes);
				return syncEventAttributes;
			}	//	end get
			set{
				ArrayList syncEventAttributes = ArrayList.Synchronized(m_eventAttributes);
				syncEventAttributes.Clear();
				syncEventAttributes.AddRange(value);
			}	//	end set
		}	//	end EventAttributesCount

		/// <summary>
		///	an user specific identification handler
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="UserData"]/doc/*'
		///	/>
		public uint UserData{
			get{return this.m_userData;}
			set{m_userData = value;}
		}	//	end UserData
		

		/// <summary>
		///	OTB internal identification handler
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="Handle"]/doc/*'
		///	/>	
		internal uint Handle{
			get{return this.m_handle;}
			set{this.m_handle = value;}
		}	//	end Handle
		
		/// <summary>
		///	The associated <see cref="AeConditionDefinition"/> attribute.
		/// </summary>		
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			property[@name="Definition"]/doc/*'
		///	/>	
		public AeConditionDefinition Definition{
			get{return this.m_definition;}
			set{this.m_definition = value;}
		}	//	end Definition
		
		//-
		#endregion

		#region //	Public Methods
		//------------------------
		
		/// <summary>
		/// Prepares a condition or a sub-condition a condition or subcondition for posting an activation
		/// </summary>
		/// <param name="aMessage">
		/// The activation event message string
		/// </param>
		/// <param name="aSeverity">
		/// The activation event severity value
		/// </param>
		/// <param name="aSubConditionName">
		/// The subcondition to be activated
		/// </param>
		/// <param name="anAckRequired">
		/// Is client acknowledgement required?
		/// </param>
		/// <param name="anOccuranceTime">
		/// The timestamp of the activation
		/// </param>
		/// <returns>
		/// S_OK - condition may be changed now
		/// </returns>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			method[@name="Activate"]/doc/*'
		///	/>
		public virtual uint Activate(
			string aMessage, 
			uint aSeverity, 
			string aSubConditionName, 
			bool anAckRequired, 
			DateTime anOccuranceTime){

			this.m_message = aMessage;
			this.m_severity = aSeverity;
			this.m_activeSubConditionName = aSubConditionName;
			this.m_ackRequired = anAckRequired;
			this.m_occurenceTime = anOccuranceTime;
			
			this.m_changeMask = (ushort)(	EnumConditionChange.ACTIVE_STATE | 
				EnumConditionChange.SEVERITY | 
				EnumConditionChange.ACK_STATE | 
				EnumConditionChange.MESSAGE | 
				EnumConditionChange.SUBCONDITION | 
				EnumConditionChange.ATTRIBUTE);

			this.m_stateChange = EnumConditionState.ACTIVE;
			
			return (uint)EnumResultCode.S_OK;

		}	//	end Activate
		

		/// <summary>
		/// Prepares a condition or a sub-condition a condition or subcondition for posting a deactivation message.
		/// </summary>
		/// <param name="aMessage">
		/// The deactivation event message string.
		/// </param>
		/// <param name="aSeverity">
		/// The activation event severity value.
		/// </param>
		/// <param name="anAckRequired">
		/// Is client acknowledgement required?
		/// </param>
		/// <param name="anOccuranceTime">
		/// The timestamp of the deactivation
		/// </param>
		/// <returns>
		/// S_OK - condition may be changed now
		/// </returns>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			method[@name="Deactivate"]/doc/*'
		///	/>
		public virtual uint Deactivate(
			string aMessage,
			uint aSeverity,
			bool anAckRequired,
			DateTime anOccuranceTime){

			this.m_message = aMessage;
			this.m_severity = aSeverity;			
			this.m_ackRequired = anAckRequired;
			this.m_occurenceTime = anOccuranceTime;
			this.m_stateChange = EnumConditionState.NO;

			this.m_changeMask = (ushort)(	EnumConditionChange.ACTIVE_STATE | 
				EnumConditionChange.SEVERITY | 
				EnumConditionChange.MESSAGE | 
				EnumConditionChange.ATTRIBUTE);
			
			return (uint)EnumResultCode.S_OK;

		}	//	end Deactivate 
		
		
		/// <summary>
		/// Prepares a condition or a sub-condition a condition or 
		/// subcondition for posting an Acknoeweledge message.
		/// </summary>
		/// <param name="anAckId">
		/// the acknowledgement Id
		/// </param>
		/// <param name="anAckComment">
		/// the acknowledgement comment
		/// </param>
		/// <param name="anAckTime">
		/// the acknowledgement timestamp
		/// </param>
		/// <returns>
		/// S_OK - condition may be changed now
		/// </returns>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			method[@name="Acknowledge"]/doc/*'
		///	/>
		public virtual uint Acknowledge(
			string anAckId, 
			string anAckComment,
			DateTime anAckTime){
			
			this.m_ackId = anAckId;
			this.m_ackComment = anAckComment;
			this.m_ackTime = anAckTime;
			
			this.m_stateChange = EnumConditionState.ACKED;

			this.m_changeMask = (ushort)(	
				EnumConditionChange.ACK_DATA |
				EnumConditionChange.ACK_STATE);
			
			return (uint)EnumResultCode.S_OK;

		}	//	end Acknowledge		
		
		/// <summary>
		/// Used to trigger the changes made to this condition - generates an event
		/// </summary>
		/// <returns>
		/// S_OK - condition changed successfully
		/// E_FAIL - failed to change condition
		/// </returns>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			method[@name="Change"]/doc/*'
		///	/>
		public virtual int Change(){
			return Application.Instance.ChangeConditions(new AeCondition[]{this});
		}	//	end Change
		
		/// <summary>
		/// Enables the condition triggering for the sepcified element
		/// </summary>
		/// <param name="anAddressSpaceElementPath">
		/// the path of the element to be enaled
		/// </param>
		/// <returns>
		/// the returned code is E_NOTIMPL. The user must implement this method
		/// </returns>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			method[@name="Enable"]/doc/*'
		///	/>
		public virtual int Enable(string anAddressSpaceElementPath){
			return (int)EnumResultCode.E_NOTIMPL;
		}	//	end Enable

		/// <summary>
		/// Disables the condition triggering for the sepcified element
		/// </summary>
		/// <param name="anAddressSpaceElementPath">
		/// the path of the element to be enaled
		/// </param>
		/// <returns>
		/// the returned code is E_NOTIMPL. The user must implement this method
		/// </returns>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			method[@name="Disable"]/doc/*'
		///	/>
		public virtual int Disable(string anAddressSpaceElementPath){

			return (int)EnumResultCode.E_NOTIMPL;
		}	//	end Disable
		
		#endregion

		#region	//	Internal Methods
		//--------------------------
		
		/// <summary>
		/// Internal helper method called to allocate an OTSConditionData class
		/// </summary>
		/// <param name="conditionData">
		/// reference to the condition data to be filled in
		/// </param>
		/// <returns></returns>
		internal uint FillOTSConditionData(ref OTSConditionData conditionData){
						
			conditionData.m_eventCategory	= this.m_eventCategory.Id;
			conditionData.m_name			= Marshal.StringToCoTaskMemUni(this.m_name);
			conditionData.m_sourcePath		= Marshal.StringToCoTaskMemUni(this.m_sourcePath);
			conditionData.m_quality			= (ushort)this.m_quality;

			conditionData.m_userData		= this.m_userData;

			conditionData.m_severity		= this.m_severity;
			conditionData.m_message			= Marshal.StringToCoTaskMemUni(this.m_message);
			conditionData.m_ackRequired		= Convert.ToByte(this.m_ackRequired);
			conditionData.m_activeSubConditionName = Marshal.StringToCoTaskMemUni(this.m_activeSubConditionName);
			conditionData.m_changeMask		= this.m_changeMask;
			conditionData.m_stateChange		= (byte)this.m_stateChange;
			
			conditionData.m_occurenceTime	= new OTDateTime(this.m_occurenceTime);
			conditionData.m_ackID			= Marshal.StringToCoTaskMemUni(this.m_ackId);
			conditionData.m_ackComment		= Marshal.StringToCoTaskMemUni(this.m_ackComment);
			conditionData.m_ackTime			= new OTDateTime(this.m_ackTime);

			conditionData.m_eventAttrCount	= (uint)this.m_eventAttributes.Count;
			if (conditionData.m_eventAttrCount > 0){
				
				IntPtr currentPointer = 
					conditionData.m_pEventAttrs = 
					Marshal.AllocCoTaskMem(((int)(conditionData.m_eventAttrCount*ValueQT.VARIANT_SIZE)));				
								
				foreach(object eventAttribute in m_eventAttributes){
					Marshal.GetNativeVariantForObject(eventAttribute, currentPointer);
					currentPointer = new IntPtr(currentPointer.ToInt64() + ValueQT.VARIANT_SIZE);
				}	//	end for
			}	//	end if
			
			return (uint)EnumResultCode.S_OK;

		}	//	end CreateOTSConditionData
				
		//-
		#endregion

		#region	//	Protected Attributes
		//------------------------------
		
		/// <summary>
		/// The current condition state
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_stateChange"]/doc/*'
		///	/>
		protected EnumConditionState m_stateChange = EnumConditionState.ENABLED;
		
		/// <summary>
		/// the mask to be filled for the next change
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_changeMask"]/doc/*'
		///	/>		
		protected ushort m_changeMask	= 0;

		/// <summary>
		/// associated category
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_eventCategory"]/doc/*'
		///	/>
		protected AeCategory m_eventCategory = null;

		/// <summary>
		/// severity of the event to be triggered when condition changes
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_severity"]/doc/*'
		///	/>
		protected uint m_severity = 0;

		/// <summary>
		/// the address space path for the triggered event
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_sourcePath"]/doc/*'
		///	/>
		protected string m_sourcePath = string.Empty;

		/// <summary>
		/// event's message
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_message"]/doc/*'
		///	/>
		protected string m_message = string.Empty;

		/// <summary>
		/// event's occurence timestamp
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_occurenceTime"]/doc/*'
		///	/>
		protected DateTime m_occurenceTime = new DateTime(1601,01,01,0,0,0,0);
		
		/// <summary>
		/// name of the condition
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_name"]/doc/*'
		///	/>
		protected string m_name = string.Empty;

		/// <summary>
		/// the currently active subcondition's name 
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_activeSubConditionName"]/doc/*'
		///	/>
		protected string m_activeSubConditionName = string.Empty;

		/// <summary>
		/// the condition's quality
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_quality"]/doc/*'
		///	/>
		protected EnumQuality m_quality = EnumQuality.BAD_WAITING_FOR_INITIAL_DATA;
		
		/// <summary>
		/// is ack required for condition's enable event
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_ackRequired"]/doc/*'
		///	/>
		protected bool m_ackRequired = false;
		
		/// <summary>
		/// tha ack Id received from the client
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_ackId"]/doc/*'
		///	/>
		protected string m_ackId = string.Empty;
		
		/// <summary>
		/// the comment received together with the ack from the client
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_ackComment"]/doc/*'
		///	/>
		protected string m_ackComment = string.Empty;

		/// <summary>
		/// the timestamp of the ack received from the client
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_ackTime"]/doc/*'
		///	/>
		protected DateTime m_ackTime = new DateTime(1601,01,01,0,0,0,0);

		/// <summary>
		/// the list of condition's event attributes
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_eventAttributes"]/doc/*'
		///	/>
		protected ArrayList m_eventAttributes = new ArrayList();

		/// <summary>
		/// internel toolkit identifier of the condition
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_userData"]/doc/*'
		///	/>
		protected uint m_userData = 0;

		/// <summary>
		/// the condition definition associated
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeCondition"]/
		///			attribute[@name="m_definition"]/doc/*'
		///	/>				
		protected AeConditionDefinition m_definition = null;
		
		//-
		#endregion
		
		#region //	Private Attributes
		//----------------------------

		/// <summary>
		/// the OTB condition identifier
		/// </summary>
		private uint m_handle = 0;
		
		//-
		#endregion

	}	//	end class AeCondition
	
	/// <summary>
	/// Stores the OPC specific properties of a <see cref="AeCondition"/>
	/// <para>
	/// Is only supposed to be used as attribute of a <see cref="AeCondition"/>
	/// </para>
	/// </summary>
	/// <include 
	///		file='TBNS.doc.xml' 
	///		path='//class[@name="AeConditionDefinition"]/doc/*'
	///	/>
	public class AeConditionDefinition{

		#region //	Public Constructors
		//-----------------------------
		
		/// <summary>
		/// Public constructor
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml'		
		///			path='//class[@name="AeConditionDefinition"]/
		///			ctor[@name="AeConditionDefinition"]/doc/*'
		///	/>
		public AeConditionDefinition(string aDefinition){
			m_definition = aDefinition;
		}	//	end Constructor
		
		/// <summary>
		/// Public destructor
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml'		
		///			path='//class[@name="AeConditionDefinition"]/
		///			dtor[@name="AeConditionDefinition"]/doc/*'
		///	/>
		~AeConditionDefinition(){
		
			this.m_definition = string.Empty;
			this.m_subconditions = null;

		}	//	end Constructor

		//-
		#endregion
	
		#region //	Public Properties 
		//---------------------------
		
		/// <summary>
		/// Definition of the condition
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeConditionDefinition"]/
		///			property[@name="Definition"]/doc/*'
		///	/>
		public string Definition{
			get{
				return this.m_definition;
			}	//	end get

		}	//	end Definition
		
		/// <summary>
		/// the hashtable containing the <see cref="AeSubConditionDefinition"/> associated to this condition
		/// <para>
		/// the the key is the subcondition definition string
		/// </para>	
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeConditionDefinition"]/
		///			property[@name="SubConditions"]/doc/*'
		///	/>
		public Hashtable SubConditions {
			get{
				Hashtable syncSubconditions = Hashtable.Synchronized(m_subconditions);
				return syncSubconditions;
			}	//	end get

		}	//	end SubConditions
		
		
		//-
		#endregion

		#region //	Public Methods
		//------------------------
		
		/// <summary>
		/// creates and adds a sub condition to the current condition
		/// </summary>
		/// <param name="aDefinition">
		/// the identifier of the subcondition
		/// </param>
		/// <param name="aDescription">
		/// a text description of the subcondition
		/// </param>
		/// <param name="aSeverity">
		/// the severity of the event benerated when this subcondition is matched
		/// </param>
		/// <returns>
		/// S_OK - succeeded
		/// </returns>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeConditionDefinition"]/
		///			method[@name="AddSubConditonData_full"]/doc/*'
		///	/>		
		public int AddSubConditonData(string aDefinition, string aDescription, uint aSeverity){
			
			AeSubConditionDefinition newSubCondition = new AeSubConditionDefinition(aDefinition, aDescription, aSeverity);
			Hashtable syncSubconditions = Hashtable.Synchronized(m_subconditions);
			syncSubconditions.Add(aDefinition, newSubCondition);
			return (int)EnumResultCode.S_OK;

		}	//	end AddSubConditonData

		/// <summary>
		/// creates and adds a sub condition to the current condition
		/// </summary>
		/// <param name="aDefinition">
		/// the identifier of the subcondition
		/// </param>
		/// <returns>
		/// S_OK - succeeded
		/// </returns>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeConditionDefinition"]/
		///			method[@name="AddSubConditonData_def"]/doc/*'
		///	/>		
		public int AddSubConditonData(string aDefinition){
			
			AeSubConditionDefinition newSubCondition = new AeSubConditionDefinition(aDefinition);
			Hashtable syncSubconditions = Hashtable.Synchronized(m_subconditions);
			syncSubconditions.Add(aDefinition, newSubCondition);
			return (int)EnumResultCode.S_OK;

		}	//	end AddSubConditonData
		
		/// <summary>
		/// retrieves a subcondition associated to the specified subcondition definition
		/// </summary>
		/// <param name="aDefinition">
		/// the identifier of the subcondition to be found
		/// </param>
		/// <returns>
		/// <para>
		/// a reference to the subcondition definition found in the table
		/// </para>
		/// <para>
		/// null when no match found 
		/// </para>
		/// </returns>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeConditionDefinition"]/
		///			method[@name="GetSubConditionDefinition"]/doc/*'
		///	/>
		public AeSubConditionDefinition GetSubConditionDefinition(string aDefinition){
		
			Hashtable syncSubconditions = Hashtable.Synchronized(m_subconditions);			
			return syncSubconditions[aDefinition] as AeSubConditionDefinition;
			
		}	//	end GetSubConditionDefinition

		/// <summary>
		/// CreateOTSConditionDefinitionData
		/// </summary>
		internal OTSConditionDefinitionData CreateOTSConditionDefinitionData(){
			
			OTSConditionDefinitionData conditionDefinitionData = new OTSConditionDefinitionData();
			conditionDefinitionData.m_definition = OTBFunctions.AllocateOTBString(this.m_definition);
			
			Hashtable syncSubconditions = Hashtable.Synchronized(m_subconditions);
			
			conditionDefinitionData.m_subConditionCount = (uint)syncSubconditions.Count;
			if (syncSubconditions.Count > 0){
				
				conditionDefinitionData.m_subConditionDefinitions = 
					OTBFunctions.OTAllocateMemory(syncSubconditions.Count * Marshal.SizeOf(typeof(IntPtr)));
				
				conditionDefinitionData.m_subConditionDescriptions = 
					OTBFunctions.OTAllocateMemory(syncSubconditions.Count * Marshal.SizeOf(typeof(IntPtr)));
				
				conditionDefinitionData.m_subConditionSeverities = 
					OTBFunctions.OTAllocateMemory(syncSubconditions.Count * Marshal.SizeOf(typeof(uint)));
				
				int index = 0;

				foreach(AeSubConditionDefinition subCondition in syncSubconditions.Values){
					
					IntPtr definitionPointer = OTBFunctions.AllocateOTBString(subCondition.Definition);
					Marshal.WriteIntPtr(
						conditionDefinitionData.m_subConditionDefinitions, 
						index*Marshal.SizeOf(typeof(IntPtr)),
						definitionPointer);
					
					IntPtr descriptionPointer = OTBFunctions.AllocateOTBString(subCondition.Description);
					Marshal.WriteIntPtr(
						conditionDefinitionData.m_subConditionDescriptions, 
						index*Marshal.SizeOf(typeof(IntPtr)),
						descriptionPointer);					
					
					Marshal.WriteInt32(conditionDefinitionData.m_subConditionSeverities, 
						index*Marshal.SizeOf(typeof(uint)),
						(int)subCondition.Severity);
					index++;
				}	//	end foreach

			}	//	end if

			return conditionDefinitionData;
			
		}	//	end CreateOTSConditionDefinitionData


		//-
		#endregion

		#region	//	Protected Attributes
		//------------------------------
		
		/// <summary>
		/// The identifier of the condition definition
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeConditionDefinition"]/
		///			attribute[@name="m_definition"]/doc/*'
		///	/>		
		protected string m_definition = string.Empty;

		/// <summary>
		/// The table containing the subconditions
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeConditionDefinition"]/
		///			attribute[@name="m_subconditions"]/doc/*'
		///	/>		
		protected Hashtable m_subconditions = new Hashtable();

		//-
		#endregion

	}	//	end AeConditionDefinition
	
	/// <summary>
	/// Stores a specific sub condition
	/// </summary>
	/// <include 
	///		file='TBNS.doc.xml' 
	///		path='//class[@name="AeSubConditionDefinition"]/doc/*'
	///	/>	
	public class AeSubConditionDefinition{

		#region //	Public Constructors
		//-----------------------------
		
		/// <summary>
		/// Public constructor 
		/// </summary>
		/// <param name="aDefinition">Definition string identifier</param>
		/// <include
		///		file='TBNS.doc.xml'
		///			path='//class[@name="AeSubConditionDefinition"]/
		///			ctor[@name="AeSubConditionDefinition_def"]/doc/*'
		///	/>
		public AeSubConditionDefinition(string aDefinition){
			this.m_definition = aDefinition;
		}	//	end Constructor
		
		/// <summary>
		/// Public constructor
		/// </summary>
		/// <param name="aDefinition">Definition string identifier</param>
		/// <param name="aDescription">Description string</param>
		/// <param name="aSeverity">associated severity of the event </param>
		/// <include
		///		file='TBNS.doc.xml'
		///			path='//class[@name="AeSubConditionDefinition"]/
		///			ctor[@name="AeSubConditionDefinition_all"]/doc/*'
		///	/>
		public AeSubConditionDefinition(
			string aDefinition, 
			string aDescription, 
			uint aSeverity){
						
			this.m_definition = aDefinition;
			this.m_description = aDescription;
			this.m_severity = aSeverity;

		}	//	end Constructor
		
		/// <summary>
		/// Destructor
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml'
		///			path='//class[@name="AeSubConditionDefinition"]/
		///			dtor[@name="AeSubConditionDefinition"]/doc/*'
		///	/>
		~AeSubConditionDefinition(){
		}	//	end dtor

		//-
		#endregion
	
		#region //	Public Properties 
		//--------------------------
		
		/// <summary>
		/// String identifier of the sub condition		
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeSubConditionDefinition"]/
		///			property[@name="Definition"]/doc/*'
		///	/>
		public string Definition{
			get{return this.m_definition;}			
		}	//	end Definition

		/// <summary>
		/// String description of the sub condition
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeSubConditionDefinition"]/
		///			property[@name="Description"]/doc/*'
		///	/>
		public string Description{
			get{return this.m_description;}
			set{this.m_description = value;}
		}	//	end Description

		/// <summary>
		/// Severity associated to this sub condition.
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeSubConditionDefinition"]/
		///			property[@name="Severity"]/doc/*'
		///	/>
		public uint Severity{
			get{return this.m_severity;}
			set{this.m_severity = value;}
		}	//	end Severity

		//-
		#endregion
		
		#region	//	Protected Attributes
		//------------------------------
		
		/// <summary>
		/// The identifier of the subcondition definition
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeSubConditionDefinition"]/
		///			attribute[@name="m_definition"]/doc/*'
		///	/>
		protected readonly string m_definition = string.Empty;

		/// <summary>
		/// The description of the sub-condition definition
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeSubConditionDefinition"]/
		///			attribute[@name="m_description"]/doc/*'
		///	/>		
		protected string m_description  = string.Empty;

		/// <summary>
		/// The severity of the sub-condition definition
		/// </summary>
		/// <include
		///		file='TBNS.doc.xml' 
		///			path='//class[@name="AeSubConditionDefinition"]/
		///			attribute[@name="m_severity"]/doc/*'
		///	/>		
		protected uint m_severity = 0;
		
		//-
		#endregion

	}	//	end AeSubConditionDefinition
	
}	//	end Softing.OPCToolbox.Server