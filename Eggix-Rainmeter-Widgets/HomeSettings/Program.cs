// OpenEggyUI 是一个开源的 Windows 桌面美化组件项目，旨在延续 EggyUI 的精神，提供轻量化、可自由组合的美化工具。
// 此代码由冷情镜像站编写。
// 本项目仅用于学习与交流，严禁将 OpenEggyUI 及其组件用于任何商业用途。

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HomeSettings
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(nint hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindowAsync(nint hWnd, int nCmdShow);

        private const int SW_SHOWNORMAL = 1;
        private const int SW_RESTORE = 9;
        private const string MutexName = @"Global\HomeSettings";

        /// <summary>
        /// 应用程序的主入口点
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Mutex? mutex = null;
            bool createdNew = false;

            try
            {
                mutex = new Mutex(true, MutexName, out createdNew);

                if (createdNew)
                {
                    RunApplication(mutex);
                }
                else
                {
                    ActivateExistingInstance();
                }
            }
            catch (AbandonedMutexException)
            {
                createdNew = true;
                RunApplication(mutex);
            }
            catch (UnauthorizedAccessException)
            {
                ShowError("无法访问系统资源，请以管理员身份运行应用程序。", "权限错误");
            }
            catch (Exception ex)
            {
                ShowError($"应用程序启动失败：{ex.Message}", "启动错误");
            }
            finally
            {
                if (createdNew)
                {
                    try
                    {
                        mutex?.ReleaseMutex();
                    }
                    catch { }
                    mutex?.Dispose();
                }
            }
        }

        /// <summary>
        /// 运行应用程序
        /// </summary>
        private static void RunApplication(Mutex? mutex)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                Application.ThreadException += Application_ThreadException;
                Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
                Application.SetColorMode(SystemColorMode.System);
                Application.EnableVisualStyles();

                try
                {
                    ApplicationConfiguration.Initialize();
                }
                catch (Exception ex)
                {
                    ShowError($"应用程序初始化失败：{ex.Message}", "初始化错误");
                    return;
                }

                Application.Run(new Settings_Window());
            }
            catch (Exception ex)
            {
                ShowError($"应用程序运行时发生错误：{ex.Message}", "运行时错误");
            }
        }

        /// <summary>
        /// 激活已存在的实例
        /// </summary>
        private static void ActivateExistingInstance()
        {
            try
            {
                Process current = Process.GetCurrentProcess();
                Process[] processes = Process.GetProcessesByName(current.ProcessName);

                if (processes.Length == 0)
                {
                    ShowError("未找到已运行的实例。", "实例查找错误");
                    return;
                }

                foreach (Process? process in processes)
                {
                    if (process is null || process.Id == current.Id)
                    {
                        continue;
                    }

                    try
                    {
                        if (process.MainWindowHandle != nint.Zero)
                        {
                            ShowWindowAsync(process.MainWindowHandle, SW_RESTORE);
                            SetForegroundWindow(process.MainWindowHandle);
                            break;
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"激活现有实例失败：{ex.Message}", "实例激活错误");
            }
        }

        /// <summary>
        /// 处理线程异常
        /// </summary>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            ShowError(
                $"发生线程异常：{ex.GetType().Name}\r\n异常信息：{ex.Message}\r\n异常堆栈：{ex.StackTrace}",
                "线程异常");
        }

        /// <summary>
        /// 处理全局异常
        /// </summary>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                ShowError(
                    $"发生全局异常：{ex.GetType().Name}\r\n异常信息：{ex.Message}\r\n异常堆栈：{ex.StackTrace}",
                    "全局异常");
            }
            else
            {
                ShowError(
                    $"发生未知错误：{e.ExceptionObject}",
                    "未知错误");
            }
        }

        /// <summary>
        /// 显示错误消息
        /// </summary>
        private static void ShowError(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
