// OpenEggyUI 是一个开源的 Windows 桌面美化组件项目，旨在延续 EggyUI 的精神，提供轻量化、可自由组合的美化工具。
// 此代码由冷情镜像站编写。
// 本项目仅用于学习与交流，严禁将 OpenEggyUI 及其组件用于任何商业用途。

using System.Diagnostics;
using System.Drawing.Text;
using HomeSettings.Properties;

namespace HomeSettings
{
    public partial class SettingsWindow : Form
    {
        private readonly string _appPath = AppDomain.CurrentDomain.BaseDirectory;
        private readonly string _avatarFolderPath;
        private readonly string _avatarImageFilePath;
        private readonly bool _darkMode = DarkModeHelper.IsDarkModeEnabled();

        public SettingsWindow()
        {
            InitializeComponent();
            
            _avatarFolderPath = Path.Combine(_appPath, "Avatars");
            _avatarImageFilePath = Path.Combine(_appPath, "avatar.png");
            
            CreateAvatarFolder();
        }

        #region 窗口方法

        /// <summary>
        /// 应用窗口主题
        /// </summary>
        private void ApplyTheme(bool darkMode)
        {
            const string fontName = "方正兰亭圆简体_粗";
            using (var installedFonts = new InstalledFontCollection())
            {
                if (installedFonts.Families.Any(f => f.Name.Equals(fontName, StringComparison.OrdinalIgnoreCase)))
                {
                    foreach (var control in this.Controls.OfType<Control>())
                    {
                        float fontSize = control.Font.Size;
                        control.Font.Dispose();
                        control.Font = new Font(fontName, fontSize, FontStyle.Regular);
                    }
                }
            }
            
            this.BackColor = darkMode ? SystemColors.Window : Color.FromArgb(11, 148, 255);
            AvatarListView.BackColor = darkMode ? SystemColors.Window : Color.FromArgb(15, 108, 230);
            AvatarListView.ForeColor = darkMode ? SystemColors.WindowText : Color.White;
            
            SetBackgroundImage(this.DeviceDpi, darkMode);
        }

        /// <summary>
        /// 设置窗口背景
        /// </summary>
        /// <param name="dpi">当前DPI</param>
        /// <param name="darkMode">深色模式状态</param>
        private void SetBackgroundImage(int dpi, bool darkMode)
        {
            try
            {
                this.BackgroundImage = darkMode ? dpi switch
                    {
                        >= 96 and < 144 => Resources.background_dark_540P,
                        >= 144 and < 192 => Resources.background_dark_720P,
                        _ => Resources.background_dark
                    }
                    : dpi switch
                    {
                        >= 96 and < 144 => Resources.background_540P,
                        >= 144 and < 192 => Resources.background_720P,
                        _ => Resources.background
                    };
            }
            catch
            {
                this.BackgroundImage = null;
            }
        }

        /// <summary>
        /// 初始化蛋仔段位信息
        /// </summary>
        private void InitializeEggyRank()
        {
            string eggyDataXmlFilePath = Path.Combine(_appPath, "EggyData.xml");

            try
            {
                EggyRank eggyRank = new EggyRank(eggyDataXmlFilePath);
                LevelLabel.Text = $"{eggyRank}\nLv.{eggyRank.CurrentRank.Level}";
            }
            catch (FileNotFoundException)
            {
                EggyRank eggyRank = new EggyRank();
                LevelLabel.Text = $"{eggyRank}\nLv.{eggyRank.CurrentRank.Level}";
            }
            catch (Exception ex)
            {
                ShowError(ex, "段位信息加载");
                LevelLabel.Text = "段位信息加载失败";
            }
        }

