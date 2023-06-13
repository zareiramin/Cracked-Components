#pragma once

#include "Helper.h"

using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;
using namespace System::Globalization;
using namespace System::Runtime::InteropServices;
using namespace System::Threading;
using namespace Graybox::OPC;

namespace Graybox { namespace OPC { namespace ServerToolkit { namespace CLRWrapper
{
	/// <summary>
	/// Represents a collection of the CultureInfo objects.
	/// </summary>
	public ref class CulturesList sealed:
		public System::Collections::IList,
		public System::Collections::ICollection,
		public System::Collections::IEnumerable
	{
	private:
		ArrayList^ m_cultures;
		IntPtr m_unm_cultures;
        /// <summary>
        /// Updates the unmanaged array of the LCIDs with the data from the m_cultures.
        /// </summary>
		void UpdateUnmanaged()
		{
			if (m_unm_cultures != IntPtr::Zero)
				Marshal::FreeHGlobal(m_unm_cultures);
			m_unm_cultures = Marshal::AllocHGlobal(sizeof(LCID) * (Count + 3));
			Marshal::WriteInt32(m_unm_cultures, sizeof(LCID)*0, LOCALE_NEUTRAL);
			Marshal::WriteInt32(m_unm_cultures, sizeof(LCID)*1, MAKELCID(MAKELANGID(LANG_ENGLISH, SUBLANG_NEUTRAL), SORT_DEFAULT));
			Marshal::WriteInt32(m_unm_cultures, sizeof(LCID)*2, LOCALE_SYSTEM_DEFAULT);
			for (Int32 i = 0; i<Count; i++)
			{
				Marshal::WriteInt32(m_unm_cultures, sizeof(LCID)*(i + 3), ((CultureInfo^)m_cultures[i])->LCID);
			}
		}
	public:
        /// <summary>
        /// Initializes a new instance of the <c>CulturesList</c> class.
        /// </summary>
		CulturesList()
		{
			m_cultures = gcnew ArrayList();
			UpdateUnmanaged();
		}
        /// <summary>
        /// Initializes a new instance of the <c>CulturesList</c> class using the given array of
        /// the <c>CultureInfo</c> objects.
        /// </summary>
        /// <param name="arr"></param>
		CulturesList(array<CultureInfo^, 1>^ arr)
		{
			m_unm_cultures = IntPtr::Zero;
			m_cultures = gcnew ArrayList(arr);
			UpdateUnmanaged();
		}
        /// <summary>
        /// Frees the unmanaged resources.
        /// </summary>
		~CulturesList()
		{
			if (m_unm_cultures != IntPtr::Zero)
				Marshal::FreeHGlobal(m_unm_cultures);
		}
        /// <summary>
        /// Gets the pointer of the unmanaged array that holds the LCIDs of
        /// the locales supported by the OPC server.
        /// </summary>
		property IntPtr Unmanaged
		{
			IntPtr get() { return m_unm_cultures; }
		}
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
		virtual property Object^ default[Int32]
		{
			Object^ get(Int32 index) { return m_cultures[index]; }
			void set(Int32 index, Object^ value)
			{
				if (dynamic_cast<CultureInfo^>(value) == nullptr) throw gcnew ArgumentException("Value must be an instance of CultureInfo type.", "value");
				m_cultures[index] = value;
				UpdateUnmanaged();
			}
		}
        /// <summary>
        /// Gets the number of elements contained in the <c>ICollection</c>.
        /// </summary>
		virtual property Int32 Count { Int32 get() { return m_cultures->Count; } }	
        /// <summary>
        /// Gets a value indicating whether access to the <c>ICollection</c> is synchronized (thread safe).
        /// </summary>
		virtual property bool IsSynchronized { bool get() { return m_cultures->IsSynchronized; } }
        /// <summary>
        /// Gets an object that can be used to synchronize access to the <c>ICollection</c>. 
        /// </summary>
		virtual property Object^ SyncRoot { Object^ get() { return m_cultures->SyncRoot; } }
        /// <summary>
        /// Gets a value indicating whether the <c>IList</c> is read-only.
        /// </summary>
		virtual property bool IsReadOnly { bool get() { return m_cultures->IsReadOnly; } }
        /// <summary>
        /// Gets a value indicating whether the <c>IList</c> has a fixed size.
        /// </summary>
		virtual property bool IsFixedSize { bool get() { return m_cultures->IsFixedSize; } }
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
		virtual System::Collections::IEnumerator^ GetEnumerator() { return m_cultures->GetEnumerator(); }
        /// <summary>
        /// Copies the elements of the <c>ICollection</c> to an <c>Array</c>, starting at a particular <c>Array</c> index.
        /// </summary>
		virtual void CopyTo(System::Array^ arr, Int32 arrIndex) { m_cultures->CopyTo(arr, arrIndex); }
        /// <summary>
        /// Determines whether the <c>IList</c> contains a specific value.
        /// </summary>
		virtual bool Contains(System::Object^ item) { return m_cultures->Contains(item); }
        /// <summary>
        /// Removes all items from the <c>IList</c>.
        /// </summary>
        virtual void Clear(void)
		{
			m_cultures->Clear();
			UpdateUnmanaged();
		}
        /// <summary>
        /// Adds an item to the <c>IList</c>.
        /// </summary>
        virtual Int32 Add(System::Object^ item)
		{
			Int32 r = m_cultures->Add((CultureInfo^)item);
			UpdateUnmanaged();
			return r;
		}
        /// <summary>
        /// Determines the index of a specific item in the <c>IList</c>.
        /// </summary>
        virtual Int32 IndexOf(System::Object^ item)
		{
			Int32 r = m_cultures->IndexOf(item);
			UpdateUnmanaged();
			return r;
		}
        /// <summary>
        /// Inserts an item to the <c>IList</c> at the specified index.
        /// </summary>
        virtual void Insert(Int32 index, System::Object^ item)
		{
			m_cultures->Insert(index, item);
			UpdateUnmanaged();
		}
        /// <summary>
        /// Removes the first occurrence of a specific object from the <c>IList</c>.
        /// </summary>
        virtual void Remove(System::Object^ item)
		{
			m_cultures->Remove(item);
			UpdateUnmanaged();
		}
        /// <summary>
        /// Removes the <c>IList</c> item at the specified index.
        /// </summary>
        virtual void RemoveAt(Int32 index)
		{
			m_cultures->RemoveAt(index);
			UpdateUnmanaged();
		}
	};

#pragma region Enums used in events handling
	/// <summary>
	/// Event flags indexes and count declaration. Used internally.
	/// </summary>
	public enum GBDataAccessEventIdx
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

#pragma endregion

#pragma region Event arguments calsses

