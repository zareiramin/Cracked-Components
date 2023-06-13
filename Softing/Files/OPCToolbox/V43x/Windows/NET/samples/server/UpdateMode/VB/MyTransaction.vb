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
Namespace Softing.OPCToolbox.UpdateMode

  Public Class MyTransaction
    Inherits DaTransaction

#Region "Public Methods"
    '--------------------

    Public Sub New(ByVal aTransactionType As EnumTransactionType, ByVal requestList As DaRequest(), ByVal aSessionKey As IntPtr)
      MyBase.New(aTransactionType, requestList, aSessionKey)
    End Sub

    Public Overloads Overrides Function HandleReadRequests() As Integer
      Dim count As Integer = Requests.Count

      Dim i As Integer = 0
      While i < count
        Dim request As DaRequest = DirectCast(Requests(i), DaRequest)
        Dim element As MyDaAddressSpaceElement = CType(request.AddressSpaceElement, MyDaAddressSpaceElement)

        If element Is Nothing Then
          request.Result = EnumResultCode.E_FAIL
                Else
                    If request.PropertyId = 0 Then

                        If element.Type = MyDaAddressSpaceElement.TYPE_POLL_SEC Then

                            ' get address space element value
                            Dim now As DateTime = DateTime.Now
                            Dim cacheValue As New ValueQT(now.Second, [Enum].ToObject(GetType(EnumQuality), EnumQuality.GOOD), now)
                            request.Value = cacheValue
                            request.Result = EnumResultCode.S_OK
                        ElseIf element.Type = MyDaAddressSpaceElement.TYPE_POLL_MIN Then

                            ' get address space element value
                            Dim now As DateTime = DateTime.Now
                            Dim cacheValue As New ValueQT(now.Minute, [Enum].ToObject(GetType(EnumQuality), EnumQuality.GOOD), now)
                            request.Value = cacheValue
                            request.Result = EnumResultCode.S_OK
                        ElseIf element.Type = MyDaAddressSpaceElement.TYPE_REPORT_SEC Then

                            ' get address space element value
                            '    this is a direct device read request
                            Dim now As DateTime = DateTime.Now
                            Dim cacheValue As New ValueQT(now.Second, [Enum].ToObject(GetType(EnumQuality), EnumQuality.GOOD), now)
                            request.Value = cacheValue
                            request.Result = EnumResultCode.S_OK
                        ElseIf element.Type = MyDaAddressSpaceElement.TYPE_REPORT_MIN Then

                            ' get address space element value
                            '    this is a direct device read request
                            Dim now As DateTime = DateTime.Now
                            Dim cacheValue As New ValueQT(now.Minute, [Enum].ToObject(GetType(EnumQuality), EnumQuality.GOOD), now)
                            request.Value = cacheValue
                            request.Result = EnumResultCode.S_OK
                        End If
                    Else
                            ' get property value
                            ' get the value from the address space element
                            element.GetPropertyValue(request)
                    End If
                End If
                i = i + 1
            End While
      Return CompleteRequests()
    End Function


    '--
#End Region
  End Class
End Namespace

