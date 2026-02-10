// OpenEggyUI 是一个开源的 Windows 桌面美化组件项目，旨在延续 EggyUI 的精神，提供轻量化、可自由组合的美化工具。
// 此代码由冷情镜像站编写。
// 本项目仅用于学习与交流，严禁将 OpenEggyUI 及其组件用于任何商业用途。

using System.Data;
using System.Diagnostics;

namespace HomeSettings
{
    public partial class Settings_Window : Form
    {
        private readonly string appPath = AppDomain.CurrentDomain.BaseDirectory;
        private readonly string avatarFolderPath;
        private readonly string avatarImageFilePath;

        public Settings_Window()
        {
            InitializeComponent();

            ApplyTheme();

            avatarFolderPath = Path.Combine(appPath, "Avatars");
            avatarImageFilePath = Path.Combine(appPath, "avatar.png");

            CreateAvatarFolder();

            InitializeEggyRank();

            LoadAvatarImage();
            LoadAvatarImageList();
        }

        #region 窗口方法

        /// <summary>
        /// 应用窗口主题
        /// </summary>
        private void ApplyTheme()
        {
            try
            {
                if (DarkModeHelper.IsDarkModeEnabled())
                {
                    this.BackgroundImage = Properties.Resources.background_dark;
                    AvatarListView.BackColor = SystemColors.Window;
                    AvatarListView.ForeColor = SystemColors.WindowText;
                }
                else
                {
                    this.BackgroundImage = Properties.Resources.background;
                    AvatarListView.BackColor = Color.FromArgb(15, 108, 230);
                    AvatarListView.ForeColor = Color.White;
                }
            }
            catch
            {
                this.BackgroundImage = null;
                AvatarListView.BackColor = SystemColors.Window;

            }
        }

        /// <summary>
        /// 初始化蛋仔段位信息
        /// </summary>
        private void InitializeEggyRank()
        {
            string eggyDataXmlFilePath = Path.Combine(appPath, "EggyData.xml");

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
                using Image tempImage = Image.FromFile(avatarImageFilePath, true);

                if (tempImage.Width > 0 && tempImage.Width <= 512 &&
                    tempImage.Height > 0 && tempImage.Height <= 512)
                {
                    AvatarPictureBox.Image = new Bitmap(tempImage);
                }
                else
                {
                    ShowError(new Exception("头像图片的尺寸必须在512x512范围内。"), "头像加载");
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
            string sourceAvatarFilePath = Path.Combine(avatarFolderPath, sourceAvatarFileName);

            try
            {
                File.Copy(sourceAvatarFilePath, avatarImageFilePath, true);
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
            return Directory.GetFiles(avatarFolderPath)
                .Where(file => Path.GetExtension(file).Equals(".png", StringComparison.CurrentCultureIgnoreCase))
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
            ListViewItem item = new ListViewItem(fileName);
            item.ImageKey = fileName;
            AvatarListView.Items.Add(item);
        }

        /// <summary>
        /// 打开头像文件夹
        /// </summary>
        private void OpenAvatarFolder()
        {
            try
            {
                Process.Start(new ProcessStartInfo(avatarFolderPath)
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
            string avatarMakerPath = Path.Combine(appPath, "AvatarMaker.exe");

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
                if (!Directory.Exists(avatarFolderPath))
                {
                    Directory.CreateDirectory(avatarFolderPath);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "头像目录创建");
            }
        }

        #endregion

        #region 窗口事件处理程序

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
            if (AvatarListView.GetItemAt(e.X, e.Y) is ListViewItem { ImageKey: string imageKey })
            {
                // 获取选中的头像文件名
                string selectedAvatarFileName = imageKey;
                // 调用ChangeAvatar方法更换头像
                ChangeAvatar(selectedAvatarFileName);
            }
        }

        private void RefreshRankInfoButton_Click(object sender, EventArgs e)
        {
            InitializeEggyRank();
        }

        #endregion
    }
}