	/// <summary>
	/// Provides data for the Graybox.OPC.OPCDAServerEvents.BeforeCreateInstance event.
	/// </summary>
	public ref class BeforeCreateInstanceArgs : public EventArgs
	{
	private:
		Boolean m_aggregating;
		Int32 m_clientid;
		ErrorCodes m_handling_error;
	public:
        /// <summary>
        /// Initializes a new instance of the BeforeCreateInstanceArgs class.
        /// </summary>
		BeforeCreateInstanceArgs(Boolean aggregating, Int32 clientid)
		{
			m_handling_error = ErrorCodes::Ok;
			m_aggregating = aggregating;
			m_clientid = clientid;
		}
		/// <summary>
		/// Gets a vaiable indicating whether the new OPC server instance is about
		/// to be created as part of an aggregate.
		/// </summary>
		property Boolean Aggregating
		{
			Boolean get() { return m_aggregating; }
		}
		/// <summary>
		/// Gets or sets an arbitrary identifier of the calling client.
		/// The same ClientId will be passed later to a number of other events handlers.
		/// </summary>
		property Int32 ClientId
		{
			Int32 get() { return m_clientid; }
			void set(Int32 val) { m_clientid = val; }
		}
		/// <summary>
		/// Gets or sets the event handing error. A value of ErrorCodes.Ok means success.
		/// </summary>
		property ErrorCodes EventHandlingError
		{
			ErrorCodes get() { return m_handling_error; }
			void set(ErrorCodes val) { m_handling_error = val; }
		}	
	};

	/// <summary>
	/// Provides data for the Graybox.OPC.OPCDAServerEvents.CreateInstance event.
	/// </summary>
	public ref class CreateInstanceArgs : public System::EventArgs
	{
	private:
		Int32 m_clientid;
	public:
        /// <summary>
        /// Initializes a new instance of the CreateInstanceArgs class.
        /// </summary>
		CreateInstanceArgs(Int32 clientid)
		{
			m_clientid = clientid;
		}
		/// <summary>
		/// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
		/// </summary>
		property Int32 ClientId
		{
			Int32 get() { return m_clientid; }
		}
	};

	/// <summary>
	/// Provides data for the Graybox.OPC.OPCDAServerEvents.DestroyInstance event.
	/// </summary>
	public ref class DestroyInstanceArgs : public System::EventArgs
	{
	private:
		Int32 m_clientid;
	public:
        /// <summary>
        /// Initializes a new instance of the DestroyInstanceArgs class.
        /// </summary>
		DestroyInstanceArgs(Int32 clientid)
		{
			m_clientid = clientid;
		}
		/// <summary>
		/// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
		/// </summary>
		property Int32 ClientId
		{
			Int32 get() { return m_clientid; }
		}
	};

	/// <summary>
	/// Provides data for the Graybox.OPC.OPCDAServerEvents.Lock event.
	/// </summary>
	public ref class LockArgs : public System::EventArgs
	{
	};

	/// <summary>
	/// Provides data for the Graybox.OPC.OPCDAServerEvents.Unlock event.
	/// </summary>
	public ref class UnlockArgs : public System::EventArgs
	{
	};

	/// <summary>
	/// Provides data for the Graybox.OPC.OPCDAServerEvents.ServerReleased event.
	/// </summary>
	public ref class ServerReleasedArgs : public System::EventArgs
	{
	private:
		Boolean m_suspend;
	public:
        /// <summary>
        /// Initializes a new instance of the ServerReleasedArgs class.
        /// </summary>
		ServerReleasedArgs(Boolean suspend)
		{
			m_suspend = suspend;
		}
		/// <summary>
		/// Gets or sets a variable inidicating whether to suspend the OPC server class object.
		/// If set to true then no new OPC server's instances can be created later.
		/// </summary>
		property Boolean Suspend
		{
			Boolean get() { return m_suspend; }
			void set(Boolean val) { m_suspend = val; };
		}
	};

