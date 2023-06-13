namespace DNPmasterFileTransfer
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
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.label16 = new System.Windows.Forms.Label();
      this.PasswordTB = new System.Windows.Forms.TextBox();
      this.label15 = new System.Windows.Forms.Label();
      this.UserNameTB = new System.Windows.Forms.TextBox();
      this.WriteBt = new System.Windows.Forms.Button();
      this.ReadBt = new System.Windows.Forms.Button();
      this.BrowseRemoteBT = new System.Windows.Forms.Button();
      this.BrowseLocalBT = new System.Windows.Forms.Button();
      this.RemoteFileNameTB = new System.Windows.Forms.TextBox();
      this.label14 = new System.Windows.Forms.Label();
      this.label13 = new System.Windows.Forms.Label();
      this.LocalFileNameTB = new System.Windows.Forms.TextBox();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.FileProgress = new System.Windows.Forms.ProgressBar();
      ((System.ComponentModel.ISupportInitialize)(this.IntegrityIntervalHr)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.IntegrityIntervalMin)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.IntegrityIntervalSec)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.EventIntervalSec)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.EventIntervalMin)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.EventIntervalHr)).BeginInit();
      this.Poll.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.groupBox3.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // Integrity
      // 
      this.Integrity.Location = new System.Drawing.Point(6, 30);
      this.Integrity.Name = "Integrity";
      this.Integrity.Size = new System.Drawing.Size(75, 23);
      this.Integrity.TabIndex = 57;
      this.Integrity.Text = "Integrity";
      this.Integrity.UseVisualStyleBackColor = true;
      this.Integrity.Click += new System.EventHandler(this.Integrity_Click);
      // 
      // Event
      // 
      this.Event.Location = new System.Drawing.Point(6, 68);
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
      this.IntegrityEnable.Location = new System.Drawing.Point(96, 33);
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
      this.EventEnable.Location = new System.Drawing.Point(96, 71);
      this.EventEnable.Name = "EventEnable";
      this.EventEnable.Size = new System.Drawing.Size(90, 17);
      this.EventEnable.TabIndex = 60;
      this.EventEnable.Text = "Repeat every";
      this.EventEnable.UseVisualStyleBackColor = true;
      this.EventEnable.CheckedChanged += new System.EventHandler(this.EventInterval_ValueChanged);
      // 
      // IntegrityIntervalHr
      // 
      this.IntegrityIntervalHr.Location = new System.Drawing.Point(193, 29);
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
      this.IntegrityIntervalMin.Location = new System.Drawing.Point(238, 29);
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
      this.IntegrityIntervalSec.Location = new System.Drawing.Point(283, 29);
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
      this.EventIntervalSec.Location = new System.Drawing.Point(283, 68);
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
      this.EventIntervalMin.Location = new System.Drawing.Point(238, 68);
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
      this.EventIntervalHr.Location = new System.Drawing.Point(193, 68);
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
      this.Poll.Location = new System.Drawing.Point(407, 117);
      this.Poll.Name = "Poll";
      this.Poll.Size = new System.Drawing.Size(333, 112);
      this.Poll.TabIndex = 71;
      this.Poll.TabStop = false;
      this.Poll.Text = "Poll";
      // 
      // EventProgressBar
      // 
      this.EventProgressBar.Location = new System.Drawing.Point(96, 92);
      this.EventProgressBar.Name = "EventProgressBar";
      this.EventProgressBar.Size = new System.Drawing.Size(221, 10);
      this.EventProgressBar.TabIndex = 73;
      // 
      // IntegrityProgressBar
      // 
      this.IntegrityProgressBar.Location = new System.Drawing.Point(96, 52);
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
      this.protocolAnalyzer.Location = new System.Drawing.Point(21, 338);
      this.protocolAnalyzer.Name = "protocolAnalyzer";
      this.protocolAnalyzer.ReadOnly = true;
      this.protocolAnalyzer.Size = new System.Drawing.Size(717, 386);
      this.protocolAnalyzer.TabIndex = 39;
      this.protocolAnalyzer.Text = "";
      this.protocolAnalyzer.WordWrap = false;
      this.protocolAnalyzer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.protocolAnalyzer_MouseDown);
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(164, 3);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(369, 101);
      this.pictureBox1.TabIndex = 72;
      this.pictureBox1.TabStop = false;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.label16);
      this.groupBox3.Controls.Add(this.PasswordTB);
      this.groupBox3.Controls.Add(this.label15);
      this.groupBox3.Controls.Add(this.UserNameTB);
      this.groupBox3.Controls.Add(this.WriteBt);
      this.groupBox3.Controls.Add(this.ReadBt);
      this.groupBox3.Controls.Add(this.BrowseRemoteBT);
      this.groupBox3.Controls.Add(this.BrowseLocalBT);
      this.groupBox3.Controls.Add(this.RemoteFileNameTB);
      this.groupBox3.Controls.Add(this.label14);
      this.groupBox3.Controls.Add(this.label13);
      this.groupBox3.Controls.Add(this.LocalFileNameTB);
      this.groupBox3.Location = new System.Drawing.Point(21, 117);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(369, 196);
      this.groupBox3.TabIndex = 73;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "File Transfer";
      // 
      // label16
      // 
      this.label16.AutoSize = true;
      this.label16.Location = new System.Drawing.Point(117, 99);
      this.label16.Name = "label16";
      this.label16.Size = new System.Drawing.Size(53, 13);
      this.label16.TabIndex = 11;
      this.label16.Text = "Password";
      // 
      // PasswordTB
      // 
      this.PasswordTB.Location = new System.Drawing.Point(122, 115);
      this.PasswordTB.Name = "PasswordTB";
      this.PasswordTB.Size = new System.Drawing.Size(100, 20);
      this.PasswordTB.TabIndex = 10;
      // 
      // label15
      // 
      this.label15.AutoSize = true;
      this.label15.Location = new System.Drawing.Point(3, 99);
      this.label15.Name = "label15";
      this.label15.Size = new System.Drawing.Size(60, 13);
      this.label15.TabIndex = 9;
      this.label15.Text = "User Name";
      // 
      // UserNameTB
      // 
      this.UserNameTB.Location = new System.Drawing.Point(6, 115);
      this.UserNameTB.Name = "UserNameTB";
      this.UserNameTB.Size = new System.Drawing.Size(100, 20);
      this.UserNameTB.TabIndex = 8;
      // 
      // WriteBt
      // 
      this.WriteBt.Location = new System.Drawing.Point(179, 153);
      this.WriteBt.Name = "WriteBt";
      this.WriteBt.Size = new System.Drawing.Size(75, 23);
      this.WriteBt.TabIndex = 7;
      this.WriteBt.Text = "Write";
      this.WriteBt.UseVisualStyleBackColor = true;
      this.WriteBt.Click += new System.EventHandler(this.WriteBt_Click);
      // 
      // ReadBt
      // 
      this.ReadBt.Location = new System.Drawing.Point(59, 153);
      this.ReadBt.Name = "ReadBt";
      this.ReadBt.Size = new System.Drawing.Size(75, 23);
      this.ReadBt.TabIndex = 6;
      this.ReadBt.Text = "Read";
      this.ReadBt.UseVisualStyleBackColor = true;
      this.ReadBt.Click += new System.EventHandler(this.ReadBt_Click);
      // 
      // BrowseRemoteBT
      // 
      this.BrowseRemoteBT.Location = new System.Drawing.Point(288, 68);
      this.BrowseRemoteBT.Name = "BrowseRemoteBT";
      this.BrowseRemoteBT.Size = new System.Drawing.Size(75, 23);
      this.BrowseRemoteBT.TabIndex = 5;
      this.BrowseRemoteBT.Text = "Browse";
      this.BrowseRemoteBT.UseVisualStyleBackColor = true;
      this.BrowseRemoteBT.Click += new System.EventHandler(this.BrowseRemoteBT_Click);
      // 
      // BrowseLocalBT
      // 
      this.BrowseLocalBT.Location = new System.Drawing.Point(288, 29);
      this.BrowseLocalBT.Name = "BrowseLocalBT";
      this.BrowseLocalBT.Size = new System.Drawing.Size(75, 23);
      this.BrowseLocalBT.TabIndex = 4;
      this.BrowseLocalBT.Text = "Browse";
      this.BrowseLocalBT.UseVisualStyleBackColor = true;
      this.BrowseLocalBT.Click += new System.EventHandler(this.BrowseLocalBT_Click);
      // 
      // RemoteFileNameTB
      // 
      this.RemoteFileNameTB.Location = new System.Drawing.Point(6, 71);
      this.RemoteFileNameTB.Name = "RemoteFileNameTB";
      this.RemoteFileNameTB.Size = new System.Drawing.Size(273, 20);
      this.RemoteFileNameTB.TabIndex = 3;
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(0, 55);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(94, 13);
      this.label14.TabIndex = 2;
      this.label14.Text = "Remote File Name";
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(0, 16);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(83, 13);
      this.label13.TabIndex = 1;
      this.label13.Text = "Local File Name";
      // 
      // LocalFileNameTB
      // 
      this.LocalFileNameTB.Location = new System.Drawing.Point(6, 32);
      this.LocalFileNameTB.Name = "LocalFileNameTB";
      this.LocalFileNameTB.Size = new System.Drawing.Size(273, 20);
      this.LocalFileNameTB.TabIndex = 0;
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.AddExtension = false;
      this.openFileDialog1.CheckFileExists = false;
      this.openFileDialog1.InitialDirectory = "c:\\temp";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.FileProgress);
      this.groupBox1.Location = new System.Drawing.Point(407, 236);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(333, 77);
      this.groupBox1.TabIndex = 74;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "File Transfer Progress";
      // 
      // FileProgress
      // 
      this.FileProgress.Location = new System.Drawing.Point(32, 34);
      this.FileProgress.Name = "FileProgress";
      this.FileProgress.Size = new System.Drawing.Size(251, 23);
      this.FileProgress.TabIndex = 0;
      // 
      // MasterForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(752, 736);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.Poll);
      this.Controls.Add(this.protocolAnalyzer);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "MasterForm";
      this.Text = "TMW .NET Protocol Component DNP Master File Transfer Example";
      ((System.ComponentModel.ISupportInitialize)(this.IntegrityIntervalHr)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.IntegrityIntervalMin)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.IntegrityIntervalSec)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.EventIntervalSec)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.EventIntervalMin)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.EventIntervalHr)).EndInit();
      this.Poll.ResumeLayout(false);
      this.Poll.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox1.ResumeLayout(false);
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
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.TextBox RemoteFileNameTB;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.TextBox LocalFileNameTB;
    private System.Windows.Forms.Button WriteBt;
    private System.Windows.Forms.Button ReadBt;
    private System.Windows.Forms.Button BrowseRemoteBT;
    private System.Windows.Forms.Button BrowseLocalBT;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.TextBox PasswordTB;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.TextBox UserNameTB;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.ProgressBar FileProgress;
  }
}

