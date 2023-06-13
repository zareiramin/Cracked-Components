Imports System
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Threading
Imports Graybox.OPC.ServerToolkit.CLRWrapper

Public Class Form1

    ''' <remarks>
    ''' In .NET CF we can only use the EventHandler delegate
    ''' to communicate with the main thread of the application.
    ''' And we can't simply call methods of the Form class from the
    ''' OPC event handler, because OPC events are raised on the
    ''' random RPC thread, not the main application thread, and
    ''' Form methods are non threadsafe.
    ''' </remarks>
    Public Sub LogMessages(ByVal sender As Object, ByVal e As EventArgs)
        Monitor.Enter(Program.SyncRoot)
        For Each s As String In Program.Messages
            listBoxLog.Items.Insert(0, s)
        Next
        Program.Messages.Clear()
        Monitor.Exit(Program.SyncRoot)
    End Sub

    Private Sub button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button1.Click
        Program.OPCServer.Suspend()
        listBoxLog.Items.Insert(0, "Suspended by UI")
    End Sub

    Private Sub button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button2.Click
        Program.OPCServer.Resume()
        listBoxLog.Items.Insert(0, "Resumed by UI")
    End Sub

    Private Sub button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button3.Click
        Program.OPCServer.Shutdown("UI request")
        listBoxLog.Items.Insert(0, "Shutdown UI request")
    End Sub
End Class
