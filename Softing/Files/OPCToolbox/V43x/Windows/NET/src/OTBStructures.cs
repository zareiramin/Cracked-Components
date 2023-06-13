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
//  Filename    : OTBStructures.cs                                            |
//  Version     : 4.31                                                        |
//  Date        : 01-August-2012                                              |
//                                                                            |
//  Description : OPC Toolbox Internal functions structures                   |
//                                                                            |
//-----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Softing.OPCToolbox.OTB {

#region Common - OT structures

	[StructLayout(LayoutKind.Sequential, Pack=4)]	
	internal struct OTObjectData {
		public IntPtr m_objectHandle;
		public uint m_userData;
	}

	[StructLayout(LayoutKind.Sequential, Pack=4)]
	internal struct OTDateTime {

		public uint dwLowDateTime;
		public uint dwHighDateTime;
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="aTimeStamp"></param>
		/// <returns></returns>
		public OTDateTime (DateTime aTimeStamp){
			
			try{
				ulong ulTime = (ulong)aTimeStamp.ToFileTimeUtc();
							
				this.dwLowDateTime = (uint)((ulTime & 0x00000000ffffffff));
				this.dwHighDateTime = (uint)((ulTime  >> 32 ) & 0x00000000ffffffff);
			}
			catch{
				this.dwHighDateTime = 0;
				this.dwLowDateTime = 0;
			}	//	end catch

		}	//	end ctr

		/// <summary>
		/// transforms the structure to DateTime
		/// </summary>
		internal DateTime ToDateTime(){
			try{
				ulong lowNibble  = this.dwLowDateTime;
				ulong highNibble = this.dwHighDateTime;			
				highNibble = (highNibble << 32) & 0xFFFFFFFF00000000;
				ulong complete = highNibble | (lowNibble & 0x00000000FFFFFFFF);
				DateTime dateTime = DateTime.FromFileTimeUtc((long)complete);
				return dateTime;
			}
			catch{
				return new DateTime(1601,1,1);
			}	//	end try ... catch
		}	//	end ToDateTime

	}	//	end OTDateTime

	[StructLayout(LayoutKind.Sequential, Pack=4)]
	internal struct OTValueData {
		public IntPtr m_value;
		public ushort m_quality;
		public OTDateTime m_timestamp;
	}

	[StructLayout(LayoutKind.Sequential, Pack=4,CharSet = CharSet.Unicode)]
	internal class OTCTraceData {
		public uint m_errorLevelMask;
		public uint m_warningLevelMask;
		public uint m_infoLevelMask ;
		public uint m_debugLevelMask;
		public byte m_enableTraceToFile ;
		[MarshalAs(UnmanagedType.LPWStr)]public string m_fileName = "";		
		public uint m_maxFileSize = 1000000;
		public uint m_maxBackups;
	}

#endregion //Common - OT structures

