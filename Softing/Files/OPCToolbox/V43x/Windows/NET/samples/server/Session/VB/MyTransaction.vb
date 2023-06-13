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
'  Description : User's specific DataAccess OPC server                       |
'                DaTransaction definition                                    |
'                                                                            |
'-----------------------------------------------------------------------------
Imports System
Imports System.Collections
Imports System.Text
Imports Softing.OPCToolbox
Imports Softing.OPCToolbox.Server
Namespace Session
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
				If request.PropertyId = 0 Then
					' get address space element value
					' take the toolbox cache value
					Dim cacheValue As ValueQT = Nothing
					request.AddressSpaceElement.GetCacheValue(cacheValue)
					request.Value = cacheValue
					request.Result = EnumResultCode.S_OK
				Else
					' get property value
					' get the value from the address space element
					Dim element As MyDaAddressSpaceElement = CType(request.AddressSpaceElement, MyDaAddressSpaceElement)
					If Not element Is Nothing Then
						element.GetPropertyValue(request)
					Else
						request.Result = EnumResultCode.E_FAIL
					End If
				End If
				System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
			End While
			Return CompleteRequests()
		End Function

		Public Overloads Overrides Function HandleWriteRequests() As Integer
			Dim count As Integer = Requests.Count

			Dim i As Integer = 0
			While i < count
				Dim request As DaRequest = CType(Requests(i), DaRequest)
				If Not request Is Nothing Then
					Dim element As MyDaAddressSpaceElement = DirectCast(request.AddressSpaceElement, MyDaAddressSpaceElement)
					If Not element Is Nothing Then
						Dim valueQT As ValueQT = request.Value
						request.Result = element.ValueChanged(valueQT)
					End If
				End If
				System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
			End While
			Return CompleteRequests()
		End Function

		'--
#End Region
	End Class
End Namespace

