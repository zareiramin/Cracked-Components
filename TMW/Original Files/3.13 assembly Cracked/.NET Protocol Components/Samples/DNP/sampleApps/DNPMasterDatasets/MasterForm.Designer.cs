namespace DNPMasterDatasets
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
      this.Output_TB = new System.Windows.Forms.RichTextBox();
      this.Read_BT = new System.Windows.Forms.Button();
      this.Send_BT = new System.Windows.Forms.Button();
      this.ReadId_TB = new System.Windows.Forms.TextBox();
      this.SendId_TB = new System.Windows.Forms.TextBox();
      this.Modify_BT = new System.Windows.Forms.Button();
      this.Display_BT = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.ModifyId_TB = new System.Windows.Forms.TextBox();
      this.DisplayId_TB = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.ModifyIndex_TB = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.ModifyValue_TB = new System.Windows.Forms.TextBox();
      this.ReadTypeCB = new System.Windows.Forms.ComboBox();
      this.SendTypeCB = new System.Windows.Forms.ComboBox();
      this.Connected_LB = new System.Windows.Forms.Label();
      this.Online_LB = new System.Windows.Forms.Label();
      this.SaveLogBt = new System.Windows.Forms.Button();
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
      this.protocolAnalyzer.Location = new System.Drawing.Point(21, 303);
      this.protocolAnalyzer.Name = "protocolAnalyzer";
      this.protocolAnalyzer.ReadOnly = true;
      this.protocolAnalyzer.Size = new System.Drawing.Size(717, 340);
      this.protocolAnalyzer.TabIndex = 39;
      this.protocolAnalyzer.Text = "";
      this.protocolAnalyzer.WordWrap = false;
      this.protocolAnalyzer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.protocolAnalyzer_MouseDown);
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(169, 12);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(369, 101);
      this.pictureBox1.TabIndex = 72;
      this.pictureBox1.TabStop = false;
      // 
      // Output_TB
      // 
      this.Output_TB.Location = new System.Drawing.Point(402, 119);
      this.Output_TB.Name = "Output_TB";
      this.Output_TB.Size = new System.Drawing.Size(323, 140);
      this.Output_TB.TabIndex = 73;
      this.Output_TB.Text = "";
      // 
      // Read_BT
      // 
      this.Read_BT.Location = new System.Drawing.Point(21, 197);
      this.Read_BT.Name = "Read_BT";
      this.Read_BT.Size = new System.Drawing.Size(138, 23);
      this.Read_BT.TabIndex = 74;
      this.Read_BT.Text = "Read from Outstation";
      this.Read_BT.UseVisualStyleBackColor = true;
      this.Read_BT.Click += new System.EventHandler(this.Read_BT_Click);
      // 
      // Send_BT
      // 
      this.Send_BT.Location = new System.Drawing.Point(21, 226);
      this.Send_BT.Name = "Send_BT";
      this.Send_BT.Size = new System.Drawing.Size(138, 23);
      this.Send_BT.TabIndex = 75;
      this.Send_BT.Text = "Send to Outstation";
      this.Send_BT.UseVisualStyleBackColor = true;
      this.Send_BT.Click += new System.EventHandler(this.Send_BT_Click);
      // 
      // ReadId_TB
      // 
      this.ReadId_TB.Location = new System.Drawing.Point(181, 200);
      this.ReadId_TB.Name = "ReadId_TB";
      this.ReadId_TB.Size = new System.Drawing.Size(33, 20);
      this.ReadId_TB.TabIndex = 76;
      this.ReadId_TB.Text = "0";
      // 
      // SendId_TB
      // 
      this.SendId_TB.Location = new System.Drawing.Point(181, 229);
      this.SendId_TB.Name = "SendId_TB";
      this.SendId_TB.Size = new System.Drawing.Size(33, 20);
      this.SendId_TB.TabIndex = 77;
      this.SendId_TB.Text = "0";
      // 
      // Modify_BT
      // 
      this.Modify_BT.Location = new System.Drawing.Point(21, 131);
      this.Modify_BT.Name = "Modify_BT";
      this.Modify_BT.Size = new System.Drawing.Size(138, 23);
      this.Modify_BT.TabIndex = 78;
      this.Modify_BT.Text = "Modify Data Set Element";
      this.Modify_BT.UseVisualStyleBackColor = true;
      this.Modify_BT.Click += new System.EventHandler(this.Modify_BT_Click);
      // 
      // Display_BT
      // 
      this.Display_BT.Location = new System.Drawing.Point(21, 160);
      this.Display_BT.Name = "Display_BT";
      this.Display_BT.Size = new System.Drawing.Size(138, 23);
      this.Display_BT.TabIndex = 79;
      this.Display_BT.Text = "Display Data Set Values";
      this.Display_BT.UseVisualStyleBackColor = true;
      this.Display_BT.Click += new System.EventHandler(this.Display_BT_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(169, 119);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(61, 13);
      this.label1.TabIndex = 80;
      this.label1.Text = "Data Set Id";
      // 
      // ModifyId_TB
      // 
      this.ModifyId_TB.Location = new System.Drawing.Point(181, 133);
      this.ModifyId_TB.Name = "ModifyId_TB";
      this.ModifyId_TB.Size = new System.Drawing.Size(33, 20);
      this.ModifyId_TB.TabIndex = 81;
      this.ModifyId_TB.Text = "0";
      // 
      // DisplayId_TB
      // 
      this.DisplayId_TB.Location = new System.Drawing.Point(181, 162);
      this.DisplayId_TB.Name = "DisplayId_TB";
      this.DisplayId_TB.Size = new System.Drawing.Size(33, 20);
      this.DisplayId_TB.TabIndex = 82;
      this.DisplayId_TB.Text = "0";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(231, 119);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(74, 13);
      this.label2.TabIndex = 83;
      this.label2.Text = "Element Index";
      // 
      // ModifyIndex_TB
      // 
      this.ModifyIndex_TB.Location = new System.Drawing.Point(250, 133);
      this.ModifyIndex_TB.Name = "ModifyIndex_TB";
      this.ModifyIndex_TB.Size = new System.Drawing.Size(36, 20);
      this.ModifyIndex_TB.TabIndex = 84;
      this.ModifyIndex_TB.Text = "2";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(323, 119);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(34, 13);
      this.label3.TabIndex = 85;
      this.label3.Text = "Value";
      // 
      // ModifyValue_TB
      // 
      this.ModifyValue_TB.Location = new System.Drawing.Point(309, 134);
      this.ModifyValue_TB.Name = "ModifyValue_TB";
      this.ModifyValue_TB.Size = new System.Drawing.Size(71, 20);
      this.ModifyValue_TB.TabIndex = 86;
      this.ModifyValue_TB.Text = "55";
      // 
      // ReadTypeCB
      // 
      this.ReadTypeCB.FormattingEnabled = true;
      this.ReadTypeCB.Location = new System.Drawing.Point(250, 199);
      this.ReadTypeCB.Name = "ReadTypeCB";
      this.ReadTypeCB.Size = new System.Drawing.Size(146, 21);
      this.ReadTypeCB.TabIndex = 87;
      // 
      // SendTypeCB
      // 
      this.SendTypeCB.FormattingEnabled = true;
      this.SendTypeCB.Location = new System.Drawing.Point(250, 229);
      this.SendTypeCB.Name = "SendTypeCB";
      this.SendTypeCB.Size = new System.Drawing.Size(146, 21);
      this.SendTypeCB.TabIndex = 90;
      // 
      // Connected_LB
      // 
      this.Connected_LB.AutoSize = true;
      this.Connected_LB.ForeColor = System.Drawing.Color.Crimson;
      this.Connected_LB.Location = new System.Drawing.Point(645, 27);
      this.Connected_LB.Name = "Connected_LB";
      this.Connected_LB.Size = new System.Drawing.Size(73, 13);
      this.Connected_LB.TabIndex = 91;
      this.Connected_LB.Text = "Disconnected";
      // 
      // Online_LB
      // 
      this.Online_LB.AutoSize = true;
      this.Online_LB.ForeColor = System.Drawing.Color.Crimson;
      this.Online_LB.Location = new System.Drawing.Point(645, 52);
      this.Online_LB.Name = "Online_LB";
      this.Online_LB.Size = new System.Drawing.Size(37, 13);
      this.Online_LB.TabIndex = 92;
      this.Online_LB.Text = "Offline";
      // 
      // SaveLogBt
      // 
      this.SaveLogBt.Location = new System.Drawing.Point(643, 78);
      this.SaveLogBt.Name = "SaveLogBt";
      this.SaveLogBt.Size = new System.Drawing.Size(75, 23);
      this.SaveLogBt.TabIndex = 98;
      this.SaveLogBt.Text = "Save Log";
      this.SaveLogBt.UseVisualStyleBackColor = true;
      this.SaveLogBt.Click += new System.EventHandler(this.SaveLogBt_Click);
      // 
      // MasterForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(752, 655);
      this.Controls.Add(this.SaveLogBt);
      this.Controls.Add(this.Online_LB);
      this.Controls.Add(this.Connected_LB);
      this.Controls.Add(this.SendTypeCB);
      this.Controls.Add(this.ReadTypeCB);
      this.Controls.Add(this.ModifyValue_TB);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.ModifyIndex_TB);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.DisplayId_TB);
      this.Controls.Add(this.ModifyId_TB);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.Display_BT);
      this.Controls.Add(this.Modify_BT);
      this.Controls.Add(this.SendId_TB);
      this.Controls.Add(this.ReadId_TB);
      this.Controls.Add(this.Send_BT);
      this.Controls.Add(this.Read_BT);
      this.Controls.Add(this.Output_TB);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.protocolAnalyzer);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "MasterForm";
      this.Text = "TMW .NET Protocol Component DNP Master Example";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Timer IntegrityPollTimer;
    private System.Windows.Forms.Timer EventPollTimer;
    private System.Windows.Forms.RichTextBox protocolAnalyzer;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.RichTextBox Output_TB;
    private System.Windows.Forms.Button Read_BT;
    private System.Windows.Forms.Button Send_BT;
    private System.Windows.Forms.TextBox ReadId_TB;
    private System.Windows.Forms.TextBox SendId_TB;
    private System.Windows.Forms.Button Modify_BT;
    private System.Windows.Forms.Button Display_BT;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox ModifyId_TB;
    private System.Windows.Forms.TextBox DisplayId_TB;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox ModifyIndex_TB;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox ModifyValue_TB;
    private System.Windows.Forms.ComboBox ReadTypeCB;
    private System.Windows.Forms.ComboBox SendTypeCB;
    private System.Windows.Forms.Label Connected_LB;
    private System.Windows.Forms.Label Online_LB;
    private System.Windows.Forms.Button SaveLogBt;
  }
}

