#pragma once

#include "GBDataAccessNative.h"
#include "Events.h"
#include "Helper.h"

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Reflection;

namespace Graybox { namespace OPC { namespace ServerToolkit { namespace CLRWrapper
{

	/// <summary>
	/// A CLR wrapper for GBDataAccess native class.
	/// </summary>
	public ref class OPCDAServer
	{
	public:
		/// <summary>
		/// Gets the object that holds the event handlers of OPCDAServer object.
		/// </summary>
		property OPCDAServerEvents^ Events
		{
			OPCDAServerEvents^ get() { return m_gbda_native->GetEvents(); }
		}

	public:
        /// <summary>
        /// Initializes a new instance of the OPCDAServer class.
        /// </summary>
		OPCDAServer()
		{
			m_gbda_native = new GBDataAccessNative(this);
		}

		~OPCDAServer()
		{
			delete m_gbda_native;
		}

	#pragma region GBClassFactory methods

	#pragma endregion

	#pragma region GBDataAccessBase methods

		#pragma region GetTags
		/// <summary>
		/// <para>Reads tag values from OPC server cache.
		/// A wrapper for GBDataAccessBase::GBGetItems method.</para>
		/// <para>GetTags calls ThrowExceptionForHR with the resulting HRESULT returned by
		/// GBDataAccessBase::GBGetItems to throw an exception if GetTags method fails.</para>
		/// </summary>
		/// <param name="tagIds">An array of tag identifiers returned earlier by CreateTag method.</param>
		/// <param name="values">A variable which recieves an array of tags values.</param>
		virtual void GetTags(
			array<Int32>^ tagIds,
			array<Object^>^% values)
		{
			Int32 c = tagIds->Length;
			IntPtr unm_v = Marshal::AllocHGlobal( (sizeof(VARIANT)+sizeof(WORD)+sizeof(::FILETIME)+sizeof(HRESULT)) * c );
			IntPtr unm_q = IntPtr(unm_v.ToInt32() + (Int32)sizeof(VARIANT)*c);
			IntPtr unm_t = IntPtr(unm_q.ToInt32() + (Int32)sizeof(WORD)*c);
			IntPtr unm_e = IntPtr(unm_t.ToInt32() + (Int32)sizeof(::FILETIME)*c);
			pin_ptr<Int32> unm_i = &tagIds[0];

			Int32 hr = m_gbda_native->GBGetItems_Caller(
				(DWORD)c,
				(DWORD*)unm_i,
				(VARIANT*)unm_v.ToPointer(),
				(WORD*)unm_q.ToPointer(),
				(::FILETIME*)unm_t.ToPointer(),
				(HRESULT*)unm_e.ToPointer());
			if FAILED(hr)
			{
				Marshal::FreeHGlobal(unm_v);
				Marshal::ThrowExceptionForHR(hr);
			}
			try
			{
				values = Marshal::GetObjectsForNativeVariants(unm_v, c);
			}
			catch (Exception^ ex)
			{
				throw ex;
			}
			finally
			{
				m_gbda_native->GBGetItems_Cleanup(c, (VARIANT*)unm_v.ToPointer());
			}
		}

		/// <param name="tagIds">An array of tag identifiers returned earlier by CreateTag method.</param>
		/// <param name="values">A variable which recieves an array of tags values.</param>
		/// <param name="qualities">A variable which recieves an array of tags qualities.</param>
		/// <param name="timestamps">A variable which recieves an array of tags timestamps.</param>
		/// <param name="errors">A variable which recieves an array of tags errors.</param>
		virtual void GetTags(
			array<Int32>^ tagIds,
			array<Object^>^% values,
			array<Quality>^% qualities,
			array<FileTime>^% timestamps,
			array<ErrorCodes>^% errors)
		{
			Int32 c = tagIds->Length;
			IntPtr unm_v = Marshal::AllocHGlobal( (sizeof(VARIANT)+sizeof(WORD)+sizeof(::FILETIME)+sizeof(HRESULT)) * c );
			IntPtr unm_q = IntPtr(unm_v.ToInt32() + (Int32)sizeof(VARIANT)*c);
			IntPtr unm_t = IntPtr(unm_q.ToInt32() + (Int32)sizeof(WORD)*c);
			IntPtr unm_e = IntPtr(unm_t.ToInt32() + (Int32)sizeof(::FILETIME)*c);
			pin_ptr<Int32> unm_i = &tagIds[0];

			Int32 hr = m_gbda_native->GBGetItems_Caller(
				(DWORD)c,
				(DWORD*)unm_i,
				(VARIANT*)unm_v.ToPointer(),
				(WORD*)unm_q.ToPointer(),
				(::FILETIME*)unm_t.ToPointer(),
				(HRESULT*)unm_e.ToPointer());
			if FAILED(hr)
			{
				Marshal::FreeHGlobal(unm_v);
				Marshal::ThrowExceptionForHR(hr);
			}
			try
			{
				values = Marshal::GetObjectsForNativeVariants(unm_v, c);

				qualities = gcnew array<Quality>(c);
				pin_ptr<Quality> p_qualities = &qualities[0];
				memcpy(p_qualities, unm_q.ToPointer(), sizeof(WORD)*c);

				timestamps = gcnew array<FileTime>(c);
				pin_ptr<FileTime> p_timestamps = &timestamps[0];
				memcpy(p_timestamps, unm_t.ToPointer(), sizeof(::FILETIME)*c);

				errors = gcnew array<ErrorCodes>(c);
				pin_ptr<ErrorCodes> p_errors = &errors[0];
				memcpy(p_errors, unm_e.ToPointer(), sizeof(HRESULT)*c);
			}
			catch (Exception^ ex)
			{
				throw ex;
			}
			finally
			{
				m_gbda_native->GBGetItems_Cleanup(c, (VARIANT*)unm_v.ToPointer());
			}
		}
		#pragma endregion

		#pragma region UpdateTags
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
		virtual void UpdateTags(
			array<Int32>^ tagIds,
			array<Object^>^ values,
			array<Quality>^ qualites,
			array<FileTime>^ timestamps,
			array<ErrorCodes>^ errors,
			Boolean wait)
		{
			Int32 i, c = tagIds->Length;
			IntPtr unm_v = IntPtr::Zero;
			if (values != nullptr)
			{
				unm_v = Marshal::AllocHGlobal(sizeof(VARIANT) * c);
				for (i = 0; i<c; i++)
					Marshal::GetNativeVariantForObject(values[i], IntPtr(unm_v.ToInt32() + i * (Int32)sizeof(VARIANT)));
			}
			pin_ptr<Int32> unm_i = &tagIds[0];
			pin_ptr<Quality> unm_q = &qualites[0];
			pin_ptr<FileTime> unm_t = &timestamps[0];
			pin_ptr<ErrorCodes> unm_e = &errors[0];
			Int32 hr = m_gbda_native->GBUpdateItems_Caller(
				(DWORD)c,
				(DWORD*)unm_i,
				values != nullptr? (VARIANT*)unm_v.ToPointer() : NULL,
				(WORD*)unm_q,
				(::FILETIME*)unm_t,
				(HRESULT*)unm_e,
				(BOOL)wait);
			Marshal::ThrowExceptionForHR(hr);
		}
		virtual void UpdateTags(
			array<Int32>^ tagIds,
			array<Quality>^ qualites,
			array<FileTime>^ timestamps,
			array<ErrorCodes>^ errors,
			Boolean wait)
		{
			UpdateTags(tagIds, nullptr, qualites, timestamps, errors, wait);
		}
        /// <param name="tagIds">An array of tag identifiers returned earlier by CreateTag method.</param>
        /// <param name="values">An array containing the new values of tags, specified by tagIds.
        ///    Pass nullptr if you want to leave the values of tags unchanged.</param>
        /// <param name="quality">A new quality for all tags, specified by tagIds.</param>
        /// <param name="timestamp">A new timestamp for all tags, specified by tagIds. It must be a time in UTC.</param>
        /// <param name="error">A new status error of tags, specified by tagIds.</param>
        /// <param name="wait">If true then UpdateTags waits for new data to be placed into the OPC server cache,
        ///   otherwise the update is carried out asynchronously.</param>
		virtual void UpdateTags(
			array<Int32>^ tagIds,
			array<Object^>^ values,
			Quality quality,
			FileTime timestamp,
			ErrorCodes error,
			Boolean wait)
		{
            int c = tagIds->Length;
            array<Quality>^ qualities = gcnew array<Quality>(c);
            array<FileTime>^ timestamps = gcnew array<FileTime>(c);
            array<ErrorCodes>^ errors = gcnew array<ErrorCodes>(c);
            for (int i = 0; i < c; i++)
            {
                qualities[i] = quality;
                timestamps[i] = timestamp;
                errors[i] = error;
            }
            UpdateTags(tagIds, values, qualities, timestamps, errors, wait);
		}
        /// <param name="tagIds">An array of tag identifiers returned earlier by CreateTag method.</param>
        /// <param name="quality">A new quality for all tags, specified by tagIds.</param>
        /// <param name="timestamp">A new timestamp for all tags, specified by tagIds. It must be a time in UTC.</param>
        /// <param name="error">A new status error of tags, specified by tagIds.</param>
        /// <param name="wait">If true then UpdateTags waits for new data to be placed into the OPC server cache,
        ///   otherwise the update is carried out asynchronously.</param>
		virtual void UpdateTags(
			array<Int32>^ tagIds,
			Quality quality,
			FileTime timestamp,
			ErrorCodes error,
			Boolean wait)
		{
            UpdateTags(tagIds, nullptr, quality, timestamp, error, wait);
		}
        /// <param name="tagIds">An array of tag identifiers returned earlier by CreateTag method.</param>
        /// <param name="values">An array containing the new values of tags, specified by tagIds.
        ///    Pass nullptr if you want to leave the values of tags unchanged.</param>
        /// <param name="wait">If true then UpdateTags waits for new data to be placed into the OPC server cache,
        ///   otherwise the update is carried out asynchronously.</param>
        virtual void UpdateTags(
			array<Int32>^ tagIds,
			array<Object^>^ values,
            Boolean wait)
        {
			UpdateTags(tagIds, values, Quality::Good, FileTime::UtcNow, ErrorCodes::Ok, wait);
        }
		#pragma endregion

		#pragma region BeginUpdate
		/// <summary>
		/// <para>Starts the transaction of OPC server secondary cache update.
		/// A wrapper for GBDataAccessBase::BeginUpdate method.</para>
		/// <para>This method must be called prior to calling SetTag.
		/// Each call to BeginUpdate must have a single call to EndUpdate.</para>
		/// <para>BeginUpdate calls ThrowExceptionForHR with the resulting HRESULT returned by
		/// GBDataAccessBase::BeginUpdate to throw an exception if BeginUpdate method fails.</para>
		/// </summary>
		virtual void BeginUpdate()
		{
			Marshal::ThrowExceptionForHR(m_gbda_native->GBBeginUpdate());
		}
		#pragma endregion

		#pragma region SetTag
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
		virtual void SetTag(
			Int32 tagId,
			Object^ value,
			Quality quality,
			FileTime timestamp,
			ErrorCodes error)
		{
			VARIANT var = {0};
			IntPtr unm_v(&var);
			if (value != nullptr) Marshal::GetNativeVariantForObject(value, unm_v);
			pin_ptr<FileTime> pt = &timestamp;
			Int32 hr = m_gbda_native->GBSetItem_Caller(
				tagId,
				(VARIANT*)unm_v.ToPointer(),
				quality.Code,
				(::FILETIME*)pt,
				(HRESULT)error);
			Marshal::ThrowExceptionForHR(hr);
		}

		/// <param name="tagId">TagID identifier of a tag, returned earlier by CreateTag.</param>
		/// <param name="value">A new value of a tag. Pass null to leave the value of a tag unchanged.</param>
		/// <param name="quality">A new quality of a tag.</param>
		/// <param name="timestamp">A new timestamp of a tag. It must be a time in UTC.</param>
		virtual void SetTag(
			Int32 tagId,
			Object^ value,
			Quality quality,
			FileTime timestamp)
		{
			VARIANT var = {0};
			IntPtr unm_v(&var);
			if (value != nullptr) Marshal::GetNativeVariantForObject(value, unm_v);
			pin_ptr<FileTime> pt = &timestamp;
			Int32 hr = m_gbda_native->GBSetItem_Caller(
				tagId,
				(VARIANT*)unm_v.ToPointer(),
				quality.Code,
				(::FILETIME*)pt,
				S_OK);
			Marshal::ThrowExceptionForHR(hr);
		}

		/// <param name="tagId">TagID identifier of a tag, returned earlier by CreateTag.</param>
		/// <param name="value">A new value of a tag. Pass null to leave the value of a tag unchanged.</param>
		virtual void SetTag(
			Int32 tagId,
			Object^ value)
		{
			SetTag(tagId, value, Quality::Good, FileTime::UtcNow, ErrorCodes::Ok);
		}
		/// <param name="tagId">TagID identifier of a tag, returned earlier by CreateTag.</param>
		/// <param name="quality">A new quality of a tag.</param>
		/// <param name="timestamp">A new timestamp of a tag. It must be a time in UTC.</param>
		/// <param name="error">A new error of a tag.</param>
		virtual void SetTag(
			Int32 tagId,
			Quality quality,
			FileTime timestamp,
			ErrorCodes error)
		{
			SetTag(tagId, nullptr, quality, timestamp, error);
		}
		/// <param name="tagId">TagID identifier of a tag, returned earlier by CreateTag.</param>
		/// <param name="quality">A new quality of a tag.</param>
		/// <param name="timestamp">A new timestamp of a tag. It must be a time in UTC.</param>
		virtual void SetTag(
			Int32 tagId,
			Quality quality,
			FileTime timestamp)
		{
			SetTag(tagId, nullptr, quality, timestamp, ErrorCodes::Ok);
		}
		/// <param name="tagId">TagID identifier of a tag, returned earlier by CreateTag.</param>
		/// <param name="quality">A new quality of a tag.</param>
		virtual void SetTag(
			Int32 tagId,
			Quality quality)
		{
			SetTag(tagId, nullptr, quality, FileTime::UtcNow, ErrorCodes::Ok);
		}
        /// <param name="tagId">TagID identifier of a tag, returned earlier by CreateTag.</param>
        /// <param name="timestamp">A new timestamp of a tag. It must be a UTC time.</param>
		virtual void SetTag(
			Int32 tagId,
			FileTime timestamp)
		{
			SetTag(tagId, nullptr, Quality::Good, timestamp, ErrorCodes::Ok);
		}
		#pragma endregion

		#pragma region EndUpdate
		/// <summary>
		/// <para>Ends the transaction of OPC server secondary cache update, which was started
		/// earlier with BeginUpdate.
		/// A wrapper for GBDataAccessBase::GBEndUpdate method.</para>
		/// <para>UpdateTags calls ThrowExceptionForHR with the resulting HRESULT returned by
		/// GBDataAccessBase::GBEndUpdate to throw an exception if EndUpdate method fails.</para>
		/// </summary>
		/// <param name="wait">If true then EndUpdate waits for the update transaction to complete (new data is stored into the OPC server cache),
		///   otherwise the update transaction is carried out asynchronously.</param>
		virtual void EndUpdate(Boolean wait)
		{
			Marshal::ThrowExceptionForHR(m_gbda_native->GBEndUpdate(wait));
		}
		#pragma endregion

		#pragma region CreateTag // zzz add noraml error handling / exeptions
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
		virtual Int32 CreateTag(
			Int32 userId,
			String^ opcItemId,
			AccessRights accessRights,
			TagOptions tagOptions,
			EUType euType,
			Object^ euInfo,
			Object^ defaultValue)
		{
			Int32 tag_id = 0;
			IntPtr unm_OpcItemId = Marshal::StringToHGlobalUni(opcItemId);
			IntPtr unm_Value = Marshal::AllocCoTaskMem(sizeof(VARIANT) * 2);
			IntPtr unm_EUInfo = IntPtr(unm_Value.ToInt32() + (Int32)sizeof(VARIANT));
			Marshal::GetNativeVariantForObject(defaultValue, unm_Value);
			switch(euType)
			{
			case EUType::noEnum:
				unm_EUInfo = IntPtr::Zero;
				break;
			case EUType::analog:
				Marshal::GetNativeVariantForObject(euInfo, unm_EUInfo);
				break;
			case EUType::enumerated:
				Marshal::GetNativeVariantForObject(euInfo, unm_EUInfo);
				break;
			}
			pin_ptr<Int32> p = &tag_id;
			Int32 hr = m_gbda_native->GBCreateItem(
				(DWORD*)p,
				(DWORD)userId,
				(LPCWSTR)unm_OpcItemId.ToPointer(),
				(DWORD)accessRights,
				(DWORD)tagOptions,
				(VARIANT*)unm_Value.ToPointer(),
				(OPCEUTYPE)euType,
				(VARIANT*)unm_EUInfo.ToPointer());
			VariantClear((VARIANT*)unm_Value.ToPointer());
			if (unm_EUInfo != IntPtr::Zero) VariantClear((VARIANT*)unm_EUInfo.ToPointer());
			Marshal::FreeHGlobal(unm_OpcItemId);
			Marshal::FreeHGlobal(unm_Value);
			Marshal::ThrowExceptionForHR(hr);
			return tag_id;
		}

		/// <param name="userId">Arbitrary UserId tag identifier. This value will be passed to the Toolkit event handlers. Its meaning is left for your consideration.</param>
		/// <param name="opcItemId">A fully qualified tag name.</param>
		/// <param name="accessRights">Indicates if this tag is read only, write only or read/write.</param>
		/// <param name="defaultValue">An Object containing the tags initial data value in its canonical data type.</param>
		virtual Int32 CreateTag(
			Int32 userId,
			String^ opcItemId,
			AccessRights accessRights,
			Object^ defaultValue)
		{
			return CreateTag(userId, opcItemId, accessRights, TagOptions::Default,
				EUType::noEnum, nullptr, defaultValue);
		}

		/// <param name="userId">Arbitrary UserId tag identifier. This value will be passed to the Toolkit event handlers. Its meaning is left for your consideration.</param>
		/// <param name="opcItemId">A fully qualified tag name.</param>
		/// <param name="accessRights">Indicates if this tag is read only, write only or read/write.</param>
		/// <param name="tagOptions">Flags. A combination of bits used to modify the way the Toolkit will proccess this tag.</param>
		/// <param name="defaultValue">An Object containing the tags initial data value in its canonical data type.</param>
		virtual Int32 CreateTag(
			Int32 userId,
			String^ opcItemId,
			AccessRights accessRights,
			TagOptions tagOptions,
			Object^ defaultValue)
		{
			return CreateTag(userId, opcItemId, accessRights, tagOptions,
				EUType::noEnum, nullptr, defaultValue);
		}

		/// <param name="userId">Arbitrary UserId tag identifier. This value will be passed to the Toolkit event handlers. Its meaning is left for your consideration.</param>
		/// <param name="opcItemId">A fully qualified tag name.</param>
		/// <param name="accessRights">Indicates if this tag is read only, write only or read/write.</param>
		/// <param name="tagOptions">Flags. A combination of bits used to modify the way the Toolkit will proccess this tag.</param>
		/// <param name="loLimit">Specifies the LO EU Range.</param>
		/// <param name="hiLimit">Specifies the HI EU Range.</param>
		/// <param name="defaultValue">An Object containing the tags initial data value in its canonical data type.</param>
		virtual Int32 CreateTag(
			Int32 userId,
			String^ opcItemId,
			AccessRights accessRights,
			TagOptions tagOptions,
			Double loLimit,
			Double hiLimit,
			Object^ defaultValue)
		{
			array<Double>^ limits = {loLimit, hiLimit};
			return CreateTag(userId, opcItemId, accessRights, tagOptions,
				EUType::analog, limits, defaultValue);
		}

		/// <param name="userId">Arbitrary UserId tag identifier. This value will be passed to the Toolkit event handlers. Its meaning is left for your consideration.</param>
		/// <param name="opcItemId">A fully qualified tag name.</param>
		/// <param name="accessRights">Indicates if this tag is read only, write only or read/write.</param>
		/// <param name="loLimit">Specifies the LO EU Range.</param>
		/// <param name="hiLimit">Specifies the HI EU Range.</param>
		/// <param name="defaultValue">An Object containing the tags initial data value in its canonical data type.</param>
		virtual Int32 CreateTag(
			Int32 userId,
			String^ opcItemId,
			AccessRights accessRights,
			Double loLimit,
			Double hiLimit,
			Object^ defaultValue)
		{
			array<Double>^ limits = {loLimit, hiLimit};
			return CreateTag(userId, opcItemId, accessRights, TagOptions::Default,
				EUType::analog, limits, defaultValue);
		}

		/// <param name="userId">Arbitrary UserId tag identifier. This value will be passed to the Toolkit event handlers. Its meaning is left for your consideration.</param>
		/// <param name="opcItemId">A fully qualified tag name.</param>
		/// <param name="accessRights">Indicates if this tag is read only, write only or read/write.</param>
		/// <param name="tagOptions">Flags. A combination of bits used to modify the way the Toolkit will proccess this tag.</param>
		/// <param name="stateNames">An array of strings which describe every possible enumerable tag value.
		///   First string in stateNames corresponds to a zero value, second corresponds to 1 and so on.</param>
		/// <param name="defaultValue">An Object containing the tags initial data value in its canonical data type.</param>
		virtual Int32 CreateTag(
			Int32 userId,
			String^ opcItemId,
			AccessRights accessRights,
			TagOptions tagOptions,
			array<String^, 1>^ stateNames,
			Object^ defaultValue)
		{
			return CreateTag(userId, opcItemId, accessRights, tagOptions,
				EUType::enumerated, stateNames, defaultValue);
		}

		/// <param name="userId">Arbitrary UserId tag identifier. This value will be passed to the Toolkit event handlers. Its meaning is left for your consideration.</param>
		/// <param name="opcItemId">A fully qualified tag name.</param>
		/// <param name="accessRights">Indicates if this tag is read only, write only or read/write.</param>
		/// <param name="stateNames">An array of strings which describe every possible enumerable tag value.
		///   First string in stateNames corresponds to a zero value, second corresponds to 1 and so on.</param>
		/// <param name="defaultValue">An Object containing the tags initial data value in its canonical data type.</param>
		virtual Int32 CreateTag(
			Int32 userId,
			String^ opcItemId,
			AccessRights accessRights,
			array<String^, 1>^ stateNames,
			Object^ defaultValue)
		{
			return CreateTag(userId, opcItemId, accessRights, TagOptions::Default,
				EUType::enumerated, stateNames, defaultValue);
		}

		#pragma endregion

		#pragma region AddProperty
		/// <summary>
		/// <para>Creates a new OPC property. 
		/// A wrapper for GBDataAccessBase::GBAddProperty method.</para>
		/// <para>Properties with PropID form 1 to 8 are created automatically during tag creation.
		/// Property 8 is created only for tags with Engineering Units defined.</para>
		/// <para>AddProperty calls ThrowExceptionForHR with the resulting HRESULT returned by
		/// the underlying GBDataAccessBase method to throw an exception if AddProperty method fails.</para>
		/// </summary>
		/// <param name="tagId">The TagId identifier of a tag for which to add the property. This is a TagId returned by CreateTag.</param>
		/// <param name="propId">The PropID identifier of a property to add. See OPC DA Specification for details.</param>
		/// <param name="defaultValue">An initial property value. Its type specifies the data type of a property.</param>
		/// <param name="description">A textual description of a property. If a description with the same PropID was added to some tag earlier, Toolkit will already have its description and you may pass null. Definitions of standard properties (properties listed in the OPC DA Spec) are always known to the Toolkit.</param>
		/// <param name="itemId">A fully qualified ItemID that can be used to access this property. If null is passed, then the property can not be accessed via an ItemID. If you are passing non-null ItemID, then it must be the ItemID of an already created tag.</param>
		virtual void AddProperty(
			Int32 tagId,
			Int32 propId,
			Object^ defaultValue,
			String^ description,
			String^ itemId)
		{
			VARIANT var;
			IntPtr unm_v(&var);
			Marshal::GetNativeVariantForObject(defaultValue, unm_v);
			pin_ptr<const wchar_t> unm_d = PtrToStringChars(description);
			pin_ptr<const wchar_t> unm_i = PtrToStringChars(itemId);
			Int32 hr = m_gbda_native->GBAddProperty_Caller(
				tagId,
				propId,
				(VARIANT*)unm_v.ToPointer(),
				(LPCWSTR)unm_d,
				(LPCWSTR)unm_i,
				0);
			Marshal::ThrowExceptionForHR(hr);
		}
		/// <param name="tagId">The TagId identifier of a tag for which to add the property. This is a TagId returned by CreateTag.</param>
		/// <param name="propId">The PropID identifier of a property to add. See OPC DA Specification for details.</param>
		/// <param name="defaultValue">An initial property value. Its type specifies the data type of a property.</param>
		/// <param name="description">A textual description of a property. If a description with the same PropID was added to some tag earlier, Toolkit will already have its description and you may pass null. Definitions of standard properties (properties listed in the OPC DA Spec) are always known to the Toolkit.</param>
		virtual void AddProperty(
			Int32 tagId,
			Int32 propId,
			Object^ defaultValue,
			String^ description)
		{
			AddProperty(tagId, propId, defaultValue, description, nullptr);
		}
		/// <param name="tagId">The TagId identifier of a tag for which to add the property. This is a TagId returned by CreateTag.</param>
		/// <param name="propId">The PropID identifier of a property to add. See OPC DA Specification for details.</param>
		/// <param name="defaultValue">An initial property value. Its type specifies the data type of a property.</param>
		virtual void AddProperty(
			Int32 tagId,
			Int32 propId,
			Object^ defaultValue)
		{
			AddProperty(tagId, propId, defaultValue, nullptr, nullptr);
		}
		/// <param name="tagId">The TagId identifier of a tag for which to add the property. This is a TagId returned by CreateTag.</param>
		/// <param name="propId">The PropID identifier of a property to add. See OPC DA Specification for details.</param>
		/// <param name="defaultValue">An initial property value. Its type specifies the data type of a property.</param>
		virtual void AddProperty(
			Int32 tagId,
			StandardProperties propId,
			Object^ defaultValue)
		{
			AddProperty(tagId, (int)propId, defaultValue, nullptr, nullptr);
		}
		/// <param name="tagId">A TagId identifier of a tag for which to add the property. This is a TagId returned by CreateTag.</param>
		/// <param name="propId">A PropID identifier of a property to add. See OPC DA Specification for details.</param>
		/// <param name="defaultValue">An initial property value. Its type specifies the data type of a property.</param>
		/// <param name="itemId">A fully qualified ItemID that can be used to access this property. If null is passed, then the property can not be accessed via an ItemID. If you are passing non-null ItemID, then it must be the ItemID of an already created tag.</param>
		virtual void AddProperty(
			Int32 tagId,
			StandardProperties propId,
			Object^ defaultValue,
			String^ itemId)
		{
			AddProperty(tagId, (int)propId, defaultValue, nullptr, itemId);
		}
		#pragma endregion

		#pragma region SetProperty
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
		virtual void SetProperty(
			Int32 tagId,
			Int32 propId,
			Object^ value)
		{
			VARIANT var;
			IntPtr unm_v(&var);
			Marshal::GetNativeVariantForObject(value, unm_v);
			Marshal::ThrowExceptionForHR(m_gbda_native->GBSetProperty_Caller(
				tagId, propId, (VARIANT*)unm_v.ToPointer()));
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
		virtual void SetProperty(
			Int32 tagId,
			StandardProperties propId,
			Object^ value)
		{
			SetProperty(tagId, (int)propId, value);
		}
		#pragma endregion

		#pragma region GetProperty
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
		virtual Object^ GetProperty(Int32 tagId, Int32 propId)
		{
			VARIANT unm_v = {0};
			Object^ value;
			Int32 hr = m_gbda_native->GBGetProperty(tagId, propId, &unm_v);
			if SUCCEEDED(hr)
			{
				value = Marshal::GetObjectForNativeVariant(IntPtr(&unm_v));
				VariantClear(&unm_v);
				return value;
			}
			Marshal::ThrowExceptionForHR(hr);
			return nullptr;
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
		virtual Object^ GetProperty(Int32 tagId, StandardProperties propId)
		{
			return GetProperty(tagId, (int)propId);
		}
		#pragma endregion

		#pragma region RemoveProperty
		/// <summary>
		/// <para>Removes the specified property of the specified tag.
		/// A wrapper for GBDataAccessBase::GBRemoveProperty method.</para>
		/// <para>RemoveProperty calls ThrowExceptionForHR with the resulting HRESULT returned by
		/// GBDataAccessBase::GBRemoveProperty to throw an exception if RemoveProperty method fails.</para>
		/// </summary>
		/// <param name="tagId">A TagId identifier of a tag whose property is to be removed. This is a TagId returned by CreateTag.</param>
		/// <param name="propId">A PropID identifier of a property to remove.</param>
		virtual void RemoveProperty(Int32 tagId, Int32 propId)
		{
			Marshal::ThrowExceptionForHR(m_gbda_native->GBRemoveProperty(tagId, propId));
		}
		/// <param name="tagId">A TagId identifier of a tag whose property is to be removed. This is a TagId returned by CreateTag.</param>
		/// <param name="propId">A PropID identifier of a standard property to remove.</param>
		virtual void RemoveProperty(Int32 tagId, StandardProperties propId)
		{
			Marshal::ThrowExceptionForHR(m_gbda_native->GBRemoveProperty(tagId, (int)propId));
		}
		#pragma endregion

		#pragma region GetBandwidth
		/// <summary>
		/// <para>Used to determine the bandwidth of the OPC server. A wrapper for GBDataAccessBase::GBGetBandwidth method.</para>
		/// <para>This method returns the worst bandwidth among all of the OPC server instances.</para>
		/// <para>GetBandwidth returns zero is bandwidth is undefiened</para>
		/// </summary>
		/// <returns>The bandwidth of this OPC server.</returns>
		virtual Int32 GetBandwidth()
		{
			Int32 hr, b;
			pin_ptr<Int32> p_b = &b;
			hr = m_gbda_native->GBGetBandwidth((DWORD*)p_b);
			if FAILED(hr) return 0;
			return b;
		}
		#pragma endregion

		#pragma region SetState
		/// <summary>
		/// <para>Sets the server state. A wrapper for GBDataAccessBase::GBSetState method.</para>
		/// <para>This method doesn't affect the OPC server behavior. It only affects the status which OPC clients will recieve via IOPCServer::GetStatus.</para>
		/// </summary>
		/// <returns>false if error has occurred, otherwise true.</returns>
		/// <param name="state">A new OPC server state.</param>
		virtual Boolean SetState(ServerState state)
		{
			return SUCCEEDED(m_gbda_native->GBSetState((OPCSERVERSTATE)state)) ? true : false;
		}
		#pragma endregion

		#pragma region Suspend
		/// <summary>
		/// <para>Suspends this OPC server. A wrapper for GBDataAccessBase::GBSuspend method.</para>
		/// <para>Sets the server status to OPC_STATUS_SUSPENDED and suspends its functioning.
		/// While in this state, OPC server ignores most of the client calls, doesn't process
		/// data and doesn't send updates (OnDataChange callbacks).</para>
		/// <para>Call Resume to resume the OPC server functions.</para>
		/// </summary>
		/// <returns>false if error has occurred, otherwise true.</returns>
		virtual Boolean Suspend()
		{
			return SUCCEEDED(m_gbda_native->GBSuspend()) ? true : false;
		}
		#pragma endregion

		#pragma region Resume
		/// <summary>
		/// <para>Resumes this OPC server. A wrapper for GBDataAccessBase::GBResume method.</para>
		/// <para>Resumes OPC server, which was previously suspended with Suspend method, and sets it status to OPC_STATUS_RUNNING.</para>
		/// </summary>
		/// <returns>false if error has occurred, otherwise true.</returns>
		virtual Boolean Resume()
		{
			return SUCCEEDED(m_gbda_native->GBResume()) ? true : false;
		}
		#pragma endregion

		#pragma region Shutdown
		/// <summary>
		/// <para>Shuts down this OPC server. A wrapper for GBDataAccessBase::GBShutdown method.</para>
		/// <para>This method will send IOPCShutdown::ShutdownRequest callback to all client advised IOPCShutdown connection point.
		/// Server functioning will stop and it can't be resumed later.</para>
		/// </summary>
		/// <returns>false if error has occurred, otherwise true.</returns>
		/// <param name="reason">An optional text string indicating the reason for the shutdown. Pass null if you don't wish to specify the reason.</param>
		virtual Boolean Shutdown(String^ reason)
		{
			if (reason == nullptr) return Shutdown();
			pin_ptr<const wchar_t> str = PtrToStringChars(reason);
			return SUCCEEDED(m_gbda_native->GBShutdown(str)) ? true : false;
		}
		virtual Boolean Shutdown()
		{
			return SUCCEEDED(m_gbda_native->GBShutdown(NULL)) ? true : false;
		}
		#pragma endregion

	#pragma endregion

	#pragma region GBDataAccess methods

		#pragma region Initialize
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
		virtual void Initialize(
			Guid classId,
			Int32 timeBase,
			Int32 minUpdateRate,
			ServerOptions flags,
			Char branchSeparator,
			Int32 maxTags,
			Int32 versionMajor,
			Int32 versionMinor,
			Int32 versionBuild,
			String^ vendorName)
		{
			pin_ptr<const wchar_t> unm_szVendorName = PtrToStringChars(vendorName);
			IntPtr unm_ClassId = Helper::GuidToHGlobal(classId);
			Int32 hr = m_gbda_native->GBInitialize(
				(CLSID*)unm_ClassId.ToPointer(),
				timeBase,
				minUpdateRate,
				(DWORD)flags | GB_SRV_FREEERRSTR,
				branchSeparator,
				maxTags,
				versionMajor,
				versionMinor,
				versionBuild,
				(LPCWSTR)unm_szVendorName);
			Marshal::FreeHGlobal(unm_ClassId);
			Marshal::ThrowExceptionForHR(hr);
		}
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
		virtual void Initialize(
			Guid classId,
			Int32 timeBase,
			Int32 minUpdateRate,
			ServerOptions flags,
			Char branchSeparator,
			Int32 maxTags)
		{
			Assembly^ ass = Assembly::GetCallingAssembly();
			Initialize(
				classId,
				timeBase,
				minUpdateRate,
				flags,
				branchSeparator,
				maxTags,
				ass->GetName()->Version->Major,
				ass->GetName()->Version->Minor,
				ass->GetName()->Version->Build,
				((AssemblyCompanyAttribute^)Attribute::GetCustomAttribute(ass, AssemblyCompanyAttribute::typeid))->Company );
		}
		#pragma endregion

		#pragma region RegisterClassObject
		/// <summary>
		/// <para>A wrapper for GBDataAccess::GBRegisterClassObject method.</para>
		/// <para>Registers an OPC server class object with OLE so client applications can connect to it.
		/// Should be called once per OPC server class on the startup.</para>
		/// <para>RegisterClassObject calls ThrowExceptionForHR with the resulting HRESULT returned by
		/// GBDataAccess::GBRegisterClassObject to throw an exception if this method fails.</para>
		/// </summary>
		virtual void RegisterClassObject()
		{
			Marshal::ThrowExceptionForHR( m_gbda_native->GBRegisterClassObject() );
		}
		#pragma endregion

		#pragma region RevokeClassObject
		/// <summary>
		/// <para>A wrapper for GBDataAccess::GBRevokeClassObject method.</para>
		/// <para>Informs OLE that a class object, previously registered with the RegisterClassObject function, is no longer available for use.</para>
		/// <para>RevokeClassObject calls ThrowExceptionForHR with the resulting HRESULT returned by
		/// GBDataAccess::GBRevokeClassObject to throw an exception if this method fails.</para>
		/// </summary>
		virtual void RevokeClassObject()
		{
			Marshal::ThrowExceptionForHR( m_gbda_native->GBRevokeClassObject() );
		}
		#pragma endregion

		#pragma region RegisterServer
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
		static void RegisterServer(
			Guid classId,
			String^ vendorName,
			String^ description,
			String^ versionIndipProgId,
			String^ versionCurrent,
			String^ serviceName)
		{
			IntPtr unm_ClassId = Helper::GuidToHGlobal(classId);
			pin_ptr<const wchar_t> unm_VendorName = PtrToStringChars(vendorName);
			pin_ptr<const wchar_t> unm_Description = PtrToStringChars(description);
			pin_ptr<const wchar_t> unm_VersionIndipProgId = PtrToStringChars(versionIndipProgId);
			pin_ptr<const wchar_t> unm_CurVersion = PtrToStringChars(versionCurrent);
			pin_ptr<const wchar_t> unm_serviceName = PtrToStringChars(serviceName);
			Int32 hr = GBDataAccessNative::GBRegisterServer(
				(CLSID*)unm_ClassId.ToPointer(),
				(LPCWSTR)unm_VendorName,
				(LPCWSTR)unm_Description,
				(LPCWSTR)unm_VersionIndipProgId,
				(LPCWSTR)unm_CurVersion,
				(LPCWSTR)unm_serviceName);
			Marshal::FreeHGlobal(unm_ClassId);
			Marshal::ThrowExceptionForHR(hr);
		}
		/// <param name="classId">A CLSID of the OPC server calss object.</param>
		/// <param name="vendorName">A vendor specific string providing additional information about the server.</param>
		/// <param name="description">A description of the server class object.</param>
		/// <param name="versionIndipProgId">A version independent ProgID of the server. For example, 'SomeOrganization.OPCServer'.</param>
		/// <param name="versionCurrent">A current version of the server class object. For example, '1.2'. Version dependent ProgID will represent
		/// the concatination of versionIndipProgId, point and versionCurrent.</param>
		static void RegisterServer(
			Guid classId,
			String^ vendorName,
			String^ description,
			String^ versionIndipProgId,
			String^ versionCurrent)
		{
			RegisterServer(classId, vendorName, description, versionIndipProgId, versionCurrent, nullptr);
		}
		#pragma endregion

		#pragma region UnregisterServer
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
		static void UnregisterServer(Guid classId)
		{
			IntPtr unm_ClassId = Helper::GuidToHGlobal(classId);
			Int32 hr = GBDataAccessNative::GBUnregisterServer((CLSID*)unm_ClassId.ToPointer());
			Marshal::FreeHGlobal(unm_ClassId);
			Marshal::ThrowExceptionForHR(hr);
		}
		#pragma endregion

	#pragma endregion

	protected:
		GBDataAccessNative* m_gbda_native;

	};

} } } }