using System;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using ZSN.Utils.Core.DI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using NLog;

namespace ZSN.Utils.Core.Helpers
{
    /// <summary>
    /// 日志记录类
    /// </summary>
    public class NLogHelper
    {
        private static readonly bool IsInit = false;

        static NLogHelper()
        {
            if (IsInit)
                return;
            IsInit = true;
            SetConfig();
        }

        private static bool _logInfoEnable = true;
        private static bool _logErrorEnable = false;
        private static bool _logExceptionEnable = false;
        private static bool _logComplementEnable = false;
        private static bool _logDebugEnable = false;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 设置初始值。
        /// </summary>
        public static void SetConfig()
        {
            _logInfoEnable = Logger.IsInfoEnabled;
            _logErrorEnable = Logger.IsErrorEnabled;
            _logExceptionEnable = Logger.IsErrorEnabled;
            _logComplementEnable = Logger.IsTraceEnabled;
            //LogFatalEnabled = logger.IsFatalEnabled;
            _logDebugEnable = Logger.IsDebugEnabled;
        }

        /// <summary>
        /// 写入普通日志消息
        /// </summary>
        /// <param name="info"></param>
        public static void WriteInfo(string info)
        {
            if (_logInfoEnable)
            {
                Logger.Info(BuildMessage(info));
            }
        }
        /// <summary>
        /// 写入Debug日志消息
        /// </summary>
        /// <param name="info"></param>
        public static void WriteDebug(string info)
        {
            if (_logDebugEnable)
            {
                Logger.Debug(BuildMessage(info));
            }
        }

        /// <summary>
        /// 写入错误日志消息
        /// </summary>
        /// <param name="info"></param>
        public static void WriteError(string info)
        {
            if (_logErrorEnable)
            {
                Logger.Error(BuildMessage(info));
            }
        }

        /// <summary>
        /// 写入异常日志信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        public static void WriteException(string info, Exception ex)
        {
            if (_logExceptionEnable)
            {
                Logger.Error(BuildMessage(info, ex));
            }
        }

        /// <summary>
        /// 写入严重错误日志消息
        /// </summary>
        /// <param name="info"></param>
        public static void WriteFatal(string info)
        {
            if (_logErrorEnable)
            {
                Logger.Fatal(BuildMessage(info));
            }
        }

        /// <summary>
        /// 写入补充日志
        /// </summary>
        /// <param name="info"></param>
        public static void WriteComplement(string info)
        {
            if (_logComplementEnable)
            {
                Logger.Trace(BuildMessage(info));
                //LogComplement.Error(info);
            }
        }
        /// <summary>
        /// 写入补充日志
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        public static void WriteComplement(string info, Exception ex)
        {
            if (_logComplementEnable)
            {
                Logger.Trace(BuildMessage(info, ex));
            }
        }


        private static string BuildMessage(string info, Exception ex = null)
        {
            var sb = new StringBuilder();
            var context = ServiceLocator.GetInstance<IHttpContextAccessor>()?.HttpContext;
            var request = context?.Request;
            sb.AppendFormat("============================\r\nTime:{0:yyyy-MM-dd HH:mm:ss.fff}\r\n{1}\r\n", DateTime.Now, info);

            if (request != null)
            {
                sb.AppendFormat("Url:{0}\r\n", request.GetDisplayUrl());
                if (null != request.UrlReferrer())
                {
                    sb.AppendFormat("UrlReferrer:{0}\r\n", request.UrlReferrer());
                }
                sb.AppendFormat("UserHostAddress:{0};{1};{2}\r\n",
                    request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    request.Headers["X-Forwarded-For"],
                    request.Headers["HTTP_NDUSER_FORWARDED_FOR_HAPROXY"]
                    );
                    //request.ServerVariables["LOCAL_ADDR"]);
            }

            if (ex != null)
            {
                if (ex is SqlException sqlEx)
                    sb.AppendFormat("Database:{0}\r\n", sqlEx.Server);
                sb.AppendFormat("Exception:{0}\r\n", ex);
            }
            sb.AppendLine();
            return sb.ToString();
        }

