#pragma once

#include <opcda.h>
#include <opcerror.h>
#include <opcerrsec.h>
#include <gb_opcda.h>

using namespace System;
using namespace System::Runtime::InteropServices;

namespace Graybox { namespace OPC { namespace ServerToolkit { namespace CLRWrapper
{
	/// <summary>
	/// A structure used to represent the date and time of an OPC item value (its timestamp).
	/// A wrapper for WinAPI FILETIME structure.
	/// </summary>
	public value class FileTime
	{
        /// <summary>
		/// The underlying FILETIME value.
		/// </summary>
		[MarshalAs(UnmanagedType::I8)]
		Int64 m_filetime;
	public:
		/// <summary>
		/// Initializes the object from the 64 bit integer.
		/// </summary>
		FileTime(Int64 binary)
		{
			m_filetime = binary;
		}
		/// <summary>
		/// Initializes the object from the DateTime object.
		/// </summary>
		FileTime(DateTime datetime)
		{
			m_filetime = datetime.ToFileTimeUtc();
		}

		/// <summary>
		/// Returns true is two objects are not equal.
		/// </summary>
        static Boolean operator !=(FileTime a, FileTime b)
		{
			return a.m_filetime != b.m_filetime;
		}

		/// <summary>
		/// Returns true is two objects are equal.
		/// </summary>
        static bool operator ==(FileTime a, FileTime b)
		{
			return a.m_filetime == b.m_filetime;
		}

		/// <summary>
		/// Represents a FileTime which value is unspecified.
		/// </summary>
		static FileTime Unspecified = FileTime(0);
		
		/// <summary>
		/// Returns a FileTime representing the current system date and time in UTC.
		/// </summary>
		static property FileTime UtcNow
		{
			FileTime get()
			{
				Int64 i;
				pin_ptr<Int64> p = &i;
				GetSystemTimeAsFileTime((LPFILETIME)p);
				return FileTime(i);
			}
		}

		/// <summary>
		/// Used to convert the FileTime object to the DateTime type.
		/// </summary>
		DateTime ToDateTime()
		{
			return DateTime::FromFileTimeUtc(m_filetime);
		}
		/// <summary>
		/// Used to convert the FileTime object to the 64 bit integer.
		/// </summary>
		Int64 ToInt64()
		{
			return m_filetime;
		}
	};


	/// <summary>
	///	A structure used to identify an OPC item.
	/// A wrapper for tha native GBItemID structure.
	/// </summary>
	[StructLayout(LayoutKind::Sequential, Pack=4)]
	public value struct ItemId
	{
		/// <summary>
		/// Identifier of an OPC tag, which is represented by this OPC item.
		/// </summary>
		[MarshalAs(UnmanagedType::U4)]
		Int32 TagId;

		/// Arbitrary identifier of an OPC tag set by OPC server implementation.
		[MarshalAs(UnmanagedType::U4)]
		Int32 UserId;

		/// Identifies the Access Path of an OPC tag.
		[MarshalAs(UnmanagedType::U4)]
		Int32 AccessPathId;
	};


