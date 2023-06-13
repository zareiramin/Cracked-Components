Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Imports TMW.SCL
Imports TMW.SCL.ProtocolAnalyzer
Imports TMW.SCL.DNP
Imports TMW.SCL.DNP.Slave


Namespace DNPslaveGUI
	Public Partial Class SlaveForm
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
		Private db As SDNPSimDatabase
		Private slaveSesn As SDNPSession
		Private slaveChan As DNPChannel
		Private pauseAnalyzer As Boolean


    Public Sub New()

      Dim applBuilder As New TMWApplicationBuilder()
      pAppl = TMWApplicationBuilder.getAppl()
      pAppl.EnableEventProcessor = True
      pAppl.InitEventProcessor()

      protocolBuffer = TMWApplicationBuilder.getProtocolBuffer()
      AddHandler protocolBuffer.ProtocolDataReadyEvent, AddressOf ProtocolEvent
      protocolBuffer.EnableCheckForDataTimer = True

      InitializeComponent()

      slaveChan = New DNPChannel(TMW_CHANNEL_OR_SESSION_TYPE.SLAVE)

      slaveChan.Type = WINIO_TYPE.TCP
      slaveChan.Name = ".NET DNP Slave"
      ' name displayed in analyzer window 
      slaveChan.WinTCPipAddress = "127.0.0.1"
      slaveChan.WinTCPipPort = 20000
      slaveChan.WinTCPmode = TCP_MODE.SERVER
      slaveChan.OpenChannel()

      slaveSesn = New SDNPSession(slaveChan)

      slaveSesn.BinaryInputScanPeriod = 500
      slaveSesn.AnalogInputScanPeriod = 500
      slaveSesn.BinaryCounterScanPeriod = 500

      slaveSesn.OpenSession()

      db = DirectCast(slaveSesn.SimDatabase, SDNPSimDatabase)
      AddHandler db.UpdateDBEvent, AddressOf UpdateDBEvent

      ' Register to receive notification of database changes


      customizeDatabase()
    End Sub

		Private Delegate Sub UpdatePointDelegate(ByVal simPoint As TMWSimPoint)

    Public Function IIf(Of T)(ByVal expression As Boolean, ByVal truePart As T, ByVal falsePart As T) As T
      If expression Then
        Return truePart
      Else
        Return falsePart
      End If
    End Function

    Private Sub updateCROB(ByVal simPoint As TMWSimPoint)
      Dim crobVal As Boolean = TryCast(simPoint, SDNPSimBinOut).Value
      Dim strVal As String = IIf(crobVal, "On", "Off")
      Dim textColor As Color = IIf(crobVal, Color.ForestGreen, Color.Red)

      Select Case simPoint.PointNumber
        Case 0
          Me.DO0Val.Text = strVal
          Me.DO0Val.ForeColor = textColor
          Exit Select
        Case 1
          Me.DO1Val.Text = strVal
          Me.DO1Val.ForeColor = textColor
          Exit Select
        Case 2
          Me.DO2Val.Text = strVal
          Me.DO2Val.ForeColor = textColor
          Exit Select
        Case Else
          protocolBuffer.Insert("Got update for uknown Binary Output point")
          Exit Select
      End Select
    End Sub

		Private Sub UpdateAnlgOut(ByVal simPoint As TMWSimPoint)
			Select Case simPoint.PointNumber
				Case 0
					Me.AO0Val.Text = TryCast(simPoint, SDNPSimAnlgOut).Value.ToString()
					Exit Select
				Case 1
					Me.AO1Val.Text = TryCast(simPoint, SDNPSimAnlgOut).Value.ToString()
					Exit Select
				Case 2
					Me.AO2Val.Text = TryCast(simPoint, SDNPSimAnlgOut).Value.ToString()
					Exit Select
				Case Else
					protocolBuffer.Insert("Got update for unknown Analog Output point")
					Exit Select
			End Select
		End Sub

		Private Sub UpdateDBEvent(ByVal simPoint As TMWSimPoint)
			If Me.InvokeRequired Then
        Me.BeginInvoke(New UpdatePointDelegate(AddressOf UpdateDBEvent), New Object() {simPoint})
			Else


				Select Case simPoint.PointType
					Case 10
						' Binary Output (CROB)
						updateCROB(simPoint)
						Exit Select
					Case 40
						' Analog Output
						UpdateAnlgOut(simPoint)
						Exit Select
					Case Else
						protocolBuffer.Insert("Unknown point type in database update routine")
						Exit Select

				End Select
			End If

		End Sub

		Private Sub ScrollToBottom()
      SendMessage(New HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), WM_VSCROLL, SB_BOTTOM, 0)
		End Sub

		Private Sub BeginUpdate()
			' Prevent the control from raising any events
      _OldEventMask = SendMessage(New HandleRef(protocolAnalyzer, protocolAnalyzer.Handle), EM_SETEVENTMASK, 0, 0)

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
          Dim pdo As ProtocolDataObject = protocolBuffer.getPdoAtIndex(i)
          If pdo IsNot Nothing Then
            protocolAnalyzer.AppendText(pdo.ProtocolText)
          End If
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

		Private Sub customizeDatabase()
			Dim i As Integer

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

				db.AddBinIn(False, 1, TMW_CLASS_MASK.ONE)
				db.AddBinOut(False, 1, TMW_CLASS_MASK.THREE, 63)
				db.AddAnlgIn(0, 1, TMW_CLASS_MASK.TWO, 5)
				db.AddAnlgOut(0, 1, TMW_CLASS_MASK.THREE)
				db.AddBinCntr(0, 1, TMW_CLASS_MASK.TWO, TMW_CLASS_MASK.TWO)
			Next

			' Don't add any of the following data types:
