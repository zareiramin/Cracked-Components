namespace I103masterGUI
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
      this.msrnd3 = new System.Windows.Forms.Label();
      this.msrnd2 = new System.Windows.Forms.Label();
      this.msrnd1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.readSPs = new System.Windows.Forms.Button();
      this.dpi3 = new System.Windows.Forms.Label();
      this.dpi2 = new System.Windows.Forms.Label();
      this.dpi1 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
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
      this.label6.Size = new System.Drawing.Size(114, 13);
      this.label6.TabIndex = 88;
      this.label6.Text = "Measurands (MSRND)";
      // 
      // msrnd3
      // 
      this.msrnd3.Location = new System.Drawing.Point(169, 134);
      this.msrnd3.Name = "msrnd3";
      this.msrnd3.Size = new System.Drawing.Size(75, 23);
      this.msrnd3.TabIndex = 87;
      this.msrnd3.Tag = "2";
      this.msrnd3.Text = "---";
      // 
      // msrnd2
      // 
      this.msrnd2.Location = new System.Drawing.Point(169, 110);
      this.msrnd2.Name = "msrnd2";
      this.msrnd2.Size = new System.Drawing.Size(75, 23);
      this.msrnd2.TabIndex = 86;
      this.msrnd2.Tag = "1";
      this.msrnd2.Text = "---";
      // 
      // msrnd1
      // 
      this.msrnd1.Location = new System.Drawing.Point(169, 87);
      this.msrnd1.Name = "msrnd1";
      this.msrnd1.Size = new System.Drawing.Size(75, 23);
      this.msrnd1.TabIndex = 85;
      this.msrnd1.Tag = "0";
      this.msrnd1.Text = "---";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 43);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(100, 13);
      this.label2.TabIndex = 80;
      this.label2.Text = "Double Points (DPI)";
      // 
      // readSPs
      // 
      this.readSPs.Location = new System.Drawing.Point(12, 12);
      this.readSPs.Name = "readSPs";
      this.readSPs.Size = new System.Drawing.Size(114, 23);
      this.readSPs.TabIndex = 92;
      this.readSPs.Text = "Integrity Poll";
      this.readSPs.UseVisualStyleBackColor = true;
      this.readSPs.Click += new System.EventHandler(this.interogationPB_Click);
      // 
      // dpi3
      // 
      this.dpi3.Location = new System.Drawing.Point(37, 134);
      this.dpi3.Name = "dpi3";
      this.dpi3.Size = new System.Drawing.Size(75, 23);
      this.dpi3.TabIndex = 95;
      this.dpi3.Tag = "2";
      this.dpi3.Text = "---";
      // 
      // dpi2
      // 
      this.dpi2.Location = new System.Drawing.Point(37, 110);
      this.dpi2.Name = "dpi2";
      this.dpi2.Size = new System.Drawing.Size(75, 23);
      this.dpi2.TabIndex = 94;
      this.dpi2.Tag = "1";
      this.dpi2.Text = "---";
      // 
      // dpi1
      // 
      this.dpi1.Location = new System.Drawing.Point(37, 87);
      this.dpi1.Name = "dpi1";
      this.dpi1.Size = new System.Drawing.Size(75, 23);
      this.dpi1.TabIndex = 93;
      this.dpi1.Tag = "0";
      this.dpi1.Text = "---";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(12, 87);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(13, 13);
      this.label5.TabIndex = 96;
      this.label5.Text = "1";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(13, 110);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(13, 13);
      this.label7.TabIndex = 97;
      this.label7.Text = "2";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(12, 134);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(13, 13);
      this.label8.TabIndex = 98;
      this.label8.Text = "3";
      // 
      // Online_LB
      // 
      this.Online_LB.AutoSize = true;
      this.Online_LB.ForeColor = System.Drawing.Color.Crimson;
      this.Online_LB.Location = new System.Drawing.Point(571, 43);
      this.Online_LB.Name = "Online_LB";
      this.Online_LB.Size = new System.Drawing.Size(37, 13);
      this.Online_LB.TabIndex = 100;
      this.Online_LB.Text = "Offline";
      // 
      // Connected_LB
      // 
      this.Connected_LB.AutoSize = true;
      this.Connected_LB.ForeColor = System.Drawing.Color.Crimson;
      this.Connected_LB.Location = new System.Drawing.Point(571, 20);
      this.Connected_LB.Name = "Connected_LB";
      this.Connected_LB.Size = new System.Drawing.Size(73, 13);
      this.Connected_LB.TabIndex = 99;
      this.Connected_LB.Text = "Disconnected";
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(468, 15);
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
      this.Controls.Add(this.label8);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.dpi3);
      this.Controls.Add(this.dpi2);
      this.Controls.Add(this.dpi1);
      this.Controls.Add(this.readSPs);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.msrnd3);
      this.Controls.Add(this.msrnd2);
      this.Controls.Add(this.msrnd1);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.protocolAnalyzer);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "MasterForm";
      this.Text = "TMW .NET Protocol Component I60870-5-103 Master Example";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.RichTextBox protocolAnalyzer;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label msrnd3;
    private System.Windows.Forms.Label msrnd2;
    private System.Windows.Forms.Label msrnd1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button readSPs;
    private System.Windows.Forms.Label dpi3;
    private System.Windows.Forms.Label dpi2;
    private System.Windows.Forms.Label dpi1;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label Online_LB;
    private System.Windows.Forms.Label Connected_LB;
    private System.Windows.Forms.Button button1;
  }
}

