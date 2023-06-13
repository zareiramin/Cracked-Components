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
'  Filename    : MyDaAddressSpaceRoot.vb                                     |
'  Version     : 4.31                                                        |
'  Date        : 01-August-2012                                              |
'                                                                            |
'  Description : User's speciffic DataAccess OPC Server                      |
'                address space root element definition                       |
'                                                                            |
'-----------------------------------------------------------------------------
Imports System
Imports System.Collections
Imports System.Text
Imports Softing.OPCToolbox
Imports Softing.OPCToolbox.Server

Namespace Softing.OPCToolbox.DllSample

    Public Class MyDaAddressSpaceRoot
        Inherits DaAddressSpaceRoot

        Public Overloads Overrides Function QueryAddressSpaceElementData(ByVal elementId As String, ByRef anAddressSpaceElement As AddressSpaceElement) As Integer
            '  TODO: add string based address space validations
            anAddressSpaceElement = Nothing
            Return CType(EnumResultCode.E_NOTIMPL, Integer)
        End Function

        Public Overrides Function QueryAddressSpaceElementChildren(ByVal anElementID As String, ByVal aChildrenList As System.Collections.ArrayList) As Integer

        End Function
    End Class
End Namespace

