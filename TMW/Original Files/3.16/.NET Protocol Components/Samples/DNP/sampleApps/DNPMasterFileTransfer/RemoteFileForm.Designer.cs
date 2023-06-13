namespace DNPmasterFileTransfer
{
  partial class RemoteFileForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteFileForm));
      this.listView1 = new System.Windows.Forms.ListView();
      this.NameCol = new System.Windows.Forms.ColumnHeader();
      this.SizeHdr = new System.Windows.Forms.ColumnHeader();
      this.TimeHdr = new System.Windows.Forms.ColumnHeader();
      this.label1 = new System.Windows.Forms.Label();
      this.DirectoryTB = new System.Windows.Forms.TextBox();
      this.UpBT = new System.Windows.Forms.Button();
      this.RefreshBT = new System.Windows.Forms.Button();
      this.FileNameTB = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.OKBT = new System.Windows.Forms.Button();
      this.CancelBT = new System.Windows.Forms.Button();
      this.imageList1 = new System.Windows.Forms.ImageList(this.components);
      this.SuspendLayout();
      // 
      // listView1
      // 
      this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameCol,
            this.SizeHdr,
            this.TimeHdr});
      this.listView1.Location = new System.Drawing.Point(31, 65);
      this.listView1.Name = "listView1";
      this.listView1.Size = new System.Drawing.Size(491, 338);
      this.listView1.TabIndex = 0;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = System.Windows.Forms.View.Details;
      this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
      // 
      // NameCol
      // 
      this.NameCol.Text = "Name";
      this.NameCol.Width = 227;
      // 
      // SizeHdr
      // 
      this.SizeHdr.Text = "Size";
      this.SizeHdr.Width = 92;
      // 
      // TimeHdr
      // 
      this.TimeHdr.Text = "Time Of Creation";
      this.TimeHdr.Width = 171;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(28, 32);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(87, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Look in Directory";
      // 
      // DirectoryTB
      // 
      this.DirectoryTB.Location = new System.Drawing.Point(121, 29);
      this.DirectoryTB.Name = "DirectoryTB";
      this.DirectoryTB.Size = new System.Drawing.Size(202, 20);
      this.DirectoryTB.TabIndex = 2;
      // 
      // UpBT
      // 
      this.UpBT.Location = new System.Drawing.Point(344, 27);
      this.UpBT.Name = "UpBT";
      this.UpBT.Size = new System.Drawing.Size(92, 23);
      this.UpBT.TabIndex = 3;
      this.UpBT.Text = "Up One Level";
      this.UpBT.UseVisualStyleBackColor = true;
      this.UpBT.Click += new System.EventHandler(this.UpBT_Click);
      // 
      // RefreshBT
      // 
      this.RefreshBT.Location = new System.Drawing.Point(462, 27);
      this.RefreshBT.Name = "RefreshBT";
      this.RefreshBT.Size = new System.Drawing.Size(75, 23);
      this.RefreshBT.TabIndex = 4;
      this.RefreshBT.Text = "Refresh";
      this.RefreshBT.UseVisualStyleBackColor = true;
      this.RefreshBT.Click += new System.EventHandler(this.RefreshBT_Click);
      // 
      // FileNameTB
      // 
      this.FileNameTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.FileNameTB.Location = new System.Drawing.Point(91, 411);
      this.FileNameTB.Name = "FileNameTB";
      this.FileNameTB.Size = new System.Drawing.Size(278, 20);
      this.FileNameTB.TabIndex = 5;
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(28, 414);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(57, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "File Name:";
      // 
      // OKBT
      // 
      this.OKBT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.OKBT.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.OKBT.Location = new System.Drawing.Point(381, 409);
      this.OKBT.Name = "OKBT";
      this.OKBT.Size = new System.Drawing.Size(75, 23);
      this.OKBT.TabIndex = 7;
      this.OKBT.Text = "Ok";
      this.OKBT.UseVisualStyleBackColor = true;
      // 
      // CancelBT
      // 
      this.CancelBT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.CancelBT.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.CancelBT.Location = new System.Drawing.Point(462, 409);
      this.CancelBT.Name = "CancelBT";
      this.CancelBT.Size = new System.Drawing.Size(75, 23);
      this.CancelBT.TabIndex = 8;
      this.CancelBT.Text = "Cancel";
      this.CancelBT.UseVisualStyleBackColor = true;
      // 
      // imageList1
      // 
      this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
      this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "Folder.ico");
      this.imageList1.Images.SetKeyName(1, "UtilityText.ico");
      // 
      // RemoteFileForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(551, 444);
      this.Controls.Add(this.CancelBT);
      this.Controls.Add(this.OKBT);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.FileNameTB);
      this.Controls.Add(this.RefreshBT);
      this.Controls.Add(this.UpBT);
      this.Controls.Add(this.DirectoryTB);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.listView1);
      this.Name = "RemoteFileForm";
      this.Text = "Browse OutstationFiles";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListView listView1;
    private System.Windows.Forms.ColumnHeader NameCol;
    private System.Windows.Forms.ColumnHeader SizeHdr;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox DirectoryTB;
    private System.Windows.Forms.Button UpBT;
    private System.Windows.Forms.Button RefreshBT;
    private System.Windows.Forms.TextBox FileNameTB;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button OKBT;
    private System.Windows.Forms.Button CancelBT;
    private System.Windows.Forms.ImageList imageList1;
    private System.Windows.Forms.ColumnHeader TimeHdr;
  }
}