Namespace DNPslaveGUI
	Partial Class SlaveForm
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
			Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(SlaveForm))
			Me.BinaryInput0 = New System.Windows.Forms.CheckBox()
			Me.protocolAnalyzer = New System.Windows.Forms.RichTextBox()
			Me.BinaryOutput0 = New System.Windows.Forms.Label()
			Me.AnalogInput0 = New System.Windows.Forms.NumericUpDown()
			Me.label1 = New System.Windows.Forms.Label()
			Me.Counter0 = New System.Windows.Forms.Button()
			Me.Counter1 = New System.Windows.Forms.Button()
			Me.label3 = New System.Windows.Forms.Label()
			Me.AnalogInput1 = New System.Windows.Forms.NumericUpDown()
			Me.BinaryOutput1 = New System.Windows.Forms.Label()
			Me.BinaryInput1 = New System.Windows.Forms.CheckBox()
			Me.Counter2 = New System.Windows.Forms.Button()
			Me.label4 = New System.Windows.Forms.Label()
			Me.AnalogInput2 = New System.Windows.Forms.NumericUpDown()
			Me.BinaryOutpu2 = New System.Windows.Forms.Label()
			Me.BinaryInput2 = New System.Windows.Forms.CheckBox()
			Me.groupBox1 = New System.Windows.Forms.GroupBox()
			Me.DO2Val = New System.Windows.Forms.Label()
			Me.DO1Val = New System.Windows.Forms.Label()
			Me.DO0Val = New System.Windows.Forms.Label()
			Me.AO2Val = New System.Windows.Forms.Label()
			Me.AO1Val = New System.Windows.Forms.Label()
			Me.AO0Val = New System.Windows.Forms.Label()
			Me.label8 = New System.Windows.Forms.Label()
			Me.label7 = New System.Windows.Forms.Label()
			Me.groupBox2 = New System.Windows.Forms.GroupBox()
			Me.label6 = New System.Windows.Forms.Label()
			Me.label5 = New System.Windows.Forms.Label()
			Me.label2 = New System.Windows.Forms.Label()
			Me.timer1 = New System.Windows.Forms.Timer(Me.components)
			DirectCast((Me.AnalogInput0), System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast((Me.AnalogInput1), System.ComponentModel.ISupportInitialize).BeginInit()
			DirectCast((Me.AnalogInput2), System.ComponentModel.ISupportInitialize).BeginInit()
			Me.groupBox1.SuspendLayout()
			Me.groupBox2.SuspendLayout()
			Me.SuspendLayout()
			' 
			' BinaryInput0
			' 
			Me.BinaryInput0.AutoSize = True
			Me.BinaryInput0.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
			Me.BinaryInput0.Location = New System.Drawing.Point(9, 46)
			Me.BinaryInput0.Name = "BinaryInput0"
			Me.BinaryInput0.Size = New System.Drawing.Size(47, 17)
			Me.BinaryInput0.TabIndex = 0
			Me.BinaryInput0.Tag = "0"
			Me.BinaryInput0.Text = "0     "
			Me.BinaryInput0.UseVisualStyleBackColor = True
			AddHandler Me.BinaryInput0.CheckedChanged, AddressOf Me.BinaryInput_CheckedChanged
			' 
			' protocolAnalyzer
			' 
      Me.protocolAnalyzer.Anchor = ((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right))
			Me.protocolAnalyzer.BackColor = System.Drawing.Color.Gainsboro
			Me.protocolAnalyzer.Font = New System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte((0)))
			Me.protocolAnalyzer.Location = New System.Drawing.Point(12, 144)
			Me.protocolAnalyzer.Name = "protocolAnalyzer"
			Me.protocolAnalyzer.[ReadOnly] = True
			Me.protocolAnalyzer.Size = New System.Drawing.Size(489, 176)
			Me.protocolAnalyzer.TabIndex = 1
			Me.protocolAnalyzer.Text = ""
			Me.protocolAnalyzer.WordWrap = False
			AddHandler Me.protocolAnalyzer.MouseDown, AddressOf Me.protocolAnalyzer_MouseDown
			' 
			' BinaryOutput0
			' 
			Me.BinaryOutput0.AutoSize = True
			Me.BinaryOutput0.Location = New System.Drawing.Point(10, 47)
			Me.BinaryOutput0.Name = "BinaryOutput0"
			Me.BinaryOutput0.Size = New System.Drawing.Size(13, 13)
			Me.BinaryOutput0.TabIndex = 2
			Me.BinaryOutput0.Text = "0"
			' 
			' AnalogInput0
			' 
			Me.AnalogInput0.Location = New System.Drawing.Point(95, 44)
			Me.AnalogInput0.Maximum = New Decimal(New Integer() {32767, 0, 0, 0})
			Me.AnalogInput0.Minimum = New Decimal(New Integer() {32768, 0, 0, -2147483648})
			Me.AnalogInput0.Name = "AnalogInput0"
			Me.AnalogInput0.Size = New System.Drawing.Size(60, 20)
			Me.AnalogInput0.TabIndex = 4
			Me.AnalogInput0.Tag = "0"
			Me.AnalogInput0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
			AddHandler Me.AnalogInput0.ValueChanged, AddressOf Me.AnalogInput_ValueChanged
			' 
			' label1
			' 
			Me.label1.AutoSize = True
			Me.label1.Location = New System.Drawing.Point(152, 47)
			Me.label1.Name = "label1"
			Me.label1.Size = New System.Drawing.Size(0, 13)
			Me.label1.TabIndex = 5
			' 
			' Counter0
			' 
			Me.Counter0.Location = New System.Drawing.Point(173, 42)
			Me.Counter0.Name = "Counter0"
			Me.Counter0.Size = New System.Drawing.Size(75, 23)
			Me.Counter0.TabIndex = 9
			Me.Counter0.Tag = "0"
			Me.Counter0.Text = "0"
			Me.Counter0.UseVisualStyleBackColor = True
			AddHandler Me.Counter0.Click, AddressOf Me.Counter_Click
			' 
			' Counter1
			' 
			Me.Counter1.Location = New System.Drawing.Point(173, 65)
			Me.Counter1.Name = "Counter1"
			Me.Counter1.Size = New System.Drawing.Size(75, 23)
			Me.Counter1.TabIndex = 18
			Me.Counter1.Tag = "1"
			Me.Counter1.Text = "0"
			Me.Counter1.UseVisualStyleBackColor = True
			AddHandler Me.Counter1.Click, AddressOf Me.Counter_Click
			' 
			' label3
			' 
			Me.label3.AutoSize = True
			Me.label3.Location = New System.Drawing.Point(152, 70)
			Me.label3.Name = "label3"
			Me.label3.Size = New System.Drawing.Size(0, 13)
			Me.label3.TabIndex = 14
			' 
			' AnalogInput1
			' 
			Me.AnalogInput1.Location = New System.Drawing.Point(95, 67)
			Me.AnalogInput1.Maximum = New Decimal(New Integer() {32767, 0, 0, 0})
			Me.AnalogInput1.Minimum = New Decimal(New Integer() {32768, 0, 0, -2147483648})
			Me.AnalogInput1.Name = "AnalogInput1"
			Me.AnalogInput1.Size = New System.Drawing.Size(60, 20)
			Me.AnalogInput1.TabIndex = 13
			Me.AnalogInput1.Tag = "1"
			Me.AnalogInput1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
			AddHandler Me.AnalogInput1.ValueChanged, AddressOf Me.AnalogInput_ValueChanged
			' 
			' BinaryOutput1
			' 
			Me.BinaryOutput1.AutoSize = True
			Me.BinaryOutput1.Location = New System.Drawing.Point(10, 70)
			Me.BinaryOutput1.Name = "BinaryOutput1"
			Me.BinaryOutput1.Size = New System.Drawing.Size(13, 13)
			Me.BinaryOutput1.TabIndex = 12
			Me.BinaryOutput1.Text = "1"
			' 
			' BinaryInput1
			' 
			Me.BinaryInput1.AutoSize = True
			Me.BinaryInput1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
			Me.BinaryInput1.Location = New System.Drawing.Point(9, 69)
			Me.BinaryInput1.Name = "BinaryInput1"
			Me.BinaryInput1.Size = New System.Drawing.Size(47, 17)
			Me.BinaryInput1.TabIndex = 11
			Me.BinaryInput1.Tag = "1"
			Me.BinaryInput1.Text = "1     "
			Me.BinaryInput1.UseVisualStyleBackColor = True
			AddHandler Me.BinaryInput1.CheckedChanged, AddressOf Me.BinaryInput_CheckedChanged
			' 
			' Counter2
			' 
			Me.Counter2.Location = New System.Drawing.Point(173, 89)
			Me.Counter2.Name = "Counter2"
			Me.Counter2.Size = New System.Drawing.Size(75, 23)
			Me.Counter2.TabIndex = 27
			Me.Counter2.Tag = "2"
			Me.Counter2.Text = "0"
			Me.Counter2.UseVisualStyleBackColor = True
			AddHandler Me.Counter2.Click, AddressOf Me.Counter_Click
			' 
			' label4
			' 
			Me.label4.AutoSize = True
			Me.label4.Location = New System.Drawing.Point(152, 94)
			Me.label4.Name = "label4"
			Me.label4.Size = New System.Drawing.Size(0, 13)
			Me.label4.TabIndex = 23
			' 
			' AnalogInput2
			' 
			Me.AnalogInput2.Location = New System.Drawing.Point(95, 91)
			Me.AnalogInput2.Maximum = New Decimal(New Integer() {32767, 0, 0, 0})
			Me.AnalogInput2.Minimum = New Decimal(New Integer() {32768, 0, 0, -2147483648})
			Me.AnalogInput2.Name = "AnalogInput2"
			Me.AnalogInput2.Size = New System.Drawing.Size(60, 20)
			Me.AnalogInput2.TabIndex = 22
			Me.AnalogInput2.Tag = "2"
			Me.AnalogInput2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
			AddHandler Me.AnalogInput2.ValueChanged, AddressOf Me.AnalogInput_ValueChanged
			' 
			' BinaryOutpu2
			' 
			Me.BinaryOutpu2.AutoSize = True
			Me.BinaryOutpu2.Location = New System.Drawing.Point(10, 94)
			Me.BinaryOutpu2.Name = "BinaryOutpu2"
			Me.BinaryOutpu2.Size = New System.Drawing.Size(13, 13)
			Me.BinaryOutpu2.TabIndex = 21
			Me.BinaryOutpu2.Text = "2"
			' 
			' BinaryInput2
			' 
			Me.BinaryInput2.AutoSize = True
			Me.BinaryInput2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
			Me.BinaryInput2.Location = New System.Drawing.Point(9, 93)
			Me.BinaryInput2.Name = "BinaryInput2"
			Me.BinaryInput2.Size = New System.Drawing.Size(47, 17)
			Me.BinaryInput2.TabIndex = 20
			Me.BinaryInput2.Tag = "2"
			Me.BinaryInput2.Text = "2     "
			Me.BinaryInput2.UseVisualStyleBackColor = True
			AddHandler Me.BinaryInput2.CheckedChanged, AddressOf Me.BinaryInput_CheckedChanged
			' 
			' groupBox1
			' 
			Me.groupBox1.Controls.Add(Me.DO2Val)
			Me.groupBox1.Controls.Add(Me.DO1Val)
			Me.groupBox1.Controls.Add(Me.DO0Val)
			Me.groupBox1.Controls.Add(Me.AO2Val)
			Me.groupBox1.Controls.Add(Me.AO1Val)
			Me.groupBox1.Controls.Add(Me.AO0Val)
			Me.groupBox1.Controls.Add(Me.label8)
			Me.groupBox1.Controls.Add(Me.label7)
			Me.groupBox1.Controls.Add(Me.BinaryOutpu2)
			Me.groupBox1.Controls.Add(Me.BinaryOutput1)
			Me.groupBox1.Controls.Add(Me.BinaryOutput0)
			Me.groupBox1.Location = New System.Drawing.Point(295, 12)
			Me.groupBox1.Name = "groupBox1"
			Me.groupBox1.Size = New System.Drawing.Size(205, 123)
			Me.groupBox1.TabIndex = 29
			Me.groupBox1.TabStop = False
			Me.groupBox1.Text = "Outputs"
			' 
			' DO2Val
			' 
			Me.DO2Val.AutoSize = True
			Me.DO2Val.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
			Me.DO2Val.ForeColor = System.Drawing.Color.Red
			Me.DO2Val.Location = New System.Drawing.Point(46, 94)
			Me.DO2Val.Name = "DO2Val"
			Me.DO2Val.Size = New System.Drawing.Size(24, 13)
			Me.DO2Val.TabIndex = 38
			Me.DO2Val.Text = "Off"
			' 
			' DO1Val
			' 
			Me.DO1Val.AutoSize = True
			Me.DO1Val.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
			Me.DO1Val.ForeColor = System.Drawing.Color.Red
			Me.DO1Val.Location = New System.Drawing.Point(46, 70)
			Me.DO1Val.Name = "DO1Val"
			Me.DO1Val.Size = New System.Drawing.Size(24, 13)
			Me.DO1Val.TabIndex = 37
			Me.DO1Val.Text = "Off"
			' 
			' DO0Val
			' 
			Me.DO0Val.AutoSize = True
			Me.DO0Val.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
			Me.DO0Val.ForeColor = System.Drawing.Color.Red
			Me.DO0Val.Location = New System.Drawing.Point(46, 47)
			Me.DO0Val.Name = "DO0Val"
			Me.DO0Val.Size = New System.Drawing.Size(24, 13)
			Me.DO0Val.TabIndex = 36
			Me.DO0Val.Text = "Off"
			' 
			' AO2Val
			' 
			Me.AO2Val.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
			Me.AO2Val.Location = New System.Drawing.Point(119, 93)
			Me.AO2Val.Name = "AO2Val"
			Me.AO2Val.Size = New System.Drawing.Size(46, 13)
			Me.AO2Val.TabIndex = 35
			Me.AO2Val.Text = "0"
			Me.AO2Val.TextAlign = System.Drawing.ContentAlignment.MiddleRight
			' 
			' AO1Val
			' 
			Me.AO1Val.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
			Me.AO1Val.Location = New System.Drawing.Point(119, 70)
			Me.AO1Val.Name = "AO1Val"
			Me.AO1Val.Size = New System.Drawing.Size(46, 13)
			Me.AO1Val.TabIndex = 34
			Me.AO1Val.Text = "0"
			Me.AO1Val.TextAlign = System.Drawing.ContentAlignment.MiddleRight
			' 
			' AO0Val
			' 
			Me.AO0Val.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte((0)))
			Me.AO0Val.Location = New System.Drawing.Point(117, 42)
			Me.AO0Val.Name = "AO0Val"
			Me.AO0Val.Size = New System.Drawing.Size(48, 23)
			Me.AO0Val.TabIndex = 33
			Me.AO0Val.Text = "0"
			Me.AO0Val.TextAlign = System.Drawing.ContentAlignment.MiddleRight
			' 
			' label8
			' 
			Me.label8.AutoSize = True
			Me.label8.Location = New System.Drawing.Point(107, 22)
			Me.label8.Name = "label8"
			Me.label8.Size = New System.Drawing.Size(80, 13)
			Me.label8.TabIndex = 32
			Me.label8.Text = "Analog Outputs"
			' 
			' label7
			' 
			Me.label7.AutoSize = True
			Me.label7.Location = New System.Drawing.Point(25, 22)
			Me.label7.Name = "label7"
			Me.label7.Size = New System.Drawing.Size(76, 13)
			Me.label7.TabIndex = 30
			Me.label7.Text = "Binary Outputs"
			' 
			' groupBox2
			' 
			Me.groupBox2.Controls.Add(Me.label6)
			Me.groupBox2.Controls.Add(Me.label5)
			Me.groupBox2.Controls.Add(Me.label2)
			Me.groupBox2.Controls.Add(Me.Counter2)
			Me.groupBox2.Controls.Add(Me.label4)
			Me.groupBox2.Controls.Add(Me.AnalogInput2)
			Me.groupBox2.Controls.Add(Me.BinaryInput2)
			Me.groupBox2.Controls.Add(Me.Counter1)
			Me.groupBox2.Controls.Add(Me.label3)
			Me.groupBox2.Controls.Add(Me.AnalogInput1)
			Me.groupBox2.Controls.Add(Me.BinaryInput1)
			Me.groupBox2.Controls.Add(Me.Counter0)
			Me.groupBox2.Controls.Add(Me.label1)
			Me.groupBox2.Controls.Add(Me.AnalogInput0)
			Me.groupBox2.Controls.Add(Me.BinaryInput0)
			Me.groupBox2.Location = New System.Drawing.Point(12, 12)
			Me.groupBox2.Name = "groupBox2"
			Me.groupBox2.Size = New System.Drawing.Size(272, 123)
			Me.groupBox2.TabIndex = 30
			Me.groupBox2.TabStop = False
			Me.groupBox2.Text = "Inputs"
			' 
			' label6
			' 
			Me.label6.AutoSize = True
			Me.label6.Location = New System.Drawing.Point(170, 22)
			Me.label6.Name = "label6"
			Me.label6.Size = New System.Drawing.Size(81, 13)
			Me.label6.TabIndex = 31
			Me.label6.Text = "Binary Counters"
			' 
			' label5
			' 
			Me.label5.AutoSize = True
			Me.label5.Location = New System.Drawing.Point(89, 22)
			Me.label5.Name = "label5"
			Me.label5.Size = New System.Drawing.Size(72, 13)
			Me.label5.TabIndex = 30
			Me.label5.Text = "Analog Inputs"
			' 
			' label2
			' 
			Me.label2.AutoSize = True
			Me.label2.Location = New System.Drawing.Point(19, 22)
			Me.label2.Name = "label2"
			Me.label2.Size = New System.Drawing.Size(68, 13)
			Me.label2.TabIndex = 29
			Me.label2.Text = "Binary Inputs"
			' 
			' SlaveForm
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
			Me.ClientSize = New System.Drawing.Size(513, 329)
			Me.Controls.Add(Me.groupBox2)
			Me.Controls.Add(Me.groupBox1)
			Me.Controls.Add(Me.protocolAnalyzer)
			Me.Icon = DirectCast((resources.GetObject("$this.Icon")), System.Drawing.Icon)
			Me.Name = "SlaveForm"
			Me.Text = "TMW .NET Protocol Component DNP Slave Example"
			DirectCast((Me.AnalogInput0), System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast((Me.AnalogInput1), System.ComponentModel.ISupportInitialize).EndInit()
			DirectCast((Me.AnalogInput2), System.ComponentModel.ISupportInitialize).EndInit()
			Me.groupBox1.ResumeLayout(False)
			Me.groupBox1.PerformLayout()
			Me.groupBox2.ResumeLayout(False)
			Me.groupBox2.PerformLayout()
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private BinaryInput0 As System.Windows.Forms.CheckBox
		Private protocolAnalyzer As System.Windows.Forms.RichTextBox
		Private BinaryOutput0 As System.Windows.Forms.Label
		Private AnalogInput0 As System.Windows.Forms.NumericUpDown
		Private label1 As System.Windows.Forms.Label
		Private Counter0 As System.Windows.Forms.Button
		Private Counter1 As System.Windows.Forms.Button
		Private label3 As System.Windows.Forms.Label
		Private AnalogInput1 As System.Windows.Forms.NumericUpDown
		Private BinaryOutput1 As System.Windows.Forms.Label
		Private BinaryInput1 As System.Windows.Forms.CheckBox
		Private Counter2 As System.Windows.Forms.Button
		Private label4 As System.Windows.Forms.Label
		Private AnalogInput2 As System.Windows.Forms.NumericUpDown
		Private BinaryOutpu2 As System.Windows.Forms.Label
		Private BinaryInput2 As System.Windows.Forms.CheckBox
		Private groupBox1 As System.Windows.Forms.GroupBox
		Private groupBox2 As System.Windows.Forms.GroupBox
		Private timer1 As System.Windows.Forms.Timer
		Private label6 As System.Windows.Forms.Label
		Private label5 As System.Windows.Forms.Label
		Private label2 As System.Windows.Forms.Label
		Private label8 As System.Windows.Forms.Label
		Private label7 As System.Windows.Forms.Label
		Private AO0Val As System.Windows.Forms.Label
		Private AO2Val As System.Windows.Forms.Label
		Private AO1Val As System.Windows.Forms.Label
		Private DO0Val As System.Windows.Forms.Label
		Private DO1Val As System.Windows.Forms.Label
		Private DO2Val As System.Windows.Forms.Label
	End Class
End Namespace

