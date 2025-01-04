using System.Collections.Generic;

namespace ZSN.Utils.Core.MemoryQueue
{
    /// <summary>
    /// 队列管理对象，相同名字的队列实例将被缓存。
    /// </summary>
    public class QueueManager
    {
        private static Dictionary<string, IAsyncQueue> _dictQueue = new Dictionary<string, IAsyncQueue>();

        /// <summary>
        /// 创建一个内存消息队列
        /// </summary>
        /// <typeparam name="T">消息实体类型</typeparam>
        /// <param name="name">队列名称</param>
        /// <param name="consumer">消息的消费委托</param>
        /// <param name="threadNumber">消费消息的任务线程数</param>
        /// <param name="takeNumber">每次最多消费的消息数量</param>
        /// <param name="maxSize">允许保留在队列中的最大消息数</param>
        /// <param name="sleepInterval">如果队列不饱和的情况下，消费线程的休眠间隔,默认为1000毫秒</param>
        /// <returns>如果该队列不存在则创建一个新的队列并缓存，如果已经存在则直接返回</returns>
        public static AsyncQueue<T> CreateQueue<T>(string name, ConsumeMessagesHandler<T> consumer, int threadNumber = 1, int takeNumber = 100, int maxSize = 10000, int sleepInterval = 1000) where T : class
        {
            IAsyncQueue queue;
            if (_dictQueue.TryGetValue(name, out queue) == false)
            {
                lock (_dictQueue)
                {
                    if (_dictQueue.TryGetValue(name, out queue) == false)
                    {
                        queue = new AsyncQueue<T>(name, consumer, threadNumber, takeNumber, maxSize, sleepInterval);
                        _dictQueue.Add(name, queue);
                    }
                }
            }
            return queue as AsyncQueue<T>;
        }
        /// <summary>
        /// 根据名称获取一个内存消息队列
        /// </summary>
        /// <param name="name">队列名称</param>
        /// <returns>如果已经存在则直接返回，如果不存在则返回null</returns>
        public static AsyncQueue<T> GetQueue<T>(string name) where T : class
        {
            IAsyncQueue queue;
            if (_dictQueue.TryGetValue(name, out queue))
            {
                return queue as AsyncQueue<T>;
            }
            return null;
        }
        /// <summary>
        /// 移除一个队列
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveQueue(string name)
        {
            IAsyncQueue queue;
            if (_dictQueue.TryGetValue(name, out queue))
            {
                lock (_dictQueue)
                {
                    _dictQueue.Remove(name);
                    queue.Stop();
                }
            }
        }
        /// <summary>
        /// 停止缓存中所有队列的线程并移除
        /// </summary>
        public static void Clear()
        {
            lock (_dictQueue)
            {
                foreach (string name in _dictQueue.Keys)
                {
                    _dictQueue[name].Stop();
                }
                _dictQueue.Clear();
            }
        }
    }
}
