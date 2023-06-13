''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''
''  This file is a part of Graybox OPC Server Toolkit.
''  Copyright (C) 2008 Graybox Software
''
''  This example demonstrates the usage the
''  OPC properties.
''
''  To register the OPC server type:
''    clropcproperties -r
''  To remove the OPC server registration type:
''    clropcproperties -u
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
    '' Here we will place the TagId of the OPC tags.
    Shared idTag As Integer
    Shared idTagNominal As Integer
    Shared idTagStatus As Integer
    '' This event will be signaled, when the OPC server is release by the clients.
    Shared eventStop As AutoResetEvent = New AutoResetEvent(False)
    '' The process entry point. Set [MTAThread] to enable free threading.
    '' Free threading is required by the OPC Toolkit.
    <MTAThread()> Public Shared Sub Main(ByVal args() As String)
        '' This will be the CLSID and the AppID of our COM-object.
        Dim srvGuid As Guid = New Guid("32468097-D88C-414f-8868-C1403DB496CE")
        '' Parse the command line args
        If args.Length > 0 Then
            Try
                '' Register the OPC server and return.
                If Not args(0).IndexOf("-r") = -1 Then
                    OPCDAServer.RegisterServer( _
                        srvGuid, _
                        "Graybox Software", _
                        "ClrOPCProperties", _
                        "Graybox.Sample.ClrOPCProperties", _
                        "1.0")
                    Exit Sub
                End If
                '' Unregister the OPC server and return.
                If Not args(0).IndexOf("-u") = -1 Then
                    OPCDAServer.UnregisterServer(srvGuid)
                    Exit Sub
                End If
                catch ex as Exception
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
        ''   Advise for the event that is called when the client reads the OPC properties.
        AddHandler srv.Events.ReadProperties, AddressOf Events_ReadProperties
        ''   Adive for the event taht will handle tag reads (from DEVICE, see OPC DA Spec).
        AddHandler srv.Events.ReadItems, AddressOf Events_ReadItems
        ''   Adive for the event taht will handle tag writes.
        AddHandler srv.Events.WriteItems, AddressOf Events_WriteItems
        '' Initialize the OPC server object and the OPC Toolkit.
        srv.Initialize(srvGuid, 50, 50, ServerOptions.NoAccessPaths, "."c, 100)
        '' Create an OPC tag. Assume that this tag corresponds to some physical
        '' controlled variable, that has its current value, its lo and hi limits and
        '' some nominal value.
        '' Here we ignore userId of a tag, because we won't use it. The default value of
        '' a tag is set to zero and its canonical data type is set to double (VT_R8).
        idTag = srv.CreateTag(0, "ControlledVariable", AccessRights.readWritable, CDbl(0))
        '' Now add a tag that will serve as a nominal value of ControlledVariable.
        '' Assume that nominal value is 50.0.
        idTagNominal = srv.CreateTag(0, "ControlledVariableNominal", AccessRights.readWritable, CDbl(50.0))
        '' Add a tag that will contain some textual description of a controlled variable.
        '' It'll be available for the OPC clients to modify and read this status.
        idTagStatus = srv.CreateTag(0, "ControlledVariableStatus", AccessRights.readWritable, "normal")
        '' Now we add the standard OPC properties.
        ''   Add lo limit property with the value of 10.0.
        srv.AddProperty(idTag, StandardProperties.LoLimit, CDbl(10))
        ''   or we could do it like this
        ''      srv.AddProperty(idTag, 309, (double)10);
        ''   Add hi limit of 90.0.
        srv.AddProperty(idTag, StandardProperties.HiLimit, CDbl(90))
        ''   Add a property, that will describe the OPC tags.
        srv.AddProperty(idTag, StandardProperties.ItemDescription, "This is a current value of a controlled variable")
        srv.AddProperty(idTagNominal, StandardProperties.ItemDescription, "This is a nominal value of a controlled variable")
        '' Now we a custom OPC property. It will be a nominal value of a variable.
        '' We will assign a PropID of 5000 to our custom property and provide its textual description.
        '' Also we report, that there is a tag, that could be read to recieve a value of a property.
        srv.AddProperty(idTag, 5000, CDbl(0), "A nominal value of a controlled variable", "ControlledVariableNominal")
        '' Add another custom property - a status of a controlled variable.
        '' Bind "ControlledVariableStatus" tag to this property.
        srv.AddProperty(idTag, 5001, "", "A textual description of a controlled variable status", _
            "ControlledVariableStatus")
        '' This dummy OPC server cache update transaction will move
        '' the default tag values to the OPC server cache. Tag qualities
        '' will be set to OPC_BAD_WAITING_FOR_INITIAL_DATA.
        srv.BeginUpdate()
        srv.EndUpdate(True)
        '' Mark the OPC server COM object as running.
        srv.RegisterClassObject()
        '' Wait until the OPC server is released.
        eventStop.WaitOne()
        '' Mark the OPC server COM object as stopped.
        srv.RevokeClassObject()
    End Sub

    '' When the "ControlledVariableStatus" tag value is being written
    '' by the OPC client, we place its value to the property 5001 of the
    '' "ControlledVariable" tag.
    Shared Sub Events_WriteItems(ByVal sender As Object, ByVal e As WriteItemsArgs)
        '' Iterate through the requested items.
        For i As Integer = 0 To e.Count - 1
            '' Skip items with the zero TagId.
            If Not e.ItemIds(i).TagId = 0 Then
                '' Process only the "ControlledVariableStatus" tag.
                If e.ItemIds(i).TagId = idTagStatus Then
                    '' Modify a value of the property 5001 of the
                    '' "ControlledVariable" tag.
                    srv.SetProperty(idTag, 5001, e.Values(i))
                End If
            End If
        Next
    End Sub

    '' When the client asks the OPC server to return a device value
    '' of the "ControlledVariableStatus" tag, a value of the property 5001
    '' of the "ControlledVariable" tag will be returned.
    Shared Sub Events_ReadItems(ByVal sender As Object, ByVal e As ReadItemsArgs)
        '' Iterate through the requested items.
        For i As Integer = 0 To e.Count - 1
            '' Skip items with the zero TagId.
            If Not e.ItemIds(i).TagId = 0 Then
                '' Process only the "ControlledVariableStatus" tag.
                If e.ItemIds(i).TagId = idTagStatus Then
                    '' Read a value of a property and place its value to the tag.
                    srv.UpdateTags( _
                        New Integer() {idTagStatus}, _
                        New Object() {srv.GetProperty(idTag, 5001)}, _
                        True)
                End If
            End If
        Next
    End Sub

    '' This handler returns the values of the properties.
    Shared Sub Events_ReadProperties(ByVal sender As Object, ByVal e As ReadPropertiesArgs)
        '' Iterate through the requested properties.
        For i As Integer = 0 To e.Count - 1
            '' If property is our custom property 5000 ("A nominal value of a controlled variable")
            If e.Properties(i) = 5000 Then
                '' If property 5000 is requested for the "ControlledVariable" tag we
                '' will return a value of the "ControlledVariableNominal" tag.
                If e.Item.TagId = idTag Then
                    '' Read a value of the "ControlledVariableNominal" tag. This tag in bound to
                    '' a property 5000 of the "ControlledVariable" tag.
                    Dim vals() As Object = Nothing
                    srv.GetTags(New Integer() {idTagNominal}, vals)
                    '' Return a value of a property.
                    e.Values(i) = vals(0)
                    '' Set the result of a property reading operation.
                    e.Errors(i) = ErrorCodes.Ok
                    '' For all other tags we will report error.
                Else
                    '' Say that we can't return a nominal value, because the property
                    '' is not supported for this tag.
                    e.Errors(i) = ErrorCodes.InvalidPropertyId
                    '' Set the event handling error to False (there was a number of errors,
                    '' while reading sertain properties).
                    e.EventHandlingError = ErrorCodes.False
                End If
            End If
        Next
        '' Do not process the properties of any other tags. Leave it for the Toolkit.
    End Sub

    '' Occures when the OPC server is released.
    Shared Sub Events_ServerReleased(ByVal sender As Object, ByVal e As ServerReleasedArgs)
        '' Signal to the main thread, that it's time to exit.
        eventStop.Set()
    End Sub
End Class