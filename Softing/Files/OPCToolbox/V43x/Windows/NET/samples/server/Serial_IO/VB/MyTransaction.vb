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
'  Filename    : MyTransaction.vb                                            |
'  Version     : 4.31                                                        |
'  Date        : 01-August-2012                                              |
'                                                                            |
'  Description : User's speciffic DataAccess OPC server                      |
'                DaTransaction definition                                    |
'                                                                            |
'-----------------------------------------------------------------------------
Imports System
Imports System.Collections
Imports System.Text
Imports Softing.OPCToolbox
Imports Softing.OPCToolbox.Server
Namespace Softing.OPCToolbox.SerialIO

  Public Class MyTransaction
    Inherits DaTransaction

#Region "Public Methods"
    '--------------------

    Public Sub New(ByVal aTransactionType As EnumTransactionType, ByVal requestList As DaRequest(), ByVal aSessionKey As IntPtr)
      MyBase.New(aTransactionType, requestList, aSessionKey)
    End Sub

    Public Overloads Overrides Function HandleReadRequests() As Integer
      '  Handle reads asynchrousouly
      Console.OpcServer.AddRequsts(Me.m_requestList)
      Return [Enum].ToObject(GetType(EnumResultCode), EnumResultCode.S_OK)

    End Function

    Public Overloads Overrides Function HandleWriteRequests() As Integer
      '  Handle writesasynchrousouly

      Console.OpcServer.AddRequsts(Me.m_requestList)
      Return [Enum].ToObject(GetType(EnumResultCode), EnumResultCode.S_OK)

    End Function

    '--
#End Region

  End Class

End Namespace