	/// <summary>
	/// Provides data for the Graybox.OPC.OPCDAServerEvents.WriteItems event.
	/// </summary>
	public ref class WriteItemsArgs : public System::EventArgs
	{
	private:
		array<ItemId>^ m_tags;
		array<Object^>^ m_values;
		array<Quality>^ m_qualities;
		array<FileTime>^ m_timestamps;
		array<ErrorCodes>^ m_errors;
		ErrorCodes m_mastererr;
		Int32 m_lcid;
		Int32 m_clientid;
		Boolean m_copy;
	public:
        /// <summary>
        /// Initializes a new instance of the WriteItemsArgs class.
        /// </summary>
		WriteItemsArgs(
			array<ItemId>^ tags,
			array<Object^>^ values,
			array<Quality>^ qualities,
			array<FileTime>^ timestamps,
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
			m_errors = gcnew array<ErrorCodes>(m_tags->Length);
			// zzz need to zero m_errors??
			//m_mastererr = 0;
		}
		/// <summary>
		/// Gets the array of an OPC item identifiers. Your implementation must ignore all ItemIds[i] for which ItemIds[i].TagId is zero.
		/// You may force ItemIds[i].TagId to zero to prevent the corresponding tag to be written
		/// (if CopyToCache is set to false, then it will have no effect).
		/// </summary>
		property array<ItemId>^ ItemIds
		{
			array<ItemId>^ get() { return m_tags; }
		}
		/// <summary>
		/// Gets the readonly array containing the new data values of a tags, identified by ItemIds.
		/// Values[i] can be null. It means that client don't want to write a data value of a tag,
		/// identified by the ItemIds[i].
		/// </summary>
		property array<Object^>^ Values
		{
			array<Object^>^ get() { return m_values; }
		}
		/// <summary>
		/// Gets the readonly array containing the new qualities of a tags, identified by ItemIds.
		/// Qualities will be null if the client is using OPC DA2 interface. It this case
		/// QualitiesSpecified property is false.
		/// Qualities[i] can be Quality.Unspecified. It means that client don't want to write a quality of a tag,
		/// identified by the ItemIds[i].
		/// </summary>
		property array<Quality>^ Qualities
		{
			array<Quality>^ get() { return m_qualities; }
		}
		/// <summary>
		/// Gets the readonly array containing the new timestamps of a tags, identified by ItemIds.
		/// Timestamps will be null if the client is using OPC DA2 interface. It this case
		/// TimestampsSpecified property is false.
		/// Timestamps[i] can be FileTime.Unspecified. It means that client don't want to write a timestamp of a tag,
		/// identified by the ItemIds[i].
		/// </summary>
		property array<FileTime>^ Timestamps
		{
			array<FileTime>^ get() { return m_timestamps; }
		}
		/// <summary>
		/// Gets the array of ErrorCodes indicating the success of the individual tag writes.
		/// Errors array is initialized with ErrorCodes.Ok values. If the error has occured while
		/// writing a tag, identified by ItemIds[i], then you sohuld store a code of this error
		/// to Errors[i] and set MasterError to ErrorCodes.False.
		/// </summary>
		property array<ErrorCodes>^ Errors
		{
			array<ErrorCodes>^ get() { return m_errors; }
		}
		/// <summary>
		/// Gets or sets a variable indicating whether the Toolkit must copy data from
		/// Values, Qualities, Timestamps and Errors to the OPC server cache.
		/// </summary>
		property Boolean CopyToCache
		{
			Boolean get() { return m_copy; }
			void set(Boolean val) { m_copy = val; }
		}
		/// <summary>
		/// Get a variable that is true when the Qualities array is specified.
		/// If QualitiesSpecified is false then the Qualities property is null
		/// and your event handler must not use it.
		/// QualitiesSpecified can be false if the OPC client called a method
		/// of OPC DA2 interface, where writing the tag quality is not supported.
		/// </summary>
		property Boolean QualitiesSpecified
		{
			Boolean get() { return (m_qualities != nullptr); }
		}
		/// <summary>
		/// Get a variable that is true when the Timestamps array is specified.
		/// If TimestampsSpecified is false then the Timestamps property is null
		/// and your event handler must not use it.
		/// TimestampsSpecified can be false if the OPC client called a method
		/// of OPC DA2 interface, where writing the tag timestamp is not supported.
		/// </summary>
		property Boolean TimestampsSpecified
		{
			Boolean get() { return (m_timestamps != nullptr); }
		}
		/// <summary>
		/// Gets the length of the ItemIds array. 
		/// </summary>
		property Int32 Count
		{
			Int32 get() { return m_tags->Length; }
		}
		/// <summary>
		/// Gets or sets the master error of a writing operation.
		/// MasterError must be set to ErrorCodes.False if some of the Errors were set to
		/// a value other than ErrorCodes.Ok.
		/// </summary>
		property ErrorCodes MasterError
		{
			ErrorCodes get() { return m_mastererr; }
			void set(ErrorCodes val) { m_mastererr = val; }
		}
		/// <summary>
		/// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
		/// </summary>
		property Int32 ClientId
		{
			Int32 get() { return m_clientid; }
		}
		/// <summary>
		/// Gets the CultureInfo object that should be used to process the Values.
		/// Culture corresponds to a value of the Locale property. If the Locale
		/// represents the default locale (LOCALE_SYSTEM_DEFAULT, LOCALE_USER_DEFAULT,
		/// or a sublanguage-neutral locale), then Culture gets the CultureInfo object,
		/// constructed from the LCID returned by ConvertDefaultLocale method called
		/// with that default locale as an argument.
		/// </summary>
		property CultureInfo^ Culture
		{
			CultureInfo^ get() { return gcnew CultureInfo(ConvertDefaultLocale(m_lcid), false); }
		}
		/// <summary>
		/// Gets the locale identifier that should be used to process the Values.
		/// This value may be a default locale (LOCALE_SYSTEM_DEFAULT, LOCALE_USER_DEFAULT,
		/// or a sublanguage-neutral locale).
		/// </summary>
		property Int32 Locale
		{
			Int32 get() { return m_lcid; }
		}
	};