        /// <summary>
        /// 写入自定义日志到自定义目录,本方法对应的Nlog.config配置示例：
        ///  &lt;targets>
        ///    &lt;target name="LogCustom" xsi:type="File" layout="${message}"
        ///          fileName="${logDirectory}\${event-context:DirOrPrefix}${date:format=yyyyMMddHH}.txt">&lt;/target>
        ///  &lt;/targets>
        ///  &lt;rules>
        ///    &lt;logger name="LogCustom" level="Warn" writeTo="LogCustom" />
        /// </summary>
        /// <param name="message">要写入的消息</param>
        /// <param name="dirOrPrefix">
        /// 写入到的子目录或文件前缀，如果字符串包含\，则是子目录
        /// 比如 aa\bb 则写入的文件名为aa目录下的bb开头加日期
        /// </param>
        public static void WriteCustom(string message, string dirOrPrefix)
        {
            WriteCustom(message, dirOrPrefix, null, true);
        }

        /// <summary>
        /// 写入自定义日志到自定义目录,本方法对应的Nlog.config配置示例：
        ///  &lt;targets>
        ///    &lt;target name="LogCustom" xsi:type="File" layout="${message}"
        ///          fileName="${logDirectory}\${event-context:DirOrPrefix}${date:format=yyyyMMddHH}.txt">&lt;/target>
        ///  &lt;/targets>
        ///  &lt;rules>
        ///    &lt;logger name="LogCustom" level="Warn" writeTo="LogCustom" />
        /// </summary>
        /// <param name="message">要写入的消息</param>
        /// <param name="dirOrPrefix">
        /// 写入到的子目录或文件前缀，如果字符串包含\，则是子目录
        /// 比如 aa\bb 则写入的文件名为aa目录下的bb开头加日期
        /// </param>
        /// <param name="addIpUrl">是否要附加ip和url等信息</param>
        public static void WriteCustom(string message, string dirOrPrefix, bool addIpUrl)
        {
            WriteCustom(message, dirOrPrefix, null, addIpUrl);
        }


        /// <summary>
        /// 写入自定义日志到自定义目录,本方法对应的Nlog.config配置示例：
        ///  &lt;targets>
        ///    &lt;target name="LogCustom" xsi:type="File" layout="${message}"
        ///          fileName="${logDirectory}\${event-context:DirOrPrefix}${date:format=yyyyMMddHH}${event-context:Suffix}.txt">&lt;/target>
        ///  &lt;/targets>
        ///  &lt;rules>
        ///    &lt;logger name="LogCustom" level="Warn" writeTo="LogCustom" />
        /// </summary>
        /// <param name="message">要写入的消息</param>
        /// <param name="dirOrPrefix">
        /// 写入到的子目录或文件前缀，如果字符串包含\，则是子目录
        /// 比如 aa\bb 则写入的文件名为aa目录下的bb开头加日期
        /// </param>
        /// <param name="suffix">写入到的文件后缀</param>
        public static void WriteCustom(string message, string dirOrPrefix, string suffix)
        {
            WriteCustom(message, dirOrPrefix, suffix, true);
        }

        /// <summary>
        /// 写入自定义日志到自定义目录,本方法对应的Nlog.config配置示例：
        ///  &lt;targets>
        ///    &lt;target name="LogCustom" xsi:type="File" layout="${message}"
        ///          fileName="${logDirectory}\${event-context:DirOrPrefix}${date:format=yyyyMMddHH}${event-context:Suffix}.txt">&lt;/target>
        ///  &lt;/targets>
        ///  &lt;rules>
        ///    &lt;logger name="LogCustom" level="Warn" writeTo="LogCustom" />
        /// </summary>
        /// <param name="message">要写入的消息</param>
        /// <param name="dirOrPrefix">
        /// 写入到的子目录或文件前缀，如果字符串包含\，则是子目录
        /// 比如 aa\bb 则写入的文件名为aa目录下的bb开头加日期
        /// </param>
        /// <param name="suffix">写入到的文件后缀</param>
        /// <param name="addIpUrl">是否要附加ip和url等信息</param>
        public static void WriteCustom(string message, string dirOrPrefix, string suffix, bool addIpUrl)
        {
            if (addIpUrl)
                message = BuildMessage(message);
            var logger1 = LogManager.GetLogger("LogCustom");
            var logEvent = new LogEventInfo(LogLevel.Warn, logger1.Name, message);
            logEvent.Properties["DirOrPrefix"] = dirOrPrefix;
            if (suffix != null)
                logEvent.Properties["Suffix"] = suffix;
            logger1.Log(logEvent);
        }
    }
}
