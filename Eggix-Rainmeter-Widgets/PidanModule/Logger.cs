namespace PidanModule;

/// <summary>
/// 日志级别枚举
/// </summary>
public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error,
    Fatal
}

/// <summary>
/// 健壮的日志记录类，用于将日志记录到文件
/// </summary>
public class Logger : IDisposable
{
    #region 私有字段

    private static readonly Lock _syncLock = new();
    private static Logger? _defaultInstance;
    private readonly string _logDirectory;
    private readonly string _logFilePath;
    private readonly LogLevel _minLogLevel;
    private readonly int _maxLogFileSizeInMB;
    private readonly int _maxLogFilesToKeep;
    private StreamWriter? _logWriter;
    private const string _logName = "OpenEggyUILog";
    private bool _disposed = false;

    #endregion

    #region 属性

    /// <summary>
    /// 获取Logger默认实例
    /// </summary>
    public static Logger Default
    {
        get
        {
            if (_defaultInstance == null)
            {
                lock (_syncLock)
                {
                    _defaultInstance ??= new Logger();
                }
            }
            return _defaultInstance;
        }
    }

    /// <summary>
    /// 获取或设置当前最小日志级别
    /// </summary>
    public LogLevel MinLogLevel
    {
        get { return _minLogLevel; }
    }

    #endregion

    #region 构造函数