'       *   Double Bit Input
'       *   String
'       *   Vterm
'       

		End Sub

		Private Sub BinaryInput_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
			Dim pointNumber As UShort = Convert.ToUInt16(TryCast(sender, Control).Tag)
			Dim point As SDNPSimBinIn = db.LookupBinIn(pointNumber)
			Dim val As BoolPointValue = DirectCast(point.PointValue, BoolPointValue)
			val.Value = TryCast(sender, CheckBox).Checked
		End Sub

		Private Sub AnalogInput_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
			Dim pointNumber As UShort = Convert.ToUInt16(TryCast(sender, Control).Tag)
			Dim point As SDNPSimAnlgIn = db.LookupAnlgIn(pointNumber)
			Dim val As DoublePointValue = DirectCast(point.PointValue, DoublePointValue)
			val.Value = Convert.ToDouble(TryCast(sender, NumericUpDown).Value)
		End Sub

		Private Sub Counter_Click(ByVal sender As Object, ByVal e As EventArgs)
			Dim pointNumber As UShort = Convert.ToUInt16(TryCast(sender, Control).Tag)
      Dim point As SDNPSimBinCntr = db.LookupBinCntr(pointNumber)
      Dim val As UInteger

      point.Value = point.Value + Convert.ToUInt32(1)


      val = Convert.ToUInt32(point.Value)
      Select Case pointNumber
        Case 0
          Counter0.Text = val.ToString()
          Exit Select
        Case 1
          Counter1.Text = val.ToString()
          Exit Select
        Case 2
          Counter2.Text = val.ToString()
          Exit Select
        Case Else
          Exit Select
      End Select
		End Sub

		Private Sub protocolAnalyzer_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
			If e.Clicks = 2 Then
				' double click toggles pausing
				If pauseAnalyzer Then
					' it's paused, so unpause it
					pauseAnalyzer = False
					Me.protocolAnalyzer.BackColor = Color.Gainsboro
				Else
					' it's not paused, so pause it
					pauseAnalyzer = True
					Me.protocolAnalyzer.BackColor = Color.MistyRose
				End If
			End If
		End Sub
	End Class
End Namespace
