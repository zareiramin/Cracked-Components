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
'  Filename    : MyDaSession.vb                                              |
'  Version     : 4.31                                                        |
'  Date        : 01-August-2012                                              |
'                                                                            |
'  Description : OPC DA Session Client template class definition             |
'                                                                            |
'-----------------------------------------------------------------------------
Imports System
Imports System.Collections
Imports Softing.OPCToolbox.Client
Imports Softing.OPCToolbox
Imports System.Threading
Namespace DAGetProperties
  Public Class MyDaSession
    Inherits DaSession

#Region "Constructor"
    '-----------------

    Public Sub New(ByVal url As String)
      MyBase.New(url)
      AddHandler GetDaPropertiesCompleted, AddressOf HandleGetDaPropertiesCompleted
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


		Public Shared Sub HandleGetDaPropertiesCompleted(ByVal aDaSession As DaSession, ByVal executionContext As UInt32, ByVal addressSpaceElementHandle As System.IntPtr, ByVal addressSpaceElementId As String, ByVal addressSpaceElementPath As String, ByVal properties As DaProperty(), _
		  ByVal result As Integer)


			If ResultCode.SUCCEEDED(result) Then

				System.Console.WriteLine("Get properties of address space element:  " + addressSpaceElementId)
				Dim i As Integer = 0
				While i < properties.Length

					System.Console.WriteLine("  Property Name: " + properties(i).Name)
					System.Console.WriteLine("  Property Id: " + properties(i).Id.ToString())
					System.Console.WriteLine("  Property Item Id: " + properties(i).ItemId)
					System.Console.WriteLine("  Property DataType: " + properties(i).DataType.ToString())
					System.Console.WriteLine("  Property description: " + properties(i).Description)
					System.Console.WriteLine("  Property value: " + properties(i).ValueQT.Data.ToString() + "  ")
					System.Console.WriteLine(" ")

					System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
				End While
			Else

				System.Console.WriteLine("Failed to asynchronously get properties of address space element: " + addressSpaceElementId)
			End If

		End Sub

		'--
#End Region

  End Class

End Namespace

