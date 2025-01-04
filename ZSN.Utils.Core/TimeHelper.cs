using System;
using System.Collections.Generic;
using System.Text;

namespace ZSN.Utils.Core
{
   public static class TimeHelper
    {
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <param name="millisecond">精度（毫秒）设置 true，则生成13位的时间戳；精度（秒）设置为 false，则生成10位的时间戳；默认为 true </param>
        /// <returns></returns>
        public static string GetCurrentTimestamp(bool millisecond = false)
        {
            return DateTime.Now.ToTimestamp(millisecond);
        }

        /// <summary>
        /// 转换指定时间得到对应的时间戳
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="millisecond">精度（毫秒）设置 true，则生成13位的时间戳；精度（秒）设置为 false，则生成10位的时间戳；默认为 true </param>
        /// <returns>返回对应的时间戳</returns>
        public static string ToTimestamp(this DateTime dateTime, bool millisecond = true)
        {
            return dateTime.ToTimestampLong(millisecond).ToString();
        }

        /// <summary>
        /// 转换指定时间得到对应的时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="millisecond">精度（毫秒）设置 true，则生成13位的时间戳；精度（秒）设置为 false，则生成10位的时间戳；默认为 true </param>
        /// <returns>返回对应的时间戳</returns>
        public static long ToTimestampLong(this DateTime dateTime, bool millisecond = true)
        {
            var ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return millisecond ? Convert.ToInt64(ts.TotalMilliseconds) : Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 转换指定时间戳到对应的时间
        /// </summary>
        /// <param name="timestamp">（10位或13位）时间戳</param>
        /// <returns>返回对应的时间</returns>
        public static DateTime ToDateTime(this string timestamp)
        {
            var tz = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return timestamp.Length == 13
                ? tz.AddMilliseconds(Convert.ToInt64(timestamp))
                : tz.AddSeconds(Convert.ToInt64(timestamp));
        }

        /// <summary>
        /// 两个时间戳相减
        /// </summary>
        /// <param name="timestampA"></param>
        /// <param name="timestampB"></param>
        /// <returns></returns>
        public static int SubtractTimestam(string timestampA,string timestampB)
        {
            return (int)(Convert.ToInt64(timestampA) - Convert.ToInt64(timestampB));
        }
    }
}
