<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Panel1 = New Panel()
        header6 = New PictureBox()
        header5 = New PictureBox()
        header4 = New PictureBox()
        customheader = New PictureBox()
        header8 = New PictureBox()
        header7 = New PictureBox()
        header3 = New PictureBox()
        header2 = New PictureBox()
        header1 = New PictureBox()
        selectcustomheader = New OpenFileDialog()
        mainheader = New PictureBox()
        Label1 = New Label()
        Button1 = New Button()
        Label2 = New Label()
        PictureBox1 = New PictureBox()
        PictureBox2 = New PictureBox()
        Panel1.SuspendLayout()
        CType(header6, ComponentModel.ISupportInitialize).BeginInit()
        CType(header5, ComponentModel.ISupportInitialize).BeginInit()
        CType(header4, ComponentModel.ISupportInitialize).BeginInit()
        CType(customheader, ComponentModel.ISupportInitialize).BeginInit()
        CType(header8, ComponentModel.ISupportInitialize).BeginInit()
        CType(header7, ComponentModel.ISupportInitialize).BeginInit()
        CType(header3, ComponentModel.ISupportInitialize).BeginInit()
        CType(header2, ComponentModel.ISupportInitialize).BeginInit()
        CType(header1, ComponentModel.ISupportInitialize).BeginInit()
        CType(mainheader, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox2, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Panel1
        ' 
        Panel1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Panel1.BackColor = Color.FromArgb(CByte(15), CByte(108), CByte(230))
        Panel1.Controls.Add(header6)
        Panel1.Controls.Add(header5)
        Panel1.Controls.Add(header4)
        Panel1.Controls.Add(customheader)
        Panel1.Controls.Add(header8)
        Panel1.Controls.Add(header7)
        Panel1.Controls.Add(header3)
        Panel1.Controls.Add(header2)
        Panel1.Controls.Add(header1)
        Panel1.Location = New Point(10, 220)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(400, 400)
        Panel1.TabIndex = 0
        ' 
        ' header6
        ' 
        header6.BackgroundImageLayout = ImageLayout.None
        header6.Image = My.Resources.Resources.header6
        header6.Location = New Point(290, 160)
        header6.Name = "header6"
        header6.Size = New Size(80, 80)
        header6.SizeMode = PictureBoxSizeMode.Zoom
        header6.TabIndex = 8
        header6.TabStop = False
        ' 
        ' header5
        ' 
        header5.BackgroundImageLayout = ImageLayout.None
        header5.Image = My.Resources.Resources.header5
        header5.Location = New Point(160, 160)
        header5.Name = "header5"
        header5.Size = New Size(80, 80)
        header5.SizeMode = PictureBoxSizeMode.Zoom
        header5.TabIndex = 7
        header5.TabStop = False
        ' 
        ' header4
        ' 
        header4.BackgroundImageLayout = ImageLayout.None
        header4.Image = My.Resources.Resources.header4
        header4.Location = New Point(30, 160)
        header4.Name = "header4"
        header4.Size = New Size(80, 80)
        header4.SizeMode = PictureBoxSizeMode.Zoom
        header4.TabIndex = 6
        header4.TabStop = False
        ' 
        ' customheader
        ' 
        customheader.BackgroundImageLayout = ImageLayout.None
        customheader.Image = My.Resources.Resources.custom
        customheader.Location = New Point(290, 290)
        customheader.Name = "customheader"
        customheader.Size = New Size(80, 80)
        customheader.SizeMode = PictureBoxSizeMode.Zoom
        customheader.TabIndex = 5
        customheader.TabStop = False
        ' 
        ' header8
        ' 
        header8.BackgroundImageLayout = ImageLayout.None
        header8.Image = My.Resources.Resources.header8
        header8.Location = New Point(160, 290)
        header8.Name = "header8"
        header8.Size = New Size(80, 80)
        header8.SizeMode = PictureBoxSizeMode.Zoom
        header8.TabIndex = 4
        header8.TabStop = False
        ' 
        ' header7
        ' 
        header7.BackgroundImageLayout = ImageLayout.None
        header7.Image = My.Resources.Resources.header7
        header7.Location = New Point(30, 290)
        header7.Name = "header7"
        header7.Size = New Size(80, 80)
        header7.SizeMode = PictureBoxSizeMode.Zoom
        header7.TabIndex = 3
        header7.TabStop = False
        ' 
        ' header3
        ' 
        header3.BackgroundImageLayout = ImageLayout.None
        header3.Image = My.Resources.Resources.header3
        header3.Location = New Point(290, 30)
        header3.Name = "header3"
        header3.Size = New Size(80, 80)
        header3.SizeMode = PictureBoxSizeMode.Zoom
        header3.TabIndex = 2
        header3.TabStop = False
        ' 
        ' header2
        ' 
        header2.BackgroundImageLayout = ImageLayout.None
        header2.Image = My.Resources.Resources.header2
        header2.Location = New Point(160, 30)
        header2.Name = "header2"
        header2.Size = New Size(80, 80)
        header2.SizeMode = PictureBoxSizeMode.Zoom
        header2.TabIndex = 1
        header2.TabStop = False
        ' 
        ' header1
        ' 
        header1.BackgroundImageLayout = ImageLayout.None
        header1.Image = My.Resources.Resources.header1
        header1.Location = New Point(30, 30)
        header1.Name = "header1"
        header1.Size = New Size(80, 80)
        header1.SizeMode = PictureBoxSizeMode.Zoom
        header1.TabIndex = 0
        header1.TabStop = False
        ' 
        ' mainheader
        ' 
        mainheader.BackColor = Color.Transparent
        mainheader.BackgroundImage = My.Resources.Resources.header1
        mainheader.BackgroundImageLayout = ImageLayout.Zoom
        mainheader.Image = My.Resources.Resources.header1
        mainheader.Location = New Point(10, 10)
        mainheader.Name = "mainheader"
        mainheader.Size = New Size(96, 96)
        mainheader.SizeMode = PictureBoxSizeMode.Zoom
        mainheader.TabIndex = 1
        mainheader.TabStop = False
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.BackColor = Color.Transparent
        Label1.Font = New Font("方正兰亭圆_GBK_中", 24.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label1.ForeColor = Color.White
        Label1.Location = New Point(112, 10)
        Label1.Name = "Label1"
        Label1.Size = New Size(144, 47)
        Label1.TabIndex = 2
        Label1.Text = "Label1"
        ' 
        ' Button1
        ' 
        Button1.Font = New Font("方正兰亭圆_GBK_中", 12.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(134))
        Button1.Location = New Point(120, 66)
        Button1.Name = "Button1"
        Button1.Size = New Size(150, 40)
        Button1.TabIndex = 3
        Button1.Text = "更改昵称"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.BackColor = Color.Transparent
        Label2.Font = New Font("方正兰亭圆_GBK_中", 16.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(134))
        Label2.ForeColor = Color.Transparent
        Label2.Location = New Point(12, 185)
        Label2.Name = "Label2"
        Label2.Size = New Size(182, 32)
        Label2.TabIndex = 4
        Label2.Text = "选择一个头像"
        ' 
        ' PictureBox1
        ' 
        PictureBox1.BackColor = Color.Transparent
        PictureBox1.Image = My.Resources.Resources.hot_100
        PictureBox1.Location = New Point(34, 112)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(160, 65)
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        PictureBox1.TabIndex = 5
        PictureBox1.TabStop = False
        ' 
        ' PictureBox2
        ' 
        PictureBox2.BackColor = Color.Transparent
        PictureBox2.Image = My.Resources.Resources.scup_1
        PictureBox2.Location = New Point(212, 112)
        PictureBox2.Name = "PictureBox2"
        PictureBox2.Size = New Size(168, 65)
        PictureBox2.SizeMode = PictureBoxSizeMode.Zoom
        PictureBox2.TabIndex = 6
        PictureBox2.TabStop = False
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(9.0F, 20.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), Image)
        BackgroundImageLayout = ImageLayout.Center
        ClientSize = New Size(420, 630)
        Controls.Add(PictureBox2)
        Controls.Add(PictureBox1)
        Controls.Add(Label2)
        Controls.Add(Button1)
        Controls.Add(Panel1)
        Controls.Add(Label1)
        Controls.Add(mainheader)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        KeyPreview = True
        MaximizeBox = False
        MinimizeBox = False
        Name = "Form1"
        ShowIcon = False
        StartPosition = FormStartPosition.CenterScreen
        Text = "鸡蛋鸭蛋荷包蛋"
        Panel1.ResumeLayout(False)
        CType(header6, ComponentModel.ISupportInitialize).EndInit()
        CType(header5, ComponentModel.ISupportInitialize).EndInit()
        CType(header4, ComponentModel.ISupportInitialize).EndInit()
        CType(customheader, ComponentModel.ISupportInitialize).EndInit()
        CType(header8, ComponentModel.ISupportInitialize).EndInit()
        CType(header7, ComponentModel.ISupportInitialize).EndInit()
        CType(header3, ComponentModel.ISupportInitialize).EndInit()
        CType(header2, ComponentModel.ISupportInitialize).EndInit()
        CType(header1, ComponentModel.ISupportInitialize).EndInit()
        CType(mainheader, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox2, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents header1 As PictureBox
    Friend WithEvents customheader As PictureBox
    Friend WithEvents header8 As PictureBox
    Friend WithEvents header7 As PictureBox
    Friend WithEvents header3 As PictureBox
    Friend WithEvents header2 As PictureBox
    Friend WithEvents header6 As PictureBox
    Friend WithEvents header5 As PictureBox
    Friend WithEvents header4 As PictureBox
    Friend WithEvents selectcustomheader As OpenFileDialog
    Friend WithEvents mainheader As PictureBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox2 As PictureBox

End Class
