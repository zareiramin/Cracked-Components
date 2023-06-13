using System;
using System.Collections.Generic;
using System.Globalization;

namespace Graybox.OPC.ServerToolkit.CLRWrapper
{

    /// <summary>
    /// Provides data for the BeforeCreateInstance event.
    /// </summary>
    public class BeforeCreateInstanceArgs : EventArgs
    {
        private ErrorCodes m_handling_error;
        private Boolean m_aggregating;
        private Int32 m_clientid;
        /// <summary>
        /// Initializes a new instance of the BeforeCreateInstanceArgs class.
        /// </summary>
        public BeforeCreateInstanceArgs(Boolean aggregating, Int32 clientid)
        {
            m_aggregating = aggregating;
            m_clientid = clientid;
        }
        /// <summary>
        /// Gets a vaiable indicating whether the new OPC server instance is about
        /// to be created as part of an aggregate.
        /// </summary>
        public Boolean Aggregating
        {
            get { return m_aggregating; }
        }
        /// <summary>
        /// Gets or sets an arbitrary identifier of the calling client.
        /// The same ClientId will be passed later to a number of other events handlers.
        /// </summary>
        public Int32 ClientId
        {
            get { return m_clientid; }
            set { m_clientid = value; }
        }
        /// <summary>
        /// Gets or sets the event handing error. A value of ErrorCodes.Ok means success.
        /// </summary>
        public ErrorCodes EventHandlingError
        {
            get { return m_handling_error; }
            set { m_handling_error = value; }
        }
    };

    /// <summary>
    /// Provides data for the CreateInstance event.
    /// </summary>
    public class CreateInstanceArgs : System.EventArgs
    {
        private Int32 m_clientid;
        /// <summary>
        /// Initializes a new instance of the CreateInstanceArgs class.
        /// </summary>
        public CreateInstanceArgs(Int32 clientid)
        {
            m_clientid = clientid;
        }
        /// <summary>
        /// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
        /// </summary>
        public Int32 ClientId
        {
            get { return m_clientid; }
        }
    };

    /// <summary>
    /// Provides data for the DestroyInstance event.
    /// </summary>
    public class DestroyInstanceArgs : System.EventArgs
    {
        private Int32 m_clientid;
        /// <summary>
        /// Initializes a new instance of the DestroyInstanceArgs class.
        /// </summary>
        public DestroyInstanceArgs(Int32 clientid)
        {
            m_clientid = clientid;
        }
        /// <summary>
        /// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
        /// </summary>
        public Int32 ClientId
        {
            get { return m_clientid; }
        }
    }

    /// <summary>
    /// Provides data for the Lock event.
    /// </summary>
    public class LockArgs : System.EventArgs
    {
    }

    /// <summary>
    /// Provides data for the Unlock event.
    /// </summary>
    public class UnlockArgs : System.EventArgs
    {
    }

    /// <summary>
    /// Provides data for the ServerReleased event.
    /// </summary>
    public class ServerReleasedArgs : System.EventArgs
    {
        private Boolean m_suspend;
        /// <summary>
        /// Initializes a new instance of the ServerReleasedArgs class.
        /// </summary>
        public ServerReleasedArgs(Boolean suspend)
        {
            m_suspend = suspend;
        }
        /// <summary>
        /// Gets or sets a variable inidicating whether to suspend the OPC server class object.
        /// If set to true then no new OPC server's instances can be created later.
        /// </summary>
        public Boolean Suspend
        {
            get { return m_suspend; }
            set { m_suspend = value; }
        }
    }

    /// <summary>
    /// Provides data for the WriteItems event.
    /// </summary>
    public class WriteItemsArgs : System.EventArgs
    {
        private ItemId[] m_tags;
        private Object[] m_values;
        private Quality[] m_qualities;
        private FileTime[] m_timestamps;
        private ErrorCodes[] m_errors;
        private ErrorCodes m_mastererr;
        private Int32 m_lcid;
        private Int32 m_clientid;
        private Boolean m_copy;

