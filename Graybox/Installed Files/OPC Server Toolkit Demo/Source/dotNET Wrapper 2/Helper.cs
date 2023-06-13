using System;
using System.Runtime.InteropServices;

namespace Graybox.OPC.ServerToolkit.CLRWrapper
{
	/// <summary>
	/// A structure used to represent the date and time of an OPC item value (its timestamp).
	/// A wrapper for WinAPI FILETIME structure.
	/// </summary>
	public struct FileTime
	{
        /// <summary>
		/// The underlying FILETIME value.
		/// </summary>
		[MarshalAs(UnmanagedType.I8)]
		private Int64 m_filetime;

		/// <summary>
		/// Initializes the object from the 64 bit integer.
		/// </summary>
		public FileTime(Int64 binary)
		{
			m_filetime = binary;
		}
		/// <summary>
		/// Initializes the object from the DateTime object.
		/// </summary>
		public FileTime(DateTime datetime)
		{
			m_filetime = datetime.ToFileTimeUtc();
		}

		/// <summary>
		/// Returns true is two objects are not equal.
		/// </summary>
        public static Boolean operator !=(FileTime a, FileTime b)
		{
			return a.m_filetime != b.m_filetime;
		}

		/// <summary>
		/// Returns true is two objects are equal.
		/// </summary>
        public static bool operator ==(FileTime a, FileTime b)
		{
			return a.m_filetime == b.m_filetime;
		}

        /// <summary>
        /// Determines whether the specified Object is equal to the current Object.
        /// </summary>
        /// <param name="o">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object o) // zzz
        {
            return (m_filetime == (Int64)o);
        }

        /// <summary>
        /// Retrieves a value that indicates the hash code value for the object.
        /// </summary>
        /// <returns>The hash code value for the object.</returns>
        public override int GetHashCode() // zzz
        {
            return 0;
        }
        
        /// <summary>
		/// Represents a FileTime which value is unspecified.
		/// </summary>
		public static FileTime Unspecified = new FileTime(0);
		
		/// <summary>
		/// Returns a FileTime representing the current system date and time in UTC.
		/// </summary>
		public static FileTime UtcNow
		{
			get { return new FileTime(DateTime.Now); }
		}

        /// <summary>
        /// Used to convert the FileTime object to the DateTime type.
        /// </summary>
        public DateTime ToDateTime()
		{
			return DateTime.FromFileTimeUtc(m_filetime);
		}

        /// <summary>
        /// Used to convert the FileTime object to the 64 bit integer.
        /// </summary>
        public Int64 ToInt64()
        {
            return m_filetime;
        }
    };


	/// <summary>
	///	A structure used to identify an OPC item.
	/// A wrapper for the native GBItemID structure.
	/// </summary>
    #if WindowsCE
    [StructLayout(LayoutKind.Sequential)]
    #else
	[StructLayout(LayoutKind.Sequential, Pack=4)]
    #endif
	public struct ItemId
	{
		/// <summary>
		/// Identifier of an OPC tag, which is represented by this OPC item.
		/// </summary>
		[MarshalAs(UnmanagedType.U4)]
		public Int32 TagId;

		/// Arbitrary identifier of an OPC tag set by OPC server implementation.
		[MarshalAs(UnmanagedType.U4)]
		public Int32 UserId;

