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
'  Filename    : MyDaItem.vb                                                 |
'  Version     : 4.31                                                        |
'  Date        : 01-August-2012                                              |
'                                                                            |
'  Description : OPC DA Item template class definition                       |
'                                                                            |
'-----------------------------------------------------------------------------
Imports System
Imports System.Collections
Imports Softing.OPCToolbox.Client
Imports Softing.OPCToolbox
Imports System.Threading
Imports System.IO
Namespace DaConsole_Service
  Public Class MyDaItem
    Inherits DaItem

#Region "Constructor"
    '-----------------

    Public Sub New(ByVal itemId As String, ByVal parentSubscription As MyDaSubscription)
      MyBase.New(itemId, parentSubscription)
      AddHandler StateChangeCompleted, AddressOf HandleStateChanged
      AddHandler PerformStateTransitionCompleted, AddressOf HandlePerformStateTransition
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

    Public Shared Sub HandleStateChanged(ByVal sender As ObjectSpaceElement, ByVal state As EnumObjectState)
			Dim item As MyDaItem = DirectCast(sender, MyDaItem)
      Dim fs As New FileStream("C:\ClientService.txt", FileMode.OpenOrCreate, FileAccess.Write)
      Dim streamWriter As New StreamWriter(fs)
      streamWriter.BaseStream.Seek(0, SeekOrigin.[End])
			streamWriter.WriteLine(item.ToString() + " " + item.Id.ToString() + " State Changed - " + state.ToString())
      streamWriter.Flush()
      streamWriter.Close()
    End Sub


		Public Shared Sub HandlePerformStateTransition(ByVal sender As ObjectSpaceElement, ByVal executionContext As UInt32, ByVal result As Integer)
			Dim fs As New FileStream("C:\ClientService.txt", FileMode.OpenOrCreate, FileAccess.Write)
			Dim streamWriter As New StreamWriter(fs)
			streamWriter.BaseStream.Seek(0, SeekOrigin.[End])
			If ResultCode.SUCCEEDED(result) Then
				Dim item As MyDaItem = DirectCast(sender, MyDaItem)
				streamWriter.WriteLine(sender.ToString() + " " + item.Id.ToString() + " Performed state transition - " + executionContext.ToString())
			Else
				streamWriter.WriteLine(sender.ToString() + "  Performed state transition failed! Result: " + result.ToString())
			End If
			streamWriter.Flush()
			streamWriter.Close()
		End Sub


		'--
#End Region

  End Class


End Namespace

