/*
 * ============================================================================
 * EggyUI - Windows 桌面美化主题包
 * 基于网易《蛋仔派对》UI风格的粉丝二次创作，非官方、非商业项目
 * ============================================================================
 *  
 * 作者: 冷落的小情绪 (SYSTEM-LIGHT)
 * 贡献者: EggyUI 开发团队 (https://github.com/SYSTEM-LIGHT/EggyUI)
 * 
 * 版权所有 (c) 2024-2025 EggyUI 开发团队
 * 
 * 许可协议:
 * 本项目为粉丝创作，严禁用于任何商业用途。
 * 所有素材均为合法获取或自主重绘，未使用任何游戏解包内容。
 * 
 * 免责声明:
 * 1. 本软件与微软、网易无关，Windows 和《蛋仔派对》分别为其所属公司的注册商标。
 * 2. 使用者需自行承担因使用本软件可能带来的风险。
 * 3. 禁止对本软件进行商业性使用、分发或集成至商业产品中。
 * 
 * ============================================================================
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ERWS
{
    public partial class Settings_Window : Form
    {
        #region 程序变量
        /// <summary>
        /// 头像文件。
        /// </summary>
        private const string HeaderFileName = "header.png";

        /// <summary>
        /// 配置文件。
        /// </summary>
        private const string ConfigFileName = "home.ini";

        /// <summary>
        /// 昵称所在行索引。
        /// </summary>
        private const int NameLineIndex = 35;

        /// <summary>
        /// home.ini默认内容。
        /// </summary>
        private static readonly string[] DefaultConfigLines =
        [
            "[Rainmeter]",
            "Update=1000",
            "Logging=0",
            "SkinPath=%USERPROFILE%\\Documents\\Rainmeter\\Skins\\",
            "",
            "[MeterLaunch1Image]",
            "Meter=Image",
            "ImageName=home.png",
            "W=285",
            "H=96",
            ";在W,H中设置图像的长和宽",
            "X=32",
            "Y=32",
            "LeftMouseUpAction=[\"%USERPROFILE%\\Documents\\Rainmeter\\Skins\\EggyUI\\Home\\ERWS.exe\"]",
            "",
            "[MeterLaunch1tx]",
            "Meter=Image",
            "ImageName=header.png",
            "W=75",
            "H=75",
            ";在W,H中设置图像的长和宽",
            "X=43",
            "Y=43",
            "LeftMouseUpAction=[\"%USERPROFILE%\\Documents\\Rainmeter\\Skins\\EggyUI\\Home\\ERWS.exe\"]",
            "",
            "[MeterLaunch1Text]",
            "Meter=String",
            "X=133",
            "Y=50",
            "FontFace=Impact",
            "FontSize=14",
            "FontColor=255,255,255,255",
            ";StringStyle=Bold",
            "SolidColor=0,0,0,1",
            "AntiAlias=1",
            "Text=Eggy",
            ";将Text=后面的内容修改为您的用户名",
            "LeftMouseUpAction=[\"%USERPROFILE%\\Documents\\Rainmeter\\Skins\\EggyUI\\Home\\ERWS.exe\"]",
            ""
        ];
        #endregion

        public Settings_Window() // 构造函数
        {
            InitializeComponent();
        }

        #region 头像更换功能
        /// <summary>
        /// 加载头像的方法。
        /// </summary>
        private void loadheader()
        {
            try
            {
                if (File.Exists(HeaderFileName))
                {
                    using var imageStream = File.OpenRead(HeaderFileName);
                    mainheader.Image = Image.FromStream(imageStream);
                }
                else
                {
                    mainheader.Image = null;
                }
            }
            catch (Exception ex)
            {
                ShowError("加载头像失败", ex);
                mainheader.Image = null;
            }
        }

        private bool TryChangeHeader(string sourceFile)
        {
            try
            {
                // 清理现有图像资源
                if (mainheader.Image != null)
                {
                    var oldImage = mainheader.Image;
                    mainheader.Image = null;
                    oldImage.Dispose();
                }

                // 安全复制文件
                File.Copy(sourceFile, HeaderFileName, true);
                loadheader();
                return true;
            }
            catch (Exception ex)
            {
                ShowError("更换头像失败", ex);
                return false;
            }
        }

        /// <summary>
        /// 更换自定义头像的方法。
        /// </summary>
        private void customheaderchange()
        {
            DialogResult result = MessageBox.Show(
                "自定义头像目前为实验性功能。\n头像图片要求：分辨率128x128，PNG格式，带Alpha通道，圆形边框\n你确定要更换自定义头像吗？",
                "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
            if (result != DialogResult.Yes) return;

            selectcustomheader.Title = "选择自定义头像";
            selectcustomheader.Filter = "PNG图片|*.png";
            selectcustomheader.FilterIndex = 1;
            selectcustomheader.RestoreDirectory = true;
            
            try
            {
                if (selectcustomheader.ShowDialog() == DialogResult.OK)
                {
                    if (TryChangeHeader(selectcustomheader.FileName))
                    {
                        MessageBox.Show("自定义头像更换成功，请刷新Rainmeter显示。", 
                            "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("自定义头像更换失败", ex);
            }
        }

        /// <summary>
        /// 更换预设头像的方法。
        /// </summary>
        /// <param name="sender">窗口上的预设头像控件。</param>
        private void presetheaderchange(object sender)
        {
            try
            {
                var clickedHeader = sender as PictureBox;
                if (clickedHeader == null) return;
                
                string headerFile = $"{clickedHeader.Name}.png";
                
                if (File.Exists(headerFile))
                {
                    if (TryChangeHeader(headerFile))
                    {
                        MessageBox.Show("头像更换成功，请刷新Rainmeter显示。",
                            "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show($"指定的预设头像文件不存在: {headerFile}", 
                        "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ShowError("更换预设头像失败", ex);
            }
        }

        private void Header_Click(object sender, EventArgs e)
        {
            presetheaderchange(sender);
        }

        private void customheader_Click(object sender, EventArgs e)
        {
            customheaderchange();
        }
        #endregion

        #region 用户昵称更改功能
        /// <summary>
        /// 从home.ini中加载昵称的方法。
        /// </summary>
        private void loadName()
        {
            try
            {
                // 确保配置文件存在
                EnsureConfigFileExists();
                
                var lines = File.ReadAllLines(ConfigFileName);
                if (lines.Length > NameLineIndex)
                {
                    string line = lines[NameLineIndex];
                    string name = line.StartsWith("Text=") 
                        ? line.Substring(5).Trim() 
                        : line.Trim();
                        
                    Label1.Text = name;
                }
            }
            catch (Exception ex)
            {
                ShowError("加载昵称失败", ex);
            }
        }

        /// <summary>
        /// 修改home.ini文件内写的昵称。
        /// </summary>
        private void changeName()
        {
            string newName = Microsoft.VisualBasic.Interaction.InputBox(
                "请输入新的昵称：", "更改昵称", "", -1, -1);
                
            if (string.IsNullOrWhiteSpace(newName)) return;

            bool success = false;
            try
            {
                // 确保配置文件存在
                EnsureConfigFileExists();
                
                // 读取配置文件为行数组
                var lines = new List<string>(File.ReadAllLines(ConfigFileName));

                // 确保行数足够
                while (lines.Count <= NameLineIndex)
                {
                    lines.Add("");
                }

                // 更新昵称行
                lines[NameLineIndex] = "Text=" + newName.Trim();

                // 使用File.Create创建文件
                using (FileStream fs = File.Create(ConfigFileName))
                using (StreamWriter writer = new(fs, Encoding.Unicode))
                {
                    foreach (string line in lines)
                    {
                        writer.WriteLine(line);
                    }
                }

                // 更新UI
                Label1.Text = newName.Trim();
                success = true;
            }
            catch (Exception ex)
            {
                ShowError("昵称更改失败", ex);
            }
            
            // 只有在操作成功时才显示成功消息
            if (success)
            {
                MessageBox.Show("昵称更改成功，请刷新Rainmeter显示。", 
                    "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void changeNameButton_Click(object sender, EventArgs e)
        {
            changeName();
        }
        #endregion

        #region 重置Rainmeter组件功能
        /// <summary>
        /// 将home.ini文件的内容改回默认的方法。
        /// </summary>
        private void ResetRainmeter()
        {
            bool success = false;
            try
            {
                // 创建默认配置文件
                CreateDefaultConfigFile();
                
                // 重置头像
                if (mainheader.Image != null)
                {
                    var oldImage = mainheader.Image;
                    mainheader.Image = null;
                    oldImage.Dispose();
                }
                
                // 使用默认头像
                if (File.Exists("header1.png"))
                {
                    TryChangeHeader("header1.png");
                }

                // 重新加载昵称
                loadName();
                success = true;
            }
            catch (Exception ex)
            {
                ShowError("重置组件失败", ex);
            }
            
            // 只有在操作成功时才显示成功消息
            if (success)
            {
                MessageBox.Show("重置此Rainmeter小组件成功！", 
                    "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ResetRainmeter();
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 显示错误。
        /// </summary>
        /// <param name="action">失败的操作。</param>
        /// <param name="ex">发生的异常。</param>
        private static void ShowError(string action, Exception ex)
        {
            MessageBox.Show(
                $"{action}，原因：\n{ex.Message}\n" +
                "如果你要反馈此问题，请将此错误消息截图并发送给@冷落的小情绪。",
                "操作失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 确保配置文件存在，如果不存在则创建默认配置
        /// </summary>
        private void EnsureConfigFileExists()
        {
            if (!File.Exists(ConfigFileName))
            {
                CreateDefaultConfigFile();
            }
        }

        /// <summary>
        /// 使用File.Create创建默认配置文件
        /// </summary>
        private void CreateDefaultConfigFile()
        {
            try
            {
                // 使用File.Create创建文件并写入内容
                using FileStream fs = File.Create(ConfigFileName);
                using StreamWriter writer = new(fs, Encoding.Unicode);
                foreach (string line in DefaultConfigLines)
                {
                    writer.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                ShowError("创建默认配置文件失败", ex);
            }
        }
        #endregion

        #region 窗体事件
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // 确保配置文件存在
                EnsureConfigFileExists();
                
                loadheader();
                loadName();
            }
            catch (Exception ex)
            {
                ShowError("初始化失败", ex);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("此功能暂未开放。", "提示", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
    }
}