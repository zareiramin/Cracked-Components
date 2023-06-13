namespace I102masterGUI
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterForm));
      this.protocolAnalyzer = new System.Windows.Forms.RichTextBox();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.label6 = new System.Windows.Forms.Label();
      this.Counter2 = new System.Windows.Forms.Label();
      this.Counter1 = new System.Windows.Forms.Label();
      this.Counter0 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.BinaryInput2 = new System.Windows.Forms.CheckBox();
      this.BinaryInput1 = new System.Windows.Forms.CheckBox();
      this.BinaryInput0 = new System.Windows.Forms.CheckBox();
      this.readSPs = new System.Windows.Forms.Button();
      this.readITs = new System.Windows.Forms.Button();
      this.Online_LB = new System.Windows.Forms.Label();
      this.Connected_LB = new System.Windows.Forms.Label();
      this.button1 = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // protocolAnalyzer
      // 
      this.protocolAnalyzer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.protocolAnalyzer.BackColor = System.Drawing.Color.Gainsboro;
      this.protocolAnalyzer.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.protocolAnalyzer.Location = new System.Drawing.Point(12, 175);
      this.protocolAnalyzer.Name = "protocolAnalyzer";
      this.protocolAnalyzer.ReadOnly = true;
      this.protocolAnalyzer.Size = new System.Drawing.Size(661, 246);
      this.protocolAnalyzer.TabIndex = 40;
      this.protocolAnalyzer.Text = "";
      this.protocolAnalyzer.WordWrap = false;
      this.protocolAnalyzer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.protocolAnalyzer_MouseDown);
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(302, 79);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(369, 87);
      this.pictureBox1.TabIndex = 73;
      this.pictureBox1.TabStop = false;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(151, 43);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(129, 13);
      this.label6.TabIndex = 88;
      this.label6.Text = "Integrated Totals (MITTA)";
      // 
      // Counter2
      // 
      this.Counter2.Location = new System.Drawing.Point(169, 134);
      this.Counter2.Name = "Counter2";
      this.Counter2.Size = new System.Drawing.Size(75, 23);
      this.Counter2.TabIndex = 87;
      this.Counter2.Tag = "2";
      this.Counter2.Text = "---";
      // 
      // Counter1
      // 
      this.Counter1.Location = new System.Drawing.Point(169, 110);
      this.Counter1.Name = "Counter1";
      this.Counter1.Size = new System.Drawing.Size(75, 23);
      this.Counter1.TabIndex = 86;
      this.Counter1.Tag = "1";
      this.Counter1.Text = "---";
      // 
      // Counter0
      // 
      this.Counter0.Location = new System.Drawing.Point(169, 87);
      this.Counter0.Name = "Counter0";
      this.Counter0.Size = new System.Drawing.Size(75, 23);
      this.Counter0.TabIndex = 85;
      this.Counter0.Tag = "0";
      this.Counter0.Text = "---";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 43);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(114, 13);
      this.label2.TabIndex = 80;
      this.label2.Text = "Binary Inputs (MSPTA)";
      // 
      // BinaryInput2
      // 
      this.BinaryInput2.AutoSize = true;
      this.BinaryInput2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.BinaryInput2.Enabled = false;
      this.BinaryInput2.Location = new System.Drawing.Point(15, 134);
      this.BinaryInput2.Name = "BinaryInput2";
      this.BinaryInput2.Size = new System.Drawing.Size(47, 17);
      this.BinaryInput2.TabIndex = 79;
      this.BinaryInput2.Tag = "2";
      this.BinaryInput2.Text = "2     ";
      this.BinaryInput2.UseVisualStyleBackColor = true;
      // 
      // BinaryInput1
      // 
      this.BinaryInput1.AutoSize = true;
      this.BinaryInput1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.BinaryInput1.Enabled = false;
      this.BinaryInput1.Location = new System.Drawing.Point(15, 110);
      this.BinaryInput1.Name = "BinaryInput1";
      this.BinaryInput1.Size = new System.Drawing.Size(47, 17);
      this.BinaryInput1.TabIndex = 78;
      this.BinaryInput1.Tag = "1";
      this.BinaryInput1.Text = "1     ";
      this.BinaryInput1.UseVisualStyleBackColor = true;
      // 
      // BinaryInput0
      // 
      this.BinaryInput0.AutoSize = true;
      this.BinaryInput0.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.BinaryInput0.Enabled = false;
      this.BinaryInput0.Location = new System.Drawing.Point(15, 87);
      this.BinaryInput0.Name = "BinaryInput0";
      this.BinaryInput0.Size = new System.Drawing.Size(47, 17);
      this.BinaryInput0.TabIndex = 77;
      this.BinaryInput0.Tag = "0";
      this.BinaryInput0.Text = "0     ";
      this.BinaryInput0.UseVisualStyleBackColor = true;
      // 
      // readSPs
      // 
      this.readSPs.Location = new System.Drawing.Point(12, 12);
      this.readSPs.Name = "readSPs";
      this.readSPs.Size = new System.Drawing.Size(114, 23);
      this.readSPs.TabIndex = 92;
      this.readSPs.Text = "Read Single Points";
      this.readSPs.UseVisualStyleBackColor = true;
      this.readSPs.Click += new System.EventHandler(this.readSinglePointsPB_Click);
      // 
      // readITs
      // 
      this.readITs.Location = new System.Drawing.Point(149, 12);
      this.readITs.Name = "readITs";
      this.readITs.Size = new System.Drawing.Size(134, 23);
      this.readITs.TabIndex = 93;
      this.readITs.Text = "Read Integrated Totals";
      this.readITs.UseVisualStyleBackColor = true;
      this.readITs.Click += new System.EventHandler(this.readITs_Click);
      // 
      // Online_LB
      // 
      this.Online_LB.AutoSize = true;
      this.Online_LB.ForeColor = System.Drawing.Color.Crimson;
      this.Online_LB.Location = new System.Drawing.Point(583, 47);
      this.Online_LB.Name = "Online_LB";
      this.Online_LB.Size = new System.Drawing.Size(37, 13);
      this.Online_LB.TabIndex = 100;
      this.Online_LB.Text = "Offline";
      // 
      // Connected_LB
      // 
      this.Connected_LB.AutoSize = true;
      this.Connected_LB.ForeColor = System.Drawing.Color.Crimson;
      this.Connected_LB.Location = new System.Drawing.Point(583, 22);
      this.Connected_LB.Name = "Connected_LB";
      this.Connected_LB.Size = new System.Drawing.Size(73, 13);
      this.Connected_LB.TabIndex = 99;
      this.Connected_LB.Text = "Disconnected";
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(491, 17);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 103;
      this.button1.Text = "Save Log";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.SaveLog_Click);
      // 
      // MasterForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(685, 433);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.Online_LB);
      this.Controls.Add(this.Connected_LB);
      this.Controls.Add(this.readITs);
      this.Controls.Add(this.readSPs);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.Counter2);
      this.Controls.Add(this.Counter1);
      this.Controls.Add(this.Counter0);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.BinaryInput2);
      this.Controls.Add(this.BinaryInput1);
      this.Controls.Add(this.BinaryInput0);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.protocolAnalyzer);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "MasterForm";
      this.Text = "TMW .NET Protocol Component I60870-5-102 Master Example";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.RichTextBox protocolAnalyzer;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label Counter2;
    private System.Windows.Forms.Label Counter1;
    private System.Windows.Forms.Label Counter0;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.CheckBox BinaryInput2;
    private System.Windows.Forms.CheckBox BinaryInput1;
    private System.Windows.Forms.CheckBox BinaryInput0;
    private System.Windows.Forms.Button readSPs;
    private System.Windows.Forms.Button readITs;
    private System.Windows.Forms.Label Online_LB;
    private System.Windows.Forms.Label Connected_LB;
    private System.Windows.Forms.Button button1;
  }
}

