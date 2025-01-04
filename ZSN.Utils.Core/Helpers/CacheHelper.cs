using System;
using System.Collections;

namespace ZSN.GovBiddingSystem.Web.Helper
{
    /// <summary>
    /// CacheHelper Web缓存帮助类 添加,移除,读取缓存
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 通过缓存键获取相应的内容
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            return (T)HttpRuntime.Cache[key];
        }

        /// <summary>
        /// 判断是否有此key的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Contains(string key)
        {
            if (HttpRuntime.Cache[key] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="CacheTimeOption">缓存的时间长短</param>
        /// <param name="dependencies">缓存依赖项</param>
        /// <param name="cacheItemPriority">优先级</param>
        public static void Set<T>(string key, T value, CacheTimeOption CacheTimeOption, CacheExpirationOption CacheExpirationOption, CacheDependency dependencies, CacheItemPriority cacheItemPriority)
        {
            if (value == null)
                return;
            DateTime absoluteExpiration = GetAbsoluteExpirationTime(CacheTimeOption, CacheExpirationOption);
            TimeSpan slidingExpiration = GetSlidingExpirationTime(CacheTimeOption, CacheExpirationOption);
            HttpRuntime.Cache.Add(key, value, dependencies, absoluteExpiration, slidingExpiration, cacheItemPriority, null);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="CacheTimeOption">缓存时间</param>
        /// <param name="CacheExpirationOption">缓存过期时间类别（绝对/弹性）</param>
        /// <param name="dependencies">缓存依赖项</param>
        public static void Set<T>(string key, T value, CacheTimeOption CacheTimeOption, CacheExpirationOption CacheExpirationOption, CacheDependency dependencies)
        {
            Set<T>(key, value, CacheTimeOption, CacheExpirationOption, dependencies, CacheItemPriority.NotRemovable);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="CacheTimeOption">缓存时间</param>
        /// <param name="CacheExpirationOption">缓存过期时间类别（绝对/弹性）</param>
        public static void Set<T>(string key, T value, CacheTimeOption CacheTimeOption, CacheExpirationOption CacheExpirationOption)
        {
            Set<T>(key, value, CacheTimeOption, CacheExpirationOption, null, CacheItemPriority.NotRemovable);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        /// <param name="CacheTimeOption">缓存时间</param>
        public static void Set<T>(string key, T value, CacheTimeOption CacheTimeOption)
        {
            Set<T>(key, value, CacheTimeOption, CacheExpirationOption.SlidingExpiration);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">值</param>
        public static void Set<T>(string key, T value)
        {
            Set<T>(key, value, CacheTimeOption.Normal);
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void Clear()
        {
            foreach (DictionaryEntry elem in HttpRuntime.Cache)
            {
                HttpRuntime.Cache.Remove(elem.Key.ToString());
            }
        }
        /// <summary>
        /// 移除相关关键字的缓存
        /// </summary>
        /// <param name="keyword">缓存关键字</param>
        public static void Remove(string keyword)
        {
            foreach (DictionaryEntry elem in HttpRuntime.Cache)
            {
                if (elem.Key.ToString().Contains(keyword))
                {
                    HttpRuntime.Cache.Remove(elem.Key.ToString());
                }
            }
        }

        /// <summary>
        /// 清除某项缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        public static void RemoveAt(string key)
        {
            if (HttpRuntime.Cache[key] != null)
            {
                HttpRuntime.Cache.Remove(key);
            }
        }

        /// <summary>
        /// 获取绝对日期时间
        /// </summary>
        /// <param name="CacheTimeOption">缓存的时间长短</param>
        /// <returns></returns>
        public static DateTime GetAbsoluteExpirationTime(CacheTimeOption CacheTimeOption, CacheExpirationOption CacheExpirationOption)
        {
            if (CacheExpirationOption == CacheExpirationOption.SlidingExpiration
                || CacheTimeOption == CacheTimeOption.NotRemovable)
            {
                return Cache.NoAbsoluteExpiration;
            }

            return DateTime.Now.AddMinutes((int)CacheTimeOption);
        }

        /// <summary>
        /// 获取弹性时间过期时间
        /// </summary>
        /// <param name="CacheTimeOption">缓存的时间长短</param>
        /// <returns></returns>
        private static TimeSpan GetSlidingExpirationTime(CacheTimeOption CacheTimeOption, CacheExpirationOption CacheExpirationOption)
        {
            if (CacheExpirationOption == CacheExpirationOption.AbsoluteExpiration
                || CacheTimeOption == CacheTimeOption.NotRemovable)
            {
                return Cache.NoSlidingExpiration;
            }

            return TimeSpan.FromMinutes((int)CacheTimeOption);
        }

        #region 2010-04-26带更新事件缓存方法
        /// <summary>
        /// 缓存数据源更新委托
        /// </summary>
        /// <returns></returns>
        public delegate object RefreshCacheDataHandler();
        /// <summary>
        /// 带ref int的缓存数据源更新委托，一般用于分页方法
        /// </summary>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public delegate object RefreshCacheDataWithRefParamHandler(ref int recordCount);
        /// <summary>
        /// 带out int的缓存数据源更新委托，一般用于分页方法
        /// </summary>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public delegate object RefreshCacheDataWithOutParamHandler(out int recordCount);
        /// <summary>
        /// 获取指定key缓存数据，如果该缓存不存在，则自动调用数据源委托生成缓存
        /// </summary>
        /// <typeparam name="T">缓存数据的类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="cacheTime">缓存时间</param>
        /// <param name="callback">数据源委托</param>
        /// <returns></returns>
        public static T Get<T>(string key, CacheTimeOption cacheTime, RefreshCacheDataHandler callback) where T : class
        {
            if (Contains(key))
            {
                return Get<T>(key);
            }
            T content = callback() as T;
            if (content != null)
            {
                Set<T>(key, content, cacheTime, CacheExpirationOption.AbsoluteExpiration);
            }
            return content;
        }
        /// <summary>
        /// 获取指定key缓存数据，如果该缓存不存在，则自动调用数据源委托生成缓存
        /// </summary>
        /// <typeparam name="T">缓存数据的类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="recordCountKey">记录总数的缓存key</param>
        /// <param name="recordCount">记录总数(ref)</param>
        /// <param name="cacheTime">缓存时间</param>
        /// <param name="callback">数据源委托</param>
        /// <returns></returns>
        public static T Get<T>(string key, string recordCountKey, ref int recordCount, CacheTimeOption cacheTime, RefreshCacheDataWithRefParamHandler callback) where T : class
        {
            if (Contains(recordCountKey))
            {
                recordCount = Get<int>(recordCountKey);
            }
            if (Contains(key))
            {
                return Get<T>(key);
            }
            T content = callback(ref recordCount) as T;
            Set<T>(key, content, cacheTime, CacheExpirationOption.AbsoluteExpiration);
            Set<int>(recordCountKey, recordCount, cacheTime, CacheExpirationOption.AbsoluteExpiration);
            return content;
        }
        /// <summary>
        /// 获取指定key缓存数据，如果该缓存不存在，则自动调用数据源委托生成缓存
        /// </summary>
        /// <typeparam name="T">缓存数据的类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="recordCountKey">记录总数的缓存key</param>
        /// <param name="recordCount">记录总数(out)</param>
        /// <param name="cacheTime">缓存时间</param>
        /// <param name="callback">数据源委托</param>
        /// <returns></returns>
        public static T Get<T>(string key, string recordCountKey, out int recordCount, CacheTimeOption cacheTime, RefreshCacheDataWithOutParamHandler callback) where T : class
        {
            recordCount = 0;
            if (Contains(recordCountKey))
            {
                recordCount = Get<int>(recordCountKey);
            }
            if (Contains(key))
            {
                return Get<T>(key);
            }
            T content = callback(out recordCount) as T;
            Set<T>(key, content, cacheTime, CacheExpirationOption.AbsoluteExpiration);
            Set<int>(recordCountKey, recordCount, cacheTime, CacheExpirationOption.AbsoluteExpiration);
            return content;
        }
        /// <summary>
        /// 获取指定key缓存数据，如果该缓存不存在，则自动调用数据源委托生成缓存(线程安全，但会造成一定性能开销)
        /// </summary>
        /// <typeparam name="T">缓存数据的类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="cacheTime">缓存时间</param>
        /// <param name="callback">数据源委托</param>
        /// <returns></returns>
        public static T GetWithLock<T>(string key, CacheTimeOption cacheTime, RefreshCacheDataHandler callback) where T : class
        {
            if (Contains(key))
            {
                return Get<T>(key);
            }
            lock (string.Intern(key))
            {
                if (Contains(key))
                {
                    return Get<T>(key);
                }
                else
                {
                    T content = callback() as T;
                    Set<T>(key, content, cacheTime, CacheExpirationOption.AbsoluteExpiration);
                    return content;
                }
            }
        }
        /// <summary>
        /// 获取指定key缓存数据，如果该缓存不存在，则自动调用数据源委托生成缓存(线程安全，但会造成一定性能开销)
        /// </summary>
        /// <typeparam name="T">缓存数据的类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="recordCountKey">记录总数的缓存key</param>
        /// <param name="recordCount">记录总数(ref)</param>
        /// <param name="cacheTime">缓存时间</param>
        /// <param name="callback">数据源委托</param>
        /// <returns></returns>
        public static T GetWithLock<T>(string key, string recordCountKey, ref int recordCount, CacheTimeOption cacheTime, RefreshCacheDataWithRefParamHandler callback) where T : class
        {
            if (Contains(recordCountKey))
            {
                recordCount = Get<int>(recordCountKey);
            }
            if (Contains(key))
            {
                return Get<T>(key);
            }
            lock (string.Intern(key))
            {
                if (Contains(recordCountKey))
                {
                    recordCount = Get<int>(recordCountKey);
                }
                if (Contains(key))
                {
                    return Get<T>(key);
                }
                else
                {
                    T content = callback(ref recordCount) as T;
                    Set<T>(key, content, cacheTime, CacheExpirationOption.AbsoluteExpiration);
                    Set<int>(recordCountKey, recordCount, cacheTime, CacheExpirationOption.AbsoluteExpiration);
                    return content;
                }
            }
        }
        /// <summary>
        /// 获取指定key缓存数据，如果该缓存不存在，则自动调用数据源委托生成缓存(线程安全，但会造成一定性能开销)
        /// </summary>
        /// <typeparam name="T">缓存数据的类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="recordCountKey">记录总数的缓存key</param>
        /// <param name="recordCount">记录总数(out)</param>
        /// <param name="cacheTime">缓存时间</param>
        /// <param name="callback">数据源委托</param>
        /// <returns></returns>
        public static T GetWithLock<T>(string key, string recordCountKey, out int recordCount, CacheTimeOption cacheTime, RefreshCacheDataWithOutParamHandler callback) where T : class
        {
            recordCount = 0;
            if (Contains(recordCountKey))
            {
                recordCount = Get<int>(recordCountKey);
            }
            if (Contains(key))
            {
                return Get<T>(key);
            }
            lock (string.Intern(key))
            {
                if (Contains(recordCountKey))
                {
                    recordCount = Get<int>(recordCountKey);
                }
                if (Contains(key))
                {
                    return Get<T>(key);
                }
                else
                {
                    T content = callback(out recordCount) as T;
                    Set<T>(key, content, cacheTime, CacheExpirationOption.AbsoluteExpiration);
                    Set<int>(recordCountKey, recordCount, cacheTime, CacheExpirationOption.AbsoluteExpiration);
                    return content;
                }
            }
        }
        #endregion 
    }


    /// <summary>
    /// 过期方式
    /// </summary>
    public enum CacheExpirationOption
    {
        /// <summary>
        /// 绝对过期
        /// </summary>
        AbsoluteExpiration = 0,
        /// <summary>
        /// 弹性过期

        /// </summary>
        SlidingExpiration = 1
    }

    /// <summary>
    /// 常用过期时间
    /// </summary>
    public enum CacheTimeOption
    {
        /// <summary>
        /// 永不过期
        /// </summary>
        NotRemovable = 0,
        /// <summary>
        /// 短时间 10分钟
        /// </summary>
        Short = 10,
        /// <summary>
        /// 一般正常过期时间30分钟
        /// </summary>
        Normal = 30,
        /// <summary>
        /// 长时间60分钟
        /// </summary>
        Long = 60,
        /// <summary>
        /// 半天时间
        /// </summary>
        HalfDay = 720,
    }

}
