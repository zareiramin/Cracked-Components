Namespace DNPmasterGUI
	Partial Class MasterForm
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(MasterForm))
			Me.label1 = New System.Windows.Forms.Label()
			Me.BinOut0On = New System.Windows.Forms.Button()
			Me.BinOut0Off = New System.Windows.Forms.Button()
			Me.label2 = New System.Windows.Forms.Label()
			Me.AnlgOut0Val = New System.Windows.Forms.NumericUpDown()
			Me.AnlgOut0Send = New System.Windows.Forms.Button()
			Me.label3 = New System.Windows.Forms.Label()
			Me.label4 = New System.Windows.Forms.Label()
			Me.label5 = New System.Windows.Forms.Label()
			Me.textBox1 = New System.Windows.Forms.TextBox()
			Me.textBox2 = New System.Windows.Forms.TextBox()
			Me.BinOut1Off = New System.Windows.Forms.Button()
			Me.BinOut1On = New System.Windows.Forms.Button()
			Me.textBox3 = New System.Windows.Forms.TextBox()
			Me.BinOut2Off = New System.Windows.Forms.Button()
			Me.BinOut2On = New System.Windows.Forms.Button()
			Me.AnlgOut1Send = New System.Windows.Forms.Button()
			Me.AnlgOut1Val = New System.Windows.Forms.NumericUpDown()
			Me.AnlgOut2Send = New System.Windows.Forms.Button()
			Me.AnlgOut2Val = New System.Windows.Forms.NumericUpDown()
			Me.groupBox1 = New System.Windows.Forms.GroupBox()
			Me.BinCntr2 = New System.Windows.Forms.Label()
			Me.BinCntr1 = New System.Windows.Forms.Label()
			Me.BinCntr0 = New System.Windows.Forms.Label()
			Me.AnlgIn2 = New System.Windows.Forms.Label()
			Me.AnlgIn1 = New System.Windows.Forms.Label()
			Me.AnlgIn0 = New System.Windows.Forms.Label()
			Me.BinIn2 = New System.Windows.Forms.Label()
			Me.BinIn1 = New System.Windows.Forms.Label()
			Me.BinIn0 = New System.Windows.Forms.Label()
			Me.label8 = New System.Windows.Forms.Label()
			Me.label7 = New System.Windows.Forms.Label()
			Me.label6 = New System.Windows.Forms.Label()
			Me.groupBox2 = New System.Windows.Forms.GroupBox()
			Me.BinOut2Feedback = New System.Windows.Forms.Label()
			Me.BinOut1Feedback = New System.Windows.Forms.Label()
			Me.BinOut0Feedback = New System.Windows.Forms.Label()
			Me.AnlgOut2Feedback = New System.Windows.Forms.Label()
			Me.AnlgOut1Feedback = New System.Windows.Forms.Label()
			Me.AnlgOut0Feedback = New System.Windows.Forms.Label()
      Me.IntegrityPB = New System.Windows.Forms.Button()
      Me.EventPB = New System.Windows.Forms.Button()
      Me.IntegrityEnable = New System.Windows.Forms.CheckBox()
      Me.EventEnable = New System.Windows.Forms.CheckBox()
      Me.IntegrityIntervalHr = New System.Windows.Forms.NumericUpDown()
      Me.label9 = New System.Windows.Forms.Label()
      Me.label10 = New System.Windows.Forms.Label()
      Me.IntegrityIntervalMin = New System.Windows.Forms.NumericUpDown()
      Me.IntegrityIntervalSec = New System.Windows.Forms.NumericUpDown()
      Me.EventIntervalSec = New System.Windows.Forms.NumericUpDown()
      Me.label11 = New System.Windows.Forms.Label()
      Me.EventIntervalMin = New System.Windows.Forms.NumericUpDown()
      Me.label12 = New System.Windows.Forms.Label()
      Me.EventIntervalHr = New System.Windows.Forms.NumericUpDown()
      Me.Poll = New System.Windows.Forms.GroupBox()
      Me.EventProgressBar = New System.Windows.Forms.ProgressBar()
      Me.IntegrityProgressBar = New System.Windows.Forms.ProgressBar()
      Me.IntegrityPollTimer = New System.Windows.Forms.Timer(Me.components)
      Me.EventPollTimer = New System.Windows.Forms.Timer(Me.components)
      Me.protocolAnalyzer = New System.Windows.Forms.RichTextBox()
      Me.pictureBox1 = New System.Windows.Forms.PictureBox()
      DirectCast((Me.AnlgOut0Val), System.ComponentModel.ISupportInitialize).BeginInit()
      DirectCast((Me.AnlgOut1Val), System.ComponentModel.ISupportInitialize).BeginInit()
      DirectCast((Me.AnlgOut2Val), System.ComponentModel.ISupportInitialize).BeginInit()
      Me.groupBox1.SuspendLayout()
      Me.groupBox2.SuspendLayout()
      DirectCast((Me.IntegrityIntervalHr), System.ComponentModel.ISupportInitialize).BeginInit()
      DirectCast((Me.IntegrityIntervalMin), System.ComponentModel.ISupportInitialize).BeginInit()
      DirectCast((Me.IntegrityIntervalSec), System.ComponentModel.ISupportInitialize).BeginInit()
      DirectCast((Me.EventIntervalSec), System.ComponentModel.ISupportInitialize).BeginInit()
      DirectCast((Me.EventIntervalMin), System.ComponentModel.ISupportInitialize).BeginInit()
      DirectCast((Me.EventIntervalHr), System.ComponentModel.ISupportInitialize).BeginInit()
      Me.Poll.SuspendLayout()
      DirectCast((Me.pictureBox1), System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      ' 
      ' label1
      ' 
      Me.label1.AutoSize = True
      Me.label1.Location = New System.Drawing.Point(78, 16)
      Me.label1.Name = "label1"
      Me.label1.Size = New System.Drawing.Size(36, 13)
      Me.label1.TabIndex = 0
      Me.label1.Text = "Binary"
      ' 
      ' BinOut0On
      ' 
      Me.BinOut0On.Location = New System.Drawing.Point(40, 40)
      Me.BinOut0On.Name = "BinOut0On"
      Me.BinOut0On.Size = New System.Drawing.Size(49, 23)
      Me.BinOut0On.TabIndex = 1
      Me.BinOut0On.Tag = "0"
      Me.BinOut0On.Text = "On"
      Me.BinOut0On.UseVisualStyleBackColor = True
      AddHandler Me.BinOut0On.Click, AddressOf Me.BinOutOn_Click
      ' 
      ' BinOut0Off
      ' 
      Me.BinOut0Off.Location = New System.Drawing.Point(95, 40)
      Me.BinOut0Off.Name = "BinOut0Off"
      Me.BinOut0Off.Size = New System.Drawing.Size(49, 23)
      Me.BinOut0Off.TabIndex = 2
      Me.BinOut0Off.Tag = "0"
      Me.BinOut0Off.Text = "Off"
      Me.BinOut0Off.UseVisualStyleBackColor = True
      AddHandler Me.BinOut0Off.Click, AddressOf Me.BinOutOff_Click
      ' 
      ' label2
      ' 
      Me.label2.AutoSize = True
      Me.label2.Location = New System.Drawing.Point(236, 16)
      Me.label2.Name = "label2"
      Me.label2.Size = New System.Drawing.Size(40, 13)
      Me.label2.TabIndex = 7
      Me.label2.Text = "Analog"
      ' 
      ' AnlgOut0Val
      ' 
      Me.AnlgOut0Val.Location = New System.Drawing.Point(188, 42)
      Me.AnlgOut0Val.Maximum = New Decimal(New Integer() {32767, 0, 0, 0})
      Me.AnlgOut0Val.Minimum = New Decimal(New Integer() {32768, 0, 0, -2147483648})
      Me.AnlgOut0Val.Name = "AnlgOut0Val"
      Me.AnlgOut0Val.Size = New System.Drawing.Size(56, 20)
      Me.AnlgOut0Val.TabIndex = 8
      Me.AnlgOut0Val.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
      ' 
      ' AnlgOut0Send
      ' 
      Me.AnlgOut0Send.Location = New System.Drawing.Point(253, 40)
      Me.AnlgOut0Send.Name = "AnlgOut0Send"
      Me.AnlgOut0Send.Size = New System.Drawing.Size(48, 23)
      Me.AnlgOut0Send.TabIndex = 11
      Me.AnlgOut0Send.Tag = "0"
      Me.AnlgOut0Send.Text = "Send"
      Me.AnlgOut0Send.UseVisualStyleBackColor = True
      AddHandler Me.AnlgOut0Send.Click, AddressOf Me.AnlgOutSend_Click
      ' 
      ' label3
      ' 
      Me.label3.AutoSize = True
      Me.label3.Location = New System.Drawing.Point(15, 49)
      Me.label3.Name = "label3"
      Me.label3.Size = New System.Drawing.Size(13, 13)
      Me.label3.TabIndex = 20
      Me.label3.Text = "0"
      Me.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
      ' 
      ' label4
      ' 
      Me.label4.AutoSize = True
      Me.label4.Location = New System.Drawing.Point(15, 75)
      Me.label4.Name = "label4"
      Me.label4.Size = New System.Drawing.Size(13, 13)
      Me.label4.TabIndex = 28
      Me.label4.Text = "1"
      Me.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
      ' 
      ' label5
      ' 
      Me.label5.AutoSize = True
      Me.label5.Location = New System.Drawing.Point(15, 103)
      Me.label5.Name = "label5"
      Me.label5.Size = New System.Drawing.Size(13, 13)
      Me.label5.TabIndex = 29
      Me.label5.Text = "2"
      Me.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
      ' 
      ' textBox1
      ' 
      Me.textBox1.BackColor = System.Drawing.SystemColors.Control
      Me.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
      Me.textBox1.Location = New System.Drawing.Point(20, 45)
      Me.textBox1.Name = "textBox1"
      Me.textBox1.Size = New System.Drawing.Size(14, 13)
      Me.textBox1.TabIndex = 40
      Me.textBox1.Text = "0"
      ' 
      ' textBox2
      ' 
      Me.textBox2.BackColor = System.Drawing.SystemColors.Control
      Me.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
      Me.textBox2.Location = New System.Drawing.Point(20, 75)
      Me.textBox2.Name = "textBox2"
      Me.textBox2.Size = New System.Drawing.Size(14, 13)
      Me.textBox2.TabIndex = 44
      Me.textBox2.Text = "1"
      ' 
      ' BinOut1Off
      ' 
      Me.BinOut1Off.Location = New System.Drawing.Point(95, 69)
      Me.BinOut1Off.Name = "BinOut1Off"
      Me.BinOut1Off.Size = New System.Drawing.Size(49, 23)
      Me.BinOut1Off.TabIndex = 42
      Me.BinOut1Off.Tag = "1"
      Me.BinOut1Off.Text = "Off"
      Me.BinOut1Off.UseVisualStyleBackColor = True
      AddHandler Me.BinOut1Off.Click, AddressOf Me.BinOutOff_Click
      ' 
      ' BinOut1On
      ' 
      Me.BinOut1On.Location = New System.Drawing.Point(40, 69)
      Me.BinOut1On.Name = "BinOut1On"
      Me.BinOut1On.Size = New System.Drawing.Size(49, 23)
      Me.BinOut1On.TabIndex = 41
      Me.BinOut1On.Tag = "1"
      Me.BinOut1On.Text = "On"
      Me.BinOut1On.UseVisualStyleBackColor = True
      AddHandler Me.BinOut1On.Click, AddressOf Me.BinOutOn_Click
      ' 
      ' textBox3
      ' 
      Me.textBox3.BackColor = System.Drawing.SystemColors.Control
      Me.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None
      Me.textBox3.Location = New System.Drawing.Point(20, 103)
      Me.textBox3.Name = "textBox3"
      Me.textBox3.Size = New System.Drawing.Size(14, 13)
      Me.textBox3.TabIndex = 48
      Me.textBox3.Text = "2"
      ' 
      ' BinOut2Off
      ' 
      Me.BinOut2Off.Location = New System.Drawing.Point(95, 98)
      Me.BinOut2Off.Name = "BinOut2Off"
      Me.BinOut2Off.Size = New System.Drawing.Size(49, 23)
      Me.BinOut2Off.TabIndex = 46
      Me.BinOut2Off.Tag = "2"
      Me.BinOut2Off.Text = "Off"
      Me.BinOut2Off.UseVisualStyleBackColor = True
      AddHandler Me.BinOut2Off.Click, AddressOf Me.BinOutOff_Click
      ' 
      ' BinOut2On
      ' 
      Me.BinOut2On.Location = New System.Drawing.Point(40, 98)
      Me.BinOut2On.Name = "BinOut2On"
      Me.BinOut2On.Size = New System.Drawing.Size(49, 23)
      Me.BinOut2On.TabIndex = 45
      Me.BinOut2On.Tag = "2"
      Me.BinOut2On.Text = "On"
      Me.BinOut2On.UseVisualStyleBackColor = True
      AddHandler Me.BinOut2On.Click, AddressOf Me.BinOutOn_Click
      ' 
      ' AnlgOut1Send
      ' 
      Me.AnlgOut1Send.Location = New System.Drawing.Point(253, 70)
      Me.AnlgOut1Send.Name = "AnlgOut1Send"
      Me.AnlgOut1Send.Size = New System.Drawing.Size(48, 23)
      Me.AnlgOut1Send.TabIndex = 50
      Me.AnlgOut1Send.Tag = "1"
      Me.AnlgOut1Send.Text = "Send"
      Me.AnlgOut1Send.UseVisualStyleBackColor = True
      AddHandler Me.AnlgOut1Send.Click, AddressOf Me.AnlgOutSend_Click
      ' 
      ' AnlgOut1Val
      ' 
      Me.AnlgOut1Val.Location = New System.Drawing.Point(188, 71)
      Me.AnlgOut1Val.Maximum = New Decimal(New Integer() {32767, 0, 0, 0})
      Me.AnlgOut1Val.Minimum = New Decimal(New Integer() {32768, 0, 0, -2147483648})
      Me.AnlgOut1Val.Name = "AnlgOut1Val"
      Me.AnlgOut1Val.Size = New System.Drawing.Size(56, 20)
      Me.AnlgOut1Val.TabIndex = 49
      Me.AnlgOut1Val.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
      ' 
      ' AnlgOut2Send
      ' 
      Me.AnlgOut2Send.Location = New System.Drawing.Point(253, 96)
      Me.AnlgOut2Send.Name = "AnlgOut2Send"
      Me.AnlgOut2Send.Size = New System.Drawing.Size(48, 23)
      Me.AnlgOut2Send.TabIndex = 53
      Me.AnlgOut2Send.Tag = "2"
      Me.AnlgOut2Send.Text = "Send"
      Me.AnlgOut2Send.UseVisualStyleBackColor = True
      AddHandler Me.AnlgOut2Send.Click, AddressOf Me.AnlgOutSend_Click
      ' 
      ' AnlgOut2Val
      ' 
      Me.AnlgOut2Val.Location = New System.Drawing.Point(188, 98)
      Me.AnlgOut2Val.Maximum = New Decimal(New Integer() {32767, 0, 0, 0})
      Me.AnlgOut2Val.Minimum = New Decimal(New Integer() {32768, 0, 0, -2147483648})
      Me.AnlgOut2Val.Name = "AnlgOut2Val"
      Me.AnlgOut2Val.Size = New System.Drawing.Size(56, 20)
      Me.AnlgOut2Val.TabIndex = 52
      Me.AnlgOut2Val.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
      AddHandler Me.AnlgOut2Val.ValueChanged, AddressOf Me.AnlgOut2Val_ValueChanged
      ' 
      ' groupBox1
      ' 
      Me.groupBox1.Controls.Add(Me.BinCntr2)
      Me.groupBox1.Controls.Add(Me.BinCntr1)
      Me.groupBox1.Controls.Add(Me.BinCntr0)
      Me.groupBox1.Controls.Add(Me.AnlgIn2)
      Me.groupBox1.Controls.Add(Me.AnlgIn1)
      Me.groupBox1.Controls.Add(Me.AnlgIn0)
      Me.groupBox1.Controls.Add(Me.BinIn2)
      Me.groupBox1.Controls.Add(Me.BinIn1)
      Me.groupBox1.Controls.Add(Me.BinIn0)
      Me.groupBox1.Controls.Add(Me.label8)
      Me.groupBox1.Controls.Add(Me.label7)
      Me.groupBox1.Controls.Add(Me.label6)
      Me.groupBox1.Controls.Add(Me.label5)
      Me.groupBox1.Controls.Add(Me.label4)
      Me.groupBox1.Controls.Add(Me.label3)
      Me.groupBox1.Location = New System.Drawing.Point(21, 16)
      Me.groupBox1.Name = "groupBox1"
      Me.groupBox1.Size = New System.Drawing.Size(334, 134)
      Me.groupBox1.TabIndex = 55
      Me.groupBox1.TabStop = False
      Me.groupBox1.Text = "Inputs"
      ' 
      ' BinCntr2
      ' 
      Me.BinCntr2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
      Me.BinCntr2.Location = New System.Drawing.Point(242, 98)
      Me.BinCntr2.Name = "BinCntr2"
      Me.BinCntr2.Size = New System.Drawing.Size(43, 23)
      Me.BinCntr2.TabIndex = 67
      Me.BinCntr2.Text = "- - - - -"
      Me.BinCntr2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      ' 
      ' BinCntr1
      ' 
      Me.BinCntr1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
      Me.BinCntr1.Location = New System.Drawing.Point(242, 70)
      Me.BinCntr1.Name = "BinCntr1"
      Me.BinCntr1.Size = New System.Drawing.Size(43, 23)
      Me.BinCntr1.TabIndex = 66
      Me.BinCntr1.Text = "- - - - -"
      Me.BinCntr1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      ' 
      ' BinCntr0
      ' 
      Me.BinCntr0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
      Me.BinCntr0.Location = New System.Drawing.Point(242, 44)
      Me.BinCntr0.Name = "BinCntr0"
      Me.BinCntr0.Size = New System.Drawing.Size(43, 23)
      Me.BinCntr0.TabIndex = 65
      Me.BinCntr0.Text = "- - - - -"
      Me.BinCntr0.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      ' 
      ' AnlgIn2
      ' 
      Me.AnlgIn2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
      Me.AnlgIn2.Location = New System.Drawing.Point(137, 99)
      Me.AnlgIn2.Name = "AnlgIn2"
      Me.AnlgIn2.Size = New System.Drawing.Size(49, 23)
      Me.AnlgIn2.TabIndex = 64
      Me.AnlgIn2.Text = "- - - - -"
      Me.AnlgIn2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      ' 
      ' AnlgIn1
      ' 
      Me.AnlgIn1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
      Me.AnlgIn1.Location = New System.Drawing.Point(137, 71)
      Me.AnlgIn1.Name = "AnlgIn1"
      Me.AnlgIn1.Size = New System.Drawing.Size(49, 23)
      Me.AnlgIn1.TabIndex = 63
      Me.AnlgIn1.Text = "- - - - -"
      Me.AnlgIn1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      ' 
      ' AnlgIn0
      ' 
      Me.AnlgIn0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
      Me.AnlgIn0.Location = New System.Drawing.Point(137, 44)
      Me.AnlgIn0.Name = "AnlgIn0"
      Me.AnlgIn0.Size = New System.Drawing.Size(49, 23)
      Me.AnlgIn0.TabIndex = 62
      Me.AnlgIn0.Text = "- - - - -"
      Me.AnlgIn0.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      ' 
      ' BinIn2
      ' 
      Me.BinIn2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
      Me.BinIn2.Location = New System.Drawing.Point(74, 104)
      Me.BinIn2.Name = "BinIn2"
      Me.BinIn2.Size = New System.Drawing.Size(27, 13)
      Me.BinIn2.TabIndex = 61
      Me.BinIn2.Text = "- - -"
      Me.BinIn2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      ' 
      ' BinIn1
      ' 
      Me.BinIn1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
      Me.BinIn1.Location = New System.Drawing.Point(74, 75)
      Me.BinIn1.Name = "BinIn1"
      Me.BinIn1.Size = New System.Drawing.Size(27, 13)
      Me.BinIn1.TabIndex = 60
      Me.BinIn1.Text = "- - -"
      Me.BinIn1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      ' 
      ' BinIn0
      ' 
      Me.BinIn0.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
      Me.BinIn0.Location = New System.Drawing.Point(74, 45)
      Me.BinIn0.Name = "BinIn0"
      Me.BinIn0.Size = New System.Drawing.Size(27, 23)
      Me.BinIn0.TabIndex = 59
      Me.BinIn0.Text = "- - -"
      Me.BinIn0.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      ' 
      ' label8
      ' 
      Me.label8.AutoSize = True
      Me.label8.Location = New System.Drawing.Point(240, 16)
      Me.label8.Name = "label8"
      Me.label8.Size = New System.Drawing.Size(49, 13)
      Me.label8.TabIndex = 57
      Me.label8.Text = "Counters"
      ' 
      ' label7
      ' 
      Me.label7.AutoSize = True
      Me.label7.Location = New System.Drawing.Point(149, 16)
      Me.label7.Name = "label7"
      Me.label7.Size = New System.Drawing.Size(40, 13)
      Me.label7.TabIndex = 56
      Me.label7.Text = "Analog"
      ' 
      ' label6
      ' 
      Me.label6.AutoSize = True
      Me.label6.Location = New System.Drawing.Point(67, 16)
      Me.label6.Name = "label6"
      Me.label6.Size = New System.Drawing.Size(36, 13)
      Me.label6.TabIndex = 56
      Me.label6.Text = "Binary"
      ' 
      ' groupBox2
      ' 
      Me.groupBox2.Controls.Add(Me.BinOut2Feedback)
      Me.groupBox2.Controls.Add(Me.BinOut1Feedback)
      Me.groupBox2.Controls.Add(Me.BinOut0Feedback)
      Me.groupBox2.Controls.Add(Me.AnlgOut2Feedback)
      Me.groupBox2.Controls.Add(Me.AnlgOut1Feedback)
      Me.groupBox2.Controls.Add(Me.AnlgOut0Feedback)
      Me.groupBox2.Controls.Add(Me.AnlgOut2Send)
      Me.groupBox2.Controls.Add(Me.AnlgOut2Val)
      Me.groupBox2.Controls.Add(Me.AnlgOut1Send)
      Me.groupBox2.Controls.Add(Me.AnlgOut1Val)
      Me.groupBox2.Controls.Add(Me.textBox3)
      Me.groupBox2.Controls.Add(Me.BinOut2Off)
      Me.groupBox2.Controls.Add(Me.BinOut2On)
      Me.groupBox2.Controls.Add(Me.textBox2)
      Me.groupBox2.Controls.Add(Me.BinOut1Off)
      Me.groupBox2.Controls.Add(Me.BinOut1On)
      Me.groupBox2.Controls.Add(Me.textBox1)
      Me.groupBox2.Controls.Add(Me.AnlgOut0Send)
      Me.groupBox2.Controls.Add(Me.AnlgOut0Val)
      Me.groupBox2.Controls.Add(Me.label2)
      Me.groupBox2.Controls.Add(Me.BinOut0Off)
      Me.groupBox2.Controls.Add(Me.BinOut0On)
      Me.groupBox2.Controls.Add(Me.label1)
      Me.groupBox2.Location = New System.Drawing.Point(366, 16)
      Me.groupBox2.Name = "groupBox2"
      Me.groupBox2.Size = New System.Drawing.Size(369, 134)
      Me.groupBox2.TabIndex = 56
      Me.groupBox2.TabStop = False
      Me.groupBox2.Text = "Outputs"
      ' 
      ' BinOut2Feedback
      ' 
      Me.BinOut2Feedback.AutoSize = True
      Me.BinOut2Feedback.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
      Me.BinOut2Feedback.ForeColor = System.Drawing.SystemColors.ControlText
      Me.BinOut2Feedback.Location = New System.Drawing.Point(151, 103)
      Me.BinOut2Feedback.Name = "BinOut2Feedback"
      Me.BinOut2Feedback.Size = New System.Drawing.Size(19, 13)
      Me.BinOut2Feedback.TabIndex = 60
      Me.BinOut2Feedback.Text = " - "
      ' 
      ' BinOut1Feedback
      ' 
      Me.BinOut1Feedback.AutoSize = True
      Me.BinOut1Feedback.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
      Me.BinOut1Feedback.ForeColor = System.Drawing.SystemColors.ControlText
      Me.BinOut1Feedback.Location = New System.Drawing.Point(151, 75)
      Me.BinOut1Feedback.Name = "BinOut1Feedback"
      Me.BinOut1Feedback.Size = New System.Drawing.Size(19, 13)
      Me.BinOut1Feedback.TabIndex = 59
      Me.BinOut1Feedback.Text = " - "
      ' 
      ' BinOut0Feedback
      ' 
      Me.BinOut0Feedback.AutoSize = True
      Me.BinOut0Feedback.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
      Me.BinOut0Feedback.ForeColor = System.Drawing.SystemColors.ControlText
      Me.BinOut0Feedback.Location = New System.Drawing.Point(151, 45)
      Me.BinOut0Feedback.Name = "BinOut0Feedback"
      Me.BinOut0Feedback.Size = New System.Drawing.Size(19, 13)
      Me.BinOut0Feedback.TabIndex = 58
      Me.BinOut0Feedback.Text = " - "
      ' 
      ' AnlgOut2Feedback
      ' 
      Me.AnlgOut2Feedback.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
      Me.AnlgOut2Feedback.Location = New System.Drawing.Point(306, 101)
      Me.AnlgOut2Feedback.Name = "AnlgOut2Feedback"
      Me.AnlgOut2Feedback.Size = New System.Drawing.Size(48, 13)
      Me.AnlgOut2Feedback.TabIndex = 57
      Me.AnlgOut2Feedback.Text = "- - - - -"
      Me.AnlgOut2Feedback.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      ' 
      ' AnlgOut1Feedback
      ' 
      Me.AnlgOut1Feedback.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
      Me.AnlgOut1Feedback.Location = New System.Drawing.Point(307, 74)
      Me.AnlgOut1Feedback.Name = "AnlgOut1Feedback"
      Me.AnlgOut1Feedback.Size = New System.Drawing.Size(47, 13)
      Me.AnlgOut1Feedback.TabIndex = 56
      Me.AnlgOut1Feedback.Text = "- - - - -"
      Me.AnlgOut1Feedback.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      ' 
      ' AnlgOut0Feedback
      ' 
      Me.AnlgOut0Feedback.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
      Me.AnlgOut0Feedback.Location = New System.Drawing.Point(307, 40)
      Me.AnlgOut0Feedback.Name = "AnlgOut0Feedback"
      Me.AnlgOut0Feedback.Size = New System.Drawing.Size(47, 23)
      Me.AnlgOut0Feedback.TabIndex = 55
      Me.AnlgOut0Feedback.Text = "- - - - -"
      Me.AnlgOut0Feedback.TextAlign = System.Drawing.ContentAlignment.MiddleRight
      ' 
      ' Integrity
      ' 
      Me.IntegrityPB.Location = New System.Drawing.Point(13, 20)
      Me.IntegrityPB.Name = "Integrity"
      Me.IntegrityPB.Size = New System.Drawing.Size(75, 23)
      Me.IntegrityPB.TabIndex = 57
      Me.IntegrityPB.Text = "Integrity"
      Me.IntegrityPB.UseVisualStyleBackColor = True
      AddHandler Me.IntegrityPB.Click, AddressOf Me.Integrity_Click
      ' 
      ' Event
      ' 
      Me.EventPB.Location = New System.Drawing.Point(13, 58)
      Me.EventPB.Name = "Event"
      Me.EventPB.Size = New System.Drawing.Size(75, 23)
      Me.EventPB.TabIndex = 58
      Me.EventPB.Text = "Event"
      Me.EventPB.UseVisualStyleBackColor = True
      AddHandler Me.EventPB.Click, AddressOf Me.Event_Click
      ' 
      ' IntegrityEnable
      ' 
      Me.IntegrityEnable.AutoSize = True
      Me.IntegrityEnable.Checked = True
      Me.IntegrityEnable.CheckState = System.Windows.Forms.CheckState.Checked
      Me.IntegrityEnable.Location = New System.Drawing.Point(103, 23)
      Me.IntegrityEnable.Name = "IntegrityEnable"
      Me.IntegrityEnable.Size = New System.Drawing.Size(90, 17)
      Me.IntegrityEnable.TabIndex = 59
      Me.IntegrityEnable.Text = "Repeat every"
      Me.IntegrityEnable.UseVisualStyleBackColor = True
      AddHandler Me.IntegrityEnable.CheckedChanged, AddressOf Me.IntegrityInterval_ValueChanged
      ' 
      ' EventEnable
      ' 
      Me.EventEnable.AutoSize = True
      Me.EventEnable.Checked = True
      Me.EventEnable.CheckState = System.Windows.Forms.CheckState.Checked
      Me.EventEnable.Location = New System.Drawing.Point(103, 61)
      Me.EventEnable.Name = "EventEnable"
      Me.EventEnable.Size = New System.Drawing.Size(90, 17)
      Me.EventEnable.TabIndex = 60
      Me.EventEnable.Text = "Repeat every"
      Me.EventEnable.UseVisualStyleBackColor = True
      AddHandler Me.EventEnable.CheckedChanged, AddressOf Me.EventInterval_ValueChanged
      ' 
      ' IntegrityIntervalHr
      ' 
      Me.IntegrityIntervalHr.Location = New System.Drawing.Point(200, 22)
      Me.IntegrityIntervalHr.Maximum = New Decimal(New Integer() {23, 0, 0, 0})
      Me.IntegrityIntervalHr.Name = "IntegrityIntervalHr"
      Me.IntegrityIntervalHr.Size = New System.Drawing.Size(34, 20)
      Me.IntegrityIntervalHr.TabIndex = 61
      Me.IntegrityIntervalHr.Tag = "Hour"
      Me.IntegrityIntervalHr.Value = New Decimal(New Integer() {1, 0, 0, 0})
      AddHandler Me.IntegrityIntervalHr.ValueChanged, AddressOf Me.IntegrityInterval_ValueChanged
      ' 
      ' label9
      ' 
      Me.label9.AutoSize = True
      Me.label9.Location = New System.Drawing.Point(233, 26)
      Me.label9.Name = "label9"
      Me.label9.Size = New System.Drawing.Size(10, 13)
      Me.label9.TabIndex = 62
      Me.label9.Text = ":"
      ' 
      ' label10
      ' 
      Me.label10.AutoSize = True
      Me.label10.Location = New System.Drawing.Point(279, 26)
      Me.label10.Name = "label10"
      Me.label10.Size = New System.Drawing.Size(10, 13)
      Me.label10.TabIndex = 64
      Me.label10.Text = ":"
      ' 
      ' IntegrityIntervalMin
      ' 
      Me.IntegrityIntervalMin.Location = New System.Drawing.Point(245, 22)
      Me.IntegrityIntervalMin.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
      Me.IntegrityIntervalMin.Name = "IntegrityIntervalMin"
      Me.IntegrityIntervalMin.Size = New System.Drawing.Size(34, 20)
      Me.IntegrityIntervalMin.TabIndex = 63
      Me.IntegrityIntervalMin.Tag = "Min"
      AddHandler Me.IntegrityIntervalMin.ValueChanged, AddressOf Me.IntegrityInterval_ValueChanged
      ' 
      ' IntegrityIntervalSec
      ' 
      Me.IntegrityIntervalSec.Location = New System.Drawing.Point(290, 22)
      Me.IntegrityIntervalSec.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
      Me.IntegrityIntervalSec.Name = "IntegrityIntervalSec"
      Me.IntegrityIntervalSec.Size = New System.Drawing.Size(34, 20)
      Me.IntegrityIntervalSec.TabIndex = 65
      Me.IntegrityIntervalSec.Tag = "sec"
      AddHandler Me.IntegrityIntervalSec.ValueChanged, AddressOf Me.IntegrityInterval_ValueChanged
      ' 
      ' EventIntervalSec
      ' 
      Me.EventIntervalSec.Location = New System.Drawing.Point(290, 58)
      Me.EventIntervalSec.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
      Me.EventIntervalSec.Name = "EventIntervalSec"
      Me.EventIntervalSec.Size = New System.Drawing.Size(34, 20)
      Me.EventIntervalSec.TabIndex = 70
      Me.EventIntervalSec.Value = New Decimal(New Integer() {5, 0, 0, 0})
      AddHandler Me.EventIntervalSec.ValueChanged, AddressOf Me.EventInterval_ValueChanged
      ' 
      ' label11
      ' 
      Me.label11.AutoSize = True
      Me.label11.Location = New System.Drawing.Point(279, 61)
      Me.label11.Name = "label11"
      Me.label11.Size = New System.Drawing.Size(10, 13)
      Me.label11.TabIndex = 69
      Me.label11.Text = ":"
      ' 
      ' EventIntervalMin
      ' 
      Me.EventIntervalMin.Location = New System.Drawing.Point(245, 58)
      Me.EventIntervalMin.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
      Me.EventIntervalMin.Name = "EventIntervalMin"
      Me.EventIntervalMin.Size = New System.Drawing.Size(34, 20)
      Me.EventIntervalMin.TabIndex = 68
      AddHandler Me.EventIntervalMin.ValueChanged, AddressOf Me.EventInterval_ValueChanged
      ' 
      ' label12
      ' 
      Me.label12.AutoSize = True
      Me.label12.Location = New System.Drawing.Point(233, 60)
      Me.label12.Name = "label12"
      Me.label12.Size = New System.Drawing.Size(10, 13)
      Me.label12.TabIndex = 67
      Me.label12.Text = ":"
      ' 
      ' EventIntervalHr
      ' 
      Me.EventIntervalHr.Location = New System.Drawing.Point(200, 58)
      Me.EventIntervalHr.Maximum = New Decimal(New Integer() {23, 0, 0, 0})
      Me.EventIntervalHr.Name = "EventIntervalHr"
      Me.EventIntervalHr.Size = New System.Drawing.Size(34, 20)
      Me.EventIntervalHr.TabIndex = 66
      AddHandler Me.EventIntervalHr.ValueChanged, AddressOf Me.EventInterval_ValueChanged
      ' 
      ' Poll
      ' 
      Me.Poll.Controls.Add(Me.EventProgressBar)
      Me.Poll.Controls.Add(Me.IntegrityProgressBar)
      Me.Poll.Controls.Add(Me.EventIntervalSec)
      Me.Poll.Controls.Add(Me.label11)
      Me.Poll.Controls.Add(Me.EventIntervalMin)
      Me.Poll.Controls.Add(Me.label12)
      Me.Poll.Controls.Add(Me.EventIntervalHr)
      Me.Poll.Controls.Add(Me.IntegrityIntervalSec)
      Me.Poll.Controls.Add(Me.label10)
      Me.Poll.Controls.Add(Me.IntegrityIntervalMin)
      Me.Poll.Controls.Add(Me.label9)
      Me.Poll.Controls.Add(Me.IntegrityIntervalHr)
      Me.Poll.Controls.Add(Me.EventEnable)
      Me.Poll.Controls.Add(Me.IntegrityEnable)
      Me.Poll.Controls.Add(Me.EventPB)
      Me.Poll.Controls.Add(Me.IntegrityPB)
      Me.Poll.Location = New System.Drawing.Point(22, 156)
      Me.Poll.Name = "Poll"
      Me.Poll.Size = New System.Drawing.Size(333, 101)
      Me.Poll.TabIndex = 71
      Me.Poll.TabStop = False
      Me.Poll.Text = "Poll"
      ' 
      ' EventProgressBar
      ' 
      Me.EventProgressBar.Location = New System.Drawing.Point(103, 82)
      Me.EventProgressBar.Name = "EventProgressBar"
      Me.EventProgressBar.Size = New System.Drawing.Size(221, 10)
      Me.EventProgressBar.TabIndex = 73
      ' 
      ' IntegrityProgressBar
      ' 
      Me.IntegrityProgressBar.Location = New System.Drawing.Point(103, 45)
      Me.IntegrityProgressBar.Name = "IntegrityProgressBar"
      Me.IntegrityProgressBar.Size = New System.Drawing.Size(221, 10)
      Me.IntegrityProgressBar.TabIndex = 72
      ' 
      ' IntegrityPollTimer
      ' 
      Me.IntegrityPollTimer.Interval = 1000
      AddHandler Me.IntegrityPollTimer.Tick, AddressOf Me.IntegrityPollTimer_Tick
      ' 
      ' EventPollTimer
      ' 
      Me.EventPollTimer.Interval = 1000
      AddHandler Me.EventPollTimer.Tick, AddressOf Me.EventPollTimer_Tick
      ' 
      ' protocolAnalyzer
      ' 
      Me.protocolAnalyzer.Anchor = ((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right))
      Me.protocolAnalyzer.BackColor = System.Drawing.Color.Gainsboro
      Me.protocolAnalyzer.Font = New System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte((0)))
      Me.protocolAnalyzer.Location = New System.Drawing.Point(21, 265)
      Me.protocolAnalyzer.Name = "protocolAnalyzer"
      Me.protocolAnalyzer.[ReadOnly] = True
      Me.protocolAnalyzer.Size = New System.Drawing.Size(717, 221)
      Me.protocolAnalyzer.TabIndex = 39
      Me.protocolAnalyzer.Text = ""
      Me.protocolAnalyzer.WordWrap = False
      AddHandler Me.protocolAnalyzer.MouseDown, AddressOf Me.protocolAnalyzer_MouseDown
      ' 
      ' pictureBox1
      ' 
      Me.pictureBox1.Image = DirectCast((resources.GetObject("pictureBox1.Image")), System.Drawing.Image)
      Me.pictureBox1.Location = New System.Drawing.Point(366, 152)
      Me.pictureBox1.Name = "pictureBox1"
      Me.pictureBox1.Size = New System.Drawing.Size(369, 101)
      Me.pictureBox1.TabIndex = 72
      Me.pictureBox1.TabStop = False
      ' 
      ' MasterForm
      ' 
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.ClientSize = New System.Drawing.Size(752, 498)
      Me.Controls.Add(Me.pictureBox1)
      Me.Controls.Add(Me.Poll)
      Me.Controls.Add(Me.groupBox2)
      Me.Controls.Add(Me.groupBox1)
      Me.Controls.Add(Me.protocolAnalyzer)
      Me.Icon = DirectCast((resources.GetObject("$this.Icon")), System.Drawing.Icon)
      Me.Name = "MasterForm"
      Me.Text = "TMW .NET Protocol Component DNP Master Example"
      DirectCast((Me.AnlgOut0Val), System.ComponentModel.ISupportInitialize).EndInit()
      DirectCast((Me.AnlgOut1Val), System.ComponentModel.ISupportInitialize).EndInit()
      DirectCast((Me.AnlgOut2Val), System.ComponentModel.ISupportInitialize).EndInit()
      Me.groupBox1.ResumeLayout(False)
      Me.groupBox1.PerformLayout()
      Me.groupBox2.ResumeLayout(False)
      Me.groupBox2.PerformLayout()
      DirectCast((Me.IntegrityIntervalHr), System.ComponentModel.ISupportInitialize).EndInit()
      DirectCast((Me.IntegrityIntervalMin), System.ComponentModel.ISupportInitialize).EndInit()
      DirectCast((Me.IntegrityIntervalSec), System.ComponentModel.ISupportInitialize).EndInit()
      DirectCast((Me.EventIntervalSec), System.ComponentModel.ISupportInitialize).EndInit()
      DirectCast((Me.EventIntervalMin), System.ComponentModel.ISupportInitialize).EndInit()
      DirectCast((Me.EventIntervalHr), System.ComponentModel.ISupportInitialize).EndInit()
      Me.Poll.ResumeLayout(False)
      Me.Poll.PerformLayout()
      DirectCast((Me.pictureBox1), System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub

#End Region

    Private label1 As System.Windows.Forms.Label
    Private BinOut0On As System.Windows.Forms.Button
    Private BinOut0Off As System.Windows.Forms.Button
    Private label2 As System.Windows.Forms.Label
    Private AnlgOut0Val As System.Windows.Forms.NumericUpDown
    Private AnlgOut0Send As System.Windows.Forms.Button
    Private label3 As System.Windows.Forms.Label
    Private label4 As System.Windows.Forms.Label
    Private label5 As System.Windows.Forms.Label
    Private textBox1 As System.Windows.Forms.TextBox
    Private textBox2 As System.Windows.Forms.TextBox
    Private BinOut1Off As System.Windows.Forms.Button
    Private BinOut1On As System.Windows.Forms.Button
    Private textBox3 As System.Windows.Forms.TextBox
    Private BinOut2Off As System.Windows.Forms.Button
    Private BinOut2On As System.Windows.Forms.Button
    Private AnlgOut1Send As System.Windows.Forms.Button
    Private AnlgOut1Val As System.Windows.Forms.NumericUpDown
    Private AnlgOut2Send As System.Windows.Forms.Button
    Private AnlgOut2Val As System.Windows.Forms.NumericUpDown
    Private groupBox1 As System.Windows.Forms.GroupBox
    Private label8 As System.Windows.Forms.Label
    Private label7 As System.Windows.Forms.Label
    Private label6 As System.Windows.Forms.Label
    Private groupBox2 As System.Windows.Forms.GroupBox
    Private IntegrityPB As System.Windows.Forms.Button
    Private EventPB As System.Windows.Forms.Button
		Private IntegrityEnable As System.Windows.Forms.CheckBox
		Private EventEnable As System.Windows.Forms.CheckBox
		Private IntegrityIntervalHr As System.Windows.Forms.NumericUpDown
		Private label9 As System.Windows.Forms.Label
		Private label10 As System.Windows.Forms.Label
		Private IntegrityIntervalMin As System.Windows.Forms.NumericUpDown
		Private IntegrityIntervalSec As System.Windows.Forms.NumericUpDown
		Private EventIntervalSec As System.Windows.Forms.NumericUpDown
		Private label11 As System.Windows.Forms.Label
		Private EventIntervalMin As System.Windows.Forms.NumericUpDown
		Private label12 As System.Windows.Forms.Label
		Private EventIntervalHr As System.Windows.Forms.NumericUpDown
		Private Poll As System.Windows.Forms.GroupBox
		Private EventProgressBar As System.Windows.Forms.ProgressBar
		Private IntegrityProgressBar As System.Windows.Forms.ProgressBar
		Private IntegrityPollTimer As System.Windows.Forms.Timer
		Private EventPollTimer As System.Windows.Forms.Timer
		Private protocolAnalyzer As System.Windows.Forms.RichTextBox
		Private pictureBox1 As System.Windows.Forms.PictureBox
		Private AnlgOut0Feedback As System.Windows.Forms.Label
		Private AnlgOut2Feedback As System.Windows.Forms.Label
		Private AnlgOut1Feedback As System.Windows.Forms.Label
		Private BinOut2Feedback As System.Windows.Forms.Label
		Private BinOut1Feedback As System.Windows.Forms.Label
		Private BinOut0Feedback As System.Windows.Forms.Label
		Private BinIn0 As System.Windows.Forms.Label
		Private BinIn2 As System.Windows.Forms.Label
		Private BinIn1 As System.Windows.Forms.Label
		Private AnlgIn0 As System.Windows.Forms.Label
		Private AnlgIn2 As System.Windows.Forms.Label
		Private AnlgIn1 As System.Windows.Forms.Label
		Private BinCntr0 As System.Windows.Forms.Label
		Private BinCntr2 As System.Windows.Forms.Label
		Private BinCntr1 As System.Windows.Forms.Label
	End Class
End Namespace

