using System.Collections.Generic;
using System.Threading;

namespace ZSN.Utils.Core.MemoryQueue
{
    /// <summary>
    /// 异步内存队列
    /// </summary>
    public class AsyncQueue<T> : IAsyncQueue where T : class
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取当前内存线程中所有消息数
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                _lstTask.ForEach(task =>
                {
                    count += task.Count;
                });
                return count;
            }
        }

        private List<AsyncQueueTask<T>> _lstTask = new List<AsyncQueueTask<T>>();
        private int _index = -1;

        /// <summary>
        /// 异步内存队列
        /// </summary>
        /// <param name="name">队列名称</param>
        /// <param name="consumeHandler">消费队列委托</param>
        /// <param name="threadNumber">存储队列消息的线程数</param>
        /// <param name="takeNumber">每次消费的数量</param>
        /// <param name="maxSize">内存中持有的最大消息数，超过该消息数量，调用PutMessage会外抛异常</param>
        /// <param name="sleepInterval">如果队列不饱和的情况下，消费线程的休眠间隔,默认为1000毫秒</param>
        public AsyncQueue(string name, ConsumeMessagesHandler<T> consumer, int threadNumber = 1, int takeNumber = 100, int maxSize = 10000, int sleepInterval = 1000)
        {
            Name = name;
            for (int i = 0; i < threadNumber; i++)
            {
                AsyncQueueTask<T> task = new AsyncQueueTask<T>(name, consumer, takeNumber, maxSize, sleepInterval);
                _lstTask.Add(task);
            }
        }

        /// <summary>
        /// 如果队列已经满了则直接上抛异常
        /// </summary>
        /// <param name="msg"></param>
        public void PutMessage(T msg)
        {
            var index = 0;
            //只有超过两个线程数时才进行任务的轮询
            if (_lstTask.Count > 1)
            {
                index = Interlocked.Increment(ref _index);
                if (index >= _lstTask.Count)
                {
                    index = 0;
                    Interlocked.Exchange(ref _index, -1);
                }
            }
            var task = _lstTask[index];
            task.PutMessage(msg);
        }

        /// <summary>
        /// 停止异步队列的任务线程
        /// </summary>
        public void Stop()
        {
            foreach (var task in _lstTask)
            {
                task.Stop();
            }
        }
    }
}
