Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic

Imports TMW.SCL
Imports TMW.SCL.ProtocolAnalyzer

Imports TMW.SCL.DNP
Imports TMW.SCL.DNP.Master

Namespace DNPmasterGUI
	Public Partial Class MasterForm
		Inherits Form
		Private Const WM_VSCROLL As Integer = 277
		Private Const SB_BOTTOM As Integer = 7
		Private _OldEventMask As Integer = 0
		Private Const WM_SETREDRAW As Integer = 11
		Private Const EM_SETEVENTMASK As Integer = 1073

		<DllImport("user32", CharSet := CharSet.Auto)> _
		Private Shared Function SendMessage(ByVal hWnd As HandleRef, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
		End Function

		Shared pAppl As TMWApplication

		Private protocolBuffer As ProtocolBuffer
		Private db As MDNPSimDatabase
		Private masterSesn As MDNPSession
		Private masterChan As DNPChannel
		Private pauseAnalyzer As Boolean

		' Timer values
		Private integrityPollInterval As Decimal
		Private integrityPollCount As Decimal
		Private eventPollInterval As Decimal
		Private eventPollCount As Decimal

    Public Sub New()

      Dim applBuilder As New TMWApplicationBuilder()
      pAppl = TMWApplicationBuilder.getAppl()
      pAppl.EnableEventProcessor = True
      pAppl.InitEventProcessor()

      protocolBuffer = TMWApplicationBuilder.getProtocolBuffer()
      AddHandler protocolBuffer.ProtocolDataReadyEvent, AddressOf ProtocolEvent
      protocolBuffer.EnableCheckForDataTimer = True
      InitializeComponent()

      masterChan = New DNPChannel(TMW_CHANNEL_OR_SESSION_TYPE.MASTER)

      masterChan.Type = WINIO_TYPE.TCP
      masterChan.Name = ".NET DNP Master"
      ' name displayed in analyzer window 
      masterChan.WinTCPipAddress = "127.0.0.1"
      masterChan.WinTCPipPort = 20000
      masterChan.WinTCPmode = TCP_MODE.CLIENT
      masterChan.OpenChannel()

      masterSesn = New MDNPSession(masterChan)
      masterSesn.AuthenticationEnabled = False

      masterSesn.OpenSession()
      AddHandler masterSesn.SessionStatisticsEvent, AddressOf masterSesn_SessionStatisticsEvent

      db = DirectCast(masterSesn.SimDatabase, MDNPSimDatabase)
      AddHandler db.UpdateDBEvent, AddressOf UpdateDBEvent
      ' Register to receive notification of database changes

      customizeDatabase()

      ' Set up integrity poll timer
      integrityPollCount = 0
      integrityPollInterval = 3600
      ' Once per hour
      IntegrityProgressBar.Value = 0
      IntegrityProgressBar.Maximum = CInt(integrityPollInterval)
      IntegrityPollTimer.Start()

      ' Set up event poll timer
      eventPollCount = 0
      eventPollInterval = 5
      EventProgressBar.Value = 0
      EventProgressBar.Maximum = CInt(eventPollInterval)
      EventPollTimer.Start()
    End Sub

    Private Function masterSesn_SessionStatisticsEvent(ByVal session As TMWSession, ByVal statData As TMWSessionStatData) As Integer
      protocolBuffer.Insert("SESSION STATISTIC: " & statData.StatType.ToString() & Constants.vbLf)
    End Function

    Private Delegate Sub UpdatePointDelegate(ByVal simPoint As TMWSimPoint)

    Private Sub customizeDatabase()
      Dim i As UShort

      db.Clear()
      For i = 0 To 2

        ' Add 3 of each of the following types:
        '       *  Binary Input
        '       *    Value = False, Flags = onLine, class = 1
        '       *  Binary Output
        '       *    Value = False, Flags = onLine, class = 3, ControlMask = 0x3ff (allow all control operations)
        '       *  Analog Input
        '       *    Value = 0, Flags = 0, classMask = 2, Deadband = 5
        '       *  Analog Output
        '       *    Value = 0, Flags = onLine; classMask = 3
        '       *  Binary Counter
        '       *    Value = 0, Flags = onLIne, classMask = 2, frozenClassMask = 2
        '       

        db.AddBinIn(i)
        db.AddBinOut(i)
        db.AddAnlgIn(i)
        db.AddAnlgOut(i)
        db.AddBinCntr(i)
      Next

      ' Don't add any of the following data types:
      '       *   Double Bit Input
      '       *   String
      '       *   Vterm
      '       

    End Sub

    Private Sub ScrollToBottom()
      SendMessage(New HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), WM_VSCROLL, SB_BOTTOM, 0)
    End Sub

    Private Sub BeginUpdate()
      ' Prevent the control from raising any events
      _OldEventMask = SendMessage(New HandleRef(protocolAnalyzer, Handle), EM_SETEVENTMASK, 0, 0)

      ' Prevent the control from redrawing itself
      SendMessage(New HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), WM_SETREDRAW, 0, 0)
    End Sub

    Private Sub EndUpdate()
      ' Allow the control to redraw itself
      SendMessage(New HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), WM_SETREDRAW, 1, 0)

      ' Allow the control to raise event messages
      SendMessage(New HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), EM_SETEVENTMASK, 0, _OldEventMask)
    End Sub

    Private Sub RemoveTopLines(ByVal numLines As Integer)
      Dim lastLine As Integer = protocolAnalyzer.Lines.GetLength(0) - 1
      If numLines < 1 Then
        Return
      ElseIf numLines > lastLine Then
        numLines = lastLine
      End If

      Dim startChar As Integer = protocolAnalyzer.GetFirstCharIndexFromLine(0)
      Dim endChar As Integer = protocolAnalyzer.GetFirstCharIndexFromLine(numLines)

      Dim b As Boolean = protocolAnalyzer.[ReadOnly]
      protocolAnalyzer.[ReadOnly] = False
      protocolAnalyzer.[Select](startChar, endChar - startChar)
      protocolAnalyzer.SelectedRtf = ""
      protocolAnalyzer.[ReadOnly] = b
    End Sub

    Private Sub ProtocolEvent(ByVal buf As ProtocolBuffer)
      If Not pauseAnalyzer Then
        buf.Lock()
        For i As Integer = buf.LastProvidedIndex To buf.LastAddedIndex - 1
          protocolAnalyzer.AppendText(protocolBuffer.getPdoAtIndex(i).ProtocolText)
          SendMessage(New HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), WM_VSCROLL, SB_BOTTOM, 0)
        Next
        buf.UnLock()

        ' remove lines from the text box
        If protocolAnalyzer.Lines.Length > 1000 Then
          BeginUpdate()
          RemoveTopLines(100)
          ScrollToBottom()
          EndUpdate()
        End If
      End If
    End Sub
    Public Function IIf(Of T)(ByVal expression As Boolean, ByVal truePart As T, ByVal falsePart As T) As T
      If expression Then
        Return truePart
      Else
        Return falsePart
      End If
    End Function

    Private Sub updateBinaryInput(ByVal simPoint As TMWSimPoint)
      Dim strVal As String = IIf(TryCast(simPoint, MDNPSimBinIn).Value, "On", "Off")
      Dim textColor As Color = IIf(TryCast(simPoint, MDNPSimBinIn).Value, Color.ForestGreen, Color.Red)

      Select Case simPoint.PointNumber
        Case 0
          BinIn0.Text = strVal
          BinIn0.ForeColor = textColor
          Exit Select
        Case 1
          BinIn1.Text = strVal
          BinIn1.ForeColor = textColor
          Exit Select
        Case 2
          BinIn2.Text = strVal
          BinIn2.ForeColor = textColor
          Exit Select
        Case Else
          protocolBuffer.Insert("Received update for unexpected binary input point: " + simPoint.PointNumber.ToString())
          Exit Select
      End Select
    End Sub

    Private Sub updateBinaryOutput(ByVal simPoint As TMWSimPoint)
      Dim strVal As String = IIf(TryCast(simPoint, MDNPSimBinOut).Value, "On", "Off")
      Dim textColor As Color = IIf(TryCast(simPoint, MDNPSimBinOut).Value, Color.ForestGreen, Color.Red)

      Select Case simPoint.PointNumber
        Case 0
          BinOut0Feedback.Text = strVal
          BinOut0Feedback.ForeColor = textColor
          Exit Select
        Case 1
          BinOut1Feedback.Text = strVal
          BinOut1Feedback.ForeColor = textColor
          Exit Select
        Case 2
          BinOut2Feedback.Text = strVal
          BinOut2Feedback.ForeColor = textColor
          Exit Select
        Case Else
          protocolBuffer.Insert("Received update for unexpected binary output point: " + simPoint.PointNumber.ToString())
          Exit Select
      End Select
    End Sub

    Private Sub updateBinCntr(ByVal simPoint As TMWSimPoint)
      Select Case simPoint.PointNumber
        Case 0
          BinCntr0.Text = TryCast(simPoint, MDNPSimBinCntr).Value.ToString()
          Exit Select
        Case 1
          BinCntr1.Text = TryCast(simPoint, MDNPSimBinCntr).Value.ToString()
          Exit Select
        Case 2
          BinCntr2.Text = TryCast(simPoint, MDNPSimBinCntr).Value.ToString()
          Exit Select
        Case Else
          protocolBuffer.Insert("Received update for unexpected analog input point: " + simPoint.PointNumber.ToString())
          Exit Select
      End Select
    End Sub

    Private Sub updateAnalogInput(ByVal simPoint As TMWSimPoint)
      Select Case simPoint.PointNumber
        Case 0
          AnlgIn0.Text = TryCast(simPoint, MDNPSimAnlgIn).Value.ToString()
          Exit Select
        Case 1
          AnlgIn1.Text = TryCast(simPoint, MDNPSimAnlgIn).Value.ToString()
          Exit Select
        Case 2
          AnlgIn2.Text = TryCast(simPoint, MDNPSimAnlgIn).Value.ToString()
          Exit Select
        Case Else
          protocolBuffer.Insert("Received update for unexpected analog input point: " + simPoint.PointNumber.ToString())
          Exit Select
      End Select
    End Sub

    Private Sub updateAnalogOutput(ByVal simPoint As TMWSimPoint)
      Select Case simPoint.PointNumber
        Case 0
          AnlgOut0Feedback.Text = TryCast(simPoint, MDNPSimAnlgOut).Value.ToString()
          Exit Select
        Case 1
          AnlgOut1Feedback.Text = TryCast(simPoint, MDNPSimAnlgOut).Value.ToString()
          Exit Select
        Case 2
          AnlgOut2Feedback.Text = TryCast(simPoint, MDNPSimAnlgOut).Value.ToString()
          Exit Select
        Case Else
          protocolBuffer.Insert("Received update for unexpected analog output point: " + simPoint.PointNumber.ToString())
          Exit Select
      End Select
    End Sub


    Private Sub UpdateDBEvent(ByVal simPoint As TMWSimPoint)
      If InvokeRequired Then
        BeginInvoke(New UpdatePointDelegate(AddressOf UpdateDBEvent), New Object() {simPoint})
      Else
        Select Case simPoint.PointType
          Case 1
            ' Binary Input
            updateBinaryInput(simPoint)
            Exit Select
          Case 10
            ' Binary Output (CROB)
            updateBinaryOutput(simPoint)
            Exit Select
          Case 20
            ' Binary Counters
            updateBinCntr(simPoint)
            Exit Select
          Case 30
            ' Analog Inputs
            updateAnalogInput(simPoint)
            Exit Select
          Case 40
            ' Analog Output
            updateAnalogOutput(simPoint)
            Exit Select
          Case Else
            protocolBuffer.Insert("Unknown point type in database update routine")
            Exit Select

        End Select
      End If

    End Sub

    Private Sub IntegrityPollTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
      If System.Math.Max(System.Threading.Interlocked.Increment(Convert.ToInt32(integrityPollCount)), integrityPollCount - 1) < integrityPollInterval Then
        IntegrityProgressBar.Increment(1)
      Else
        ' time to do a poll

        Integrity_Click(sender, e)

        integrityPollCount = 0
        IntegrityProgressBar.Value = 0
        IntegrityProgressBar.Maximum = CInt(integrityPollInterval)
        IntegrityPollTimer.Start()
      End If
    End Sub

    Private Sub EventPollTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)

      If eventPollCount <= eventPollInterval Then
        EventProgressBar.Increment(1)
      Else
        ' time to do a poll
        Event_Click(sender, e)

        eventPollCount = 0
        EventProgressBar.Value = 0
        EventProgressBar.Maximum = CInt(eventPollInterval)
      End If
      eventPollCount = eventPollCount + 1
    End Sub

    Private Sub Integrity_Click(ByVal sender As Object, ByVal e As EventArgs)
      protocolBuffer.Insert("" & Chr(10) & "Requested Integrity Poll" & Chr(10) & "")
      Dim request As New MDNPRequest(masterSesn)
      request.IntegrityPoll(True)
    End Sub

    Private Sub Event_Click(ByVal sender As Object, ByVal e As EventArgs)
      protocolBuffer.Insert("" & Chr(10) & "Requested Event Poll" & Chr(10) & "")

      Dim request As New MDNPRequest(masterSesn)
      request.ReadClass(MDNPRequest.DNP_QUALIFIER.Q_ALL_POINTS, 0, False, True, True, True)

    End Sub

    Private Sub AnlgOut2Val_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Private Sub BinOutOn_Click(ByVal sender As Object, ByVal e As EventArgs)
      ' Determine which point this request is for
      Dim pointNumber As UShort = Convert.ToUInt16(TryCast(sender, Control).Tag)

      ' Build and send the request
      Dim crobData As New CROBInfo(pointNumber, CROBInfo.CROB_CTRL.LON, 0, 0)
      Dim crobArray As CROBInfo() = {crobData}

      Dim point As MDNPSimBinOut = db.LookupBinOut(pointNumber)
      Dim request As New MDNPRequest(masterSesn)
      request.BinaryCommand(MDNPRequest.DNP_FUNCTION_CODE.[SELECT], True, True, 100, MDNPRequest.DNP_QUALIFIER.Q_8BIT_INDEX, crobArray)
      ' request.BinaryOutWrite((byte)DNP_QUALIFIER.Q_8BIT_START_STOP, pointNumber, pointNumber, val);
    End Sub

    Private Sub BinOutOff_Click(ByVal sender As Object, ByVal e As EventArgs)
      ' Determine which point this request is for
      Dim pointNumber As UShort = Convert.ToUInt16(TryCast(sender, Control).Tag)

      ' Build and send the request
      Dim crobData As New CROBInfo(pointNumber, CROBInfo.CROB_CTRL.LOFF, 0, 0)
      Dim crobArray As CROBInfo() = {crobData}

      Dim point As MDNPSimBinOut = db.LookupBinOut(pointNumber)
      Dim request As New MDNPRequest(masterSesn)
      request.BinaryCommand(MDNPRequest.DNP_FUNCTION_CODE.[SELECT], True, True, 100, MDNPRequest.DNP_QUALIFIER.Q_8BIT_INDEX, crobArray)
      ' request.BinaryOutWrite((byte)DNP_QUALIFIER.Q_8BIT_START_STOP, pointNumber, pointNumber, val);
    End Sub

    Private Sub AnlgOutSend_Click(ByVal sender As Object, ByVal e As EventArgs)
      Dim val As Double
      ' Determine which point this request is for
      Dim pointNumber As UShort = Convert.ToUInt16(TryCast(sender, Control).Tag)

      ' Get the value to write for this point
      Select Case pointNumber
        Case 0
          val = CDbl(AnlgOut0Val.Value)
          Exit Select
        Case 1
          val = CDbl(AnlgOut1Val.Value)
          Exit Select
        Case 2
          val = CDbl(AnlgOut2Val.Value)
          Exit Select
        Case Else
          val = 0
          protocolBuffer.Insert("Unexected analog output command for undefined point: " + pointNumber.ToString())
          Exit Select
      End Select

      ' Build and send the request
      Dim anlgData As New AnalogInfo(pointNumber, val)
      Dim anlgArray As AnalogInfo() = {anlgData}

      Dim point As MDNPSimAnlgOut = db.LookupAnlgOut(pointNumber)
      Dim request As New MDNPRequest(masterSesn)
      request.AnalogCommand(MDNPRequest.DNP_FUNCTION_CODE.[SELECT], True, True, 100, MDNPRequest.DNP_QUALIFIER.Q_8BIT_INDEX, 2, _
       anlgArray)
    End Sub

    Private Sub IntegrityInterval_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
      IntegrityPollTimer.[Stop]()
      integrityPollInterval = (IntegrityIntervalHr.Value * 3600) + (IntegrityIntervalMin.Value * 60) + (IntegrityIntervalSec.Value)
      IntegrityProgressBar.Maximum = CInt(integrityPollInterval)

      If integrityPollInterval = 0 Then
        IntegrityEnable.Checked = False
      End If

      If IntegrityEnable.Checked Then
        IntegrityPollTimer.Start()
      Else
        ' if unchecked, reset the progress bar and current count
        IntegrityProgressBar.Value = 0
        integrityPollCount = 0
      End If
    End Sub

    Private Sub EventInterval_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
      EventPollTimer.[Stop]()
      eventPollInterval = (EventIntervalHr.Value * 3600) + (EventIntervalMin.Value * 60) + (EventIntervalSec.Value)
      EventProgressBar.Maximum = CInt(eventPollInterval)

      If eventPollInterval = 0 Then
        EventEnable.Checked = False
      End If

      If EventEnable.Checked Then
        EventPollTimer.Start()
      Else
        ' if unchecked, reset the progress bar and current count to 0
        EventProgressBar.Value = 0
        eventPollCount = 0
      End If
    End Sub

    Private Sub protocolAnalyzer_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
      If e.Clicks = 2 Then
        ' double click toggles pausing
        If pauseAnalyzer Then
          ' it's paused, so unpause it
          pauseAnalyzer = False
          protocolAnalyzer.BackColor = Color.Gainsboro
        Else
          ' it's not paused, so pause it
          pauseAnalyzer = True
          protocolAnalyzer.BackColor = Color.MistyRose
        End If
      End If
    End Sub
    End Class
End Namespace
