using System.Xml.Serialization;

namespace HomeSettings
{
    /// <summary>
    /// 蛋仔信息类，用于管理蛋仔段位信息
    /// </summary>
    public sealed class EggyRank
    {
        /// <summary>
        /// 构造函数，初始化默认蛋仔信息
        /// </summary>
        public EggyRank() : this(BasicEggyRank.ChickenEgg, 3, 30) { }

        /// <summary>
        /// 构造函数，初始化蛋仔信息
        /// </summary>
        /// <param name="currentBasicEggyRank">当前大段位</param>
        /// <param name="currentSmallRank">当前小段位</param>
        /// <param name="currentLevel">当前等级</param>
        public EggyRank(BasicEggyRank currentBasicEggyRank, int currentSmallRank, int currentLevel)
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
                BasicEggyRank.QuailEgg => "鹌鹑蛋",
                BasicEggyRank.PigeonEgg => "鸽子蛋",
                BasicEggyRank.ChickenEgg => "鸡蛋",
                BasicEggyRank.GooseEgg => "鹅蛋",
                BasicEggyRank.OstrichEgg => "鸵鸟蛋",
                BasicEggyRank.DinosaurEgg => "恐龙蛋",
                BasicEggyRank.PeakPhoenixEgg => "巅峰凤凰蛋",
                BasicEggyRank.SuperPhoenixEgg => "超级凤凰蛋",
                BasicEggyRank.InvinciblePhoenixEgg => "无敌凤凰蛋",
                _ => "未知段位"
            };
        }

        /// <summary>
        /// 获取完整段位信息字符串
        /// </summary>
        public override string ToString()
        {
            if ((int)CurrentRank.BasicRank <= 6)
            {
                return $"{GetBasicRankName()} {CurrentRank.SmallRank}阶";
            }
            return $"{GetBasicRankName()} {CurrentRank.SmallRank}分";
          
        }

        /// <summary>
        /// 从XML文件加载段位信息
        /// </summary>
        /// <param name="filePath">XML文件路径</param>
        public void LoadFromXml(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("找不到指定的XML文件", filePath);
            }

            var serializer = new XmlSerializer(typeof(EggyRankData));
            using var reader = new StreamReader(filePath);

            if (serializer.Deserialize(reader) is EggyRankData data)
            {
                CurrentRank = new EggyRankInfo
                {
                    BasicRank = (BasicEggyRank)data.BasicRank,
                    SmallRank = data.SmallRank,
                    Level = data.Level
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
                BasicRank = (int)CurrentRank.BasicRank,
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
                BasicRank = BasicEggyRank.QuailEgg,
                SmallRank = 1,
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
        public BasicEggyRank BasicRank;

        /// <summary>
        /// 巅峰派对小段位
        /// </summary>
        public int SmallRank;

        /// <summary>
        /// 等级
        /// </summary>
        public int Level;
    }

    /// <summary>
    /// 用于XML序列化的段位数据类
    /// </summary>
    public sealed class EggyRankData
    {
        /// <summary>
        /// 大段位索引
        /// </summary>
        public int BasicRank { get; set; }

        /// <summary>
        /// 小段位
        /// </summary>
        public int SmallRank { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }
    }

    /// <summary>
    /// 巅峰派对大段位枚举常数
    /// </summary>
    public enum BasicEggyRank
    {
        /// <summary>
        /// 鹌鹑蛋
        /// </summary>
        QuailEgg = 1,

        /// <summary>
        /// 鸽子蛋
        /// </summary>
        PigeonEgg = 2,

        /// <summary>
        /// 鸡蛋
        /// </summary>
        ChickenEgg = 3,

        /// <summary>
        /// 鹅蛋
        /// </summary>
        GooseEgg = 4,

        /// <summary>
        /// 鸵鸟蛋
        /// </summary>
        OstrichEgg = 5,

        /// <summary>
        /// 恐龙蛋
        /// </summary>
        DinosaurEgg = 6,

        /// <summary>
        /// 巅峰凤凰蛋
        /// </summary>
        PeakPhoenixEgg = 7,

        /// <summary>
        /// 超级凤凰蛋
        /// </summary>
        SuperPhoenixEgg = 8,

        /// <summary>
        /// 无敌凤凰蛋
        /// </summary>
        InvinciblePhoenixEgg = 9
    }
}
