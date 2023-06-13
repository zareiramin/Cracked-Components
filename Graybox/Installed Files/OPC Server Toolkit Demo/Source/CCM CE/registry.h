//
// Copyright (c) Microsoft Corporation.  All rights reserved.
//
//
// Use of this source code is subject to the terms of the Microsoft end-user
// license agreement (EULA) under which you licensed this SOFTWARE PRODUCT.
// If you did not accept the terms of the EULA, you are not authorized to use
// this source code. For a copy of the EULA, please see the LICENSE.RTF on your
// install media.
//
/*++
THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
PARTICULAR PURPOSE.

Sample code derived from "Inside Distributed COM" by Guy and Henry Eddon (© 1998,
Guy and Henry Eddon, published by Microsoft Press, ISBN 1-57231-849-X). 
Modified with permission.
*/

// Registry.h
// This function will register a component.
HRESULT RegisterServer(const WCHAR * szModuleName, REFCLSID clsid, const WCHAR * szFriendlyName, const WCHAR * szVerIndProgID, const WCHAR * szProgID, const WCHAR * szThreadingModel);

// This function will unregister a component.
HRESULT UnregisterServer(REFCLSID clsid, const WCHAR * szVerIndProgID, const WCHAR * szProgID);