        private void ShowError(Exception ex, string failActionTitle)
        {
            MessageBox.Show(
                $"{failActionTitle}发生异常：{ex.GetType().Name}\r\n异常信息：{ex.Message}\r\n异常堆栈：{ex.StackTrace}",
                $"{failActionTitle}错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 加载头像图片
        /// </summary>
        private void LoadAvatarImage()
        {
            AvatarPictureBox.Image?.Dispose();
            AvatarPictureBox.Image = null;

            try
            {
                using Image tempImage = Image.FromFile(_avatarImageFilePath, true);
                Size imageSize = tempImage.Size;

                if (imageSize is { IsEmpty: false, Height: <= 512, Width: <= 512 })
                {
                    AvatarPictureBox.Image = new Bitmap(tempImage);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(imageSize), "头像图片的尺寸必须在512x512范围内。");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "头像加载");
            }
        }

        /// <summary>
        /// 更换头像
        /// </summary>
        /// <param name="sourceAvatarFileName">目标头像文件文件名</param>
        private void ChangeAvatar(string sourceAvatarFileName)
        {
            string sourceAvatarFilePath = Path.Combine(_avatarFolderPath, sourceAvatarFileName);

            try
            {
                File.Copy(sourceAvatarFilePath, _avatarImageFilePath, true);
                LoadAvatarImage();
            }
            catch (Exception ex)
            {
                ShowError(ex, "头像更换");
            }
        }

        /// <summary>
        /// 加载头像列表
        /// </summary>
        private void LoadAvatarImageList()
        {
            ClearAvatarList();

            try
            {
                LoadAvatarImagesFromDirectory();
            }
            catch (Exception ex)
            {
                ShowError(ex, "头像列表加载");
            }
        }

        /// <summary>
        /// 清空头像列表
        /// </summary>
        private void ClearAvatarList()
        {
            AvatarListView.Items.Clear();
            AvatarImageList.Images.Clear();
        }

        /// <summary>
        /// 从目录加载头像图片
        /// </summary>
        private void LoadAvatarImagesFromDirectory()
        {
            string[] imageFiles = GetAvatarImageFiles();

            foreach (string filePath in imageFiles)
            {
                LoadSingleAvatarImage(filePath);
            }
        }

        /// <summary>
        /// 获取头像图片文件列表
        /// </summary>
        private string[] GetAvatarImageFiles()
        {
            return Directory.GetFiles(_avatarFolderPath)
                .Where(file => Path.GetExtension(file).Equals(".png", StringComparison.OrdinalIgnoreCase))
                .OrderBy(file => file)
                .ToArray(); 
        }

        /// <summary>
        /// 加载单个头像图片
        /// </summary>
        private void LoadSingleAvatarImage(string filePath)
        {
            string fileName = Path.GetFileName(filePath);

            if (!TryLoadImageToList(filePath, fileName))
            {
                return;
            }

            AddAvatarListViewItem(fileName);
        }

        /// <summary>
        /// 尝试加载图片到ImageList
        /// </summary>
        private bool TryLoadImageToList(string filePath, string fileName)
        {
            try
            {
                using Image tempImage = Image.FromFile(filePath);
                Image imageCopy = new Bitmap(tempImage);
                AvatarImageList.Images.Add(fileName, imageCopy);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 添加头像到ListView
        /// </summary>
        private void AddAvatarListViewItem(string fileName)
        {
            ListViewItem item = new ListViewItem(fileName)
            {
                ImageKey = fileName
            };
            AvatarListView.Items.Add(item);
        }

        /// <summary>
        /// 打开头像文件夹
        /// </summary>
        private void OpenAvatarFolder()
        {
            try
            {
                Process.Start(new ProcessStartInfo(_avatarFolderPath)
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "头像文件夹打开");
            }
        }

        /// <summary>
        /// 运行头像制作器
        /// </summary>
        private void RunAvatarMaker()
        {
            string avatarMakerPath = Path.Combine(_appPath, "AvatarMaker.exe");

            try
            {
                Process.Start(new ProcessStartInfo(avatarMakerPath)
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                ShowError(ex, "头像制作器运行");
            }
        }

        /// <summary>
        /// 创建头像文件夹
        /// </summary>
        private void CreateAvatarFolder()
        {
            try
            {
                // 如果头像目录不存在则创建头像目录
                if (!Directory.Exists(_avatarFolderPath))
                {
                    Directory.CreateDirectory(_avatarFolderPath);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "头像目录创建");
            }
        }

        #endregion

        #region 窗口事件处理程序

        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(() =>
            {
                ApplyTheme(_darkMode);

                InitializeEggyRank();

                LoadAvatarImage();
                LoadAvatarImageList();
            });

        }

        private void SettingsWindow_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            SetBackgroundImage(e.DeviceDpiNew, _darkMode);
        }

        private void OpenAvatarFolderButton_Click(object sender, EventArgs e)
        {
            OpenAvatarFolder();
        }

        private void RunAvatarMakerButton_Click(object sender, EventArgs e)
        {
            RunAvatarMaker();
        }

        private void RefreshAvatarButton_Click(object sender, EventArgs e)
        {
            LoadAvatarImage();
        }

        private void RefreshAvatarListButton_Click(object sender, EventArgs e)
        {
            LoadAvatarImageList();
        }

        private void AvatarListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 使用模式匹配减少空引用检查
            if (AvatarListView.GetItemAt(e.X, e.Y) is { ImageKey: { } imageKey })
            {
                // 获取选中的头像文件名
                // 调用ChangeAvatar方法更换头像
                ChangeAvatar(imageKey);
            }
        }

        private void RefreshRankInfoButton_Click(object sender, EventArgs e)
        {
            InitializeEggyRank();
        }

        #endregion
    }
}