        /// <summary>
        /// Initializes a new instance of the WriteItemsArgs class.
        /// </summary>
        public WriteItemsArgs(
            ItemId[] tags,
            Object[] values,
            Quality[] qualities,
            FileTime[] timestamps,
            Int32 lcid,
            Int32 clientid)
        {
            m_tags = tags;
            m_values = values;
            m_qualities = qualities;
            m_timestamps = timestamps;
            m_lcid = lcid;
            m_clientid = clientid;
            m_copy = true;
            m_errors = new ErrorCodes[m_tags.Length];
        }
        /// <summary>
        /// Gets the array of an OPC item identifiers. Your implementation must ignore all ItemIds[i] for which ItemIds[i].TagId is zero.
        /// You may force ItemIds[i].TagId to zero to prevent the corresponding tag to be written
        /// (if CopyToCache is set to false, then it will have no effect).
        /// </summary>
        public ItemId[] ItemIds
        {
            get { return m_tags; }
        }
        /// <summary>
        /// Gets the readonly array containing the new data values of a tags, identified by ItemIds.
        /// Values[i] can be null. It means that client don't want to write a data value of a tag,
        /// identified by the ItemIds[i].
        /// </summary>
        public Object[] Values
        {
            get { return m_values; }
        }
        /// <summary>
        /// Gets the readonly array containing the new qualities of a tags, identified by ItemIds.
        /// Qualities will be null if the client is using OPC DA2 interface. It this case
        /// QualitiesSpecified property is false.
        /// Qualities[i] can be Quality.Unspecified. It means that client don't want to write a quality of a tag,
        /// identified by the ItemIds[i].
        /// </summary>
        public Quality[] Qualities
        {
            get { return m_qualities; }
        }
        /// <summary>
        /// Gets the readonly array containing the new timestamps of a tags, identified by ItemIds.
        /// Timestamps will be null if the client is using OPC DA2 interface. It this case
        /// TimestampsSpecified property is false.
        /// Timestamps[i] can be FileTime.Unspecified. It means that client don't want to write a timestamp of a tag,
        /// identified by the ItemIds[i].
        /// </summary>
        public FileTime[] Timestamps
        {
            get { return m_timestamps; }
        }
        /// <summary>
        /// Gets the array of ErrorCodes indicating the success of the individual tag writes.
        /// Errors array is initialized with ErrorCodes.Ok values. If the error has occured while
        /// writing a tag, identified by ItemIds[i], then you sohuld store a code of this error
        /// to Errors[i] and set MasterError to ErrorCodes.False.
        /// </summary>
        public ErrorCodes[] Errors
        {
            get { return m_errors; }
        }
        /// <summary>
        /// Gets or sets a variable indicating whether the Toolkit must copy data from
        /// Values, Qualities, Timestamps and Errors to the OPC server cache.
        /// </summary>
        public Boolean CopyToCache
        {
            get { return m_copy; }
            set { m_copy = value; }
        }
        /// <summary>
        /// Get a variable that is true when the Qualities array is specified.
        /// If QualitiesSpecified is false then the Qualities property is null
        /// and your event handler must not use it.
        /// QualitiesSpecified can be false if the OPC client called a method
        /// of OPC DA2 interface, where writing the tag quality is not supported.
        /// </summary>
        public Boolean QualitiesSpecified
        {
            get { return (m_qualities != null); }
        }
        /// <summary>
        /// Get a variable that is true when the Timestamps array is specified.
        /// If TimestampsSpecified is false then the Timestamps property is null
        /// and your event handler must not use it.
        /// TimestampsSpecified can be false if the OPC client called a method
        /// of OPC DA2 interface, where writing the tag timestamp is not supported.
        /// </summary>
        public Boolean TimestampsSpecified
        {
            get { return (m_timestamps != null); }
        }
        /// <summary>
        /// Gets the length of the ItemIds array. 
        /// </summary>
        public Int32 Count
        {
            get { return m_tags.Length; }
        }
        /// <summary>
        /// Gets or sets the master error of a writing operation.
        /// MasterError must be set to ErrorCodes.False if some of the Errors were set to
        /// a value other than ErrorCodes.Ok.
        /// </summary>
        public ErrorCodes MasterError
        {
            get { return m_mastererr; }
            set { m_mastererr = value; }
        }
        /// <summary>
        /// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
        /// </summary>
        public Int32 ClientId
        {
            get { return m_clientid; }
        }
        /// <summary>
        /// Gets the CultureInfo object that should be used to process the Values.
        /// Culture corresponds to a value of the Locale property. If the Locale
        /// represents the default locale (LOCALE_SYSTEM_DEFAULT, LOCALE_USER_DEFAULT,
        /// or a sublanguage-neutral locale), then Culture gets the CultureInfo object,
        /// constructed from the LCID returned by ConvertDefaultLocale method called
        /// with that default locale as an argument.
        /// </summary>
        public CultureInfo Culture
        {
            get { return new CultureInfo(Helpers.ConvertDefaultLocale(m_lcid), false); }
        }
        /// <summary>
        /// Gets the locale identifier that should be used to process the Values.
        /// This value may be a default locale (LOCALE_SYSTEM_DEFAULT, LOCALE_USER_DEFAULT,
        /// or a sublanguage-neutral locale).
        /// </summary>
        public Int32 Locale
        {
            get { return m_lcid; }
        }
    };

