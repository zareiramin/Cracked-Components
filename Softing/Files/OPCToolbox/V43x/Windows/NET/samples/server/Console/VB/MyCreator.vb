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
'  Description : User's specific OPC Server's objects creator class          |
'                                                                            |
'-----------------------------------------------------------------------------
Imports System
Imports System.Collections
Imports System.Text
Imports System.Runtime.InteropServices
Imports Softing.OPCToolbox
Imports Softing.OPCToolbox.Server
Namespace Console
	Public Class MyCreator
		Inherits Creator
#Region "Public Methods"
		'---------------------

		Public Overloads Overrides Function CreateInternalAeAddressSpaceElement(ByVal aName As String, ByVal anUserData As UInt32, ByVal anObjectHandle As System.IntPtr, ByVal aParentHandle As System.IntPtr) As AeAddressSpaceElement
			Return New MyAeAddressSpaceElement(aName, anUserData, anObjectHandle, aParentHandle)
		End Function

		Public Overloads Overrides Function CreateInternalDaAddressSpaceElement(ByVal anItemId As String, ByVal aName As String, ByVal anUserData As UInt32, ByVal anObjectHandle As IntPtr, ByVal aParentHandle As IntPtr) As DaAddressSpaceElement
			Return DirectCast(New MyDaAddressSpaceElement(anItemId, aName, anUserData, anObjectHandle, aParentHandle), DaAddressSpaceElement)
		End Function


		Public Overloads Overrides Function CreateDaAddressSpaceRoot() As DaAddressSpaceRoot
			Return DirectCast(New MyDaAddressSpaceRoot, DaAddressSpaceRoot)
		End Function

		Public Overloads Overrides Function CreateTransaction(ByVal transactionType As EnumTransactionType, ByVal requestList As DaRequest(), ByVal sessionKey As IntPtr) As DaTransaction
			Return DirectCast(New MyTransaction(transactionType, requestList, sessionKey), DaTransaction)
		End Function

		Public Overridable Function CreateMyDaAddressSpaceElement() As DaAddressSpaceElement
			Return DirectCast(New MyDaAddressSpaceElement, DaAddressSpaceElement)
		End Function

		Public Overloads Overrides Function CreateRequest(ByVal aTransactionType As EnumTransactionType, ByVal aSessionHandle As System.IntPtr, ByVal anElement As DaAddressSpaceElement, ByVal aPropertyId As Integer, ByVal aRequestHandle As System.IntPtr) As DaRequest
			Return New MyRequest(aTransactionType, aSessionHandle, anElement, aPropertyId, aRequestHandle)
		End Function

		Public Overloads Overrides Function CreateWebTemplate() As WebTemplate
			Return New MyWebTemplate
		End Function

		'--
#End Region

	End Class
End Namespace