    /// <summary>
	/// The set of possible OPC server states.
	/// </summary>
    public enum class ServerState
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
    };

	
    /// <summary>
	/// Defines possible OPC item access rights.
	/// </summary>
    public enum class AccessRights
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
    public enum class EUType
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
	/// Defines the possible limit status bits.
	/// </summary>
    public enum class LimitBits
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
    public enum class QualityBits
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
    public enum class QualityMasks
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
    [Serializable]
    public value struct Quality
    {
        /// <summary>
		/// A 'bad' quality value.
		/// </summary>
		static const Quality Bad = Quality(QualityBits::bad);

        /// <summary>
		/// A 'good' quality value.
		/// </summary>
        static const Quality Good = Quality(QualityBits::good);

		/// <summary>
		/// An 'unspecified' quality value.
		/// </summary>
		static const Quality Unspecified = Quality(0xFFFF);

        /// <summary>
		/// Initializes the object with the specified quality.
		/// </summary>
        Quality(QualityBits quality)
		{
			m_code = (UInt16)quality;
		}

        /// <summary>
		/// Initializes the object from the contents of a 16 bit integer.
		/// </summary>
        Quality(UInt16 code)
		{
			m_code = code;
		}

        /// <summary>
		/// Returns true if the objects are not equal.
		/// </summary>
        static Boolean operator !=(Quality a, Quality b)
		{
			return a.Code != b.Code;
		}

        /// <summary>
		/// Returns true if the objects are equal.
		/// </summary>
        static bool operator ==(Quality a, Quality b)
		{
			return a.Code == b.Code;
		}

        /// <summary>
		/// The value in the limit bits field.
		/// </summary>
		property LimitBits LimitStatus
		{
			LimitBits get() { return (LimitBits)(m_code & (Int32)QualityMasks::limitMask); }
			void set(LimitBits val) { m_code = m_code & ~(Int32)QualityMasks::limitMask | (Int32)val; }
		}

        /// <summary>
		/// The value in the quality bits field.
		/// </summary>
		property QualityBits QualityStatus
		{
			QualityBits get() { return (QualityBits)(m_code & (Int32)QualityMasks::qualityMask); }
			void set(QualityBits val) { m_code = m_code & ~(Int32)QualityMasks::qualityMask | (Int32)val; }
		}

        /// <summary>
		/// The value in the vendor bits field.
		/// </summary>
        property Byte VendorStatus
		{
			Byte get() { return m_code >> 8; }
			void set(Byte val) { m_code = (m_code & (~(Int32)QualityMasks::vendorMask)) | ((Int32)val << 8); }
		}

        /// <summary>
		/// Returns the quality as a 16 bit integer.
		/// </summary>
        property UInt16 Code
		{
			UInt16 get() { return m_code; }
			void set(UInt16 val) { m_code = val; }
		}

		/// <summary>
		/// Returns ture if quality is 'bad'.
		/// </summary>
		property Boolean IsBad
		{
			Boolean get() { return (m_code & OPC_QUALITY_MASK) == OPC_QUALITY_BAD; }
		}

		/// <summary>
		/// Returns ture if quality is 'good'.
		/// </summary>
		property Boolean IsGood
		{
			Boolean get() { return (m_code & OPC_QUALITY_MASK) == OPC_QUALITY_GOOD; }
		}

		/// <summary>
		/// Returns ture if quality is 'uncertain'.
		/// </summary>
		property Boolean IsUncertain
		{
			Boolean get() { return (m_code & OPC_QUALITY_MASK) == OPC_QUALITY_UNCERTAIN; }
		}

		/// Converts a quality to a string with the format: 'quality[limit]:vendor'.
        //override string ToString();

	private:
		[MarshalAs(UnmanagedType::U2)]
		Int16 m_code;
    };

	/// <summary>
	/// The set of flags used to modify the way the Toolkit will process the OPC tag.
	/// </summary>
	[FlagsAttribute]
	public enum class TagOptions : Int32
	{
		/// <summary>
		/// The default value (zero).
		/// </summary>
		Default = 0,

		/// <summary>
		/// If set then tag value will be sent to OPC Clients via IOPCDataCallback interface every time
		/// its timestamp changes, even if its value has not changed.
		/// </summary>
		DontCompareValues = GB_TAG_NOVALCMP,

		/// <summary>
		/// If set then OPC clients are allowed to request the tag value in its canonical data type only.
		/// </summary>
		CanonicalOnly = GB_TAG_CANONONLY,
		
		/// <summary>
		/// Useless in CLR environment. Do not use.
		/// </summary>
		DontCopyName = GB_TAG_DONTCOPYSTR
	};

