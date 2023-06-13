namespace I103slaveGUI
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
      this.protocolAnalyzer = new System.Windows.Forms.RichTextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.msrnd1 = new System.Windows.Forms.TextBox();
      this.msrnd2 = new System.Windows.Forms.TextBox();
      this.msrnd3 = new System.Windows.Forms.TextBox();
      this.dpi1 = new System.Windows.Forms.CheckBox();
      this.dpi2 = new System.Windows.Forms.CheckBox();
      this.dpi3 = new System.Windows.Forms.CheckBox();
      this.Online_LB = new System.Windows.Forms.Label();
      this.Connected_LB = new System.Windows.Forms.Label();
      this.button1 = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // protocolAnalyzer
      // 
      this.protocolAnalyzer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.protocolAnalyzer.BackColor = System.Drawing.Color.Gainsboro;
      this.protocolAnalyzer.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.protocolAnalyzer.Location = new System.Drawing.Point(12, 148);
      this.protocolAnalyzer.Name = "protocolAnalyzer";
      this.protocolAnalyzer.ReadOnly = true;
      this.protocolAnalyzer.Size = new System.Drawing.Size(506, 175);
      this.protocolAnalyzer.TabIndex = 2;
      this.protocolAnalyzer.Text = "";
      this.protocolAnalyzer.WordWrap = false;
      this.protocolAnalyzer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.protocolAnalyzer_MouseDown);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(14, 40);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(73, 13);
      this.label2.TabIndex = 33;
      this.label2.Text = "Double Points";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(270, 40);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(60, 13);
      this.label6.TabIndex = 41;
      this.label6.Text = "Measurand";
      // 
      // msrnd1
      // 
      this.msrnd1.Location = new System.Drawing.Point(255, 62);
      this.msrnd1.Name = "msrnd1";
      this.msrnd1.Size = new System.Drawing.Size(100, 20);
      this.msrnd1.TabIndex = 42;
      this.msrnd1.TextChanged += new System.EventHandler(this.MSRND_TextChanged);
      // 
      // msrnd2
      // 
      this.msrnd2.Location = new System.Drawing.Point(255, 85);
      this.msrnd2.Name = "msrnd2";
      this.msrnd2.Size = new System.Drawing.Size(100, 20);
      this.msrnd2.TabIndex = 43;
      this.msrnd2.TextChanged += new System.EventHandler(this.MSRND_TextChanged);
      // 
      // msrnd3
      // 
      this.msrnd3.Location = new System.Drawing.Point(255, 107);
      this.msrnd3.Name = "msrnd3";
      this.msrnd3.Size = new System.Drawing.Size(100, 20);
      this.msrnd3.TabIndex = 44;
      this.msrnd3.TextChanged += new System.EventHandler(this.MSRND_TextChanged);
      // 
      // dpi1
      // 
      this.dpi1.AutoSize = true;
      this.dpi1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.dpi1.Location = new System.Drawing.Point(34, 65);
      this.dpi1.Name = "dpi1";
      this.dpi1.Size = new System.Drawing.Size(32, 17);
      this.dpi1.TabIndex = 48;
      this.dpi1.Text = "1";
      this.dpi1.UseVisualStyleBackColor = true;
      this.dpi1.CheckedChanged += new System.EventHandler(this.DPI_CheckChanged);
      // 
      // dpi2
      // 
      this.dpi2.AutoSize = true;
      this.dpi2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.dpi2.Location = new System.Drawing.Point(34, 89);
      this.dpi2.Name = "dpi2";
      this.dpi2.Size = new System.Drawing.Size(32, 17);
      this.dpi2.TabIndex = 49;
      this.dpi2.Text = "2";
      this.dpi2.UseVisualStyleBackColor = true;
      this.dpi2.CheckedChanged += new System.EventHandler(this.DPI_CheckChanged);
      // 
      // dpi3
      // 
      this.dpi3.AutoSize = true;
      this.dpi3.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.dpi3.Location = new System.Drawing.Point(34, 112);
      this.dpi3.Name = "dpi3";
      this.dpi3.Size = new System.Drawing.Size(32, 17);
      this.dpi3.TabIndex = 50;
      this.dpi3.Text = "3";
      this.dpi3.UseVisualStyleBackColor = true;
      this.dpi3.CheckedChanged += new System.EventHandler(this.DPI_CheckChanged);
      // 
      // Online_LB
      // 
      this.Online_LB.AutoSize = true;
      this.Online_LB.ForeColor = System.Drawing.Color.Crimson;
      this.Online_LB.Location = new System.Drawing.Point(431, 87);
      this.Online_LB.Name = "Online_LB";
      this.Online_LB.Size = new System.Drawing.Size(37, 13);
      this.Online_LB.TabIndex = 100;
      this.Online_LB.Text = "Offline";
      // 
      // Connected_LB
      // 
      this.Connected_LB.AutoSize = true;
      this.Connected_LB.ForeColor = System.Drawing.Color.Crimson;
      this.Connected_LB.Location = new System.Drawing.Point(431, 62);
      this.Connected_LB.Name = "Connected_LB";
      this.Connected_LB.Size = new System.Drawing.Size(73, 13);
      this.Connected_LB.TabIndex = 99;
      this.Connected_LB.Text = "Disconnected";
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(429, 108);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 103;
      this.button1.Text = "Save Log";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.SaveLog_Click);
      // 
      // SlaveForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(530, 335);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.Online_LB);
      this.Controls.Add(this.Connected_LB);
      this.Controls.Add(this.dpi3);
      this.Controls.Add(this.dpi2);
      this.Controls.Add(this.dpi1);
      this.Controls.Add(this.msrnd3);
      this.Controls.Add(this.msrnd2);
      this.Controls.Add(this.msrnd1);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.protocolAnalyzer);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "SlaveForm";
      this.Text = "TMW .NET Protocol Component I60870-5-103 Slave Example";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.RichTextBox protocolAnalyzer;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox msrnd1;
    private System.Windows.Forms.TextBox msrnd2;
    private System.Windows.Forms.TextBox msrnd3;
    private System.Windows.Forms.CheckBox dpi1;
    private System.Windows.Forms.CheckBox dpi2;
    private System.Windows.Forms.CheckBox dpi3;
    private System.Windows.Forms.Label Online_LB;
    private System.Windows.Forms.Label Connected_LB;
    private System.Windows.Forms.Button button1;
  }
}

