// OpenEggyUI 是一个开源的 Windows 桌面美化组件项目，旨在延续 EggyUI 的精神，提供轻量化、可自由组合的美化工具。
// 此代码由冷情镜像站编写。
// 本项目仅用于学习与交流，严禁将 OpenEggyUI 及其组件用于任何商业用途。

using Microsoft.Win32;

namespace HomeSettings
{
    public static class DarkModeHelper
    {
        /// <summary>
        /// 检查当前是否启用深色模式
        /// </summary>
        public static bool IsDarkModeEnabled()
        {
            try
            {
                // Windows 10/11
                using (var key = Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    if (key?.GetValue("AppsUseLightTheme") is int lightTheme)
                    {
                        return lightTheme == 0;
                    }
                }
            }
            catch
            {
                // 注册表访问失败
            }

            return false;
        }
    }
}
