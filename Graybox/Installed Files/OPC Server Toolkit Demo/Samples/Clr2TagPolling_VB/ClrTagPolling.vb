''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''
''  This file is a part of Graybox OPC Server Toolkit.
''  Copyright (C) 2008 Graybox Software
''
''  This example demonstrates the basic OPC server for
''  a simulated generic automation device.
''
''  The simulated device has a number of holding registers,
''  which can be read or preset, and a number of input
''  registers, which are read only.
''  A saw waves are simulated on the device inputs.
''  New values of the input registers are generated each
''  100 ms.
''  The device simulator occasionally produces the
''  communication errors during the simulated reading and
''  writing of its registers.
''
''  The OPC server provides an OPC tag for each device
''  register. Device polling starts with 500 ms updaterate,
''  when the OPC clients requests the OPC tags.
''
''  To register the OPC server type:
''    clrtagpolling -r
''  To remove the OPC server registration type:
''    clrtagpolling -u
''
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports System
Imports System.Threading
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports Graybox.OPC.ServerToolkit.CLRWrapper

''' <summary>
''' This class is simulating a physical device from which
''' the OPC server reads the data.
''' The simulated device is assumed to have a number floating point registers.
''' The device have 5 holding register, which can be read
''' with ReadInputRegister or preset with WriteHoldingRegister.
''' And there are 5 input register, which can be read with ReadInputRegister.
''' Input registers have random values.
''' Each of these functions simulate a failure of reading or writing in
''' the random moments of time, simulating communication errors.
''' Values of the input registers are constantly incremented by 0.1 units to
''' simulated the saw wave.
''' </summary>
Public Class Device
    ''' <summary>
    ''' This structure represent a pair on value of the device register and
    ''' the flag that indicates the quality of this value.
    ''' </summary>
    Public Structure DeviceRegValue
        Public Value As Double
        Public ValueIsOk As Boolean
        Sub New(ByVal aValue As Double, ByVal aValueIsOk As Boolean)
            Value = aValue
            ValueIsOk = aValueIsOk
        End Sub
    End Structure
    ''' <summary>
    ''' Random numbers generator used in the simulation.
    ''' </summary>
    Private Shared m_random As Random = New Random()
    ''' <summary>
    ''' The array that simulates the input registers.
    ''' </summary>
    Private m_inputRegisters(0 To 4) As Double
    ''' <summary>
    ''' The array that simulates the holding registers.
    ''' </summary>
    Private m_holdingRegisters(0 To 4) As Double
    ''' <summary>
    ''' Timer used to generate the values of the input registers.
    ''' </summary>
    Private m_simTimer As Timer
    ''' <summary>
    ''' Generate new values of the input registers. This function simulates
    ''' the internal activity of the device.
    ''' </summary>
    ''' <param name="param">The Device object.</param>
    Private Sub Simulate(ByVal param As Object)
        Dim thisDevice As Device = CType(param, Device)
        Monitor.Enter(thisDevice)
        For i As Integer = 0 To 4
            m_inputRegisters(i) = m_inputRegisters(i) + 0.1
            If m_inputRegisters(i) > 100.0 Then m_inputRegisters(i) = 0.0
        Next
        Monitor.Exit(thisDevice)
    End Sub
    ''' <summary>
    ''' Initializes a new object of the Device calss.
    ''' </summary>
    Public Sub New()
        '' Initialize input register with the random values.
        For i As Integer = 0 To 4
            m_inputRegisters(i) = m_random.NextDouble() * 100
        Next
        '' Create a timer that will call the Simulate function
        '' to simulate the saw wave on each input register.
        m_simTimer = New Timer(AddressOf Simulate, Me, 0, 100)
    End Sub
    ''' <summary>
    ''' Simulates the reading of the input register.
    ''' </summary>
    ''' <param name="registerIndex">The index of the input register to read.</param>
    ''' <returns>A DeviceRegValue structure.</returns>
    Public Function ReadInputRegister(ByVal registerIndex As Integer) As DeviceRegValue
        '' Simulate the communication failure with the 20% probability.
        If m_random.Next(0, 99) > 80 Then Return New DeviceRegValue(0, False)
        '' Lock this Device object to prevent the simultaneous access
        '' to the m_inputRegisters array from the multiple threads.
        Dim val As DeviceRegValue
        Monitor.Enter(Me)
        '' Return the current value of the input register and mark
        '' this value as 'good' (no read error).
        val = New DeviceRegValue(m_inputRegisters(registerIndex), True)
        Monitor.Exit(Me)
        Return val
    End Function
    ''' <summary>
    ''' Simulates the reading of the holding register.
    ''' </summary>
    ''' <param name="registerIndex">The index of the holding register to read.</param>
    ''' <returns>A DeviceRegValue structure.</returns>
    Public Function ReadHoldingRegister(ByVal registerIndex As Integer) As DeviceRegValue
        '' Simulate the communication failure with the 20% probability.
        If m_random.Next(0, 99) > 80 Then Return New DeviceRegValue(0, False)
        '' Lock this Device object to prevent the simultaneous access
        '' to the m_holdingRegisters array from the multiple threads.
        Dim val As DeviceRegValue
        Monitor.Enter(Me)
        '' Return the current value of the holding register and mark
        '' this value as 'good' (no read error).
        val = New DeviceRegValue(m_holdingRegisters(registerIndex), True)
        Monitor.Exit(Me)
        Return val
    End Function
    ''' <summary>
    ''' Simulates the writing of the holding register.
    ''' </summary>
    ''' <param name="registerIndex">The index of the holding register to write.</param>
    ''' <param name="value">A new value of the register.</param>
    ''' <returns><c>false</c> if there was a communication error with the device.</returns>
    Public Function WriteHoldingRegister(ByVal registerIndex As Integer, ByVal value As Double) As Boolean
        '' Simulate the communication failure with the 20% probability.
        If m_random.Next(0, 99) > 80 Then Return False
        '' Lock this Device object to prevent the simultaneous access
        '' to the m_holdingRegisters array from the multiple threads.
        Monitor.Enter(Me)
        '' Write a new value into the holding register.
        m_holdingRegisters(registerIndex) = value
        Monitor.Exit(Me)
        '' Report that writing has succeeded.
        Return True
    End Function
End Class

''' <summary>
''' Register types enumeration.
''' </summary>
Public Enum RegisterType
    ''' <summary>
    ''' A holding register. Used to store an arbitrary floating point value.
    ''' This register is available for reading and writing.
    ''' </summary>
    Holding
    ''' <summary>
    ''' An input register. This register is read only.
    ''' </summary>
    Input
End Enum

''' <summary>
''' Describes a tag of the OPC server.
''' </summary>
Public Class TagDescription
    ''' <summary>
    ''' Initializes a new instance of the TagDescription class.
    ''' </summary>
    ''' <param name="registerType">Type of the register that corresponds to the OPC tag,
    ''' described by this TagDescription object.</param>
    ''' <param name="registerIndex">An index of the register of an underlying physical device.
    ''' OPC tag, described by this object of the TagDescription class, represents that register.</param>
    ''' <param name="tagId">A TagId identifier of the OPC tag,
    ''' described by this TagDescription object.</param>
    Public Sub New(ByVal aRegisterType As RegisterType, ByVal aRegisterIndex As Integer, ByVal aTagId As Integer)
        m_registerType = aRegisterType
        m_registerIndex = aRegisterIndex
        m_tagId = aTagId
        m_isActive = 0
    End Sub
    Private m_registerType As RegisterType
    Private m_registerIndex As Integer
    Private m_tagId As Integer
    Private m_isActive As Integer
    ''' <summary>
    ''' Gets the index of the register of an underlying physical device.
    ''' OPC tag, described by this object of the TagDescription class, represents that register.
    ''' </summary>
    Public ReadOnly Property RegisterIndex() As Integer
        Get
            Return m_registerIndex
        End Get
    End Property
    ''' <summary>
    ''' Gets the register type of an underlying physical device.
    ''' </summary>
    Public ReadOnly Property RegisterType() As RegisterType
        Get
            Return m_registerType
        End Get
    End Property
    ''' <summary>
    ''' A TagId identifier of the OPC tag. This id is return by the <c>OPCDAServer.CreateTag</c> function.
    ''' </summary>
    Public ReadOnly Property TagId() As Integer
        Get
            Return m_tagId
        End Get
    End Property
    ''' <summary>
    ''' Gets or sets the value indicating whether the OPC tag is active.
    ''' If tag is active then it should be periodically polled from the underlying physical device.
    ''' </summary>
    Public Property Active() As Boolean
        '' Use interlocked access to the m_isActive member to workaround the multithreading.
        Get
            If Interlocked.CompareExchange(m_isActive, 0, 0) = 0 Then Return False Else Return True
            '    return ((Interlocked.CompareExchange(ref m_isActive, 0, 0) != 0))
        End Get
        Set(ByVal value As Boolean)
            If value Then Interlocked.Exchange(m_isActive, 1) Else Interlocked.Exchange(m_isActive, 0)
        End Set
    End Property
End Class

''' <summary>
''' A class that contains the application entry point.
''' </summary>
Public Class Program
    '' The Device object representing the physical device.
    Shared device As Device = New Device()
    '' The OPC server object used to provide the OPC interface for the
    '' physical device.
    Shared srv As OPCDAServer = New OPCDAServer()
    '' This event will be signaled, when the OPC server is release by the clients.
    Shared eventStop As AutoResetEvent = New AutoResetEvent(False)
    '' A list containig the descriptions of the OPC tags.
    Shared tagDescrs As List(Of TagDescription) = New List(Of TagDescription)()
    '' The process entry point. Set [MTAThread] to enable free threading.
    '' Free threading is required by the OPC Toolkit.
    <MTAThread()> Public Shared Sub Main(ByVal args() As String)
        '' This will be the CLSID and the AppID of our COM-object.
        Dim srvGuid As Guid = New Guid("F041E2EB-067B-4aa0-8557-5C5B2A6BAFDA")
        '' Parse the command line args.
        If args.Length > 0 Then
            Try
                '' Register the OPC server and return.
                If (Not args(0).IndexOf("-r") = -1) Then
                    OPCDAServer.RegisterServer( _
                        srvGuid, _
                        "Graybox Software", _
                        "ClrTagPolling", _
                        "Graybox.Sample.ClrTagPolling", _
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
        '' Advise for the OPC Toolkit events.
        AddHandler srv.Events.ServerReleased, AddressOf Events_ServerReleased
        AddHandler srv.Events.ReadItems, AddressOf Events_ReadItems
        AddHandler srv.Events.WriteItems, AddressOf Events_WriteItems
        AddHandler srv.Events.ActivateItems, AddressOf Events_ActivateItems
        AddHandler srv.Events.DeactivateItems, AddressOf Events_DeactivateItems
        '' Initialize the OPC server object and the OPC Toolkit.
        srv.Initialize(srvGuid, 50, 50, ServerOptions.NoAccessPaths, "."c, 100)
        '' Now we create the OPC tags and fill in the tagDescrs list.
        '' We place the index of the TagDescription object in the tagDescrs container
        '' into userId of the tag, that is described by that TagDescription object.
        '' Also we place a TagId identifier of each tag into the corresponding TagDescription object.
        For i As Integer = 0 To 4
            Dim tagId As Integer
            '' Create an OPC tag for the holding register.
            tagId = srv.CreateTag( _
                tagDescrs.Count, _
                "Device.HoldingRegisters.Reg" + i.ToString(), _
                AccessRights.readWritable, _
                CDbl(0))
            '' Add a description of the created tag to the tagDescrs.
            tagDescrs.Add(New TagDescription(RegisterType.Holding, i, tagId))
            '' Create an OPC tag for the input register.
            tagId = srv.CreateTag( _
                tagDescrs.Count, _
                "Device.InputRegisters.Reg" + i.ToString(), _
                AccessRights.readable, _
                CDbl(0))
            '' Add a description of the created tag to the tagDescrs.
            tagDescrs.Add(New TagDescription(RegisterType.Input, i, tagId))
        Next
        '' Mark the OPC server COM object as running.
        srv.RegisterClassObject()
        '' Wait until the OPC server is released.
        '' While it is not released, poll the device registers each 500 ms.
        '' We only poll the registers that is requested by the OPC clients.
        While Not eventStop.WaitOne(500, False)
            '' Begin the update of the OPC server cache.
            srv.BeginUpdate()
            For Each tagDescr As TagDescription In tagDescrs
                '' Poll the device. Read the device register if the
                '' corresponding OPC tag is requseted by the OPC clients.
                If tagDescr.Active Then ReadDevice(tagDescr)
            Next
            '' Finish the update of the OPC server cache. We pass false,
            '' because its unnecessary for this update to be synchronous.
            srv.EndUpdate(False)
        End While
        '' Mark the OPC server COM object as stopped.
        srv.RevokeClassObject()
    End Sub

    ''' <summary>
    ''' Polls the device.
    ''' </summary>
    ''' <param name="tagDescr">Contains information about the OPC tag and the
    ''' corresponding device register.</param>
    Shared Sub ReadDevice(ByVal tagDescr As TagDescription)
        Dim deviceValue As Device.DeviceRegValue
        '' Perform the device reading.
        Select Case tagDescr.RegisterType
            Case RegisterType.Holding
                deviceValue = device.ReadHoldingRegister(tagDescr.RegisterIndex)
            Case RegisterType.Input
                deviceValue = device.ReadInputRegister(tagDescr.RegisterIndex)
            Case Else
                Throw New NotSupportedException()
        End Select
        If deviceValue.ValueIsOk Then
            '' Place the read value into the OPC server cache,
            '' since the device reading completed successfully.
            srv.SetTag(tagDescr.TagId, deviceValue.Value)
        Else
            '' There was a communication failure.
            '' Set the quality of the OPC tag to indicate that. Do not update
            '' the tag data value.
            srv.SetTag(tagDescr.TagId, New Quality(QualityBits.badCommFailure))
        End If
    End Sub

    ''' <summary>
    ''' Modifies the Active property of the TagDescription objects
    ''' in the tagDescrs list.
    ''' </summary>
    ''' <param name="items">An array of an OPC item identifiers.</param>
    ''' <param name="active">New value of the Active property.</param>
    Shared Sub ChangeTagActivation(ByVal items As ItemId(), ByVal active As Boolean)
        For i As Integer = 0 To items.Length - 1
            '' Skip items with the zero TagId.
            If Not items(i).TagId = 0 Then
                '' Use the UserId to retrieve a TagDescription object, that
                '' describes the OPC tag, that has became active (or inactive).
                Dim tagDescr As TagDescription = tagDescrs(items(i).UserId)
                '' Write the Active property.
                tagDescr.Active = active
            End If
        Next
    End Sub

    ''' <summary>
    ''' A handler for the DeactivateItems event of the OPCDAServer object.
    ''' </summary>
    Shared Sub Events_DeactivateItems(ByVal sender As Object, ByVal e As DeactivateItemsArgs)
        ChangeTagActivation(e.ItemIds, False)
    End Sub

    ''' <summary>
    ''' A handler for the ActivateItems event of the OPCDAServer object.
    ''' </summary>
    Shared Sub Events_ActivateItems(ByVal sender As Object, ByVal e As ActivateItemsArgs)
        ChangeTagActivation(e.ItemIds, True)
    End Sub

    ''' <summary>
    ''' A handler for the WriteItems event of the OPCDAServer object.
    ''' We do not update the OPC server cache here.
    ''' </summary>
    Shared Sub Events_WriteItems(ByVal sender As Object, ByVal e As WriteItemsArgs)
        Try
            '' Say that we don't the OPC Toolkit to copy the tag values,
            '' contained in the WriteItemsArgs, to the OPC server cache.
            e.CopyToCache = False
            '' Iterate through the requested items.
            For i As Integer = 0 To e.Count - 1
                '' Skip items with the zero TagId.
                If Not e.ItemIds(i).TagId = 0 Then
                    Try
                        '' Use the UserId to retrieve a TagDescription object, that
                        '' describes the OPC tag, that has became active (or inactive).
                        Dim tagDescr As TagDescription = tagDescrs(e.ItemIds(i).UserId)
                        Dim writtenOk As Boolean
                        '' Try to convert the values being written to the data type, that
                        '' is acceptable by the device.
                        Dim newValue As Double = CType(e.Values(i), IConvertible).ToDouble(e.Culture)
                        '' Write a new value to the device.
                        writtenOk = device.WriteHoldingRegister(tagDescr.RegisterIndex, newValue)
                        '' Throw an exception if there was a communicaiton error.
                        If Not writtenOk Then Throw New Exception()
                    Catch ex As Exception
                        '' Indicate that error has occured during the tag writing.
                        e.Errors(i) = ErrorCodes.Fail
                        Throw ex
                    End Try
                End If
            Next
        Catch
            '' Indicate that there was some error during the tags writing operation.
            e.MasterError = ErrorCodes.False
        End Try
    End Sub

    ''' <summary>
    ''' A handler for the ReadItems event of the OPCDAServer object.
    ''' </summary>
    Shared Sub Events_ReadItems(ByVal sender As Object, ByVal e As ReadItemsArgs)
        Try
            '' Let the Toolkit know that we don't return tag values
            '' in the ReadItemsArgs.
            e.ValuesReturned = False
            '' Start the trnsaction of the OPC server cache update.
            srv.BeginUpdate()
            '' Iterate through the requested items.
            For i As Integer = 0 To e.Count - 1
                '' Skip items with the zero TagId.
                If Not e.ItemIds(i).TagId = 0 Then
                    '' Use the UserId to retrieve a TagDescription object, that
                    '' describes the OPC tag, that has became active (or inactive).
                    Dim tagDescr As TagDescription = tagDescrs(e.ItemIds(i).UserId)
                    '' Read the device and update the OPC tag value.
                    ReadDevice(tagDescr)
                End If
            Next
        Finally
            '' Finish the OPC server cache update transaction. We pass true,
            '' because it is necessary to wait until this transaction completes,
            '' before the control returns to the calling client.
            '' If refuse to do so, then the client will possibly recieve the old
            '' tag values, because the new ones will not be placed in the OPC server
            '' cache yet.
            srv.EndUpdate(True)
        End Try
    End Sub

    ''' <summary>
    ''' A handler for the ServerReleased event of the OPCDAServer object.
    ''' </summary>
    Shared Sub Events_ServerReleased(ByVal sender As Object, ByVal e As ServerReleasedArgs)
        '' Signal to the main thread, that it's time to exit.
        eventStop.Set()
    End Sub
End Class
