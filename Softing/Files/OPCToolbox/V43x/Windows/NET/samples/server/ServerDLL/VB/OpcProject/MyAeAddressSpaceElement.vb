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
'  Filename    : MyAeAddressSpaceElement.vb                                  |
'  Version     : 4.31                                                        |
'  Date        : 01-August-2012                                              |
'                                                                            |
'  Description : User's speciffic Alarms and Events OPC Server               |
'                address space element definition                            |
'                                                                            |
'-----------------------------------------------------------------------------
Imports System
Imports System.Collections
Imports System.Text
Imports Softing.OPCToolbox
Imports Softing.OPCToolbox.Server
Namespace Softing.OPCToolbox.DllSample
    Public Class MyAeAddressSpaceElement
        Inherits AeAddressSpaceElement
#Region "Constructors"
        '-------------------

        Public Sub New(ByVal aName As String, ByVal anUserData As System.UInt32, ByVal anObjectHandle As IntPtr, ByVal aParentHandle As IntPtr)
            MyBase.New(aName, anUserData, anObjectHandle, aParentHandle)
        End Sub

        Public Sub New()
        End Sub

        '--
#End Region
    End Class
End Namespace

