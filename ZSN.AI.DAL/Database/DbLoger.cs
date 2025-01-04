using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZSN.Utils.Core.DI;
using ZSN.Utils.Core.Helpers;
using Microsoft.AspNetCore.Http;
using ZSN.Utils.Core.Extensions;

namespace ZSN.AI.DAL
{
    public class DbLoger
    {
        private const string SqlKey = "__ZSN_DbHelper_SQL__";

        private const string SqlSplitKey = "_$#$_";

        public static void AddLog(string text)
        {
            var time = DateTime.Now;
            var logStr = $"时间：{time:yyyy-MM-dd HH:mm:ss.fffffff},{Environment.NewLine}SQL：{text}.";
            if (HttpContextHelper.Session == null)
            {
                NLogHelper.WriteCustom(logStr, "/SQL/");
                return;
            }
            var log = HttpContextHelper.Session.Get<string>(SqlKey);
            log = log.IsNullOrEmpty() ? "" : log;
            var logList = log.Split(SqlSplitKey).ToList();
            logList.Add(logStr);
            HttpContextHelper.Session.Set(SqlKey, string.Join(SqlSplitKey, logList));
        }

        public static void InitLog()
        {
            HttpContextHelper.Session?.Set(SqlKey, "");
        }

        public static string GetLog()
        {
            if (HttpContextHelper.Session == null)
                return "";
            var log = HttpContextHelper.Session.Get<string>(SqlKey);
            log = log.IsNullOrEmpty() ? "" : log;
            var logList = log.Split(SqlSplitKey).ToList();
            return string.Join(Environment.NewLine, logList);
        }
    }
}