		/// Identifies the Access Path of an OPC tag.
		[MarshalAs(UnmanagedType.U4)]
		public Int32 AccessPathId;
	};    
    
    
    /// <summary>
	/// The set of flags used to modify the OPC server behavior.
	/// </summary>
	[FlagsAttribute]
	public enum ServerOptions : uint
	{
		/// <summary>
		/// The default value (zero).
		/// </summary>
		Default = 0,
        /// <summary>
        /// OPC DA3 servers only. When the OPC Client browses the server address space it will
        /// be reported that all of the tags (tags, not the folders conotaing tags) possibly has
        /// children tags. Such behavior is acceptable by the specification. This can improve
        /// performance if server has a large amount of tags (about 10000 and more).
        /// If this flag is not set, then OPC Server will find out exactly whether the tag has any children or not.
        /// </summary>
        AssumeChildlessTags = 0x00000001,
        /// <summary>
        /// OPC DA3 servers only. If this flag is set, then the toolkit will assume,
        /// that tags can't have any children. This doesn't mean that there can be no
        /// tag folders in the address space hierarchy. If set, AssumeChildlessTags flag is ignored.
        /// </summary>
        ChildlessTags = 0x00000002,
        /// <summary>
        /// Ignore OPC item access paths. If your OPC Server does not use access paths,
        /// then this bit should be set to one in order to ensure its compliance this
        /// OPC Data Access specification.
        /// </summary>
        NoAccessPaths = 0x00000008,
		/// <summary>
		/// Used internally.
		/// </summary>
        FreeStrings = 0x00001000,
		/// <summary>
		/// If set the property 8 (Item EU Info) value will be reported
		/// for any engineering units type. Otherwise, the property 8 will
		/// be returned for the enumerated tags only.
		/// </summary>
        ReturnAnyEUInfo = 0x00002000,
        /// <summary>
        /// If set, then the LCID check is performed each time the OPC Client
        /// tries to create an OPC Group or set it's locale. Only LCIDs which
        /// are supported by the OPC Server are accepted.
        /// If this bit is not set, then any LCID is accepted for OPC Groups.
        /// </summary>
        GroupCheckLocale = 0x00004000,
		/// <summary>
		/// If set, then the default quality for the tags, being written by
		/// the OPC Clients, is OPC_QUALITY_LOCAL_OVERRIDE. Otherwise it is
		/// OPC_QUALITY_GOOD.
        /// </summary>
		DefaultOverrideQuality = 0x01000000,
		/// <summary>
		/// If set, then any OPC Client read request, both DEVICE and CACHE,
		/// will cause ReadItems event to be invoked.
		/// Otherwise ReadItems event is invoked, then the reading is performed
		/// with DEVICE datasource only.
        /// </summary>
        AlwaysDevice = 0x02000000,
        /// <summary>
        /// If set, then DataUpdate event is enabled. If this flag is not set, then
        /// DataUpdate event won't be invoked even if it is advised. WARNING, using
        /// DataUpdate event may significantly reduce .NET OPC Server performance
        /// because of a massive native to managed data marshaling. Use with care.
        /// </summary>
        NotifyDataUpdates = 0x04000000
    }

