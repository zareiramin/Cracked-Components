'-----------------------------------------------------------------------------
'                                                                            |
'                   Softing Industrial Automation GmbH                       |
'                        Richard-Reitzner-Allee 6                            |
'                           85540 Haar, Germany                              |
'                                                                            |
'                 This is a part of the Softing OPC Toolbox                  |
'       Copyright (c) 1998 - 2012 Softing Industrial Automation GmbH         |
'                           All Rights Reserved                              |
'                                                                            |
'-----------------------------------------------------------------------------
'-----------------------------------------------------------------------------
'                             OPC Toolbox NET                                |
'                                                                            |
'  Filename    : MyCreator.vb                                                |
'  Version     : 4.31                                                        |
'  Date        : 01-August-2012                                              |
'                                                                            |
'  Description : User's speciffic OPC Server's objects creator class         |
'                                                                            |
'-----------------------------------------------------------------------------
Imports System
Imports System.Collections
Imports System.Text
Imports System.Runtime.InteropServices
Imports Softing.OPCToolbox
Imports Softing.OPCToolbox.Server
Namespace Softing.OPCToolbox.AddressSpace
  Public Class MyCreator
    Inherits Creator

    Public Overloads Overrides Function CreateDaAddressSpaceRoot() As DaAddressSpaceRoot
      Return DirectCast(New MyDaAddressSpaceRoot(), DaAddressSpaceRoot)
    End Function

    Public Overloads Overrides Function CreateTransaction(ByVal aTransactionType As EnumTransactionType, ByVal aRequests As DaRequest(), ByVal aSessionKey As IntPtr) As DaTransaction

      Return DirectCast(New MyTransaction(aTransactionType, aRequests, aSessionKey), DaTransaction)
    End Function

    Public Overloads Overrides Function CreateInternalDaAddressSpaceElement(ByVal anItemId As String, ByVal aName As String, ByVal anUserData As System.UInt32, ByVal anObjectHandle As IntPtr, ByVal aParentHandle As IntPtr) As DaAddressSpaceElement

      Return DirectCast(New MyDaAddressSpaceElement(anItemId, aName, anUserData, anObjectHandle, aParentHandle), DaAddressSpaceElement)

    End Function

  End Class

End Namespace

