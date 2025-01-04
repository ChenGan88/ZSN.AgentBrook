using Enyim.Caching;
using Enyim.Caching.Memcached;
using Just.Library.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace XiGua.Bussiness.Cache
{
    /// <summary>
    /// 基于memcached实现的同步锁
    /// </summary>
    public class MemcachedLock : BaseCacheLock
    {
        private MemcachedClient _cache;
        public MemcachedLock(string resourceName, string key, int retryCount, TimeSpan retryDelay)
            : base(resourceName, key, retryCount, retryDelay)
        {
            _cache = MemCache.getInstance();
            LockNow();//立即等待并抢夺锁
        }

        private static Random _rnd = new Random();

        private string GetLockKey(string resourceName)
        {
            return string.Format("{0}:{1}", "Lock", resourceName);
        }

        private bool RetryLock(string resourceName, int retryCount, TimeSpan retryDelay, Func<bool> action)
        {
            int currentRetry = 0;
            int maxRetryDelay = (int)retryDelay.TotalMilliseconds;
            while (currentRetry++ < retryCount)
            {
                if (action())
                {
                    return true;//取得锁
                }
                Thread.Sleep(_rnd.Next(maxRetryDelay));
            }
            return false;
        }

        public override bool Lock(string resourceName)
        {
            return Lock(resourceName, 10, new TimeSpan(0, 0, 0, 0, 20));
        }

        public override bool Lock(string resourceName, int retryCount, TimeSpan retryDelay)
        {
            var key = GetFinalKey(resourceName);
            var successfull = RetryLock(key, retryCount /*暂时不限制*/, retryDelay, () =>
            {
                try
                {
                    if (_cache.Store(StoreMode.Add, key, new object(), new TimeSpan(0, 0, 10)))
                    {
                        return true;//取得锁
                    }
                    else
                    {
                        return false;//已被别人锁住，没有取得锁
                    }

                    //if (_cache.Get(key) != null)
                    //{
                    //    return false;//已被别人锁住，没有取得锁
                    //}
                    //else
                    //{
                    //    _cache.Store(StoreMode.Set, key, new object(), new TimeSpan(0, 0, 10));//创建锁
                    //    return true;//取得锁
                    //}
                }
                catch (Exception ex)
                {
                    LogHelper.WriteException("Memcached同步锁发生异常", ex);
                    return false;
                }
            });
            return successfull;
        }

        public override void UnLock(string resourceName)
        {
            var key = GetFinalKey(resourceName);
            _cache.Remove(key);
        }

        public string GetFinalKey(string key, bool isFullKey = false)
        {
            return isFullKey ? key : String.Format("XiGua:MemcachedLock:{0}", key);
        }

        public static ICacheLock BeginCacheLock(string resourceName, string key, int retryCount = 0, TimeSpan retryDelay = new TimeSpan())
        {
            return new MemcachedLock(resourceName, key, retryCount, retryDelay);
        }
    }
}