	/// <summary>
	/// The set of flags used to modify the OPC server behavior.
	/// </summary>
	[FlagsAttribute]
	public enum class ServerOptions : Int32
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
		AssumeChildlessTags = GB_SRV_DONTFINDCHLD,
        /// <summary>
        /// OPC DA3 servers only. If this flag is set, then the toolkit will assume,
        /// that tags can't have any children. This doesn't mean that there can be no
        /// tag folders in the address space hierarchy. If set, AssumeChildlessTags flag is ignored.
        /// </summary>
		ChildlessTags = GB_SRV_CHILDLESS,
        /// <summary>
        /// Ignore OPC item access paths. If your OPC Server does not use access paths,
        /// then this bit should be set to one in order to ensure its compliance this
        /// OPC Data Access specification.
        /// </summary>
		NoAccessPaths = GB_SRV_NOACCESSPATH,
		/// <summary>
		/// Used internally.
		/// </summary>
		FreeStrings = GB_SRV_FREEERRSTR,
		/// <summary>
		/// If set the property 8 (Item EU Info) value will be reported
		/// for any engineering units type. Otherwise, the property 8 will
		/// be returned for the enumerated tags only.
		/// </summary>
		ReturnAnyEUInfo = GB_SRV_RETANYEUINFO,
        /// <summary>
        /// If set, then the LCID check is performed each time the OPC Client
        /// tries to create an OPC Group or set it's locale. Only LCIDs which
        /// are supported by the OPC Server are accepted.
        /// If this bit is not set, then any LCID is accepted for OPC Groups.
        /// </summary>
		GroupCheckLocale = GB_SRV_GRPCHECKLCID,
		/// <summary>
		/// If set, then the default quality for the tags, being written by
		/// the OPC Clients, is OPC_QUALITY_LOCAL_OVERRIDE. Otherwise it is
		/// OPC_QUALITY_GOOD.
        /// </summary>
		DefaultOverrideQuality = GB_SRV_USEOVRDQUAL,
		/// <summary>
		/// If set, then any OPC Client read request, both DEVICE and CACHE,
		/// will cause ReadItems event to be invoked.
		/// Otherwise ReadItems event is invoked, then the reading is performed
		/// with DEVICE datasource only.
        /// </summary>
		AlwaysDevice = GB_SRV_ALWAYSDEVICE,
		/// <summary>
		/// If set, then DataUpdate event is enabled. If this flag is not set, then
		/// DataUpdate event won't be invoked even if it is advised. WARNING, using
		/// DataUpdate event may significantly reduce .NET OPC Server performance
		/// because of a massive native to managed data marshaling. Use with care.
        /// </summary>
		NotifyDataUpdates = GB_SRV_DATANOTIFY
	};

	/// <summary>
	/// Contains the error codes (HRESULTs) used by OPC Data Access methods.
	/// </summary>
	public enum class ErrorCodes : Int32
	{
		/// <summary>
		///  The value of the handle is invalid.
		/// </summary>
		InvalidHandle = OPC_E_INVALIDHANDLE,
		/// <summary>
		///  The server cannot convert the data between the specified format and/or requested data type and the canonical data type. 
		/// </summary>
		BadType = OPC_E_BADTYPE,
		/// <summary>
		/// The requested operation cannot be done on a public group.
		/// </summary>
		GroupIsPublic = OPC_E_PUBLIC,
		/// <summary>
		/// The item's access rights do not allow the operation.
		/// </summary>
		BadItemsAccessRights = OPC_E_BADRIGHTS,
		/// <summary>
		/// The item ID does not conform to the server's syntax.
		/// </summary>
		UnknownItemId = OPC_E_UNKNOWNITEMID,
		/// <summary>
		/// The item ID does not conform to the server's syntax.
		/// </summary>
		InvalidItemId = OPC_E_INVALIDITEMID,
		/// <summary>
		/// The filter string was not valid.
		/// </summary>
		InvalidFilter = OPC_E_INVALIDFILTER,
		/// <summary>
		/// The item's access path is not known to the server.
		/// </summary>
		UnknownAccessPath = OPC_E_UNKNOWNPATH,
		/// <summary>
		/// The value was out of range.
		/// </summary>
		OutOfRange = OPC_E_RANGE,
		/// <summary>
		/// Duplicate name not allowed.
		/// </summary>
		DuplicateName = OPC_E_DUPLICATENAME,
		/// <summary>
		/// The server does not support the requested data rate but will use the closest available rate.
		/// </summary>
		UnsupportedRate = OPC_S_UNSUPPORTEDRATE,
		/// <summary>
		/// A value passed to write was accepted but the output was clamped.
		/// </summary>
		Clamped = OPC_S_CLAMP,
		/// <summary>
		/// The operation cannot be performed because the object is bering referenced.
		/// </summary>
		InUse = OPC_S_INUSE,
		/// <summary>
		/// The server's configuration file is an invalid format.
		/// </summary>
		InvalidConfig = OPC_E_INVALIDCONFIGFILE,
		/// <summary>
		/// The requested object (e.g. a public group) was not found.
		/// </summary>
		NotFound = OPC_E_NOTFOUND,
		/// <summary>
		/// The specified property ID is not valid for the item.
		/// </summary>
		InvalidPropertyId = OPC_E_INVALID_PID,
		/// <summary>
		/// The item deadband has not been set for this item.
		/// </summary>
		DeadbandNotSet = OPC_E_DEADBANDNOTSET,
		/// <summary>
		/// The item does not support deadband.
		/// </summary>
		DeadBandNotSupported = OPC_E_DEADBANDNOTSUPPORTED,
		/// <summary>
		/// The server does not support buffering of data items that are collected at a faster rate than the group update rate.
		/// </summary>
		BufferingNotSupported = OPC_E_NOBUFFERING,
		/// <summary>
		/// The continuation point is not valid.
		/// </summary>
		InvalidContinuationPoint = OPC_E_INVALIDCONTINUATIONPOINT,
		/// <summary>
		/// Not every detected change has been returned since the server's buffer reached its limit and had to purge out the oldest data.
		/// </summary>
		QueueOverflow = OPC_S_DATAQUEUEOVERFLOW,
		/// <summary>
		/// There is no sampling rate set for the specified item.  
		/// </summary>
		RateNotSet = OPC_E_RATENOTSET,
		/// <summary>
		/// The server does not support writing of quality and/or timestamp.
		/// </summary>
		NotSupported = OPC_E_NOTSUPPORTED,

