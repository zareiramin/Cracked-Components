Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Threading

Imports TMW.SCL
Imports TMW.SCL.DNP
Imports TMW.SCL.DNP.Master
Imports TMW.SCL.DNP.Slave
Imports TMW.SCL.ProtocolAnalyzer


Namespace DNPDatabaseEvents
	Class Program

    Private Shared bSerialMode As Boolean = False

    Private Shared mdb As MDNPDatabase
    Private Shared masterSesn As MDNPSession
    Private Shared masterChan As DNPChannel

    Private Shared sdb As SDNPDatabase
    Private Shared slaveSesn As SDNPSession
    Private Shared slaveChan As DNPChannel


    Public Shared Sub Main(ByVal args As String())
      Console.WriteLine("Press 'Y' to communicate on Serial ports COM3(for slave) and COM4(for master), press any other key to continue")
      Dim key As ConsoleKeyInfo = Console.ReadKey()
      If key.Key = ConsoleKey.Y Then
        bSerialMode = True
      End If

      Console.WriteLine("")
      Console.WriteLine("Press Enter to end test")
      Console.WriteLine("")

      Try
       
        Dim applBuilder As New TMWApplicationBuilder()
        Dim pAppl As TMWApplication = TMWApplicationBuilder.getAppl()
        pAppl.EnableEventProcessor = True
        pAppl.InitEventProcessor()

        OpenSlave()
        OpenMaster()

        Thread.Sleep(1000)

        ' Issue an integrity poll
        Dim request As New MDNPRequest(masterSesn)
        request.IntegrityPoll(True)
      Catch e As Exception
        Console.WriteLine(e.ToString())
      End Try
      Console.ReadKey()
    End Sub

    ''' <summary>
    ''' Called by SCL when database is first opened
    ''' </summary>
    ''' <param name="db">database object that called the event</param>
    Private Shared Sub SlaveInitEvent(ByVal db As SDNPDatabase)
      Console.WriteLine("Slave:InitEvent called")

      ''' Set database Tag to something meaningful, like a pointer or reference
      ''' It will be available to all database Event routines. 
      db.Tag = 44

      ''' Perform other initialization processing here.
      Return
    End Sub
    ''' <summary>
    ''' Called by SCL to get the number of points
    ''' </summary>
    ''' <param name="db">database object that called the event</param>
    ''' <returns>number of points</returns>
    Private Shared Function SlaveBinInQuantityEvent(ByVal db As SDNPDatabase) As UShort
      Console.WriteLine("Slave:BinInQuantity: return 2 points")
      Return 2
    End Function

    ''' <summary>
    ''' Called by SCL to get a cookie for the given point number
    ''' </summary>
    ''' <param name="db">database that called this event</param>
    ''' <param name="pointNum">point number</param>
    ''' <returns>cookie that identifies point</returns>
    Private Shared Function SlaveBinInGetPointEvent(ByVal db As SDNPDatabase, ByVal pointNum As UShort) As Integer
      Dim cookie As Integer = pointNum + 100
      Console.WriteLine("Slave:BinInGetPoint: Return {0} as cookie for Point Number {1}", cookie, pointNum)
      Return cookie
    End Function

    ''' <summary>
    ''' Called by SCL to get value
    ''' </summary>
    ''' <param name="pointId">cookie of the point to access</param>
    ''' <param name="value">value to be returned</param>
    Private Shared Sub SlaveBinInReadEvent(ByVal pointId As Integer, ByRef value As Boolean, ByVal flags As DNPDatabaseFlags)
      If pointId = 100 Then
        Console.WriteLine("Slave:BinInRead: Point ID: {0} Set value = true", pointId)
        value = True
        flags.Value = DNPDatabaseFlags.ON_LINE
      ElseIf pointId = 101 Then
        Console.WriteLine("Slave:BinInRead: Point ID: {0} Set value = false", pointId)
        value = False
        flags.Value = DNPDatabaseFlags.OFF_LINE
      Else
        Throw New Exception("Unknown point id")
      End If
    End Sub

    ''' <summary>
    ''' Called by SCL when database is first opened
    ''' </summary>
    ''' <param name="db">database object that called the event</param>
    Private Shared Sub MasterInitEvent(ByVal db As MDNPDatabase)
      Console.WriteLine("Master:InitEvent called")

      ''' Set database Tag to something meaningful, like a pointer or reference
      ''' It will be available to all database Event routines. 
      db.Tag = 55

      ''' Perform other initialization processing here.

      Return
    End Sub
    ''' <summary>
    ''' Called by the SCL to store a binary input
    ''' </summary>
    ''' <param name="db">database object that called the event</param>
    ''' <param name="pointNumber">point number</param>
    ''' <param name="value">value</param>
    ''' <param name="flags">flags</param>
    ''' <param name="isEvent">event</param>
    ''' <param name="pTimeStamp">time stamp</param>
    ''' <returns>success/failure</returns>
    Private Shared Function Master_StoreBinaryInputEvent(ByVal db As MDNPDatabase, ByVal pointNumber As UShort, ByVal value As Boolean, ByVal flags As DNPDatabaseFlags, ByVal isEvent As Boolean, ByVal pTimeStamp As TMWTime) As Boolean
      Console.WriteLine("Master:StoreBinaryInput: Point Number {0} Value {1} Flags {2}", pointNumber, value, flags.ToString())
      Return True
    End Function

    ''' <summary>
    ''' Called by SCL to get the number of points
    ''' </summary>
    ''' <param name="db">database object that called the event</param>
    ''' <returns>number of points</returns>
    Private Shared Function SlaveBinOutQuantityEvent(ByVal db As SDNPDatabase) As Byte
      Console.WriteLine("Slave:BinOutQuantity: Set Bin Out to 2 points")
      Return 2
    End Function

    ''' <summary>
    ''' Called by SCL to get a cookie for the given point number
    ''' </summary>
    ''' <param name="db">database that called this event</param>
    ''' <param name="pointNum">point number</param>
    ''' <returns>cookie that identifies point</returns>
    Private Shared Function SlaveBinOutGetPointEvent(ByVal db As SDNPDatabase, ByVal pointNum As UShort) As Integer
      Dim cookie As Integer = pointNum + 300
      Console.WriteLine("Slave:BinOutGetPoint: Return {0} as cookie for Point Number {1}", cookie, pointNum)
      Return cookie
    End Function

    ''' <summary>
    ''' Called by SCL to get value
    ''' </summary>
    ''' <param name="pointId">cookie of the point to access</param>
    ''' <param name="value">value to be returned</param>
    Private Shared Sub SlaveBinOutReadEvent(ByVal pointId As Integer, ByRef value As Boolean, ByVal flags As DNPDatabaseFlags)
      If pointId = 300 Then
        Console.WriteLine("Slave:BinOutRead: Point ID: {0} Set value = true", pointId)
        value = True
        flags.Value = DNPDatabaseFlags.ON_LINE
      ElseIf pointId = 301 Then
        Console.WriteLine("Slave:BinOutRead: Point ID: {0} Set value = false", pointId)
        value = False
        flags.Value = DNPDatabaseFlags.OFF_LINE
      Else
        Throw New Exception("Unknown point id")
      End If
    End Sub

    ''' <summary>
    ''' Called by the SCL to store a binary output
    ''' </summary>
    ''' <param name="db">database object that called the event</param>
    ''' <param name="pointNumber">point number</param>
    ''' <param name="value">value</param>
    ''' <param name="flags">flags</param>
    ''' <param name="isEvent">event</param>
    ''' <param name="pTimeStamp">time stamp</param>
    ''' <returns>success/failure</returns>
    Private Shared Function Master_StoreBinaryOutputEvent(ByVal db As MDNPDatabase, ByVal pointNumber As UShort, ByVal value As Boolean, ByVal flags As DNPDatabaseFlags, ByVal isEvent As Boolean, ByVal pTimeStamp As TMWTime) As Boolean
      Console.WriteLine("Master:StoreBinaryOutput: Point Number {0} Value {1} Flags {2}", pointNumber, value, flags.ToString())
      Return True
    End Function

    ''' <summary>
    ''' Called by SCL to get the number of points
    ''' </summary>
    ''' <param name="db">database object that called the event</param>
    ''' <returns>number of points</returns>
    Private Shared Function SlaveAnlgOutQuantityEvent(ByVal db As SDNPDatabase) As Byte
      Console.WriteLine("Slave:AnlgOutQuantity: return 2 points")
      Return 2
    End Function

    ''' <summary>
    ''' Called by SCL to get a cookie for the given point number
    ''' </summary>
    ''' <param name="db">database that called this event</param>
    ''' <param name="pointNum">point number</param>
    ''' <returns>cookie that identifies point</returns>
    Private Shared Function SlaveAnlgOutGetPointEvent(ByVal db As SDNPDatabase, ByVal pointNum As UShort) As Integer
      Dim cookie As Integer = pointNum + 400
      Console.WriteLine("Slave:AnlgOutGetPoint: Return {0} as cookie for Point Number {1}", cookie, pointNum)
      Return cookie
    End Function

    ''' <summary>
    ''' Called by SCL to get value
    ''' </summary>
    ''' <param name="pointId">cookie of the point to access</param>
    ''' <param name="value">value to be returned</param>
    ''' <param name="Flags">flags</param>
    Private Shared Sub SlaveAnlgOutReadEvent(ByVal pointId As Integer, ByVal pValue As TMWAnalogVal, ByVal flags As DNPDatabaseFlags)
      If pointId = 400 Then
        Console.WriteLine("Slave:AnlgOutRead: Point ID: {0} Set value = 15", pointId)
        pValue.SetShortValue(15)
        flags.Value = DNPDatabaseFlags.ON_LINE
      ElseIf pointId = 401 Then
        Console.WriteLine("Slave:AnlgOutRead: Point ID: {0} Set value = 16", pointId)
        pValue.SetShortValue(16)
        flags.Value = DNPDatabaseFlags.ON_LINE
      Else
        Throw New Exception("Unknown point id")
      End If
    End Sub

    ''' <summary>
    ''' Called by the SCL to store a analog output
    ''' </summary>
    ''' <param name="db">database object that called the event</param>
    ''' <param name="pointNumber">point number</param>
    ''' <param name="value">value</param>
    ''' <param name="flags">flags</param>
    ''' <param name="isEvent">event</param>
    ''' <param name="pTimeStamp">time stamp</param>
    ''' <returns>success/failure</returns>
    Private Shared Function Master_StoreAnalogOutputEvent(ByVal db As MDNPDatabase, ByVal pointNumber As UShort, ByVal value As TMWAnalogVal, ByVal flags As DNPDatabaseFlags, ByVal isEvent As Boolean, ByVal pTimeStamp As TMWTime) As Boolean
      Console.WriteLine("Master:StoreAnalogOutput: Point Number {0} Value {1} Flags {2}", pointNumber, value.ToString(), flags.ToString())
      Return True
    End Function

    ''' <summary>
    ''' Called by SCL to get the number of points
    ''' </summary>
    ''' <param name="db">database object that called the event</param>
    ''' <returns>number of points</returns>
    Private Shared Function SlaveAnlgInQuantityEvent(ByVal db As SDNPDatabase) As Byte
      Console.WriteLine("Slave:AnlgInQuantity: return 2 points")
      Return 2
    End Function

    ''' <summary>
    ''' Called by SCL to get a cookie for the given point number
    ''' </summary>
    ''' <param name="db">database that called this event</param>
    ''' <param name="pointNum">point number</param>
    ''' <returns>cookie that identifies point</returns>
    Private Shared Function SlaveAnlgInGetPointEvent(ByVal db As SDNPDatabase, ByVal pointNum As UShort) As Integer
      Dim cookie As Integer = pointNum + 500
      Console.WriteLine("Slave:AnlgInGetPoint: Return {0} as cookie for Point Number {1}", cookie, pointNum)
      Return cookie
    End Function

    ''' <summary>
    ''' Called by SCL to get value
    ''' </summary>
    ''' <param name="pointId">cookie of the point to access</param>
    ''' <param name="value">value to be returned</param>
    ''' <param name="Flags">flags</param>
    Private Shared Sub SlaveAnlgInReadEvent(ByVal pointId As Integer, ByVal pValue As TMWAnalogVal, ByVal flags As DNPDatabaseFlags)
      If pointId = 500 Then
        Console.WriteLine("Slave:AnlgInRead: Point ID: {0} Set value = 25", pointId)
        pValue.SetShortValue(25)
        flags.Value = DNPDatabaseFlags.ON_LINE
      ElseIf pointId = 501 Then
        Console.WriteLine("Slave:AnlgInRead: Point ID: {0} Set value = 26", pointId)
        pValue.SetShortValue(26)
        flags.Value = CByte((DNPDatabaseFlags.LOCAL_FORCED Or DNPDatabaseFlags.ON_LINE))
      Else
        Throw New Exception("Unknown point id")
      End If
    End Sub

    ''' <summary>
    ''' Called by the SCL to store a analog output
    ''' </summary>
    ''' <param name="db">database object that called the event</param>
    ''' <param name="pointNumber">point number</param>
    ''' <param name="value">value</param>
    ''' <param name="flags">flags</param>
    ''' <param name="isEvent">event</param>
    ''' <param name="pTimeStamp">time stamp</param>
    ''' <returns>success/failure</returns>
    Private Shared Function Master_StoreAnalogInputEvent(ByVal db As MDNPDatabase, ByVal pointNumber As UShort, ByVal value As TMWAnalogVal, ByVal flags As DNPDatabaseFlags, ByVal isEvent As Boolean, ByVal pTimeStamp As TMWTime) As Boolean
      Console.WriteLine("Master:StoreAnalogInput: Point Number {0} Value {1} Flags {2}", pointNumber, value.ToString(), flags.ToString())
      Return True
    End Function


    Private Shared Sub OpenSlave()
      slaveChan = New DNPChannel(TMW_CHANNEL_OR_SESSION_TYPE.SLAVE)

      If bSerialMode = True Then
        slaveChan.Type = WINIO_TYPE.RS232
        slaveChan.Name = ".NET DNP Slave"
        ' name displayed in analyzer window 
        slaveChan.Win232comPortName = "COM3"
        slaveChan.Win232baudRate = "9600"
        slaveChan.Win232numDataBits = RS232_DATA_BITS.BITS_8
        slaveChan.Win232numStopBits = RS232_STOP_BITS.BITS_1
        slaveChan.Win232portMode = RS232_PORT_MODE.NONE
        '				slaveChan.Win232bSyncMode = True
      Else
        slaveChan.Type = WINIO_TYPE.TCP
        slaveChan.Name = ".NET DNP Slave"
        ' name displayed in analyzer window 
        slaveChan.WinTCPipAddress = "127.0.0.1"
        slaveChan.WinTCPipPort = 20000
        slaveChan.WinTCPmode = TCP_MODE.SERVER
      End If
      slaveChan.OpenChannel()

      slaveSesn = New SDNPSession(slaveChan)
      slaveSesn.UnsolAllowed = True

      SDNPDatabase.UseSimDatabase = False
      AddHandler SDNPDatabase.InitEvent, AddressOf SlaveInitEvent
      slaveSesn.OpenSession()

      sdb = DirectCast(slaveSesn.SimDatabase, SDNPDatabase)

      AddHandler sdb.BinInQuantityEvent, AddressOf SlaveBinInQuantityEvent
      AddHandler sdb.BinInGetPointEvent, AddressOf SlaveBinInGetPointEvent
      AddHandler SDNPDatabase.BinInReadEvent, AddressOf SlaveBinInReadEvent
      AddHandler sdb.BinOutQuantityEvent, AddressOf SlaveBinOutQuantityEvent
      AddHandler sdb.BinOutGetPointEvent, AddressOf SlaveBinOutGetPointEvent
      AddHandler SDNPDatabase.BinOutReadEvent, AddressOf SlaveBinOutReadEvent
      AddHandler sdb.AnlgInQuantityEvent, AddressOf SlaveAnlgInQuantityEvent
      AddHandler sdb.AnlgInGetPointEvent, AddressOf SlaveAnlgInGetPointEvent
      AddHandler SDNPDatabase.AnlgInReadEvent, AddressOf SlaveAnlgInReadEvent
      AddHandler sdb.AnlgOutQuantityEvent, AddressOf SlaveAnlgOutQuantityEvent
      AddHandler sdb.AnlgOutGetPointEvent, AddressOf SlaveAnlgOutGetPointEvent
      AddHandler SDNPDatabase.AnlgOutReadEvent, AddressOf SlaveAnlgOutReadEvent

      ' binary inputs

      ' binary outputs

      ' analog inputs

      ' analog outputs

    End Sub

    Private Shared Sub OpenMaster()
      masterChan = New DNPChannel(TMW_CHANNEL_OR_SESSION_TYPE.MASTER)

      If bSerialMode = True Then
        masterChan.Type = WINIO_TYPE.RS232
        masterChan.Name = ".NET DNP Master"
        ' name displayed in analyzer window 
        masterChan.Win232comPortName = "COM4"
        masterChan.Win232baudRate = "9600"
        masterChan.Win232numDataBits = RS232_DATA_BITS.BITS_8
        masterChan.Win232numStopBits = RS232_STOP_BITS.BITS_1
        masterChan.Win232portMode = RS232_PORT_MODE.NONE
        '				masterChan.Win232bSyncMode = True
      Else
        masterChan.Type = WINIO_TYPE.TCP
        masterChan.Name = ".NET DNP Master"
        ' name displayed in analyzer window 
        masterChan.WinTCPipAddress = "127.0.0.1"
        masterChan.WinTCPipPort = 20000
        masterChan.WinTCPmode = TCP_MODE.CLIENT
      End If
      masterChan.OpenChannel()

      masterSesn = New MDNPSession(masterChan)
      masterSesn.AutoClassPollIIN = True
      masterSesn.AutoEnableUnsolClass1 = True
      masterSesn.AutoEnableUnsolClass2 = True
      masterSesn.AutoEnableUnsolClass3 = True

      MDNPDatabase.UseSimDatabase = False
      AddHandler MDNPDatabase.InitEvent, AddressOf MasterInitEvent
      masterSesn.OpenSession()

      mdb = DirectCast(masterSesn.SimDatabase, MDNPDatabase)

      AddHandler mdb.StoreBinaryInputEvent, AddressOf Master_StoreBinaryInputEvent
      AddHandler mdb.StoreBinaryOutputEvent, AddressOf Master_StoreBinaryOutputEvent
      AddHandler mdb.StoreAnalogInputEvent, AddressOf Master_StoreAnalogInputEvent
      AddHandler mdb.StoreAnalogOutputEvent, AddressOf Master_StoreAnalogOutputEvent

      ' register events on Master to store data

    End Sub
  End Class
End Namespace
