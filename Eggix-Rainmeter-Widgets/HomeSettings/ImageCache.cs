using System.Collections.Concurrent;

namespace HomeSettings;

/// <summary>
/// 图片缓存类，提供图片加载和缓存功能
/// </summary>
public static class ImageCache
{
    private static readonly ConcurrentDictionary<string, Image> _cache = new();
    private static readonly SemaphoreSlim _semaphore = new(5, 5);

    /// <summary>
    /// 从文件加载图片，使用缓存
    /// </summary>
    /// <param name="filePath">图片文件路径</param>
    /// <returns>加载的图片对象</returns>
    public static async Task<Image> LoadImageAsync(string filePath)
    {
        if (_cache.TryGetValue(filePath, out var cachedImage))
        {
            return CloneImage(cachedImage);
        }

        await _semaphore.WaitAsync();
        try
        {
            if (_cache.TryGetValue(filePath, out cachedImage))
            {
                return CloneImage(cachedImage);
            }

            var image = await Task.Run(() =>
            {
                using var tempImage = Image.FromFile(filePath, true);
                return new Bitmap(tempImage);
            });

            _cache[filePath] = image;
            return CloneImage(image);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// 同步加载图片，使用缓存
    /// </summary>
    /// <param name="filePath">图片文件路径</param>
    /// <returns>加载的图片对象</returns>
    public static Image LoadImage(string filePath)
    {
        if (_cache.TryGetValue(filePath, out var cachedImage))
        {
            return CloneImage(cachedImage);
        }

        lock (_cache)
        {
            if (_cache.TryGetValue(filePath, out cachedImage))
            {
                return CloneImage(cachedImage);
            }

            using var tempImage = Image.FromFile(filePath, true);
            var image = new Bitmap(tempImage);
            _cache[filePath] = image;
            return CloneImage(image);
        }
    }

    /// <summary>
    /// 预加载多个图片到缓存
    /// </summary>
    /// <param name="filePaths">图片文件路径列表</param>
    public static async Task PreloadImagesAsync(IEnumerable<string> filePaths)
    {
        var tasks = filePaths.Select(async path =>
        {
            if (!_cache.ContainsKey(path))
            {
                await LoadImageAsync(path);
            }
        });

        await Task.WhenAll(tasks);
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
    }

    /// <summary>
    /// 清除指定文件的缓存
    /// </summary>
    /// <param name="filePath">文件路径</param>
    public static void Remove(string filePath)
    {
        if (_cache.TryRemove(filePath, out var image))
        {
            image.Dispose();
        }
    }

    private static Image CloneImage(Image source)
    {
        return new Bitmap(source);
    }
}
