using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Graybox.OPC.ServerToolkit.CLRWrapper
{

    /// <summary>
    /// This class contains the methods imported from gbdaflat.dll.
    /// These methods are used istead of the methods of GBDataAccess class,
    /// located in gbda3.dll. We need it because we can't directly derive
    /// from this C++ native class in our C# code.
    /// </summary>
    internal static class Gbdaflat
    {
        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_Constructor")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_Constructor", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern IntPtr GBDataAccess_Constructor(
            bool bThreadSafeAdvise,
            IntPtr pfnBeforeCreateInstance,
            IntPtr pfnCreateInstance,
            IntPtr pfnDestroyInstance,
            IntPtr pfnLock,
            IntPtr pfnUnlock,
            IntPtr pfnServerReleased,
            IntPtr pfnWriteItems,
            IntPtr pfnReadItems,
            IntPtr pfnDataUpdate,
            IntPtr pfnActivateItems,
            IntPtr pfnDeactivateItems,
            IntPtr pfnGetErrorString,
            IntPtr pfnQueryLocales,
            IntPtr pfnBrowseAccessPaths,
            IntPtr pfnQueryItem,
            IntPtr pfnReadProperties
            );

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_Advise")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_Advise", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern int GBDataAccess_Advise(
            IntPtr pNativeInstance,
            int nEventIdx);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_Unadvise")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_Unadvise", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern int GBDataAccess_Unadvise(
            IntPtr pNativeInstance,
            int nEventIdx);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_Destructor")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_Destructor", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern void GBDataAccess_Destructor(
            IntPtr pNativeInstance);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBInitialize")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBInitialize", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBInitialize(
            [In] IntPtr pNativeInstance,
            [In] IntPtr pClassId,
            [In] Int32 dwTimeBase,
            [In] Int32 dwMinUpdateRate,
            [In] Int32 dwFlags,
            [In] Char cSeparator,
            [In] Int32 dwTagMax,
            [In] Int32 dwVersionMajor,
            [In] Int32 dwVersionMinor,
            [In] Int32 dwVersionBuild,
            [In, MarshalAs(UnmanagedType.LPWStr)] String szVendorName);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBRegisterClassObject")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBRegisterClassObject", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBRegisterClassObject(
            IntPtr pNativeInstance);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBRevokeClassObject")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBRevokeClassObject", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBRevokeClassObject(
            IntPtr pNativeInstance);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBRegisterServer")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBRegisterServer", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBRegisterServer(
            IntPtr pClassID,
            [In, MarshalAs(UnmanagedType.LPWStr)] String szVendorName,
            [In, MarshalAs(UnmanagedType.LPWStr)] String szDescription,
            [In, MarshalAs(UnmanagedType.LPWStr)] String szVersionIndipProgID,
            [In, MarshalAs(UnmanagedType.LPWStr)] String szCurVersion,
            [In, MarshalAs(UnmanagedType.LPWStr)] String szServiceName);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBUnregisterServer")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBUnregisterServer", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBUnregisterServer(IntPtr pClassID);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBGetItems")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBGetItems", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBGetItems(
            [In] IntPtr pNativeInstance,
		    [In] Int32 dwCount,
		    [In] IntPtr pdwTagIDs,
            [In] IntPtr pvValues,
            [In] IntPtr pwQualities,
            [In] IntPtr pftTimeStamps,
            [In] IntPtr pdwErrors);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBGetItems_Cleanup")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBGetItems_Cleanup", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern void GBDataAccess_GBGetItems_Cleanup(
            Int32 nCount,
            IntPtr pVars);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBUpdateItems")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBUpdateItems", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBUpdateItems(
            IntPtr pNativeInstance,
            Int32 dwCount,
		    IntPtr pdwItemID,
            IntPtr pvValues,
            IntPtr pwQualites,
            IntPtr pftTimestamps,
            IntPtr pdwErrors,
		    Boolean bWait);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBBeginUpdate")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBBeginUpdate", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBBeginUpdate([In] IntPtr pNativeInstance);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBSetItem")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBSetItem", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBSetItem(
            [In] IntPtr pNativeInstance,
            [In] Int32 dwTagID,
            [In, MarshalAs(UnmanagedType.Struct)] ref Object pvValue,
            [In] Int16 wQuality,
            [In] ref FileTime pftTimestamp,
		    [In] Int32 dwError);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBEndUpdate")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBEndUpdate", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBEndUpdate(
            [In] IntPtr pNativeInstance,
            [In] Boolean bWait);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBCreateItem")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBCreateItem", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBCreateItem(
            [In] IntPtr pNativeInstance,
            [Out] out Int32 pdwItemID,
            [In] Int32 dwUserID,
            [In, MarshalAs(UnmanagedType.LPWStr)] String szOpcItemID,
            [In] Int32 dwAccessRights,
            [In] Int32 dwFlags,
            [In, MarshalAs(UnmanagedType.Struct)] ref Object pvValue,
            [In] Int32 dwEUType,
            [In, MarshalAs(UnmanagedType.Struct)] ref Object pvEUInfo);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBCreateItemAnalog")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBCreateItemAnalog", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBCreateItemAnalog(
		    IntPtr pNativeInstance,
		    ref Int32 pdwItemID,
		    Int32 dwUserID,
		    [MarshalAs(UnmanagedType.LPWStr)] String szOpcItemID,
		    Int32 dwAccessRights,
		    Int32 dwFlags,
		    [MarshalAs(UnmanagedType.Struct)] ref Object pvValue,
		    double dLowRange,
		    double dHighRange);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBCreateItemEnum")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBCreateItemEnum", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBCreateItemEnum(
		    IntPtr pNativeInstance,
		    ref Int32 pdwItemID,
		    Int32 dwUserID,
		    [MarshalAs(UnmanagedType.LPWStr)] String szOpcItemID,
		    Int32 dwAccessRights,
		    Int32 dwFlags,
		    [MarshalAs(UnmanagedType.Struct)] ref Object pvValue,
		    Int32 dwEnumCount,
		    IntPtr pszEnumStrings);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBAddProperty")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBAddProperty", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBAddProperty(
		    [In] IntPtr pNativeInstance,
		    [In] Int32 dwTagID,
		    [In] Int32 dwPropID,
            [In, MarshalAs(UnmanagedType.Struct)] ref Object pvValue,
		    [In, MarshalAs(UnmanagedType.LPWStr)] String szDescription,
		    [In, MarshalAs(UnmanagedType.LPWStr)] String szOpcItemID,
    		[In] Int32 dwFlags);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBSetProperty")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBSetProperty", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBSetProperty(
		    [In] IntPtr pNativeInstance,
		    [In] Int32 dwTagID,
		    [In] Int32 dwPropID,
		    [In, MarshalAs(UnmanagedType.Struct)] ref Object pvValue);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBGetProperty")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBGetProperty", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBGetProperty(
		    [In] IntPtr pNativeInstance,
		    [In] Int32 dwTagID,
		    [In] Int32 dwPropID,
            /*[In] IntPtr pvValue*/[Out, MarshalAs(UnmanagedType.Struct)] out Object pvValue);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBRemoveProperty")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBRemoveProperty", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBRemoveProperty(
		    IntPtr pNativeInstance,
		    Int32 dwTagID,
		    Int32 dwPropID);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBGetBandwidth")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBGetBandwidth", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBGetBandwidth(
		    IntPtr pNativeInstance,
		    ref Int32 pdwBandwidth);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBSetState")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBSetState", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBSetState(
		    IntPtr pNativeInstance,
		    Int32 dwServerState);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBSuspend")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBSuspend", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBSuspend(IntPtr pNativeInstance);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBResume")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBResume", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBResume(IntPtr pNativeInstance);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBShutdown")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBShutdown", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBShutdown(
		    IntPtr pNativeInstance,
		    [MarshalAs(UnmanagedType.LPWStr)] String szReason);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBDisconnectClients")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBDisconnectClients", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBDisconnectClients(IntPtr pNativeInstance);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBGetItemCanonicalType")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBGetItemCanonicalType", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBGetItemCanonicalType(
		    IntPtr pNativeInstance,
            Int32 dwTagId);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBGetItemDefaultValue")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBGetItemDefaultValue", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern IntPtr GBDataAccess_GBGetItemDefaultValue(
		    IntPtr pNativeInstance,
            Int32 dwTagId);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBGetItemEUType")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBGetItemEUType", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern Int32 GBDataAccess_GBGetItemEUType(
		    IntPtr pNativeInstance,
            Int32 dwTagId);

        #if WindowsCE
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBGetItemEUInfo")]
        #else
        [DllImport("gbdaflat.dll", EntryPoint = "GBDataAccess_GBGetItemEUInfo", CallingConvention = CallingConvention.Cdecl)]
        #endif
        public static extern IntPtr GBDataAccess_GBGetItemEUInfo(
		    IntPtr pNativeInstance,
            Int32 dwTagId);

    }


    #region Native callbacks from GBDataAccess descendant in the native part of a dotNET wrapper

    #if !WindowsCE
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    #endif
    internal delegate Int32 NativeCallback_BeforeCreateInstance(
        bool bAggregating,
        ref Int32 pdwClientId);

    #if !WindowsCE
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    #endif
    internal delegate void NativeCallback_CreateInstance(Int32 dwClientId);

    #if !WindowsCE
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    #endif
    internal delegate void NativeCallback_DestroyInstance(Int32 dwClientId);

    #if !WindowsCE
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    #endif
    internal delegate void NativeCallback_Lock();

    #if !WindowsCE
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    #endif
    internal delegate void NativeCallback_Unlock();

    #if !WindowsCE
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    #endif
    internal delegate Int32 NativeCallback_ServerReleased();

    #if !WindowsCE
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    #endif
    internal delegate Int32 NativeCallback_WriteItems(
		Int32 dwCount,
        IntPtr pTags,
        IntPtr pvValues,
        IntPtr pwQualities,
        IntPtr pftTimestamps,
        IntPtr pErrors,
		ref ErrorCodes pMasterError,
		Int32 dwLcid,
		Int32 dwClientID);

    #if !WindowsCE
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    #endif
    internal delegate Int32 NativeCallback_ReadItems(
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
		Int32 dwClientID);

    #if !WindowsCE
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    #endif
    internal delegate void NativeCallback_DataUpdate(
        Int32 dwCount,
        IntPtr pTags,
        IntPtr pValues,
        IntPtr pQualities,
        IntPtr pTimestamps,
        IntPtr pErrors,
        Int32 dwClientResult,
        Int32 dwClientID);

    #if !WindowsCE
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    #endif
    internal delegate void NativeCallback_ActivateItems(
		Int32 dwCount,
		IntPtr pTags);

    #if !WindowsCE
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    #endif
    internal delegate void NativeCallback_DeactivateItems(
        Int32 dwCount,
        IntPtr pTags);

    #if !WindowsCE
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    #endif
	internal delegate Int32 NativeCallback_GetErrorString(
		Int32 dwError,
		Int32 dwLcid,
		[In, Out, MarshalAs(UnmanagedType.LPWStr)] ref String pszErrorString,
		Int32 dwClientID);

    #if !WindowsCE
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    #endif
	internal delegate Int32 NativeCallback_QueryLocales(
		[In, Out] ref Int32 pdwCount,
		[In, Out] ref IntPtr ppdwLcid);

    #if !WindowsCE
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    #endif
	internal delegate Int32 NativeCallback_BrowseAccessPaths(
		[In, MarshalAs(UnmanagedType.LPWStr)] String szItemID,
		[In, Out] ref Int32 pdwAccessPathCount,
		[In, Out] ref IntPtr ppszAccessPaths,
		[In] Int32 dwClientID);

    #if !WindowsCE
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    #endif
	internal delegate Int32 NativeCallback_QueryItem(
		[In, MarshalAs(UnmanagedType.LPWStr)] String szItemID,
		[In, MarshalAs(UnmanagedType.LPWStr)] String szAccessPath,
		[In] Int16 wDataType,
		[In] Boolean bAddItem,
		[In, Out] ref Int32 pdwTagID,
		[In] IntPtr pdwAccessPathID,
		[In] Int32 dwClientID);

    #if !WindowsCE
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    #endif
	internal delegate Int32 NativeCallback_ReadProperties(
		[In] ref ItemId pTag,
		[In] Int32 dwCount,
		[In] IntPtr pdwPropIDs,
		[In] IntPtr pvValues,
		[In] IntPtr pdwErrors,
		[In] Int32 dwLcid,
		[In] Int32 dwClientID);

    #endregion
}