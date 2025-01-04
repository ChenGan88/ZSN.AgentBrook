using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XiGua.Bussiness.Cache
{
    /// <summary>
    /// 缓存锁接口
    /// </summary>
    public interface ICacheLock : IDisposable
    {
        /// <summary>
        /// 是否成功获得锁
        /// </summary>
        bool LockSuccessful { get; set; }

        ///// <summary>
        ///// 立即开始锁定
        ///// </summary>
        ///// <returns></returns>
        //ICacheLock LockNow();

        /// <summary>
        /// 开始锁
        /// </summary>
        /// <param name="resourceName">资源名称，即锁的标识</param>
        bool Lock(string resourceName);

        /// <summary>
        /// 开始锁，并设置重试条件
        /// </summary>
        /// <param name="resourceName">资源名称，即锁的标识</param>
        /// <param name="retryCount">重试次数</param>
        /// <param name="retryDelay">每次重试延时</param>
        /// <returns></returns>
        bool Lock(string resourceName, int retryCount, TimeSpan retryDelay);

        //bool IsLocked(string resourceName);

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="resourceName">需要释放锁的 Key，即锁的标识</param>
        void UnLock(string resourceName);
    }
}
