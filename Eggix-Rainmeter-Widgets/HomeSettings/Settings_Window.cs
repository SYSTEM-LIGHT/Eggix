// OpenEggyUI 是一个开源的 Windows 桌面美化组件项目，旨在延续 EggyUI 的精神，提供轻量化、可自由组合的美化工具。
// 此代码由冷情镜像站编写。
// 本项目仅用于学习与交流，严禁将 OpenEggyUI 及其组件用于任何商业用途。

using System.Data;
using System.Diagnostics;
using System.Security;

namespace HomeSettings
{
    public partial class Settings_Window : Form
    {
        string appPath = AppDomain.CurrentDomain.BaseDirectory;
        string avatarFolderPath;
        string avatarImageFilePath;

        public Settings_Window()
        {
            InitializeComponent();

            ApplyTheme();

            avatarFolderPath = Path.Combine(appPath, "Avatars");
            avatarImageFilePath = Path.Combine(appPath, "avatar.png");

            CreateAvatarFolder();
            
            LoadAvatarImage();
            LoadAvatarImageList();
        }

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
                }
                else
                {
                    this.BackgroundImage = Properties.Resources.background;
                }
            }
            catch
            {
                this.BackgroundImage = null;
            }
        }

        /// <summary>
        /// 显示错误提示
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="failActionTitle">发生错误的操作标题</param>
        private void ShowError(string message, string failActionTitle)
        {
            MessageBox.Show(message, failActionTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 加载头像图片
        /// </summary>
        private void LoadAvatarImage()
        {
            // 释放头像控件的图片资源
            AvatarPictureBox.Image?.Dispose();
            AvatarPictureBox.Image = null;

            try
            {
                if (File.Exists(avatarImageFilePath))
                {
                    using Image tempImage = Image.FromFile(avatarImageFilePath, true);

                    // 判断图片尺寸是否满足在512x512范围内的要求
                    if (tempImage.Width > 0 && tempImage.Width <= 512 &&
                        tempImage.Height > 0 && tempImage.Height <= 512)
                    {
                        // 创建图片副本，避免文件被锁定
                        // 创建的Bitmap对象作为AvatarBox控件的图像，在刷新头像的时候会被Dispose掉
                        AvatarPictureBox.Image = new Bitmap(tempImage);
                    }
                    else
                    {
                        // 如果图片尺寸不满足要求
                        ShowError($"头像图片的尺寸必须在512x512范围内。", "头像加载失败");
                    }
                    // 对于简化的 using 声明（例如 using var imageStream = ...;），
                    // 资源会在当前作用域结束时自动释放，无需手动调用 Close() 或 Dispose()。
                }
                else
                {
                    // 当头像图片文件不存在时
                    ShowError($"找不到头像图片文件：{avatarImageFilePath}", "头像加载失败");
                }
            }
            catch (SecurityException)
            {
                // 捕获权限异常
                ShowError("尝试访问头像图片时遇到权限错误，请以管理员身份运行本程序！", "头像加载失败");
            }
            catch (IOException ioex)
            {
                // 捕获IO异常
                ShowError($"尝试访问头像图片文件时遇到文件IO错误：{ioex.Message}", "头像加载失败");
            }
            catch (OutOfMemoryException oomex)
            {
                // 捕获图像解码异常（不是真的内存不足）
                ShowError($"头像图片文件损坏或格式错误，无法正常解码！{oomex.Message}", "头像加载失败");
            }
            catch (Exception ex)
            {
                // 捕获其他异常
                ShowError($"尝试访问头像文件时遇到错误：{ex.Message}", "头像加载失败");
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
                if (File.Exists(sourceAvatarFilePath))
                {
                    File.Copy(sourceAvatarFilePath, avatarImageFilePath, true);
                    LoadAvatarImage();
                }
                else
                {
                    ShowError($"目标头像文件不存在：{avatarImageFilePath}", "头像更换失败");
                }
            }
            catch (IOException ioex)
            {
                ShowError($"尝试访问目标头像文件时遇到文件IO错误：{ioex.Message}", "头像加载失败");
            }
            catch (Exception ex)
            {
                ShowError($"尝试更换头像时遇到错误：{ex.Message}", "头像加载失败");
            }
        }

        /// <summary>
        /// 加载头像列表
        /// </summary>
        private void LoadAvatarImageList()
        {
            // 清空现有列表
            AvatarListView.Items.Clear();
            AvatarImageList.Images.Clear();

            try
            {
                if (Directory.Exists(avatarFolderPath))
                {
                    
                    // 获取文件夹中的所有PNG图片文件
                    string[] imageFiles = Directory.GetFiles(avatarFolderPath)
                        .Where(file => Path.GetExtension(file).Equals(
                            ".png", StringComparison.CurrentCultureIgnoreCase))
                        .OrderBy(file => file)
                        .ToArray();

                    // 遍历所有图片文件
                    foreach (string filePath in imageFiles)
                    {
                        string fileName = Path.GetFileName(filePath);

                        try
                        {
                            // 加载图片到imageList
                            using (Image tempImage = Image.FromFile(filePath))
                            {
                                // 创建图片副本，避免文件被锁定
                                Image imageCopy = new Bitmap(tempImage);
                                AvatarImageList.Images.Add(fileName, imageCopy);
                            }

                            // 创建ListViewItem
                            ListViewItem item = new ListViewItem(fileName);
                            item.ImageKey = fileName;
                            AvatarListView.Items.Add(item);
                        }
                        catch
                        {
                            // 忽略异常，继续处理下一个文件
                        }
                    }
                }
                else
                {
                    ShowError("头像文件夹不存在！", "头像列表加载失败");
                }
            }
            catch (SecurityException)
            {
                // 捕获权限异常
                ShowError("尝试访问头像文件夹时遇到权限错误，请以管理员身份运行本程序！", "头像列表加载失败");
            }
            catch (IOException ioex)
            {
                // 捕获IO异常
                ShowError($"尝试访问头像文件夹时遇到文件IO错误：{ioex.Message}", "头像列表加载失败");
            }
            catch (Exception ex)
            {
                // 捕获其他异常
                ShowError($"尝试加载头像列表时遇到错误：{ex.Message}", "头像列表加载失败");
            }
        }

        /// <summary>
        /// 打开头像文件夹
        /// </summary>
        private void OpenAvatarFolder()
        {
            // 打开头像目录
            try
            {
                if (Directory.Exists(avatarFolderPath))
                {
                    Process.Start(new ProcessStartInfo(avatarFolderPath)
                    {
                        UseShellExecute = true
                    });
                }
                else
                {
                    ShowError("头像文件夹不存在！", "头像文件夹打开失败");
                }
            }
            catch (Exception ex)
            {
                ShowError($"尝试打开头像文件夹时发生错误：{ex.Message}", "头像文件夹打开失败");
            }
        }

        /// <summary>
        /// 运行头像制作器
        /// </summary>
        private void RunAvatarMaker()
        {
            // 头像制作器暂未设计完成。
            string avatarMakerPath = Path.Combine(appPath, "AvatarMaker.exe");

            // 打开头像目录
            try
            {
                if (File.Exists(avatarMakerPath))
                {
                    Process.Start(new ProcessStartInfo(avatarMakerPath)
                    {
                        UseShellExecute = true
                    });
                }
                else
                {
                    ShowError("头像制作器文件不存在！", "头像制作器运行失败");
                }
            }
            catch (Exception ex)
            {
                ShowError($"尝试运行头像制作器时发生错误：{ex.Message}", "头像制作器运行失败");
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
            catch (SecurityException)
            {
                // 捕获权限异常
                ShowError("尝试创建头像目录时遇到权限错误，请以管理员身份运行本程序！", "头像目录创建失败");
            }
            catch (IOException ioex)
            {
                // 捕获IO异常
                ShowError($"尝试创建头像目录时遇到文件IO错误：{ioex.Message}", "头像目录创建失败");
            }
            catch (Exception ex)
            {
                // 捕获其他异常
                ShowError($"尝试创建头像目录时遇到错误：{ex.Message}", "头像目录创建失败");
            }
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
            if (AvatarListView.GetItemAt(e.X, e.Y) is ListViewItem { ImageKey: string imageKey })
            {
                // 获取选中的头像文件名
                string selectedAvatarFileName = imageKey;
                // 调用ChangeAvatar方法更换头像
                ChangeAvatar(selectedAvatarFileName);
            }
        }
    }
}
