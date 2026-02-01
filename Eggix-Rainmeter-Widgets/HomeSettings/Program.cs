// OpenEggyUI 是一个开源的 Windows 桌面美化组件项目，旨在延续 EggyUI 的精神，提供轻量化、可自由组合的美化工具。
// 此代码由冷情镜像站编写。
// 本项目仅用于学习与交流，严禁将 OpenEggyUI 及其组件用于任何商业用途。

namespace HomeSettings
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点
        /// </summary>
        [STAThread]
        public static int Main()
        {
            try
            {
                // 设置高DPI模式
                Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

                // 根据系统深色模式状态切换应用深色模式状态
                Application.SetColorMode(SystemColorMode.System);

                // 初始化应用程序配置
                ApplicationConfiguration.Initialize();

                // 运行应用程序
                Application.Run(new Settings_Window());
            }
            catch (Exception ex)
            {
                // 显示错误消息
                MessageBox.Show($"应用程序启动时遇到错误：\n{ex}", "应用程序错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }

            return 0;
        }
    }
}