    /// <summary>
    /// Provides data for the ReadItems event.
    /// </summary>
    public class ReadItemsArgs : System.EventArgs
    {
        private ItemId[] m_tags;
        private Object[] m_values;
        private Quality[] m_qualities;
        private FileTime[] m_timestamps;
        private ErrorCodes[] m_errors;
        private ErrorCodes m_mastererr;
        private ErrorCodes m_masterqual;
        private Int32 m_lcid;
        private Int32 m_clientid;
        private Boolean m_copy;
        private Boolean m_canreturnval;
        private Boolean m_partial;
        private static String m_ex1 = " property is not accesible because the refresh operation was requested by the OPC client.";
        /// <summary>
        /// Initializes a new instance of the ReadItemsArgs class.
        /// </summary>
        public ReadItemsArgs(
            ItemId[] tags,
            Boolean can_return_valus,
            Int32 lcid,
            Int32 clientid)
        {
            m_tags = tags;
            m_lcid = lcid;
            m_clientid = clientid;
            m_canreturnval = can_return_valus;
        }
        /// <summary>
        /// Gets the count of the OPC items being read.
        /// </summary>
        public Int32 Count
        {
            get { return m_tags.Length; }
        }
        /// <summary>
        /// Gets the array of an OPC item identifiers. Your implementation must ignore all ItemIds[i] for which ItemIds[i].TagId is zero.
        /// Some of ItemIds[i].TagId may be forced to zero if the event handler sets the ValuesReturnedPartial property to true
        /// (see ValuesReturnedPartial description for details).
        /// </summary>
        public ItemId[] ItemIds
        {
            get { return m_tags; }
        }
        /// <summary>
        /// Gets or sets an array of data values of the tags, identified by ItemIds.
        /// If the ValuesAcceptable property is false, then Values property is
        /// not accessible and must not be used.
        /// Values is initialized to null then the event handler is called.
        /// </summary>
        public Object[] Values
        {
            get
            {
                if (!m_canreturnval) throw new System.InvalidOperationException("Values" + m_ex1);
                return m_values;
            }
            set
            {
                if (!m_canreturnval) throw new System.InvalidOperationException("Values" + m_ex1);
                m_values = value;
            }
        }
        /// <summary>
        /// Gets or sets an array of qualities of the tags, identified by ItemIds.
        /// If the ValuesAcceptable property is false, then Qualities property is
        /// not accessible and must not be used.
        /// Qualities is initialized to null then the event handler is called.
        /// If you store a bad or an uncertain quality to Qualities[i], then you
        /// must set the MasterQuality to ErrorCodes.False.
        /// </summary>
        public Quality[] Qualities
        {
            get
            {
                if (!m_canreturnval) throw new System.InvalidOperationException("Qualities" + m_ex1);
                return m_qualities;
            }
            set
            {
                if (!m_canreturnval) throw new System.InvalidOperationException("Qualities" + m_ex1);
                m_qualities = value;
            }
        }
        /// <summary>
        /// Gets or sets an array of UTC timestamps of the tags, identified by ItemIds.
        /// If the ValuesAcceptable property is false, then Timestamps property is
        /// not accessible and must not be used.
        /// Timestamps is initialized to null then the event handler is called.
        /// </summary>
        public FileTime[] Timestamps
        {
            get
            {
                if (!m_canreturnval) throw new System.InvalidOperationException("Timestamps" + m_ex1);
                return m_timestamps;
            }
            set
            {
                if (!m_canreturnval) throw new System.InvalidOperationException("Timestamps" + m_ex1);
                m_timestamps = value;
            }
        }
        /// <summary>
        /// Gets or sets an array of errors of the tags, identified by ItemIds.
        /// If the ValuesAcceptable property is false, then Errors property is
        /// not accessible and must not be used.
        /// Errors is initialized to null then the event handler is called.
        /// Errors indicates the success of the individual tag reads. If some of Errors[i] is
        /// set to a value other than ErrorCodes.Ok, then you must set the MasterError to ErrorCodes.False.
        /// </summary>
        public ErrorCodes[] Errors
        {
            get
            {
                if (!m_canreturnval) throw new System.InvalidOperationException("Errors" + m_ex1);
                return m_errors;
            }
            set
            {
                if (!m_canreturnval) throw new System.InvalidOperationException("Errors" + m_ex1);
                m_errors = value;
            }
        }
        /// <summary>
        /// Gets or sets the master error of a reading operation.
        /// MasterError must be set to ErrorCodes.False if some of the Errors were set to
        /// a value other than ErrorCodes.Ok.
        /// </summary>
        public ErrorCodes MasterError
        {
            get { return m_mastererr; }
            set { m_mastererr = value; }
        }
        /// <summary>
        /// Gets or sets the master quality of a reading operation.
        /// MasterQuality must be set to ErrorCodes.False if some of the Qualities were set to
        /// a value representing a bad or an uncertain quality.
        /// </summary>
        public ErrorCodes MasterQuality
        {
            get { return m_masterqual; }
            set { m_masterqual = value; }
        }
        /// <summary>
        /// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
        /// </summary>
        public Int32 ClientId
        {
            get { return m_clientid; }
        }
        /// <summary>
        /// Gets the CultureInfo object that should be used to process the Values.
        /// Culture corresponds to a value of the Locale property. If the Locale
        /// represents the default locale (LOCALE_SYSTEM_DEFAULT, LOCALE_USER_DEFAULT,
        /// or a sublanguage-neutral locale), then Culture gets the CultureInfo object,
        /// constructed from the LCID returned by ConvertDefaultLocale method called
        /// with that default locale as an argument.
        /// </summary>
        public CultureInfo Culture
        {
            get { return new CultureInfo(Helpers.ConvertDefaultLocale(m_lcid), false); }
        }
        /// <summary>
        /// Gets the locale identifier that should be used to process the Values.
        /// This value may be a default locale (LOCALE_SYSTEM_DEFAULT, LOCALE_USER_DEFAULT,
        /// or a sublanguage-neutral locale).
        /// </summary>
        public Int32 Locale
        {
            get { return m_lcid; }
        }
        /// <summary>
        /// The event handler may set this property to true if it is needed to return the calling
        /// OPC client values, qualities and timestamps from Values, Qualities and Timestamps arrays
        /// for only a part of tags, identified by ItemIds, and pass values, qualities and timestamps
        /// from the server's cache for the rest of the tags.
        /// If ValuesReturnedPartial set to true, then the ItemIds[i].TagId must be forced to zero for
        /// each tag for which the event handler stored it's value, quality and timestamp into
        /// Value[i], Quality[i] and Timestamp[i].
        /// </summary>
        public Boolean ValuesReturnedPartial
        {
            get { return m_partial; }
            set { m_partial = value; }
        }
        /// <summary>
        /// The event handler must set this property to true if it placed something in
        /// Values, Qualities, Timestamps and Errors properties.
        /// Do not set ValuesReturned to true if ValuesAcceptable is false.
        /// </summary>
        public Boolean ValuesReturned
        {
            get { return m_copy; }
            set { m_copy = value; }
        }
        /// <summary>
        /// Gets the value that is true if the event handler may store somthing into
        /// Values, Qualities, Timestamps and Errors properties.
        /// Event may be invoked as result of the OPC client request to refresh tags
        /// of some OPC group form the DEVICE. Such request can be a call to IOPCAsyncIO2::Refresh2 or IOPCAsyncIO3::RefreshMaxAge. 
        /// It such case ValuesAcceptable will return false.
        /// </summary>
        public Boolean ValuesAcceptable
        {
            get { return m_canreturnval; }
        }
    }