	/// <summary>
	/// Contains the error codes (HRESULTs) used by OPC Data Access methods.
	/// </summary>
	public enum ErrorCodes : uint
	{
        /// <summary>
        ///  The value of the handle is invalid.
        /// </summary>
        InvalidHandle = 0xC0040001,
        /// <summary>
        ///  The server cannot convert the data between the specified format
        /// and/or requested data type and the canonical data type. 
        /// </summary>
        BadType = 0xC0040004,
        /// <summary>
        /// The requested operation cannot be done on a public group.
        /// </summary>
        GroupIsPublic = 0xC0040005,
        /// <summary>
        /// The item's access rights do not allow the operation.
        /// </summary>
        BadItemsAccessRights = 0xC0040006,
        /// <summary>
        /// The item ID does not conform to the server's syntax.
        /// </summary>
        UnknownItemId = 0xC0040007,
        /// <summary>
        /// The item ID does not conform to the server's syntax.
        /// </summary>
        InvalidItemId = 0xC0040008,
        /// <summary>
        /// The filter string was not valid.
        /// </summary>
        InvalidFilter = 0xC0040009,
        /// <summary>
        /// The item's access path is not known to the server.
        /// </summary>
        UnknownAccessPath = 0xC004000A,
        /// <summary>
        /// The value was out of range.
        /// </summary>
        OutOfRange = 0xC004000B,
        /// <summary>
        /// Duplicate name not allowed.
        /// </summary>
        DuplicateName = 0xC004000C,
        /// <summary>
        /// The server does not support the requested data rate but will use the closest available rate.
        /// </summary>
        UnsupportedRate = 0x0004000D,
        /// <summary>
        /// A value passed to write was accepted but the output was clamped.
        /// </summary>
        Clamped = 0x0004000E,
        /// <summary>
        /// The operation cannot be performed because the object is bering referenced.
        /// </summary>
        InUse = 0x0004000F,
        /// <summary>
        /// The server's configuration file is an invalid format.
        /// </summary>
        InvalidConfig = 0xC0040010,
        /// <summary>
        /// The requested object (e.g. a public group) was not found.
        /// </summary>
        NotFound = 0xC0040011,
        /// <summary>
        /// The specified property ID is not valid for the item.
        /// </summary>
        InvalidPropertyId = 0xC0040203,
        /// <summary>
        /// The item deadband has not been set for this item.
        /// </summary>
        DeadbandNotSet = 0xC0040400,
        /// <summary>
        /// The item does not support deadband.
        /// </summary>
        DeadBandNotSupported = 0xC0040401,
        /// <summary>
        /// The server does not support buffering of data items that are collected at a faster rate than the group update rate.
        /// </summary>
        BufferingNotSupported = 0xC0040402,
        /// <summary>
        /// The continuation point is not valid.
        /// </summary>
        InvalidContinuationPoint = 0xC0040403,
        /// <summary>
        /// Not every detected change has been returned since the server's buffer reached its limit and had to purge out the oldest data.
        /// </summary>
        QueueOverflow = 0x00040404,
        /// <summary>
        /// There is no sampling rate set for the specified item.  
        /// </summary>
        RateNotSet = 0xC0040405,
        /// <summary>
        /// The server does not support writing of quality and/or timestamp.
        /// </summary>
        NotSupported = 0xC0040406,

        /// <summary>
        /// A session using private OPC credentials is already active.
        /// </summary>
        PrivateActive = 0xC0040301,
        /// <summary>
        /// Server requires higher impersonation level to access secured data.
        /// </summary>
        LowImpersonation = 0xC0040302,
        /// <summary>
        /// Server expected higher level of package privacy.
        /// </summary>
        LowAuthentication = 0xC0040303,
        
        /// <summary>
        /// No error.
        /// </summary>
        Ok = 0,
        /// <summary>
        /// The operation has completed but there was several errors.
        /// </summary>
        False = 1,
        /// <summary>
        /// The operation has failed.
        /// </summary>
        Fail = 0x80004005,
        /// <summary>
        /// The operation was aborted.
        /// </summary>
        Abort = 0x80004004,
        /// <summary>
        /// An unexpected error has occured.
        /// </summary>
        Unexpected = 0x8000FFFF,
        /// <summary>
        /// The method is not implemented.
        /// </summary>
        NotImplemented = 0x80004001,
        /// <summary>
        /// Access denied.
        /// </summary>
        AccessDenied = 0x80070005,
        /// <summary>
        /// Ran out of memory.
        /// </summary>
        OutOfMemory = 0x8007000E,
        /// <summary>
        /// One or more arguments are invalid.
        /// </summary>
        InvalidArguments = 0x80070057
    }

	/// <summary>
	/// The set of flags used to modify the way the Toolkit will process the OPC tag.
	/// </summary>
	[FlagsAttribute]
	public enum TagOptions : uint
	{
		/// <summary>
		/// The default value (zero).
		/// </summary>
		Default = 0,

		/// <summary>
		/// If set then tag value will be sent to OPC Clients via IOPCDataCallback interface every time
		/// its timestamp changes, even if its value has not changed.
		/// </summary>
        DontCompareValues = 0x00000200,

		/// <summary>
		/// If set then OPC clients are allowed to request the tag value in its canonical data type only.
		/// </summary>
        CanonicalOnly = 0x00000100,
		
