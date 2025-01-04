using ZSN.Utils.Core.DI;
using ZSN.Utils.Core.Extensions;
using Microsoft.Extensions.Configuration;

namespace ZSN.Utils.Core.Helpers
{
    /// <summary>
    /// 配置文件读取工具类
    /// </summary>
    public class ConfigHelper
    {
        public static IConfiguration Configuration =>
            ServiceLocator.GetInstance<IConfiguration>()
            ?? new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();


        public static IConfigurationSection GetSection(string key)
        {
            return Configuration.GetSection(key);
        }

        public static string GetConfigurationValue(string key)
        {
            return Configuration[key];
        }

        public static string GetConfigurationValue(string section, string key)
        {
            return GetSection(section)?[key];
        }

        public static string GetConnectionString(string key)
        {
            string _conn = Configuration.GetConnectionString(key);
            return _conn;
        }

        #region GetString 获取配置字符串值

        /// <summary>
        ///     获取配置字符串值
        /// </summary>
        /// <param name="configStr">配置名称</param>
        /// <param name="defaultStr">没有配置项时返回的字符串</param>
        /// <returns>字符串值</returns>
        public static string GetString(string configStr, string defaultStr = "")
        {
            var result = GetConfigurationValue(configStr);
            if (result == null)
                result = defaultStr;
            return result;
        }

        #endregion

        #region GetInt 获取配置整数值，无值返回 -1

        /// <summary>
        ///     获取配置整数值，无值返回 -1
        /// </summary>
        /// <param name="configStr">配置名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>整数值</returns>
        public static int GetInt(string configStr, int defaultValue = -1)
        {
            return GetString(configStr).ToInt32(defaultValue);
        }

        #endregion

        #region GetDecimal 获取配置浮点值

        /// <summary>
        ///     获取配置浮点值
        /// </summary>
        /// <param name="configStr">配置名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>浮点值</returns>
        public static decimal GetDecimal(string configStr, decimal defaultValue = -1)
        {
            return GetString(configStr).ToDecimal(defaultValue);
        }

        #endregion

        #region GetBool 获取配置布尔值

        /// <summary>
        ///     获取配置布尔值(1或true为真，不区分大小写)
        /// </summary>
        /// <param name="configStr">配置名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>布尔值</returns>
        public static bool GetBool(string configStr, bool defaultValue = false)
        {
            return GetString(configStr).ToBoolean(defaultValue);
        }

        #endregion
    }
}
