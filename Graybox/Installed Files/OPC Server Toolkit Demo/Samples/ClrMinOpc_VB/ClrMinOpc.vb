''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''
''  This file is a part of Graybox OPC Server Toolkit.
''  Copyright (C) 2008-2009 Graybox Software
''
''  This example demonstrates the minimal OPC server.
''
''  This sample creates
''  an OPC server with 10 tags. Two properties are added
''  for each tag: LoLimit (set to 0) and HiLimit (set to 100).
''  The values of tags are incremented constantly.
''  WriteItems event handler allows OPC clients to set a
''  new tags data value if this value is in the range from
''  0 to 100. Otherwise, an error is reported to the calling
''  client. 
''
''  To register the OPC server type:
''    clrminopc -r
''  To remove the OPC server registration type:
''    clrminopc -u
''
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports System
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports Graybox.OPC.ServerToolkit.CLRWrapper

Public Class Program
    '' This int will be set to 1 when it's time to exit.
    Shared time_to_stop As Integer = 0
    '' Count of OPC tags to create.
    Shared tag_count As Integer = 10
    '' The TagId identifiers of OPC tags.
    Shared tag_ids(tag_count) As Integer
    '' The process entry point. Set [MTAThread] to enable free threading.
    '' Free threading is required by the OPC Toolkit.
    <MTAThread()> Public Shared Sub Main(ByVal args() As String)
        '' This will be the CLSID and the AppID of our COM-object.
        Dim srv_guid As Guid = New Guid("EA1370BF-AC53-41f2-940D-3A834208BBFB")
        '' Parse the command line args
        If (args.Length > 0) Then
            Try
                '' Register the OPC server and return.
                If (Not args(0).IndexOf("-r") = -1) Then
                    OPCDAServer.RegisterServer( _
                        srv_guid, _
                        "Graybox Software", _
                        "ClrMinOpc", _
                        "Graybox.Sample.ClrMinOpc", _
                        "1.0")
                    Exit Sub
                End If
                '' Remove the OPC server registration and return.
                If (Not args(0).IndexOf("-u") = -1) Then
                    OPCDAServer.UnregisterServer(srv_guid)
                    Exit Sub
                End If
            Catch ex As Exception
                Console.WriteLine(ex.Message)
                Exit Sub
            End Try
        End If
        '' Create an object of the OPC Server class.
        Dim srv As OPCDAServer = New OPCDAServer()
        '' Advise for the OPC Toolkit events.
        AddHandler srv.Events.WriteItems, AddressOf Events_WriteItems
        AddHandler srv.Events.ServerReleased, AddressOf Events_ServerReleased
        '' Initialize the OPC server object and the OPC Toolkit.
        srv.Initialize(srv_guid, 50, 50, ServerOptions.NoAccessPaths, "."c, 100)
        '' Create the OPC tags.
        For i As Integer = 0 To tag_count - 1
            '' Create a tag.
            tag_ids(i) = srv.CreateTag(i, "Folder.Tag" + i.ToString(), AccessRights.readWritable, i)
            '' Add a couple of standard OPC properties.
            srv.AddProperty(tag_ids(i), StandardProperties.LoLimit, CDbl(0))
            srv.AddProperty(tag_ids(i), StandardProperties.HiLimit, CDbl(100))
        Next
        '' Mark the OPC server COM object as running.
        srv.RegisterClassObject()
        '' Wait until the OPC server is released by the clients.
        '' Periodically update tags values while the OPC server is not released.
        While System.Threading.Interlocked.CompareExchange(time_to_stop, 1, 1) = 0
            System.Threading.Thread.Sleep(200)
            '' Begin the update of the OPC server cache.
            srv.BeginUpdate()
            '' Get current values of the tags.
            Dim values() As Object = Nothing
            srv.GetTags(tag_ids, values)
            '' Calculate new values and place them into the OPC server cache.
            For i As Integer = 0 To tag_count - 1
                srv.SetTag(tag_ids(i), (CInt(values(i)) + 1) Mod 100, Quality.Good, Graybox.OPC.ServerToolkit.CLRWrapper.FileTime.UtcNow)
            Next
            '' Finish the update of the OPC server cache. We pass false,
            '' because its unnecessary for this update to be synchronous.
            srv.EndUpdate(False)
        End While
        '' Mark the OPC server COM object as stopped.
        srv.RevokeClassObject()
    End Sub

    '' A handler for the WriteItems event of the OPCDAServer object.
    '' We do not update the OPC server cache here.
    Shared Sub Events_WriteItems(ByVal sender As Object, ByVal e As WriteItemsArgs)
        For i As Integer = 0 To e.Count - 1
            If Not e.ItemIds(i).TagId = 0 Then
                Try
                    Dim v As Integer = Convert.ToInt32(e.Values(i), System.Globalization.CultureInfo.InvariantCulture)
                    If (v < 0) Or (v > 100) Then Throw New ArgumentOutOfRangeException()
                Catch ex As Exception
                    e.Errors(i) = System.Runtime.InteropServices.Marshal.GetHRForException(ex)
                    e.ItemIds(i).TagId = 0
                    e.MasterError = ErrorCodes.False
                End Try
            End If
        Next
    End Sub

    '' A handler for the ServerReleased event of the OPCDAServer object.
    Shared Sub Events_ServerReleased(ByVal sender As Object, ByVal e As ServerReleasedArgs)
        '' Make the OPC server object 'suspended'.
        '' No new OPC server instances can be created by the clients
        '' from this moment.
        e.Suspend = True
        '' Signal the main thread, that it's time to exit.
        System.Threading.Interlocked.Exchange(time_to_stop, 1)
    End Sub
End Class

