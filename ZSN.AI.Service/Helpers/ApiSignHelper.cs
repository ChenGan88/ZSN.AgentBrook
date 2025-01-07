using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ZSN.AI.Entity;
using ZSN.Utils.Core.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace ZSN.AI.Service.Helpers
{
    public class ApiSignHelper
    {
        public static string GetSign(Dictionary<string, object> dic, string api_secret)
        {
            //var dicKeyList = dic.OrderBy(k => k.Key).Select(k => k.Key).ToList();
            var dicKeyList = dic.OrderBy(k => k.Key.ToLower()).Select(k => k.Key).ToList();
            var tempStr = "";
            foreach (string key in dicKeyList)
            {
                // 将布尔类型值转换为小写字符串 "true" 或 "false"
                tempStr += key.Trim() + (dic.ContainsKey(key) ? (dic[key] != null
                    ? (dic[key] is bool ? dic[key].ToString().ToLower() : dic[key].ToString())
                    : "")
                : "").Trim();
            }
            var resultStr2 = tempStr + "AppKEY" + api_secret;

            return EncryptHelper.MD5Encrypt(resultStr2).ToUpper();

        }
        public static string GetSignStr(Dictionary<string, object> dic)
        {
            var dicKeyList = dic.OrderBy(k => k.Key).Select(k => k.Key).ToList();
            var tempStr = "";
            foreach (string key in dicKeyList)
            {
                tempStr += key.Trim() + (dic.ContainsKey(key) ? (dic[key] != null ? dic[key].ToString() : "") : "").Trim();
            }
            return tempStr;

        }
    }
}
