// OpenEggyUI 是一个开源的 Windows 桌面美化组件项目，旨在延续 EggyUI 的精神，提供轻量化、可自由组合的美化工具。
// 此代码由冷情镜像站编写。
// 本项目仅用于学习与交流，严禁将 OpenEggyUI 及其组件用于任何商业用途。

using HomeSettings.Properties;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace HomeSettings
{
    public partial class SettingsWindow : Form
    {
        private int _lastBackgroundDpi = -1;
        private static readonly string CustomFontName = "方正兰亭圆简体_粗";
        private static bool? _isCustomFontAvailable;

        public SettingsWindow()
        {
            InitializeComponent();
            CreateAvatarFolder();

            this.HandleCreated += SettingsWindow_HandleCreated;
            this.FormClosed += SettingsWindow_FormClosed;
        }

        #region 窗口方法

        /// <summary>
        /// 应用窗口主题
        /// </summary>
        private void ApplyTheme()
        {
            if (!this.IsHandleCreated)
            {
                return;
            }

            _isCustomFontAvailable ??= FontCache.IsFontInstalled(CustomFontName);

            if (_isCustomFontAvailable.Value)
            {
                foreach (var control in this.Controls.OfType<Control>())
                {
                    control.Font = FontCache.GetFont(CustomFontName, control.Font.Size, FontStyle.Regular);
                }
            }

            this.BackColor = AppStatus.IsDarkModeAndWin11 ? SystemColors.Window : Color.FromArgb(11, 148, 255);
            AvatarListView.BackColor = AppStatus.IsDarkModeAndWin11 ? SystemColors.Window : Color.FromArgb(15, 108, 230);
            AvatarListView.ForeColor = AppStatus.IsDarkModeAndWin11 ? SystemColors.WindowText : Color.White;

            SetBackgroundImage(this.DeviceDpi);
        }

        /// <summary>
        /// 设置窗口背景
        /// </summary>
        /// <param name="dpi">当前 DPI</param>
        private void SetBackgroundImage(int dpi)
        {
            if (_lastBackgroundDpi == dpi)
            {
                return;
            }

            _lastBackgroundDpi = dpi;

            try
            {
                this.BackgroundImage = AppStatus.IsDarkModeAndWin11 ? dpi switch
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
        private async Task InitializeEggyRankAsync()
        {
            string eggyDataXmlFilePath = Path.Combine(AppStatus.AppPath, "EggyData.xml");

            try
            {
                var eggyRank = await Task.Run(() => new EggyRank(eggyDataXmlFilePath));
                UpdateLevelLabel(eggyRank);
            }
            catch (FileNotFoundException)
            {
                var eggyRank = new EggyRank();
                UpdateLevelLabel(eggyRank);
            }
            catch (Exception ex)
            {
                ShowError(ex, "段位信息加载");
                UpdateLevelLabelError();
            }
        }

        private void UpdateLevelLabel(EggyRank eggyRank)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => UpdateLevelLabel(eggyRank));
                return;
            }
            LevelLabel.Text = $"{eggyRank}\nLv.{eggyRank.CurrentRank.Level}";
        }

        private void UpdateLevelLabelError()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(UpdateLevelLabelError);
                return;
            }
            LevelLabel.Text = "段位信息加载失败";
        }

        private static void ShowError(Exception ex, string failActionTitle)
        {
            MessageBox.Show(
                $"{failActionTitle}发生异常：{ex.GetType().Name}\r\n异常信息：{ex.Message}\r\n异常堆栈：{ex.StackTrace}",
                $"{failActionTitle}错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 加载头像图片
        /// </summary>
        private async void LoadAvatarImage()
        {
            ClearAvatarPictureBox();

            try
            {
                var image = await ImageCache.LoadImageAsync(AppStatus.AvatarImageFilePath);
                Size imageSize = image.Size;

                if (imageSize is { IsEmpty: false, Height: <= 512, Width: <= 512 })
                {
                    SetAvatarPictureBoxImage(image);
                }
                else
                {
                    image.Dispose();
                    throw new ArgumentOutOfRangeException(nameof(imageSize), "头像图片的尺寸必须在 512x512 范围内。");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "头像加载");
            }
        }

        private void ClearAvatarPictureBox()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(ClearAvatarPictureBox);
                return;
            }
            AvatarPictureBox.Image?.Dispose();
            AvatarPictureBox.Image = null;
        }

        private void SetAvatarPictureBoxImage(Image image)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => SetAvatarPictureBoxImage(image));
                return;
            }
            AvatarPictureBox.Image = image;
        }

        /// <summary>
        /// 更换头像
        /// </summary>
        /// <param name="sourceAvatarFileName">目标头像文件文件名</param>
        private void ChangeAvatar(string sourceAvatarFileName)
        {
            string sourceAvatarFilePath = Path.Combine(AppStatus.AvatarFolderPath, sourceAvatarFileName);

            try
            {
                File.Copy(sourceAvatarFilePath, AppStatus.AvatarImageFilePath, true);
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
        private async void LoadAvatarImageList()
        {
            ClearAvatarList();

            try
            {
                var items = await LoadAvatarImagesFromDirectoryAsync();
                AddAvatarItemsToListView(items);
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
            if (this.InvokeRequired)
            {
                this.Invoke(ClearAvatarList);
                return;
            }
            AvatarListView.Items.Clear();
            AvatarImageList.Images.Clear();
        }

        /// <summary>
        /// 从目录加载头像图片
        /// </summary>
        private async Task<List<(string FileName, Image Image)>> LoadAvatarImagesFromDirectoryAsync()
        {
            string[] imageFiles = GetAvatarImageFiles();
            var items = new List<(string FileName, Image Image)>();

            foreach (string filePath in imageFiles)
            {
                var item = await LoadSingleAvatarImageAsync(filePath);
                if (item is not null)
                {
                    items.Add(item.Value);
                }
            }

            return items;
        }

        /// <summary>
        /// 批量添加头像到 ListView
        /// </summary>
        private void AddAvatarItemsToListView(IEnumerable<(string FileName, Image Image)> items)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => AddAvatarItemsToListView(items));
                return;
            }

            foreach (var (fileName, image) in items)
            {
                AvatarImageList.Images.Add(fileName, image);
                ListViewItem item = new ListViewItem(fileName) { ImageKey = fileName };
                AvatarListView.Items.Add(item);
            }
        }

        /// <summary>
        /// 获取头像图片文件列表
        /// </summary>
        private static string[] GetAvatarImageFiles()
        {
            return [ .. from file in Directory.GetFiles(AppStatus.AvatarFolderPath)
                where Path.GetExtension(file).Equals(".png", StringComparison.OrdinalIgnoreCase)
                orderby file
                select file ];
        }

        /// <summary>
        /// 加载单个头像图片
        /// </summary>
        private async Task<(string FileName, Image Image)?> LoadSingleAvatarImageAsync(string filePath)
        {
            string fileName = Path.GetFileName(filePath);

            var image = await TryLoadImageAsync(filePath);
            return image is null ? null : (fileName, image);
        }

        /// <summary>
        /// 尝试加载图片
        /// </summary>
        private async Task<Image?> TryLoadImageAsync(string filePath)
        {
            try
            {
                return await ImageCache.LoadImageAsync(filePath);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 打开头像文件夹
        /// </summary>
        private static void OpenAvatarFolder()
        {
            try
            {
                Process.Start(new ProcessStartInfo(AppStatus.AvatarFolderPath)
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
        private static void RunAvatarMaker()
        {
            string avatarMakerPath = Path.Combine(AppStatus.AppPath, "AvatarMaker.exe");

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
        private static void CreateAvatarFolder()
        {
            try
            {
                // 如果头像目录不存在则创建头像目录
                if (!Directory.Exists(AppStatus.AvatarFolderPath))
                {
                    Directory.CreateDirectory(AppStatus.AvatarFolderPath);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "头像目录创建");
            }
        }
        
        #endregion

        #region 窗口事件处理程序

        private void SettingsWindow_FormClosed(object? sender, FormClosedEventArgs e)
        {
            ImageCache.Clear();
            FontCache.Clear();
        }

        private void SettingsWindow_HandleCreated(object? sender, EventArgs e)
        {
            ApplyTheme();
        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var tasks = new List<Task>
            {
                InitializeEggyRankAsync(),
                LoadAvatarImageAsync(),
                LoadAvatarImageListAsync()
            };

            await Task.WhenAll(tasks);
        }

        private async Task LoadAvatarImageAsync()
        {
            await Task.Run(LoadAvatarImage);
        }

        private async Task LoadAvatarImageListAsync()
        {
            await Task.Run(LoadAvatarImageList);
        }

        private void SettingsWindow_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            SetBackgroundImage(e.DeviceDpiNew);
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
            _ = RefreshAvatarListAsync();
        }

        private async Task RefreshAvatarListAsync()
        {
            ClearAvatarList();
            var items = await LoadAvatarImagesFromDirectoryAsync();
            AddAvatarItemsToListView(items);
        }

        private void RefreshRankInfoButton_Click(object sender, EventArgs e)
        {
            _ = InitializeEggyRankAsync();
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

        #endregion
    }
}