    /// <summary>
    /// Provides data for the DataUpdate event.
    /// </summary>
    public class DataUpdateArgs : System.EventArgs
    {
        private ItemId[] m_tags;
        private Object[] m_values;
        private Quality[] m_qualities;
        private FileTime[] m_timestamps;
        private ErrorCodes[] m_errors;
        private ErrorCodes m_clientResult;
        private Int32 m_clientid;

        /// <summary>
        /// Initializes a new instance of the DataUpdateArgs class.
        /// </summary>
        public DataUpdateArgs(
            ItemId[] tags,
            Object[] values,
            Quality[] qualities,
            FileTime[] timestamps,
            ErrorCodes[] errors,
            ErrorCodes clientResult,
            Int32 clientid)
        {
            m_tags = tags;
            m_values = values;
            m_qualities = qualities;
            m_timestamps = timestamps;
            m_errors = errors;
            m_clientResult = clientResult;
            m_clientid = clientid;
        }
        /// <summary>
        /// Gets the readonly array of OPC item identifiers. Your implementation must ignore all ItemIds[i] for which ItemIds[i].TagId is zero.
        /// </summary>
        public ItemId[] ItemIds
        {
            get { return m_tags; }
        }
        /// <summary>
        /// Gets the readonly array containing the data values of tags, identified by ItemIds.
        /// </summary>
        public Object[] Values
        {
            get { return m_values; }
        }
        /// <summary>
        /// Gets the readonly array containing the qualities of tags, identified by ItemIds.
        /// </summary>
        public Quality[] Qualities
        {
            get { return m_qualities; }
        }
        /// <summary>
        /// Gets the readonly array containing the timestamps of tags, identified by ItemIds.
        /// </summary>
        public FileTime[] Timestamps
        {
            get { return m_timestamps; }
        }
        /// <summary>
        /// Gets the readonly array of ErrorCodes indicating the success of the individual tag reading.
        /// </summary>
        public ErrorCodes[] Errors
        {
            get { return m_errors; }
        }
        /// <summary>
        /// Gets the length of the ItemIds array. 
        /// </summary>
        public Int32 Count
        {
            get { return m_tags.Length; }
        }
        /// <summary>
        /// Gets the error code returned by the OPC client after the processing of the data callback.
        /// </summary>
        public ErrorCodes ClientResult
        {
            get { return m_clientResult; }
        }
        /// <summary>
        /// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
        /// </summary>
        public Int32 ClientId
        {
            get { return m_clientid; }
        }
    };