		/// <summary>
		/// Useless in CLR environment. Do not use.
		/// </summary>
        DontCopyName = 0x00000001
	};


    /// <summary>
	/// Defines possible OPC item access rights.
	/// </summary>
    public enum AccessRights
    {
		/// <summary>
		/// OPC clients can't access a tag.
		/// </summary>
		noAccess = 0,
		/// <summary>
		/// A tag is readable.
		/// </summary>
        readable = 1,
		/// <summary>
		/// A tag is writable.
		/// </summary>
        writable = 2,
		/// <summary>
		/// A tag is readable and writeable.
		/// </summary>
        readWritable = 3
    };


    /// <summary>
	/// Defines possible OPC item engineering unit types
	/// </summary>
    public enum EUType
	{
		/// <summary>
		/// No engineering units is defined for a tag.
		/// </summary>
        noEnum = 0,
		/// <summary>
		/// A tag is analog and has a defined low and high limits, represented by engineering unit.
		/// </summary>
        analog = 1,
		/// <summary>
		/// A tag is an enumerated value and its possible states are described with a textual descriptions,
		/// defined by engineering unit.
		/// </summary>
        enumerated = 2
    };

    /// <summary>
	/// The set of possible OPC server states.
	/// </summary>
    public enum ServerState
    {
        /// <summary>
		/// The server state is unknown.
		/// </summary>
        unknown = 0,
        /// <summary>
		/// The server is running normally.
		/// </summary>
        running = 1,
		/// <summary>
		/// The server is not functioning due to a fatal error.
		/// </summary>
        failed = 2,
		/// <summary>
		/// The server cannot load its configuration information.
		/// </summary>
        noConfig = 3,
        /// <summary>
		/// The server has halted all communication with the underlying hardware.
		/// </summary>
        suspended = 4,
		/// <summary>
		/// The server is disconnected from the underlying hardware.
		/// </summary>
        test = 5,
		/// <summary>
		/// The server cannot communicate with the underlying hardware.
		/// </summary>
        commFault = 6
    }


    /// <summary>
	/// Defines the possible limit status bits.
	/// </summary>
    public enum LimitBits
    {
        /// <summary>
        /// The value is free to move up or down.
        /// </summary>
        none = 0,
        /// <summary>
        /// The value has ‘pegged’ at some lower limit.
        /// </summary>
        low = 1,
        /// <summary>
        /// The value has ‘pegged’ at some high limit.
        /// </summary>
        high = 2,
        /// <summary>
        /// The value is a constant and cannot move.
        /// </summary>
        constant = 3
    };

