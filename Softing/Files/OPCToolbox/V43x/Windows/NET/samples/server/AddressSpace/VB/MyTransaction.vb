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

Namespace Softing.OPCToolbox.AddressSpace

  Public Class MyTransaction
    Inherits DaTransaction

    Public Sub New(ByVal aTransactionType As EnumTransactionType, ByVal aRequests As DaRequest(), ByVal aSessionKey As IntPtr)
      MyBase.New(aTransactionType, aRequests, aSessionKey)

    End Sub

    Public Overloads Overrides Function HandleReadRequests() As Integer

      Dim count As Integer = Requests.Count
      Dim i As Integer = 0
      While i < count

        Dim request As DaRequest = DirectCast(Requests(i), DaRequest)
        If request.PropertyId = 0 Then
          ' get address space element value
          ' take the toolbox cache value
          Dim valueQT As ValueQT = Nothing
          request.AddressSpaceElement.GetCacheValue(valueQT)
          request.Value = valueQT
          request.Result = EnumResultCode.S_OK
        Else
          ' get property value
          ' get the value from the address space element
          Dim element As MyDaAddressSpaceElement = DirectCast(request.AddressSpaceElement, MyDaAddressSpaceElement)
          Dim qualityGood As EnumQuality = CType([Enum].ToObject(GetType(EnumQuality), EnumQuality.GOOD), EnumQuality)
          request.Value = New ValueQT(element.ObjectType, qualityGood, DateTime.Now)
          request.Result = EnumResultCode.S_OK
        End If

        System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
      End While

      Return CompleteRequests()

    End Function

    Public Overloads Overrides Function HandleWriteRequests() As Integer

      Dim result As Integer = ValuesChanged()
      If ResultCode.FAILED(result) Then
        Return result
      End If

      Return CompleteRequests()

    End Function
  End Class
End Namespace