		/// <summary>
		/// A session using private OPC credentials is already active.
		/// </summary>
		PrivateActive = OPC_E_PRIVATE_ACTIVE,
		/// <summary>
		/// Server requires higher impersonation level to access secured data.
		/// </summary>
		LowImpersonation = OPC_E_LOW_IMPERS_LEVEL,
		/// <summary>
		/// Server expected higher level of package privacy.
		/// </summary>
		LowAuthentication = OPC_S_LOW_AUTHN_LEVEL,

		/// <summary>
		/// No error.
		/// </summary>
		Ok = S_OK,
		/// <summary>
		/// The operation has completed but there was several errors.
		/// </summary>
		False = S_FALSE,
		/// <summary>
		/// The operation has failed.
		/// </summary>
		Fail = E_FAIL,
		/// <summary>
		/// The operation was aborted.
		/// </summary>
		Abort = E_ABORT,
		/// <summary>
		/// An unexpected error has occured.
		/// </summary>
		Unexpected = E_UNEXPECTED,
		/// <summary>
		/// The method is not implemented.
		/// </summary>
		NotImplemented = E_NOTIMPL,
		/// <summary>
		/// Access denied.
		/// </summary>
		AccessDenied = E_ACCESSDENIED,
        /// <summary>
        /// Ran out of memory.
        /// </summary>
		OutOfMemory = E_OUTOFMEMORY,
        /// <summary>
        /// One or more arguments are invalid.
        /// </summary>
		InvalidArguments = E_INVALIDARG
	};


