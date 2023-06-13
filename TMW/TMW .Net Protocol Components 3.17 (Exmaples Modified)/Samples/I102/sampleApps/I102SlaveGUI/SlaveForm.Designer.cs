namespace I102slaveGUI
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
      this.BinaryInput0 = new System.Windows.Forms.CheckBox();
      this.BinaryInput1 = new System.Windows.Forms.CheckBox();
      this.BinaryInput2 = new System.Windows.Forms.CheckBox();
      this.label6 = new System.Windows.Forms.Label();
      this.Counter2 = new System.Windows.Forms.Button();
      this.Counter1 = new System.Windows.Forms.Button();
      this.Counter0 = new System.Windows.Forms.Button();
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
      this.label2.Size = new System.Drawing.Size(68, 13);
      this.label2.TabIndex = 33;
      this.label2.Text = "Binary Inputs";
      // 
      // BinaryInput0
      // 
      this.BinaryInput0.AutoSize = true;
      this.BinaryInput0.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.BinaryInput0.Location = new System.Drawing.Point(17, 64);
      this.BinaryInput0.Name = "BinaryInput0";
      this.BinaryInput0.Size = new System.Drawing.Size(47, 17);
      this.BinaryInput0.TabIndex = 30;
      this.BinaryInput0.Tag = "0";
      this.BinaryInput0.Text = "0     ";
      this.BinaryInput0.UseVisualStyleBackColor = true;
      this.BinaryInput0.CheckedChanged += new System.EventHandler(this.BinaryInput_CheckedChanged);
      // 
      // BinaryInput1
      // 
      this.BinaryInput1.AutoSize = true;
      this.BinaryInput1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.BinaryInput1.Location = new System.Drawing.Point(17, 87);
      this.BinaryInput1.Name = "BinaryInput1";
      this.BinaryInput1.Size = new System.Drawing.Size(47, 17);
      this.BinaryInput1.TabIndex = 31;
      this.BinaryInput1.Tag = "1";
      this.BinaryInput1.Text = "1     ";
      this.BinaryInput1.UseVisualStyleBackColor = true;
      this.BinaryInput1.CheckedChanged += new System.EventHandler(this.BinaryInput_CheckedChanged);
      // 
      // BinaryInput2
      // 
      this.BinaryInput2.AutoSize = true;
      this.BinaryInput2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.BinaryInput2.Location = new System.Drawing.Point(17, 111);
      this.BinaryInput2.Name = "BinaryInput2";
      this.BinaryInput2.Size = new System.Drawing.Size(47, 17);
      this.BinaryInput2.TabIndex = 32;
      this.BinaryInput2.Tag = "2";
      this.BinaryInput2.Text = "2     ";
      this.BinaryInput2.UseVisualStyleBackColor = true;
      this.BinaryInput2.CheckedChanged += new System.EventHandler(this.BinaryInput_CheckedChanged);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(270, 40);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(87, 13);
      this.label6.TabIndex = 41;
      this.label6.Text = "Integrated Totals";
      // 
      // Counter2
      // 
      this.Counter2.Location = new System.Drawing.Point(273, 107);
      this.Counter2.Name = "Counter2";
      this.Counter2.Size = new System.Drawing.Size(75, 23);
      this.Counter2.TabIndex = 40;
      this.Counter2.Tag = "2";
      this.Counter2.Text = "0";
      this.Counter2.UseVisualStyleBackColor = true;
      this.Counter2.Click += new System.EventHandler(this.Counter_Click);
      // 
      // Counter1
      // 
      this.Counter1.Location = new System.Drawing.Point(273, 83);
      this.Counter1.Name = "Counter1";
      this.Counter1.Size = new System.Drawing.Size(75, 23);
      this.Counter1.TabIndex = 39;
      this.Counter1.Tag = "1";
      this.Counter1.Text = "0";
      this.Counter1.UseVisualStyleBackColor = true;
      this.Counter1.Click += new System.EventHandler(this.Counter_Click);
      // 
      // Counter0
      // 
      this.Counter0.Location = new System.Drawing.Point(273, 60);
      this.Counter0.Name = "Counter0";
      this.Counter0.Size = new System.Drawing.Size(75, 23);
      this.Counter0.TabIndex = 38;
      this.Counter0.Tag = "0";
      this.Counter0.Text = "0";
      this.Counter0.UseVisualStyleBackColor = true;
      this.Counter0.Click += new System.EventHandler(this.Counter_Click);
      // 
      // Online_LB
      // 
      this.Online_LB.AutoSize = true;
      this.Online_LB.ForeColor = System.Drawing.Color.Crimson;
      this.Online_LB.Location = new System.Drawing.Point(445, 65);
      this.Online_LB.Name = "Online_LB";
      this.Online_LB.Size = new System.Drawing.Size(37, 13);
      this.Online_LB.TabIndex = 100;
      this.Online_LB.Text = "Offline";
      // 
      // Connected_LB
      // 
      this.Connected_LB.AutoSize = true;
      this.Connected_LB.ForeColor = System.Drawing.Color.Crimson;
      this.Connected_LB.Location = new System.Drawing.Point(445, 40);
      this.Connected_LB.Name = "Connected_LB";
      this.Connected_LB.Size = new System.Drawing.Size(73, 13);
      this.Connected_LB.TabIndex = 99;
      this.Connected_LB.Text = "Disconnected";
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(443, 105);
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
      this.Controls.Add(this.label6);
      this.Controls.Add(this.Counter2);
      this.Controls.Add(this.Counter1);
      this.Controls.Add(this.Counter0);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.BinaryInput2);
      this.Controls.Add(this.BinaryInput1);
      this.Controls.Add(this.BinaryInput0);
      this.Controls.Add(this.protocolAnalyzer);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "SlaveForm";
      this.Text = "TMW .NET Protocol Component I60870-5-102 Slave Example";
      this.Load += new System.EventHandler(this.SlaveForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.RichTextBox protocolAnalyzer;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.CheckBox BinaryInput0;
    private System.Windows.Forms.CheckBox BinaryInput1;
    private System.Windows.Forms.CheckBox BinaryInput2;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Button Counter2;
    private System.Windows.Forms.Button Counter1;
    private System.Windows.Forms.Button Counter0;
    private System.Windows.Forms.Label Online_LB;
    private System.Windows.Forms.Label Connected_LB;
    private System.Windows.Forms.Button button1;
  }
}

