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
'  Description : User's specific DataAccess OPC Server                       |
'                address space root element definition                       |
'                                                                            |
'-----------------------------------------------------------------------------
Imports System
Imports System.Collections
Imports System.Text
Imports Softing.OPCToolbox
Imports Softing.OPCToolbox.Server
Namespace Console
	Public Class MyDaAddressSpaceRoot
		Inherits DaAddressSpaceRoot
#Region "Public Methods"
		'--------------------

		Public Overloads Overrides Function QueryAddressSpaceElementData(ByVal anElementId As String, ByRef anElement As AddressSpaceElement) As Integer

			Dim element As New MyDaAddressSpaceElement
			element.ItemId = anElementId
			anElement = element

			Return EnumResultCode.S_OK

		End Function

		'--
#End Region

	End Class
End Namespace

