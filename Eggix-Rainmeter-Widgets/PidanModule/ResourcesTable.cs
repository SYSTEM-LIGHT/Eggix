// 使用文件级命名空间声明（C# 10 及以上版本），将命名空间直接作用于整个文件，无需额外的大括号包裹。
namespace PidanModule;

/// <summary>
/// 资源表类，用于存储和管理资源。
/// </summary>
/// <typeparam name="T">资源的类型</typeparam>
/// <remarks>
/// 该类实现了 <see cref="IDisposable"/> 接口，用于在不再需要资源时释放资源；
/// 同时实现了 <see cref="IDictionary{TKey, TValue}"/> 和 <see cref="IReadOnlyDictionary{TKey, TValue}"/> 接口，
/// 提供键值对集合的完整操作能力和只读访问支持。
/// </remarks>
public class ResourcesTable<T> : IDisposable, IDictionary<string, T>, IReadOnlyDictionary<string, T>
    where T : notnull // 确保资源类型不为 null
{
    #region 字段

    // 禁用 IDE0044 警告，因为该字典需要在运行时动态增删资源，不能设为只读。
#pragma warning disable IDE0044
    /// <summary>
    /// 资源字典，用于存储资源。
    /// </summary>
    /// <remarks>
    /// 该字典用于存储资源，键为资源名称，值为资源实例。
    /// </remarks>
    private Dictionary<string, T> Resources;
#pragma warning restore IDE0044

    /// <summary>
    /// 表示资源是否已被释放的标志。
    /// </summary>
    private bool _disposed = false;
    
    /// <summary>
    /// 获取一个值，指示是否已释放资源。
    /// </summary>
    /// <returns>如果已释放资源，则为 true；否则为 false。</returns>
    /// <remarks>
    /// 该属性用于检查资源是否已被释放，以避免在已释放资源上进行操作。
    /// </remarks>
    public bool IsDisposed => _disposed;

    #endregion

    #region 构造函数

    /// <summary>
    /// 初始化 <see cref="ResourcesTable{T}"/> 类的新实例。
    /// </summary>
    public ResourcesTable()
    {
        // 使用 C# 12 引入的集合表达式（Collection Expression）语法初始化资源字典为空字典，
        // 避免 null 引用异常。集合表达式在 C# 12（.NET 8 起）中新增，提供一种简洁、统一的方式来创建集合。
        Resources = [];
    }

    /// <summary>
    /// 使用指定的键值对初始化 <see cref="ResourcesTable{T}"/> 类的新实例。
    /// </summary>
    /// <param name="initialResources">用于初始化字典的键值对集合。</param>
    /// <exception cref="ArgumentNullException">当 initialResources 为 null 时抛出。</exception>
    public ResourcesTable(IEnumerable<KeyValuePair<string, T>> initialResources)
    {
        // ArgumentNullException.ThrowIfNull(initialResources) 是 .NET 6 引入的语法糖，
        // 等价于 if (initialResources == null) throw new ArgumentNullException(nameof(initialResources));
        // 该特性自 C# 10 / .NET 6 起可用。
        ArgumentNullException.ThrowIfNull(initialResources);

        // 不采用 C# 12 的集合展开表达式“Resources = [.. initialResources];”，
        // 因其在旧工具链中可读性差，且隐式遍历会带来额外性能开销；
        // 使用简化的 new 表达式（C# 9.0 起）可直接利用 Dictionary 的构造函数），
        // 语义清晰、效率更高。
        Resources = new(initialResources);
    }

    /// <summary>
    /// 使用指定的键值对初始化 <see cref="ResourcesTable{T}"/> 类的新实例。
    /// </summary>
    /// <param name="initialResources">用于初始化字典的键值对。</param>
    public ResourcesTable(params KeyValuePair<string, T>[] initialResources)
    {
        Resources = new(initialResources);
    }

    #endregion

    #region IDisposable 实现

    /// <summary>
    /// 析构函数，用于释放非托管资源。
    /// </summary>
    ~ResourcesTable()
    {
        // 调用 Dispose(false) 释放非托管资源。
        Dispose(false);
    }

    /// <summary>
    /// 释放托管和非托管资源。
    /// </summary>
    public void Dispose()
    {
        // 调用 Dispose(true) 释放托管资源和非托管资源。
        Dispose(true);

        // 抑制析构函数的调用，因为资源已被释放。
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 释放托管和非托管资源。
    /// </summary>
    /// <param name="disposing">如果为 true，则释放托管和非托管资源；如果为 false，则仅释放非托管资源。</param>
    protected virtual void Dispose(bool disposing)
    {
        // 如果 _disposed 为 true，则说明资源已被释放，无需重复操作。
        if (!_disposed)
        {
            // 如果 disposing 为 true，则释放托管资源。
            if (disposing)
            {
                try
                {
                    // 检查资源类型是否实现了 IDisposable 接口，
                    // 如果实现了，则调用其 Dispose 方法释放资源。
                    // 这里使用 typeof(IDisposable).IsAssignableFrom(typeof(T)) 来检查类型
                    // 是否实现了 IDisposable 接口，而不是直接使用 typeof(T) is IDisposable，
                    // 因为 typeof(T) 返回的是类型元数据（Type 类型），而不是实例类型。
                    if (typeof(IDisposable).IsAssignableFrom(typeof(T)))
                    {
                        foreach (var item in Resources.Values)
                        {
                            try
                            {
                                (item as IDisposable)?.Dispose();
                            }
                            catch { } // 忽略释放资源时的异常，继续释放其他资源。
                        }
                    }
                    
                    // 清空资源字典，释放对资源的引用。
                    Resources.Clear();
                }
                catch { } // 忽略清空资源字典时的异常，继续执行后续操作。

                _disposed = true;
            }
        }
    }

    #endregion

    #region IDictionary<string, T> 实现

    /// <summary>
    /// 获取包含字典中所有键的集合。
    /// </summary>
    public ICollection<string> Keys => Resources.Keys;

    /// <summary>
    /// 获取包含字典中所有值的集合。
    /// </summary>
    public ICollection<T> Values => Resources.Values;

    /// <summary>
    /// 获取或设置具有指定键的元素。
    /// </summary>
    /// <param name="key">要获取或设置的元素的键。</param>
    /// <returns>具有指定键的元素。</returns>
    /// <exception cref="ObjectDisposedException">当资源已被释放时抛出。</exception>
    /// <exception cref="ArgumentNullException">当 key 为 null 时抛出。</exception>
    public T this[string key]
    {
        get
        {
            // ObjectDisposedException.ThrowIf(_disposed, this); 是 .NET 6 / C# 10 引入的语法糖，
            // 等价于：if (_disposed) throw new ObjectDisposedException(nameof(ResourcesTable<T>));
            ObjectDisposedException.ThrowIf(_disposed, this);
            ArgumentNullException.ThrowIfNull(key);
            return Resources[key];
        }
        set
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            ArgumentNullException.ThrowIfNull(key);
            Resources[key] = value;
        }
    }

    /// <summary>
    /// 获取字典中包含的键/值对的数量。
    /// </summary>
    public int Count => Resources.Count;

    /// <summary>
    /// 获取一个值，该值指示字典是否为只读。
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// 将带有指定键和值的元素添加到字典中。
    /// </summary>
    /// <param name="key">要添加的元素的键。</param>
    /// <param name="value">要添加的元素的值。</param>
    /// <exception cref="ObjectDisposedException">当资源已被释放时抛出。</exception>
    /// <exception cref="ArgumentNullException">当 key 为 null 时抛出。</exception>
    /// <exception cref="ArgumentException">当字典中已存在具有相同键的元素时抛出。</exception>
    public void Add(string key, T value)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(key);
        Resources.Add(key, value);
    }

    /// <summary>
    /// 从字典中移除所有键/值对。
    /// </summary>
    /// <exception cref="ObjectDisposedException">当资源已被释放时抛出。</exception>
    public void Clear()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        Resources.Clear();
    }

    /// <summary>
    /// 确定字典是否包含具有指定键的元素。
    /// </summary>
    /// <param name="key">要在字典中查找的键。</param>
    /// <returns>如果字典包含具有指定键的元素，则为 true；否则为 false。</returns>
    /// <exception cref="ObjectDisposedException">当资源已被释放时抛出。</exception>
    /// <exception cref="ArgumentNullException">当 key 为 null 时抛出。</exception>
    public bool ContainsKey(string key)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(key);
        return Resources.ContainsKey(key);
    }

    /// <summary>
    /// 从字典中移除带有指定键的元素。
    /// </summary>
    /// <param name="key">要移除的元素的键。</param>
    /// <returns>如果该元素已成功移除，则为 true；否则为 false。</returns>
    /// <exception cref="ObjectDisposedException">当资源已被释放时抛出。</exception>
    /// <exception cref="ArgumentNullException">当 key 为 null 时抛出。</exception>
    public bool Remove(string key)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(key);
        return Resources.Remove(key);
    }

    /// <summary>
    /// 获取与指定键关联的值。
    /// </summary>
    /// <param name="key">要获取其值的键。</param>
    /// <param name="value">当此方法返回时，如果找到指定键，则包含与该键关联的值；否则，包含值类型的默认值。</param>
    /// <returns>如果字典包含具有指定键的元素，则为 true；否则为 false。</returns>
    /// <exception cref="ObjectDisposedException">当资源已被释放时抛出。</exception>
    public bool TryGetValue(string key, out T value)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return Resources.TryGetValue(key, out value!);
    }

    #endregion

    #region IReadOnlyDictionary<string, T> 显式实现

    /// <summary>
    /// 获取包含字典中所有键的只读集合。
    /// </summary>
    IEnumerable<string> IReadOnlyDictionary<string, T>.Keys => Resources.Keys;

    /// <summary>
    /// 获取包含字典中所有值的只读集合。
    /// </summary>
    IEnumerable<T> IReadOnlyDictionary<string, T>.Values => Resources.Values;

    #endregion

    #region ICollection<KeyValuePair<string, T>> 实现

    /// <summary>
    /// 将指定的键/值对添加到字典中。
    /// </summary>
    /// <param name="item">要添加到字典中的键/值对。</param>
    /// <exception cref="ObjectDisposedException">当资源已被释放时抛出。</exception>
    /// <exception cref="ArgumentException">当字典中已存在具有相同键的元素时抛出。</exception>
    public void Add(KeyValuePair<string, T> item)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ((ICollection<KeyValuePair<string, T>>)Resources).Add(item);
    }

    /// <summary>
    /// 确定字典是否包含特定的键/值对。
    /// </summary>
    /// <param name="item">要在字典中查找的键/值对。</param>
    /// <returns>如果字典包含特定的键/值对，则为 true；否则为 false。</returns>
    /// <exception cref="ObjectDisposedException">当资源已被释放时抛出。</exception>
    public bool Contains(KeyValuePair<string, T> item)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return ((ICollection<KeyValuePair<string, T>>)Resources).Contains(item);
    }

    /// <summary>
    /// 从特定的数组索引开始，将字典的元素复制到数组中。
    /// </summary>
    /// <param name="array">作为从字典复制的元素的目标的一维数组。</param>
    /// <param name="arrayIndex">数组中开始复制的索引。</param>
    /// <exception cref="ObjectDisposedException">当资源已被释放时抛出。</exception>
    /// <exception cref="ArgumentNullException">当 array 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 arrayIndex 小于 0 时抛出。</exception>
    /// <exception cref="ArgumentException">当 array 不是一维数组或 arrayIndex 超出数组范围时抛出。</exception>
    public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(array);
        if (arrayIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Array index cannot be negative.");
        }
        if (array.Rank != 1)
        {
            throw new ArgumentException("Array must be one-dimensional.", nameof(array));
        }
        if (arrayIndex >= array.Length)
        {
            throw new ArgumentException("Array index is out of range.", nameof(arrayIndex));
        }
        ((ICollection<KeyValuePair<string, T>>)Resources).CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// 从字典中移除特定的键/值对。
    /// </summary>
    /// <param name="item">要从字典中移除的键/值对。</param>
    /// <returns>如果该键/值对已成功移除，则为 true；否则为 false。</returns>
    /// <exception cref="ObjectDisposedException">当资源已被释放时抛出。</exception>
    public bool Remove(KeyValuePair<string, T> item)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return ((ICollection<KeyValuePair<string, T>>)Resources).Remove(item);
    }

    #endregion

    #region IEnumerable<KeyValuePair<string, T>> 实现

    /// <summary>
    /// 返回一个循环访问字典的枚举器。
    /// </summary>
    /// <returns>字典的枚举器。</returns>
    /// <exception cref="ObjectDisposedException">当资源已被释放时抛出。</exception>
    public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return Resources.GetEnumerator();
    }

    /// <summary>
    /// 返回一个循环访问字典的枚举器。
    /// </summary>
    /// <returns>字典的枚举器。</returns>
    /// <exception cref="ObjectDisposedException">当资源已被释放时抛出。</exception>
    IEnumerator IEnumerable.GetEnumerator()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return ((IEnumerable)Resources).GetEnumerator();
    }

    #endregion
}
