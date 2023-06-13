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
'  Filename    : MyAeSession.vb                                              |
'  Version     : 4.31                                                        |
'  Date        : 01-August-2012                                              |
'                                                                            |
'  Description : OPC AE Session template class definition                    |
'                                                                            |
'-----------------------------------------------------------------------------
Imports System
Imports System.Collections
Imports Softing.OPCToolbox.Client
Imports Softing.OPCToolbox
Imports System.Threading
Imports Microsoft.VisualBasic
Namespace AEQuerySourceConditions
	Public Class MyAeSession
		Inherits AeSession

#Region "Constructor"
		'-----------------

		Public Sub New(ByVal url As String)
			MyBase.New(url)
			AddHandler QueryAeSourceConditionsCompleted, AddressOf HandleQueryAeSourceConditionsCompleted
		End Sub

		'--
#End Region

#Region "Private Members"
		'---------------------

#Region "Private Attributes"
		'------------------------


		'--
#End Region

		'--
#End Region

#Region "Public Methods"
		'---------------------

		'--
#End Region

#Region "Public Properties"
		'-----------------------


		'--
#End Region

#Region "Handles"
		'---------------------

		Public Shared Sub HandleQueryAeSourceConditionsCompleted(ByVal anAeSession As AeSession, ByVal executionOptions As UInt32, ByVal sourcePath As String, ByVal conditionNames As String(), ByVal result As Integer)

			If ResultCode.SUCCEEDED(result) Then

				System.Console.WriteLine("" & Chr(10).ToString() & " Source conditions of " + sourcePath + " :")
				Dim i As Integer = 0
				While i < conditionNames.Length
					System.Console.WriteLine("  [" + i.ToString() + "] " + conditionNames(i))
					System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
				End While
			Else

				System.Console.WriteLine("Failed to asynchronously query the conditions of source: " + sourcePath)
			End If

		End Sub


		'--
#End Region

	End Class

End Namespace