    /// <summary>
    /// 私有构造函数，初始化Logger实例
    /// </summary>
    /// <param name="logDirectory">日志目录</param>
    /// <param name="minLogLevel">最小日志级别</param>
    /// <param name="maxLogFileSizeInMB">单个日志文件最大大小（MB）</param>
    /// <param name="maxLogFilesToKeep">保留的日志文件数量</param>
    private Logger(
        string? logDirectory = null,
        LogLevel minLogLevel = LogLevel.Debug,
        int maxLogFileSizeInMB = 10,
        int maxLogFilesToKeep = 10)
    {
        _minLogLevel = minLogLevel;
        _maxLogFileSizeInMB = maxLogFileSizeInMB > 0 ? maxLogFileSizeInMB : 10;
        _maxLogFilesToKeep = maxLogFilesToKeep > 0 ? maxLogFilesToKeep : 10;

        // 设置日志目录
        if (string.IsNullOrWhiteSpace(logDirectory))
        {
            // 默认使用应用程序目录下的Logs文件夹
            _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        }
        else
        {
            _logDirectory = logDirectory;
        }

        // 确保日志目录存在
        try
        {
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }
        catch
        {
            // 如果无法创建日志目录，则回退到临时目录
            try
            {
                _logDirectory = Path.Combine(Path.GetTempPath(), $"{_logName}Logs");
                if (!Directory.Exists(_logDirectory))
                {
                    Directory.CreateDirectory(_logDirectory);
                }
            }
            catch
            {
                // 如果连临时目录都无法创建，则使用应用程序目录
                _logDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        // 设置日志文件路径
        string logFileName = $"{_logName}_{DateTime.Now:yyyyMMdd}.log";
        _logFilePath = Path.Combine(_logDirectory, logFileName);

        // 初始化日志写入器
        InitializeLogWriter();
    }

    #endregion

    #region 公共方法

    /// <summary>
    /// 写入日志行
    /// </summary>
    /// <param name="message">日志消息</param>
    /// <param name="level">日志级别</param>
    /// <param name="category">日志分类</param>
    public void WriteLog(string message, LogLevel level = LogLevel.Info, string? category = null)
    {
        if (level < _minLogLevel)
        {
            return;
        }

        try
        {
            lock (_syncLock)
            {
                // 检查日志文件大小，如果超过限制则创建新文件
                CheckLogFileSize();

                // 格式化日志消息
                string formattedMessage = FormatLogMessage(message, level, category);

                // 写入日志文件
                if (_logWriter != null && _logWriter.BaseStream != null)
                {
                    _logWriter.WriteLine(formattedMessage);
                    _logWriter.Flush(); // 立即刷新缓冲区，确保日志写入文件
                }
                else
                {
                    // 如果日志写入器不可用，尝试重新初始化
                    InitializeLogWriter();
                    if (_logWriter != null)
                    {
                        _logWriter.WriteLine(formattedMessage);
                        _logWriter.Flush();
                    }
                    else
                    {
                        // 如果仍然无法写入日志，则输出到调试控制台
                        System.Diagnostics.Debug.WriteLine($"无法写入日志文件: {formattedMessage}");
                    }
                }

                // 同时输出到调试控制台
                System.Diagnostics.Debug.WriteLine(formattedMessage);
            }
        }
        catch (Exception ex)
        {
            // 如果日志记录过程中发生异常，尝试输出到调试控制台
            System.Diagnostics.Debug.WriteLine($"日志记录失败: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"原始日志消息: {message}");
        }
    }

    /// <summary>
    /// 写入调试级别日志
    /// </summary>
    /// <param name="message">日志消息</param>
    /// <param name="category">日志分类</param>
    [Conditional("DEBUG")]
    public void WriteDebugLog(string message, string? category = null)
    {
        WriteLog(message, LogLevel.Debug, category);
    }

    /// <summary>
    /// 写入信息级别日志
    /// </summary>
    /// <param name="message">日志消息</param>
    /// <param name="category">日志分类</param>
    public void WriteInfoLog(string message, string? category = null)
    {
        WriteLog(message, LogLevel.Info, category);
    }

    /// <summary>
    /// 写入警告级别日志
    /// </summary>
    /// <param name="message">日志消息</param>
    /// <param name="category">日志分类</param>
    public void WriteWarningLog(string message, string? category = null)
    {
        WriteLog(message, LogLevel.Warning, category);
    }

    /// <summary>
    /// 写入错误级别日志
    /// </summary>
    /// <param name="message">日志消息</param>
    /// <param name="category">日志分类</param>
    public void WriteErrorLog(string message, string? category = null)
    {
        WriteLog(message, LogLevel.Error, category);
    }

    /// <summary>
    /// 写入致命错误级别日志
    /// </summary>
    /// <param name="message">日志消息</param>
    /// <param name="category">日志分类</param>
    public void WriteFatalLog(string message, string? category = null)
    {
        WriteLog(message, LogLevel.Fatal, category);
    }

    /// <summary>
    /// 写入异常信息日志
    /// </summary>
    /// <param name="ex">异常对象</param>
    /// <param name="message">附加消息</param>
    /// <param name="category">日志分类</param>
    public void WriteExceptionLog(Exception exception, string? message = null, string? category = null)
    {
        if (exception == null)
        {
            WriteWarningLog("尝试记录空异常", category);
            return;
        }

        string exceptionMessage = string.IsNullOrEmpty(message)
            ? $"异常: {exception.GetType().Name} - {exception.Message}"
            : $"{message} - 异常: {exception.GetType().Name} - {exception.Message}";

        WriteLog(exceptionMessage, LogLevel.Error, category);

        // 记录异常堆栈跟踪
        if (!string.IsNullOrEmpty(exception.StackTrace))
        {
            WriteLog($"堆栈跟踪: {exception.StackTrace}", LogLevel.Error, category);
        }

        // 记录内部异常
        if (exception.InnerException != null)
        {
            WriteExceptionLog(exception.InnerException, "内部异常", category);
        }
    }

    #endregion

    #region 私有方法

    /// <summary>
    /// 初始化日志写入器
    /// </summary>
    private void InitializeLogWriter()
    {
        try
        {
            // 确保之前的写入器已正确关闭
            if (_logWriter != null)
            {
                try
                {
                    _logWriter.Flush();
                    _logWriter.Close();
                    _logWriter.Dispose();
                }
                catch
                {
                    // 忽略关闭时的异常
                }
                _logWriter = null;
            }

            // 检查是否是第一次创建日志文件
            bool isFirstTime = !File.Exists(_logFilePath);

            // 创建新的日志写入器
            _logWriter = new StreamWriter(_logFilePath, true, Encoding.UTF8) { AutoFlush = true };

            // 如果是第一次创建日志文件，写入反馈声明
            if (isFirstTime)
            {
                WriteFeedbackStatement();
            }

            // 写入初始化日志
            string initMessage = FormatLogMessage("日志系统初始化", LogLevel.Info, "System");
            _logWriter.WriteLine(initMessage);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"初始化日志写入器失败: {ex.Message}");
            _logWriter = null;
        }
    }

    /// <summary>
    /// 检查日志文件大小，如果超过限制则创建新文件
    /// </summary>
    private void CheckLogFileSize()
    {
        try
        {
            if (!File.Exists(_logFilePath))
            {
                return;
            }

            FileInfo fileInfo = new(_logFilePath);
            long maxSizeBytes = _maxLogFileSizeInMB * 1024 * 1024;

            if (fileInfo.Length >= maxSizeBytes)
            {
                // 关闭当前日志写入器
                if (_logWriter != null)
                {
                    _logWriter.Flush();
                    _logWriter.Close();
                    _logWriter.Dispose();
                    _logWriter = null;
                }

                // 创建新的日志文件名（添加时间戳）
                string newFileName = $"{_logName}_{DateTime.Now:yyyyMMdd_HHmmss}.log";
                string newFilePath = Path.Combine(_logDirectory, newFileName);

                // 重命名当前日志文件
                File.Move(_logFilePath, newFilePath);

                // 清理旧日志文件
                CleanOldLogFiles();

                // 重新初始化日志写入器
                InitializeLogWriter();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"检查日志文件大小时发生错误: {ex.Message}");
        }
    }

    /// <summary>
    /// 清理旧日志文件
    /// </summary>
    private void CleanOldLogFiles()
    {
        try
        {
            // 获取所有日志文件
            DirectoryInfo logDirInfo = new(_logDirectory);
            FileInfo[] logFiles = [.. logDirInfo.GetFiles($"{_logName}_*.log").OrderBy(f => f.CreationTime)];

            // 如果日志文件数量超过限制，删除最旧的文件
            if (logFiles.Length > _maxLogFilesToKeep)
            {
                int filesToDelete = logFiles.Length - _maxLogFilesToKeep;
                for (int i = 0; i < filesToDelete; i++)
                {
                    try
                    {
                        logFiles[i].Delete();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"删除日志文件失败: {logFiles[i].Name} - {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"清理旧日志文件时发生错误: {ex.Message}");
        }
    }

    /// <summary>
    /// 写入反馈声明
    /// </summary>
    private void WriteFeedbackStatement()
    {
        try
        {
            string feedbackMessage = @"================================================================================
反馈声明：
如果遇到任何问题，请将此日志文件发到OpenEggyUI交流群或私发给SYSTEM-LIGHT
QQ号：2311622884
OpenEggyUI交流群：1062926070
================================================================================";

            _logWriter?.WriteLine(feedbackMessage);
            _logWriter?.Flush();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"写入反馈声明失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 格式化日志消息
    /// </summary>
    /// <param name="message">原始消息</param>
    /// <param name="level">日志级别</param>
    /// <param name="category">日志分类</param>
    /// <returns>格式化后的日志消息</returns>
    private static string FormatLogMessage(string message, LogLevel level, string? category)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        string levelStr = level.ToString().ToUpper();
        string categoryStr = string.IsNullOrEmpty(category) ? "" : $"[{category}] ";

        return $"[{timestamp}] [{levelStr}] {categoryStr}{message}";
    }

    #endregion

    #region IDisposable 实现

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    /// <param name="disposing">是否正在释放托管资源</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // 释放托管资源
                if (_logWriter != null)
                {
                    try
                    {
                        // 写入关闭日志
                        string closeMessage = FormatLogMessage("日志系统关闭", LogLevel.Info, "System");
                        _logWriter.WriteLine(closeMessage);
                        _logWriter.Flush();
                        _logWriter.Close();
                    }
                    catch
                    {
                        // 忽略关闭时的异常
                    }
                    _logWriter.Dispose();
                    _logWriter = null;
                }
            }

            _disposed = true;
        }
    }

    /// <summary>
    /// 析构函数
    /// </summary>
    ~Logger()
    {
        Dispose(false);
    }

    #endregion
}