	/// <summary>
	/// Contains the IDs of the standard OPC properties.
	/// </summary>
	public enum class StandardProperties : Int32
    {
        /// <summary>
        /// Item Access Rights
        /// </summary>
        ItemAccessRights = OPC_PROPERTY_ACCESS_RIGHTS,
        /// <summary>
        /// Alarm Area List
        /// </summary>
        AlarmAreaList = OPC_PROPERTY_ALARM_AREA_LIST,
        /// <summary>
        /// Alarm Area List
        /// </summary>
        AlarmQuickHelp = OPC_PROPERTY_ALARM_QUICK_HELP,
        /// <summary>
        /// Alarm Area List
        /// </summary>
        RateOfChangeLimit = OPC_PROPERTY_CHANGE_RATE_LIMIT,
        /// <summary>
        /// Contact Close Label
        /// </summary>
        ContactCloseLabel = OPC_PROPERTY_CLOSE_LABEL,
        /// <summary>
        /// Condition Logic
        /// </summary>
        ConditionLogic = OPC_PROPERTY_CONDITION_LOGIC,
        /// <summary>
        /// Condition Status
        /// </summary>
        ConditionStatus = OPC_PROPERTY_CONDITION_STATUS,
        /// <summary>
        /// Consistency Window
        /// </summary>
        ConsistencyWindow = OPC_PROPERTY_CONSISTENCY_WINDOW,
        /// <summary>
        /// Data Filter Value
        /// </summary>
        DataFilterValue = OPC_PROPERTY_DATA_FILTER_VALUE,
        /// <summary>
        /// Item Canonical Data Type
        /// </summary>
        ItemCanonicalDataType = OPC_PROPERTY_DATATYPE,
        /// <summary>
        /// Deadband
        /// </summary>
        Deadband = OPC_PROPERTY_DEADBAND,
        /// <summary>
        /// Item Description
        /// </summary>
        ItemDescription = OPC_PROPERTY_DESCRIPTION,
        /// <summary>
        /// Deviation Limit
        /// </summary>
        DeviationLimit = OPC_PROPERTY_DEVIATION_LIMIT,
        /// <summary>
        /// Dictionary
        /// </summary>
        Dictionary = OPC_PROPERTY_DICTIONARY,
        /// <summary>
        /// Dictionary ID
        /// </summary>
        DictionaryID = OPC_PROPERTY_DICTIONARY_ID,
        /// <summary>
        /// Item EU Info
        /// </summary>
        ItemEUInfo = OPC_PROPERTY_EU_INFO,
        /// <summary>
        /// Item EU Type
        /// </summary>
        ItemEUType = OPC_PROPERTY_EU_TYPE,
        /// <summary>
        /// EU Units
        /// </summary>
        EUUnits = OPC_PROPERTY_EU_UNITS,
        /// <summary>
        /// Hi Limit
        /// </summary>
        HiLimit = OPC_PROPERTY_HI_LIMIT,
        /// <summary>
        /// High EU
        /// </summary>
        HighEU = OPC_PROPERTY_HIGH_EU,
        /// <summary>
        /// High Instrument Range
        /// </summary>
        HighInstrumentRange = OPC_PROPERTY_HIGH_IR,
        /// <summary>
        /// HiHi Limit
        /// </summary>
        HiHiLimit = OPC_PROPERTY_HIHI_LIMIT,
        /// <summary>
        /// Limit Exceeded
        /// </summary>
        LimitExceeded = OPC_PROPERTY_LIMIT_EXCEEDED,
        /// <summary>
        /// Lo Limit
        /// </summary>
        LoLimit = OPC_PROPERTY_LO_LIMIT,
        /// <summary>
        /// LoLo Limit
        /// </summary>
        LoLoLimit = OPC_PROPERTY_LOLO_LIMIT,
        /// <summary>
        /// Low EU
        /// </summary>
        LowEU = OPC_PROPERTY_LOW_EU,
        /// <summary>
        /// Low Instrument Range
        /// </summary>
        LowInstrumentRange = OPC_PROPERTY_LOW_IR,
        /// <summary>
        /// Contact Open Label
        /// </summary>
        ContactOpenLabel = OPC_PROPERTY_OPEN_LABEL,
        /// <summary>
        /// Primary Alarm Area
        /// </summary>
        PrimaryAlarmArea = OPC_PROPERTY_PRIMARY_ALARM_AREA,
        /// <summary>
        /// Item Quality
        /// </summary>
        ItemQuality = OPC_PROPERTY_QUALITY,
        /// <summary>
        /// Server Scan Rate
        /// </summary>
        ServerScanRate = OPC_PROPERTY_SCAN_RATE,
        /// <summary>
        /// Sound File
        /// </summary>
        SoundFile = OPC_PROPERTY_SOUND_FILE,
        /// <summary>
        /// Item Timestamp
        /// </summary>
        ItemTimestamp = OPC_PROPERTY_TIMESTAMP,
        /// <summary>
        /// Item Timezone
        /// </summary>
        ItemTimezone = OPC_PROPERTY_TIMEZONE,
        /// <summary>
        /// Type Description
        /// </summary>
        TypeDescription = OPC_PROPERTY_TYPE_DESCRIPTION,
        /// <summary>
        /// Type ID
        /// </summary>
        TypeID = OPC_PROPERTY_TYPE_ID,
        /// <summary>
        /// Type System ID
        /// </summary>
        TypeSystemID = OPC_PROPERTY_TYPE_SYSTEM_ID,
        /// <summary>
        /// Unconverted Item ID
        /// </summary>
        UnconvertedItemID = OPC_PROPERTY_UNCONVERTED_ITEM_ID,
        /// <summary>
        /// Unfiltered Item ID
        /// </summary>
        UnfilteredItemID = OPC_PROPERTY_UNFILTERED_ITEM_ID,
        /// <summary>
        /// Item Value
        /// </summary>
        ItemValue = OPC_PROPERTY_VALUE,
        /// <summary>
        /// Write Behavior
        /// </summary>
        WriteBehavior = OPC_PROPERTY_WRITE_BEHAVIOR
    };

	/// <summary>
	/// Contains a helper functions.
	/// </summary>
	public ref class Helper
	{
	public:
		/// <summary>
		/// Converts a Guid object to a native GUID structure.
		/// </summary>
		/// <param name="guid">A Guid object to convert.</param>
		/// <returns>A pointer to memory allocated in the local unmanaged heap to hold the resulting GUID structure.</returns>
		static IntPtr GuidToHGlobal(Guid guid)
		{
			IntPtr ptr = Marshal::AllocHGlobal(16);
			Marshal::Copy(guid.ToByteArray(), 0, ptr, 16);
			return ptr;
		}
		template < class T, class U > 
		static Boolean isinst(U u)
		{
			return dynamic_cast< T >(u) != nullptr;
		}
	};

} } } }