    /// <summary>
	/// Defines the possible quality status bits.
	/// </summary>
    public enum QualityBits : ushort
    {
        /// <summary>
        /// The value is bad but no specific reason is known.
        /// </summary>
        bad = 0,
        /// <summary>
        /// There is some server specific problem with the configuration.
        /// For example the item in question has been deleted from the configuration.
        /// </summary>
        badConfigurationError = 4,
        /// <summary>
        /// The input is required to be logically connected to something but is not.
        /// This quality may reflect that no value is available at this time,
        /// for reasons like the value may have not been provided by the data source.
        /// </summary>
        badNotConnected = 8,
        /// <summary>
        /// A device failure has been detected.
        /// </summary>
        badDeviceFailure = 12,
        /// <summary>
        /// A sensor failure had been detected
        /// (the ’Limits’ field can provide additional diagnostic information in some situations).
        /// </summary>
        badSensorFailure = 16,
        /// <summary>
        /// Communications have failed. However, the last known value is available.
        /// </summary>
        badLastKnownValue = 20,
        /// <summary>
        /// Communications have failed. There is no last known value is available.
        /// </summary>
        badCommFailure = 24,
        /// <summary>
        /// The block is off scan or otherwise locked. This quality is also used when the
        /// active state of the item or the group containing the item is InActive.
        /// </summary>
        badOutOfService = 28,
        /// <summary>
        /// After Items are added to a group, it may take some time for the server to actually
        /// obtain values for these items. In such cases the client might perform a read (from cache),
        /// or establish a ConnectionPoint based subscription and/or execute a Refresh on such
        /// a subscription before the values are available. OPC DA 3.0 or newer servers.
        /// </summary>
        badWaitingForInitialData = 32,
        /// <summary>
        /// The quality of the value is uncertain. There is no specific reason why the value is uncertain.
        /// </summary>
        uncertain = 64,
        /// <summary>
        /// Whatever was writing this value has stopped doing so.
        /// The returned value should be regarded as ‘stale’.
        /// </summary>
        uncertainLastUsableValue = 68,
        /// <summary>
        /// Either the value has ‘pegged’ at one of the sensor limits (in which case the limit field
        /// should be set to 1 or 2) or the sensor is otherwise known to be out of calibration via
        /// some form of internal diagnostics (in which case the limit field should be 0).
        /// </summary>
        uncertainSensorNotAccurate = 80,
        /// <summary>
        /// The returned value is outside the limits defined for this parameter.
        /// Note that in this case (per the Fieldbus Specification) the ‘Limits’ field indicates
        /// which limit has been exceeded but does NOT necessarily imply that the value cannot move farther out of range.
        /// </summary>
        uncertainEUExceeded = 84,
        /// <summary>
        /// The value is derived from multiple sources and has less than the required number of Good sources.
        /// </summary>
        uncertainSubNormal = 88,
        /// <summary>
        /// The value is good. There are no special conditions.
        /// </summary>
        good = 192,
        /// <summary>
        /// The value has been Overridden. Typically this is means the input has been
        /// disconnected and a manually entered value has been ‘forced’.
        /// </summary>
        goodLocalOverride = 216
    };

    /// <summary>
	/// Defines bit masks for the quality.
	/// </summary>
    public enum QualityMasks
    {
        /// <summary>
        /// These bits are available for vendor specific use.
        /// </summary>
        vendorMask = 0xff00,
        /// <summary>
        /// These bits hold the limit status.
        /// </summary>
        limitMask = 3,
        /// <summary>
        /// These bits hold the quality status and substatus.
        /// </summary>
        qualityMask = 252,
    };

    /// <summary>
	/// Contains the quality field for an item value.
	/// </summary>
    public struct Quality
    {
        /// <summary>
		/// A 'bad' quality value.
		/// </summary>
		public static Quality Bad = new Quality(QualityBits.bad);

        /// <summary>
		/// A 'good' quality value.
		/// </summary>
        public static Quality Good = new Quality(QualityBits.good);

		/// <summary>
		/// An 'unspecified' quality value.
		/// </summary>
        public static Quality Unspecified = new Quality(0xFFFF);

        /// <summary>
		/// Initializes the object with the specified quality.
		/// </summary>
        public Quality(QualityBits quality)
		{
			m_code = (UInt16)quality;
		}

        /// <summary>
		/// Initializes the object from the contents of a 16 bit integer.
		/// </summary>
        public Quality(UInt16 code)
		{
			m_code = code;
		}

        /// <summary>
		/// Returns true if the objects are not equal.
		/// </summary>
        public static Boolean operator !=(Quality a, Quality b)
		{
			return a.Code != b.Code;
		}

        /// <summary>
		/// Returns true if the objects are equal.
		/// </summary>
        public static bool operator ==(Quality a, Quality b)
		{
			return a.Code == b.Code;
		}

        /// <summary>
        /// Determines whether the specified Object is equal to the current Object.
        /// </summary>
        /// <param name="o">The Object to compare with the current Object.</param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object o)
        {
            return (m_code == (ushort)o);
        }

        /// <summary>
        /// Retrieves a value that indicates the hash code value for the object.
        /// </summary>
        /// <returns>The hash code value for the object.</returns>
        public override int GetHashCode()
        {
            return Code;
        }