    /// <summary>
    /// Provides data for the ActivateItems event.
    /// </summary>
    public class ActivateItemsArgs : System.EventArgs
    {
        private ItemId[] m_tags;
        /// <summary>
        /// Initializes a new instance of the ActivateItemsArgs class.
        /// </summary>
        public ActivateItemsArgs(ItemId[] tags)
        {
            m_tags = tags;
        }
        /// <summary>
        /// Gets the readonly array of an OPC item identifiers. Your implementation must ignore all ItemIds[i] for which ItemIds[i].TagId is zero.
        /// The rest values in ItemIds describes the tags that became active. 
        /// </summary>
        public ItemId[] ItemIds
        {
            get { return m_tags; }
        }
        /// <summary>
        /// Get the count of items in the <c>ItemIds</c> array.
        /// </summary>
        public int Count
        {
            get { return m_tags.Length; }
        }
    }

    /// <summary>
    /// Provides data for the DeactivateItems event.
    /// </summary>
    public class DeactivateItemsArgs : System.EventArgs
    {
        private ItemId[] m_tags;
        /// <summary>
        /// Initializes a new instance of the DeactivateItemsArgs class.
        /// </summary>
        public DeactivateItemsArgs(ItemId[] tags)
        {
            m_tags = tags;
        }
        /// <summary>
        /// Gets the readonly array of an OPC item identifiers. Your implementation must ignore all ItemIds[i] for which ItemIds[i].TagId is zero.
        /// The rest values in ItemIds describes the tags that became inactive. 
        /// </summary>
        public ItemId[] ItemIds
        {
            get { return m_tags; }
        }
        /// <summary>
        /// Get the count of items in the <c>ItemIds</c> array.
        /// </summary>
        public int Count
        {
            get { return m_tags.Length; }
        }
    }

