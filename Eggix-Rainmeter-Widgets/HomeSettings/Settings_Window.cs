// ============================================================================
// 模块名称: Settings_Window
// 创建者: SYSTEM-LIGHT
// 项目: Eggix - 《蛋仔派对》风格桌面组件
// 描述: 
// - 此代码文件为《蛋仔派对》风格桌面组件的主窗口类，负责处理用户交互，
//   并与其他组件协调工作。
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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HomeSettings.Tools;

namespace HomeSettings
{
    public partial class Settings_Window : Form
    {
        #region 初始化功能

        /// <summary>
        /// 构造函数。
        /// </summary>
        public Settings_Window()
        {
            // 初始化组件。
            InitializeComponent();

            // 初始化背景图片。
            InitializeBackgroundImage();

            // 初始化事件处理程序。
            InitializeEventHandler();

            // 初始化时加载当前昵称
            LoadCurrentName();
            
            // 初始化时加载当前头像
            LoadCurrentHeader();
        }

        /// <summary>
        /// 初始化事件处理程序。
        /// </summary>
        private void InitializeEventHandler()
        {
            try
            {
                Logger.Default.WriteInfoLog("开始初始化事件处理程序", "初始化");

                // 验证ChangeNameButton控件
                Logger.Default.WriteDebugLog("准备绑定ChangeNameButton的Click事件到ChangeNameButton_Click处理程序", "初始化");
                if (ChangeNameButton != null && !ChangeNameButton.IsDisposed)
                {
                    ChangeNameButton.Click += ChangeNameButton_Click;
                    Logger.Default.WriteDebugLog("ChangeNameButton的Click事件已成功绑定到ChangeNameButton_Click处理程序", "初始化");
                }
                else
                {
                    Logger.Default.WriteWarningLog("ChangeNameButton控件为空或已释放，跳过事件注册", "初始化");
                }

                // 验证并注册header控件事件处理程序
                var headerControls = new List<HeaderControlInfo>
                {
                    new HeaderControlInfo { Control = header1, Name = "header1" },
                    new HeaderControlInfo { Control = header2, Name = "header2" },
                    new HeaderControlInfo { Control = header3, Name = "header3" },
                    new HeaderControlInfo { Control = header4, Name = "header4" },
                    new HeaderControlInfo { Control = header5, Name = "header5" },
                    new HeaderControlInfo { Control = header6, Name = "header6" },
                    new HeaderControlInfo { Control = header7, Name = "header7" },
                    new HeaderControlInfo { Control = header8, Name = "header8" },
                    new HeaderControlInfo { Control = header9, Name = "header9" },
                    new HeaderControlInfo { Control = header10, Name = "header10" },
                    new HeaderControlInfo { Control = header11, Name = "header11" },
                    new HeaderControlInfo { Control = header12, Name = "header12" }
                };

                Logger.Default.WriteDebugLog($"准备绑定 {headerControls.Count} 个头像控件的Click事件到PresentHeader_Click处理程序", "初始化");
                
                int successCount = 0;
                int failureCount = 0;

                foreach (var headerControl in headerControls)
                {
                    try
                    {
                        Logger.Default.WriteDebugLog($"正在绑定控件 {headerControl.Name} 的Click事件到PresentHeader_Click处理程序", "初始化");
                        if (headerControl.Control != null && !headerControl.Control.IsDisposed)
                        {
                            headerControl.Control.Click += PresentHeader_Click;
                            successCount++;
                            Logger.Default.WriteDebugLog($"{headerControl.Name}的Click事件已成功绑定到PresentHeader_Click处理程序", "初始化");
                        }
                        else
                        {
                            failureCount++;
                            Logger.Default.WriteWarningLog($"{headerControl.Name}控件为空或已释放，跳过事件注册", "初始化");
                        }
                    }
                    catch (Exception ex)
                    {
                        failureCount++;
                        Logger.Default.WriteErrorLog($"注册{headerControl.Name}事件处理程序时发生错误：{ex.Message}", "初始化");
                    }
                }

                // 验证CustomHeader控件
                Logger.Default.WriteDebugLog("准备绑定CustomHeader的Click事件到CustomHeader_Click处理程序", "初始化");
                if (CustomHeader != null && !CustomHeader.IsDisposed)
                {
                    CustomHeader.Click += CustomHeader_Click;
                    Logger.Default.WriteDebugLog("CustomHeader的Click事件已成功绑定到CustomHeader_Click处理程序", "初始化");
                }
                else
                {
                    Logger.Default.WriteWarningLog("CustomHeader控件为空或已释放，跳过事件注册", "初始化");
                }

                Logger.Default.WriteInfoLog($"事件处理程序初始化完成，成功：{successCount}，失败：{failureCount}", "初始化");
                Logger.Default.WriteInfoLog("控件事件绑定初始化结束", "初始化");

                // 如果有失败的控件注册，显示警告
                if (failureCount > 0)
                {
                    Logger.Default.WriteWarningLog($"警告：有 {failureCount} 个控件事件注册失败", "初始化");
                }
            }
            catch (Exception ex)
            {
                Logger.Default.WriteErrorLog($"初始化事件处理程序时发生错误：{ex.Message}", "初始化");
                Logger.Default.WriteErrorLog($"堆栈跟踪：{ex.StackTrace}", "初始化");
                
                // 显示错误提示，但不阻止程序继续运行
                try
                {
                    MessageBox.Show("初始化事件处理程序时发生错误，部分功能可能无法正常使用。", "初始化警告",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception msgEx)
                {
                    Logger.Default.WriteErrorLog($"显示错误提示时发生错误：{msgEx.Message}", "初始化");
                }
            }
        }

        /// <summary>
        /// 头像控件信息类
        /// </summary>
        private class HeaderControlInfo
        {
            public PictureBox Control { get; set; }
            public string Name { get; set; }
        }

        /// <summary>
        /// 初始化背景图片。
        /// </summary>
        private void InitializeBackgroundImage()
        {
            try
            {
                Logger.Default.WriteInfoLog("开始初始化背景图片", "初始化");

                // 验证Screen.PrimaryScreen是否可用
                if (Screen.PrimaryScreen == null)
                {
                    Logger.Default.WriteWarningLog("无法获取主屏幕信息，使用默认背景图片", "初始化");
                    SetDefaultBackgroundImage();
                    return;
                }

                // 安全获取屏幕高度
                int screenHeight;
                try
                {
                    screenHeight = Screen.PrimaryScreen.Bounds.Height;
                    Logger.Default.WriteDebugLog($"检测到屏幕高度：{screenHeight}像素", "初始化");
                }
                catch (Exception ex)
                {
                    Logger.Default.WriteErrorLog($"获取屏幕高度时发生错误：{ex.Message}，使用默认背景图片", "初始化");
                    SetDefaultBackgroundImage();
                    return;
                }

                // 根据屏幕分辨率选择合适的背景图片
                // 考虑到目前8K显示器不仅价格高昂，并且与之适配的内容资源极为匮乏，
                // 在实际使用场景中，绝大多数用户的设备分辨率都在4K及以下，
                // 看起来几乎没人会使用8K屏幕，因此未对8K分辨率做额外处理。
                // 实际上，此代码是开发者在4K屏幕上进行开发的，因此对4K分辨率做了适配处理。
                Image backgroundImage = null;
                string resolutionType = "";

                try
                {
                    if (screenHeight >= 2160) // 4K分辨率
                    {
                        backgroundImage = Properties.Resources.MuMu12_20240723_162110_4K;
                        resolutionType = "4K";
                    }
                    else if (screenHeight >= 1440) // 2K分辨率
                    {
                        backgroundImage = Properties.Resources.MuMu12_20240723_162110_2K;
                        resolutionType = "2K";
                    }
                    else // 1080P及以下分辨率
                    {
                        backgroundImage = Properties.Resources.MuMu12_20240723_162110;
                        resolutionType = "1080P及以下";
                    }

                    // 验证获取的背景图片是否有效
                    if (backgroundImage != null)
                    {
                        BackgroundImage = backgroundImage;
                        Logger.Default.WriteInfoLog($"已设置{resolutionType}分辨率背景图片，尺寸：{backgroundImage.Width}x{backgroundImage.Height}", "初始化");
                    }
                    else
                    {
                        Logger.Default.WriteErrorLog($"获取{resolutionType}分辨率背景图片失败，使用默认背景图片", "初始化");
                        SetDefaultBackgroundImage();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Default.WriteErrorLog($"设置{resolutionType}分辨率背景图片时发生错误：{ex.Message}，使用默认背景图片", "初始化");
                    SetDefaultBackgroundImage();
                }
            }
            catch (Exception ex)
            {
                Logger.Default.WriteErrorLog($"初始化背景图片时发生错误：{ex.Message}", "初始化");
                Logger.Default.WriteErrorLog($"堆栈跟踪：{ex.StackTrace}", "初始化");
                
                // 尝试使用默认背景图片
                SetDefaultBackgroundImage();
                
                // 显示错误提示，但不阻止程序继续运行
                try
                {
                    MessageBox.Show("初始化背景图片时发生错误，将使用默认背景图片。", "初始化警告",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception msgEx)
                {
                    Logger.Default.WriteErrorLog($"显示错误提示时发生错误：{msgEx.Message}", "初始化");
                }
            }
        }

        /// <summary>
        /// 设置默认背景图片
        /// </summary>
        private void SetDefaultBackgroundImage()
        {
            try
            {
                // 尝试使用1080P及以下分辨率的图片作为默认背景
                Image defaultImage = Properties.Resources.MuMu12_20240723_162110;
                
                if (defaultImage != null)
                {
                    BackgroundImage = defaultImage;
                    Logger.Default.WriteInfoLog($"已设置默认背景图片，尺寸：{defaultImage.Width}x{defaultImage.Height}", "初始化");
                }
                else
                {
                    Logger.Default.WriteErrorLog("获取默认背景图片失败", "初始化");
                    
                    // 如果默认图片也无法获取，尝试创建一个纯色背景
                    try
                    {
                        Bitmap solidBackground = new Bitmap(1920, 1080);
                        using (Graphics graphics = Graphics.FromImage(solidBackground))
                        {
                            graphics.Clear(Color.DarkBlue);
                        }
                        BackgroundImage = solidBackground;
                        Logger.Default.WriteInfoLog("已创建纯色背景图片作为替代", "初始化");
                    }
                    catch (Exception ex)
                    {
                        Logger.Default.WriteErrorLog($"创建纯色背景图片时发生错误：{ex.Message}", "初始化");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Default.WriteErrorLog($"设置默认背景图片时发生错误：{ex.Message}", "初始化");
            }
        }

        #endregion
    }
}