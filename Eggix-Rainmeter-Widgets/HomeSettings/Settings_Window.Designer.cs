// ============================================================================
// 模块名称: Settings_Window.Designer
// 创建者: SYSTEM-LIGHT
// 项目: Eggix - 《蛋仔派对》风格桌面组件
// 描述: 
// - 此代码文件为设置窗口类的设计器文件，负责定义窗口的可视化元素。
// 设计思路:
// - 基于模块化原则构建，确保功能的独立性与可测试性
// - 遵循清晰的单一职责原则，避免逻辑过度耦合
// - 通过依赖注入管理组件生命周期，提升可维护性
// 重要说明:
// - 此组件为Eggix项目的一部分，是EggyUI项目的非官方精神续作
// - 所有视觉元素均为重制或合法获取，未使用任何游戏解包素材
// - 此代码基于.NET Framework 4.6.2，专为性能、安全性与长期维护性而设计
// 注意事项:
// - 修改此代码前请确保理解其在整个组件生态系统中的角色
// - 对公共API的任何更改都需要同步更新相关文档与依赖模块
// ============================================================================

namespace HomeSettings
{
    partial class Settings_Window
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings_Window));
            this.Panel1 = new System.Windows.Forms.Panel();
            this.header12 = new System.Windows.Forms.PictureBox();
            this.header9 = new System.Windows.Forms.PictureBox();
            this.header11 = new System.Windows.Forms.PictureBox();
            this.header10 = new System.Windows.Forms.PictureBox();
            this.header6 = new System.Windows.Forms.PictureBox();
            this.header5 = new System.Windows.Forms.PictureBox();
            this.CustomHeader = new System.Windows.Forms.PictureBox();
            this.header4 = new System.Windows.Forms.PictureBox();
            this.header8 = new System.Windows.Forms.PictureBox();
            this.header7 = new System.Windows.Forms.PictureBox();
            this.header3 = new System.Windows.Forms.PictureBox();
            this.header2 = new System.Windows.Forms.PictureBox();
            this.header1 = new System.Windows.Forms.PictureBox();
            this.SelectCustomHeaderDialog = new System.Windows.Forms.OpenFileDialog();
            this.CurrentHeader = new System.Windows.Forms.PictureBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.ChangeNameButton = new System.Windows.Forms.Button();
            this.HeaderLabel = new System.Windows.Forms.Label();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.PictureBox2 = new System.Windows.Forms.PictureBox();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.header12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.header9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.header11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.header10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.header6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.header5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.header4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.header8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.header7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.header3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.header2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.header1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.AutoScroll = true;
            this.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(108)))), ((int)(((byte)(230)))));
            this.Panel1.Controls.Add(this.header12);
            this.Panel1.Controls.Add(this.header9);
            this.Panel1.Controls.Add(this.header11);
            this.Panel1.Controls.Add(this.header10);
            this.Panel1.Controls.Add(this.header6);
            this.Panel1.Controls.Add(this.header5);
            this.Panel1.Controls.Add(this.CustomHeader);
            this.Panel1.Controls.Add(this.header4);
            this.Panel1.Controls.Add(this.header8);
            this.Panel1.Controls.Add(this.header7);
            this.Panel1.Controls.Add(this.header3);
            this.Panel1.Controls.Add(this.header2);
            this.Panel1.Controls.Add(this.header1);
            this.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Panel1.Location = new System.Drawing.Point(0, 184);
            this.Panel1.Margin = new System.Windows.Forms.Padding(2);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(336, 320);
            this.Panel1.TabIndex = 0;
            // 
            // header12
            // 
            this.header12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.header12.Image = global::HomeSettings.Properties.Resources.header12;
            this.header12.Location = new System.Drawing.Point(232, 336);
            this.header12.Margin = new System.Windows.Forms.Padding(2);
            this.header12.Name = "header12";
            this.header12.Size = new System.Drawing.Size(64, 64);
            this.header12.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.header12.TabIndex = 12;
            this.header12.TabStop = false;
            // 
            // header9
            // 
            this.header9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.header9.Image = global::HomeSettings.Properties.Resources.header9;
            this.header9.Location = new System.Drawing.Point(232, 232);
            this.header9.Margin = new System.Windows.Forms.Padding(2);
            this.header9.Name = "header9";
            this.header9.Size = new System.Drawing.Size(64, 64);
            this.header9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.header9.TabIndex = 11;
            this.header9.TabStop = false;
            // 
            // header11
            // 
            this.header11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.header11.Image = global::HomeSettings.Properties.Resources.header11;
            this.header11.Location = new System.Drawing.Point(128, 336);
            this.header11.Margin = new System.Windows.Forms.Padding(2);
            this.header11.Name = "header11";
            this.header11.Size = new System.Drawing.Size(64, 64);
            this.header11.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.header11.TabIndex = 10;
            this.header11.TabStop = false;
            // 
            // header10
            // 
            this.header10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.header10.Image = global::HomeSettings.Properties.Resources.header10;
            this.header10.Location = new System.Drawing.Point(24, 336);
            this.header10.Margin = new System.Windows.Forms.Padding(2);
            this.header10.Name = "header10";
            this.header10.Size = new System.Drawing.Size(64, 64);
            this.header10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.header10.TabIndex = 9;
            this.header10.TabStop = false;
            // 
            // header6
            // 
            this.header6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.header6.Image = global::HomeSettings.Properties.Resources.header6;
            this.header6.Location = new System.Drawing.Point(232, 128);
            this.header6.Margin = new System.Windows.Forms.Padding(2);
            this.header6.Name = "header6";
            this.header6.Size = new System.Drawing.Size(64, 64);
            this.header6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.header6.TabIndex = 8;
            this.header6.TabStop = false;
            // 
            // header5
            // 
            this.header5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.header5.Image = global::HomeSettings.Properties.Resources.header5;
            this.header5.Location = new System.Drawing.Point(128, 128);
            this.header5.Margin = new System.Windows.Forms.Padding(2);
            this.header5.Name = "header5";
            this.header5.Size = new System.Drawing.Size(64, 64);
            this.header5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.header5.TabIndex = 7;
            this.header5.TabStop = false;
            // 
            // CustomHeader
            // 
            this.CustomHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CustomHeader.Image = ((System.Drawing.Image)(resources.GetObject("CustomHeader.Image")));
            this.CustomHeader.Location = new System.Drawing.Point(24, 440);
            this.CustomHeader.Margin = new System.Windows.Forms.Padding(2);
            this.CustomHeader.Name = "CustomHeader";
            this.CustomHeader.Size = new System.Drawing.Size(64, 64);
            this.CustomHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CustomHeader.TabIndex = 5;
            this.CustomHeader.TabStop = false;
            // 
            // header4
            // 
            this.header4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.header4.Image = global::HomeSettings.Properties.Resources.header4;
            this.header4.Location = new System.Drawing.Point(24, 128);
            this.header4.Margin = new System.Windows.Forms.Padding(2);
            this.header4.Name = "header4";
            this.header4.Size = new System.Drawing.Size(64, 64);
            this.header4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.header4.TabIndex = 6;
            this.header4.TabStop = false;
            // 
            // header8
            // 
            this.header8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.header8.Image = global::HomeSettings.Properties.Resources.header8;
            this.header8.Location = new System.Drawing.Point(128, 232);
            this.header8.Margin = new System.Windows.Forms.Padding(2);
            this.header8.Name = "header8";
            this.header8.Size = new System.Drawing.Size(64, 64);
            this.header8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.header8.TabIndex = 4;
            this.header8.TabStop = false;
            // 
            // header7
            // 
            this.header7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.header7.Image = global::HomeSettings.Properties.Resources.header7;
            this.header7.Location = new System.Drawing.Point(24, 232);
            this.header7.Margin = new System.Windows.Forms.Padding(2);
            this.header7.Name = "header7";
            this.header7.Size = new System.Drawing.Size(64, 64);
            this.header7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.header7.TabIndex = 3;
            this.header7.TabStop = false;
            // 
            // header3
            // 
            this.header3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.header3.Image = global::HomeSettings.Properties.Resources.header3;
            this.header3.Location = new System.Drawing.Point(232, 24);
            this.header3.Margin = new System.Windows.Forms.Padding(2);
            this.header3.Name = "header3";
            this.header3.Size = new System.Drawing.Size(64, 64);
            this.header3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.header3.TabIndex = 2;
            this.header3.TabStop = false;
            // 
            // header2
            // 
            this.header2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.header2.Image = global::HomeSettings.Properties.Resources.header2;
            this.header2.Location = new System.Drawing.Point(128, 24);
            this.header2.Margin = new System.Windows.Forms.Padding(2);
            this.header2.Name = "header2";
            this.header2.Size = new System.Drawing.Size(64, 64);
            this.header2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.header2.TabIndex = 1;
            this.header2.TabStop = false;
            // 
            // header1
            // 
            this.header1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.header1.Image = global::HomeSettings.Properties.Resources.header1;
            this.header1.Location = new System.Drawing.Point(24, 24);
            this.header1.Margin = new System.Windows.Forms.Padding(2);
            this.header1.Name = "header1";
            this.header1.Size = new System.Drawing.Size(64, 64);
            this.header1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.header1.TabIndex = 0;
            this.header1.TabStop = false;
            // 
            // SelectCustomHeaderDialog
            // 
            this.SelectCustomHeaderDialog.Filter = "受支持的图片文件|*.png;";
            this.SelectCustomHeaderDialog.Title = "选择自定义头像";
            // 
            // CurrentHeader
            // 
            this.CurrentHeader.BackColor = System.Drawing.Color.Transparent;
            this.CurrentHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.CurrentHeader.Location = new System.Drawing.Point(8, 8);
            this.CurrentHeader.Margin = new System.Windows.Forms.Padding(2);
            this.CurrentHeader.Name = "CurrentHeader";
            this.CurrentHeader.Size = new System.Drawing.Size(77, 77);
            this.CurrentHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CurrentHeader.TabIndex = 1;
            this.CurrentHeader.TabStop = false;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.BackColor = System.Drawing.Color.Transparent;
            this.NameLabel.Font = new System.Drawing.Font("方正兰亭圆简体_中", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NameLabel.ForeColor = System.Drawing.Color.White;
            this.NameLabel.Location = new System.Drawing.Point(90, 8);
            this.NameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(209, 38);
            this.NameLabel.TabIndex = 2;
            this.NameLabel.Text = "玩家名六个字";
            // 
            // ChangeNameButton
            // 
            this.ChangeNameButton.Font = new System.Drawing.Font("方正兰亭圆简体_中", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ChangeNameButton.Location = new System.Drawing.Point(97, 53);
            this.ChangeNameButton.Margin = new System.Windows.Forms.Padding(2);
            this.ChangeNameButton.Name = "ChangeNameButton";
            this.ChangeNameButton.Size = new System.Drawing.Size(120, 32);
            this.ChangeNameButton.TabIndex = 3;
            this.ChangeNameButton.Text = "更改昵称";
            this.ChangeNameButton.UseVisualStyleBackColor = true;
            // 
            // HeaderLabel
            // 
            this.HeaderLabel.AutoSize = true;
            this.HeaderLabel.BackColor = System.Drawing.Color.Transparent;
            this.HeaderLabel.Font = new System.Drawing.Font("方正兰亭圆简体_中", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HeaderLabel.ForeColor = System.Drawing.Color.Transparent;
            this.HeaderLabel.Location = new System.Drawing.Point(10, 148);
            this.HeaderLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.HeaderLabel.Name = "HeaderLabel";
            this.HeaderLabel.Size = new System.Drawing.Size(150, 27);
            this.HeaderLabel.TabIndex = 4;
            this.HeaderLabel.Text = "选择一个头像";
            // 
            // PictureBox1
            // 
            this.PictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox1.Location = new System.Drawing.Point(27, 90);
            this.PictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(128, 52);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox1.TabIndex = 5;
            this.PictureBox1.TabStop = false;
            // 
            // PictureBox2
            // 
            this.PictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox2.Location = new System.Drawing.Point(170, 90);
            this.PictureBox2.Margin = new System.Windows.Forms.Padding(2);
            this.PictureBox2.Name = "PictureBox2";
            this.PictureBox2.Size = new System.Drawing.Size(134, 52);
            this.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox2.TabIndex = 6;
            this.PictureBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::HomeSettings.Properties.Resources.MuMu12_20240723_162110_4K;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(336, 504);
            this.Controls.Add(this.PictureBox2);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.HeaderLabel);
            this.Controls.Add(this.ChangeNameButton);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.CurrentHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "鸡蛋鸭蛋荷包蛋";
            this.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.header12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.header9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.header11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.header10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.header6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.header5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.header4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.header8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.header7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.header3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.header2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.header1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Panel1;
        private System.Windows.Forms.PictureBox header1;
        private System.Windows.Forms.OpenFileDialog SelectCustomHeaderDialog;
        private System.Windows.Forms.PictureBox CurrentHeader;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Button ChangeNameButton;
        private System.Windows.Forms.Label HeaderLabel;
        private System.Windows.Forms.PictureBox PictureBox1;
        private System.Windows.Forms.PictureBox PictureBox2;
        private System.Windows.Forms.PictureBox header6;
        private System.Windows.Forms.PictureBox header5;
        private System.Windows.Forms.PictureBox header4;
        private System.Windows.Forms.PictureBox CustomHeader;
        private System.Windows.Forms.PictureBox header8;
        private System.Windows.Forms.PictureBox header7;
        private System.Windows.Forms.PictureBox header3;
        private System.Windows.Forms.PictureBox header2;
        private System.Windows.Forms.PictureBox header9;
        private System.Windows.Forms.PictureBox header11;
        private System.Windows.Forms.PictureBox header10;
        private System.Windows.Forms.PictureBox header12;
    }
}