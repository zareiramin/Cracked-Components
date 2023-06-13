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
'  Filename    : MyRequest.vb                                                |
'  Version     : 4.31                                                        |
'  Date        : 01-August-2012                                              |
'                                                                            |
'  Description : User's speciffic DataAccess OPC server                      |
'                DaRequest definition                                        |
'                                                                            |
'-----------------------------------------------------------------------------
Imports System
Imports System.Collections
Imports System.Text
Imports Softing.OPCToolbox
Imports Softing.OPCToolbox.Server

Namespace Softing.OPCToolbox.DllSample

    Public Class MyRequest
        Inherits DaRequest

        Public Sub New(ByVal transactionType As EnumTransactionType, ByVal sessionHandle As IntPtr, ByVal aDaAddressSpaceElement As DaAddressSpaceElement, ByVal propertyID As Integer, ByVal requestHandle As IntPtr)
            MyBase.New(transactionType, sessionHandle, aDaAddressSpaceElement, propertyID, requestHandle)
        End Sub
    End Class
End Namespace

