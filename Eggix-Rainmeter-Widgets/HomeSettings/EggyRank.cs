using System.Xml;
using System.Xml.Serialization;

namespace HomeSettings;

/// <summary>
/// 蛋仔信息类，用于管理蛋仔段位信息
/// </summary>
public sealed class EggyRank
{
    /// <summary>
    /// 构造函数，初始化默认蛋仔信息
    /// </summary>
    public EggyRank() : this(3, 3, 30) { }

    /// <summary>
    /// 构造函数，初始化蛋仔信息
    /// </summary>
    /// <param name="currentBasicEggyRank">当前大段位</param>
    /// <param name="currentSmallRank">当前小段位</param>
    /// <param name="currentLevel">当前等级</param>
    public EggyRank(long currentBasicEggyRank, long currentSmallRank, long currentLevel)
    {
        CurrentRank = new EggyRankInfo
        {
            BasicRank = currentBasicEggyRank,
            SmallRank = currentSmallRank,
            Level = currentLevel
        };
    }

    /// <summary>
    /// 构造函数，从XML文件加载段位信息
    /// </summary>
    /// <param name="filePath">XML文件路径</param>
    public EggyRank(string filePath)
    {
        LoadFromXml(filePath);
    }

    /// <summary>
    /// 当前段位信息
    /// </summary>
    public EggyRankInfo CurrentRank { get; private set; }

    /// <summary>
    /// 获取大段位名称
    /// </summary>
    public string GetBasicRankName()
    {
        return CurrentRank.BasicRank switch
        {
            1 => "鹌鹑蛋",
            2 => "鸽子蛋",
            3 => "鸡蛋",
            4 => "鹅蛋",
            5 => "鸵鸟蛋",
            6 => "恐龙蛋",
            7 => "巅峰凤凰蛋",
            8 => "超级凤凰蛋",
            9 => "无敌凤凰蛋",
            _ => "未知段位"
        };
    }

    /// <summary>
    /// 获取完整段位信息字符串
    /// </summary>
    public override string ToString()
    {
        return CurrentRank.BasicRank is >= 1 and <= 6
        ? $"{GetBasicRankName()} {CurrentRank.SmallRank}阶"
        : $"{GetBasicRankName()} {CurrentRank.SmallRank}分";
    }

    /// <summary>
    /// 从 XML 文件加载段位信息
    /// </summary>
    /// <param name="filePath">XML 文件路径</param>
    public void LoadFromXml(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("找不到指定的 XML 文件", filePath);
        }

        try
        {
            var serializer = new XmlSerializer(typeof(EggyRankData));
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var reader = XmlReader.Create(fileStream, new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                MaxCharactersFromEntities = 0,
                ValidationType = ValidationType.None
            });

            if (serializer.Deserialize(reader) is EggyRankData data)
            {
                double basicRankValue = Math.Clamp(data.BasicRank, 1, 9);

                CurrentRank = new EggyRankInfo
                {
                    BasicRank = unchecked((long)(double.IsFinite(basicRankValue) ? basicRankValue : 9)),
                    SmallRank = unchecked((long)data.SmallRank),
                    Level = unchecked((long)data.Level)
                };
            }
        }
        catch
        {
            CurrentRank = new EggyRankInfo
            {
                BasicRank = 3,
                SmallRank = 3,
                Level = 30
            };
        }
    }

    /// <summary>
    /// 保存段位信息到XML文件
    /// </summary>
    /// <param name="filePath">XML文件路径</param>
    public void SaveToXml(string filePath)
    {
        var data = new EggyRankData
        {
            BasicRank = CurrentRank.BasicRank,
            SmallRank = CurrentRank.SmallRank,
            Level = CurrentRank.Level
        };

        var serializer = new XmlSerializer(typeof(EggyRankData));
        using var writer = new StreamWriter(filePath);
        serializer.Serialize(writer, data);
    }

    /// <summary>
    /// 重置为默认段位
    /// </summary>
    public void ResetToDefault()
    {
        CurrentRank = new EggyRankInfo
        {
            BasicRank = 3,
            SmallRank = 3,
            Level = 30
        };
    }
}

/// <summary>
/// 巅峰派对段位信息结构体
/// </summary>
public struct EggyRankInfo
{
    /// <summary>
    /// 巅峰派对大段位
    /// </summary>
    internal long BasicRank;

    /// <summary>
    /// 巅峰派对小段位
    /// </summary>
    internal long SmallRank;

    /// <summary>
    /// 等级
    /// </summary>
    internal long Level;
}

/// <summary>
/// 用于XML序列化的段位数据类
/// </summary>
public sealed class EggyRankData
{
    /// <summary>
    /// 大段位索引
    /// </summary>
    public double BasicRank { get; init; }

    /// <summary>
    /// 小段位
    /// </summary>
    public double SmallRank { get; init; }

    /// <summary>
    /// 等级
    /// </summary>
    public double Level { get; init; }
}