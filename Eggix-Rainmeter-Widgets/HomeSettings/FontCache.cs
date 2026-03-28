using System.Collections.Concurrent;
using System.Drawing.Text;

namespace HomeSettings;

/// <summary>
/// 字体缓存类，提供字体加载和缓存功能
/// </summary>
public static class FontCache
{
    private static readonly ConcurrentDictionary<string, Font> _cache = new();
    private static readonly object _fontCollectionLock = new();
    private static FontFamily? _cachedFontFamily;
    private static bool _fontFamilyChecked;

    /// <summary>
    /// 获取缓存的字体
    /// </summary>
    /// <param name="fontName">字体名称</param>
    /// <param name="size">字体大小</param>
    /// <param name="style">字体样式</param>
    /// <returns>字体对象</returns>
    public static Font GetFont(string fontName, float size, FontStyle style = FontStyle.Regular)
    {
        string key = $"{fontName}_{size}_{style}";

        if (_cache.TryGetValue(key, out var cachedFont))
        {
            return cachedFont;
        }

        lock (_fontCollectionLock)
        {
            if (_cache.TryGetValue(key, out cachedFont))
            {
                return cachedFont;
            }

            var fontFamily = GetFontFamily(fontName);
            var font = fontFamily != null ? new Font(fontFamily, size, style) : new Font(fontName, size, style);
            _cache[key] = font;
            return font;
        }
    }

    /// <summary>
    /// 检查字体是否已安装
    /// </summary>
    /// <param name="fontName">字体名称</param>
    /// <returns>如果字体已安装返回 true</returns>
    public static bool IsFontInstalled(string fontName)
    {
        return GetFontFamily(fontName) != null;
    }

    /// <summary>
    /// 清除缓存
    /// </summary>
    public static void Clear()
    {
        foreach (var kvp in _cache)
        {
            kvp.Value.Dispose();
        }
        _cache.Clear();
        _cachedFontFamily = null;
        _fontFamilyChecked = false;
    }

    /// <summary>
    /// 清除指定字体的缓存
    /// </summary>
    /// <param name="fontName">字体名称</param>
    /// <param name="size">字体大小</param>
    /// <param name="style">字体样式</param>
    public static void Remove(string fontName, float size, FontStyle style = FontStyle.Regular)
    {
        string key = $"{fontName}_{size}_{style}";
        if (_cache.TryRemove(key, out var font))
        {
            font.Dispose();
        }
    }

    private static FontFamily? GetFontFamily(string fontName)
    {
        if (!_fontFamilyChecked)
        {
            lock (_fontCollectionLock)
            {
                if (!_fontFamilyChecked)
                {
                    using var installedFonts = new InstalledFontCollection();
                    var family = installedFonts.Families
                        .FirstOrDefault(f => f.Name.Equals(fontName, StringComparison.OrdinalIgnoreCase));
                    
                    if (family != null)
                    {
                        _cachedFontFamily = family;
                    }
                    _fontFamilyChecked = true;
                }
            }
        }

        return _cachedFontFamily;
    }
}
