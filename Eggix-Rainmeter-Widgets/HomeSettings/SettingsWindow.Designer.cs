// OpenEggyUI 是一个开源的 Windows 桌面美化组件项目，旨在延续 EggyUI 的精神，提供轻量化、可自由组合的美化工具。
// 此代码由冷情镜像站编写。
// 本项目仅用于学习与交流，严禁将 OpenEggyUI 及其组件用于任何商业用途。

namespace HomeSettings
{
    public partial class SettingsWindow
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            AvatarListView = new System.Windows.Forms.ListView();
            AvatarImageList = new System.Windows.Forms.ImageList(components);
            AvatarPictureBox = new System.Windows.Forms.PictureBox();
            NameLabel = new System.Windows.Forms.Label();
            TipLabel1 = new System.Windows.Forms.Label();
            OpenAvatarFolderButton = new System.Windows.Forms.Button();
            RunAvatarMakerButton = new System.Windows.Forms.Button();
            RefreshAvatarButton = new System.Windows.Forms.Button();
            RefreshAvatarListButton = new System.Windows.Forms.Button();
            LevelLabel = new System.Windows.Forms.Label();
            AppMenu = new System.Windows.Forms.ContextMenuStrip(components);
            CreateRankFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            SaveRankFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            CreateHomeIniFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            RefreshRankInfoButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)AvatarPictureBox).BeginInit();
            AppMenu.SuspendLayout();
            SuspendLayout();
            // 
            // AvatarListView
            // 
            AvatarListView.BackColor = System.Drawing.SystemColors.Window;
            AvatarListView.LargeImageList = AvatarImageList;
            AvatarListView.Location = new System.Drawing.Point(800, 100);
            AvatarListView.Margin = new System.Windows.Forms.Padding(6);
            AvatarListView.MultiSelect = false;
            AvatarListView.Name = "AvatarListView";
            AvatarListView.Size = new System.Drawing.Size(756, 756);
            AvatarListView.TabIndex = 0;
            AvatarListView.UseCompatibleStateImageBehavior = false;
            AvatarListView.MouseDoubleClick += AvatarListView_MouseDoubleClick;
            // 
            // AvatarImageList
            // 
            AvatarImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            AvatarImageList.ImageSize = new System.Drawing.Size(80, 80);
            AvatarImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // AvatarPictureBox
            // 
            AvatarPictureBox.BackColor = System.Drawing.Color.Transparent;
            AvatarPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            AvatarPictureBox.Location = new System.Drawing.Point(20, 20);
            AvatarPictureBox.Margin = new System.Windows.Forms.Padding(6);
            AvatarPictureBox.Name = "AvatarPictureBox";
            AvatarPictureBox.Size = new System.Drawing.Size(240, 234);
            AvatarPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            AvatarPictureBox.TabIndex = 1;
            AvatarPictureBox.TabStop = false;
            // 
            // NameLabel
            // 
            NameLabel.AutoSize = true;
            NameLabel.BackColor = System.Drawing.Color.Transparent;
            NameLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
            NameLabel.ForeColor = System.Drawing.Color.White;
            NameLabel.Location = new System.Drawing.Point(272, 17);
            NameLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new System.Drawing.Size(434, 76);
            NameLabel.TabIndex = 2;
            NameLabel.Text = "玩家名六个字";
            // 
            // TipLabel1
            // 
            TipLabel1.AutoSize = true;
            TipLabel1.BackColor = System.Drawing.Color.Transparent;
            TipLabel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
            TipLabel1.ForeColor = System.Drawing.Color.White;
            TipLabel1.Location = new System.Drawing.Point(800, 17);
            TipLabel1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            TipLabel1.Name = "TipLabel1";
            TipLabel1.Size = new System.Drawing.Size(434, 76);
            TipLabel1.TabIndex = 4;
            TipLabel1.Text = "选择一个头像";
            // 
            // OpenAvatarFolderButton
            // 
            OpenAvatarFolderButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
            OpenAvatarFolderButton.Location = new System.Drawing.Point(20, 266);
            OpenAvatarFolderButton.Margin = new System.Windows.Forms.Padding(6);
            OpenAvatarFolderButton.Name = "OpenAvatarFolderButton";
            OpenAvatarFolderButton.Size = new System.Drawing.Size(375, 100);
            OpenAvatarFolderButton.TabIndex = 3;
            OpenAvatarFolderButton.Text = "打开头像文件夹";
            OpenAvatarFolderButton.UseVisualStyleBackColor = true;
            OpenAvatarFolderButton.Click += OpenAvatarFolderButton_Click;
            // 
            // RunAvatarMakerButton
            // 
            RunAvatarMakerButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
            RunAvatarMakerButton.Location = new System.Drawing.Point(407, 266);
            RunAvatarMakerButton.Margin = new System.Windows.Forms.Padding(6);
            RunAvatarMakerButton.Name = "RunAvatarMakerButton";
            RunAvatarMakerButton.Size = new System.Drawing.Size(375, 100);
            RunAvatarMakerButton.TabIndex = 5;
            RunAvatarMakerButton.Text = "打开头像制作器";
            RunAvatarMakerButton.UseVisualStyleBackColor = true;
            RunAvatarMakerButton.Click += RunAvatarMakerButton_Click;
            // 
            // RefreshAvatarButton
            // 
            RefreshAvatarButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
            RefreshAvatarButton.Location = new System.Drawing.Point(20, 378);
            RefreshAvatarButton.Margin = new System.Windows.Forms.Padding(6);
            RefreshAvatarButton.Name = "RefreshAvatarButton";
            RefreshAvatarButton.Size = new System.Drawing.Size(375, 100);
            RefreshAvatarButton.TabIndex = 6;
            RefreshAvatarButton.Text = "刷新头像";
            RefreshAvatarButton.UseVisualStyleBackColor = true;
            RefreshAvatarButton.Click += RefreshAvatarButton_Click;
            // 
            // RefreshAvatarListButton
            // 
            RefreshAvatarListButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
            RefreshAvatarListButton.Location = new System.Drawing.Point(407, 378);
            RefreshAvatarListButton.Margin = new System.Windows.Forms.Padding(6);
            RefreshAvatarListButton.Name = "RefreshAvatarListButton";
            RefreshAvatarListButton.Size = new System.Drawing.Size(375, 100);
            RefreshAvatarListButton.TabIndex = 7;
            RefreshAvatarListButton.Text = "刷新头像列表";
            RefreshAvatarListButton.UseVisualStyleBackColor = true;
            RefreshAvatarListButton.Click += RefreshAvatarListButton_Click;
            // 
            // LevelLabel
            // 
            LevelLabel.AutoSize = true;
            LevelLabel.BackColor = System.Drawing.Color.Transparent;
            LevelLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
            LevelLabel.ForeColor = System.Drawing.Color.White;
            LevelLabel.Location = new System.Drawing.Point(277, 100);
            LevelLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            LevelLabel.Name = "LevelLabel";
            LevelLabel.Size = new System.Drawing.Size(94, 46);
            LevelLabel.TabIndex = 8;
            LevelLabel.Text = "Lv.0";
            // 
            // AppMenu
            // 
            AppMenu.ImageScalingSize = new System.Drawing.Size(40, 40);
            AppMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { CreateRankFileMenuItem, SaveRankFileMenuItem, CreateHomeIniFileMenuItem, AboutMenuItem });
            AppMenu.Name = "AppMenu";
            AppMenu.Size = new System.Drawing.Size(401, 188);
            // 
            // CreateRankFileMenuItem
            // 
            CreateRankFileMenuItem.Name = "CreateRankFileMenuItem";
            CreateRankFileMenuItem.Size = new System.Drawing.Size(400, 46);
            CreateRankFileMenuItem.Text = "创建段位配置文件(&C)";
            // 
            // SaveRankFileMenuItem
            // 
            SaveRankFileMenuItem.Name = "SaveRankFileMenuItem";
            SaveRankFileMenuItem.Size = new System.Drawing.Size(400, 46);
            SaveRankFileMenuItem.Text = "保存段位配置文件(&S)";
            // 
            // CreateHomeIniFileMenuItem
            // 
            CreateHomeIniFileMenuItem.Name = "CreateHomeIniFileMenuItem";
            CreateHomeIniFileMenuItem.Size = new System.Drawing.Size(400, 46);
            CreateHomeIniFileMenuItem.Text = "创建home.ini文件(&H）";
            // 
            // AboutMenuItem
            // 
            AboutMenuItem.Name = "AboutMenuItem";
            AboutMenuItem.Size = new System.Drawing.Size(400, 46);
            AboutMenuItem.Text = "关于(&A)";
            // 
            // RefreshRankInfoButton
            // 
            RefreshRankInfoButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)134));
            RefreshRankInfoButton.Location = new System.Drawing.Point(20, 490);
            RefreshRankInfoButton.Margin = new System.Windows.Forms.Padding(6);
            RefreshRankInfoButton.Name = "RefreshRankInfoButton";
            RefreshRankInfoButton.Size = new System.Drawing.Size(375, 100);
            RefreshRankInfoButton.TabIndex = 9;
            RefreshRankInfoButton.Text = "刷新段位信息";
            RefreshRankInfoButton.UseVisualStyleBackColor = true;
            RefreshRankInfoButton.Click += RefreshRankInfoButton_Click;
            // 
            // SettingsWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(18F, 39F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(((int)((byte)11)), ((int)((byte)148)), ((int)((byte)255)));
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ClientSize = new System.Drawing.Size(1600, 900);
            ContextMenuStrip = AppMenu;
            Controls.Add(RefreshRankInfoButton);
            Controls.Add(LevelLabel);
            Controls.Add(RefreshAvatarListButton);
            Controls.Add(RefreshAvatarButton);
            Controls.Add(RunAvatarMakerButton);
            Controls.Add(TipLabel1);
            Controls.Add(AvatarListView);
            Controls.Add(OpenAvatarFolderButton);
            Controls.Add(NameLabel);
            Controls.Add(AvatarPictureBox);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(6);
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "鸡蛋鸭蛋荷包蛋";
            Load += SettingsWindow_Load;
            DpiChanged += SettingsWindow_DpiChanged;
            ((System.ComponentModel.ISupportInitialize)AvatarPictureBox).EndInit();
            AppMenu.ResumeLayout(false);
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
        private Label LevelLabel;
        private ContextMenuStrip AppMenu;
        private ToolStripMenuItem CreateRankFileMenuItem;
        private ToolStripMenuItem AboutMenuItem;
        private ToolStripMenuItem CreateHomeIniFileMenuItem;
        private Button RefreshRankInfoButton;
        private ToolStripMenuItem SaveRankFileMenuItem;
    }
}