// OpenEggyUI 是一个开源的 Windows 桌面美化组件项目，旨在延续 EggyUI 的精神，提供轻量化、可自由组合的美化工具。
// 此代码由冷情镜像站编写。
// 本项目仅用于学习与交流，严禁将 OpenEggyUI 及其组件用于任何商业用途。

namespace HomeSettings
{
    public partial class Settings_Window
    {
        private System.ComponentModel.IContainer components = null;
        
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
        /// 初始化窗口
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            AvatarListView = new ListView();
            AvatarImageList = new ImageList(components);
            AvatarPictureBox = new PictureBox();
            NameLabel = new Label();
            TipLabel1 = new Label();
            OpenAvatarFolderButton = new Button();
            RunAvatarMakerButton = new Button();
            RefreshAvatarButton = new Button();
            RefreshAvatarListButton = new Button();
            ((System.ComponentModel.ISupportInitialize)AvatarPictureBox).BeginInit();
            SuspendLayout();
            // 
            // AvatarListView
            // 
            AvatarListView.LargeImageList = AvatarImageList;
            AvatarListView.Location = new Point(800, 100);
            AvatarListView.Margin = new Padding(6);
            AvatarListView.MultiSelect = false;
            AvatarListView.Name = "AvatarListView";
            AvatarListView.Size = new Size(756, 756);
            AvatarListView.TabIndex = 0;
            AvatarListView.UseCompatibleStateImageBehavior = false;
            AvatarListView.MouseDoubleClick += AvatarListView_MouseDoubleClick;
            // 
            // AvatarImageList
            // 
            AvatarImageList.ColorDepth = ColorDepth.Depth32Bit;
            AvatarImageList.ImageSize = new Size(80, 80);
            AvatarImageList.TransparentColor = Color.Transparent;
            // 
            // AvatarPictureBox
            // 
            AvatarPictureBox.BackColor = Color.Transparent;
            AvatarPictureBox.BackgroundImageLayout = ImageLayout.Zoom;
            AvatarPictureBox.Location = new Point(20, 20);
            AvatarPictureBox.Margin = new Padding(6);
            AvatarPictureBox.Name = "AvatarPictureBox";
            AvatarPictureBox.Size = new Size(240, 234);
            AvatarPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            AvatarPictureBox.TabIndex = 1;
            AvatarPictureBox.TabStop = false;
            // 
            // NameLabel
            // 
            NameLabel.AutoSize = true;
            NameLabel.BackColor = Color.Transparent;
            NameLabel.Font = new Font("方正兰亭圆简体_粗", 20F, FontStyle.Regular, GraphicsUnit.Point, 134);
            NameLabel.ForeColor = Color.White;
            NameLabel.Location = new Point(272, 17);
            NameLabel.Margin = new Padding(6, 0, 6, 0);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new Size(435, 77);
            NameLabel.TabIndex = 2;
            NameLabel.Text = "玩家名六个字";
            // 
            // TipLabel1
            // 
            TipLabel1.AutoSize = true;
            TipLabel1.BackColor = Color.Transparent;
            TipLabel1.Font = new Font("方正兰亭圆简体_粗", 20F, FontStyle.Regular, GraphicsUnit.Point, 134);
            TipLabel1.ForeColor = Color.White;
            TipLabel1.Location = new Point(800, 17);
            TipLabel1.Margin = new Padding(6, 0, 6, 0);
            TipLabel1.Name = "TipLabel1";
            TipLabel1.Size = new Size(435, 77);
            TipLabel1.TabIndex = 4;
            TipLabel1.Text = "选择一个头像";
            // 
            // OpenAvatarFolderButton
            // 
            OpenAvatarFolderButton.Font = new Font("方正兰亭圆简体_粗", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            OpenAvatarFolderButton.Location = new Point(20, 266);
            OpenAvatarFolderButton.Margin = new Padding(6);
            OpenAvatarFolderButton.Name = "OpenAvatarFolderButton";
            OpenAvatarFolderButton.Size = new Size(375, 100);
            OpenAvatarFolderButton.TabIndex = 3;
            OpenAvatarFolderButton.Text = "打开头像文件夹";
            OpenAvatarFolderButton.UseVisualStyleBackColor = true;
            OpenAvatarFolderButton.Click += OpenAvatarFolderButton_Click;
            // 
            // RunAvatarMakerButton
            // 
            RunAvatarMakerButton.Font = new Font("方正兰亭圆简体_粗", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            RunAvatarMakerButton.Location = new Point(407, 266);
            RunAvatarMakerButton.Margin = new Padding(6);
            RunAvatarMakerButton.Name = "RunAvatarMakerButton";
            RunAvatarMakerButton.Size = new Size(375, 100);
            RunAvatarMakerButton.TabIndex = 5;
            RunAvatarMakerButton.Text = "打开头像制作器";
            RunAvatarMakerButton.UseVisualStyleBackColor = true;
            RunAvatarMakerButton.Click += RunAvatarMakerButton_Click;
            // 
            // RefreshAvatarButton
            // 
            RefreshAvatarButton.Font = new Font("方正兰亭圆简体_粗", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            RefreshAvatarButton.Location = new Point(20, 378);
            RefreshAvatarButton.Margin = new Padding(6);
            RefreshAvatarButton.Name = "RefreshAvatarButton";
            RefreshAvatarButton.Size = new Size(375, 100);
            RefreshAvatarButton.TabIndex = 6;
            RefreshAvatarButton.Text = "刷新头像";
            RefreshAvatarButton.UseVisualStyleBackColor = true;
            RefreshAvatarButton.Click += RefreshAvatarButton_Click;
            // 
            // RefreshAvatarListButton
            // 
            RefreshAvatarListButton.Font = new Font("方正兰亭圆简体_粗", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            RefreshAvatarListButton.Location = new Point(407, 378);
            RefreshAvatarListButton.Margin = new Padding(6);
            RefreshAvatarListButton.Name = "RefreshAvatarListButton";
            RefreshAvatarListButton.Size = new Size(375, 100);
            RefreshAvatarListButton.TabIndex = 7;
            RefreshAvatarListButton.Text = "刷新头像列表";
            RefreshAvatarListButton.UseVisualStyleBackColor = true;
            RefreshAvatarListButton.Click += RefreshAvatarListButton_Click;
            // 
            // Settings_Window
            // 
            AutoScaleDimensions = new SizeF(18F, 39F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1600, 900);
            Controls.Add(RefreshAvatarListButton);
            Controls.Add(RefreshAvatarButton);
            Controls.Add(RunAvatarMakerButton);
            Controls.Add(TipLabel1);
            Controls.Add(AvatarListView);
            Controls.Add(OpenAvatarFolderButton);
            Controls.Add(NameLabel);
            Controls.Add(AvatarPictureBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            Margin = new Padding(6);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Settings_Window";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "鸡蛋鸭蛋荷包蛋";
            ((System.ComponentModel.ISupportInitialize)AvatarPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox AvatarPictureBox;
        private System.Windows.Forms.Label NameLabel;
        private ListView AvatarListView;
        private ImageList AvatarImageList;
        private Label TipLabel1;
        private Button OpenAvatarFolderButton;
        private Button RunAvatarMakerButton;
        private Button RefreshAvatarButton;
        private Button RefreshAvatarListButton;
    }
}