        /// <summary>
		/// The value in the limit bits field.
		/// </summary>
		public LimitBits LimitStatus
		{
			get { return (LimitBits)(m_code & (Int32)QualityMasks.limitMask); }
			set { m_code = (UInt16)(m_code & ~(Int32)QualityMasks.limitMask | (Int32)value); }
		}

        /// <summary>
		/// The value in the quality bits field.
		/// </summary>
		public QualityBits QualityStatus
		{
			get { return (QualityBits)(m_code & (Int32)QualityMasks.qualityMask); }
			set { m_code = (UInt16)(m_code & ~(Int32)QualityMasks.qualityMask | (Int32)value); }
		}

        /// <summary>
		/// The value in the vendor bits field.
		/// </summary>
        public Byte VendorStatus
		{
			get { return (byte)(m_code >> 8); }
			set { m_code = (UInt16)((m_code & (~(Int32)QualityMasks.vendorMask)) | ((Int32)value << 8)); }
		}

        /// <summary>
		/// Returns the quality as a 16 bit integer.
		/// </summary>
        public UInt16 Code
		{
			get { return m_code; }
			set { m_code = value; }
		}

		/// <summary>
		/// Returns ture if quality is 'bad'.
		/// </summary>
		public Boolean IsBad
		{
            get { return (m_code & 0xc0) == 0; }
		}

		/// <summary>
		/// Returns ture if quality is 'good'.
		/// </summary>
		public Boolean IsGood
		{
            get { return (m_code & 0xc0) == 0xc0; }
		}

		/// <summary>
		/// Returns ture if quality is 'uncertain'.
		/// </summary>
		public Boolean IsUncertain
		{
            get { return (m_code & 0xc0) == 0x40; }
		}

		/// Converts a quality to a string with the format: 'quality[limit]:vendor'.
        //override string ToString();

		[MarshalAs(UnmanagedType.U2)]
		private UInt16 m_code;
    };

