using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ZSN.Utils.Core.Extensions
{
    public static class HttpContextExtensions
    {
        //public static string GetClientUserIp(this HttpContext context)
        //{
        //    var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        //    if (string.IsNullOrEmpty(ip))
        //    {
        //        ip = context.Connection.RemoteIpAddress.ToString();
        //    }

        //    return ip;
        //}
    }
}
