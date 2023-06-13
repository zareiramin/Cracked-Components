<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Private mainMenu1 As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.listBoxLog = New System.Windows.Forms.ListBox
        Me.button3 = New System.Windows.Forms.Button
        Me.button2 = New System.Windows.Forms.Button
        Me.button1 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'listBoxLog
        '
        Me.listBoxLog.Location = New System.Drawing.Point(4, 29)
        Me.listBoxLog.Name = "listBoxLog"
        Me.listBoxLog.Size = New System.Drawing.Size(228, 98)
        Me.listBoxLog.TabIndex = 7
        '
        'button3
        '
        Me.button3.Location = New System.Drawing.Point(160, 3)
        Me.button3.Name = "button3"
        Me.button3.Size = New System.Drawing.Size(72, 20)
        Me.button3.TabIndex = 6
        Me.button3.Text = "Shutdown"
        '
        'button2
        '
        Me.button2.Location = New System.Drawing.Point(82, 3)
        Me.button2.Name = "button2"
        Me.button2.Size = New System.Drawing.Size(72, 20)
        Me.button2.TabIndex = 5
        Me.button2.Text = "Resume"
        '
        'button1
        '
        Me.button1.Location = New System.Drawing.Point(4, 3)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(72, 20)
        Me.button1.TabIndex = 4
        Me.button1.Text = "Suspend"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(238, 133)
        Me.Controls.Add(Me.listBoxLog)
        Me.Controls.Add(Me.button3)
        Me.Controls.Add(Me.button2)
        Me.Controls.Add(Me.button1)
        Me.Menu = Me.mainMenu1
        Me.Name = "Form1"
        Me.Text = "ClrLifeTime"
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents listBoxLog As System.Windows.Forms.ListBox
    Private WithEvents button3 As System.Windows.Forms.Button
    Private WithEvents button2 As System.Windows.Forms.Button
    Private WithEvents button1 As System.Windows.Forms.Button
End Class
