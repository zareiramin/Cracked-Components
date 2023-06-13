''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''
''  This file is a part of Graybox OPC Server Toolkit.
''  Copyright (C) 2008 Graybox Software
''
''  This example demonstrates the usage of the OPC tags
''  creation methods, including the dynamic tags creation.
''
''  To register the OPC server type:
''    clrcreatetags -r
''  To remove the OPC server registration type:
''    clrcreatetags -u
''
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports System
Imports System.Threading
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports Graybox.OPC.ServerToolkit.CLRWrapper

Public Class Program
    '' The OPC Server object.
    Shared srv As OPCDAServer
    '' This event will be signaled, when the OPC server is release by the clients.
    Shared eventStop As AutoResetEvent = New AutoResetEvent(False)
    '' The process entry point. Set [MTAThread] to enable free threading.
    '' Free threading is required by the OPC Toolkit.
    <MTAThread()> Public Shared Sub Main(ByVal args() As String)
        '' This will be the CLSID and the AppID of our COM-object.
        Dim srvGuid As Guid = New Guid("25F33FF2-D15D-4a00-81F7-7D2E453B9D95")
        '' Parse the command line args
        If args.Length > 0 Then
            Try
                '' Register the OPC server and return.
                If (Not args(0).IndexOf("-r") = -1) Then
                    OPCDAServer.RegisterServer( _
                        srvGuid, _
                        "Graybox Software", _
                        "ClrCreateTags", _
                        "Graybox.Sample.ClrCreateTags", _
                        "1.0")
                    Exit Sub
                End If
                '' Remove the OPC server registration and return.
                If (Not args(0).IndexOf("-u") = -1) Then
                    OPCDAServer.UnregisterServer(srvGuid)
                    Exit Sub
                End If
            Catch ex As Exception
                Console.WriteLine(ex.Message)
                Exit Sub
            End Try
        End If
        '' Create an object of the OPC Server class.
        srv = New OPCDAServer()
        '' Advise for the events.
        ''   Advise for the event that is triggered when the last instance
        ''   of the OPC server is released.
        AddHandler srv.Events.ServerReleased, AddressOf Events_ServerReleased
        ''   Advise for the event, that is used to create the tags "on demand".
        AddHandler srv.Events.QueryItem, AddressOf Events_QueryItem
        '' Initialize the OPC server object and the OPC Toolkit.
        srv.Initialize(srvGuid, 50, 50, ServerOptions.NoAccessPaths, "."c, 100)
        '' Now we create the OPC tags.
        '' Here we ignore userId of tags, because we won't use it, but in the real
        '' application they could be used to identify our tags in the event handlers.
        '' Also we ignore the returned TagIds. If we were to use our tags later (set or get
        '' their values, add properties and so on), then we would store the returned
        '' TagIds somewhere.
        ''   Create a readable string tag.
        srv.CreateTag(0, "Readable.String", AccessRights.readable, "default value")
        ''   Create a writeonly tag with the data type of double (VT_R8) and preset its
        ''   value to 10.23.
        srv.CreateTag(0, "Writeable.Double", AccessRights.writable, CDbl(10.23))
        ''   Create a tag that can't be read or set (actually, it'll be useless).
        srv.CreateTag(0, "NoAccess.Int", AccessRights.noAccess, 0)
        ''   Create an int tag. Allow to read and write this tag.
        ''   It will be possible for the client to request a value
        ''   of this tag in its canonical data type only. If a client will ask the
        ''   OPC server to return a value of this tag in string data type (or in any type
        ''   other then int), it will recieve an error.
        srv.CreateTag(0, "Readwriteable.IntOnly", AccessRights.readWritable, _
            TagOptions.CanonicalOnly, 123)
        ''   Another int tag. TagOptions.DontCompareValues flag indicates, that a value of
        ''   this tag will be sent to OPC Clients via IOPCDataCallback interface every time
        ''   its timestamp changes, even if its value has not changed.
        srv.CreateTag(0, "Readwriteable.IntUnusual", AccessRights.readWritable, _
            TagOptions.DontCompareValues, 123)
        ''   Create tag with the analog engineering units. This tag will have lo and hi limits
        ''   set to 10.0 and 40.0 respectively.
        srv.CreateTag(0, "Readwriteable.Analog", AccessRights.readWritable, 10.0, 40.0, 25.6)
        ''   Create tag with the enumerated engineering units. It will be an int tag, that
        ''   have string descriptions of its possible value. A value of zero will
        ''   correspond to "Open", one - to "Close" and two - to "Malfunction".
        ''   A default value of a new tag is preset to 1 ("Close" state).
        srv.CreateTag(0, "Readwriteable.Enum", AccessRights.readWritable, _
            New String() {"Open", "Close", "Malfunction"}, 1)
        ''  Create an another enumerated tag. Use other overload.
        srv.CreateTag(0, "Readable.EUTag", AccessRights.readable, _
            TagOptions.Default, EUType.enumerated, _
            New String() {"Operational", "Alarm"}, 0)
        '' Mark the OPC server COM object as running.
        srv.RegisterClassObject()
        '' Wait until the OPC server is released.
        eventStop.WaitOne()
        '' Mark the OPC server COM object as stopped.
        srv.RevokeClassObject()
    End Sub

    Shared Sub Events_QueryItem(ByVal sender As Object, ByVal e As QueryItemArgs)
        '' For clarity we only process the situations where
        '' the creation of a new tag is expected.
        If Not e.CreateTag Then Exit Sub
        Dim rights As AccessRights = AccessRights.noAccess
        '' Determine which rights to assign to a new tag.
        '' If the name of the requested tag starts with "Readable", then the tag
        '' is created read only, and so on.
        If e.TagName.StartsWith("Readable.") Then
            rights = AccessRights.readable
        ElseIf e.TagName.StartsWith("Writeable.") Then
            rights = AccessRights.writable
        ElseIf e.TagName.StartsWith("Readwriteable.") Then
            rights = AccessRights.readWritable
        ElseIf e.TagName.StartsWith("NoAccess.") Then
            rights = AccessRights.noAccess
        Else
            '' Report error. Unsupported tag naming syntax.
            e.EventHandlingError = ErrorCodes.UnknownItemId
            Exit Sub
        End If
        '' Create a new integer tag with the name as being queried.
        '' Return its TagId to the calling client.
        e.TagId = srv.CreateTag(0, e.TagName, rights, 0)
    End Sub

    Shared Sub Events_ServerReleased(ByVal sender As Object, ByVal e As ServerReleasedArgs)
        '' Signal to the main thread, that it's time to exit.
        eventStop.Set()
    End Sub
End Class