namespace DNPMasterSA
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
      this.IntegrityPollTimer = new System.Windows.Forms.Timer(this.components);
      this.EventPollTimer = new System.Windows.Forms.Timer(this.components);
      this.protocolAnalyzer = new System.Windows.Forms.RichTextBox();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.Online_LB = new System.Windows.Forms.Label();
      this.Connected_LB = new System.Windows.Forms.Label();
      this.button1 = new System.Windows.Forms.Button();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.BinOut2Feedback = new System.Windows.Forms.Label();
      this.BinOut1Feedback = new System.Windows.Forms.Label();
      this.BinOut0Feedback = new System.Windows.Forms.Label();
      this.AnlgOut2Feedback = new System.Windows.Forms.Label();
      this.AnlgOut1Feedback = new System.Windows.Forms.Label();
      this.AnlgOut0Feedback = new System.Windows.Forms.Label();
      this.AnlgOut2Send = new System.Windows.Forms.Button();
      this.AnlgOut2Val = new System.Windows.Forms.NumericUpDown();
      this.AnlgOut1Send = new System.Windows.Forms.Button();
      this.AnlgOut1Val = new System.Windows.Forms.NumericUpDown();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.BinOut2Off = new System.Windows.Forms.Button();
      this.BinOut2On = new System.Windows.Forms.Button();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.BinOut1Off = new System.Windows.Forms.Button();
      this.BinOut1On = new System.Windows.Forms.Button();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.AnlgOut0Send = new System.Windows.Forms.Button();
      this.AnlgOut0Val = new System.Windows.Forms.NumericUpDown();
      this.label2 = new System.Windows.Forms.Label();
      this.BinOut0Off = new System.Windows.Forms.Button();
      this.BinOut0On = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.UserNameTB = new System.Windows.Forms.TextBox();
      this.SendStatusChangeButton = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.OperationCB = new System.Windows.Forms.ComboBox();
      this.KeyChangeMethodCB = new System.Windows.Forms.ComboBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.UserInfoTB = new System.Windows.Forms.RichTextBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label6 = new System.Windows.Forms.Label();
      this.UserRoleCB = new System.Windows.Forms.ComboBox();
      this.label7 = new System.Windows.Forms.Label();
      this.StatusChangeSequenceUpDown = new System.Windows.Forms.NumericUpDown();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.AnlgOut2Val)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.AnlgOut1Val)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.AnlgOut0Val)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.StatusChangeSequenceUpDown)).BeginInit();
      this.SuspendLayout();
      // 
      // protocolAnalyzer
      // 
      this.protocolAnalyzer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.protocolAnalyzer.BackColor = System.Drawing.Color.Gainsboro;
      this.protocolAnalyzer.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.protocolAnalyzer.Location = new System.Drawing.Point(12, 491);
      this.protocolAnalyzer.Name = "protocolAnalyzer";
      this.protocolAnalyzer.ReadOnly = true;
      this.protocolAnalyzer.Size = new System.Drawing.Size(717, 354);
      this.protocolAnalyzer.TabIndex = 39;
      this.protocolAnalyzer.Text = "";
      this.protocolAnalyzer.WordWrap = false;
      this.protocolAnalyzer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.protocolAnalyzer_MouseDown);
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(357, 156);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(369, 101);
      this.pictureBox1.TabIndex = 72;
      this.pictureBox1.TabStop = false;
      // 
      // Online_LB
      // 
      this.Online_LB.AutoSize = true;
      this.Online_LB.ForeColor = System.Drawing.Color.Crimson;
      this.Online_LB.Location = new System.Drawing.Point(582, 9);
      this.Online_LB.Name = "Online_LB";
      this.Online_LB.Size = new System.Drawing.Size(37, 13);
      this.Online_LB.TabIndex = 94;
      this.Online_LB.Text = "Offline";
      // 
      // Connected_LB
      // 
      this.Connected_LB.AutoSize = true;
      this.Connected_LB.ForeColor = System.Drawing.Color.Crimson;
      this.Connected_LB.Location = new System.Drawing.Point(503, 9);
      this.Connected_LB.Name = "Connected_LB";
      this.Connected_LB.Size = new System.Drawing.Size(73, 13);
      this.Connected_LB.TabIndex = 93;
      this.Connected_LB.Text = "Disconnected";
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(654, 4);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 98;
      this.button1.Text = "Save Log";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.SaveLog_Click);
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.BinOut2Feedback);
      this.groupBox2.Controls.Add(this.BinOut1Feedback);
      this.groupBox2.Controls.Add(this.BinOut0Feedback);
      this.groupBox2.Controls.Add(this.AnlgOut2Feedback);
      this.groupBox2.Controls.Add(this.AnlgOut1Feedback);
      this.groupBox2.Controls.Add(this.AnlgOut0Feedback);
      this.groupBox2.Controls.Add(this.AnlgOut2Send);
      this.groupBox2.Controls.Add(this.AnlgOut2Val);
      this.groupBox2.Controls.Add(this.AnlgOut1Send);
      this.groupBox2.Controls.Add(this.AnlgOut1Val);
      this.groupBox2.Controls.Add(this.textBox3);
      this.groupBox2.Controls.Add(this.BinOut2Off);
      this.groupBox2.Controls.Add(this.BinOut2On);
      this.groupBox2.Controls.Add(this.textBox2);
      this.groupBox2.Controls.Add(this.BinOut1Off);
      this.groupBox2.Controls.Add(this.BinOut1On);
      this.groupBox2.Controls.Add(this.textBox1);
      this.groupBox2.Controls.Add(this.AnlgOut0Send);
      this.groupBox2.Controls.Add(this.AnlgOut0Val);
      this.groupBox2.Controls.Add(this.label2);
      this.groupBox2.Controls.Add(this.BinOut0Off);
      this.groupBox2.Controls.Add(this.BinOut0On);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Location = new System.Drawing.Point(357, 25);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(381, 125);
      this.groupBox2.TabIndex = 100;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Outputs";
      // 
      // BinOut2Feedback
      // 
      this.BinOut2Feedback.AutoSize = true;
      this.BinOut2Feedback.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.BinOut2Feedback.ForeColor = System.Drawing.SystemColors.ControlText;
      this.BinOut2Feedback.Location = new System.Drawing.Point(151, 103);
      this.BinOut2Feedback.Name = "BinOut2Feedback";
      this.BinOut2Feedback.Size = new System.Drawing.Size(19, 13);
      this.BinOut2Feedback.TabIndex = 60;
      this.BinOut2Feedback.Text = " - ";
      // 
      // BinOut1Feedback
      // 
      this.BinOut1Feedback.AutoSize = true;
      this.BinOut1Feedback.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.BinOut1Feedback.ForeColor = System.Drawing.SystemColors.ControlText;
      this.BinOut1Feedback.Location = new System.Drawing.Point(151, 75);
      this.BinOut1Feedback.Name = "BinOut1Feedback";
      this.BinOut1Feedback.Size = new System.Drawing.Size(19, 13);
      this.BinOut1Feedback.TabIndex = 59;
      this.BinOut1Feedback.Text = " - ";
      // 
      // BinOut0Feedback
      // 
      this.BinOut0Feedback.AutoSize = true;
      this.BinOut0Feedback.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.BinOut0Feedback.ForeColor = System.Drawing.SystemColors.ControlText;
      this.BinOut0Feedback.Location = new System.Drawing.Point(151, 45);
      this.BinOut0Feedback.Name = "BinOut0Feedback";
      this.BinOut0Feedback.Size = new System.Drawing.Size(19, 13);
      this.BinOut0Feedback.TabIndex = 58;
      this.BinOut0Feedback.Text = " - ";
      // 
      // AnlgOut2Feedback
      // 
      this.AnlgOut2Feedback.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AnlgOut2Feedback.Location = new System.Drawing.Point(306, 101);
      this.AnlgOut2Feedback.Name = "AnlgOut2Feedback";
      this.AnlgOut2Feedback.Size = new System.Drawing.Size(48, 13);
      this.AnlgOut2Feedback.TabIndex = 57;
      this.AnlgOut2Feedback.Text = "- - - - -";
      this.AnlgOut2Feedback.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // AnlgOut1Feedback
      // 
      this.AnlgOut1Feedback.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AnlgOut1Feedback.Location = new System.Drawing.Point(307, 74);
      this.AnlgOut1Feedback.Name = "AnlgOut1Feedback";
      this.AnlgOut1Feedback.Size = new System.Drawing.Size(47, 13);
      this.AnlgOut1Feedback.TabIndex = 56;
      this.AnlgOut1Feedback.Text = "- - - - -";
      this.AnlgOut1Feedback.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // AnlgOut0Feedback
      // 
      this.AnlgOut0Feedback.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AnlgOut0Feedback.Location = new System.Drawing.Point(307, 40);
      this.AnlgOut0Feedback.Name = "AnlgOut0Feedback";
      this.AnlgOut0Feedback.Size = new System.Drawing.Size(47, 23);
      this.AnlgOut0Feedback.TabIndex = 55;
      this.AnlgOut0Feedback.Text = "- - - - -";
      this.AnlgOut0Feedback.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // AnlgOut2Send
      // 
      this.AnlgOut2Send.Location = new System.Drawing.Point(253, 96);
      this.AnlgOut2Send.Name = "AnlgOut2Send";
      this.AnlgOut2Send.Size = new System.Drawing.Size(48, 23);
      this.AnlgOut2Send.TabIndex = 53;
      this.AnlgOut2Send.Tag = "2";
      this.AnlgOut2Send.Text = "Send";
      this.AnlgOut2Send.UseVisualStyleBackColor = true;
      this.AnlgOut2Send.Click += new System.EventHandler(this.AnlgOutSend_Click);
      // 
      // AnlgOut2Val
      // 
      this.AnlgOut2Val.Location = new System.Drawing.Point(188, 98);
      this.AnlgOut2Val.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
      this.AnlgOut2Val.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
      this.AnlgOut2Val.Name = "AnlgOut2Val";
      this.AnlgOut2Val.Size = new System.Drawing.Size(56, 20);
      this.AnlgOut2Val.TabIndex = 52;
      this.AnlgOut2Val.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // AnlgOut1Send
      // 
      this.AnlgOut1Send.Location = new System.Drawing.Point(253, 70);
      this.AnlgOut1Send.Name = "AnlgOut1Send";
      this.AnlgOut1Send.Size = new System.Drawing.Size(48, 23);
      this.AnlgOut1Send.TabIndex = 50;
      this.AnlgOut1Send.Tag = "1";
      this.AnlgOut1Send.Text = "Send";
      this.AnlgOut1Send.UseVisualStyleBackColor = true;
      this.AnlgOut1Send.Click += new System.EventHandler(this.AnlgOutSend_Click);
      // 
      // AnlgOut1Val
      // 
      this.AnlgOut1Val.Location = new System.Drawing.Point(188, 71);
      this.AnlgOut1Val.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
      this.AnlgOut1Val.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
      this.AnlgOut1Val.Name = "AnlgOut1Val";
      this.AnlgOut1Val.Size = new System.Drawing.Size(56, 20);
      this.AnlgOut1Val.TabIndex = 49;
      this.AnlgOut1Val.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
      // BinOut2Off
      // 
      this.BinOut2Off.Location = new System.Drawing.Point(95, 98);
      this.BinOut2Off.Name = "BinOut2Off";
      this.BinOut2Off.Size = new System.Drawing.Size(49, 23);
      this.BinOut2Off.TabIndex = 46;
      this.BinOut2Off.Tag = "2";
      this.BinOut2Off.Text = "Off";
      this.BinOut2Off.UseVisualStyleBackColor = true;
      this.BinOut2Off.Click += new System.EventHandler(this.BinOutOff_Click);
      // 
      // BinOut2On
      // 
      this.BinOut2On.Location = new System.Drawing.Point(40, 98);
      this.BinOut2On.Name = "BinOut2On";
      this.BinOut2On.Size = new System.Drawing.Size(49, 23);
      this.BinOut2On.TabIndex = 45;
      this.BinOut2On.Tag = "2";
      this.BinOut2On.Text = "On";
      this.BinOut2On.UseVisualStyleBackColor = true;
      this.BinOut2On.Click += new System.EventHandler(this.BinOutOn_Click);
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
      // BinOut1Off
      // 
      this.BinOut1Off.Location = new System.Drawing.Point(95, 69);
      this.BinOut1Off.Name = "BinOut1Off";
      this.BinOut1Off.Size = new System.Drawing.Size(49, 23);
      this.BinOut1Off.TabIndex = 42;
      this.BinOut1Off.Tag = "1";
      this.BinOut1Off.Text = "Off";
      this.BinOut1Off.UseVisualStyleBackColor = true;
      this.BinOut1Off.Click += new System.EventHandler(this.BinOutOff_Click);
      // 
      // BinOut1On
      // 
      this.BinOut1On.Location = new System.Drawing.Point(40, 69);
      this.BinOut1On.Name = "BinOut1On";
      this.BinOut1On.Size = new System.Drawing.Size(49, 23);
      this.BinOut1On.TabIndex = 41;
      this.BinOut1On.Tag = "1";
      this.BinOut1On.Text = "On";
      this.BinOut1On.UseVisualStyleBackColor = true;
      this.BinOut1On.Click += new System.EventHandler(this.BinOutOn_Click);
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
      // AnlgOut0Send
      // 
      this.AnlgOut0Send.Location = new System.Drawing.Point(253, 40);
      this.AnlgOut0Send.Name = "AnlgOut0Send";
      this.AnlgOut0Send.Size = new System.Drawing.Size(48, 23);
      this.AnlgOut0Send.TabIndex = 11;
      this.AnlgOut0Send.Tag = "0";
      this.AnlgOut0Send.Text = "Send";
      this.AnlgOut0Send.UseVisualStyleBackColor = true;
      this.AnlgOut0Send.Click += new System.EventHandler(this.AnlgOutSend_Click);
      // 
      // AnlgOut0Val
      // 
      this.AnlgOut0Val.Location = new System.Drawing.Point(188, 42);
      this.AnlgOut0Val.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
      this.AnlgOut0Val.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
      this.AnlgOut0Val.Name = "AnlgOut0Val";
      this.AnlgOut0Val.Size = new System.Drawing.Size(56, 20);
      this.AnlgOut0Val.TabIndex = 8;
      this.AnlgOut0Val.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(236, 16);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(40, 13);
      this.label2.TabIndex = 7;
      this.label2.Text = "Analog";
      // 
      // BinOut0Off
      // 
      this.BinOut0Off.Location = new System.Drawing.Point(95, 40);
      this.BinOut0Off.Name = "BinOut0Off";
      this.BinOut0Off.Size = new System.Drawing.Size(49, 23);
      this.BinOut0Off.TabIndex = 2;
      this.BinOut0Off.Tag = "0";
      this.BinOut0Off.Text = "Off";
      this.BinOut0Off.UseVisualStyleBackColor = true;
      this.BinOut0Off.Click += new System.EventHandler(this.BinOutOff_Click);
      // 
      // BinOut0On
      // 
      this.BinOut0On.Location = new System.Drawing.Point(40, 40);
      this.BinOut0On.Name = "BinOut0On";
      this.BinOut0On.Size = new System.Drawing.Size(49, 23);
      this.BinOut0On.TabIndex = 1;
      this.BinOut0On.Tag = "0";
      this.BinOut0On.Text = "On";
      this.BinOut0On.UseVisualStyleBackColor = true;
      this.BinOut0On.Click += new System.EventHandler(this.BinOutOn_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(78, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(36, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Binary";
      // 
      // UserNameTB
      // 
      this.UserNameTB.Location = new System.Drawing.Point(33, 70);
      this.UserNameTB.Name = "UserNameTB";
      this.UserNameTB.Size = new System.Drawing.Size(133, 20);
      this.UserNameTB.TabIndex = 101;
      this.UserNameTB.Text = "Globally Unique Name 1";
      // 
      // SendStatusChangeButton
      // 
      this.SendStatusChangeButton.Location = new System.Drawing.Point(76, 222);
      this.SendStatusChangeButton.Name = "SendStatusChangeButton";
      this.SendStatusChangeButton.Size = new System.Drawing.Size(144, 23);
      this.SendStatusChangeButton.TabIndex = 102;
      this.SendStatusChangeButton.Text = "Send User Status Change";
      this.SendStatusChangeButton.UseVisualStyleBackColor = true;
      this.SendStatusChangeButton.Click += new System.EventHandler(this.ActAsAuthority_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(30, 54);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(60, 13);
      this.label3.TabIndex = 103;
      this.label3.Text = "User Name";
      // 
      // OperationCB
      // 
      this.OperationCB.FormattingEnabled = true;
      this.OperationCB.Items.AddRange(new object[] {
            "Add",
            "Change",
            "Delete"});
      this.OperationCB.Location = new System.Drawing.Point(33, 182);
      this.OperationCB.Name = "OperationCB";
      this.OperationCB.Size = new System.Drawing.Size(74, 21);
      this.OperationCB.TabIndex = 104;
      this.OperationCB.Text = "Add";
      // 
      // KeyChangeMethodCB
      // 
      this.KeyChangeMethodCB.FormattingEnabled = true;
      this.KeyChangeMethodCB.Items.AddRange(new object[] {
            "AES-128 / SHA-1 HMAC",
            "AES-256 / SHA-256-HMAC",
            "RSA-1024 / SHA-1-HMAC",
            "RSA-2048 / SHA-256-HMAC",
            "RSA-3072 / SHA-256-HMAC"});
      this.KeyChangeMethodCB.Location = new System.Drawing.Point(33, 129);
      this.KeyChangeMethodCB.Name = "KeyChangeMethodCB";
      this.KeyChangeMethodCB.Size = new System.Drawing.Size(187, 21);
      this.KeyChangeMethodCB.TabIndex = 105;
      this.KeyChangeMethodCB.Text = "AES-128 / SHA-1 HMAC";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(33, 166);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(53, 13);
      this.label4.TabIndex = 106;
      this.label4.Text = "Operation";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(30, 104);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(104, 13);
      this.label5.TabIndex = 107;
      this.label5.Text = "Key Change Method";
      // 
      // UserInfoTB
      // 
      this.UserInfoTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.UserInfoTB.BackColor = System.Drawing.Color.Gainsboro;
      this.UserInfoTB.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UserInfoTB.Location = new System.Drawing.Point(12, 265);
      this.UserInfoTB.Name = "UserInfoTB";
      this.UserInfoTB.ReadOnly = true;
      this.UserInfoTB.Size = new System.Drawing.Size(717, 209);
      this.UserInfoTB.TabIndex = 108;
      this.UserInfoTB.Text = "";
      this.UserInfoTB.WordWrap = false;
      // 
      // groupBox1
      // 
      this.groupBox1.Location = new System.Drawing.Point(212, 70);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(8, 8);
      this.groupBox1.TabIndex = 109;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "groupBox1";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(212, 54);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(54, 13);
      this.label6.TabIndex = 111;
      this.label6.Text = "User Role";
      // 
      // UserRoleCB
      // 
      this.UserRoleCB.FormattingEnabled = true;
      this.UserRoleCB.Items.AddRange(new object[] {
            "Viewer",
            "Operator",
            "Engineer",
            "Installer",
            "SecAdm",
            "SecAud",
            "RBACmnt",
            "SingleUser"});
      this.UserRoleCB.Location = new System.Drawing.Point(212, 70);
      this.UserRoleCB.Name = "UserRoleCB";
      this.UserRoleCB.Size = new System.Drawing.Size(74, 21);
      this.UserRoleCB.TabIndex = 110;
      this.UserRoleCB.Text = "Viewer";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(150, 166);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(169, 13);
      this.label7.TabIndex = 113;
      this.label7.Text = "Status Change Sequence Number";
      // 
      // StatusChangeSequenceUpDown
      // 
      this.StatusChangeSequenceUpDown.Location = new System.Drawing.Point(153, 182);
      this.StatusChangeSequenceUpDown.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
      this.StatusChangeSequenceUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.StatusChangeSequenceUpDown.Name = "StatusChangeSequenceUpDown";
      this.StatusChangeSequenceUpDown.Size = new System.Drawing.Size(120, 20);
      this.StatusChangeSequenceUpDown.TabIndex = 114;
      this.toolTip1.SetToolTip(this.StatusChangeSequenceUpDown, "Status Change Sequence Number must increment for each User Status Change Request " +
              "or Outstation will return an error.");
      this.StatusChangeSequenceUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // MasterForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(752, 857);
      this.Controls.Add(this.StatusChangeSequenceUpDown);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.UserRoleCB);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.UserInfoTB);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.KeyChangeMethodCB);
      this.Controls.Add(this.OperationCB);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.SendStatusChangeButton);
      this.Controls.Add(this.UserNameTB);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.Online_LB);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.Connected_LB);
      this.Controls.Add(this.protocolAnalyzer);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "MasterForm";
      this.Text = "TMW .NET DNP Master Secure Authentication SAv5 Example";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.AnlgOut2Val)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.AnlgOut1Val)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.AnlgOut0Val)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.StatusChangeSequenceUpDown)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Timer IntegrityPollTimer;
    private System.Windows.Forms.Timer EventPollTimer;
    private System.Windows.Forms.RichTextBox protocolAnalyzer;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Label Online_LB;
    private System.Windows.Forms.Label Connected_LB;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Label BinOut2Feedback;
    private System.Windows.Forms.Label BinOut1Feedback;
    private System.Windows.Forms.Label BinOut0Feedback;
    private System.Windows.Forms.Label AnlgOut2Feedback;
    private System.Windows.Forms.Label AnlgOut1Feedback;
    private System.Windows.Forms.Label AnlgOut0Feedback;
    private System.Windows.Forms.Button AnlgOut2Send;
    private System.Windows.Forms.NumericUpDown AnlgOut2Val;
    private System.Windows.Forms.Button AnlgOut1Send;
    private System.Windows.Forms.NumericUpDown AnlgOut1Val;
    private System.Windows.Forms.TextBox textBox3;
    private System.Windows.Forms.Button BinOut2Off;
    private System.Windows.Forms.Button BinOut2On;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.Button BinOut1Off;
    private System.Windows.Forms.Button BinOut1On;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Button AnlgOut0Send;
    private System.Windows.Forms.NumericUpDown AnlgOut0Val;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button BinOut0Off;
    private System.Windows.Forms.Button BinOut0On;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox UserNameTB;
    private System.Windows.Forms.Button SendStatusChangeButton;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox OperationCB;
    private System.Windows.Forms.ComboBox KeyChangeMethodCB;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.RichTextBox UserInfoTB;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.ComboBox UserRoleCB;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.NumericUpDown StatusChangeSequenceUpDown;
    private System.Windows.Forms.ToolTip toolTip1;
  }
}

