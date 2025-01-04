using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSN.AI.Entity
{
    public enum ErrorCode
    {
        Error = 40001,
        DataEmpty = 40002,
        ParameterError = 40003,
        ServerError = 40004,

        AccountError = 50001,
        AccountLock = 50002,
        VCodeError = 50003,
        VCodeDuplicateRequest = 50004,
        PasswordError = 50005,

        NoModel = 501001,
        NoInputs = 501002,
        Locked = 501003,

        DataFormatError = 60001,
        DataAlreadyExists = 60002,

        WeixinMiniAppError = 70001,
        WeixinMiniAppRequestError = 70002,
        WeixinMiniAppMemberAcctokenError = 70003,

        TokenCheckError = 80001,
        MemberTokenCheckError = 80002,
        RefreshTokenError = 80003,
        RefreshTokenErrorNODeviceID = 80004,

        SignError = 90001,
        MemberSignError = 90002,
        TimestampError = 90003,
    }

    public enum SystemStatus
    {
        Normal = 0,
        Locked = -1,
        Unpublished = 0,
        Published = 1,
        Unaudited = 0,
        Audited = 1,
    }
    /// <summary>
    /// 返回消息
    /// </summary>
    public class JsonMsg<T> where T : class
    {
        /// <summary>
        /// 成功失败
        /// </summary>
        public bool Status { get; set; }
        public bool Success { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int ErrorCode { get; set; }
        public string SessionID { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string ErrorDesc { get; set; } = string.Empty;

        /// <summary>
        /// 内容
        /// </summary>
        public T Data { get; set; }


        public static JsonMsg<T> OK(T obj, string msg = "Success", string SessionID = "")
        {
            return new JsonMsg<T>() { Status = true, Success = true, ErrorCode = 0, ErrorDesc = msg, SessionID = SessionID, Data = obj };
        }

        public static JsonMsg<T> Error(T obj, ErrorCode errorCode)
        {
            return new JsonMsg<T>() { Status = false, Success = false, ErrorCode = (int)errorCode, ErrorDesc = errorCode.ToString(), Data = obj };
        }
    }
}