	/// <summary>
	/// Contains the IDs of the standard OPC properties.
	/// </summary>
	public enum StandardProperties : int
    {
        /// <summary>
        /// Item Access Rights
        /// </summary>
        ItemAccessRights = 5,
        /// <summary>
        /// Alarm Area List
        /// </summary>
        AlarmAreaList = 302,
        /// <summary>
        /// Alarm Quick Help
        /// </summary>
        AlarmQuickHelp = 301,
        /// <summary>
        /// Rate of Change Limit
        /// </summary>
        RateOfChangeLimit = 311,
        /// <summary>
        /// Contact Close Label
        /// </summary>
        ContactCloseLabel = 106,
        /// <summary>
        /// Condition Logic
        /// </summary>
        ConditionLogic = 304,
        /// <summary>
        /// Condition Status
        /// </summary>
        ConditionStatus = 300,
        /// <summary>
        /// Consistency Window
        /// </summary>
        ConsistencyWindow = 605,
        /// <summary>
        /// Data Filter Value
        /// </summary>
        DataFilterValue = 609,
        /// <summary>
        /// Item Canonical Data Type
        /// </summary>
        ItemCanonicalDataType = 1,
        /// <summary>
        /// Deadband
        /// </summary>
        Deadband = 306,
        /// <summary>
        /// Item Description
        /// </summary>
        ItemDescription = 101,
        /// <summary>
        /// Deviation Limit
        /// </summary>
        DeviationLimit = 312,
        /// <summary>
        /// Dictionary
        /// </summary>
        Dictionary = 603,
        /// <summary>
        /// Dictionary ID
        /// </summary>
        DictionaryID = 601,
        /// <summary>
        /// Item EU Info
        /// </summary>
        ItemEUInfo = 8,
        /// <summary>
        /// Item EU Type
        /// </summary>
        ItemEUType = 7,
        /// <summary>
        /// EU Units
        /// </summary>
        EUUnits = 100,
        /// <summary>
        /// Hi Limit
        /// </summary>
        HiLimit = 308,
        /// <summary>
        /// High EU
        /// </summary>
        HighEU = 102,
        /// <summary>
        /// High Instrument Range
        /// </summary>
        HighInstrumentRange = 104,
        /// <summary>
        /// HiHi Limit
        /// </summary>
        HiHiLimit = 307,
        /// <summary>
        /// Limit Exceeded
        /// </summary>
        LimitExceeded = 305,
        /// <summary>
        /// Lo Limit
        /// </summary>
        LoLimit = 309,
        /// <summary>
        /// LoLo Limit
        /// </summary>
        LoLoLimit = 310,
        /// <summary>
        /// Low EU
        /// </summary>
        LowEU = 103,
        /// <summary>
        /// Low Instrument Range
        /// </summary>
        LowInstrumentRange = 105,
        /// <summary>
        /// Contact Open Label
        /// </summary>
        ContactOpenLabel = 107,
        /// <summary>
        /// Primary Alarm Area
        /// </summary>
        PrimaryAlarmArea = 303,
        /// <summary>
        /// Item Quality
        /// </summary>
        ItemQuality = 3,
        /// <summary>
        /// Server Scan Rate
        /// </summary>
        ServerScanRate = 6,
        /// <summary>
        /// Sound File
        /// </summary>
        SoundFile = 313,
        /// <summary>
        /// Item Timestamp
        /// </summary>
        ItemTimestamp = 4,
        /// <summary>
        /// Item Timezone
        /// </summary>
        ItemTimezone = 108,
        /// <summary>
        /// Type Description
        /// </summary>
        TypeDescription = 604,
        /// <summary>
        /// Type ID
        /// </summary>
        TypeID = 602,
        /// <summary>
        /// Type System ID
        /// </summary>
        TypeSystemID = 600,
        /// <summary>
        /// Unconverted Item ID
        /// </summary>
        UnconvertedItemID = 607,
        /// <summary>
        /// Unfiltered Item ID
        /// </summary>
        UnfilteredItemID = 608,
        /// <summary>
        /// Item Value
        /// </summary>
        ItemValue = 2,
        /// <summary>
        /// Write Behavior
        /// </summary>
        WriteBehavior = 606
    }

    #region Internal definitions

    /// <summary>
    /// A wrapper for constants defined in gb_opcda.h.
    /// Used by the wrapper internally.
    /// </summary>
    internal enum NativeConstants
    {
        GB_RET_NOP              = 0x00000000,
        GB_RET_CACHE            = 0x00000001,
        GB_RET_ARG              = 0x00000002,

        GB_SRV_DONTFINDCHLD     = 0x00000001,
        GB_SRV_CHILDLESS        = 0x00000002,
        GB_SRV_NOACCESSPATH     = 0x00000008,
        GB_SRV_FREEERRSTR       = 0x00001000,

        GB_TAG_NOVALCMP         = 0x00000200,
        GB_TAG_CANONONLY        = 0x00000100,
        GB_TAG_DONTCOPYSTR      = 0x00000001,

        GB_PROP_DONTCOPYSTR     = 0x00000001
    }

    internal static class Helpers
    {
        /// <summary>
        /// Size in bytes of a native VARIANT structure.
        /// </summary>
        public static readonly int SizeOfVariant = IntPtr.Size * 2 + 8;
        /// <summary>
        /// Size in bytes of a GUID structure.
        /// </summary>
        public static readonly int SizeOfGuid = Marshal.SizeOf(typeof(Guid));

        /// <summary>
        /// This method is used to marshal a Guid structure to the
        /// managed code. We need it because .NET CF 2.0 doesn't support
        /// marshaling from Guid to UnmanagedType.LPStruct (unsupported
        /// operation exception occures).
        /// </summary>
        /// <param name="guid">A Guid object to convert.</param>
        /// <returns>A pointer to memory allocated in the local unmanaged heap to hold the resulting GUID structure.</returns>
        public static IntPtr GuidToHglobal(Guid guid)
        {
            IntPtr ptr = Marshal.AllocHGlobal(Helpers.SizeOfGuid);
            Marshal.Copy(guid.ToByteArray(), 0, ptr, Helpers.SizeOfGuid);
            return ptr;
        }

