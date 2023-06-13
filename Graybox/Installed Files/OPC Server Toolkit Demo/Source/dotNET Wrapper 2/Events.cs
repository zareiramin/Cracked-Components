using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Graybox.OPC.ServerToolkit.CLRWrapper
{
	/// <summary>
	/// Represents a collection of the CultureInfo objects.
    /// This class is used in the QueryLocalesEventArgs class.
	/// </summary>
	public sealed class CulturesList:
		System.Collections.IList,
		System.Collections.ICollection,
		System.Collections.IEnumerable
	{
	    private	ArrayList m_cultures;
		private IntPtr m_unm_cultures;
        /// <summary>
        /// Updates the unmanaged array of the LCIDs with the data from the m_cultures.
        /// </summary>
		private void UpdateUnmanaged()
		{
			if (m_unm_cultures != IntPtr.Zero) Marshal.FreeHGlobal(m_unm_cultures);
			m_unm_cultures = Marshal.AllocHGlobal(4 * (Count + 3));
			Marshal.WriteInt32(m_unm_cultures, 4*0, 0);     // 0 is LOCALE_NEUTRAL
			Marshal.WriteInt32(m_unm_cultures, 4*1, 9);     // 9 is MAKELCID(MAKELANGID(LANG_ENGLISH, SUBLANG_NEUTRAL), SORT_DEFAULT)
			Marshal.WriteInt32(m_unm_cultures, 4*2, 0x800); // 800h is LOCALE_SYSTEM_DEFAULT
			for (Int32 i = 0; i<Count; i++)
			{
				Marshal.WriteInt32(m_unm_cultures, 4*(i + 3), ((CultureInfo)m_cultures[i]).LCID);
			}
		}
        /// <summary>
        /// Initializes a new instance of the <c>CulturesList</c> class.
        /// </summary>
        public CulturesList()
		{
			m_cultures = new ArrayList();
			UpdateUnmanaged();
		}
        /// <summary>
        /// Initializes a new instance of the <c>CulturesList</c> class using the given array of
        /// the <c>CultureInfo</c> objects.
        /// </summary>
        /// <param name="arr"></param>
		public CulturesList(CultureInfo[] arr)
		{
			m_unm_cultures = IntPtr.Zero;
			m_cultures = new ArrayList(arr);
			UpdateUnmanaged();
		}
        /// <summary>
        /// Frees the unmanaged resources.
        /// </summary>
		~CulturesList()
		{
			if (m_unm_cultures != IntPtr.Zero)
				Marshal.FreeHGlobal(m_unm_cultures);
		}
        /// <summary>
        /// Gets the pointer of the unmanaged array that holds the LCIDs of
        /// the locales supported by the OPC server.
        /// </summary>
		public IntPtr Unmanaged
		{
			get { return m_unm_cultures; }
		}
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
		public Object this[int index]
		{
			get { return m_cultures[index]; }
			set
			{
				if (!(value is CultureInfo)) throw new ArgumentException("Value must be an instance of CultureInfo type.", "value");
				m_cultures[index] = value;
				UpdateUnmanaged();
			}
		}
        /// <summary>
        /// Gets the number of elements contained in the <c>ICollection</c>.
        /// </summary>
        public Int32 Count { get { return m_cultures.Count; } }
        /// <summary>
        /// Gets a value indicating whether access to the <c>ICollection</c> is synchronized (thread safe).
        /// </summary>
        public bool IsSynchronized { get { return m_cultures.IsSynchronized; } }
        /// <summary>
        /// Gets an object that can be used to synchronize access to the <c>ICollection</c>. 
        /// </summary>
        public Object SyncRoot { get { return m_cultures.SyncRoot; } }
        /// <summary>
        /// Gets a value indicating whether the <c>IList</c> is read-only.
        /// </summary>
        public bool IsReadOnly { get { return m_cultures.IsReadOnly; } }
        /// <summary>
        /// Gets a value indicating whether the <c>IList</c> has a fixed size.
        /// </summary>
        public bool IsFixedSize { get { return m_cultures.IsFixedSize; } }
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        public System.Collections.IEnumerator GetEnumerator() { return m_cultures.GetEnumerator(); }
        /// <summary>
        /// Copies the elements of the <c>ICollection</c> to an <c>Array</c>, starting at a particular <c>Array</c> index.
        /// </summary>
        public void CopyTo(System.Array arr, Int32 arrIndex) { m_cultures.CopyTo(arr, arrIndex); }
        /// <summary>
        /// Determines whether the <c>IList</c> contains a specific value.
        /// </summary>
        public bool Contains(System.Object item) { return m_cultures.Contains(item); }
        /// <summary>
        /// Removes all items from the <c>IList</c>.
        /// </summary>
        public void Clear()
		{
			m_cultures.Clear();
			UpdateUnmanaged();
		}
        /// <summary>
        /// Adds an item to the <c>IList</c>.
        /// </summary>
        public int Add(Object item)
		{
			int r = m_cultures.Add((CultureInfo)item);
			UpdateUnmanaged();
			return r;
		}
        /// <summary>
        /// Determines the index of a specific item in the <c>IList</c>.
        /// </summary>
        public Int32 IndexOf(System.Object item)
		{
			Int32 r = m_cultures.IndexOf(item);
			UpdateUnmanaged();
			return r;
		}
        /// <summary>
        /// Inserts an item to the <c>IList</c> at the specified index.
        /// </summary>
        public void Insert(Int32 index, System.Object item)
		{
			m_cultures.Insert(index, item);
			UpdateUnmanaged();
		}
        /// <summary>
        /// Removes the first occurrence of a specific object from the <c>IList</c>.
        /// </summary>
        public void Remove(System.Object item)
		{
			m_cultures.Remove(item);
			UpdateUnmanaged();
		}
        /// <summary>
        /// Removes the <c>IList</c> item at the specified index.
        /// </summary>
        public void RemoveAt(Int32 index)
		{
			m_cultures.RemoveAt(index);
			UpdateUnmanaged();
		}
	};

    #region Enums used in events handling
    /// <summary>
    /// Event flags indexes and count declaration. Used internally.
    /// </summary>
    internal enum GBDataAccessEventIdx
    {
        EventIdx_BeforeCreateInstance = 0,
        EventIdx_CreateInstance,
        EventIdx_DestroyInstance,
        EventIdx_Lock,
        EventIdx_Unlock,
        EventIdx_ServerReleased,
        EventIdx_WriteItems,
        EventIdx_ReadItems,
        EventIdx_DataUpdate,
        EventIdx_ActivateItems,
        EventIdx_DeactivateItems,
        EventIdx_GetErrorString,
        EventIdx_QueryLocales,
        EventIdx_BrowseAccessPaths,
        EventIdx_QueryItem,
        EventIdx_ReadProperties,
        EventIdx_Count
    };
    #endregion

    #region Event delegates

    /// <summary>
    /// Represents the method that will handle the <c>BeforeCreateInstance</c> event of an <c>OPCDAServer</c>.
    /// </summary>
    public delegate void BeforeCreateInstanceEventHandler(Object sender, BeforeCreateInstanceArgs e);
    /// <summary>
    /// Represents the method that will handle the <c>CreateInstance</c> event of an <c>OPCDAServer</c>.
    /// </summary>
    public delegate void CreateInstanceEventHandler(Object sender, CreateInstanceArgs e);
    /// <summary>
    /// Represents the method that will handle the <c>DestroyInstance</c> event of an <c>OPCDAServer</c>.
    /// </summary>
    public delegate void DestroyInstanceEventHandler(Object sender, DestroyInstanceArgs e);
    /// <summary>
    /// Represents the method that will handle the <c>Lock</c> event of an <c>OPCDAServer</c>.
    /// </summary>
    public delegate void LockEventHandler(Object sender, LockArgs e);
    /// <summary>
    /// Represents the method that will handle the <c>Unlock</c> event of an <c>OPCDAServer</c>.
    /// </summary>
    public delegate void UnlockEventHandler(Object sender, UnlockArgs e);
    /// <summary>
    /// Represents the method that will handle the <c>ServerReleased</c> event of an <c>OPCDAServer</c>.
    /// </summary>
    public delegate void ServerReleasedEventHandler(Object sender, ServerReleasedArgs e);
    /// <summary>
    /// Represents the method that will handle the <c>WriteItems</c> event of an <c>OPCDAServer</c>.
    /// </summary>
    public delegate void WriteItemsEventHandler(Object sender, WriteItemsArgs e);
    /// <summary>
    /// Represents the method that will handle the <c>ReadItems</c> event of an <c>OPCDAServer</c>.
    /// </summary>
    public delegate void ReadItemsEventHandler(Object sender, ReadItemsArgs e);
    /// <summary>
    /// Represents the method that will handle the <c>DataUpdate</c> event of an <c>OPCDAServer</c>.
    /// </summary>
    public delegate void DataUpdateEventHandler(Object sender, DataUpdateArgs e);
    /// <summary>
    /// Represents the method that will handle the <c>ActivateItems</c> event of an <c>OPCDAServer</c>.
    /// </summary>
    public delegate void ActivateItemsEventHandler(Object sender, ActivateItemsArgs e);
    /// <summary>
    /// Represents the method that will handle the <c>DeactivateItems</c> event of an <c>OPCDAServer</c>.
    /// </summary>
    public delegate void DeactivateItemsEventHandler(Object sender, DeactivateItemsArgs e);
    /// <summary>
    /// Represents the method that will handle the <c>GetErrorString</c> event of an <c>OPCDAServer</c>.
    /// </summary>
    public delegate void GetErrorStringEventHandler(Object sender, GetErrorStringArgs e);
    /// <summary>
    /// Represents the method that will handle the <c>QueryLocales</c> event of an <c>OPCDAServer</c>.
    /// </summary>
    public delegate void QueryLocalesEventHandler(Object sender, QueryLocalesArgs e);
    /// <summary>
    /// Represents the method that will handle the <c>BrowseAccessPaths</c> event of an <c>OPCDAServer</c>.
    /// </summary>
    public delegate void BrowseAccessPathsEventHandler(Object sender, BrowseAccessPathsArgs e);
    /// <summary>
    /// Represents the method that will handle the <c>QueryItem</c> event of an <c>OPCDAServer</c>.
    /// </summary>
    public delegate void QueryItemEventHandler(Object sender, QueryItemArgs e);
    /// <summary>
    /// Represents the method that will handle the <c>ReadProperties</c> event of an <c>OPCDAServer</c>.
    /// </summary>
    public delegate void ReadPropertiesEventHandler(Object sender, ReadPropertiesArgs e);

    #endregion

    /// <summary>
    /// A class that holds the events of an <c>OPCDAServer</c>.
    /// </summary>
    public class OPCDAServerEvents
    {
        private IntPtr m_NativeHandle = IntPtr.Zero;
        private OPCDAServer m_Server = null;
        /// <summary>
        /// Creates a new instance of the OPCDAServerEvents class.
        /// </summary>
        /// <param name="nativeHandle">The handle of the instance of a native GBDataAccess class.</param>
        /// <param name="server">The parent OPCDAServer object.</param>
        public OPCDAServerEvents(IntPtr nativeHandle, OPCDAServer server)
        {
            m_NativeHandle = nativeHandle;
            m_Server = server;
        }

        private BeforeCreateInstanceEventHandler m_BeforeCreateInstance;
        /// <summary>
        /// Raises the BeforeCreateInstance event of the OPCDAServer instance.
        /// </summary>
        public void OnBeforeCreateInstance(BeforeCreateInstanceArgs e)
        {
            m_BeforeCreateInstance(m_Server, e);
        }
        /// <summary>
        /// <para>This event is invoked by the class factory immediately before the creation of a new OPC server instance.
        /// In its handler you may decide whether to allow the OPC client to connect to the OPC server.</para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBOnBeforeCreateInstance callback.</para>
        /// </summary>
        public event BeforeCreateInstanceEventHandler BeforeCreateInstance
        {
            add
            {
                m_BeforeCreateInstance += value;
                Gbdaflat.GBDataAccess_Advise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_BeforeCreateInstance);
            }
            remove
            {
                m_BeforeCreateInstance -= value;
                Gbdaflat.GBDataAccess_Unadvise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_BeforeCreateInstance);
            }
        }

        private CreateInstanceEventHandler m_CreateInstance;
        /// <summary>
        /// Raises the CreateInstance event of the OPCDAServer instance.
        /// </summary>
        public void OnCreateInstance(CreateInstanceArgs e)
        {
            m_CreateInstance(m_Server, e);
        }
        /// <summary>
        /// <para>Occures when the new instance of an OPC server is created.</para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBOnCreateInstance callback.</para>
        /// </summary>
        public event CreateInstanceEventHandler CreateInstance
        {
            add
            {
                m_CreateInstance += value;
                Gbdaflat.GBDataAccess_Advise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_CreateInstance);
            }
            remove
            {
                m_CreateInstance -= value;
                Gbdaflat.GBDataAccess_Unadvise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_CreateInstance);
            }
        }

        private DestroyInstanceEventHandler m_DestroyInstance;
        /// <summary>
        /// Raises the DestroyInstance event of the OPCDAServer instance.
        /// </summary>
        public void OnDestroyInstance(DestroyInstanceArgs e)
        {
            m_DestroyInstance(m_Server, e);
        }
        /// <summary>
        /// <para>Occures when the instance of an OPC server is destroyed.</para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBOnDestroyInstance callback.</para>
        /// </summary>
        public event DestroyInstanceEventHandler DestroyInstance
        {
            add
            {
                m_DestroyInstance += value;
                Gbdaflat.GBDataAccess_Advise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_DestroyInstance);
            }
            remove
            {
                m_DestroyInstance -= value;
                Gbdaflat.GBDataAccess_Unadvise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_DestroyInstance);
            }
        }

        private LockEventHandler m_Lock;
        /// <summary>
        /// Raises the Lock event of the OPCDAServer instance.
        /// </summary>
        public void OnLock(LockArgs e)
        {
            m_Lock(m_Server, e);
        }
        /// <summary>
        /// <para>Invoked when the class factory lock counter is incremented.</para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBOnLock callback.</para>
        /// </summary>
        public event LockEventHandler Lock
        {
            add
            {
                m_Lock += value;
                Gbdaflat.GBDataAccess_Advise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_Lock);
            }
            remove
            {
                m_Lock -= value;
                Gbdaflat.GBDataAccess_Unadvise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_Lock);
            }
        }

        private UnlockEventHandler m_Unlock;
        /// <summary>
        /// Raises the Unlock event of the OPCDAServer instance.
        /// </summary>
        public void OnUnlock(UnlockArgs e)
        {
            m_Unlock(m_Server, e);
        }
        /// <summary>
        /// <para>Invoked when the class factory Unlock counter is decremented.</para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBOnUnLock callback.</para>
        /// </summary>
        public event UnlockEventHandler Unlock
        {
            add
            {
                m_Unlock += value;
                Gbdaflat.GBDataAccess_Advise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_Unlock);
            }
            remove
            {
                m_Unlock -= value;
                Gbdaflat.GBDataAccess_Unadvise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_Unlock);
            }
        }

        private ServerReleasedEventHandler m_ServerReleased;
        /// <summary>
        /// Raises the ServerReleased event of the OPCDAServer instance.
        /// </summary>
        public void OnServerReleased(ServerReleasedArgs e)
        {
            m_ServerReleased(m_Server, e);
        }
        /// <summary>
        /// <para>Occures when the last instance of an OPC server is destroyed.
        /// It the ServerReleased event handler you can decide whether to allow
        /// the OPC clients to connect to the server later or to suspend the
        /// OPC server class object.</para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBOnServerReleased callback.</para>
        /// </summary>
        public event ServerReleasedEventHandler ServerReleased
        {
            add
            {
                m_ServerReleased += value;
                Gbdaflat.GBDataAccess_Advise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_ServerReleased);
            }
            remove
            {
                m_ServerReleased -= value;
                Gbdaflat.GBDataAccess_Unadvise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_ServerReleased);
            }
        }

        
        private WriteItemsEventHandler m_WriteItems;
        /// <summary>
        /// Raises the WriteItems event of the OPCDAServer instance.
        /// </summary>
        public void OnWriteItems(WriteItemsArgs e)
        {
            m_WriteItems(m_Server, e);
        }
        /// <summary>
        /// <para>Invoked when the OPC client writes new data to the OPC server's tags.</para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBOnWriteItems callback.</para>
        /// </summary>
        public event WriteItemsEventHandler WriteItems
        {
            add
            {
                m_WriteItems += value;
                Gbdaflat.GBDataAccess_Advise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_WriteItems);
            }
            remove
            {
                m_WriteItems -= value;
                Gbdaflat.GBDataAccess_Unadvise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_WriteItems);
            }
        }

        private ReadItemsEventHandler m_ReadItems;
        /// <summary>
        /// Raises the ReadItems event of the OPCDAServer instance.
        /// </summary>
        public void OnReadItems(ReadItemsArgs e)
        {
            m_ReadItems(m_Server, e);
        }
        /// <summary>
        /// <para>Occures when the OPC client requests tag reading from the DEVICE.
        /// Its handler must poll the underlying devices and place the new tags values into the OPC server cache.</para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBOnReadItems callback.</para>
        /// </summary>
        public event ReadItemsEventHandler ReadItems
        {
            add
            {
                m_ReadItems += value;
                Gbdaflat.GBDataAccess_Advise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_ReadItems);
            }
            remove
            {
                m_ReadItems -= value;
                Gbdaflat.GBDataAccess_Unadvise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_ReadItems);
            }
        }

        private DataUpdateEventHandler m_DataUpdate;
        /// <summary>
        /// Raises the DataUpdate event of the OPCDAServer instance.
        /// </summary>
        public void OnDataUpdate(DataUpdateArgs e)
        {
            m_DataUpdate(m_Server, e);
        }
        /// <summary>
        /// <para>Occures when the OPC server has sent a data update to the client.</para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBDataUpdate callback.</para>
        /// <para>To enable this event, set <c>ServerOptions.NotifyDataUpdates</c> flag then you call
        /// <c>OPCDAServer.Initialize</c>.</para>
        /// <para>WARNING. Handling this event may cause a significant decrease in performance
        /// because of a massive data marshaling from native to managed code.</para>
        /// </summary>
        public event DataUpdateEventHandler DataUpdate
        {
            add
            {
                m_DataUpdate += value;
                Gbdaflat.GBDataAccess_Advise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_DataUpdate);
            }
            remove
            {
                m_DataUpdate -= value;
                Gbdaflat.GBDataAccess_Unadvise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_DataUpdate);
            }
        }

        private ActivateItemsEventHandler m_ActivateItems;
        /// <summary>
        /// Raises the ActivateItems event of the OPCDAServer instance.
        /// </summary>
        public void OnActivateItems(ActivateItemsArgs e)
        {
            m_ActivateItems(m_Server, e);
        }
        /// <summary>
        /// <para>Occures when some tags becomes active.
        /// It happens when the OPC client requests these tags to be periodically updated from
        /// the underlying devices and
        /// their values to be sent to the clients via IOPCDataCallback.</para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBOnActivate callback.</para>
        /// </summary>
        public event ActivateItemsEventHandler ActivateItems
        {
            add
            {
                m_ActivateItems += value;
                Gbdaflat.GBDataAccess_Advise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_ActivateItems);
            }
            remove
            {
                m_ActivateItems -= value;
                Gbdaflat.GBDataAccess_Unadvise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_ActivateItems);
            }
        }

        private DeactivateItemsEventHandler m_DeactivateItems;
        /// <summary>
        /// Raises the DeactivateItems event of the OPCDAServer instance.
        /// </summary>
        public void OnDeactivateItems(DeactivateItemsArgs e)
        {
            m_DeactivateItems(m_Server, e);
        }
        /// <summary>
        /// <para>Occures when some tags becomes inactive.
        /// It happens when no more OPC clients are waiting for these tags to be updated
        /// and their values to be sent to the clients.</para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBOnDeactivate callback.</para>
        /// </summary>
        public event DeactivateItemsEventHandler DeactivateItems
        {
            add
            {
                m_DeactivateItems += value;
                Gbdaflat.GBDataAccess_Advise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_DeactivateItems);
            }
            remove
            {
                m_DeactivateItems -= value;
                Gbdaflat.GBDataAccess_Unadvise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_DeactivateItems);
            }
        }

        private GetErrorStringEventHandler m_GetErrorString;
        /// <summary>
        /// Raises the GetErrorString event of the OPCDAServer instance.
        /// </summary>
        public void OnGetErrorString(GetErrorStringArgs e)
        {
            m_GetErrorString(m_Server, e);
        }
        /// <summary>
        /// <para>Occures when the OPC client queries for the error description.</para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBOnGetErrorString callback.</para>
        /// </summary>
        public event GetErrorStringEventHandler GetErrorString
        {
            add
            {
                m_GetErrorString += value;
                Gbdaflat.GBDataAccess_Advise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_GetErrorString);
            }
            remove
            {
                m_GetErrorString -= value;
                Gbdaflat.GBDataAccess_Unadvise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_GetErrorString);
            }
        }

        private QueryLocalesEventHandler m_QueryLocales;
        /// <summary>
        /// Raises the QueryLocales event of the OPCDAServer instance.
        /// </summary>
        public void OnQueryLocales(QueryLocalesArgs e)
        {
            m_QueryLocales(m_Server, e);
        }
        /// <summary>
        /// <para>Occures when the client requests a list of locales supported by the server.</para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBOnQueryLocales callback.</para>
        /// </summary>
        public event QueryLocalesEventHandler QueryLocales
        {
            add
            {
                m_QueryLocales += value;
                Gbdaflat.GBDataAccess_Advise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_QueryLocales);
            }
            remove
            {
                m_QueryLocales -= value;
                Gbdaflat.GBDataAccess_Unadvise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_QueryLocales);
            }
        }

        private BrowseAccessPathsEventHandler m_BrowseAccessPaths;
        /// <summary>
        /// Raises the QueryLocales event of the OPCDAServer instance.
        /// </summary>
        public void OnBrowseAccessPaths(BrowseAccessPathsArgs e)
        {
            m_BrowseAccessPaths(m_Server, e);
        }
        /// <summary>
        /// <para>Occures when the OPC client requests a list of the tag's access paths.</para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBOnBrowseAccessPath callback.</para>
        /// </summary>
        public event BrowseAccessPathsEventHandler BrowseAccessPaths
        {
            add
            {
                m_BrowseAccessPaths += value;
                Gbdaflat.GBDataAccess_Advise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_BrowseAccessPaths);
            }
            remove
            {
                m_BrowseAccessPaths -= value;
                Gbdaflat.GBDataAccess_Unadvise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_BrowseAccessPaths);
            }
        }

        private QueryItemEventHandler m_QueryItem;
        /// <summary>
        /// Raises the QueryItem event of the OPCDAServer instance.
        /// </summary>
        public void OnQueryItem(QueryItemArgs e)
        {
            m_QueryItem(m_Server, e);
        }
        /// <summary>
        /// <para>Invoked when one of the following occures:
        /// <list type="number">
        /// <item><description>The client tries to validate a tag that is not yet created.</description></item>
        /// <item><description>The client tries to add a tag to a group, and this tag is not yet created.</description></item>
        /// <item><description>The client tries to read or write a tag that is not yet created.</description></item>
        /// <item><description>The client tries to read properties of a tag that is not yet created.</description></item>
        /// </list></para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBOnQueryItem callback.</para>
        /// </summary>
        public event QueryItemEventHandler QueryItem
        {
            add
            {
                m_QueryItem += value;
                Gbdaflat.GBDataAccess_Advise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_QueryItem);
            }
            remove
            {
                m_QueryItem -= value;
                Gbdaflat.GBDataAccess_Unadvise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_QueryItem);
            }
        }
        private ReadPropertiesEventHandler m_ReadProperties;
        /// <summary>
        /// Raises the ReadProperties event of the OPCDAServer instance.
        /// </summary>
        public void OnReadProperties(ReadPropertiesArgs e)
        {
            m_ReadProperties(m_Server, e);
        }
        /// <summary>
        /// <para>Occures when the client reads the values of the tag's properties.</para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBOnGetProperties callback.</para>
        /// </summary>
        public event ReadPropertiesEventHandler ReadProperties
        {
            add
            {
                m_ReadProperties += value;
                Gbdaflat.GBDataAccess_Advise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_ReadProperties);
            }
            remove
            {
                m_ReadProperties -= value;
                Gbdaflat.GBDataAccess_Unadvise(m_NativeHandle, (int)GBDataAccessEventIdx.EventIdx_ReadProperties);
            }
        }

    }

}
