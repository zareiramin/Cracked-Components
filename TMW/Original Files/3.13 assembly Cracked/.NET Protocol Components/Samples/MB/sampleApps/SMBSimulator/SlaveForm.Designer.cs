namespace SMBSimulator
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
      this.disc0 = new System.Windows.Forms.CheckBox();
      this.disc1 = new System.Windows.Forms.CheckBox();
      this.disc2 = new System.Windows.Forms.CheckBox();
      this.coil0 = new System.Windows.Forms.Label();
      this.coil1 = new System.Windows.Forms.Label();
      this.coil2 = new System.Windows.Forms.Label();
      this.coil3 = new System.Windows.Forms.Label();
      this.coil4 = new System.Windows.Forms.Label();
      this.coil5 = new System.Windows.Forms.Label();
      this.coil6 = new System.Windows.Forms.Label();
      this.coil7 = new System.Windows.Forms.Label();
      this.coil8 = new System.Windows.Forms.Label();
      this.coil9 = new System.Windows.Forms.Label();
      this.ireg0Value = new System.Windows.Forms.Label();
      this.ireg1Value = new System.Windows.Forms.Label();
      this.ireg2Value = new System.Windows.Forms.Label();
      this.ireg0 = new System.Windows.Forms.TrackBar();
      this.ireg1 = new System.Windows.Forms.TrackBar();
      this.ireg2 = new System.Windows.Forms.TrackBar();
      this.hreg0 = new System.Windows.Forms.Label();
      this.hreg1 = new System.Windows.Forms.Label();
      this.hreg2 = new System.Windows.Forms.Label();
      this.hreg3 = new System.Windows.Forms.Label();
      this.hreg4 = new System.Windows.Forms.Label();
      this.hreg5 = new System.Windows.Forms.Label();
      this.hreg6 = new System.Windows.Forms.Label();
      this.hreg7 = new System.Windows.Forms.Label();
      this.hreg8 = new System.Windows.Forms.Label();
      this.hreg9 = new System.Windows.Forms.Label();
      this.closePB = new System.Windows.Forms.Button();
      this.protocolAnalyzerTextCtrl = new System.Windows.Forms.RichTextBox();
      ((System.ComponentModel.ISupportInitialize)(this.ireg0)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ireg1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ireg2)).BeginInit();
      this.SuspendLayout();
      // 
      // disc0
      // 
      this.disc0.AutoSize = true;
      this.disc0.Location = new System.Drawing.Point(34, 27);
      this.disc0.Name = "disc0";
      this.disc0.Size = new System.Drawing.Size(101, 17);
      this.disc0.TabIndex = 0;
      this.disc0.Text = "Discrete Input 0";
      this.disc0.UseVisualStyleBackColor = true;
      this.disc0.CheckedChanged += new System.EventHandler(this.disc_CheckedChanged);
      // 
      // disc1
      // 
      this.disc1.AutoSize = true;
      this.disc1.Location = new System.Drawing.Point(34, 51);
      this.disc1.Name = "disc1";
      this.disc1.Size = new System.Drawing.Size(101, 17);
      this.disc1.TabIndex = 1;
      this.disc1.Text = "Discrete Input 1";
      this.disc1.UseVisualStyleBackColor = true;
      this.disc1.CheckedChanged += new System.EventHandler(this.disc_CheckedChanged);
      // 
      // disc2
      // 
      this.disc2.AutoSize = true;
      this.disc2.Location = new System.Drawing.Point(34, 75);
      this.disc2.Name = "disc2";
      this.disc2.Size = new System.Drawing.Size(101, 17);
      this.disc2.TabIndex = 2;
      this.disc2.Text = "Discrete Input 2";
      this.disc2.UseVisualStyleBackColor = true;
      this.disc2.CheckedChanged += new System.EventHandler(this.disc_CheckedChanged);
      // 
      // coil0
      // 
      this.coil0.AutoSize = true;
      this.coil0.Location = new System.Drawing.Point(147, 27);
      this.coil0.Name = "coil0";
      this.coil0.Size = new System.Drawing.Size(63, 13);
      this.coil0.TabIndex = 3;
      this.coil0.Text = "Coil 0: ????";
      // 
      // coil1
      // 
      this.coil1.AutoSize = true;
      this.coil1.Location = new System.Drawing.Point(147, 55);
      this.coil1.Name = "coil1";
      this.coil1.Size = new System.Drawing.Size(63, 13);
      this.coil1.TabIndex = 4;
      this.coil1.Text = "Coil 1: ????";
      // 
      // coil2
      // 
      this.coil2.AutoSize = true;
      this.coil2.Location = new System.Drawing.Point(147, 83);
      this.coil2.Name = "coil2";
      this.coil2.Size = new System.Drawing.Size(63, 13);
      this.coil2.TabIndex = 4;
      this.coil2.Text = "Coil 2: ????";
      // 
      // coil3
      // 
      this.coil3.AutoSize = true;
      this.coil3.Location = new System.Drawing.Point(147, 111);
      this.coil3.Name = "coil3";
      this.coil3.Size = new System.Drawing.Size(63, 13);
      this.coil3.TabIndex = 4;
      this.coil3.Text = "Coil 3: ????";
      // 
      // coil4
      // 
      this.coil4.AutoSize = true;
      this.coil4.Location = new System.Drawing.Point(147, 139);
      this.coil4.Name = "coil4";
      this.coil4.Size = new System.Drawing.Size(63, 13);
      this.coil4.TabIndex = 4;
      this.coil4.Text = "Coil 4: ????";
      // 
      // coil5
      // 
      this.coil5.AutoSize = true;
      this.coil5.Location = new System.Drawing.Point(147, 167);
      this.coil5.Name = "coil5";
      this.coil5.Size = new System.Drawing.Size(63, 13);
      this.coil5.TabIndex = 4;
      this.coil5.Text = "Coil 5: ????";
      // 
      // coil6
      // 
      this.coil6.AutoSize = true;
      this.coil6.Location = new System.Drawing.Point(147, 195);
      this.coil6.Name = "coil6";
      this.coil6.Size = new System.Drawing.Size(63, 13);
      this.coil6.TabIndex = 4;
      this.coil6.Text = "Coil 6: ????";
      // 
      // coil7
      // 
      this.coil7.AutoSize = true;
      this.coil7.Location = new System.Drawing.Point(147, 223);
      this.coil7.Name = "coil7";
      this.coil7.Size = new System.Drawing.Size(63, 13);
      this.coil7.TabIndex = 4;
      this.coil7.Text = "Coil 7: ????";
      // 
      // coil8
      // 
      this.coil8.AutoSize = true;
      this.coil8.Location = new System.Drawing.Point(147, 251);
      this.coil8.Name = "coil8";
      this.coil8.Size = new System.Drawing.Size(63, 13);
      this.coil8.TabIndex = 4;
      this.coil8.Text = "Coil 8: ????";
      // 
      // coil9
      // 
      this.coil9.AutoSize = true;
      this.coil9.Location = new System.Drawing.Point(147, 279);
      this.coil9.Name = "coil9";
      this.coil9.Size = new System.Drawing.Size(63, 13);
      this.coil9.TabIndex = 4;
      this.coil9.Text = "Coil 9: ????";
      // 
      // ireg0Value
      // 
      this.ireg0Value.AutoSize = true;
      this.ireg0Value.Location = new System.Drawing.Point(226, 27);
      this.ireg0Value.Name = "ireg0Value";
      this.ireg0Value.Size = new System.Drawing.Size(112, 13);
      this.ireg0Value.TabIndex = 5;
      this.ireg0Value.Text = "Input Register 0: ????";
      // 
      // ireg1Value
      // 
      this.ireg1Value.AutoSize = true;
      this.ireg1Value.Location = new System.Drawing.Point(229, 92);
      this.ireg1Value.Name = "ireg1Value";
      this.ireg1Value.Size = new System.Drawing.Size(112, 13);
      this.ireg1Value.TabIndex = 6;
      this.ireg1Value.Text = "Input Register 1: ????";
      // 
      // ireg2Value
      // 
      this.ireg2Value.AutoSize = true;
      this.ireg2Value.Location = new System.Drawing.Point(229, 156);
      this.ireg2Value.Name = "ireg2Value";
      this.ireg2Value.Size = new System.Drawing.Size(112, 13);
      this.ireg2Value.TabIndex = 7;
      this.ireg2Value.Text = "Input Register 2: ????";
      // 
      // ireg0
      // 
      this.ireg0.Location = new System.Drawing.Point(229, 44);
      this.ireg0.Maximum = 100;
      this.ireg0.Name = "ireg0";
      this.ireg0.Size = new System.Drawing.Size(104, 45);
      this.ireg0.TabIndex = 8;
      this.ireg0.TickFrequency = 10;
      this.ireg0.ValueChanged += new System.EventHandler(this.ireg_ValueChanged);
      // 
      // ireg1
      // 
      this.ireg1.Location = new System.Drawing.Point(232, 108);
      this.ireg1.Maximum = 100;
      this.ireg1.Name = "ireg1";
      this.ireg1.Size = new System.Drawing.Size(104, 45);
      this.ireg1.TabIndex = 9;
      this.ireg1.TickFrequency = 10;
      this.ireg1.ValueChanged += new System.EventHandler(this.ireg_ValueChanged);
      // 
      // ireg2
      // 
      this.ireg2.Location = new System.Drawing.Point(229, 173);
      this.ireg2.Maximum = 100;
      this.ireg2.Name = "ireg2";
      this.ireg2.Size = new System.Drawing.Size(104, 45);
      this.ireg2.TabIndex = 10;
      this.ireg2.TickFrequency = 10;
      this.ireg2.ValueChanged += new System.EventHandler(this.ireg_ValueChanged);
      // 
      // hreg0
      // 
      this.hreg0.AutoSize = true;
      this.hreg0.Location = new System.Drawing.Point(370, 27);
      this.hreg0.Name = "hreg0";
      this.hreg0.Size = new System.Drawing.Size(130, 13);
      this.hreg0.TabIndex = 11;
      this.hreg0.Text = "Holding Register 0: ?????";
      // 
      // hreg1
      // 
      this.hreg1.AutoSize = true;
      this.hreg1.Location = new System.Drawing.Point(370, 55);
      this.hreg1.Name = "hreg1";
      this.hreg1.Size = new System.Drawing.Size(130, 13);
      this.hreg1.TabIndex = 12;
      this.hreg1.Text = "Holding Register 1: ?????";
      // 
      // hreg2
      // 
      this.hreg2.AutoSize = true;
      this.hreg2.Location = new System.Drawing.Point(370, 83);
      this.hreg2.Name = "hreg2";
      this.hreg2.Size = new System.Drawing.Size(130, 13);
      this.hreg2.TabIndex = 12;
      this.hreg2.Text = "Holding Register 2: ?????";
      // 
      // hreg3
      // 
      this.hreg3.AutoSize = true;
      this.hreg3.Location = new System.Drawing.Point(370, 111);
      this.hreg3.Name = "hreg3";
      this.hreg3.Size = new System.Drawing.Size(130, 13);
      this.hreg3.TabIndex = 12;
      this.hreg3.Text = "Holding Register 3: ?????";
      // 
      // hreg4
      // 
      this.hreg4.AutoSize = true;
      this.hreg4.Location = new System.Drawing.Point(370, 139);
      this.hreg4.Name = "hreg4";
      this.hreg4.Size = new System.Drawing.Size(130, 13);
      this.hreg4.TabIndex = 12;
      this.hreg4.Text = "Holding Register 4: ?????";
      // 
      // hreg5
      // 
      this.hreg5.AutoSize = true;
      this.hreg5.Location = new System.Drawing.Point(370, 167);
      this.hreg5.Name = "hreg5";
      this.hreg5.Size = new System.Drawing.Size(130, 13);
      this.hreg5.TabIndex = 12;
      this.hreg5.Text = "Holding Register 5: ?????";
      // 
      // hreg6
      // 
      this.hreg6.AutoSize = true;
      this.hreg6.Location = new System.Drawing.Point(370, 195);
      this.hreg6.Name = "hreg6";
      this.hreg6.Size = new System.Drawing.Size(130, 13);
      this.hreg6.TabIndex = 12;
      this.hreg6.Text = "Holding Register 6: ?????";
      // 
      // hreg7
      // 
      this.hreg7.AutoSize = true;
      this.hreg7.Location = new System.Drawing.Point(370, 223);
      this.hreg7.Name = "hreg7";
      this.hreg7.Size = new System.Drawing.Size(130, 13);
      this.hreg7.TabIndex = 12;
      this.hreg7.Text = "Holding Register 7: ?????";
      // 
      // hreg8
      // 
      this.hreg8.AutoSize = true;
      this.hreg8.Location = new System.Drawing.Point(370, 251);
      this.hreg8.Name = "hreg8";
      this.hreg8.Size = new System.Drawing.Size(130, 13);
      this.hreg8.TabIndex = 12;
      this.hreg8.Text = "Holding Register 8: ?????";
      // 
      // hreg9
      // 
      this.hreg9.AutoSize = true;
      this.hreg9.Location = new System.Drawing.Point(370, 279);
      this.hreg9.Name = "hreg9";
      this.hreg9.Size = new System.Drawing.Size(130, 13);
      this.hreg9.TabIndex = 12;
      this.hreg9.Text = "Holding Register 9: ?????";
      // 
      // closePB
      // 
      this.closePB.Location = new System.Drawing.Point(12, 274);
      this.closePB.Name = "closePB";
      this.closePB.Size = new System.Drawing.Size(75, 23);
      this.closePB.TabIndex = 13;
      this.closePB.Text = "Close";
      this.closePB.UseVisualStyleBackColor = true;
      this.closePB.Click += new System.EventHandler(this.closePB_Click);
      // 
      // protocolAnalyzerTextCtrl
      // 
      this.protocolAnalyzerTextCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.protocolAnalyzerTextCtrl.BackColor = System.Drawing.Color.Gainsboro;
      this.protocolAnalyzerTextCtrl.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.protocolAnalyzerTextCtrl.Location = new System.Drawing.Point(12, 313);
      this.protocolAnalyzerTextCtrl.Name = "protocolAnalyzerTextCtrl";
      this.protocolAnalyzerTextCtrl.ReadOnly = true;
      this.protocolAnalyzerTextCtrl.Size = new System.Drawing.Size(506, 299);
      this.protocolAnalyzerTextCtrl.TabIndex = 14;
      this.protocolAnalyzerTextCtrl.Text = "";
      this.protocolAnalyzerTextCtrl.WordWrap = false;
      this.protocolAnalyzerTextCtrl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.protocolAnalyzerTextCtrl_MouseDown);
      // 
      // SlaveForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(530, 624);
      this.ControlBox = false;
      this.Controls.Add(this.protocolAnalyzerTextCtrl);
      this.Controls.Add(this.closePB);
      this.Controls.Add(this.hreg0);
      this.Controls.Add(this.hreg1);
      this.Controls.Add(this.hreg2);
      this.Controls.Add(this.hreg3);
      this.Controls.Add(this.hreg4);
      this.Controls.Add(this.hreg5);
      this.Controls.Add(this.hreg6);
      this.Controls.Add(this.hreg7);
      this.Controls.Add(this.hreg8);
      this.Controls.Add(this.hreg9);
      this.Controls.Add(this.ireg2);
      this.Controls.Add(this.ireg1);
      this.Controls.Add(this.ireg2Value);
      this.Controls.Add(this.ireg1Value);
      this.Controls.Add(this.ireg0Value);
      this.Controls.Add(this.coil0);
      this.Controls.Add(this.coil1);
      this.Controls.Add(this.coil2);
      this.Controls.Add(this.coil3);
      this.Controls.Add(this.coil4);
      this.Controls.Add(this.coil5);
      this.Controls.Add(this.coil6);
      this.Controls.Add(this.coil7);
      this.Controls.Add(this.coil8);
      this.Controls.Add(this.coil9);
      this.Controls.Add(this.disc2);
      this.Controls.Add(this.disc1);
      this.Controls.Add(this.disc0);
      this.Controls.Add(this.ireg0);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "SlaveForm";
      this.Text = "MODBUS slave simulator";
      ((System.ComponentModel.ISupportInitialize)(this.ireg0)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ireg1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ireg2)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckBox disc0;
    private System.Windows.Forms.CheckBox disc1;
    private System.Windows.Forms.CheckBox disc2;
    private System.Windows.Forms.Label coil0;
    private System.Windows.Forms.Label coil1;
    private System.Windows.Forms.Label coil2;
    private System.Windows.Forms.Label coil3;
    private System.Windows.Forms.Label coil4;
    private System.Windows.Forms.Label coil5;
    private System.Windows.Forms.Label coil6;
    private System.Windows.Forms.Label coil7;
    private System.Windows.Forms.Label coil8;
    private System.Windows.Forms.Label coil9;
    private System.Windows.Forms.Label ireg0Value;
    private System.Windows.Forms.Label ireg1Value;
    private System.Windows.Forms.Label ireg2Value;
    private System.Windows.Forms.TrackBar ireg0;
    private System.Windows.Forms.TrackBar ireg1;
    private System.Windows.Forms.TrackBar ireg2;
    private System.Windows.Forms.Label hreg0;
    private System.Windows.Forms.Label hreg1;
    private System.Windows.Forms.Label hreg2;
    private System.Windows.Forms.Label hreg3;
    private System.Windows.Forms.Label hreg4;
    private System.Windows.Forms.Label hreg5;
    private System.Windows.Forms.Label hreg6;
    private System.Windows.Forms.Label hreg7;
    private System.Windows.Forms.Label hreg8;
    private System.Windows.Forms.Label hreg9;
    private System.Windows.Forms.Button closePB;
    private System.Windows.Forms.RichTextBox protocolAnalyzerTextCtrl;
  }
}