    /// <summary>
    /// Provides data for the GetErrorString event.
    /// </summary>
    public class GetErrorStringArgs : EventArgs
    {
        private Int32 m_errorcode;
        private Int32 m_lcid;
        private String m_errorstring;
        private Int32 m_clientid;
        private ErrorCodes m_handling_error;
        /// <summary>
        /// Initializes a new instance of the GetErrorStringArgs class.
        /// </summary>
        public GetErrorStringArgs(Int32 errorcode, Int32 lcid, Int32 clientid)
        {
            m_errorcode = errorcode;
            m_lcid = lcid;
            m_errorstring = null;
            m_clientid = clientid;
        }
        /// <summary>
        /// Gets the error code for which the textual description is requested by the OPC client.
        /// </summary>
        public Int32 RequestedErrorCode
        {
            get { return m_errorcode; }
        }
        /// <summary>
        /// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
        /// </summary>
        public Int32 ClientId
        {
            get { return m_clientid; }
        }
        /// <summary>
        /// Gets or sets the textual description of the requested error code in the RequestedErrorCode property.
        /// The event handler must set this property to a string object with the error code description.
        /// If the event handler can't provide the error description, then it should set the EventHandlingError to a failure code.
        /// </summary>
        public String RequestedErrorString
        {
            get { return m_errorstring; }
            set { m_errorstring = value; }
        }
        /// <summary>
        /// Gets the CultureInfo object that can be used to determine the language of the error textual description.
        /// Culture corresponds to a value of the Locale property. If the Locale
        /// represents the default locale (LOCALE_SYSTEM_DEFAULT, LOCALE_USER_DEFAULT,
        /// or a sublanguage-neutral locale), then Culture gets the CultureInfo object,
        /// constructed from the LCID returned by ConvertDefaultLocale method called
        /// with that default locale as an argument.
        /// </summary>
        public CultureInfo Culture
        {
            get { return new CultureInfo(Helpers.ConvertDefaultLocale(m_lcid), false); }
        }
        /// <summary>
        /// Gets the locale identifier that can be used to determine the language of the error textual description.
        /// This value may be a default locale (LOCALE_SYSTEM_DEFAULT, LOCALE_USER_DEFAULT,
        /// or a sublanguage-neutral locale).
        /// </summary>
        public Int32 Locale
        {
            get { return m_lcid; }
        }
        /// <summary>
        /// Gets or sets the event handing error. A value of ErrorCodes.Ok means success.
        /// </summary>
        public ErrorCodes EventHandlingError
        {
            get { return m_handling_error; }
            set { m_handling_error = value; }
        }
    }

