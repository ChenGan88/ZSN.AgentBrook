using Enyim.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;

namespace XiGua.Bussiness.Cache
{
    public class MemCache
    {
        private static MemcachedClient MemClient;
        static readonly object padlock = new object();
        public static MemcachedClient getInstance(bool isUseConfiguration = true)
        {
            if (MemClient == null)
            {
                lock (padlock)
                {
                    if (MemClient == null)
                    {
                        MemClientInit(isUseConfiguration);
                        MemClient.NodeFailed += p => { throw new Exception("memcache 连接池异常"); };
                    }
                }
            }
            return MemClient;
        }


        public static void ClearInstance()
        {
            MemClient = null;
        }

        private static void MemClientInit(bool isUseConfiguration)
        {
            if (isUseConfiguration)
            {
                MemClient = new MemcachedClient();
               
                return;
            }
            //初始化缓存
            MemcachedClientConfiguration memConfig = new MemcachedClientConfiguration();
            IPAddress newaddress = IPAddress.Parse("101.37.21.136");//外网
                                    //IPAddress.Parse(Dns.GetHostEntry("2f362979be4543b0.m.cnhzalicm10pub001.ocs.aliyuncs.com").AddressList[0].ToString());//替换为OCS地址
            IPEndPoint ipEndPoint = new IPEndPoint(newaddress, 11211);

            // 配置文件 - ip
            memConfig.Servers.Add(ipEndPoint);
            // 配置文件 - 协议
            memConfig.Protocol = MemcachedProtocol.Binary;
            // 配置文件-权限
            memConfig.Authentication.Type = typeof(PlainTextAuthenticator);
            memConfig.Authentication.Parameters["zone"] = "";
            memConfig.Authentication.Parameters["userName"] = "2f362979be4543b0";
            memConfig.Authentication.Parameters["password"] = "Vlifun2016";
            //下面请根据实例的最大连接数进行设置
            memConfig.SocketPool.MinPoolSize = 10;
            memConfig.SocketPool.MaxPoolSize = 100;
            MemClient = new MemcachedClient(memConfig);
        }
    }
}
