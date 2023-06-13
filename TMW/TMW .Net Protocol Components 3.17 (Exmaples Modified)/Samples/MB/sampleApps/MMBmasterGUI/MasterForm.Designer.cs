namespace MMBmasterGUI
{
  partial class MasterForm
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterForm));
      this.label1 = new System.Windows.Forms.Label();
      this.Coil0On = new System.Windows.Forms.Button();
      this.Coil0Off = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.HoldingRegister0Val = new System.Windows.Forms.NumericUpDown();
      this.HoldingRegister0Send = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.Coil1Off = new System.Windows.Forms.Button();
      this.Coil1On = new System.Windows.Forms.Button();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.Coil2Off = new System.Windows.Forms.Button();
      this.Coil2On = new System.Windows.Forms.Button();
      this.HoldingRegister1Send = new System.Windows.Forms.Button();
      this.HoldingRegister1Val = new System.Windows.Forms.NumericUpDown();
      this.HoldingRegister2Send = new System.Windows.Forms.Button();
      this.HoldingRegister2Val = new System.Windows.Forms.NumericUpDown();
      this.InputsGroupBox = new System.Windows.Forms.GroupBox();
      this.InputRegister2 = new System.Windows.Forms.Label();
      this.HoldingRegister2 = new System.Windows.Forms.Label();
      this.InputRegister1 = new System.Windows.Forms.Label();
      this.HoldingRegister1 = new System.Windows.Forms.Label();
      this.InputRegister0 = new System.Windows.Forms.Label();
      this.HoldingRegister0 = new System.Windows.Forms.Label();
      this.label14 = new System.Windows.Forms.Label();
      this.DiscIn2 = new System.Windows.Forms.Label();
      this.DiscIn1 = new System.Windows.Forms.Label();
      this.DiscIn0 = new System.Windows.Forms.Label();
      this.Coil2 = new System.Windows.Forms.Label();
      this.Coil1 = new System.Windows.Forms.Label();
      this.Coil0 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.OutputsGroupBox = new System.Windows.Forms.GroupBox();
      this.PollReadPB = new System.Windows.Forms.Button();
      this.PollEnableChkBox = new System.Windows.Forms.CheckBox();
      this.PollIntervalMilliSecNumUpDown = new System.Windows.Forms.NumericUpDown();
      this.PollGroupBox = new System.Windows.Forms.GroupBox();
      this.label9 = new System.Windows.Forms.Label();
      this.ReadProgressBar = new System.Windows.Forms.ProgressBar();
      this.ReadPollTimer = new System.Windows.Forms.Timer(this.components);
      this.protocolAnalyzerTextCtrl = new System.Windows.Forms.RichTextBox();
      this.tmwLogoPictureBox = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.HoldingRegister0Val)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.HoldingRegister1Val)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.HoldingRegister2Val)).BeginInit();
      this.InputsGroupBox.SuspendLayout();
      this.OutputsGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PollIntervalMilliSecNumUpDown)).BeginInit();
      this.PollGroupBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.tmwLogoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(78, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(29, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Coils";
      // 
      // Coil0On
      // 
      this.Coil0On.Location = new System.Drawing.Point(40, 40);
      this.Coil0On.Name = "Coil0On";
      this.Coil0On.Size = new System.Drawing.Size(49, 23);
      this.Coil0On.TabIndex = 1;
      this.Coil0On.Tag = "0";
      this.Coil0On.Text = "On";
      this.Coil0On.UseVisualStyleBackColor = true;
      this.Coil0On.Click += new System.EventHandler(this.CoilOn_Click);
      // 
      // Coil0Off
      // 
      this.Coil0Off.Location = new System.Drawing.Point(95, 40);
      this.Coil0Off.Name = "Coil0Off";
      this.Coil0Off.Size = new System.Drawing.Size(49, 23);
      this.Coil0Off.TabIndex = 2;
      this.Coil0Off.Tag = "0";
      this.Coil0Off.Text = "Off";
      this.Coil0Off.UseVisualStyleBackColor = true;
      this.Coil0Off.Click += new System.EventHandler(this.CoilOff_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(236, 16);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(90, 13);
      this.label2.TabIndex = 7;
      this.label2.Text = "Holding Registers";
      // 
      // HoldingRegister0Val
      // 
      this.HoldingRegister0Val.Location = new System.Drawing.Point(228, 42);
      this.HoldingRegister0Val.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
      this.HoldingRegister0Val.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
      this.HoldingRegister0Val.Name = "HoldingRegister0Val";
      this.HoldingRegister0Val.Size = new System.Drawing.Size(56, 20);
      this.HoldingRegister0Val.TabIndex = 8;
      this.HoldingRegister0Val.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // HoldingRegister0Send
      // 
      this.HoldingRegister0Send.Location = new System.Drawing.Point(293, 40);
      this.HoldingRegister0Send.Name = "HoldingRegister0Send";
      this.HoldingRegister0Send.Size = new System.Drawing.Size(48, 23);
      this.HoldingRegister0Send.TabIndex = 11;
      this.HoldingRegister0Send.Tag = "0";
      this.HoldingRegister0Send.Text = "Send";
      this.HoldingRegister0Send.UseVisualStyleBackColor = true;
      this.HoldingRegister0Send.Click += new System.EventHandler(this.WriteHoldingRegister_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(15, 49);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(13, 13);
      this.label3.TabIndex = 20;
      this.label3.Text = "0";
      this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(15, 75);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(13, 13);
      this.label4.TabIndex = 28;
      this.label4.Text = "1";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(15, 103);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(13, 13);
      this.label5.TabIndex = 29;
      this.label5.Text = "2";
      this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBox1
      // 
      this.textBox1.BackColor = System.Drawing.SystemColors.Control;
      this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBox1.Location = new System.Drawing.Point(20, 45);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(14, 13);
      this.textBox1.TabIndex = 40;
      this.textBox1.Text = "0";
      // 
      // textBox2
      // 
      this.textBox2.BackColor = System.Drawing.SystemColors.Control;
      this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBox2.Location = new System.Drawing.Point(20, 75);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(14, 13);
      this.textBox2.TabIndex = 44;
      this.textBox2.Text = "1";
      // 
      // Coil1Off
      // 
      this.Coil1Off.Location = new System.Drawing.Point(95, 69);
      this.Coil1Off.Name = "Coil1Off";
      this.Coil1Off.Size = new System.Drawing.Size(49, 23);
      this.Coil1Off.TabIndex = 42;
      this.Coil1Off.Tag = "1";
      this.Coil1Off.Text = "Off";
      this.Coil1Off.UseVisualStyleBackColor = true;
      this.Coil1Off.Click += new System.EventHandler(this.CoilOff_Click);
      // 
      // Coil1On
      // 
      this.Coil1On.Location = new System.Drawing.Point(40, 69);
      this.Coil1On.Name = "Coil1On";
      this.Coil1On.Size = new System.Drawing.Size(49, 23);
      this.Coil1On.TabIndex = 41;
      this.Coil1On.Tag = "1";
      this.Coil1On.Text = "On";
      this.Coil1On.UseVisualStyleBackColor = true;
      this.Coil1On.Click += new System.EventHandler(this.CoilOn_Click);
      // 
      // textBox3
      // 
      this.textBox3.BackColor = System.Drawing.SystemColors.Control;
      this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.textBox3.Location = new System.Drawing.Point(20, 103);
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new System.Drawing.Size(14, 13);
      this.textBox3.TabIndex = 48;
      this.textBox3.Text = "2";
      // 
      // Coil2Off
      // 
      this.Coil2Off.Location = new System.Drawing.Point(95, 98);
      this.Coil2Off.Name = "Coil2Off";
      this.Coil2Off.Size = new System.Drawing.Size(49, 23);
      this.Coil2Off.TabIndex = 46;
      this.Coil2Off.Tag = "2";
      this.Coil2Off.Text = "Off";
      this.Coil2Off.UseVisualStyleBackColor = true;
      this.Coil2Off.Click += new System.EventHandler(this.CoilOff_Click);
      // 
      // Coil2On
      // 
      this.Coil2On.Location = new System.Drawing.Point(40, 98);
      this.Coil2On.Name = "Coil2On";
      this.Coil2On.Size = new System.Drawing.Size(49, 23);
      this.Coil2On.TabIndex = 45;
      this.Coil2On.Tag = "2";
      this.Coil2On.Text = "On";
      this.Coil2On.UseVisualStyleBackColor = true;
      this.Coil2On.Click += new System.EventHandler(this.CoilOn_Click);
      // 
      // HoldingRegister1Send
      // 
      this.HoldingRegister1Send.Location = new System.Drawing.Point(293, 70);
      this.HoldingRegister1Send.Name = "HoldingRegister1Send";
      this.HoldingRegister1Send.Size = new System.Drawing.Size(48, 23);
      this.HoldingRegister1Send.TabIndex = 50;
      this.HoldingRegister1Send.Tag = "1";
      this.HoldingRegister1Send.Text = "Send";
      this.HoldingRegister1Send.UseVisualStyleBackColor = true;
      this.HoldingRegister1Send.Click += new System.EventHandler(this.WriteHoldingRegister_Click);
      // 
      // HoldingRegister1Val
      // 
      this.HoldingRegister1Val.Location = new System.Drawing.Point(228, 71);
      this.HoldingRegister1Val.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
      this.HoldingRegister1Val.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
      this.HoldingRegister1Val.Name = "HoldingRegister1Val";
      this.HoldingRegister1Val.Size = new System.Drawing.Size(56, 20);
      this.HoldingRegister1Val.TabIndex = 49;
      this.HoldingRegister1Val.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // HoldingRegister2Send
      // 
      this.HoldingRegister2Send.Location = new System.Drawing.Point(293, 96);
      this.HoldingRegister2Send.Name = "HoldingRegister2Send";
      this.HoldingRegister2Send.Size = new System.Drawing.Size(48, 23);
      this.HoldingRegister2Send.TabIndex = 53;
      this.HoldingRegister2Send.Tag = "2";
      this.HoldingRegister2Send.Text = "Send";
      this.HoldingRegister2Send.UseVisualStyleBackColor = true;
      this.HoldingRegister2Send.Click += new System.EventHandler(this.WriteHoldingRegister_Click);
      // 
      // HoldingRegister2Val
      // 
      this.HoldingRegister2Val.Location = new System.Drawing.Point(228, 98);
      this.HoldingRegister2Val.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
      this.HoldingRegister2Val.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
      this.HoldingRegister2Val.Name = "HoldingRegister2Val";
      this.HoldingRegister2Val.Size = new System.Drawing.Size(56, 20);
      this.HoldingRegister2Val.TabIndex = 52;
      this.HoldingRegister2Val.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // InputsGroupBox
      // 
      this.InputsGroupBox.Controls.Add(this.InputRegister2);
      this.InputsGroupBox.Controls.Add(this.HoldingRegister2);
      this.InputsGroupBox.Controls.Add(this.InputRegister1);
      this.InputsGroupBox.Controls.Add(this.HoldingRegister1);
      this.InputsGroupBox.Controls.Add(this.InputRegister0);
      this.InputsGroupBox.Controls.Add(this.HoldingRegister0);
      this.InputsGroupBox.Controls.Add(this.label14);
      this.InputsGroupBox.Controls.Add(this.DiscIn2);
      this.InputsGroupBox.Controls.Add(this.DiscIn1);
      this.InputsGroupBox.Controls.Add(this.DiscIn0);
      this.InputsGroupBox.Controls.Add(this.Coil2);
      this.InputsGroupBox.Controls.Add(this.Coil1);
      this.InputsGroupBox.Controls.Add(this.Coil0);
      this.InputsGroupBox.Controls.Add(this.label8);
      this.InputsGroupBox.Controls.Add(this.label7);
      this.InputsGroupBox.Controls.Add(this.label6);
      this.InputsGroupBox.Controls.Add(this.label5);
      this.InputsGroupBox.Controls.Add(this.label4);
      this.InputsGroupBox.Controls.Add(this.label3);
      this.InputsGroupBox.Location = new System.Drawing.Point(21, 16);
      this.InputsGroupBox.Name = "InputsGroupBox";
      this.InputsGroupBox.Size = new System.Drawing.Size(419, 134);
      this.InputsGroupBox.TabIndex = 55;
      this.InputsGroupBox.TabStop = false;
      this.InputsGroupBox.Text = "Inputs";
      // 
      // InputRegister2
      // 
      this.InputRegister2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.InputRegister2.Location = new System.Drawing.Point(345, 98);
      this.InputRegister2.Name = "InputRegister2";
      this.InputRegister2.Size = new System.Drawing.Size(43, 23);
      this.InputRegister2.TabIndex = 76;
      this.InputRegister2.Text = "- - - - -";
      this.InputRegister2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // HoldingRegister2
      // 
      this.HoldingRegister2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.HoldingRegister2.Location = new System.Drawing.Point(246, 98);
      this.HoldingRegister2.Name = "HoldingRegister2";
      this.HoldingRegister2.Size = new System.Drawing.Size(43, 23);
      this.HoldingRegister2.TabIndex = 67;
      this.HoldingRegister2.Text = "- - - - -";
      this.HoldingRegister2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // InputRegister1
      // 
      this.InputRegister1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.InputRegister1.Location = new System.Drawing.Point(345, 70);
      this.InputRegister1.Name = "InputRegister1";
      this.InputRegister1.Size = new System.Drawing.Size(43, 23);
      this.InputRegister1.TabIndex = 75;
      this.InputRegister1.Text = "- - - - -";
      this.InputRegister1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // HoldingRegister1
      // 
      this.HoldingRegister1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.HoldingRegister1.Location = new System.Drawing.Point(246, 70);
      this.HoldingRegister1.Name = "HoldingRegister1";
      this.HoldingRegister1.Size = new System.Drawing.Size(43, 23);
      this.HoldingRegister1.TabIndex = 66;
      this.HoldingRegister1.Text = "- - - - -";
      this.HoldingRegister1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // InputRegister0
      // 
      this.InputRegister0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.InputRegister0.Location = new System.Drawing.Point(345, 44);
      this.InputRegister0.Name = "InputRegister0";
      this.InputRegister0.Size = new System.Drawing.Size(43, 23);
      this.InputRegister0.TabIndex = 74;
      this.InputRegister0.Text = "- - - - -";
      this.InputRegister0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // HoldingRegister0
      // 
      this.HoldingRegister0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.HoldingRegister0.Location = new System.Drawing.Point(246, 44);
      this.HoldingRegister0.Name = "HoldingRegister0";
      this.HoldingRegister0.Size = new System.Drawing.Size(43, 23);
      this.HoldingRegister0.TabIndex = 65;
      this.HoldingRegister0.Text = "- - - - -";
      this.HoldingRegister0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(331, 16);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(78, 13);
      this.label14.TabIndex = 73;
      this.label14.Text = "Input Registers";
      // 
      // DiscIn2
      // 
      this.DiscIn2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DiscIn2.Location = new System.Drawing.Point(137, 99);
      this.DiscIn2.Name = "DiscIn2";
      this.DiscIn2.Size = new System.Drawing.Size(49, 23);
      this.DiscIn2.TabIndex = 64;
      this.DiscIn2.Text = "- - - - -";
      this.DiscIn2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // DiscIn1
      // 
      this.DiscIn1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DiscIn1.Location = new System.Drawing.Point(137, 71);
      this.DiscIn1.Name = "DiscIn1";
      this.DiscIn1.Size = new System.Drawing.Size(49, 23);
      this.DiscIn1.TabIndex = 63;
      this.DiscIn1.Text = "- - - - -";
      this.DiscIn1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // DiscIn0
      // 
      this.DiscIn0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DiscIn0.Location = new System.Drawing.Point(137, 44);
      this.DiscIn0.Name = "DiscIn0";
      this.DiscIn0.Size = new System.Drawing.Size(49, 23);
      this.DiscIn0.TabIndex = 62;
      this.DiscIn0.Text = "- - - - -";
      this.DiscIn0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // Coil2
      // 
      this.Coil2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Coil2.Location = new System.Drawing.Point(68, 104);
      this.Coil2.Name = "Coil2";
      this.Coil2.Size = new System.Drawing.Size(27, 13);
      this.Coil2.TabIndex = 61;
      this.Coil2.Text = "- - -";
      this.Coil2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // Coil1
      // 
      this.Coil1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Coil1.Location = new System.Drawing.Point(68, 75);
      this.Coil1.Name = "Coil1";
      this.Coil1.Size = new System.Drawing.Size(27, 13);
      this.Coil1.TabIndex = 60;
      this.Coil1.Text = "- - -";
      this.Coil1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // Coil0
      // 
      this.Coil0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Coil0.Location = new System.Drawing.Point(68, 45);
      this.Coil0.Name = "Coil0";
      this.Coil0.Size = new System.Drawing.Size(27, 23);
      this.Coil0.TabIndex = 59;
      this.Coil0.Text = "- - -";
      this.Coil0.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(225, 16);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(90, 13);
      this.label8.TabIndex = 57;
      this.label8.Text = "Holding Registers";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(126, 16);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(78, 13);
      this.label7.TabIndex = 56;
      this.label7.Text = "Discrete Inputs";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(67, 16);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(29, 13);
      this.label6.TabIndex = 56;
      this.label6.Text = "Coils";
      // 
      // OutputsGroupBox
      // 
      this.OutputsGroupBox.Controls.Add(this.HoldingRegister2Send);
      this.OutputsGroupBox.Controls.Add(this.HoldingRegister2Val);
      this.OutputsGroupBox.Controls.Add(this.HoldingRegister1Send);
      this.OutputsGroupBox.Controls.Add(this.HoldingRegister1Val);
      this.OutputsGroupBox.Controls.Add(this.textBox3);
      this.OutputsGroupBox.Controls.Add(this.Coil2Off);
      this.OutputsGroupBox.Controls.Add(this.Coil2On);
      this.OutputsGroupBox.Controls.Add(this.textBox2);
      this.OutputsGroupBox.Controls.Add(this.Coil1Off);
      this.OutputsGroupBox.Controls.Add(this.Coil1On);
      this.OutputsGroupBox.Controls.Add(this.textBox1);
      this.OutputsGroupBox.Controls.Add(this.HoldingRegister0Send);
      this.OutputsGroupBox.Controls.Add(this.HoldingRegister0Val);
      this.OutputsGroupBox.Controls.Add(this.label2);
      this.OutputsGroupBox.Controls.Add(this.Coil0Off);
      this.OutputsGroupBox.Controls.Add(this.Coil0On);
      this.OutputsGroupBox.Controls.Add(this.label1);
      this.OutputsGroupBox.Location = new System.Drawing.Point(446, 16);
      this.OutputsGroupBox.Name = "OutputsGroupBox";
      this.OutputsGroupBox.Size = new System.Drawing.Size(369, 134);
      this.OutputsGroupBox.TabIndex = 56;
      this.OutputsGroupBox.TabStop = false;
      this.OutputsGroupBox.Text = "Outputs";
      // 
      // PollReadPB
      // 
      this.PollReadPB.Location = new System.Drawing.Point(13, 20);
      this.PollReadPB.Name = "PollReadPB";
      this.PollReadPB.Size = new System.Drawing.Size(75, 23);
      this.PollReadPB.TabIndex = 57;
      this.PollReadPB.Text = "Read";
      this.PollReadPB.UseVisualStyleBackColor = true;
      this.PollReadPB.Click += new System.EventHandler(this.Poll_Click);
      // 
      // PollEnableChkBox
      // 
      this.PollEnableChkBox.AutoSize = true;
      this.PollEnableChkBox.Checked = true;
      this.PollEnableChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.PollEnableChkBox.Location = new System.Drawing.Point(103, 23);
      this.PollEnableChkBox.Name = "PollEnableChkBox";
      this.PollEnableChkBox.Size = new System.Drawing.Size(90, 17);
      this.PollEnableChkBox.TabIndex = 59;
      this.PollEnableChkBox.Text = "Repeat every";
      this.PollEnableChkBox.UseVisualStyleBackColor = true;
      this.PollEnableChkBox.CheckedChanged += new System.EventHandler(this.ChangePollPeriod_Click);
      // 
      // PollIntervalMilliSecNumUpDown
      // 
      this.PollIntervalMilliSecNumUpDown.Location = new System.Drawing.Point(199, 22);
      this.PollIntervalMilliSecNumUpDown.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
      this.PollIntervalMilliSecNumUpDown.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
      this.PollIntervalMilliSecNumUpDown.Name = "PollIntervalMilliSecNumUpDown";
      this.PollIntervalMilliSecNumUpDown.Size = new System.Drawing.Size(74, 20);
      this.PollIntervalMilliSecNumUpDown.TabIndex = 65;
      this.PollIntervalMilliSecNumUpDown.Tag = "";
      this.PollIntervalMilliSecNumUpDown.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
      this.PollIntervalMilliSecNumUpDown.ValueChanged += new System.EventHandler(this.ChangePollPeriod_Click);
      // 
      // PollGroupBox
      // 
      this.PollGroupBox.Controls.Add(this.label9);
      this.PollGroupBox.Controls.Add(this.ReadProgressBar);
      this.PollGroupBox.Controls.Add(this.PollIntervalMilliSecNumUpDown);
      this.PollGroupBox.Controls.Add(this.PollEnableChkBox);
      this.PollGroupBox.Controls.Add(this.PollReadPB);
      this.PollGroupBox.Location = new System.Drawing.Point(21, 169);
      this.PollGroupBox.Name = "PollGroupBox";
      this.PollGroupBox.Size = new System.Drawing.Size(365, 75);
      this.PollGroupBox.TabIndex = 71;
      this.PollGroupBox.TabStop = false;
      this.PollGroupBox.Text = "PollGroupBox";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(280, 25);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(74, 13);
      this.label9.TabIndex = 73;
      this.label9.Text = "* 0.1 Seconds";
      // 
      // ReadProgressBar
      // 
      this.ReadProgressBar.Location = new System.Drawing.Point(103, 45);
      this.ReadProgressBar.Name = "ReadProgressBar";
      this.ReadProgressBar.Size = new System.Drawing.Size(246, 10);
      this.ReadProgressBar.TabIndex = 72;
      // 
      // ReadPollTimer
      // 
      this.ReadPollTimer.Tick += new System.EventHandler(this.PollTimer_Tick);
      // 
      // protocolAnalyzerTextCtrl
      // 
      this.protocolAnalyzerTextCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.protocolAnalyzerTextCtrl.BackColor = System.Drawing.Color.Gainsboro;
      this.protocolAnalyzerTextCtrl.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.protocolAnalyzerTextCtrl.Location = new System.Drawing.Point(21, 265);
      this.protocolAnalyzerTextCtrl.Name = "protocolAnalyzerTextCtrl";
      this.protocolAnalyzerTextCtrl.ReadOnly = true;
      this.protocolAnalyzerTextCtrl.Size = new System.Drawing.Size(794, 221);
      this.protocolAnalyzerTextCtrl.TabIndex = 39;
      this.protocolAnalyzerTextCtrl.Text = "";
      this.protocolAnalyzerTextCtrl.WordWrap = false;
      this.protocolAnalyzerTextCtrl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.protocolAnalyzerTextCtrl_MouseDown);
      // 
      // tmwLogoPictureBox
      // 
      this.tmwLogoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("tmwLogoPictureBox.Image")));
      this.tmwLogoPictureBox.Location = new System.Drawing.Point(446, 152);
      this.tmwLogoPictureBox.Name = "tmwLogoPictureBox";
      this.tmwLogoPictureBox.Size = new System.Drawing.Size(369, 101);
      this.tmwLogoPictureBox.TabIndex = 72;
      this.tmwLogoPictureBox.TabStop = false;
      // 
      // MasterForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(829, 498);
      this.Controls.Add(this.tmwLogoPictureBox);
      this.Controls.Add(this.PollGroupBox);
      this.Controls.Add(this.OutputsGroupBox);
      this.Controls.Add(this.InputsGroupBox);
      this.Controls.Add(this.protocolAnalyzerTextCtrl);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "MasterForm";
      this.Text = "TMW .NET Protocol Component Modbus Master Example";
      ((System.ComponentModel.ISupportInitialize)(this.HoldingRegister0Val)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.HoldingRegister1Val)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.HoldingRegister2Val)).EndInit();
      this.InputsGroupBox.ResumeLayout(false);
      this.InputsGroupBox.PerformLayout();
      this.OutputsGroupBox.ResumeLayout(false);
      this.OutputsGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PollIntervalMilliSecNumUpDown)).EndInit();
      this.PollGroupBox.ResumeLayout(false);
      this.PollGroupBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.tmwLogoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button Coil0On;
    private System.Windows.Forms.Button Coil0Off;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.NumericUpDown HoldingRegister0Val;
    private System.Windows.Forms.Button HoldingRegister0Send;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.Button Coil1Off;
    private System.Windows.Forms.Button Coil1On;
    private System.Windows.Forms.TextBox textBox3;
    private System.Windows.Forms.Button Coil2Off;
    private System.Windows.Forms.Button Coil2On;
    private System.Windows.Forms.Button HoldingRegister1Send;
    private System.Windows.Forms.NumericUpDown HoldingRegister1Val;
    private System.Windows.Forms.Button HoldingRegister2Send;
    private System.Windows.Forms.NumericUpDown HoldingRegister2Val;
    private System.Windows.Forms.GroupBox InputsGroupBox;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.GroupBox OutputsGroupBox;
    private System.Windows.Forms.Button PollReadPB;
    private System.Windows.Forms.CheckBox PollEnableChkBox;
    private System.Windows.Forms.NumericUpDown PollIntervalMilliSecNumUpDown;
    private System.Windows.Forms.GroupBox PollGroupBox;
    private System.Windows.Forms.ProgressBar ReadProgressBar;
    private System.Windows.Forms.Timer ReadPollTimer;
    private System.Windows.Forms.RichTextBox protocolAnalyzerTextCtrl;
    private System.Windows.Forms.PictureBox tmwLogoPictureBox;
    private System.Windows.Forms.Label Coil0;
    private System.Windows.Forms.Label Coil2;
    private System.Windows.Forms.Label Coil1;
    private System.Windows.Forms.Label DiscIn0;
    private System.Windows.Forms.Label DiscIn2;
    private System.Windows.Forms.Label DiscIn1;
    private System.Windows.Forms.Label HoldingRegister0;
    private System.Windows.Forms.Label HoldingRegister2;
    private System.Windows.Forms.Label HoldingRegister1;
    private System.Windows.Forms.Label InputRegister2;
    private System.Windows.Forms.Label InputRegister1;
    private System.Windows.Forms.Label InputRegister0;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.Label label9;
  }
}

