using System;
using System.Collections.Generic;
using System.Text;

namespace HomeSettings;

internal static class AppStatus
{
    public static string AppPath => AppDomain.CurrentDomain.BaseDirectory;
    public static string AvatarFolderPath => Path.Combine(AppPath, "Avatars");
    public static string AvatarImageFilePath => Path.Combine(AppPath, "avatar.png");
    public static readonly bool _darkMode = DarkModeHelper.IsDarkModeEnabled();
    //public static bool IsDarkMode => _darkMode;
    public static bool IsDarkModeAndWin11 => _darkMode && SystemBuildVersion > 22000; // 临时属性
    public static int SystemBuildVersion => Environment.OSVersion.Version.Build;
}
