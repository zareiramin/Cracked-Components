namespace DNPmasterModem
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
      this.Integrity = new System.Windows.Forms.Button();
      this.Event = new System.Windows.Forms.Button();
      this.IntegrityEnable = new System.Windows.Forms.CheckBox();
      this.EventEnable = new System.Windows.Forms.CheckBox();
      this.IntegrityIntervalHr = new System.Windows.Forms.NumericUpDown();
      this.label9 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.IntegrityIntervalMin = new System.Windows.Forms.NumericUpDown();
      this.IntegrityIntervalSec = new System.Windows.Forms.NumericUpDown();
      this.EventIntervalSec = new System.Windows.Forms.NumericUpDown();
      this.label11 = new System.Windows.Forms.Label();
      this.EventIntervalMin = new System.Windows.Forms.NumericUpDown();
      this.label12 = new System.Windows.Forms.Label();
      this.EventIntervalHr = new System.Windows.Forms.NumericUpDown();
      this.Poll = new System.Windows.Forms.GroupBox();
      this.EventProgressBar = new System.Windows.Forms.ProgressBar();
      this.IntegrityProgressBar = new System.Windows.Forms.ProgressBar();
      this.IntegrityPollTimer = new System.Windows.Forms.Timer(this.components);
      this.EventPollTimer = new System.Windows.Forms.Timer(this.components);
      this.protocolAnalyzer = new System.Windows.Forms.RichTextBox();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.HardwareCB = new System.Windows.Forms.CheckBox();
      this.BaudLB = new System.Windows.Forms.Label();
      this.BaudTB = new System.Windows.Forms.TextBox();
      this.InitBT = new System.Windows.Forms.Button();
      this.InitTB = new System.Windows.Forms.TextBox();
      this.DisconnectBT = new System.Windows.Forms.Button();
      this.DisconnectTB = new System.Windows.Forms.TextBox();
      this.PortLB = new System.Windows.Forms.Label();
      this.PortTB = new System.Windows.Forms.TextBox();
      this.DisabledCB = new System.Windows.Forms.CheckBox();
      this.ConnectBT = new System.Windows.Forms.Button();
      this.ConnectTB = new System.Windows.Forms.TextBox();
      this.CloseBT = new System.Windows.Forms.Button();
      this.OpenBT = new System.Windows.Forms.Button();
      this.OutDataTB = new System.Windows.Forms.RichTextBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      ((System.ComponentModel.ISupportInitialize)(this.IntegrityIntervalHr)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.IntegrityIntervalMin)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.IntegrityIntervalSec)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.EventIntervalSec)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.EventIntervalMin)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.EventIntervalHr)).BeginInit();
      this.Poll.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // Integrity
      // 
      this.Integrity.Location = new System.Drawing.Point(13, 20);
      this.Integrity.Name = "Integrity";
      this.Integrity.Size = new System.Drawing.Size(75, 23);
      this.Integrity.TabIndex = 57;
      this.Integrity.Text = "Integrity";
      this.Integrity.UseVisualStyleBackColor = true;
      this.Integrity.Click += new System.EventHandler(this.Integrity_Click);
      // 
      // Event
      // 
      this.Event.Location = new System.Drawing.Point(13, 58);
      this.Event.Name = "Event";
      this.Event.Size = new System.Drawing.Size(75, 23);
      this.Event.TabIndex = 58;
      this.Event.Text = "Event";
      this.Event.UseVisualStyleBackColor = true;
      this.Event.Click += new System.EventHandler(this.Event_Click);
      // 
      // IntegrityEnable
      // 
      this.IntegrityEnable.AutoSize = true;
      this.IntegrityEnable.Location = new System.Drawing.Point(103, 23);
      this.IntegrityEnable.Name = "IntegrityEnable";
      this.IntegrityEnable.Size = new System.Drawing.Size(90, 17);
      this.IntegrityEnable.TabIndex = 59;
      this.IntegrityEnable.Text = "Repeat every";
      this.IntegrityEnable.UseVisualStyleBackColor = true;
      this.IntegrityEnable.CheckedChanged += new System.EventHandler(this.IntegrityInterval_ValueChanged);
      // 
      // EventEnable
      // 
      this.EventEnable.AutoSize = true;
      this.EventEnable.Location = new System.Drawing.Point(103, 61);
      this.EventEnable.Name = "EventEnable";
      this.EventEnable.Size = new System.Drawing.Size(90, 17);
      this.EventEnable.TabIndex = 60;
      this.EventEnable.Text = "Repeat every";
      this.EventEnable.UseVisualStyleBackColor = true;
      this.EventEnable.CheckedChanged += new System.EventHandler(this.EventInterval_ValueChanged);
      // 
      // IntegrityIntervalHr
      // 
      this.IntegrityIntervalHr.Location = new System.Drawing.Point(200, 22);
      this.IntegrityIntervalHr.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
      this.IntegrityIntervalHr.Name = "IntegrityIntervalHr";
      this.IntegrityIntervalHr.Size = new System.Drawing.Size(34, 20);
      this.IntegrityIntervalHr.TabIndex = 61;
      this.IntegrityIntervalHr.Tag = "Hour";
      this.IntegrityIntervalHr.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.IntegrityIntervalHr.ValueChanged += new System.EventHandler(this.IntegrityInterval_ValueChanged);
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(233, 26);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(10, 13);
      this.label9.TabIndex = 62;
      this.label9.Text = ":";
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(279, 26);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(10, 13);
      this.label10.TabIndex = 64;
      this.label10.Text = ":";
      // 
      // IntegrityIntervalMin
      // 
      this.IntegrityIntervalMin.Location = new System.Drawing.Point(245, 22);
      this.IntegrityIntervalMin.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
      this.IntegrityIntervalMin.Name = "IntegrityIntervalMin";
      this.IntegrityIntervalMin.Size = new System.Drawing.Size(34, 20);
      this.IntegrityIntervalMin.TabIndex = 63;
      this.IntegrityIntervalMin.Tag = "Min";
      this.IntegrityIntervalMin.ValueChanged += new System.EventHandler(this.IntegrityInterval_ValueChanged);
      // 
      // IntegrityIntervalSec
      // 
      this.IntegrityIntervalSec.Location = new System.Drawing.Point(290, 22);
      this.IntegrityIntervalSec.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
      this.IntegrityIntervalSec.Name = "IntegrityIntervalSec";
      this.IntegrityIntervalSec.Size = new System.Drawing.Size(34, 20);
      this.IntegrityIntervalSec.TabIndex = 65;
      this.IntegrityIntervalSec.Tag = "sec";
      this.IntegrityIntervalSec.ValueChanged += new System.EventHandler(this.IntegrityInterval_ValueChanged);
      // 
      // EventIntervalSec
      // 
      this.EventIntervalSec.Location = new System.Drawing.Point(290, 58);
      this.EventIntervalSec.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
      this.EventIntervalSec.Name = "EventIntervalSec";
      this.EventIntervalSec.Size = new System.Drawing.Size(34, 20);
      this.EventIntervalSec.TabIndex = 70;
      this.EventIntervalSec.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
      this.EventIntervalSec.ValueChanged += new System.EventHandler(this.EventInterval_ValueChanged);
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(279, 61);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(10, 13);
      this.label11.TabIndex = 69;
      this.label11.Text = ":";
      // 
      // EventIntervalMin
      // 
      this.EventIntervalMin.Location = new System.Drawing.Point(245, 58);
      this.EventIntervalMin.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
      this.EventIntervalMin.Name = "EventIntervalMin";
      this.EventIntervalMin.Size = new System.Drawing.Size(34, 20);
      this.EventIntervalMin.TabIndex = 68;
      this.EventIntervalMin.ValueChanged += new System.EventHandler(this.EventInterval_ValueChanged);
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(233, 60);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(10, 13);
      this.label12.TabIndex = 67;
      this.label12.Text = ":";
      // 
      // EventIntervalHr
      // 
      this.EventIntervalHr.Location = new System.Drawing.Point(200, 58);
      this.EventIntervalHr.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
      this.EventIntervalHr.Name = "EventIntervalHr";
      this.EventIntervalHr.Size = new System.Drawing.Size(34, 20);
      this.EventIntervalHr.TabIndex = 66;
      this.EventIntervalHr.ValueChanged += new System.EventHandler(this.EventInterval_ValueChanged);
      // 
      // Poll
      // 
      this.Poll.Controls.Add(this.EventProgressBar);
      this.Poll.Controls.Add(this.IntegrityProgressBar);
      this.Poll.Controls.Add(this.EventIntervalSec);
      this.Poll.Controls.Add(this.label11);
      this.Poll.Controls.Add(this.EventIntervalMin);
      this.Poll.Controls.Add(this.label12);
      this.Poll.Controls.Add(this.EventIntervalHr);
      this.Poll.Controls.Add(this.IntegrityIntervalSec);
      this.Poll.Controls.Add(this.label10);
      this.Poll.Controls.Add(this.IntegrityIntervalMin);
      this.Poll.Controls.Add(this.label9);
      this.Poll.Controls.Add(this.IntegrityIntervalHr);
      this.Poll.Controls.Add(this.EventEnable);
      this.Poll.Controls.Add(this.IntegrityEnable);
      this.Poll.Controls.Add(this.Event);
      this.Poll.Controls.Add(this.Integrity);
      this.Poll.Location = new System.Drawing.Point(393, 128);
      this.Poll.Name = "Poll";
      this.Poll.Size = new System.Drawing.Size(333, 105);
      this.Poll.TabIndex = 71;
      this.Poll.TabStop = false;
      this.Poll.Text = "Poll";
      // 
      // EventProgressBar
      // 
      this.EventProgressBar.Location = new System.Drawing.Point(103, 82);
      this.EventProgressBar.Name = "EventProgressBar";
      this.EventProgressBar.Size = new System.Drawing.Size(221, 10);
      this.EventProgressBar.TabIndex = 73;
      // 
      // IntegrityProgressBar
      // 
      this.IntegrityProgressBar.Location = new System.Drawing.Point(103, 45);
      this.IntegrityProgressBar.Name = "IntegrityProgressBar";
      this.IntegrityProgressBar.Size = new System.Drawing.Size(221, 10);
      this.IntegrityProgressBar.TabIndex = 72;
      // 
      // IntegrityPollTimer
      // 
      this.IntegrityPollTimer.Interval = 1000;
      this.IntegrityPollTimer.Tick += new System.EventHandler(this.IntegrityPollTimer_Tick);
      // 
      // EventPollTimer
      // 
      this.EventPollTimer.Interval = 1000;
      this.EventPollTimer.Tick += new System.EventHandler(this.EventPollTimer_Tick);
      // 
      // protocolAnalyzer
      // 
      this.protocolAnalyzer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.protocolAnalyzer.BackColor = System.Drawing.Color.Gainsboro;
      this.protocolAnalyzer.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.protocolAnalyzer.Location = new System.Drawing.Point(23, 352);
      this.protocolAnalyzer.Name = "protocolAnalyzer";
      this.protocolAnalyzer.ReadOnly = true;
      this.protocolAnalyzer.Size = new System.Drawing.Size(717, 389);
      this.protocolAnalyzer.TabIndex = 39;
      this.protocolAnalyzer.Text = "";
      this.protocolAnalyzer.WordWrap = false;
      this.protocolAnalyzer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.protocolAnalyzer_MouseDown);
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(155, 12);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(369, 101);
      this.pictureBox1.TabIndex = 72;
      this.pictureBox1.TabStop = false;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.HardwareCB);
      this.groupBox1.Controls.Add(this.BaudLB);
      this.groupBox1.Controls.Add(this.BaudTB);
      this.groupBox1.Controls.Add(this.InitBT);
      this.groupBox1.Controls.Add(this.InitTB);
      this.groupBox1.Controls.Add(this.DisconnectBT);
      this.groupBox1.Controls.Add(this.DisconnectTB);
      this.groupBox1.Controls.Add(this.PortLB);
      this.groupBox1.Controls.Add(this.PortTB);
      this.groupBox1.Controls.Add(this.DisabledCB);
      this.groupBox1.Controls.Add(this.ConnectBT);
      this.groupBox1.Controls.Add(this.ConnectTB);
      this.groupBox1.Controls.Add(this.CloseBT);
      this.groupBox1.Controls.Add(this.OpenBT);
      this.groupBox1.Location = new System.Drawing.Point(32, 119);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(343, 227);
      this.groupBox1.TabIndex = 73;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Connection";
      // 
      // HardwareCB
      // 
      this.HardwareCB.AutoSize = true;
      this.HardwareCB.Checked = true;
      this.HardwareCB.CheckState = System.Windows.Forms.CheckState.Checked;
      this.HardwareCB.Location = new System.Drawing.Point(191, 48);
      this.HardwareCB.Name = "HardwareCB";
      this.HardwareCB.Size = new System.Drawing.Size(133, 17);
      this.HardwareCB.TabIndex = 13;
      this.HardwareCB.Text = "Hardware Flow Control";
      this.HardwareCB.UseVisualStyleBackColor = true;
      // 
      // BaudLB
      // 
      this.BaudLB.AutoSize = true;
      this.BaudLB.Location = new System.Drawing.Point(21, 48);
      this.BaudLB.Name = "BaudLB";
      this.BaudLB.Size = new System.Drawing.Size(32, 13);
      this.BaudLB.TabIndex = 12;
      this.BaudLB.Text = "Baud";
      // 
      // BaudTB
      // 
      this.BaudTB.Location = new System.Drawing.Point(91, 45);
      this.BaudTB.Name = "BaudTB";
      this.BaudTB.Size = new System.Drawing.Size(68, 20);
      this.BaudTB.TabIndex = 11;
      this.BaudTB.Text = "9600";
      // 
      // InitBT
      // 
      this.InitBT.Location = new System.Drawing.Point(24, 131);
      this.InitBT.Name = "InitBT";
      this.InitBT.Size = new System.Drawing.Size(75, 23);
      this.InitBT.TabIndex = 10;
      this.InitBT.Text = "Init Modem";
      this.InitBT.UseVisualStyleBackColor = true;
      this.InitBT.Click += new System.EventHandler(this.InitBT_Click);
      // 
      // InitTB
      // 
      this.InitTB.Location = new System.Drawing.Point(146, 134);
      this.InitTB.Name = "InitTB";
      this.InitTB.Size = new System.Drawing.Size(191, 20);
      this.InitTB.TabIndex = 9;
      this.InitTB.Text = "ATQ0V1E1";
      // 
      // DisconnectBT
      // 
      this.DisconnectBT.Location = new System.Drawing.Point(24, 183);
      this.DisconnectBT.Name = "DisconnectBT";
      this.DisconnectBT.Size = new System.Drawing.Size(75, 23);
      this.DisconnectBT.TabIndex = 8;
      this.DisconnectBT.Text = "Disconnect";
      this.DisconnectBT.UseVisualStyleBackColor = true;
      this.DisconnectBT.Click += new System.EventHandler(this.DisconnectBT_Click);
      // 
      // DisconnectTB
      // 
      this.DisconnectTB.Location = new System.Drawing.Point(146, 186);
      this.DisconnectTB.Name = "DisconnectTB";
      this.DisconnectTB.Size = new System.Drawing.Size(191, 20);
      this.DisconnectTB.TabIndex = 7;
      this.DisconnectTB.Text = "+++ATH";
      // 
      // PortLB
      // 
      this.PortLB.AutoSize = true;
      this.PortLB.Location = new System.Drawing.Point(21, 22);
      this.PortLB.Name = "PortLB";
      this.PortLB.Size = new System.Drawing.Size(53, 13);
      this.PortLB.TabIndex = 6;
      this.PortLB.Text = "COM Port";
      // 
      // PortTB
      // 
      this.PortTB.Location = new System.Drawing.Point(91, 19);
      this.PortTB.Name = "PortTB";
      this.PortTB.Size = new System.Drawing.Size(68, 20);
      this.PortTB.TabIndex = 5;
      this.PortTB.Text = "COM1";
      // 
      // DisabledCB
      // 
      this.DisabledCB.AutoSize = true;
      this.DisabledCB.Checked = true;
      this.DisabledCB.CheckState = System.Windows.Forms.CheckState.Checked;
      this.DisabledCB.Location = new System.Drawing.Point(191, 22);
      this.DisabledCB.Name = "DisabledCB";
      this.DisabledCB.Size = new System.Drawing.Size(135, 17);
      this.DisabledCB.TabIndex = 4;
      this.DisabledCB.Text = "Disabled Data Transfer";
      this.DisabledCB.UseVisualStyleBackColor = true;
      this.DisabledCB.CheckedChanged += new System.EventHandler(this.DisabledCB_CheckedChanged);
      // 
      // ConnectBT
      // 
      this.ConnectBT.Location = new System.Drawing.Point(24, 157);
      this.ConnectBT.Name = "ConnectBT";
      this.ConnectBT.Size = new System.Drawing.Size(75, 23);
      this.ConnectBT.TabIndex = 3;
      this.ConnectBT.Text = "Connect";
      this.ConnectBT.UseVisualStyleBackColor = true;
      this.ConnectBT.Click += new System.EventHandler(this.ConnectBT_Click);
      // 
      // ConnectTB
      // 
      this.ConnectTB.Location = new System.Drawing.Point(146, 160);
      this.ConnectTB.Name = "ConnectTB";
      this.ConnectTB.Size = new System.Drawing.Size(191, 20);
      this.ConnectTB.TabIndex = 2;
      this.ConnectTB.Text = "ATDT5551212";
      // 
      // CloseBT
      // 
      this.CloseBT.Location = new System.Drawing.Point(146, 84);
      this.CloseBT.Name = "CloseBT";
      this.CloseBT.Size = new System.Drawing.Size(75, 23);
      this.CloseBT.TabIndex = 1;
      this.CloseBT.Text = "Close";
      this.CloseBT.UseVisualStyleBackColor = true;
      this.CloseBT.Click += new System.EventHandler(this.CloseBT_Click);
      // 
      // OpenBT
      // 
      this.OpenBT.Location = new System.Drawing.Point(24, 84);
      this.OpenBT.Name = "OpenBT";
      this.OpenBT.Size = new System.Drawing.Size(75, 23);
      this.OpenBT.TabIndex = 0;
      this.OpenBT.Text = "Open";
      this.OpenBT.UseVisualStyleBackColor = true;
      this.OpenBT.Click += new System.EventHandler(this.OpenBT_Click);
      // 
      // OutDataTB
      // 
      this.OutDataTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.OutDataTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.OutDataTB.Location = new System.Drawing.Point(13, 19);
      this.OutDataTB.Name = "OutDataTB";
      this.OutDataTB.Size = new System.Drawing.Size(314, 82);
      this.OutDataTB.TabIndex = 74;
      this.OutDataTB.Text = "";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.OutDataTB);
      this.groupBox2.Location = new System.Drawing.Point(393, 239);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(347, 107);
      this.groupBox2.TabIndex = 75;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Modem Data";
      // 
      // MasterForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(752, 753);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.Poll);
      this.Controls.Add(this.protocolAnalyzer);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "MasterForm";
      this.Text = "TMW .NET Protocol Component DNP Master Example";
      ((System.ComponentModel.ISupportInitialize)(this.IntegrityIntervalHr)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.IntegrityIntervalMin)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.IntegrityIntervalSec)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.EventIntervalSec)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.EventIntervalMin)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.EventIntervalHr)).EndInit();
      this.Poll.ResumeLayout(false);
      this.Poll.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button Integrity;
    private System.Windows.Forms.Button Event;
    private System.Windows.Forms.CheckBox IntegrityEnable;
    private System.Windows.Forms.CheckBox EventEnable;
    private System.Windows.Forms.NumericUpDown IntegrityIntervalHr;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.NumericUpDown IntegrityIntervalMin;
    private System.Windows.Forms.NumericUpDown IntegrityIntervalSec;
    private System.Windows.Forms.NumericUpDown EventIntervalSec;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.NumericUpDown EventIntervalMin;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.NumericUpDown EventIntervalHr;
    private System.Windows.Forms.GroupBox Poll;
    private System.Windows.Forms.ProgressBar EventProgressBar;
    private System.Windows.Forms.ProgressBar IntegrityProgressBar;
    private System.Windows.Forms.Timer IntegrityPollTimer;
    private System.Windows.Forms.Timer EventPollTimer;
    private System.Windows.Forms.RichTextBox protocolAnalyzer;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button CloseBT;
    private System.Windows.Forms.Button OpenBT;
    private System.Windows.Forms.Button ConnectBT;
    private System.Windows.Forms.TextBox ConnectTB;
    private System.Windows.Forms.CheckBox DisabledCB;
    private System.Windows.Forms.Label PortLB;
    private System.Windows.Forms.TextBox PortTB;
    private System.Windows.Forms.Button InitBT;
    private System.Windows.Forms.TextBox InitTB;
    private System.Windows.Forms.Button DisconnectBT;
    private System.Windows.Forms.TextBox DisconnectTB;
    private System.Windows.Forms.Label BaudLB;
    private System.Windows.Forms.TextBox BaudTB;
    private System.Windows.Forms.RichTextBox OutDataTB;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.CheckBox HardwareCB;
  }
}