	/// <summary>
	/// Provides data for the QueryLocales event.
	/// </summary>
    public class QueryLocalesArgs : EventArgs
	{
    	private	CulturesList m_cultures;
        private ErrorCodes m_handling_error;
        /// <summary>
        /// Initializes a new instance of the GetErrorStringArgs class.
        /// </summary>
	    public QueryLocalesArgs(CulturesList cultures)
		{
			m_cultures = cultures;
		}
		/// <summary>
		/// Gets or sets the CulturesList object that holds the list of the cultures,
		/// supported by the OPC server.
		/// </summary>
		public CulturesList Cultures
		{
			get { return m_cultures; }
			set { m_cultures = value; }
		}
        /// <summary>
        /// Gets or sets the event handing error. A value of ErrorCodes.Ok means success.
        /// </summary>
        public ErrorCodes EventHandlingError
        {
            get { return m_handling_error; }
            set { m_handling_error = value; }
        }
    }

	/// <summary>
	/// Provides data for the BrowseAccessPaths event.
	/// </summary>
    public class BrowseAccessPathsArgs : EventArgs
	{
	    private	String m_itemid;
	    private String[] m_paths;
		private Int32 m_clientid;
        private ErrorCodes m_handling_error;

        /// <summary>
        /// Initializes a new instance of the BrowseAccessPathsArgs class.
        /// </summary>
	    public BrowseAccessPathsArgs(String itemid, Int32 clientid)
		{
			m_itemid = itemid;
			m_clientid = clientid;
		}
		/// <summary>
		/// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
		/// </summary>
		public Int32 ClientId
		{
			get { return m_clientid; }
		}
		/// <summary>
		/// Gets the fully qualified name of a tag for which the access paths are requested by the OPC client.
		/// </summary>
		public String TagName
		{
			get { return m_itemid; }
		}
		/// <summary>
		/// Gets or sets an array of access paths.
		/// The event handler must set this property or set the EventHandlingError to a failure code.
		/// </summary>
		public String[] AccessPaths
		{
			get { return m_paths; }
			set { m_paths = value; }
		}
        /// <summary>
        /// Gets or sets the event handing error. A value of ErrorCodes.Ok means success.
        /// </summary>
        public ErrorCodes EventHandlingError
        {
            get { return m_handling_error; }
            set { m_handling_error = value; }
        }
    }

	/// <summary>
	/// Provides data for the QueryItem event.
	/// </summary>
    public class QueryItemArgs : EventArgs
	{
	    private	String m_itemid;
		private	String m_accpath;
		private	Int32 m_clientid;
		private	Int16 m_reqtype;
		private	Boolean m_additem;
		private	Int32 m_tagid;
		private	Int32 m_accpathid;
        private ErrorCodes m_handling_error;

        /// <summary>
        /// Initializes a new instance of the QueryItemArgs class.
        /// </summary>
        public QueryItemArgs(String itemid, String accpath, Int16 reqtype, Boolean additem, Int32 clientid)
		{
			m_itemid = itemid;
			m_accpath = accpath;
			m_reqtype = reqtype;
			m_additem = additem;
			m_clientid = clientid;
		}
		/// <summary>
		/// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
		/// </summary>
		public Int32 ClientId
		{
			get { return m_clientid; }
		}
		/// <summary>
		/// Gets a fully qualified name of a tag that is bieng queried by the client.
		/// </summary>
		public String TagName
		{
			get { return m_itemid; }
		}
		/// <summary>
		/// Gets the access path of the queried OPC item.
		/// </summary>
		public String AccessPath
		{
			get { return m_accpath; }
		}
		/// <summary>
		/// Gets a value that is true if the event handler must create a tag, which name
		/// is specified in the ItemId.
		/// </summary>
		public Boolean CreateTag
		{
			get { return m_additem; }
		}
		/// <summary>
		/// Gets the VARTYPE representing the data type that is requested by the client.
		/// </summary>
		public Int16 DataType
		{
			get { return m_reqtype; }
		}
		/// <summary>
		/// Gets or sets the TagId of a tag.
		/// The event handler must set this property to a TagId identifier of a tag that
		/// was created by the event handler as result of the client's request.
		/// </summary>
		public Int32 TagId
		{
			get { return m_tagid; }
			set { m_tagid = value; }
		}
		/// <summary>
		/// Gets or sets the AccessPathId of a tag.
		/// You may set this property to an arbitrary identifier,
		/// that will be passed later to the other event handlers as an ItemIds[i].AccessPathId value.
		/// </summary>
		public Int32 AccessPathId
		{
			get { return m_accpathid; }
			set { m_accpathid = value; }
		}
        /// <summary>
        /// Gets or sets the event handing error. A value of ErrorCodes.Ok means success.
        /// </summary>
        public ErrorCodes EventHandlingError
        {
            get { return m_handling_error; }
            set { m_handling_error = value; }
        }
    }

