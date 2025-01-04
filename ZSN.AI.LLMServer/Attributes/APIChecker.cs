
using ZSN.AI.Service.WebHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using ZSN.Utils.Core.Helpers;
using ZSN.AI.LLMServer.Controllers;
using ZSN.AI.Service.Helpers;
using static ZSN.AI.Service.Controllers.CommonApiBaseController;
using ZSN.Utils.Core;
using Newtonsoft.Json.Linq;
using ZSN.Utils.Core.Extensions;
using ZSN.AI.LLMServer.Helpers;
using Microsoft.AspNetCore.Http;
using ZSN.AI.Entity;

namespace ZSN.AI.LLMServer.Attributes
{
    /// <summary>
    /// API过滤器
    /// </summary>
    public class APIChecker: ActionFilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// 是否进行Token认证
        /// </summary>
        public bool Token = true;
        /// <summary>
        /// 是否进行签名校验
        /// </summary>
        public bool Sign = ConfigHelper.GetBool("CheckSign",true);
        /// <summary>
        /// 是否进行时间戳校验
        /// </summary>
        public bool Timestamp = ConfigHelper.GetBool("CheckTimestamp", true);
        public bool Stream = false;

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            Stream = IsStream(context);

            bool token_checked = false;
            if (Token)
            {
                token_checked = CheckToken(context);
            }
            else
            {
                token_checked = true;
            }
            if (token_checked)
            {
                if (Sign)
                {
                    var rst = CheckSign(context);
                    if (!rst)
                    {
                        context.Result = GetErrorResult(ErrorCode.SignError);
                    }
                }
                if (Timestamp)
                {
                    var rst = CheckTimestamp(context);
                    if (!rst)
                    {
                        context.Result = GetErrorResult(ErrorCode.TimestampError);
                    }
                }
            }
            else
            {
                context.Result = GetErrorResult(ErrorCode.TokenCheckError);
            }

            
            base.OnActionExecuting(context);
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next) {
            if (Stream)
            {
                context.HttpContext.Response.Headers.Add("Content-Type", "text/event-stream");
                context.HttpContext.Response.Headers.Add("Cache-Control", "no-cache");

                //await context.HttpContext.Response.WriteAsync(context.Result.ToJsonString());
                //await next();
                //await context.HttpContext.Response.Body.FlushAsync();
            }

            base.OnResultExecutionAsync(context, next);
        }

        public void OnException(ExceptionContext context)
        {
            DefaultLogService.AddOperationLog(5, context.Exception, "", "APIChecker内部错误");
            NLogHelper.WriteException(ErrorCode.Error.ToString(), context.Exception);
            context.Result = GetErrorResult(ErrorCode.Error);
        }

        public static JsonResult GetErrorResult(ErrorCode errorCode)
        {
            return GetErrorResult((int)errorCode, errorCode.ToString());
        }
        public static JsonResult GetErrorResult(int code, string detail)
        {
            return new JsonResult(new
            {
                Success = false,
                ErrorCode = code,
                ErrorDesc = detail
            });
        }

        public bool CheckToken(ActionExecutingContext actionContext)
        {
            try
            {
                string _Token = (actionContext.Controller as ApiBaseController)?.Token;

                if (string.IsNullOrEmpty(_Token))
                {
                    return false;
                }
                else
                {
                    string userID = "";
                    string timestamp = "";
                    string now_timestamp = TimeHelper.GetCurrentTimestamp();

                    GetUserIdByToken(_Token,out userID,out timestamp);
                    
                    if (userID != "")
                    {
                        if (TimeHelper.SubtractTimestam(now_timestamp, timestamp) <= ConfigHelper.GetInt("AccessTokenTimeOut"))//Token有效时间
                        {
                            return true;
                            //RoadFlow.Platform.Users users = new RoadFlow.Platform.Users();
                            //RoadFlow.Data.Model.Users byAccount = users.Get(Guid.Parse(userID));
                            /*
                            if (byAccount != null)
                            {
                                if (byAccount.Status == 0)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                            */
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                DefaultLogService.AddOperationLog(5, e, "", ErrorCode.TokenCheckError.ToString());
                NLogHelper.WriteException("Token校验失败", e);
                return false;
            }
        }

        public bool IsStream(ActionExecutingContext actionContext)
        {
            string obj = (actionContext.Controller as ApiBaseController)?.BodyParams;
            if (string.IsNullOrEmpty(obj))
            {
                return false;
            }
            else
            {
                JObject DataObj = JsonConvert.DeserializeObject<JObject>(obj);
                return DataObj.Value<bool>("Stream");
            }
        }
        public bool CheckSign(ActionExecutingContext actionContext)
        {
            try
            {
                string obj = (actionContext.Controller as ApiBaseController)?.BodyParams;

                if (string.IsNullOrEmpty(obj))
                {
                    return false;
                }
                else
                {
                    var rst = false;
                    //当Access Token不存在时使用RSA签名认证
                    string _Token = (actionContext.Controller as ApiBaseController)?.Token;
                    if (_Token.IsNullOrEmpty())
                    {
                        JObject DataObj = JsonConvert.DeserializeObject<JObject>(obj);
                        string Data = DataObj.Value<string>("Data");
                        string oldSign = DataObj.Value<string>("Sign");

                        //APISettings aPISettings = SettingsService.Current;
                        //var PublicKey = aPISettings.PublicKey;
                        //var Server_PrivateKey = ConfigHelper.GetString("RSAPrivateKey");
                        //Data = EncryptHelper.RSADecrypt(Server_PrivateKey, Data);

                        rst = true;// EncryptHelper.RSAVerifySign(PublicKey, oldSign, Data);
                        if (!rst)
                        {
                            DefaultLogService.AddOperationLog(5, $"RSA验签失败<br/>oldSign:{oldSign}<br/>{Data}");
                        }
                    }
                    else
                    {
                        Dictionary<string, object> tmpDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(obj);
                        /*
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        foreach (var p in tmpDic)
                        {
                            var val = p.Value.ToString();
                            if (val.IndexOf('{') == 0)
                            {
                                var td = JsonConvert.DeserializeObject<Dictionary<string, object>>(val);
                                dic.Add(p.Key, JsonConvert.SerializeObject(td.OrderBy(t => t.Key, StringComparer.Ordinal).ToDictionary(k => k.Key, v => v.Value)));
                            }
                            else
                            {
                                dic.Add(p.Key, val);
                            }
                        }
                        */
                        if (!tmpDic.ContainsKey("Sign"))
                            return false;

                        string oldSign = tmpDic["Sign"]?.ToString();
                        tmpDic.Remove("Sign");

                        var api_secret = _Token;
                        string sign = ApiSignHelper.GetSign(tmpDic, api_secret);
                        rst = oldSign.ToUpper() == sign.ToUpper();

                        if (!rst)
                        {
                            DefaultLogService.AddOperationLog(5, $"AccessToken验签失败<br/>oldSign:{oldSign}<br/>sign:{sign}<br/>{JsonConvert.SerializeObject(tmpDic, Formatting.Indented)}");
                        }
                    }
                    return rst;
                }
            }
            catch (Exception e)
            {
                DefaultLogService.AddOperationLog(5, e, "", "验签错误2");
                NLogHelper.WriteException("验签错误2", e);
                return false;
            }
        }

        public bool CheckTimestamp(ActionExecutingContext actionContext)
        {
            try
            {
                string obj = (actionContext.Controller as ApiBaseController)?.BodyParams;

                if (string.IsNullOrEmpty(obj))
                {
                    return false;
                }
                else
                {
                    bool rst = false;
                    Dictionary<string, object> tmpDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(obj);
                    foreach (var p in tmpDic)
                    { 
                        if(p.Key.ToLower() == "timestamp")
                        {
                            var timestamp = p.Value.ToString();
                            string now_timestamp = timestamp.Length == 13 ? TimeHelper.GetCurrentTimestamp(true) : TimeHelper.GetCurrentTimestamp();

                            if (TimeHelper.SubtractTimestam(now_timestamp, timestamp) <= ConfigHelper.GetInt("TimestampTimeOut"))//数据通信延迟有效时间3秒
                            {
                                rst = true;
                            }
                            else
                            {
                                rst = false;
                            }
                            break;
                        }
                    }
                    return rst;
                }
            }
            catch (Exception e)
            {
                DefaultLogService.AddOperationLog(5, e, "", "Timestamp Error!");
                NLogHelper.WriteException("Timestamp Error!", e);
                return false;
            }
        }
        public static Dictionary<string, string> GetRequestParams(HttpContext context)
        {
            var pd = new Dictionary<string, string>();

            if (context.Request.Method == "POST")
            {
                var pv = GetPostValues(context);
                if (pv.Count > 0)
                {
                    return pv;
                }
            }

            if (context.Request.QueryString.HasValue)
            {
                var q = context.Request.QueryString.Value;
                if (!q.IsNullOrEmpty() && q != "?")
                {
                    pd.Add("GET", q);
                }
            }
            return pd;
        }

        /// <summary>
        /// 读取request 的提交内容
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetPostValues(HttpContext context)
        {
            var data = new Dictionary<string, string>();
            try
            {
                if (context.Request.Form != null)
                {
                    var f1 = context.Request.Form;
                    foreach (var formKey in f1.Keys)
                    {
                        if (data.ContainsKey(formKey))
                        {
                            data[formKey] = f1[formKey];
                        }
                        else
                        {
                            data.Add(formKey, f1[formKey]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //ignored
            }
            var f2 = context.Request.Query;
            foreach (var formKey in f2.Keys)
            {
                if (data.ContainsKey(formKey))
                {
                    data[formKey] = f2[formKey];
                }
                else
                {
                    data.Add(formKey, f2[formKey]);
                }
            }
            return data;
        }

    }

}
