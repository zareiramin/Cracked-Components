''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''
''  This file is a part of Graybox OPC Server Toolkit.
''  Copyright (C) 2008 Graybox Software
''
''  An OPC server written in C#. This OPC server contains
''  no tags. 
''
''  Demonstarates the usage of dotNET Wrapper for Graybox
''  OPC Server Toolkit and the usage of the following events:
''  Unlock, Lock, DestroyInstance, CreateInstance,
''  BeforeCreateInstance and ServerReleased. 
''
''  To register the OPC server type:
''    clrlifetime -r
''  To remove the OPC server registration type:
''    clrlifetime -u
''
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Threading
Imports Graybox.OPC.ServerToolkit.CLRWrapper

Public Class Program
    Public Shared m_init As Integer = 0
    Private Shared m_new_client_id As Integer = 0
    Private Shared m_messages As List(Of String) = New List(Of String)()
    Private Shared m_guid As Guid = New Guid("AA4E7383-D1AB-4f1c-AA5E-A820DB3BB9E5")
    Private Shared m_srv As OPCDAServer = New OPCDAServer()
    Private Shared m_form As Form1 = Nothing
    Private Shared Sub AddLogMessage(ByVal s As String)
        Monitor.Enter(SyncRoot)
        m_messages.Add(s)
        Monitor.Exit(SyncRoot)
        If Interlocked.CompareExchange(m_init, 1, 1) = 1 Then
            m_form.Invoke(New EventHandler(AddressOf m_form.LogMessages), m_srv, EventArgs.Empty)
        End If
    End Sub
    Public Shared ReadOnly Property SyncRoot() As Object
        Get
            Return m_messages
        End Get
    End Property
    Public Shared ReadOnly Property Messages() As List(Of String)
        Get
            Return m_messages
        End Get
    End Property
    Public Shared ReadOnly Property OPCServer() As OPCDAServer
        Get
            Return m_srv
        End Get
    End Property

    <MTAThread()> Public Shared Sub Main(ByVal args() As String)
        '' Parse the command line args
        If args.Length > 0 Then
            Try
                '' Register the OPC server and return.
                If Not args(0).IndexOf("-r") = -1 Then
                    OPCDAServer.RegisterServer( _
                        m_guid, _
                        "Graybox Software", _
                        "ClrLifeTime", _
                        "Graybox.Sample.ClrLifeTime", _
                        "1.0")
                    Exit Sub
                End If
                '' Unregister the OPC server and return.
                If Not args(0).IndexOf("-u") = -1 Then
                    OPCDAServer.UnregisterServer(m_guid)
                    Exit Sub
                End If
            Catch ex As Exception
                Console.WriteLine(ex.Message)
                Exit Sub
            End Try
        End If

        '' Initialize the OPC server object and the OPC Toolkit
        m_srv.Initialize(Program.m_guid, 100, 100, ServerOptions.NoAccessPaths, "."c, 100)
        AddHandler m_srv.Events.ServerReleased, AddressOf Events_ServerReleased
        AddHandler m_srv.Events.BeforeCreateInstance, AddressOf Events_BeforeCreateInstance
        AddHandler m_srv.Events.CreateInstance, AddressOf Events_CreateInstance
        AddHandler m_srv.Events.DestroyInstance, AddressOf Events_DestroyInstance
        AddHandler m_srv.Events.Lock, AddressOf Events_Lock
        AddHandler m_srv.Events.Unlock, AddressOf Events_Unlock
        '' Mark the OPC server COM object as running
        m_srv.RegisterClassObject()

        m_form = New Form1()
        Interlocked.Increment(m_init)

        '' Run the application
        Application.Run(m_form)

        m_srv.Shutdown("Server is terminating")
        m_srv.RevokeClassObject()
    End Sub

    Public Shared Sub Events_Unlock(ByVal sender As Object, ByVal e As UnlockArgs)
        AddLogMessage("Lock counter has been decremented")
    End Sub

    Public Shared Sub Events_Lock(ByVal sender As Object, ByVal e As LockArgs)
        AddLogMessage("Lock counter has been incremented")
    End Sub

    Public Shared Sub Events_DestroyInstance(ByVal sender As Object, ByVal e As DestroyInstanceArgs)
        AddLogMessage("[ClientId " + e.ClientId.ToString() + "] Client has disconnected")
    End Sub

    Public Shared Sub Events_CreateInstance(ByVal sender As Object, ByVal e As CreateInstanceArgs)
        AddLogMessage("[ClientId " + e.ClientId.ToString() + "] Client has connected")
    End Sub

    Public Shared Sub Events_BeforeCreateInstance(ByVal sender As Object, ByVal e As BeforeCreateInstanceArgs)
        AddLogMessage("Connection request")
        Dim dr As DialogResult = MessageBox.Show( _
            "A client is requesting a connection." + ControlChars.NewLine + _
            "Allow the client to connect to ClrLifeTime?", _
            "ClrLifeTime", _
            MessageBoxButtons.YesNo, _
            MessageBoxIcon.Question, _
            MessageBoxDefaultButton.Button1)
        '' Restrict creation of a new OPC server instance
        If dr = DialogResult.No Then
            e.EventHandlingError = ErrorCodes.AccessDenied
            Exit Sub
        End If
        e.ClientId = Interlocked.Increment(m_new_client_id)
        AddLogMessage("Connection request accepted [ClientId " + e.ClientId.ToString() + "]")
    End Sub

    Public Shared Sub Events_ServerReleased(ByVal sender As Object, ByVal e As ServerReleasedArgs)
        AddLogMessage("OPC server has been released")
        Dim dr As DialogResult = MessageBox.Show( _
            "Last instance of ClrLifeTime OPC server" + ControlChars.NewLine + _
            "has been released." + ControlChars.NewLine + ControlChars.NewLine + _
            "Allow clients to connect to ClrLifeTime in the furute?", _
            "ClrLifeTime", _
            MessageBoxButtons.YesNo, _
            MessageBoxIcon.Question, _
            MessageBoxDefaultButton.Button1)
        e.Suspend = (dr = DialogResult.No)
    End Sub

End Class