	/// <summary>
	/// Provides data for the Graybox.OPC.OPCDAServerEvents.ReadItems event.
	/// </summary>
	public ref class ReadItemsArgs : public System::EventArgs
	{
	private:
		array<ItemId>^ m_tags;
		array<Object^>^ m_values;
		array<Quality>^ m_qualities;
		array<FileTime>^ m_timestamps;
		array<ErrorCodes>^ m_errors;
		ErrorCodes m_mastererr;
		ErrorCodes m_masterqual;
		Int32 m_lcid;
		Int32 m_clientid;
		Boolean m_copy;
		Boolean m_canreturnval;
		Boolean m_partial;
		static String^ m_ex1 = gcnew String(" property is not accesible because the refresh operation was requested by the OPC client.");
	public:
        /// <summary>
        /// Initializes a new instance of the ReadItemsArgs class.
        /// </summary>
		ReadItemsArgs(
			array<ItemId>^ tags,
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
		property Int32 Count
		{
			Int32 get() { return m_tags->Length; }
		}
		/// <summary>
		/// Gets the array of an OPC item identifiers. Your implementation must ignore all ItemIds[i] for which ItemIds[i].TagId is zero.
		/// Some of ItemIds[i].TagId may be forced to zero if the event handler sets the ValuesReturnedPartial property to true
		/// (see ValuesReturnedPartial description for details).
		/// </summary>
		property array<ItemId>^ ItemIds
		{
			array<ItemId>^ get() { return m_tags; }
		}
		/// <summary>
		/// Gets or sets an array of data values of the tags, identified by ItemIds.
		/// If the ValuesAcceptable property is false, then Values property is
		/// not accessible and must not be used.
		/// Values is initialized to null then the event handler is called.
		/// </summary>
		property array<Object^>^ Values
		{
			array<Object^>^ get()
			{
				if (!m_canreturnval) throw gcnew System::InvalidOperationException("Values" + m_ex1);
				return m_values;
			}
			void set(array<Object^>^ val)
			{
				if (!m_canreturnval) throw gcnew System::InvalidOperationException("Values" + m_ex1);
				m_values = val;
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
		property array<Quality>^ Qualities
		{
			array<Quality>^ get()
			{
				if (!m_canreturnval) throw gcnew System::InvalidOperationException("Qualities" + m_ex1);
				return m_qualities;
			}
			void set(array<Quality>^ val)
			{
				if (!m_canreturnval) throw gcnew System::InvalidOperationException("Qualities" + m_ex1);
				m_qualities  = val;
			}
		}
		/// <summary>
		/// Gets or sets an array of UTC timestamps of the tags, identified by ItemIds.
		/// If the ValuesAcceptable property is false, then Timestamps property is
		/// not accessible and must not be used.
		/// Timestamps is initialized to null then the event handler is called.
		/// </summary>
		property array<FileTime>^ Timestamps
		{
			array<FileTime>^ get()
			{
				if (!m_canreturnval) throw gcnew System::InvalidOperationException("Timestamps" + m_ex1);
				return m_timestamps;
			}
			void set(array<FileTime>^ val)
			{
				if (!m_canreturnval) throw gcnew System::InvalidOperationException("Timestamps" + m_ex1);
				m_timestamps = val;
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
		property array<ErrorCodes>^ Errors
		{
			array<ErrorCodes>^ get()
			{
				if (!m_canreturnval) throw gcnew System::InvalidOperationException("Errors" + m_ex1);
				return m_errors;
			}
			void set(array<ErrorCodes>^ val)
			{
				if (!m_canreturnval) throw gcnew System::InvalidOperationException("Errors" + m_ex1);
				m_errors = val;
			}
		}
		/// <summary>
		/// Gets or sets the master error of a reading operation.
		/// MasterError must be set to ErrorCodes.False if some of the Errors were set to
		/// a value other than ErrorCodes.Ok.
		/// </summary>
		property ErrorCodes MasterError
		{
			ErrorCodes get() { return m_mastererr; }
			void set(ErrorCodes val) { m_mastererr = val; }
		}
		/// <summary>
		/// Gets or sets the master quality of a reading operation.
		/// MasterQuality must be set to ErrorCodes.False if some of the Qualities were set to
		/// a value representing a bad or an uncertain quality.
		/// </summary>
		property ErrorCodes MasterQuality
		{
			ErrorCodes get() { return m_masterqual; }
			void set(ErrorCodes val) { m_masterqual = val; }
		}
		/// <summary>
		/// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
		/// </summary>
		property Int32 ClientId
		{
			Int32 get() { return m_clientid; }
		}
		/// <summary>
		/// Gets the CultureInfo object that should be used to process the Values.
		/// Culture corresponds to a value of the Locale property. If the Locale
		/// represents the default locale (LOCALE_SYSTEM_DEFAULT, LOCALE_USER_DEFAULT,
		/// or a sublanguage-neutral locale), then Culture gets the CultureInfo object,
		/// constructed from the LCID returned by ConvertDefaultLocale method called
		/// with that default locale as an argument.
		/// </summary>
		property CultureInfo^ Culture
		{
			CultureInfo^ get() { return gcnew CultureInfo(ConvertDefaultLocale(m_lcid), false); }
		}
		/// <summary>
		/// Gets the locale identifier that should be used to process the Values.
		/// This value may be a default locale (LOCALE_SYSTEM_DEFAULT, LOCALE_USER_DEFAULT,
		/// or a sublanguage-neutral locale).
		/// </summary>
		property Int32 Locale
		{
			Int32 get() { return m_lcid; }
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
		property Boolean ValuesReturnedPartial
		{
			Boolean get() { return m_partial; }
			void set(Boolean val) { m_partial = val; }
		}
		/// <summary>
		/// The event handler must set this property to true if it placed something in
		/// Values, Qualities, Timestamps and Errors properties.
		/// Do not set ValuesReturned to true if ValuesAcceptable is false.
		/// </summary>
		property Boolean ValuesReturned
		{
			Boolean get() { return m_copy; }
			void set(Boolean val) { m_copy = val; }
		}
		/// <summary>
		/// Gets the value that is true if the event handler may store somthing into
		/// Values, Qualities, Timestamps and Errors properties.
		/// Event may be invoked as result of the OPC client request to refresh tags
		/// of some OPC group form the DEVICE. Such request can be a call to IOPCAsyncIO2::Refresh2 or IOPCAsyncIO3::RefreshMaxAge. 
		/// It such case ValuesAcceptable will return false.
		/// </summary>
		property Boolean ValuesAcceptable
		{
			Boolean get() { return m_canreturnval; }
		}
	};

	/// <summary>
	/// Provides data for the Graybox.OPC.OPCDAServerEvents.DataUpdate event.
	/// </summary>
	public ref class DataUpdateArgs : public System::EventArgs
	{
	private:
		array<ItemId>^ m_tags;
		array<Object^>^ m_values;
		array<Quality>^ m_qualities;
		array<FileTime>^ m_timestamps;
		array<ErrorCodes>^ m_errors;
		Int32 m_clientResult;
		Int32 m_clientid;
	public:
        /// <summary>
        /// Initializes a new instance of the DataUpdateArgs class.
        /// </summary>
		DataUpdateArgs(
			array<ItemId>^ tags,
			array<Object^>^ values,
			array<Quality>^ qualities,
			array<FileTime>^ timestamps,
			array<ErrorCodes>^ errors,
			Int32 clientResult,
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
		property array<ItemId>^ ItemIds
		{
			array<ItemId>^ get() { return m_tags; }
		}
		/// <summary>
		/// Gets the readonly array containing the new data values of a tags, identified by ItemIds.
		/// Values[i] can be null. It means that client don't want to write a data value of a tag,
		/// identified by the ItemIds[i].
		/// </summary>
		property array<Object^>^ Values
		{
			array<Object^>^ get() { return m_values; }
		}
		/// <summary>
		/// Gets the readonly array containing the qualities of tags, identified by ItemIds.
		/// </summary>
		property array<Quality>^ Qualities
		{
			array<Quality>^ get() { return m_qualities; }
		}
		/// <summary>
		/// Gets the readonly array containing the timestamps of tags, identified by ItemIds.
		/// </summary>
		property array<FileTime>^ Timestamps
		{
			array<FileTime>^ get() { return m_timestamps; }
		}
        /// <summary>
        /// Gets the readonly array of ErrorCodes indicating the success of the individual tag reading.
        /// </summary>
		property array<ErrorCodes>^ Errors
		{
			array<ErrorCodes>^ get() { return m_errors; }
		}
		/// <summary>
		/// Gets the length of the ItemIds array. 
		/// </summary>
		property Int32 Count
		{
			Int32 get() { return m_tags->Length; }
		}
        /// <summary>
        /// Gets the error code returned by the OPC client after the processing of the data callback.
        /// </summary>
		property ErrorCodes ClientResult
		{
			ErrorCodes get() { return (ErrorCodes)m_clientResult; }
		}
		/// <summary>
		/// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
		/// </summary>
		property Int32 ClientId
		{
			Int32 get() { return m_clientid; }
		}
	};

	/// <summary>
	/// Provides data for the Graybox.OPC.OPCDAServerEvents.ActivateItems event.
	/// </summary>
	public ref class ActivateItemsArgs : public System::EventArgs
	{
	private:
		array<ItemId, 1>^ m_tags;
	public:
        /// <summary>
        /// Initializes a new instance of the ActivateItemsArgs class.
        /// </summary>
		ActivateItemsArgs(array<ItemId, 1>^ tags)
		{
			m_tags = tags;
		}
		/// <summary>
		/// Gets the readonly array of an OPC item identifiers. Your implementation must ignore all ItemIds[i] for which ItemIds[i].TagId is zero.
		/// The rest values in ItemIds describes the tags that became active. 
		/// </summary>
		property array<ItemId>^ ItemIds
		{
			array<ItemId, 1>^ get() { return m_tags; }
		}
        /// <summary>
        /// Get the count of items in the <c>ItemIds</c> array.
        /// </summary>
		property Int32 Count
		{
			Int32 get() { return m_tags->Length; }
		}
	};

	/// <summary>
	/// Provides data for the Graybox.OPC.OPCDAServerEvents.DeactivateItems event.
	/// </summary>
	public ref class DeactivateItemsArgs : public System::EventArgs
	{
	private:
		array<ItemId, 1>^ m_tags;
	public:
        /// <summary>
        /// Initializes a new instance of the DeactivateItemsArgs class.
        /// </summary>
		DeactivateItemsArgs(array<ItemId, 1>^ tags)
		{
			m_tags = tags;
		}
		/// <summary>
		/// Gets the readonly array of an OPC item identifiers. Your implementation must ignore all ItemIds[i] for which ItemIds[i].TagId is zero.
		/// The rest values in ItemIds describes the tags that became inactive. 
		/// </summary>
		property array<ItemId>^ ItemIds
		{
			array<ItemId, 1>^ get() { return m_tags; }
		}
        /// <summary>
        /// Get the count of items in the <c>ItemIds</c> array.
        /// </summary>
		property Int32 Count
		{
			Int32 get() { return m_tags->Length; }
		}
	};

	/// <summary>
	/// Provides data for the Graybox.OPC.OPCDAServerEvents.GetErrorString event.
	/// </summary>
	public ref class GetErrorStringArgs : public EventArgs
	{
	private:
		Int32 m_errorcode;
		Int32 m_lcid;
		String^ m_errorstring;
		Int32 m_clientid;
		ErrorCodes m_handling_error;
	public:
        /// <summary>
        /// Initializes a new instance of the GetErrorStringArgs class.
        /// </summary>
		GetErrorStringArgs(Int32 errorcode, Int32 lcid, Int32 clientid)
		{
			m_handling_error = ErrorCodes::Ok;
			m_errorcode = errorcode;
			m_lcid = lcid;
			m_errorstring = nullptr;
			m_clientid = clientid;
		}
		/// <summary>
		/// Gets the error code for which the textual description is requested by the OPC client.
		/// </summary>
		property Int32 RequestedErrorCode
		{
			Int32 get() { return m_errorcode; }
		}
		/// <summary>
		/// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
		/// </summary>
		property Int32 ClientId
		{
			Int32 get() { return m_clientid; }
		}
		/// <summary>
		/// Gets or sets the textual description of the requested error code in the RequestedErrorCode property.
		/// The event handler must set this property to a string object with the error code description.
		/// If the event handler can't provide the error description, then it should set the EventHandlingError to a failure code.
		/// </summary>
		property String^ RequestedErrorString
		{
			String^ get() { return m_errorstring; }
			void set(String^ val) { m_errorstring = val; }
		}
		/// <summary>
		/// Gets the CultureInfo object that can be used to determine the language of the error textual description.
		/// Culture corresponds to a value of the Locale property. If the Locale
		/// represents the default locale (LOCALE_SYSTEM_DEFAULT, LOCALE_USER_DEFAULT,
		/// or a sublanguage-neutral locale), then Culture gets the CultureInfo object,
		/// constructed from the LCID returned by ConvertDefaultLocale method called
		/// with that default locale as an argument.
		/// </summary>
		property CultureInfo^ Culture
		{
			CultureInfo^ get() { return gcnew CultureInfo(ConvertDefaultLocale(m_lcid), false); }
		}
		/// <summary>
		/// Gets the locale identifier that can be used to determine the language of the error textual description.
		/// This value may be a default locale (LOCALE_SYSTEM_DEFAULT, LOCALE_USER_DEFAULT,
		/// or a sublanguage-neutral locale).
		/// </summary>
		property Int32 Locale
		{
			Int32 get() { return m_lcid; }
		}
		/// <summary>
		/// Gets or sets the event handing error. A value of ErrorCodes.Ok means success.
		/// </summary>
		property ErrorCodes EventHandlingError
		{
			ErrorCodes get() { return m_handling_error; }
			void set(ErrorCodes val) { m_handling_error = val; }
		}	
	};

	/// <summary>
	/// Provides data for the Graybox.OPC.OPCDAServerEvents.QueryLocales event.
	/// </summary>
	public ref class QueryLocalesArgs : public EventArgs
	{
	private:
		CulturesList^ m_cultures;
		ErrorCodes m_handling_error;
	public:
        /// <summary>
        /// Initializes a new instance of the GetErrorStringArgs class.
        /// </summary>
		QueryLocalesArgs(CulturesList^ cultures)
		{
			m_handling_error = ErrorCodes::Ok;
			m_cultures = cultures;
		}
		/// <summary>
		/// Gets or sets the CulturesList object that holds the list of the cultures,
		/// supported by the OPC server.
		/// </summary>
		property CulturesList^ Cultures
		{
			CulturesList^ get() { return m_cultures; }
			void set(CulturesList^ val) { m_cultures = val; }
		}
		/// <summary>
		/// Gets or sets the event handing error. A value of ErrorCodes.Ok means success.
		/// </summary>
		property ErrorCodes EventHandlingError
		{
			ErrorCodes get() { return m_handling_error; }
			void set(ErrorCodes val) { m_handling_error = val; }
		}	
	};

	/// <summary>
	/// Provides data for the Graybox.OPC.OPCDAServerEvents.BrowseAccessPaths event.
	/// </summary>
	public ref class BrowseAccessPathsArgs : public EventArgs
	{
	private:
		String^ m_itemid;
		array<String^>^ m_paths;
		Int32 m_clientid;
		ErrorCodes m_handling_error;
	public:
        /// <summary>
        /// Initializes a new instance of the BrowseAccessPathsArgs class.
        /// </summary>
		BrowseAccessPathsArgs(String^ itemid, Int32 clientid)
		{
			m_handling_error = ErrorCodes::Ok;
			m_itemid = itemid;
			m_clientid = clientid;
		}
		/// <summary>
		/// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
		/// </summary>
		property Int32 ClientId
		{
			Int32 get() { return m_clientid; }
		}
		/// <summary>
		/// Gets the fully qualified name of a tag for which the access paths are requested by the OPC client.
		/// </summary>
		property String^ TagName
		{
			String^ get() { return m_itemid; }
		}
		/// <summary>
		/// Gets or sets an array of access paths.
		/// The event handler must set this property or set the EventHandlingError to a failure code.
		/// </summary>
		property array<String^>^ AccessPaths
		{
			array<String^>^ get() { return m_paths; }
			void set(array<String^>^ val) { m_paths = val; }
		}
		/// <summary>
		/// Gets or sets the event handing error. A value of ErrorCodes.Ok means success.
		/// </summary>
		property ErrorCodes EventHandlingError
		{
			ErrorCodes get() { return m_handling_error; }
			void set(ErrorCodes val) { m_handling_error = val; }
		}	
	};

	/// <summary>
	/// Provides data for the Graybox.OPC.OPCDAServerEvents.QueryItem event.
	/// </summary>
	public ref class QueryItemArgs : public EventArgs
	{
	private:
		String^ m_itemid;
		String^ m_accpath;
		Int32 m_clientid;
		Int16 m_reqtype;
		Boolean m_additem;
		Int32 m_tagid;
		Int32 m_accpathid;
		ErrorCodes m_handling_error;

	public:
        /// <summary>
        /// Initializes a new instance of the QueryItemArgs class.
        /// </summary>
		QueryItemArgs(String^ itemid, String^ accpath, Int16 reqtype, Boolean additem, Int32 clientid)
		{
			m_handling_error = ErrorCodes::Ok;
			m_itemid = itemid;
			m_accpath = accpath;
			m_reqtype = reqtype;
			m_additem = additem;
			m_clientid = clientid;
		}
		/// <summary>
		/// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
		/// </summary>
		property Int32 ClientId
		{
			Int32 get() { return m_clientid; }
		}
		/// <summary>
		/// Gets a fully qualified name of a tag that is bieng queried by the client.
		/// </summary>
		property String^ TagName
		{
			String^ get() { return m_itemid; }
		}
		/// <summary>
		/// Gets the access path of the queried OPC item.
		/// </summary>
		property String^ AccessPath
		{
			String^ get() { return m_accpath; }
		}
		/// <summary>
		/// Gets a value that is true if the event handler must create a tag, which name
		/// is specified in the ItemId.
		/// </summary>
		property Boolean CreateTag
		{
			Boolean get() { return m_additem; }
		}
		/// <summary>
		/// Gets the VARTYPE representing the data type that is requested by the client.
		/// </summary>
		property Int16 DataType
		{
			Int16 get() { return m_reqtype; }
		}
		/// <summary>
		/// Gets or sets the TagId of a tag.
		/// The event handler must set this property to a TagId identifier of a tag that
		/// was created by the event handler as result of the client's request.
		/// </summary>
		property Int32 TagId
		{
			Int32 get() { return m_tagid; }
			void set(Int32 val) { m_tagid = val; }
		}
		/// <summary>
		/// Gets or sets the AccessPathId of a tag.
		/// You may set this property to an arbitrary identifier,
		/// that will be passed later to the other event handlers as an ItemIds[i].AccessPathId value.
		/// </summary>
		property Int32 AccessPathId
		{
			Int32 get() { return m_accpathid; }
			void set(Int32 val) { m_accpathid = val; }
		}
		/// <summary>
		/// Gets or sets the event handing error. A value of ErrorCodes.Ok means success.
		/// </summary>
		property ErrorCodes EventHandlingError
		{
			ErrorCodes get() { return m_handling_error; }
			void set(ErrorCodes val) { m_handling_error = val; }
		}	
	};

	/// <summary>
	/// Provides data for the Graybox.OPC.OPCDAServerEvents.ReadProperties event.
	/// </summary>
	public ref class ReadPropertiesArgs : public EventArgs
	{
	private:
		ItemId m_tag;
		array<Int32>^ m_propids;
		Int32 m_lcid;
		Int32 m_clientid;
		ErrorCodes m_handling_error;

		array<Object^>^ m_values;
		array<ErrorCodes>^ m_errors;

	public:
        /// <summary>
        /// Initializes a new instance of the ReadPropertiesArgs class.
        /// </summary>
		ReadPropertiesArgs(ItemId tag, array<Int32>^ propids, array<ErrorCodes>^ errs, Int32 lcid, Int32 clientid)
		{
			m_handling_error = ErrorCodes::Ok;
			m_tag = tag;
			m_propids = propids;
			m_lcid = lcid;
			m_clientid = clientid;
			m_errors = errs;
			m_values = gcnew array<Object^>(propids->Length);
			EventHandlingError = ErrorCodes::False;
		}
		/// <summary>
		/// Gets the identifier of a calling client (ClientId), which was set earlier in the BeforeCreateInstance event handler.
		/// </summary>
		property Int32 ClientId
		{
			Int32 get() { return m_clientid; }
		}
		/// <summary>
		/// Gets the CultureInfo object that can be used to determine the language of the property value (if it is a textual value).
		/// Culture corresponds to a value of the Locale property. If the Locale
		/// represents the default locale (LOCALE_SYSTEM_DEFAULT, LOCALE_USER_DEFAULT,
		/// or a sublanguage-neutral locale), then Culture gets the CultureInfo object,
		/// constructed from the LCID returned by ConvertDefaultLocale method called
		/// with that default locale as an argument.
		/// </summary>
		property CultureInfo^ Culture
		{
			CultureInfo^ get() { return gcnew CultureInfo(ConvertDefaultLocale(m_lcid), false); }
		}
		/// <summary>
		/// Gets the locale identifier that can be used to determine the language of the property value (if it is a textual value).
		/// This value may be a default locale (LOCALE_SYSTEM_DEFAULT, LOCALE_USER_DEFAULT,
		/// or a sublanguage-neutral locale).
		/// </summary>
		property Int32 Locale
		{
			Int32 get() { return m_lcid; }
		}
		/// <summary>
		/// Gets the array containing the PropID identifiers of the properties those values are requested.
		/// </summary>
		property array<Int32>^ Properties
		{
			array<Int32>^ get() { return m_propids; }
		}
		/// <summary>
		/// Gets the ItemId structure that identifies the tag for which the properties values are requested.
		/// </summary>
		property ItemId Item
		{
			ItemId get() { return m_tag; }
		}
		/// <summary>
		/// Gets the length of the Properties array.
		/// </summary>
		property Int32 Count
		{
			Int32 get() { return m_propids->Length; }
		}
		/// <summary>
		/// Gets an array object there the propties values sohuld be stored.
		/// The values must me storted in their canonical data types.
		/// </summary>
		property array<Object^>^ Values
		{
			array<Object^>^ get() { return m_values; }
		}
		/// <summary>
		/// Gets an array object there the individual reading return codes for each property should be stored.
		/// Before the handler is invoked, Errors is filled with ErrorCodes.InvalidPropertyId values.
		/// If your handler processes a property with the PropID of Properties[i], then it must store
		/// it's value to Values[i] and store the reading error to Error[i].
		/// All properties that were not processed by the handler (the corresponding Error[i] is still
		/// ErrorCodes.InvalidPropertyId) will be processed by the Toolkit.
		/// </summary>
		property array<ErrorCodes>^ Errors
		{
			array<ErrorCodes>^ get() { return m_errors;	}
		}
		/// <summary>
		/// Gets or sets the event handing error. A value of ErrorCodes.Ok means success.
		/// </summary>
		property ErrorCodes EventHandlingError
		{
			ErrorCodes get() { return m_handling_error; }
			void set(ErrorCodes val) { m_handling_error = val; }
		}	
	};

#pragma endregion


	public delegate void BeforeCreateInstanceEventHandler(Object^ sender, BeforeCreateInstanceArgs^ e);
	public delegate void CreateInstanceEventHandler(Object^ sender, CreateInstanceArgs^ e);
	public delegate void DestroyInstanceEventHandler(Object^ sender, DestroyInstanceArgs^ e);
	public delegate void LockEventHandler(Object^ sender, LockArgs^ e);
	public delegate void UnlockEventHandler(Object^ sender, UnlockArgs^ e);
	public delegate void ServerReleasedEventHandler(Object^ sender, ServerReleasedArgs^ e);
	public delegate void WriteItemsEventHandler(Object^ sender, WriteItemsArgs^ e);
	public delegate void ReadItemsEventHandler(Object^ sender, ReadItemsArgs^ e);
	public delegate void DataUpdateEventHandler(Object^ sender, DataUpdateArgs^ e);
	public delegate void ActivateItemsEventHandler(Object^ sender, ActivateItemsArgs^ e);
	public delegate void DeactivateItemsEventHandler(Object^ sender, DeactivateItemsArgs^ e);
	public delegate void GetErrorStringEventHandler(Object^ sender, GetErrorStringArgs^ e);
	public delegate void QueryLocalesEventHandler(Object^ sender, QueryLocalesArgs^ e);
	public delegate void BrowseAccessPathsEventHandler(Object^ sender, BrowseAccessPathsArgs^ e);
	public delegate void QueryItemEventHandler(Object^ sender, QueryItemArgs^ e);
	public delegate void ReadPropertiesEventHandler(Object^ sender, ReadPropertiesArgs^ e);


	public ref class OPCDAServerEvents
	{
	private:
		BeforeCreateInstanceEventHandler^ m_BeforeCreateInstance;
		CreateInstanceEventHandler^ m_CreateInstance;
		DestroyInstanceEventHandler^ m_DestroyInstance;
		LockEventHandler^ m_Lock;
		UnlockEventHandler^ m_Unlock;
		ServerReleasedEventHandler^ m_ServerReleased;
		WriteItemsEventHandler^ m_WriteItems;
		ReadItemsEventHandler^ m_ReadItems;
		DataUpdateEventHandler^ m_DataUpdate;
		ActivateItemsEventHandler^ m_ActivateItems;
		DeactivateItemsEventHandler^ m_DeactivateItems;
		GetErrorStringEventHandler^ m_GetErrorString;
		QueryLocalesEventHandler^ m_QueryLocales;
		BrowseAccessPathsEventHandler^ m_BrowseAccessPaths;
		QueryItemEventHandler^ m_QueryItem;
		ReadPropertiesEventHandler^ m_ReadProperties;

		IntPtr m_p_event_flags;
		void SetEventFlag(Int32 flag_idx, Int32 value)
		{
			InterlockedExchange(((volatile LONG*)m_p_event_flags.ToPointer())+flag_idx, value);
		}
	public:
		OPCDAServerEvents(IntPtr p_event_flags)
		{
			m_BeforeCreateInstance = nullptr;
			m_CreateInstance = nullptr;
			m_DestroyInstance = nullptr;
			m_Lock = nullptr;
			m_Unlock = nullptr;
			m_ServerReleased = nullptr;
			m_WriteItems = nullptr;
			m_ReadItems = nullptr;
			m_DataUpdate = nullptr;
			m_ActivateItems = nullptr;
			m_DeactivateItems = nullptr;
			m_GetErrorString = nullptr;
			m_QueryLocales = nullptr;
			m_BrowseAccessPaths = nullptr;
			m_QueryItem = nullptr;
			m_ReadProperties = nullptr;
			m_p_event_flags = p_event_flags;
		}

		#define GBEvents_Event(name) \
			event name##EventHandler^ name \
			{ \
				void add(name##EventHandler^ d) \
				{ \
					m_##name += d; \
					SetEventFlag(EventIdx_##name, 1); \
				} \
				void remove(name##EventHandler^ d) \
				{ \
					m_##name -= d; \
					if (!m_##name) SetEventFlag(EventIdx_##name, 0); \
				} \
				void raise(Object^ o, name##Args^ e) \
				{ \
					m_##name->Invoke(o, e); \
				} \
			}

		/// <summary>
		/// <para>This event is invoked by the class factory immediately before the creation of a new OPC server instance.
		/// In its handler you may decide whether to allow the OPC client to connect to the OPC server.</para>
		/// <para>This event is a wrapper for the native GBDataAccessBase::GBOnBeforeCreateInstance callback.</para>
		/// </summary>
		GBEvents_Event(BeforeCreateInstance)

		/// <summary>
		/// <para>Occures when the new instance of an OPC server is created.</para>
		/// <para>This event is a wrapper for the native GBDataAccessBase::GBOnCreateInstance callback.</para>
		/// </summary>
		GBEvents_Event(CreateInstance)

		/// <summary>
		/// <para>Occures when the instance of an OPC server is destroyed.</para>
		/// <para>This event is a wrapper for the native GBDataAccessBase::GBOnDestroyInstance callback.</para>
		/// </summary>
		GBEvents_Event(DestroyInstance)

		/// <summary>
		/// <para>Invoked when the class factory lock counter is incremented.</para>
		/// <para>This event is a wrapper for the native GBDataAccessBase::GBOnLock callback.</para>
		/// </summary>
		GBEvents_Event(Lock)

		/// <summary>
		/// <para>Invoked when the class factory lock counter is decremented.</para>
		/// <para>This event is a wrapper for the native GBDataAccessBase::GBOnUnLock callback.</para>
		/// </summary>
		GBEvents_Event(Unlock)

		/// <summary>
		/// <para>Occures when the last instance of an OPC server is destroyed.
		/// It the ServerReleased event handler you can decide whether to allow
		/// the OPC clients to connect to the server later or to suspend the
		/// OPC server class object.</para>
		/// <para>This event is a wrapper for the native GBDataAccessBase::GBOnServerReleased callback.</para>
		/// </summary>
		GBEvents_Event(ServerReleased)

		/// <summary>
		/// <para>Invoked when the OPC client writes new data to the OPC server's tags.</para>
		/// <para>This event is a wrapper for the native GBDataAccessBase::GBOnWriteItems callback.</para>
		/// </summary>
		GBEvents_Event(WriteItems)

		/// <summary>
		/// <para>Occures when the OPC client requests tag reading from the DEVICE.
		/// Its handler must poll the underlying devices and place the new tags values into the OPC server cache.</para>
		/// <para>This event is a wrapper for the native GBDataAccessBase::GBOnReadItems callback.</para>
		/// </summary>
		GBEvents_Event(ReadItems)

        /// <summary>
        /// <para>Occures when the OPC server has sent a data update to the client.</para>
        /// <para>This event is a wrapper for the native GBDataAccessBase::GBDataUpdate callback.</para>
        /// <para>To enable this event, set <c>ServerOptions.NotifyDataUpdates</c> flag then you call
        /// <c>OPCDAServer.Initialize</c>.</para>
        /// <para>WARNING. Handling this event may cause a significant decrease in performance
        /// because of a massive data marshaling from native to managed code.</para>
        /// </summary>
		GBEvents_Event(DataUpdate)

		/// <summary>
		/// <para>Occures when some tags becomes active.
		/// It happens when the OPC client requests these tags to be periodically updated from
		/// the underlying devices and
		/// their values to be sent to the clients via IOPCDataCallback.</para>
		/// <para>This event is a wrapper for the native GBDataAccessBase::GBOnActivate callback.</para>
		/// </summary>
		GBEvents_Event(ActivateItems)

		/// <summary>
		/// <para>Occures when some tags becomes inactive.
		/// It happens when no more OPC clients are waiting for these tags to be updated
		/// and their values to be sent to the clients.</para>
		/// <para>This event is a wrapper for the native GBDataAccessBase::GBOnDeactivate callback.</para>
		/// </summary>
		GBEvents_Event(DeactivateItems)

		/// <summary>
		/// <para>Occures when the OPC client queries for the error description.</para>
		/// <para>This event is a wrapper for the native GBDataAccessBase::GBOnGetErrorString callback.</para>
		/// </summary>
		GBEvents_Event(GetErrorString)

		/// <summary>
		/// <para>Occures when the client requests a list of locales supported by the server.</para>
		/// <para>This event is a wrapper for the native GBDataAccessBase::GBOnQueryLocales callback.</para>
		/// </summary>
		GBEvents_Event(QueryLocales)

		/// <summary>
		/// <para>Occures when the OPC client requests a list of the tag's access paths.</para>
		/// <para>This event is a wrapper for the native GBDataAccessBase::GBOnBrowseAccessPath callback.</para>
		/// </summary>
		GBEvents_Event(BrowseAccessPaths)

		/// <summary>
		/// <para>Invoked when one of the following occures: 
		/// 1) The client tries to validate a tag that is not yet created.
		/// 2) The client tries to add a tag to a group, and this tag is not yet created.
		/// 3) The client tries to read or write a tag that is not yet created.
		/// 4) The client tries to read properties of a tag that is not yet created.</para>
		/// <para>This event is a wrapper for the native GBDataAccessBase::GBOnQueryItem callback.</para>
		/// </summary>
		GBEvents_Event(QueryItem)

		/// <summary>
		/// <para> Occures when the client reads the values of the tag's properties.</para>
		/// <para>This event is a wrapper for the native GBDataAccessBase::GBOnGetProperties callback.</para>
		/// </summary>
		GBEvents_Event(ReadProperties)
	};

	///////////////////////////////////////////////////////////////////////////
} } } }