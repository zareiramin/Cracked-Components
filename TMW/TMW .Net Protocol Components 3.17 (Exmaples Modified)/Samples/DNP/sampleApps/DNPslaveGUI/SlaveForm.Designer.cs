namespace DNPslaveGUI
{
    partial class SlaveForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlaveForm));
            this.BinaryInput0 = new System.Windows.Forms.CheckBox();
            this.protocolAnalyzer = new System.Windows.Forms.RichTextBox();
            this.BinaryOutput0 = new System.Windows.Forms.Label();
            this.AnalogInput0 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.Counter0 = new System.Windows.Forms.Button();
            this.Counter1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.AnalogInput1 = new System.Windows.Forms.NumericUpDown();
            this.BinaryOutput1 = new System.Windows.Forms.Label();
            this.BinaryInput1 = new System.Windows.Forms.CheckBox();
            this.Counter2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.AnalogInput2 = new System.Windows.Forms.NumericUpDown();
            this.BinaryOutpu2 = new System.Windows.Forms.Label();
            this.BinaryInput2 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DO2Val = new System.Windows.Forms.Label();
            this.DO1Val = new System.Windows.Forms.Label();
            this.DO0Val = new System.Windows.Forms.Label();
            this.AO2Val = new System.Windows.Forms.Label();
            this.AO1Val = new System.Windows.Forms.Label();
            this.AO0Val = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Online_LB = new System.Windows.Forms.Label();
            this.Connected_LB = new System.Windows.Forms.Label();
            this.SaveLogBt = new System.Windows.Forms.Button();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDownPort = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.AnalogInput0)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnalogInput1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnalogInput2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).BeginInit();
            this.SuspendLayout();
            // 
            // BinaryInput0
            // 
            this.BinaryInput0.AutoSize = true;
            this.BinaryInput0.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BinaryInput0.Location = new System.Drawing.Point(9, 46);
            this.BinaryInput0.Name = "BinaryInput0";
            this.BinaryInput0.Size = new System.Drawing.Size(47, 17);
            this.BinaryInput0.TabIndex = 0;
            this.BinaryInput0.Tag = "0";
            this.BinaryInput0.Text = "0     ";
            this.BinaryInput0.UseVisualStyleBackColor = true;
            this.BinaryInput0.CheckedChanged += new System.EventHandler(this.BinaryInput_CheckedChanged);
            // 
            // protocolAnalyzer
            // 
            this.protocolAnalyzer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.protocolAnalyzer.BackColor = System.Drawing.Color.Gainsboro;
            this.protocolAnalyzer.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.protocolAnalyzer.Location = new System.Drawing.Point(12, 144);
            this.protocolAnalyzer.Name = "protocolAnalyzer";
            this.protocolAnalyzer.ReadOnly = true;
            this.protocolAnalyzer.Size = new System.Drawing.Size(972, 501);
            this.protocolAnalyzer.TabIndex = 1;
            this.protocolAnalyzer.Text = "W";
            this.protocolAnalyzer.WordWrap = false;
            this.protocolAnalyzer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.protocolAnalyzer_MouseDown);
            // 
            // BinaryOutput0
            // 
            this.BinaryOutput0.AutoSize = true;
            this.BinaryOutput0.Location = new System.Drawing.Point(10, 47);
            this.BinaryOutput0.Name = "BinaryOutput0";
            this.BinaryOutput0.Size = new System.Drawing.Size(13, 13);
            this.BinaryOutput0.TabIndex = 2;
            this.BinaryOutput0.Text = "0";
            // 
            // AnalogInput0
            // 
            this.AnalogInput0.Location = new System.Drawing.Point(95, 44);
            this.AnalogInput0.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.AnalogInput0.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.AnalogInput0.Name = "AnalogInput0";
            this.AnalogInput0.Size = new System.Drawing.Size(60, 20);
            this.AnalogInput0.TabIndex = 4;
            this.AnalogInput0.Tag = "0";
            this.AnalogInput0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.AnalogInput0.ValueChanged += new System.EventHandler(this.AnalogInput_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(152, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 5;
            // 
            // Counter0
            // 
            this.Counter0.Location = new System.Drawing.Point(173, 42);
            this.Counter0.Name = "Counter0";
            this.Counter0.Size = new System.Drawing.Size(75, 23);
            this.Counter0.TabIndex = 9;
            this.Counter0.Tag = "0";
            this.Counter0.Text = "0";
            this.Counter0.UseVisualStyleBackColor = true;
            this.Counter0.Click += new System.EventHandler(this.Counter_Click);
            // 
            // Counter1
            // 
            this.Counter1.Location = new System.Drawing.Point(173, 65);
            this.Counter1.Name = "Counter1";
            this.Counter1.Size = new System.Drawing.Size(75, 23);
            this.Counter1.TabIndex = 18;
            this.Counter1.Tag = "1";
            this.Counter1.Text = "0";
            this.Counter1.UseVisualStyleBackColor = true;
            this.Counter1.Click += new System.EventHandler(this.Counter_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(152, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 14;
            // 
            // AnalogInput1
            // 
            this.AnalogInput1.Location = new System.Drawing.Point(95, 67);
            this.AnalogInput1.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.AnalogInput1.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.AnalogInput1.Name = "AnalogInput1";
            this.AnalogInput1.Size = new System.Drawing.Size(60, 20);
            this.AnalogInput1.TabIndex = 13;
            this.AnalogInput1.Tag = "1";
            this.AnalogInput1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.AnalogInput1.ValueChanged += new System.EventHandler(this.AnalogInput_ValueChanged);
            // 
            // BinaryOutput1
            // 
            this.BinaryOutput1.AutoSize = true;
            this.BinaryOutput1.Location = new System.Drawing.Point(10, 70);
            this.BinaryOutput1.Name = "BinaryOutput1";
            this.BinaryOutput1.Size = new System.Drawing.Size(13, 13);
            this.BinaryOutput1.TabIndex = 12;
            this.BinaryOutput1.Text = "1";
            // 
            // BinaryInput1
            // 
            this.BinaryInput1.AutoSize = true;
            this.BinaryInput1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BinaryInput1.Location = new System.Drawing.Point(9, 69);
            this.BinaryInput1.Name = "BinaryInput1";
            this.BinaryInput1.Size = new System.Drawing.Size(47, 17);
            this.BinaryInput1.TabIndex = 11;
            this.BinaryInput1.Tag = "1";
            this.BinaryInput1.Text = "1     ";
            this.BinaryInput1.UseVisualStyleBackColor = true;
            this.BinaryInput1.CheckedChanged += new System.EventHandler(this.BinaryInput_CheckedChanged);
            // 
            // Counter2
            // 
            this.Counter2.Location = new System.Drawing.Point(173, 89);
            this.Counter2.Name = "Counter2";
            this.Counter2.Size = new System.Drawing.Size(75, 23);
            this.Counter2.TabIndex = 27;
            this.Counter2.Tag = "2";
            this.Counter2.Text = "0";
            this.Counter2.UseVisualStyleBackColor = true;
            this.Counter2.Click += new System.EventHandler(this.Counter_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(152, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 23;
            // 
            // AnalogInput2
            // 
            this.AnalogInput2.Location = new System.Drawing.Point(95, 91);
            this.AnalogInput2.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.AnalogInput2.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.AnalogInput2.Name = "AnalogInput2";
            this.AnalogInput2.Size = new System.Drawing.Size(60, 20);
            this.AnalogInput2.TabIndex = 22;
            this.AnalogInput2.Tag = "2";
            this.AnalogInput2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.AnalogInput2.ValueChanged += new System.EventHandler(this.AnalogInput_ValueChanged);
            // 
            // BinaryOutpu2
            // 
            this.BinaryOutpu2.AutoSize = true;
            this.BinaryOutpu2.Location = new System.Drawing.Point(10, 94);
            this.BinaryOutpu2.Name = "BinaryOutpu2";
            this.BinaryOutpu2.Size = new System.Drawing.Size(13, 13);
            this.BinaryOutpu2.TabIndex = 21;
            this.BinaryOutpu2.Text = "2";
            // 
            // BinaryInput2
            // 
            this.BinaryInput2.AutoSize = true;
            this.BinaryInput2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BinaryInput2.Location = new System.Drawing.Point(9, 93);
            this.BinaryInput2.Name = "BinaryInput2";
            this.BinaryInput2.Size = new System.Drawing.Size(47, 17);
            this.BinaryInput2.TabIndex = 20;
            this.BinaryInput2.Tag = "2";
            this.BinaryInput2.Text = "2     ";
            this.BinaryInput2.UseVisualStyleBackColor = true;
            this.BinaryInput2.CheckedChanged += new System.EventHandler(this.BinaryInput_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DO2Val);
            this.groupBox1.Controls.Add(this.DO1Val);
            this.groupBox1.Controls.Add(this.DO0Val);
            this.groupBox1.Controls.Add(this.AO2Val);
            this.groupBox1.Controls.Add(this.AO1Val);
            this.groupBox1.Controls.Add(this.AO0Val);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.BinaryOutpu2);
            this.groupBox1.Controls.Add(this.BinaryOutput1);
            this.groupBox1.Controls.Add(this.BinaryOutput0);
            this.groupBox1.Location = new System.Drawing.Point(670, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(205, 123);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Outputs";
            // 
            // DO2Val
            // 
            this.DO2Val.AutoSize = true;
            this.DO2Val.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DO2Val.ForeColor = System.Drawing.Color.Red;
            this.DO2Val.Location = new System.Drawing.Point(46, 94);
            this.DO2Val.Name = "DO2Val";
            this.DO2Val.Size = new System.Drawing.Size(24, 13);
            this.DO2Val.TabIndex = 38;
            this.DO2Val.Text = "Off";
            // 
            // DO1Val
            // 
            this.DO1Val.AutoSize = true;
            this.DO1Val.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DO1Val.ForeColor = System.Drawing.Color.Red;
            this.DO1Val.Location = new System.Drawing.Point(46, 70);
            this.DO1Val.Name = "DO1Val";
            this.DO1Val.Size = new System.Drawing.Size(24, 13);
            this.DO1Val.TabIndex = 37;
            this.DO1Val.Text = "Off";
            // 
            // DO0Val
            // 
            this.DO0Val.AutoSize = true;
            this.DO0Val.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DO0Val.ForeColor = System.Drawing.Color.Red;
            this.DO0Val.Location = new System.Drawing.Point(46, 47);
            this.DO0Val.Name = "DO0Val";
            this.DO0Val.Size = new System.Drawing.Size(24, 13);
            this.DO0Val.TabIndex = 36;
            this.DO0Val.Text = "Off";
            // 
            // AO2Val
            // 
            this.AO2Val.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AO2Val.Location = new System.Drawing.Point(119, 93);
            this.AO2Val.Name = "AO2Val";
            this.AO2Val.Size = new System.Drawing.Size(46, 13);
            this.AO2Val.TabIndex = 35;
            this.AO2Val.Text = "0";
            this.AO2Val.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AO1Val
            // 
            this.AO1Val.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AO1Val.Location = new System.Drawing.Point(119, 70);
            this.AO1Val.Name = "AO1Val";
            this.AO1Val.Size = new System.Drawing.Size(46, 13);
            this.AO1Val.TabIndex = 34;
            this.AO1Val.Text = "0";
            this.AO1Val.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AO0Val
            // 
            this.AO0Val.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AO0Val.Location = new System.Drawing.Point(117, 42);
            this.AO0Val.Name = "AO0Val";
            this.AO0Val.Size = new System.Drawing.Size(48, 23);
            this.AO0Val.TabIndex = 33;
            this.AO0Val.Text = "0";
            this.AO0Val.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(107, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 32;
            this.label8.Text = "Analog Outputs";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Binary Outputs";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.Counter2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.AnalogInput2);
            this.groupBox2.Controls.Add(this.BinaryInput2);
            this.groupBox2.Controls.Add(this.Counter1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.AnalogInput1);
            this.groupBox2.Controls.Add(this.BinaryInput1);
            this.groupBox2.Controls.Add(this.Counter0);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.AnalogInput0);
            this.groupBox2.Controls.Add(this.BinaryInput0);
            this.groupBox2.Location = new System.Drawing.Point(387, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(272, 123);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Inputs";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(170, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Binary Counters";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(89, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Analog Inputs";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Binary Inputs";
            // 
            // Online_LB
            // 
            this.Online_LB.AutoSize = true;
            this.Online_LB.ForeColor = System.Drawing.Color.Crimson;
            this.Online_LB.Location = new System.Drawing.Point(894, 79);
            this.Online_LB.Name = "Online_LB";
            this.Online_LB.Size = new System.Drawing.Size(37, 13);
            this.Online_LB.TabIndex = 96;
            this.Online_LB.Text = "Offline";
            // 
            // Connected_LB
            // 
            this.Connected_LB.AutoSize = true;
            this.Connected_LB.ForeColor = System.Drawing.Color.Crimson;
            this.Connected_LB.Location = new System.Drawing.Point(894, 54);
            this.Connected_LB.Name = "Connected_LB";
            this.Connected_LB.Size = new System.Drawing.Size(73, 13);
            this.Connected_LB.TabIndex = 95;
            this.Connected_LB.Text = "Disconnected";
            // 
            // SaveLogBt
            // 
            this.SaveLogBt.Location = new System.Drawing.Point(897, 100);
            this.SaveLogBt.Name = "SaveLogBt";
            this.SaveLogBt.Size = new System.Drawing.Size(75, 23);
            this.SaveLogBt.TabIndex = 97;
            this.SaveLogBt.Text = "Save Log";
            this.SaveLogBt.UseVisualStyleBackColor = true;
            this.SaveLogBt.Click += new System.EventHandler(this.SaveLog_Click);
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(112, 46);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(120, 20);
            this.txtIP.TabIndex = 98;
            this.txtIP.Text = "127.0.0.1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(126, 95);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 100;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(86, 54);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 13);
            this.label9.TabIndex = 101;
            this.label9.Text = "IP:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(77, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 13);
            this.label10.TabIndex = 102;
            this.label10.Text = "Port:";
            // 
            // numericUpDownPort
            // 
            this.numericUpDownPort.Location = new System.Drawing.Point(112, 73);
            this.numericUpDownPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPort.Name = "numericUpDownPort";
            this.numericUpDownPort.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownPort.TabIndex = 103;
            this.numericUpDownPort.Value = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            // 
            // SlaveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(996, 654);
            this.Controls.Add(this.numericUpDownPort);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.SaveLogBt);
            this.Controls.Add(this.Online_LB);
            this.Controls.Add(this.Connected_LB);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.protocolAnalyzer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SlaveForm";
            this.Text = "TMW .NET Protocol Component DNP Slave Example";
            ((System.ComponentModel.ISupportInitialize)(this.AnalogInput0)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnalogInput1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AnalogInput2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox BinaryInput0;
      private System.Windows.Forms.RichTextBox protocolAnalyzer;
      private System.Windows.Forms.Label BinaryOutput0;
      private System.Windows.Forms.NumericUpDown AnalogInput0;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Button Counter0;
      private System.Windows.Forms.Button Counter1;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.NumericUpDown AnalogInput1;
      private System.Windows.Forms.Label BinaryOutput1;
      private System.Windows.Forms.CheckBox BinaryInput1;
      private System.Windows.Forms.Button Counter2;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.NumericUpDown AnalogInput2;
      private System.Windows.Forms.Label BinaryOutpu2;
      private System.Windows.Forms.CheckBox BinaryInput2;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Label AO0Val;
      private System.Windows.Forms.Label AO2Val;
      private System.Windows.Forms.Label AO1Val;
      private System.Windows.Forms.Label DO0Val;
      private System.Windows.Forms.Label DO1Val;
      private System.Windows.Forms.Label DO2Val;
      private System.Windows.Forms.Label Online_LB;
      private System.Windows.Forms.Label Connected_LB;
      private System.Windows.Forms.Button SaveLogBt;
      private System.Windows.Forms.TextBox txtIP;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.NumericUpDown numericUpDownPort;
    }
}