	/// <summary>
	/// Provides data for the ReadProperties event.
	/// </summary>
	public class ReadPropertiesArgs : EventArgs
	{
	    private	ItemId m_tag;
		private Int32[] m_propids;
		private Int32 m_lcid;
		private Int32 m_clientid;
		private Object[] m_values;
		private ErrorCodes[] m_errors;
        private ErrorCodes m_handling_error;

        /// <summary>
        /// Initializes a new instance of the ReadPropertiesArgs class.
        /// </summary>
	    public ReadPropertiesArgs(ItemId tag, Int32[] propids, ErrorCodes[] errs, Int32 lcid, Int32 clientid)
		{
			m_tag = tag;
			m_propids = propids;
			m_lcid = lcid;
			m_clientid = clientid;
			m_errors = errs;
			m_values = new Object[propids.Length];
			EventHandlingError = ErrorCodes.False;
		}
        /// <summary>
        /// Gets or sets the event handing error. A value of ErrorCodes.Ok means success.
        /// </summary>
        public ErrorCodes EventHandlingError
        {
            get { return m_handling_error; }
            set { m_handling_error = value; }
        }
		/// <summary>
		/// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
		/// </summary>
		public Int32 ClientId
		{
			get { return m_clientid; }
		}
		/// <summary>
		/// Gets the CultureInfo object that can be used to determine the language of the property value (if it is a textual value).
		/// Culture corresponds to a value of the Locale property. If the Locale
		/// represents the default locale (LOCALE_SYSTEM_DEFAULT, LOCALE_USER_DEFAULT,
		/// or a sublanguage-neutral locale), then Culture gets the CultureInfo object,
		/// constructed from the LCID returned by ConvertDefaultLocale method called
		/// with that default locale as an argument.
		/// </summary>
		public CultureInfo Culture
		{
			get { return new CultureInfo(Helpers.ConvertDefaultLocale(m_lcid), false); }
		}
		/// <summary>
		/// Gets the locale identifier that can be used to determine the language of the property value (if it is a textual value).
		/// This value may be a default locale (LOCALE_SYSTEM_DEFAULT, LOCALE_USER_DEFAULT,
		/// or a sublanguage-neutral locale).
		/// </summary>
		public Int32 Locale
		{
			get { return m_lcid; }
		}
		/// <summary>
		/// Gets the array containing the PropID identifiers of the properties those values are requested.
		/// </summary>
		public Int32[] Properties
		{
			get { return m_propids; }
		}
		/// <summary>
		/// Gets the ItemId structure that identifies the tag for which the properties values are requested.
		/// </summary>
		public ItemId Item
		{
			get { return m_tag; }
		}
		/// <summary>
		/// Gets the length of the Properties array.
		/// </summary>
		public Int32 Count
		{
			get { return m_propids.Length; }
		}
		/// <summary>
		/// Gets an array object there the propties values sohuld be stored.
		/// The values must me storted in their canonical data types.
		/// </summary>
		public Object[] Values
		{
			get { return m_values; }
		}
		/// <summary>
		/// Gets an array object there the individual reading return codes for each property should be stored.
		/// Before the handler is invoked, Errors is filled with ErrorCodes.InvalidPropertyId values.
		/// If your handler processes a property with the PropID of Properties[i], then it must store
		/// it's value to Values[i] and store the reading error to Error[i].
		/// All properties that were not processed by the handler (the corresponding Error[i] is still
		/// ErrorCodes.InvalidPropertyId) will be processed by the Toolkit.
		/// </summary>
		public ErrorCodes[] Errors
		{
			get { return m_errors;	}
		}
	}
}