#region Client - OTC structures
	
	[StructLayout(LayoutKind.Sequential, Pack=4, CharSet=CharSet.Unicode)]	
	internal class OTCCallbackFunctions {
		public OTOnTrace m_trace;
		public OTCOnStateChange m_stateChange;
		public OTCOnDataChange  m_dataChange;
		public OTCOnReadComplete m_readComplete;
		public OTCOnWriteComplete m_writeComplete;
		public OTCOnPerfromStateTransition m_perfromStateTransition;
		public OTCOnServerShutdown m_serverShutdown;
		public OTCOnUpdateAttributes m_updateAttributes;
		public OTCOnUpdateDAItemAttributes m_updateDaItemAttributes;
		public OTCOnValidateDAItems m_validateDaItems;
		public OTCOnGetServerStatus m_getServerStatus;	
		public OTCOnBrowseAddressSpace m_browseAddressSpace;
		public OTCOnGetDAProperties m_onGetDaProperties;
		public OTCOnBrowseServer m_browseServer;
		public OTCOnReceivedEvents m_receivedEvents;
		public OTCOnRefreshAEConditions m_refreshConditions;
		public OTCOnAcknowledgeAEConditions m_acknowledgeConditions;
		public OTCOnQueryAvailableAEFilters m_queryAvailableFilters;
		public OTCOnQueryAECategories m_queryCategories;
		public OTCOnQueryAEAttributes m_queryAttributes;
		public OTCOnQueryAEConditions m_queryConditons;
		public OTCOnQueryAESubConditions m_querySubConditions;
		public OTCOnQueryAESourceConditions m_querySourceConditions;
		public OTCOnGetAEConditionState m_getConditionState;
		public OTCOnEnableAEConditions m_enableConditions;
		public OTCOnGetErrorString m_getErrorString;
		public OTCOnLogon m_logon;
		public OTCOnLogoff m_logoff;
	}
	
	[StructLayout(LayoutKind.Sequential, Pack=4,CharSet=CharSet.Unicode)]	
	internal struct OTCServerData { 
		internal IntPtr m_clsid;
		internal IntPtr m_progId;
		internal IntPtr m_vProgId;
		internal IntPtr m_description;
		internal IntPtr m_url;
		internal byte m_opcSpecification;
	}	

	/// <summary>
	/// Keeps initiation data for the OPC Client
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack=4,CharSet=CharSet.Unicode)]
	internal class OTCInitData { 
		/// <summary>Used OTB version </summary>
		public short m_versionOtb = 431;
		
		/// <summary>Name of service; empty if no service application</summary>
		public string m_serviceName;
	}

	[StructLayout(LayoutKind.Sequential, Pack=4)]	
	internal struct OTCObjectContext {
		public	OTObjectData m_objectData;
		public	uint m_objectType;
		public	OTObjectData m_sessionData;
		public	OTObjectData m_subscriptionData;
	}

	[StructLayout(LayoutKind.Sequential, Pack=4)]
	internal struct OTCExecutionOptions {
		
		internal byte m_executionType;
		internal uint m_executionContext;
		
	}

	[StructLayout(LayoutKind.Sequential, Pack=4,CharSet=CharSet.Unicode)]
	internal class OTCServerStatus{
		public uint m_state = 0 ;                // COM + XML
		public OTDateTime m_startTime = new OTDateTime() ;      // COM + XML
		public OTDateTime m_currentTime = new OTDateTime();    // COM + XML
		public IntPtr m_vendorInfo = IntPtr.Zero;         // COM + XML
		public IntPtr m_productVersion = IntPtr.Zero;     // COM + XML
		public int m_countLCIDs = 0;
		public IntPtr m_supportedLCIDs = IntPtr.Zero ;      // COM + XML
		public OTDateTime m_lastUpdateTime = new OTDateTime() ; // COM
		public int m_groupCount = 0;           // COM
		public int m_bandwidth  = 0;            // COM
		public IntPtr m_statusInfo = IntPtr.Zero;         //       XML
	}	
	
	[StructLayout(LayoutKind.Sequential, Pack=4,CharSet = CharSet.Unicode)]
	internal struct OTCAddressSpaceBrowseOptions{
		
		//filter
		internal byte m_type;
		internal IntPtr m_elementNameFilter;
		internal uint m_accessRightsFilter;		// DA V1 and V2
		internal ushort m_dataTypeFilter;		// DA V1 and V2
		internal IntPtr m_vendorFilter;			// DA V3 and XML-DA

		// retrieve options
		internal uint m_maxElements;			// DA V3 and XML-DA

		// retrieve what data
		internal byte m_retrieveItemID;
		internal byte m_retrieveProperties;		// DA V3 and XML-DA
		internal byte m_retrievePropertyValues;	// DA V3 and XML-DA

		internal byte m_forceBrowseUp; //DA V2
	}

	[StructLayout(LayoutKind.Sequential, Pack=4,CharSet = CharSet.Unicode)]
	internal struct OTCAddressSpaceElementData {
		internal IntPtr m_objectHandle ;
		internal IntPtr m_name;
		internal byte m_type ;
		
		//DA
		internal IntPtr m_itemID ;
		internal IntPtr m_itemPath ;
	} 

	[StructLayout(LayoutKind.Sequential, Pack=4,CharSet = CharSet.Unicode)]
	internal class OTCObjectAttributes{

		// DA Session
		internal uint m_daSessionLCID = new uint();
		internal IntPtr m_daSessionClientName = IntPtr.Zero;
		internal IntPtr m_daSessionURL = IntPtr.Zero;
        internal uint m_daSessionRequestTimeout = 0;
        internal byte m_daSessionSupportedOPCSpecification = 0;
		internal byte m_daSessionForcedOPCSpecification = 0;
		internal uint m_daSessionComClsctx = 0;
        internal bool m_daSessionPerformInitialRead = false;

        // DA Subscription
		internal IntPtr  m_daSubscriptionName = IntPtr.Zero;
		internal uint m_daSubscriptionRequestedUpdateRate = 0;
		internal uint m_daSubscriptionRevisedUpdateRate= 0;
		internal float m_daSubscriptionDeadband = 0;
		internal int m_daSubscriptionTimeBias = 0;
		internal uint m_daSubscriptionLCID = 0;
		internal uint m_daSubscriptionRequestedKeepAliveTime = 0;
		internal uint m_daSubscriptionRevisedKeepAliveTime = 0;

		// DA Item
		internal IntPtr m_daItemID = IntPtr.Zero;
		internal IntPtr m_daItemPath = IntPtr.Zero;
		internal ushort m_daItemNativeDatatype = 0;
		internal ushort m_daItemRequestedDatatype = 0;
		internal uint m_daItemAccessRights = 0;
		internal float m_daItemDeadband = 0;
		internal byte  m_daItemEuType = 0;
		internal IntPtr m_daItemEuInfo =IntPtr.Zero;// *OTCVariant	

		// AE Session
		internal uint m_aeSessionLCID = 0;
		internal IntPtr m_aeSessionClientName = IntPtr.Zero;
		internal IntPtr m_aeSessionURL = IntPtr.Zero;
		internal uint  m_aeSessionComClsctx = 0;

		// AE Subscription
		internal uint m_aeSubscriptionRequestedBufferTime = 0;
		internal uint m_aeSubscriptionRevisedBufferTime = 0;
		internal uint m_aeSubscriptionRequestedMaxSize = 0;
		internal uint m_aeSubscriptionRevisedMaxSize = 0;
		internal uint m_aeSubscriptionFilterSeverityLow = 0;
		internal uint m_aeSubscriptionFilterSeverityHigh = 00;
		internal uint m_aeSubscriptionFilterEventTypes = 0;
		internal uint m_aeSubscriptionFilterCountCategrories = 0;
		internal IntPtr m_aeSubscriptionFilterCategrories = IntPtr.Zero;
		internal uint m_aeSubscriptionFilterCountAreas = 0;		
		internal IntPtr m_aeSubscriptionFilterAreas = IntPtr.Zero;
		internal uint m_aeSubscriptionFilterCountSources = 0;
		internal IntPtr m_aeSubscriptionFilterSources = IntPtr.Zero;
		internal uint m_aeSubscriptionCountReturnedAttributes = 0;
		internal IntPtr m_aeSubscriptionReturnedAttributes = IntPtr.Zero;
	}
	
	[StructLayout(LayoutKind.Sequential, Pack=4, CharSet = CharSet.Unicode)]
	internal class OTCGetDAPropertiesOptions {
		
		// get properties stored in the address space element
		public byte m_getLocalFromAddressSpaceElement = 0;  
		
		// only used if !m_getLocalFromAddressSpaceElement
		public byte m_whatPropertyData = 0;
		
		public uint m_propertyCount = 0;
		public IntPtr m_pPropertyIDs = IntPtr.Zero;
		public IntPtr m_pPropertyNames = IntPtr.Zero;

	}	//	end struct OTCGetDAPropertiesOptions
	
	[StructLayout(LayoutKind.Sequential, Pack=4, CharSet = CharSet.Unicode)]
	internal struct OTCDAPropertyData {	
		
		public uint m_ID;// = 0;
		public IntPtr m_name;// = IntPtr.Zero;	
		public IntPtr m_itemID;// = IntPtr.Zero;
		public IntPtr m_itemPath;// = IntPtr.Zero;
		public IntPtr m_description;// = IntPtr.Zero;	
		public ushort m_datatype ;//= 0;				
		public OTValueData m_value;// = new OTValueData();//OTCValueData
		public int m_result;// = -1;
	}	//	end struct OTCDAPropertyData

	[StructLayout(LayoutKind.Sequential, Pack=4, CharSet = CharSet.Unicode)]
	internal struct OTCEventData {
			public uint m_eventType;
			public uint m_eventCategory;       // event category id
			public uint m_severity;            // severity
			public IntPtr m_sourcePath;         // fully qualified path of event source
			public IntPtr m_message;            // event message 
			public OTDateTime m_occurenceTime; // time of event occurance
			public uint m_eventAttrCount;
			public IntPtr m_pEventAttrs;  // OTCVariant* array with all event attributes
			public IntPtr m_actorID;
			// condition related event data
			public ushort m_changeMask;
			public byte m_newState;
			public byte m_ackRequired;
			public ushort m_quality;
			public IntPtr m_conditionName;
			public IntPtr m_subConditionName;
			public OTDateTime m_activeTime; 
			public uint m_cookie;
	} 
	[StructLayout(LayoutKind.Sequential, Pack=4, CharSet = CharSet.Unicode)]
	internal class OTCAEConditionStateData{
		// condition state data
		public byte m_state = 0;
		public IntPtr m_actSubConditionName = IntPtr.Zero; //string
		public IntPtr m_actSubConditionDefinition = IntPtr.Zero;//string
		public uint m_actSubConditionSeverity = 0;
		public IntPtr m_actSubConditionDescription = IntPtr.Zero;//string
		public OTDateTime m_conditionActiveTime = new OTDateTime();
		public OTDateTime m_conditionAckTime = new OTDateTime();
		public OTDateTime m_subConditionActiveTime = new OTDateTime();
		public OTDateTime m_conditionInactiveTime = new OTDateTime();
		public ushort m_quality = 0;
		public IntPtr m_ackID = IntPtr.Zero; // string
		public IntPtr m_ackComment = IntPtr.Zero;//string
		public uint m_subConditionCount = 0;
		public IntPtr m_pSubConditionDefinition = IntPtr.Zero;//string*
		public IntPtr m_pSubConditionSeverity = IntPtr.Zero;//uint*
		public IntPtr m_pSubConditionDescription = IntPtr.Zero;//string*
		public IntPtr m_pSubConditionName = IntPtr.Zero;//string*
		public uint m_eventAttrCount = 0;
		public IntPtr m_pEventAttrs = IntPtr.Zero;  // OTCVariant* array with all event attributes
		public IntPtr m_pAttrErrors = IntPtr.Zero; //int*
	
	}
	[StructLayout(LayoutKind.Sequential, Pack=4, CharSet = CharSet.Unicode)]
	internal class OTCAEReturnedAttributesData{
		public uint m_categoryID = 0;
		public uint m_countAttributeIDs = 0;
		public IntPtr m_attributeIDs = IntPtr.Zero;
	}
