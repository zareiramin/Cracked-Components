using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Graybox.OPC.ServerToolkit.CLRWrapper
{
    /// <summary>
    /// A CLR wrapper for the native GBDataAccess class.
    /// </summary>
    public class OPCDAServer
    {
        private IntPtr m_NativeHandle = IntPtr.Zero;

        private OPCDAServerEvents m_Events;
		/// <summary>
		/// Gets the object that holds the event handlers of an OPCDAServer object.
		/// </summary>
		public OPCDAServerEvents Events
		{
			get { return m_Events; }
        }

        #region Event handlers ----------------------------------------------------------------

        private Int32 OnBeforeCreateInstanceCallback(bool bAggregating, ref Int32 pdwClientId)
        {
            BeforeCreateInstanceArgs e = new BeforeCreateInstanceArgs(bAggregating, pdwClientId);
            m_Events.OnBeforeCreateInstance(e);
            pdwClientId = e.ClientId;
            return (int)e.EventHandlingError;
        }

        private void OnCreateInstanceCallback(Int32 dwClientId)
        {
            CreateInstanceArgs e = new CreateInstanceArgs(dwClientId);
            m_Events.OnCreateInstance(e);
        }

        private void OnDestroyInstanceCallback(Int32 dwClientId)
        {
            m_Events.OnDestroyInstance(new DestroyInstanceArgs(dwClientId));
        }

        private void OnLockCallback()
        {
            m_Events.OnLock(new LockArgs());
        }

        private void OnUnlockCallback()
        {
            m_Events.OnUnlock(new UnlockArgs());
        }

        private Int32 OnServerReleasedCallback()
        {
            ServerReleasedArgs e = new ServerReleasedArgs(true);
            m_Events.OnServerReleased(e);
            if (e.Suspend) return (Int32)ErrorCodes.False;
            return (Int32)ErrorCodes.Ok;
        }

        unsafe private Int32 OnWriteItemsCallback(
            Int32 dwCount,
            IntPtr pTags,
            IntPtr pvValues,
            IntPtr pwQualities,
            IntPtr pftTimestamps,
            IntPtr pErrors,
            ref ErrorCodes pMasterError,
            Int32 dwLcid,
            Int32 dwClientID)
        {
            ItemId[] tags = new ItemId[dwCount];
            #if WindowsCE   // can't use Marshal.GetObjectsForNativeVariants in .NET CF
            object[] vals = Helpers.GetObjectsForNativeVariants(pvValues, dwCount);
            #else
            object[] vals = Marshal.GetObjectsForNativeVariants(pvValues, dwCount);
            #endif
            Quality[] quals = null;
            FileTime[] times = null;
            fixed (ItemId* p = &tags[0])
            {
                for (int i = 0; i < dwCount; i++) p[i] = ((ItemId*)pTags.ToPointer())[i];
            }
            if (pwQualities != IntPtr.Zero)
            {
                quals = new Quality[dwCount];
                fixed (Quality* p = &quals[0])
                {
                    for (int i = 0; i < dwCount; i++) p[i] = ((Quality*)pwQualities.ToPointer())[i];
                }
            }
            if (pftTimestamps != IntPtr.Zero)
            {
                times = new FileTime[dwCount];
                fixed (FileTime* p = &times[0])
                {
                    for (int i = 0; i < dwCount; i++) p[i] = ((FileTime*)pftTimestamps.ToPointer())[i];
                }
            }
            WriteItemsArgs e = new WriteItemsArgs(tags, vals, quals, times, dwLcid, dwClientID);
            m_Events.OnWriteItems(e);
            pMasterError = e.MasterError;
            fixed (ItemId* p = &tags[0])
            {
                for (int i = 0; i < dwCount; i++) ((ItemId*)pTags.ToPointer())[i] = p[i];
            }
            fixed (ErrorCodes* p = &e.Errors[0])
            {
                for (int i = 0; i < dwCount; i++) ((ErrorCodes*)pErrors.ToPointer())[i] = p[i];
            }
            if (e.CopyToCache) return (Int32)NativeConstants.GB_RET_CACHE;
            return (Int32)NativeConstants.GB_RET_NOP;
        }

        unsafe private Int32 OnReadItemsCallback(
                Int32 dwCount,
                IntPtr pTags,
                IntPtr pValues,
                IntPtr pQualities,
                IntPtr pTimestamps,
                IntPtr pErrors,
                ref ErrorCodes pMasterError,
                ref ErrorCodes pMasterQuality,
                IntPtr pRequestedTypes,
                Int32 dwLcid,
                Int32 dwClientID)
        {
            ItemId[] tags = new ItemId[dwCount];
            fixed (ItemId* p = &tags[0])
            {
                for (int i = 0; i < dwCount; i++) p[i] = ((ItemId*)pTags.ToPointer())[i];
            }
            ReadItemsArgs e = new ReadItemsArgs(tags, (pValues != IntPtr.Zero), dwLcid, dwClientID);
            m_Events.OnReadItems(e);
            if (pValues != IntPtr.Zero && e.ValuesReturned)
            {
                pMasterError = e.MasterError;
                pMasterQuality = e.MasterQuality;
                if (e.ValuesReturnedPartial)
                {
                    fixed (ItemId* p_tags = &tags[0])
                    {
                        fixed (ErrorCodes* p_errors = &e.Errors[0])
                        {
                            fixed (FileTime* p_timestamps = &e.Timestamps[0])
                            {
                                fixed (Quality* p_qualities = &e.Qualities[0])
                                {
                                    Int16* p_vt = (Int16*)pRequestedTypes.ToPointer();
                                    for (int i = 0; i < dwCount; i++)
                                    {
                                        ((ItemId*)pTags.ToPointer())[i] = p_tags[i];
                                        if (e.ItemIds[i].TagId != 0 || e.Values[i] == null) continue;
                                        ((Quality*)pQualities.ToPointer())[i] = p_qualities[i];
                                        ((FileTime*)pTimestamps.ToPointer())[i] = p_timestamps[i];
                                        ((ErrorCodes*)pErrors.ToPointer())[i] = p_errors[i];
                                        if (p_errors[i] < 0) continue;
                                        IntPtr unm_v = new IntPtr(&(((byte*)pValues.ToPointer())[i * Helpers.SizeOfVariant]));
                                        Marshal.GetNativeVariantForObject(e.Values[i], unm_v);
                                        int hr = Helpers.VariantChangeTypeEx(unm_v, unm_v, dwLcid, 0, p_vt[i]);
                                        if (hr < 0)
                                        {
                                            ((ErrorCodes*)pErrors.ToPointer())[i] = (ErrorCodes)hr;
                                            pMasterError = ErrorCodes.False;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return (Int32)NativeConstants.GB_RET_CACHE;
                }
                else
                {
                    fixed (ItemId* p_tags = &tags[0])
                    {
                        fixed (ErrorCodes* p_errors = &e.Errors[0])
                        {
                            fixed (FileTime* p_timestamps = &e.Timestamps[0])
                            {
                                fixed (Quality* p_qualities = &e.Qualities[0])
                                {
                                    Int16* p_vt = (Int16*)pRequestedTypes.ToPointer();
                                    for (int i = 0; i < dwCount; i++)
                                    {
                                        ((ErrorCodes*)pErrors.ToPointer())[i] = p_errors[i];
                                        ((FileTime*)pTimestamps.ToPointer())[i] = p_timestamps[i];
                                        ((Quality*)pQualities.ToPointer())[i] = p_qualities[i];
                                        if (e.ItemIds[i].TagId == 0 || (int)e.Errors[i] < 0) continue;
                                        IntPtr unm_v = new IntPtr(&(((byte*)pValues.ToPointer())[i * Helpers.SizeOfVariant]));
                                        Marshal.GetNativeVariantForObject(e.Values[i], unm_v);
                                        int hr = Helpers.VariantChangeTypeEx(unm_v, unm_v, dwLcid, 0, p_vt[i]);
                                        if (hr < 0)
                                        {
                                            ((ErrorCodes*)pErrors.ToPointer())[i] = (ErrorCodes)hr;
                                            pMasterError = ErrorCodes.False;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return (Int32)NativeConstants.GB_RET_ARG;
                }
            }
            return (Int32)NativeConstants.GB_RET_CACHE;
        }

        unsafe private void OnDataUpdateCallback(
                Int32 dwCount,
                IntPtr pTags,
                IntPtr pvValues,
                IntPtr pwQualities,
                IntPtr pftTimestamps,
                IntPtr pdwErrors,
                Int32 dwClientResult,
                Int32 dwClientID)
        {
            ItemId[] tags = new ItemId[dwCount];
            #if WindowsCE   // can't use Marshal.GetObjectsForNativeVariants in .NET CF
            object[] vals = Helpers.GetObjectsForNativeVariants(pvValues, dwCount);
            #else
            object[] vals = Marshal.GetObjectsForNativeVariants(pvValues, dwCount);
            #endif
            Quality[] quals = new Quality[dwCount];
            FileTime[] times = new FileTime[dwCount];
            ErrorCodes[] errors = new ErrorCodes[dwCount];
            fixed (ItemId* p_tags = &tags[0])
            {
                fixed (Quality* p_quals = &quals[0])
                {
                    fixed (FileTime* p_times = &times[0])
                    {
                        fixed (ErrorCodes* p_errors = &errors[0])
                        {
                            for (int i = 0; i < dwCount; i++)
                            {
                                p_tags[i] = ((ItemId*)pTags.ToPointer())[i];
                                p_quals[i] = ((Quality*)pwQualities.ToPointer())[i];
                                p_times[i] = ((FileTime*)pftTimestamps.ToPointer())[i];
                                p_errors[i] = ((ErrorCodes*)pdwErrors.ToPointer())[i];
                            }
                        }
                    }
                }
            }
            DataUpdateArgs e = new DataUpdateArgs(tags, vals, quals, times, errors, (ErrorCodes)dwClientResult, dwClientID);
            m_Events.OnDataUpdate(e);
        }

        unsafe private void OnActivateItemsCallback(Int32 dwCount, IntPtr pTags)
        {
            ItemId[] tags = new ItemId[dwCount];
            fixed (ItemId* p = &tags[0])
            {
                for (int i = 0; i < dwCount; i++) p[i] = ((ItemId*)pTags.ToPointer())[i];
            }
            m_Events.OnActivateItems(new ActivateItemsArgs(tags));
        }

        unsafe private void OnDeactivateItemsCallback(Int32 dwCount, IntPtr pTags)
        {
            ItemId[] tags = new ItemId[dwCount];
            fixed (ItemId* p = &tags[0])
            {
                for (int i = 0; i < dwCount; i++) p[i] = ((ItemId*)pTags.ToPointer())[i];
            }
            m_Events.OnDeactivateItems(new DeactivateItemsArgs(tags));
        }

        private Int32 OnGetErrorStringCallback(
            Int32 dwError,
            Int32 dwLcid,
            ref String pszErrorString,
            Int32 dwClientID)
        {
            GetErrorStringArgs e = new GetErrorStringArgs(dwError, dwLcid, dwClientID);
            m_Events.OnGetErrorString(e);
            if ((int)e.EventHandlingError >= 0)
            {
                if (e.RequestedErrorString == null) return unchecked((int)ErrorCodes.Fail);
                pszErrorString = e.RequestedErrorString;
            }
            return (int)e.EventHandlingError;
        }

        private Int32 OnQueryLocalesCallback(
            ref Int32 pdwCount,
            ref IntPtr ppdwLcid)
        {
            QueryLocalesArgs e = new QueryLocalesArgs(null);
            m_Events.OnQueryLocales(e);
            if ((int)e.EventHandlingError >= 0)
            {
                pdwCount = e.Cultures.Count + 3;
                ppdwLcid = e.Cultures.Unmanaged;
            }
            else
            {
                pdwCount = 0;
                ppdwLcid = IntPtr.Zero;
            }
            return (int)e.EventHandlingError;
        }

        unsafe private Int32 OnBrowseAccessPathsCallback(
            String szItemID,
            ref Int32 pdwAccessPathCount,
            ref IntPtr ppszAccessPaths,
            Int32 dwClientID)
        {
            BrowseAccessPathsArgs e = new BrowseAccessPathsArgs(szItemID, dwClientID);
            m_Events.OnBrowseAccessPaths(e);
            if ((int)e.EventHandlingError >= 0)
            {
                if (e.AccessPaths == null || e.AccessPaths.Length == 0)
                {
                    pdwAccessPathCount = 0;
                    return (int)ErrorCodes.False;
                }
                pdwAccessPathCount = e.AccessPaths.Length;
                IntPtr paths = Marshal.AllocHGlobal(IntPtr.Size * e.AccessPaths.Length);
                for (int i = 0; i < e.AccessPaths.Length; i++)
                {
                    #if WindowsCE
                    Marshal.WriteIntPtr(new IntPtr( &( ((byte*)paths.ToPointer())[i * IntPtr.Size] ) ),
                        Helpers.StringToHGlobalUni(e.AccessPaths[i]));
                    #else
                    Marshal.WriteIntPtr(paths, IntPtr.Size * i, Marshal.StringToHGlobalUni(e.AccessPaths[i]));
                    #endif
                }
                ppszAccessPaths = paths;
            }
            else
            {
                pdwAccessPathCount = 0;
            }
            return (int)e.EventHandlingError;
        }

        private Int32 OnQueryItemCallback(
            String szItemID,
            String szAccessPath,
            Int16 wDataType,
            Boolean bAddItem,
            ref Int32 pdwTagID,
            IntPtr pdwAccessPathID,
            Int32 dwClientID)
        {
            QueryItemArgs e = new QueryItemArgs(szItemID, szAccessPath, wDataType, bAddItem, dwClientID);
            m_Events.OnQueryItem(e);
            pdwTagID = e.TagId;
            if (pdwAccessPathID != IntPtr.Zero) Marshal.WriteInt32(pdwAccessPathID, e.AccessPathId);
            return (int)e.EventHandlingError;
        }

        unsafe private Int32 OnReadPropertiesCallback(
            ref ItemId pTag,
            Int32 dwCount,
            IntPtr pdwPropIDs,
            IntPtr pvValues,
            IntPtr pdwErrors,
            Int32 dwLcid,
            Int32 dwClientID)
        {
            int[] props = new int[dwCount];
            ErrorCodes[] errors = new ErrorCodes[dwCount];
            for (int i = 0; i < dwCount; i++)
            {
                props[i] = ((Int32*)pdwPropIDs.ToPointer())[i];
                errors[i] = ((ErrorCodes*)pdwErrors.ToPointer())[i];
            }
            ReadPropertiesArgs e = new ReadPropertiesArgs(pTag, props, errors, dwLcid, dwClientID);
            m_Events.OnReadProperties(e);
            if ((int)e.EventHandlingError < 0) return (int)e.EventHandlingError;
            for (int i = 0; i < dwCount; i++)
            {
                ((ErrorCodes*)pdwErrors.ToPointer())[i] = e.Errors[i];
                if ((int)e.Errors[i] < 0) continue;
                Marshal.GetNativeVariantForObject(e.Values[i], new IntPtr(&((byte*)pvValues.ToPointer())[Helpers.SizeOfVariant * i]));
            }
            return (int)e.EventHandlingError;
        }

        #endregion

        private NativeCallback_BeforeCreateInstance m_BeforeCreateInstance;
        private NativeCallback_CreateInstance m_CreateInstance;
        private NativeCallback_DestroyInstance m_DestroyInstance;
        private NativeCallback_Lock m_Lock;
        private NativeCallback_Unlock m_Unlock;
        private NativeCallback_ServerReleased m_ServerReleased;
        private NativeCallback_WriteItems m_WriteItems;
        private NativeCallback_ReadItems m_ReadItems;
        private NativeCallback_DataUpdate m_DataUpdate;
        private NativeCallback_ActivateItems m_ActivateItems;
        private NativeCallback_DeactivateItems m_DeactivateItems;
        private NativeCallback_GetErrorString m_GetErrorString;
        private NativeCallback_QueryLocales m_QueryLocales;
        private NativeCallback_BrowseAccessPaths m_BrowseAccessPaths;
        private NativeCallback_QueryItem m_QueryItem;
        private NativeCallback_ReadProperties m_ReadProperties;

        private void Construct(bool threadSafeAdvise)
        {
            m_BeforeCreateInstance = new NativeCallback_BeforeCreateInstance(OnBeforeCreateInstanceCallback);
            m_CreateInstance = new NativeCallback_CreateInstance(OnCreateInstanceCallback);
            m_DestroyInstance = new NativeCallback_DestroyInstance(OnDestroyInstanceCallback);
            m_Lock = new NativeCallback_Lock(OnLockCallback);
            m_Unlock = new NativeCallback_Unlock(OnUnlockCallback);
            m_ServerReleased = new NativeCallback_ServerReleased(OnServerReleasedCallback);
            m_WriteItems = new NativeCallback_WriteItems(OnWriteItemsCallback);
            m_ReadItems = new NativeCallback_ReadItems(OnReadItemsCallback);
            m_DataUpdate = new NativeCallback_DataUpdate(OnDataUpdateCallback);
            m_ActivateItems = new NativeCallback_ActivateItems(OnActivateItemsCallback);
            m_DeactivateItems = new NativeCallback_DeactivateItems(OnDeactivateItemsCallback);
            m_GetErrorString = new NativeCallback_GetErrorString(OnGetErrorStringCallback);
            m_QueryLocales = new NativeCallback_QueryLocales(OnQueryLocalesCallback);
            m_BrowseAccessPaths = new NativeCallback_BrowseAccessPaths(OnBrowseAccessPathsCallback);
            m_QueryItem = new NativeCallback_QueryItem(OnQueryItemCallback);
            m_ReadProperties = new NativeCallback_ReadProperties(OnReadPropertiesCallback);
            
            m_NativeHandle = Gbdaflat.GBDataAccess_Constructor(
                threadSafeAdvise,
                Marshal.GetFunctionPointerForDelegate(m_BeforeCreateInstance),
                Marshal.GetFunctionPointerForDelegate(m_CreateInstance),
                Marshal.GetFunctionPointerForDelegate(m_DestroyInstance),
                Marshal.GetFunctionPointerForDelegate(m_Lock),
                Marshal.GetFunctionPointerForDelegate(m_Unlock),
                Marshal.GetFunctionPointerForDelegate(m_ServerReleased),
                Marshal.GetFunctionPointerForDelegate(m_WriteItems),
                Marshal.GetFunctionPointerForDelegate(m_ReadItems),
                Marshal.GetFunctionPointerForDelegate(m_DataUpdate),
                Marshal.GetFunctionPointerForDelegate(m_ActivateItems),
                Marshal.GetFunctionPointerForDelegate(m_DeactivateItems),
                Marshal.GetFunctionPointerForDelegate(m_GetErrorString),
                Marshal.GetFunctionPointerForDelegate(m_QueryLocales),
                Marshal.GetFunctionPointerForDelegate(m_BrowseAccessPaths),
                Marshal.GetFunctionPointerForDelegate(m_QueryItem),
                Marshal.GetFunctionPointerForDelegate(m_ReadProperties)
                );
        }

        /// <summary>
        /// <para>Initializes a new instance of the OPCDAServer class.</para>
        /// <para>Calls the constructor of the GBDataAccess native class.</para>
        /// </summary>
        public OPCDAServer()
        {
            Construct(true);
            m_Events = new OPCDAServerEvents(m_NativeHandle, this);
            m_Events.CreateInstance += new CreateInstanceEventHandler(m_Events_CreateInstance);
            m_Events.DestroyInstance += new DestroyInstanceEventHandler(m_Events_DestroyInstance);
        }

        /// <summary>
        /// Dummy handler, used to prevent the CoReleaseServerProcess function 
        /// from being called by the Toolkit default handler.
        /// </summary>
        void m_Events_DestroyInstance(object sender, DestroyInstanceArgs e)
        {
        }

        /// <summary>
        /// Dummy handler, used to prevent the CoAddRefServerProcess function 
        /// from being called by the Toolkit default handler.
        /// </summary>
        private void m_Events_CreateInstance(object sender, CreateInstanceArgs e)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the OPCDAServer class.</para>
        /// <para>Calls the constructor of the GBDataAccess native class.</para>
        /// </summary>
        /// <param name="threadSafeAdvise">If true then you can add and remove the event handlers at any time.
        /// If false then you should add the event handlers before calling the OPCDAServer.Initialize method
        /// and not add or remove the event handlers later. The second case is a little faster.</param>
        public OPCDAServer(bool threadSafeAdvise)
        {
            Construct(threadSafeAdvise);
        }

		/// <summary>
        /// Releases the unmanaged resources used by the OPCDAServer, calling the destructor of the
        /// GBDataAccess native class.
		/// </summary>
        ~OPCDAServer()
        {
            Gbdaflat.GBDataAccess_Destructor(m_NativeHandle);
        }

        #region GBDataAccess methods ----------------------------------------------------------------------------

        #region Initialize
        /// <summary>
        /// <para>Initializes the OPC Server and the Toolkit library. A wrapper for GBDataAccess::GBInitialize method.</para>
        /// <para>Initialize must be called prior to any other OPCDAServer member functions.</para>
        /// <para>Initialize calls ThrowExceptionForHR with the resulting HRESULT returned by
        /// GBDataAccess::GBInitialize to throw an exception if Initialize method fails.</para>
        /// </summary>
        /// <param name="classId">The CLSID of the COM object representing the OPC server.</param>
        /// <param name="timeBase">The period with which the OPC server cache is scanned for the changes.
        /// Any OPC group updaterate will be revised to be a multiple of timeBase.</param>
        /// <param name="minUpdateRate">The minimal update rate, in milliseconds, allowed for OPC groups.
        /// The revised updaterate of any OPC group will be the minimal timeBase multiple which is less than minUpdateRate.</param>
        /// <param name="flags">Flags affecting the OPC server functionality.</param>
        /// <param name="branchSeparator">Branch separator for tag names in OPC Server address space hierarchy. 
        /// It must be an ANSI symbol.</param>
        /// <param name="maxTags">The maximal tags count which can be created in the OPC server address space.
        /// Attempt to specify a number greater than the supported tags maximum of your Toolkit version will
        /// result in the exception.</param>
        /// <param name="versionMajor">The major version of the OPC server, which is reported to the OPC clients.</param>
        /// <param name="versionMinor">The minor version of the OPC server, which is reported to the OPC clients.</param>
        /// <param name="versionBuild">The build number of the OPC server, which is reported to the OPC clients.</param>
        /// <param name="vendorName">A vendor specific string providing additional information about the OPC server.</param>
        public void Initialize(
            Guid classId,
            Int32 timeBase,
            Int32 minUpdateRate,
            ServerOptions flags,
            Char branchSeparator,
            Int32 maxTags,
            Int32 versionMajor,
            Int32 versionMinor,
            Int32 versionBuild,
            String vendorName)
        {
            IntPtr h_guid = Helpers.GuidToHglobal(classId);
            int hr = Gbdaflat.GBDataAccess_GBInitialize(
                m_NativeHandle,
                h_guid,
                timeBase,
                minUpdateRate,
                (int)flags | (int)NativeConstants.GB_SRV_FREEERRSTR,
                branchSeparator,
                maxTags,
                versionMajor,
                versionMinor,
                versionBuild,
                vendorName);
            Marshal.FreeHGlobal(h_guid);
            Marshal.ThrowExceptionForHR(hr);
        }
        /// <summary>
        /// <para>Initializes the OPC Server and the Toolkit library. A wrapper for GBDataAccess::GBInitialize method.</para>
        /// <para>Initialize must be called prior to any other OPCDAServer member functions.</para>
        /// <para>This overload uses the assembly attributes to determine the OPC server version and the vendor name.</para>
        /// <para>Initialize calls ThrowExceptionForHR with the resulting HRESULT returned by
        /// GBDataAccess::GBInitialize to throw an exception if Initialize method fails.</para>
        /// </summary>
        /// <param name="classId">The CLSID of the COM object representing the OPC server.</param>
        /// <param name="timeBase">The period with which the OPC server cache is scanned for the changes.
        /// Any OPC group updaterate will be revised to be a multiple of timeBase.</param>
        /// <param name="minUpdateRate">The minimal update rate, in milliseconds, allowed for OPC groups.
        /// The revised updaterate of any OPC group will be the minimal timeBase multiple which is less than minUpdateRate.</param>
        /// <param name="flags">Flags affecting the OPC server functionality.</param>
        /// <param name="branchSeparator">Branch separator for tag names in OPC Server address space hierarchy. 
        /// It must be an ANSI symbol.</param>
        /// <param name="maxTags">The maximal tags count which can be created in the OPC server address space.
        /// Attempt to specify a number greater than the supported tags maximum of your Toolkit version will
        /// result in the exception.</param>
        public void Initialize(
            Guid classId,
            Int32 timeBase,
            Int32 minUpdateRate,
            ServerOptions flags,
            Char branchSeparator,
            Int32 maxTags)
        {
            Assembly ass = Assembly.GetCallingAssembly();
			Initialize(
				classId,
				timeBase,
				minUpdateRate,
				flags,
				branchSeparator,
				maxTags,
                ass.GetName().Version.Major,
                ass.GetName().Version.Minor,
                ass.GetName().Version.Build,
                ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(ass, typeof(AssemblyCompanyAttribute))).Company	);
        }
        #endregion

        #region RegisterClassObject
        /// <summary>
        /// <para>A wrapper for GBDataAccess::GBRegisterClassObject method.</para>
        /// <para>Registers an OPC server class object with OLE so client applications can connect to it.
        /// Should be called once per OPC server class on the startup.</para>
        /// <para>RegisterClassObject calls ThrowExceptionForHR with the resulting HRESULT returned by
        /// GBDataAccess::GBRegisterClassObject to throw an exception if this method fails.</para>
        /// </summary>
        public void RegisterClassObject()
        {
            Marshal.ThrowExceptionForHR( Gbdaflat.GBDataAccess_GBRegisterClassObject(m_NativeHandle) );
        }
        #endregion

        #region RevokeClassObject
        /// <summary>
        /// <para>A wrapper for GBDataAccess::GBRevokeClassObject method.</para>
        /// <para>Informs OLE that a class object, previously registered with the RegisterClassObject function, is no longer available for use.</para>
        /// <para>RevokeClassObject calls ThrowExceptionForHR with the resulting HRESULT returned by
        /// GBDataAccess::GBRevokeClassObject to throw an exception if this method fails.</para>
        /// </summary>
        public void RevokeClassObject()
        {
            Marshal.ThrowExceptionForHR( Gbdaflat.GBDataAccess_GBRevokeClassObject(m_NativeHandle) );
        }
        #endregion

        #region RegisterServer
        /// <summary>
        /// <para>Registers the OPC server in the system registry. A wrapper for GBDataAccess::GBRegisterServer method.</para>
        /// <para>You can register your OPC server as a regular COM Server or as an Windows NT service.
        /// Note that registering as a service means only the additional LocalService value under the
        /// AppID key. You have to create and register a service with the Service Control Manager by yourself.</para>
        /// </summary>
        /// <para>RegisterServer calls ThrowExceptionForHR with the resulting HRESULT returned by
        /// GBDataAccess::GBRegisterServer to throw an exception if this method fails.</para>
        /// <param name="classId">A CLSID of the OPC server calss object.</param>
        /// <param name="vendorName">A vendor specific string providing additional information about the server.</param>
        /// <param name="description">A description of the server class object.</param>
        /// <param name="versionIndipProgId">A version independent ProgID of the server. For example, 'SomeOrganization.OPCServer'.</param>
        /// <param name="versionCurrent">A current version of the server class object. For example, '1.2'. Version dependent ProgID will represent
        /// the concatination of versionIndipProgId, point and versionCurrent.</param>
        /// <param name="serviceName">Windows NT service name. If null, then the OPC server will be registered as a regular COM server. Otherwise,
        /// the OPC server will be registered as a service.</param>
        public static void RegisterServer(
            Guid classId,
            String vendorName,
            String description,
            String versionIndipProgId,
            String versionCurrent,
            String serviceName)
        {
            IntPtr h_guid = Helpers.GuidToHglobal(classId);
            int hr = Gbdaflat.GBDataAccess_GBRegisterServer(h_guid, vendorName, description, versionIndipProgId, versionCurrent, serviceName);
            Marshal.FreeHGlobal(h_guid);
            Marshal.ThrowExceptionForHR(hr);
        }
        /// <summary>
        /// <para>Registers the OPC server in the system registry. A wrapper for GBDataAccess::GBRegisterServer method.</para>
        /// </summary>
        /// <para>RegisterServer calls ThrowExceptionForHR with the resulting HRESULT returned by
        /// GBDataAccess::GBRegisterServer to throw an exception if this method fails.</para>
        /// <param name="classId">A CLSID of the OPC server calss object.</param>
        /// <param name="vendorName">A vendor specific string providing additional information about the server.</param>
        /// <param name="description">A description of the server class object.</param>
        /// <param name="versionIndipProgId">A version independent ProgID of the server. For example, 'SomeOrganization.OPCServer'.</param>
        /// <param name="versionCurrent">A current version of the server class object. For example, '1.2'. Version dependent ProgID will represent
        /// the concatination of versionIndipProgId, point and versionCurrent.</param>
        public static void RegisterServer(
            Guid classId,
            String vendorName,
            String description,
            String versionIndipProgId,
            String versionCurrent)
        {
            RegisterServer(classId, vendorName, description, versionIndipProgId, versionCurrent, null);
        }
        #endregion

        #region UnregisterServer
        /// <summary>
        /// <para>Removes the previous OPC Server registration from the system registry. 
        /// A wrapper for GBDataAccess::GBUnregisterServer method.</para>
        /// <para>You can register your OPC server as a regular COM Server or as an Windows NT service.
        /// Note that registering as a service means only the additional LocalService value under the
        /// AppID key. You have to create and register a service with the Service Control Manager by yourself.</para>
        /// <para>UnregisterServer calls ThrowExceptionForHR with the resulting HRESULT returned by
        /// GBDataAccess::GBUnregisterServer to throw an exception if this method fails.</para>
        /// </summary>
        /// <param name="classId">A CLSID of the OPC server calss object.</param>
        public static void UnregisterServer(Guid classId)
        {
            IntPtr h_guid = Helpers.GuidToHglobal(classId);
            int hr = Gbdaflat.GBDataAccess_GBUnregisterServer(h_guid);
            Marshal.FreeHGlobal(h_guid);
            Marshal.ThrowExceptionForHR(hr);
        }
        #endregion

        #endregion ----------------------------------------------------------------------------------------------

        #region GBDataAccessBase methods ------------------------------------------------------------------------

        #region GetTags
        /// <summary>
		/// <para>Reads tag values from OPC server cache.
		/// A wrapper for GBDataAccessBase::GBGetItems method.</para>
		/// <para>GetTags calls ThrowExceptionForHR with the resulting HRESULT returned by
		/// GBDataAccessBase::GBGetItems to throw an exception if GetTags method fails.</para>
		/// </summary>
		/// <param name="tagIds">An array of tag identifiers returned earlier by CreateTag method.</param>
		/// <param name="values">A variable which recieves an array of tags values.</param>
		unsafe public void GetTags(
			Int32[] tagIds,
			ref Object[] values)
		{
			int c = tagIds.Length;
            IntPtr unm_v = Marshal.AllocHGlobal((Helpers.SizeOfVariant + 14) * c);
            IntPtr unm_q = new IntPtr(&((byte*)unm_v.ToPointer())[Helpers.SizeOfVariant * c]);
            IntPtr unm_t = new IntPtr(&((byte*)unm_q.ToPointer())[2 * c]);
            IntPtr unm_e = new IntPtr(&((byte*)unm_t.ToPointer())[8 * c]);
            int hr;
            fixed (Int32* unm_i = &tagIds[0])
            {
                hr = Gbdaflat.GBDataAccess_GBGetItems(
                    m_NativeHandle,
                    c,
                    new IntPtr(unm_i),
                    unm_v,
                    unm_q,
                    unm_t,
                    unm_e);
            }
			if (hr != 0)
			{
				Marshal.FreeHGlobal(unm_v);
                values = null;
                Marshal.ThrowExceptionForHR(hr);
                return;
			}
			try
			{
                #if WindowsCE
				values = Helpers.GetObjectsForNativeVariants(unm_v, c);
                #else
				values = Marshal.GetObjectsForNativeVariants(unm_v, c);
                #endif
            }
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
                Gbdaflat.GBDataAccess_GBGetItems_Cleanup(c, unm_v);
			}
		}
        /// <summary>
        /// <para>Reads tag values from OPC server cache.
        /// A wrapper for GBDataAccessBase::GBGetItems method.</para>
        /// <para>GetTags calls ThrowExceptionForHR with the resulting HRESULT returned by
        /// GBDataAccessBase::GBGetItems to throw an exception if GetTags method fails.</para>
        /// </summary>
        /// <param name="tagIds">An array of tag identifiers returned earlier by CreateTag method.</param>
		/// <param name="values">A variable which recieves an array of tags values.</param>
		/// <param name="qualities">A variable which recieves an array of tags qualities.</param>
		/// <param name="timestamps">A variable which recieves an array of tags timestamps.</param>
		/// <param name="errors">A variable which recieves an array of tags errors.</param>
		unsafe public void GetTags(
			Int32[] tagIds,
			ref Object[] values,
			ref Quality[] qualities,
			ref FileTime[] timestamps,
			ref ErrorCodes[] errors)
		{
			int c = tagIds.Length;
            IntPtr unm_v = Marshal.AllocHGlobal((Helpers.SizeOfVariant + 14) * c);
            IntPtr unm_q = new IntPtr(&((byte*)unm_v.ToPointer())[Helpers.SizeOfVariant * c]);
            IntPtr unm_t = new IntPtr(&((byte*)unm_q.ToPointer())[2 * c]);
            IntPtr unm_e = new IntPtr(&((byte*)unm_t.ToPointer())[8 * c]);
            int hr;
            fixed (Int32* unm_i = &tagIds[0])
            {
                hr = Gbdaflat.GBDataAccess_GBGetItems(
                    m_NativeHandle,
                    c,
                    new IntPtr(unm_i),
                    unm_v,
                    unm_q,
                    unm_t,
                    unm_e);
            }
			if (hr != 0)
			{
				Marshal.FreeHGlobal(unm_v);
                values = null;
                qualities = null;
                timestamps = null;
                errors = null;
                Marshal.ThrowExceptionForHR(hr);
                return;
			}
			try
			{
                #if WindowsCE
                values = Helpers.GetObjectsForNativeVariants(unm_v, c);
                #else
				values = Marshal.GetObjectsForNativeVariants(unm_v, c);
                #endif
                qualities = new Quality[c];
                fixed (Quality* p = &qualities[0])
                {
                    for (int i = 0; i < c; i++) p[i] = ((Quality*)unm_q.ToPointer())[i];
                }
                timestamps = new FileTime[c];
                fixed (FileTime* p = &timestamps[0])
                {
                    for (int i = 0; i < c; i++) p[i] = ((FileTime*)unm_t.ToPointer())[i];
                }
                errors = new ErrorCodes[c];
                fixed (ErrorCodes* p = &errors[0])
                {
                    for (int i = 0; i < c; i++) p[i] = ((ErrorCodes*)unm_e.ToPointer())[i];
                }
            }
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
                Gbdaflat.GBDataAccess_GBGetItems_Cleanup(c, unm_v);
			}
		}
		#endregion

		#region UpdateTags
		/// <summary>
		/// <para>Stores new values, qualities, timestamps and errors of tags into the OPC server cache.
		/// A wrapper for GBDataAccessBase::GBUpdateItems method.</para>
		/// <para>UpdateTags calls ThrowExceptionForHR with the resulting HRESULT returned by
		/// GBDataAccessBase::GBUpdateItems to throw an exception if UpdateTags method fails.</para>
		/// </summary>
		/// <param name="tagIds">An array of tag identifiers returned earlier by CreateTag method.</param>
		/// <param name="values">An array containing the new values of tags, specified by tagIds.
		///    Pass null if you want to leave the values of tags unchanged.</param>
		/// <param name="qualites">An array containing the new qualites of tags, specified by tagIds.</param>
		/// <param name="timestamps">An array containing the new timestamps of tags, specified by tagIds. It must be a time in UTC.</param>
		/// <param name="errors">An array containing the new errors of tags, specified by tagIds.</param>
		/// <param name="wait">If true then UpdateTags waits for new data to be placed into the OPC server cache,
		///   otherwise the update is carried out asynchronously.</param>
		unsafe public void UpdateTags(
			Int32[] tagIds,
			Object[] values,
			Quality[] qualites,
			FileTime[] timestamps,
			ErrorCodes[] errors,
			Boolean wait)
		{
			Int32 hr, i, c = tagIds.Length;
            int unm_sizeofValues = Helpers.SizeOfVariant * c;
            IntPtr unm_v = Marshal.AllocHGlobal(unm_sizeofValues);
            if (values != null)
            {
                for (i = 0; i < c; i++)
                    Marshal.GetNativeVariantForObject(values[i], new IntPtr(&((byte*)unm_v.ToPointer())[Helpers.SizeOfVariant * i]));
            }
            else
            {
                // Zero out memory block, because we need VT_EMPTY VARIANT values
                for (i = 0; i < unm_sizeofValues / 4; i++)
                    Marshal.WriteInt32(unm_v, i * 4, 0);
            }
            fixed (Int32* p_tagIds = &tagIds[0])
            {
                fixed (Quality* p_qualites = &qualites[0])
                {
                    fixed (FileTime* p_timestamps = &timestamps[0])
                    {
                        fixed (ErrorCodes* p_errors = &errors[0])
                        {
                            hr = Gbdaflat.GBDataAccess_GBUpdateItems(m_NativeHandle, c,
                                new IntPtr(p_tagIds), unm_v, new IntPtr(p_qualites),
                                new IntPtr(p_timestamps), new IntPtr(p_errors), wait);
                        }
                    }
                }
            }
            Gbdaflat.GBDataAccess_GBGetItems_Cleanup(values == null ? 0 : c, unm_v);
			Marshal.ThrowExceptionForHR(hr);
		}
        /// <summary>
        /// <para>This overload stores new qualities, timestamps and errors of tags into the OPC server cache,
        /// leaving their values unchanged.</para>
        /// <para>A wrapper for GBDataAccessBase::GBUpdateItems method.</para>
        /// </summary>
        /// <param name="tagIds">An array of tag identifiers returned earlier by CreateTag method.</param>
        /// <param name="qualites">An array containing the new qualites of tags, specified by tagIds.</param>
        /// <param name="timestamps">An array containing the new timestamps of tags, specified by tagIds. It must be a time in UTC.</param>
        /// <param name="errors">An array containing the new errors of tags, specified by tagIds.</param>
        /// <param name="wait">If true then UpdateTags waits for new data to be placed into the OPC server cache,
        ///   otherwise the update is carried out asynchronously.</param>
        public void UpdateTags(
			Int32[] tagIds,
			Quality[] qualites,
			FileTime[] timestamps,
			ErrorCodes[] errors,
			Boolean wait)
		{
			UpdateTags(tagIds, null, qualites, timestamps, errors, wait);
		}
        /// <summary>
        /// <para>This overload stores new values, qualities, timestamps and errors of tags into the OPC server cache.
        /// All the tags are updated with the same quality, timestamp and error.</para>
        /// <para>A wrapper for GBDataAccessBase::GBUpdateItems method.</para>
        /// </summary>
        /// <param name="tagIds">An array of tag identifiers returned earlier by CreateTag method.</param>
        /// <param name="values">An array containing the new values of tags, specified by tagIds.
        ///    Pass null if you want to leave the values of tags unchanged.</param>
        /// <param name="quality">A new quality for all tags, specified by tagIds.</param>
        /// <param name="timestamp">A new timestamp for all tags, specified by tagIds. It must be a time in UTC.</param>
        /// <param name="error">A new status error of tags, specified by tagIds.</param>
        /// <param name="wait">If true then UpdateTags waits for new data to be placed into the OPC server cache,
        ///   otherwise the update is carried out asynchronously.</param>
        public void UpdateTags(
            Int32[] tagIds,
            Object[] values,
            Quality quality,
            FileTime timestamp,
            ErrorCodes error,
            Boolean wait)
        {
            int c = tagIds.Length;
			Quality[] qualites = new Quality[c];
			FileTime[] timestamps = new FileTime[c];
            ErrorCodes[] errors = new ErrorCodes[c];
            for (int i = 0; i < c; i++)
            {
                qualites[i] = quality;
                timestamps[i] = timestamp;
                errors[i] = error;
            }
            UpdateTags(tagIds, values, qualites, timestamps, errors, wait);
        }
        /// <summary>
        /// <para>This overload stores new qualities, timestamps and errors of tags into the OPC server cache,
        /// leaving their values unchanged.
        /// All the tags are updated with the same quality, timestamp and error.</para>
        /// <para>A wrapper for GBDataAccessBase::GBUpdateItems method.</para>
        /// </summary>
        /// <param name="tagIds">An array of tag identifiers returned earlier by CreateTag method.</param>
        /// <param name="quality">A new quality for all tags, specified by tagIds.</param>
        /// <param name="timestamp">A new timestamp for all tags, specified by tagIds. It must be a time in UTC.</param>
        /// <param name="error">A new status error of tags, specified by tagIds.</param>
        /// <param name="wait">If true then UpdateTags waits for new data to be placed into the OPC server cache,
        ///   otherwise the update is carried out asynchronously.</param>
        public void UpdateTags(
            Int32[] tagIds,
            Quality quality,
            FileTime timestamp,
            ErrorCodes error,
            Boolean wait)
        {
            UpdateTags(tagIds, null, quality, timestamp, error, wait);
        }
        /// <summary>
        /// <para>This overload stores new values, qualities, timestamps and errors of tags into the OPC server cache.
        /// For each tag quality is set to Good, timestamp is set to the current time and error is set to Ok.</para>
        /// <para>A wrapper for GBDataAccessBase::GBUpdateItems method.</para>
        /// </summary>
        /// <param name="tagIds">An array of tag identifiers returned earlier by CreateTag method.</param>
        /// <param name="values">An array containing the new values of tags, specified by tagIds.
        ///    Pass null if you want to leave the values of tags unchanged.</param>
        /// <param name="wait">If true then UpdateTags waits for new data to be placed into the OPC server cache,
        ///   otherwise the update is carried out asynchronously.</param>
        public void UpdateTags(
            Int32[] tagIds,
            Object[] values,
            Boolean wait)
        {
            UpdateTags(tagIds, values, Quality.Good, FileTime.UtcNow, ErrorCodes.Ok, wait);
        }
        #endregion

        #region BeginUpdate
        /// <summary>
        /// <para>Starts the transaction of OPC server secondary cache update.
        /// A wrapper for the GBDataAccessBase::BeginUpdate method.</para>
        /// <para>This method must be called prior to calling SetTag.
        /// Each call to BeginUpdate must correspond to a single call to EndUpdate.</para>
        /// <para>BeginUpdate calls ThrowExceptionForHR with the resulting HRESULT returned by
        /// GBDataAccessBase::BeginUpdate to throw an exception if BeginUpdate method fails.</para>
        /// </summary>
        public void BeginUpdate()
		{
			Marshal.ThrowExceptionForHR( Gbdaflat.GBDataAccess_GBBeginUpdate(m_NativeHandle) );
		}
        #endregion

        #region SetTag
		/// <summary>
		/// <para>Sets a new value, quality, timestamp and error of a single tag.
		/// A wrapper for GBDataAccessBase::GBSetItem method.</para>
		/// <para>This method can be used only between BeginUpdate and EndUpdate methods.
		/// There may be arbitrary number of SetTag calls inside one BeginUpdate / EndUpdate pair.</para>
		/// <para>SetTag calls ThrowExceptionForHR with the resulting HRESULT returned by
		/// GBDataAccessBase::GBSetItem to throw an exception if SetTag method fails.</para>
		/// </summary>
		/// <param name="tagId">TagID identifier of a tag, returned earlier by CreateTag.</param>
		/// <param name="value">A new value of a tag. Pass null to leave the value of a tag unchanged.</param>
		/// <param name="quality">A new quality of a tag.</param>
		/// <param name="timestamp">A new timestamp of a tag. It must be a time in UTC.</param>
		/// <param name="error">A new error of a tag.</param>
		public void SetTag(
			Int32 tagId,
			Object value,
			Quality quality,
			FileTime timestamp,
			ErrorCodes error)
		{
            int hr = Gbdaflat.GBDataAccess_GBSetItem(
                 m_NativeHandle,
                 tagId,
                 ref value,
                 (Int16)quality.Code,
                 ref timestamp,
                 (Int32)error);
            Marshal.ThrowExceptionForHR(hr);
		}
		/// <summary>
		/// <para>Sets a new value, quality and timestamp of a single tag.
        /// This overload sets the tag error to ErrorCodes.Ok.</para>
		/// <para>A wrapper for GBDataAccessBase::GBSetItem method.</para>
		/// </summary>
		/// <param name="tagId">TagID identifier of a tag, returned earlier by CreateTag.</param>
		/// <param name="value">A new value of a tag. Pass null to leave the value of a tag unchanged.</param>
		/// <param name="quality">A new quality of a tag.</param>
		/// <param name="timestamp">A new timestamp of a tag. It must be a time in UTC.</param>
		public void SetTag(
			Int32 tagId,
			Object value,
			Quality quality,
			FileTime timestamp)
		{
            SetTag(tagId, value, quality, timestamp, ErrorCodes.Ok);
		}
        /// <summary>
        /// <para>Sets a new value of a single tag.
        /// This overload sets the tag error to ErrorCodes.Ok, the tag quality to Quality.Good,
        /// the tag timestamp to the current time, the tag error to ErrorCodes.Ok.</para>
        /// <para>A wrapper for GBDataAccessBase::GBSetItem method.</para>
        /// </summary>
		/// <param name="tagId">TagID identifier of a tag, returned earlier by CreateTag.</param>
		/// <param name="value">A new value of a tag. Pass null to leave the value of a tag unchanged.</param>
		public void SetTag(
			Int32 tagId,
			Object value)
		{
			SetTag(tagId, value, Quality.Good, FileTime.UtcNow, ErrorCodes.Ok);
		}
        /// <summary>
        /// <para>Sets a new quality, timestamp and error of a single tag.
        /// This overload leaves the tag data value unchanged.</para>
        /// <para>A wrapper for GBDataAccessBase::GBSetItem method.</para>
        /// </summary>
		/// <param name="tagId">TagID identifier of a tag, returned earlier by CreateTag.</param>
		/// <param name="quality">A new quality of a tag.</param>
		/// <param name="timestamp">A new timestamp of a tag. It must be a time in UTC.</param>
		/// <param name="error">A new error of a tag.</param>
		public void SetTag(
			Int32 tagId,
			Quality quality,
			FileTime timestamp,
			ErrorCodes error)
		{
			SetTag(tagId, null, quality, timestamp, error);
		}
        /// <summary>
        /// <para>Sets a new quality and timestamp of a single tag.
        /// This overload leaves the tag data value unchanged and sets the tag error to ErrorCodes.Ok.</para>
        /// <para>A wrapper for GBDataAccessBase::GBSetItem method.</para>
        /// </summary>
        /// <param name="tagId">TagID identifier of a tag, returned earlier by CreateTag.</param>
		/// <param name="quality">A new quality of a tag.</param>
		/// <param name="timestamp">A new timestamp of a tag. It must be a time in UTC.</param>
		public void SetTag(
			Int32 tagId,
			Quality quality,
			FileTime timestamp)
		{
			SetTag(tagId, null, quality, timestamp, ErrorCodes.Ok);
		}
        /// <summary>
        /// <para>Sets a new quality of a single tag.
        /// This overload leaves the tag data value unchanged, sets the tag error to ErrorCodes.Ok,
        /// sets the tag timestamp to the current time.</para>
        /// <para>A wrapper for GBDataAccessBase::GBSetItem method.</para>
        /// </summary>
		/// <param name="tagId">TagID identifier of a tag, returned earlier by CreateTag.</param>
		/// <param name="quality">A new quality of a tag.</param>
        public void SetTag(
			Int32 tagId,
			Quality quality)
		{
			SetTag(tagId, null, quality, FileTime.UtcNow, ErrorCodes.Ok);
		}
        /// <summary>
        /// <para>Sets a new timestamp of a single tag.
        /// This overload leaves the tag data value unchanged, sets the tag error to ErrorCodes.Ok,
        /// sets the tag quality to Quality.Good.</para>
        /// <para>A wrapper for GBDataAccessBase::GBSetItem method.</para>
        /// </summary>
        /// <param name="tagId">TagID identifier of a tag, returned earlier by CreateTag.</param>
        /// <param name="timestamp">A new timestamp of a tag. It must be a UTC time.</param>
        public void SetTag(
            Int32 tagId,
            FileTime timestamp)
        {
            SetTag(tagId, null, Quality.Good, timestamp, ErrorCodes.Ok);
        }
        #endregion

        #region EndUpdate
        /// <summary>
        /// <para>Ends the transaction of OPC server secondary cache update, which was started
        /// earlier with BeginUpdate.
        /// A wrapper for the GBDataAccessBase::GBEndUpdate method.</para>
        /// <para>UpdateTags calls ThrowExceptionForHR with the resulting HRESULT returned by the
        /// GBDataAccessBase::GBEndUpdate to throw an exception if the EndUpdate method fails.</para>
        /// </summary>
        /// <param name="wait">If true then EndUpdate waits for the update transaction to complete
        /// (new data is stored into the OPC server cache),
        /// otherwise the update transaction is carried out asynchronously.</param>
        public void EndUpdate(Boolean wait)
		{
            Marshal.ThrowExceptionForHR(Gbdaflat.GBDataAccess_GBEndUpdate(m_NativeHandle, wait));
        }
        #endregion

        #region CreateTag
        /// <summary>
		/// <para>Creates a new OPC server tag.
		/// A wrapper for GBDataAccessBase::GBCreateItem, GBDataAccessBase::GBCreateItemAnalog or
		/// GBDataAccessBase::GBCreateItemEnum method depending on which overload is called.</para>
		/// <para>UpdateTags calls ThrowExceptionForHR with the resulting HRESULT returned by
		/// the underlying GBDataAccessBase method to throw an exception if CreateTag method fails.</para>
		/// </summary>
		/// <returns>The TagId identifier of the created tag.</returns>
		/// <param name="userId">Arbitrary UserId tag identifier. This value will be passed to the Toolkit event handlers. Its meaning is left for your consideration.</param>
		/// <param name="opcItemId">A fully qualified tag name.</param>
		/// <param name="accessRights">Indicates if this tag is read only, write only or read/write.</param>
		/// <param name="tagOptions">Flags. A combination of bits used to modify the way the Toolkit will proccess this tag.</param>
		/// <param name="euType">Indicates the type of Engineering Units (EU) information (if any) contained in euInfo.</param>
		/// <param name="euInfo">EU information. Its value depends on euType.
		///   If euType is EUType.noEnum then euInfo must be null.
		///   If euType is EUType.analog then euInfo must contain an array of two doubles indicating LOW and HI EU range.
		///   If euType is EUType.enumerated then euInfo must contain an array of strings which describe every possible tag value
		///   (first string in euType corresponds to a zero value, second corresponds to 1 and so on).</param>
        /// <param name="defaultValue">An Object containing the tags initial data value in its canonical data type.</param>
        public Int32 CreateTag(
			Int32 userId,
			String opcItemId,
			AccessRights accessRights,
			TagOptions tagOptions,
			EUType euType,
			Object euInfo,
            Object defaultValue)
        {
            Int32 tagId;
            int hr = Gbdaflat.GBDataAccess_GBCreateItem(
                m_NativeHandle,
                out tagId,
                userId,
                opcItemId,
                (int)accessRights,
                (int)tagOptions,
                ref defaultValue,
                (int)euType,
                ref euInfo);
            Marshal.ThrowExceptionForHR(hr);
            return tagId;
        }
        /// <summary>
		/// <para>Creates a new OPC tag without engineering units.
        /// This overload sets the tag options to TagOptions.Default.</para>
		/// <para>A wrapper for GBDataAccessBase::GBCreateItem.</para>
		/// </summary>
		/// <returns>The TagId identifier of the created tag.</returns>
		/// <param name="userId">Arbitrary UserId tag identifier. This value will be passed to the Toolkit event handlers. Its meaning is left for your consideration.</param>
		/// <param name="opcItemId">A fully qualified tag name.</param>
		/// <param name="accessRights">Indicates if this tag is read only, write only or read/write.</param>
		/// <param name="defaultValue">An Object containing the tags initial data value in its canonical data type.</param>
		public Int32 CreateTag(
			Int32 userId,
			String opcItemId,
			AccessRights accessRights,
			Object defaultValue)
		{
			return CreateTag(userId, opcItemId, accessRights, TagOptions.Default,
				EUType.noEnum, null, defaultValue);
		}
        /// <summary>
		/// <para>Creates a new OPC tag without engineering units.</para>
		/// <para>A wrapper for GBDataAccessBase::GBCreateItem.</para>
		/// </summary>
		/// <returns>The TagId identifier of the created tag.</returns>
		/// <param name="userId">Arbitrary UserId tag identifier. This value will be passed to the Toolkit event handlers. Its meaning is left for your consideration.</param>
		/// <param name="opcItemId">A fully qualified tag name.</param>
		/// <param name="accessRights">Indicates if this tag is read only, write only or read/write.</param>
		/// <param name="tagOptions">Flags. A combination of bits used to modify the way the Toolkit will proccess this tag.</param>
		/// <param name="defaultValue">An Object containing the tags initial data value in its canonical data type.</param>
		public Int32 CreateTag(
			Int32 userId,
			String opcItemId,
			AccessRights accessRights,
			TagOptions tagOptions,
			Object defaultValue)
		{
			return CreateTag(userId, opcItemId, accessRights, tagOptions,
                EUType.noEnum, null, defaultValue);
		}
        /// <summary>
		/// <para>Creates a new OPC tag with analog engineering units.</para>
		/// <para>A wrapper for GBDataAccessBase::GBCreateItem.</para>
		/// </summary>
		/// <returns>The TagId identifier of the created tag.</returns>
		/// <param name="userId">Arbitrary UserId tag identifier. This value will be passed to the Toolkit event handlers. Its meaning is left for your consideration.</param>
		/// <param name="opcItemId">A fully qualified tag name.</param>
		/// <param name="accessRights">Indicates if this tag is read only, write only or read/write.</param>
		/// <param name="tagOptions">Flags. A combination of bits used to modify the way the Toolkit will proccess this tag.</param>
		/// <param name="loLimit">Specifies the LO EU Range.</param>
		/// <param name="hiLimit">Specifies the HI EU Range.</param>
        /// <param name="defaultValue">An Object containing the tags initial data value in its canonical data type.</param>
        public Int32 CreateTag(
			Int32 userId,
			String opcItemId,
			AccessRights accessRights,
			TagOptions tagOptions,
			Double loLimit,
			Double hiLimit,
            Object defaultValue)
		{
			return CreateTag(userId, opcItemId, accessRights, tagOptions,
                EUType.analog, new Double[] { loLimit, hiLimit }, defaultValue);
		}
        /// <summary>
		/// <para>Creates a new OPC tag with analog engineering units.
        /// This overload sets the tag options to TagOptions.Default.</para>
		/// <para>A wrapper for GBDataAccessBase::GBCreateItem.</para>
		/// </summary>
		/// <returns>The TagId identifier of the created tag.</returns>
		/// <param name="userId">Arbitrary UserId tag identifier. This value will be passed to the Toolkit event handlers. Its meaning is left for your consideration.</param>
		/// <param name="opcItemId">A fully qualified tag name.</param>
		/// <param name="accessRights">Indicates if this tag is read only, write only or read/write.</param>
		/// <param name="loLimit">Specifies the LO EU Range.</param>
		/// <param name="hiLimit">Specifies the HI EU Range.</param>
        /// <param name="defaultValue">An Object containing the tags initial data value in its canonical data type.</param>
        public Int32 CreateTag(
			Int32 userId,
			String opcItemId,
			AccessRights accessRights,
			Double loLimit,
			Double hiLimit,
            Object defaultValue)
		{
            return CreateTag(userId, opcItemId, accessRights, TagOptions.Default,
                EUType.analog, new Double[] { loLimit, hiLimit }, defaultValue);
		}
        /// <summary>
		/// <para>Creates a new OPC tag with enumerated engineering units.</para>
		/// <para>A wrapper for GBDataAccessBase::GBCreateItem.</para>
		/// </summary>
		/// <returns>The TagId identifier of the created tag.</returns>
		/// <param name="userId">Arbitrary UserId tag identifier. This value will be passed to the Toolkit event handlers. Its meaning is left for your consideration.</param>
		/// <param name="opcItemId">A fully qualified tag name.</param>
		/// <param name="accessRights">Indicates if this tag is read only, write only or read/write.</param>
		/// <param name="tagOptions">Flags. A combination of bits used to modify the way the Toolkit will proccess this tag.</param>
		/// <param name="stateNames">An array of strings which describe every possible enumerable tag value.
		///   First string in stateNames corresponds to a zero value, second corresponds to 1 and so on.</param>
        /// <param name="defaultValue">An Object containing the tags initial data value in its canonical data type.</param>
        public Int32 CreateTag(
			Int32 userId,
			String opcItemId,
			AccessRights accessRights,
			TagOptions tagOptions,
            String[] stateNames,
			Object defaultValue)
		{
			return CreateTag(userId, opcItemId, accessRights, tagOptions,
                EUType.enumerated, stateNames, defaultValue);
		}
        /// <summary>
		/// <para>Creates a new OPC tag with enumerated engineering units.
        /// This overload sets the tag options to TagOptions.Default.</para>
		/// <para>A wrapper for GBDataAccessBase::GBCreateItem.</para>
		/// </summary>
		/// <returns>The TagId identifier of the created tag.</returns>
		/// <param name="userId">Arbitrary UserId tag identifier. This value will be passed to the Toolkit event handlers. Its meaning is left for your consideration.</param>
		/// <param name="opcItemId">A fully qualified tag name.</param>
		/// <param name="accessRights">Indicates if this tag is read only, write only or read/write.</param>
		/// <param name="stateNames">An array of strings which describe every possible enumerable tag value.
		///   First string in stateNames corresponds to a zero value, second corresponds to 1 and so on.</param>
        /// <param name="defaultValue">An Object containing the tags initial data value in its canonical data type.</param>
        public Int32 CreateTag(
			Int32 userId,
			String opcItemId,
			AccessRights accessRights,
			String[] stateNames,
            Object defaultValue)
		{
			return CreateTag(userId, opcItemId, accessRights, TagOptions.Default,
                EUType.enumerated, stateNames, defaultValue);
		}
        #endregion

		#region AddProperty
		/// <summary>
        /// <para>Creates a new OPC property. It can be a standard property or a custom property.</para>
        /// <para>A wrapper for GBDataAccessBase::GBAddProperty method.</para>
		/// <para>Properties with PropID form 1 to 8 are created automatically during tag creation.
		/// Property 8 is created only for tags with Engineering Units defined.</para>
		/// <para>AddProperty calls ThrowExceptionForHR with the resulting HRESULT returned by
		/// the underlying GBDataAccessBase method to throw an exception if AddProperty method fails.</para>
		/// </summary>
		/// <param name="tagId">The TagId identifier of a tag for which to add the property. This is a TagId returned by CreateTag.</param>
		/// <param name="propId">The PropID identifier of a property to add. See OPC DA Specification for details.</param>
		/// <param name="defaultValue">An initial property value. Its type specifies the data type of a property.</param>
		/// <param name="description">A textual description of a property. If a description with the same PropID was added to some tag earlier, Toolkit will already have its description and you may pass null. Definitions of the standard properties (properties listed in the OPC DA Spec) are always known to the Toolkit.</param>
		/// <param name="itemId">A fully qualified ItemID that can be used to access this property. If null is passed, then the property can not be accessed via an ItemID. If you are passing non-null ItemID, then it must be the ItemID of an already created tag.</param>
		unsafe public void AddProperty(
			Int32 tagId,
			Int32 propId,
			Object defaultValue,
			String description,
			String itemId)
		{
			int hr = Gbdaflat.GBDataAccess_GBAddProperty(
                m_NativeHandle,
				tagId,
				propId,
                ref defaultValue,
				description,
				itemId,
				0);
			Marshal.ThrowExceptionForHR(hr);
		}
        /// <summary>
        /// <para>Creates a new OPC property. It can be a standard property or a custom property.</para>
        /// <para>A wrapper for GBDataAccessBase::GBAddProperty method.</para>
        /// </summary>
        /// <param name="tagId">The TagId identifier of a tag for which to add the property. This is a TagId returned by CreateTag.</param>
		/// <param name="propId">The PropID identifier of a property to add. See OPC DA Specification for details.</param>
		/// <param name="defaultValue">An initial property value. Its type specifies the data type of a property.</param>
		/// <param name="description">A textual description of a property. If a description with the same PropID was added to some tag earlier, Toolkit will already have its description and you may pass null. Definitions of standard properties (properties listed in the OPC DA Spec) are always known to the Toolkit.</param>
		public void AddProperty(
			Int32 tagId,
			Int32 propId,
			Object defaultValue,
			String description)
		{
			AddProperty(tagId, propId, defaultValue, description, null);
		}
        /// <summary>
        /// <para>Creates a new OPC property. It can be a standard property or a custom property.</para>
        /// <para>A wrapper for GBDataAccessBase::GBAddProperty method.</para>
        /// </summary>
        /// <param name="tagId">The TagId identifier of a tag for which to add the property. This is a TagId returned by CreateTag.</param>
		/// <param name="propId">The PropID identifier of a property to add. See OPC DA Specification for details.</param>
		/// <param name="defaultValue">An initial property value. Its type specifies the data type of a property.</param>
        public void AddProperty(
			Int32 tagId,
			Int32 propId,
			Object defaultValue)
		{
			AddProperty(tagId, propId, defaultValue, null, null);
		}
        /// <summary>
        /// <para>Creates a new OPC property. It can be a standard property or a custom property.</para>
        /// <para>A wrapper for GBDataAccessBase::GBAddProperty method.</para>
        /// </summary>
        /// <param name="tagId">The TagId identifier of a tag for which to add the property. This is a TagId returned by CreateTag.</param>
		/// <param name="propId">The PropID identifier of a property to add. See OPC DA Specification for details.</param>
		/// <param name="defaultValue">An initial property value. Its type specifies the data type of a property.</param>
        public void AddProperty(
			Int32 tagId,
			StandardProperties propId,
			Object defaultValue)
		{
			AddProperty(tagId, (int)propId, defaultValue, null, null);
		}
        /// <summary>
        /// <para>Creates a new OPC property. It can be a standard property or a custom property.</para>
        /// <para>A wrapper for GBDataAccessBase::GBAddProperty method.</para>
        /// </summary>
        /// <param name="tagId">A TagId identifier of a tag for which to add the property. This is a TagId returned by CreateTag.</param>
		/// <param name="propId">A PropID identifier of a property to add. See OPC DA Specification for details.</param>
		/// <param name="defaultValue">An initial property value. Its type specifies the data type of a property.</param>
		/// <param name="itemId">A fully qualified ItemID that can be used to access this property. If null is passed, then the property can not be accessed via an ItemID. If you are passing non-null ItemID, then it must be the ItemID of an already created tag.</param>
        public void AddProperty(
			Int32 tagId,
			StandardProperties propId,
			Object defaultValue,
			String itemId)
		{
			AddProperty(tagId, (int)propId, defaultValue, null, itemId);
		}
		#endregion
    
		#region SetProperty
		/// <summary>
		/// <para>Sets the new value of a tag property.
		/// A wrapper for GBDataAccessBase::GBSetProperty method.</para>
		/// <para>If property is associated with another tag, then value of this tag will not be changed.</para>
		/// <para>SetProperty calls ThrowExceptionForHR with the resulting HRESULT returned by
		/// GBDataAccessBase::GBSetProperty to throw an exception if SetProperty method fails.</para>
		/// </summary>
		/// <param name="tagId">A TagId identifier of a tag whose property is being set. This is a TagId returned by CreateTag.</param>
		/// <param name="propId">A PropID identifier of a property to set.</param>
		/// <param name="value">A new value of a property.</param>
		public void SetProperty(
			Int32 tagId,
			Int32 propId,
			Object value)
		{
			Marshal.ThrowExceptionForHR(Gbdaflat.GBDataAccess_GBSetProperty(
                m_NativeHandle, tagId, propId, ref value));
		}

        /// <summary>
        /// <para>Sets the new value of a tag property.
        /// A wrapper for GBDataAccessBase::GBSetProperty method.</para>
        /// <para>If property is associated with another tag, then value of this tag will not be changed.</para>
        /// <para>SetProperty calls ThrowExceptionForHR with the resulting HRESULT returned by
        /// GBDataAccessBase::GBSetProperty to throw an exception if SetProperty method fails.</para>
        /// </summary>
        /// <param name="tagId">A TagId identifier of a tag whose property is being set. This is a TagId returned by CreateTag.</param>
        /// <param name="propId">A PropID identifier of a property to set.</param>
        /// <param name="value">A new value of a property.</param>
        public void SetProperty(
            Int32 tagId,
            StandardProperties propId,
            Object value)
        {
            Marshal.ThrowExceptionForHR(Gbdaflat.GBDataAccess_GBSetProperty(
                m_NativeHandle, tagId, (Int32)propId, ref value));
        }
        #endregion
 
		#region GetProperty
		/// <summary>
		/// <para>Gets the current value of a tag property.
		/// A wrapper for GBDataAccessBase::GBGetProperty method.</para>
		/// <para>If the property is associated with another tag, then no reading of this tag value will be done.</para>
		/// <para>GetProperty calls ThrowExceptionForHR with the resulting HRESULT returned by
		/// GBDataAccessBase::GBGetProperty to throw an exception if GetProperty method fails.</para>
		/// </summary>
		/// <returns>The current value of the specified tag property.</returns>
		/// <param name="tagId">A TagId identifier of a tag whose property is to be read. This is a TagId returned by CreateTag.</param>
		/// <param name="propId">A PropID identifier of a property to read.</param>
		public Object GetProperty(Int32 tagId, Int32 propId)
		{
            object val;
            int hr = Gbdaflat.GBDataAccess_GBGetProperty(m_NativeHandle, tagId, propId, out val);
            Marshal.ThrowExceptionForHR(hr);
            return val;
		}
        /// <summary>
        /// <para>Gets the current value of the standard property of a tag.
        /// A wrapper for GBDataAccessBase::GBGetProperty method.</para>
        /// <para>If the property is associated with another tag, then no reading of this tag value will be done.</para>
        /// <para>GetProperty calls ThrowExceptionForHR with the resulting HRESULT returned by
        /// GBDataAccessBase::GBGetProperty to throw an exception if GetProperty method fails.</para>
        /// </summary>
        /// <returns>The current value of the specified tag property.</returns>
        /// <param name="tagId">A TagId identifier of a tag whose property is to be read. This is a TagId returned by CreateTag.</param>
        /// <param name="propId">A PropID identifier of a standard property to read.</param>
        public Object GetProperty(Int32 tagId, StandardProperties propId)
        {
            return GetProperty(tagId, (int)propId);
        }
		#endregion
    
		#region RemoveProperty
		/// <summary>
		/// <para>Removes the specified property of the specified tag.
		/// A wrapper for GBDataAccessBase::GBRemoveProperty method.</para>
		/// <para>RemoveProperty calls ThrowExceptionForHR with the resulting HRESULT returned by
		/// GBDataAccessBase::GBRemoveProperty to throw an exception if RemoveProperty method fails.</para>
		/// </summary>
		/// <param name="tagId">A TagId identifier of a tag whose property is to be removed. This is a TagId returned by CreateTag.</param>
		/// <param name="propId">A PropID identifier of a property to remove.</param>
		public void RemoveProperty(Int32 tagId, Int32 propId)
		{
			Marshal.ThrowExceptionForHR(Gbdaflat.GBDataAccess_GBRemoveProperty(m_NativeHandle, tagId, propId));
		}
		/// <summary>
		/// <para>Removes the specified standard property of the specified tag.
		/// A wrapper for GBDataAccessBase::GBRemoveProperty method.</para>
		/// <para>RemoveProperty calls ThrowExceptionForHR with the resulting HRESULT returned by
		/// GBDataAccessBase::GBRemoveProperty to throw an exception if RemoveProperty method fails.</para>
		/// </summary>
		/// <param name="tagId">A TagId identifier of a tag whose property is to be removed. This is a TagId returned by CreateTag.</param>
		/// <param name="propId">A PropID identifier of a standard property to remove.</param>
		public void RemoveProperty(Int32 tagId, StandardProperties propId)
		{
			Marshal.ThrowExceptionForHR(Gbdaflat.GBDataAccess_GBRemoveProperty(m_NativeHandle, tagId, (int)propId));
		}
		#endregion

		#region GetBandwidth
		/// <summary>
		/// <para>Used to determine the bandwidth of the OPC server. A wrapper for GBDataAccessBase::GBGetBandwidth method.</para>
		/// <para>This method returns the worst bandwidth among all of the OPC server instances.</para>
		/// <para>GetBandwidth returns zero is bandwidth is undefiened</para>
		/// </summary>
		/// <returns>The bandwidth of this OPC server.</returns>
		public Int32 GetBandwidth()
		{
            Int32 hr;
            Int32 b = 0;
			hr = Gbdaflat.GBDataAccess_GBGetBandwidth(m_NativeHandle, ref b);
			if (hr < 0) return 0;
			return b;
		}
		#endregion

		#region SetState
		/// <summary>
		/// <para>Sets the server state. A wrapper for GBDataAccessBase::GBSetState method.</para>
		/// <para>This method doesn't affect the OPC server behavior. It only affects the status which OPC clients will recieve via IOPCServer::GetStatus.</para>
		/// </summary>
		/// <returns>false if error has occurred, otherwise true.</returns>
		/// <param name="state">A new OPC server state.</param>
		public Boolean SetState(ServerState state)
		{
            int hr = Gbdaflat.GBDataAccess_GBSetState(m_NativeHandle, (int)state);
            if (hr >= 0) return true;
            return false;
		}
		#endregion

		#region Suspend
		/// <summary>
		/// <para>Suspends the OPC server. A wrapper for GBDataAccessBase::GBSuspend method.</para>
		/// <para>Sets the server status to OPC_STATUS_SUSPENDED and suspends its functioning.
		/// While in this state, OPC server ignores most of the client calls, doesn't process
		/// data and doesn't send updates (OnDataChange callbacks).</para>
		/// <para>Call Resume to resume the OPC server functions.</para>
		/// </summary>
		/// <returns>false if error has occurred, otherwise true.</returns>
        public Boolean Suspend()
		{
            int hr = Gbdaflat.GBDataAccess_GBSuspend(m_NativeHandle);
            if (hr >= 0) return true;
            return false;
        }
		#endregion

		#region Resume
		/// <summary>
		/// <para>Resumes the OPC server. A wrapper for GBDataAccessBase::GBResume method.</para>
		/// <para>Resumes OPC server, which was previously suspended with Suspend method, and sets it status to OPC_STATUS_RUNNING.</para>
		/// </summary>
		/// <returns>false if error has occurred, otherwise true.</returns>
		public Boolean Resume()
		{
            int hr = Gbdaflat.GBDataAccess_GBResume(m_NativeHandle);
            if (hr >= 0) return true;
            return false;
        }
		#endregion

		#region Shutdown
		/// <summary>
		/// <para>Shuts down the OPC server. A wrapper for GBDataAccessBase::GBShutdown method.</para>
		/// <para>This method will send IOPCShutdown::ShutdownRequest callback to all client advised IOPCShutdown connection point.
		/// Server functioning will stop and it can't be resumed later.</para>
		/// </summary>
		/// <returns>false if error has occurred, otherwise true.</returns>
		/// <param name="reason">An optional text string indicating the reason for the shutdown. Pass null if you don't wish to specify the reason.</param>
		public Boolean Shutdown(String reason)
		{
            int hr = Gbdaflat.GBDataAccess_GBShutdown(m_NativeHandle, reason);
            if (hr >= 0) return true;
            return false;
        }
        /// <summary>
        /// <para>Shuts down the OPC server. A wrapper for GBDataAccessBase::GBShutdown method.</para>
        /// <para>No reason for the shutdown is provided.</para>
        /// </summary>
        /// <returns>false if error has occurred, otherwise true.</returns>
        public Boolean Shutdown()
		{
            return Shutdown(null);
        }
		#endregion

        #endregion
    }
}