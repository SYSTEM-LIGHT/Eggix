// OpenEggyUI 是一个开源的 Windows 桌面美化组件项目，旨在延续 EggyUI 的精神，提供轻量化、可自由组合的美化工具。
// 此代码由冷情镜像站编写。
// 本项目仅用于学习与交流，严禁将 OpenEggyUI 及其组件用于任何商业用途。

using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace HomeSettings;

public static partial class DarkModeHelper
{
    /// <summary>
    /// 检查当前是否启用深色模式
    /// </summary>
    public static bool IsDarkModeEnabled()
    {
        try
        {
            // Windows 10/11
            using var key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            if (key?.GetValue("AppsUseLightTheme") is int lightTheme)
            {
                return lightTheme == 0;
            }
        }
        catch
        {
            // 注册表访问失败
        }

        return false;
    }

    // 对于Windows 10系统的深色模式支持尚未完成。目前仅有Windows 11系统支持深色模式。

    /*
    [LibraryImport("dwmapi.dll")]
    public static partial int DwmSetWindowAttribute(nint hwnd, int dwAttribute, ref int pvAttribute, int cbAttribute);

    public static void ApplyTitleBarDarkMode(nint hwnd)
    {
        if (AppStatus.IsDarkMode && Environment.OSVersion.Version.Build is >= 17763 and < 22000)
        {
            int useDarkMode = AppStatus.IsDarkMode ? 1 : 0;
            DwmSetWindowAttribute(hwnd, 20, ref useDarkMode, sizeof(int));

            int captionColor = 0x2B2B2B;
            DwmSetWindowAttribute(hwnd, 35, ref captionColor, sizeof(int));
        }
    }

    public static void ApplyControlDarkMode(Form form)
    {
        form.BackColor = Environment.OSVersion.Version.Build switch
        {
            >= 17763 and < 22000 => SystemColors.ControlDarkDark,
            > 22000 => SystemColors.Control,
            _ => Color.FromArgb(242, 234, 96)
        };
    }
    */
}