#endregion //Client - OTC structures

#region Server - OTS structures

	[StructLayout(LayoutKind.Sequential, Pack=4)]
	internal class OTSCallbackFunctions{
		
		public OTOnTrace m_OTOnTrace;
		public OTSShutdown m_OTSShutdown;
		public OTSHandleReadRequests m_OTSHandleReadRequests;
		public OTSHandleWriteRequests m_OTSHandleWriteRequests;
		public OTSQueryProperties m_OTSQueryProperties;
		public OTSQueryAddressSpaceElementData m_OTSQueryAddressSpaceElementData;
		public OTSQueryAddressSpaceElementChildren m_OTSQueryAddressSpaceElementChildren;
		public OTSChangeSessionState m_OTSChangeSessionState;
		public OTSQueryCacheValue m_OTSQueryCacheValue;
		public OTSChangeItems m_OTSChangeItems;
		public OTSCreateAddressSpaceElement m_OTSCreateAddressSpaceElement;
		public OTSDestroyAddressSpaceElement m_OTSDestroyAddressSpaceElement;
		public OTSQueryConditions m_OTSQueryConditions;
		public OTSAcknowledgeCondition m_OTSAcknowledgeCondition;
		public OTSQueryConditionDefinition m_OTSQueryConditionDefinition;
		public OTSEnableConditions m_OTSEnableConditions;
		public OTSWebHandleTemplate m_OTSWebHandleTemplate;		
	}

	/// <summary>
	/// Keeps initiaton data for the OPC Server
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack=4, CharSet=CharSet.Unicode)]
	internal class OTSInitData {
		/// <summary>Used OTS version </summary>
		public short m_versionOTS;

		/// <summary>Application type of OPC server </summary>
		public byte m_appType;

		/// <summary>Name of service; empty if no service application</summary>
		public string m_serviceName;

		/// <summary>Class ID of DA server </summary>
		public string m_clsidDA;

		/// <summary>Prog ID of DA server </summary>
		public string m_progIdDA;

		/// <summary>Version independent prog ID of DA server </summary>
		public string m_verIndProgIdDA;

		/// <summary>Class ID of AE server </summary>
		public string m_clsidAE;

		/// <summary>Prog ID of AE server </summary>
		public string m_progIdAE;

		/// <summary>Version independent prog ID of AE server </summary>
		public string m_verIndProgIdAE;

		/// <summary>Description of server </summary>
		public string m_description;

		/// <summary>IP port number of HTTP server </summary>
		public int m_ipPortHTTP;

		/// <summary>URL of XML-DA server </summary>
		public string m_urlDA;
		
		/// <summary>Major version number of the server </summary>
		public short m_majorVersion;

		/// <summary>Minor version number of the server </summary>
		public short m_minorVersion;
		
		/// <summary>Patch version number of the server </summary>
		public short m_patchVersion;
		
		/// <summary>Build number of the server </summary>
		public short m_buildNumber;
		
		/// <summary>Version info of the server </summary>
		public string m_vendorInfo;				
				
		/// <summary>Delimiter sign for namespace levels in the item ID </summary>
		public char m_addressSpaceDelimiter;		 
		
		/// <summary>Period in ms to check the notification connections to the clients </summary>
		public int m_clientCheckPeriod;				
		
		/// <summary>Delimiter sign for DA properties in the item ID </summary>
		public char m_propertyDelimiter;     		
		
		/// <summary>Minimal update rate for a group (ms) </summary>
		public int m_minUpdateRateDA;

		/// <summary>support to enable or disable conditions</summary>	
		public byte m_supportDisableConditions;
		
		/// <summary> root directory for web pages</summary>
		public string m_webRootDirectory;
		
		/// <summary>root file of the web server; default: index.html</summary>
		public string m_webRootFile;
		
		/// <summary>web administrator user password</summary>
		public string m_webAdministratorPassword;

		/// <summary>web operator user password</summary>
		public string m_webOperatorPassword;

		/// <summary>The port number which is used by TunnelProtocol to listen for connections</summary>
		public ushort m_tpPort;

		/// <summary> the number of TP credentials</summary>
		public int m_tpCredentialsNumber;

		/// <summary>the list containing the TP users</summary>
		public IntPtr m_tpUsers;
		
		/// <summary>the list containing the passwords</summary>		
		public IntPtr m_tpPasswords;
		
		public void Release()
		{
			if (this.m_tpCredentialsNumber > 0)
			{				
				for (int index = 0; index < m_tpCredentialsNumber; index++)
				{
					IntPtr stringPointer = Marshal.ReadIntPtr(
						m_tpUsers,
						index * Marshal.SizeOf(typeof(IntPtr)));
					OTBFunctions.OTFreeMemory(stringPointer);

					stringPointer = Marshal.ReadIntPtr(
						m_tpPasswords, 
						index * Marshal.SizeOf(typeof(IntPtr)));										
					OTBFunctions.OTFreeMemory(stringPointer);
				}	//	end for				
				
				m_tpCredentialsNumber = 0;
				m_tpUsers = IntPtr.Zero;
				m_tpPasswords = IntPtr.Zero;

			}	//	end if
		}	//	end Release

	}	// end class 


	/// <summary>
	/// Keeps data for the NamespaceElement
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
	internal class OTSAddressSpaceElementData{

		public byte m_elementType;
		public IntPtr m_name;
		public IntPtr m_itemID;
		public byte m_accessRights;
		public byte m_ioMode;
		public byte m_hasChildren;
		public byte m_isBrowsable;
		public ushort m_datatype;
		public uint m_userData;
	}

	/// <summary>
	/// Keeps data of the request for the inner layer
	/// </summary>	
	[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
	internal struct OTSRequestData{

		public OTObjectData m_object;		
		public int m_propertyID;
		public IntPtr m_requestHandle;
		public IntPtr m_sessionHandle;
	}
	
	
	[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]  
	internal struct OTSItemData{
		public OTObjectData m_object;
		public byte m_active;
		public int m_sampleRate;
	}	//	end class OTSItemData
	
	[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]  
	internal class OTSSessionData{
		public short m_state = 0;			// client state : OTS_CLIENTSTATE_XXX
		public byte m_type = 0;				// type of the client : OPC_CLIENTTYPE_XXX
		
		// OTS_CLIENTSTATE_LOGON
		public string m_userName = string.Empty; // logon user name
		public string m_password = string.Empty; // logon password

		public string m_clientName = string.Empty; // client name
		public byte m_clientRemote = 0;	// 0 for localhost, != 0 for remote
		public string m_clientIp = string.Empty;             // client Ip (and port)
	}
	
	[StructLayout(LayoutKind.Sequential, Pack = 4,CharSet = CharSet.Unicode)] 
	internal class OTSPropertyData{

		public int m_pid = 0;					// property id
		public IntPtr m_name = IntPtr.Zero;		// property name
		public IntPtr m_itemID = IntPtr.Zero;	// property part of the item id
		public IntPtr m_descr = IntPtr.Zero;	// description 
		public ushort m_datatype = 0;			// data type
		public byte m_accessRights = 0;			// access rights over item

	}	//	OTSDAPropertyData
	
	[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
	internal class OTSEventData {

		public uint m_eventType = 0;				//	
		public uint m_eventCategory = 0;			//	event category id
		public uint m_severity = 0;					//	severity				
		public IntPtr m_sourcePath = IntPtr.Zero;	//	fully qualified path of event source		
		public IntPtr m_message = IntPtr.Zero;		//	event message		
		public OTDateTime m_occurenceTime;			//	time of event occurance		
		public IntPtr m_actorID = IntPtr.Zero;		//	the actor generating the event
		public uint m_eventAttrCount = 0;			//	the counter of attributes		
		public IntPtr m_pEventAttrs = IntPtr.Zero;	//	array with all event attributes
		
		public void Release(){
			
			Marshal.FreeCoTaskMem(this.m_sourcePath);
			Marshal.FreeCoTaskMem(this.m_message);
			Marshal.FreeCoTaskMem(this.m_actorID);

			if (this.m_eventAttrCount != 0 && 
				this.m_pEventAttrs != IntPtr.Zero){
					
				IntPtr currentPointer = this.m_pEventAttrs;
				for (int index = 0; index < this.m_eventAttrCount; index++){

					Win32Functions.VariantClear(currentPointer);
					currentPointer = new IntPtr(currentPointer.ToInt64() + ValueQT.VARIANT_SIZE);
				}	//	end for

				Marshal.FreeCoTaskMem(this.m_pEventAttrs);
			}	//	end if

		}	//	end Release

	}	//	 end class OTSEventData;
	
	[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
	internal class OTSConditionData {
	
		public byte m_stateChange	= 0;
		public ushort m_changeMask	= 0;
		public uint m_eventCategory = 0;			// event category id
		public uint m_severity = 0;					// severity
		public IntPtr m_sourcePath = IntPtr.Zero;	// fully qualified path of event source
		public IntPtr m_message = IntPtr.Zero;		// event message 
		public OTDateTime m_occurenceTime;			// time of event occurance
		public IntPtr m_name = IntPtr.Zero;
		public IntPtr m_activeSubConditionName = IntPtr.Zero;
		public ushort m_quality = 0;
		public byte m_ackRequired = 0;
		public IntPtr m_ackID = IntPtr.Zero;
		public IntPtr m_ackComment = IntPtr.Zero;
		public OTDateTime m_ackTime;
		public uint m_eventAttrCount = 0;
		public IntPtr m_pEventAttrs = IntPtr.Zero;	// array with all event attributes
		public uint m_userData = 0;
	
		/// <summary>
		/// Release
		/// </summary>		
		public void Release(){
			
			Marshal.FreeCoTaskMem(this.m_name);
			Marshal.FreeCoTaskMem(this.m_sourcePath);
			Marshal.FreeCoTaskMem(this.m_message);
			Marshal.FreeCoTaskMem(this.m_activeSubConditionName);
			Marshal.FreeCoTaskMem(this.m_ackID);
			Marshal.FreeCoTaskMem(this.m_ackComment);

			if (this.m_eventAttrCount != 0 && 
				this.m_pEventAttrs != IntPtr.Zero){
				
				IntPtr currentPointer = this.m_pEventAttrs;
				for (int index = 0; index < this.m_eventAttrCount; index++){

					Win32Functions.VariantClear(currentPointer);
					currentPointer = new IntPtr(currentPointer.ToInt64() + ValueQT.VARIANT_SIZE);
				}	//	end for

				Marshal.FreeCoTaskMem(this.m_pEventAttrs);
			}	//	end if
		}	//	end Release

	}	//	end class OTSConditionData
	

	[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
	internal class OTSConditionDefinitionData {
		
		public IntPtr m_definition = IntPtr.Zero;
		public uint m_subConditionCount = 0;
		public IntPtr m_subConditionDefinitions = IntPtr.Zero;
		public IntPtr m_subConditionDescriptions  = IntPtr.Zero;
		public IntPtr m_subConditionSeverities = IntPtr.Zero;

		public void Release(){
			
			OTBFunctions.OTFreeMemory(this.m_definition);

			if (this.m_subConditionCount > 0){
				
				for (int index = 0; index < m_subConditionCount; index++){
					
					IntPtr stringPointer = Marshal.ReadIntPtr(
						m_subConditionDefinitions, 
						index * Marshal.SizeOf(typeof(IntPtr)));
					OTBFunctions.OTFreeMemory(stringPointer);

					stringPointer = Marshal.ReadIntPtr(
						m_subConditionDescriptions, 
						index * Marshal.SizeOf(typeof(IntPtr)));					
					OTBFunctions.OTFreeMemory(stringPointer);

				}	//	end for
				
				OTBFunctions.OTFreeMemory(this.m_subConditionDefinitions);
				OTBFunctions.OTFreeMemory(this.m_subConditionDescriptions);
				OTBFunctions.OTFreeMemory(this.m_subConditionSeverities);
			}	//	end if

		}	//	end Release
	
	}	//	end class OTSConditionDefinitionData
	
#endregion // Server - OTS structures

}	//	end namespace Softing.OPCToolbox.OTB
