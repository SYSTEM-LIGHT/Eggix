// ============================================================================
// 模块名称: Program
// 创建者: SYSTEM-LIGHT
// 项目: Eggix - 《蛋仔派对》风格桌面组件
// 描述: 
// - 此代码文件为程序启动类，负责初始化高DPI感知设置，处理异常，
//   并启动《蛋仔派对》风格桌面组件的主窗口。
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
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using HomeSettings.Tools;

namespace HomeSettings
{
    /// <summary>
    /// 程序启动类
    /// </summary>
    internal static class Program
    {
        // 导入Windows API函数以设置DPI感知
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // 防御性编程：检查是否已设置高DPI感知模式
                try
                {
                    // 设置高DPI感知模式
                    if (Environment.OSVersion.Version.Major >= 6)
                    {
                        if (!SetProcessDPIAware())
                        {
                            // 如果设置失败，抛出Win32异常
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                    }
                }
                catch (Win32Exception winex)
                {
                    Logger.Default.WriteErrorLog($"设置高DPI状态时发生异常：{winex.Message}", "程序初始化");
                }
                catch (Exception ex)
                {
                    Logger.Default.WriteErrorLog($"设置高DPI状态时发生异常：{ex.Message}", "程序初始化");
                }

                // 设置应用程序的默认字体以确保在高DPI下正确显示
                Application.SetCompatibleTextRenderingDefault(false);
                Application.EnableVisualStyles();

                // 设置应用程序的高DPI自动缩放
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                // 启动主窗口
                Application.Run(new Settings_Window());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"应用程序启动时发生错误：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