        #if WindowsCE
        /// <summary>
        /// Converts an array of COM VARIANTs to an array of objects. 
        /// <para>We need it because .NET CF 2 does not support GetObjectsForNativeVariants.</para>
        /// </summary>
        /// <param name="aSrcNativeVariant">An IntPtr containing the first element of an array of COM VARIANTs.</param>
        /// <param name="cVars">The count of COM VARIANTs in aSrcNativeVariant.</param>
        /// <returns>An object array corresponding to aSrcNativeVariant.</returns>
        unsafe public static object[] GetObjectsForNativeVariants(IntPtr aSrcNativeVariant, int cVars)
        {
            object[] vals = new object[cVars];
            for (int i = 0; i < cVars; i++)
            {
                vals[i] = Marshal.GetObjectForNativeVariant(new IntPtr(&((byte*)aSrcNativeVariant.ToPointer())[i * Helpers.SizeOfVariant]));
            }
            return vals;
        }
#endif

#if WindowsCE
        /// <summary>
        /// Copies the contents of a managed String into unmanaged memory.
        /// <para>We need it because .NET CF 2 does not support StringToHGlobalUni.</para>
        /// </summary>
        /// <param name="str">A managed string to be copied.</param>
        /// <returns>The address, in unmanaged memory, to where the s was copied,
        /// or 0 if a null string was supplied.</returns>
        public static IntPtr StringToHGlobalUni(String str)
        {
            if (str == null) return IntPtr.Zero;
            char[] chars = str.ToCharArray();
            IntPtr ptr = Marshal.AllocHGlobal((chars.Length + 1) * 2);
            Marshal.Copy(chars, 0, ptr, chars.Length);
            Marshal.WriteInt16(ptr, chars.Length * 2, 0);
            return ptr;
        }
#endif

        /// <summary>
        /// Defines the native WinAPI ConvertDefaultLocale function.
        /// </summary>
        [DllImport("kernel32.dll", EntryPoint = "ConvertDefaultLocale", CallingConvention = CallingConvention.Winapi)]
        public static extern Int32 ConvertDefaultLocale(Int32 dwLcid);

        /// <summary>
        /// Defines the native WinAPI VariantClear function. This function clears a variant.
        /// </summary>
        /// <param name="pvarg">Pointer to the VARIANTARG to clear.</param>
        /// <returns>Returns the HRESULT.</returns>
        [DllImport("oleaut32.dll", EntryPoint = "VariantClear", CallingConvention = CallingConvention.Winapi)]
        public static extern Int32 VariantClear(IntPtr pvarg);

        /// <summary>
        /// Defines the native WinAPI VariantChangeTypeEx function.
        /// This function converts a variant from one type to another, using a LCID.
        /// </summary>
        /// <param name="pvargDest">Pointer to the VARIANTARG to receive the coerced type. 
        /// If this is the same as pvarSrc, the variant is converted in place.</param>
        /// <param name="pvarSrc">Pointer to the source VARIANTARG to be coerced.</param>
        /// <param name="lcid">LCID for the variant to coerce.</param>
        /// <param name="wFlags">Flags that control the coercion.</param>
        /// <param name="vt">Specifies the type to coerce to.</param>
        /// <returns>Returns the HRESULT.</returns>
        [DllImport("oleaut32.dll", EntryPoint = "VariantChangeTypeEx", CallingConvention = CallingConvention.Winapi)]
        public static extern Int32 VariantChangeTypeEx(
            IntPtr pvargDest,
            IntPtr pvarSrc,
            Int32 lcid,
            Int16 wFlags,
            Int16 vt);
    }
    #endregion
}
