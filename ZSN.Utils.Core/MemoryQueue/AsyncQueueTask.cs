using System;
using System.Threading;
using ZSN.Utils.Core.Helpers;

namespace ZSN.Utils.Core.MemoryQueue
{
    /// <summary>
    /// 异步队列任务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncQueueTask<T> where T : class
    {
        private Thread _tdTask;
        private BlockingQueue<T> _messageQueue;
        private int _takeNumber;
        private ConsumeMessagesHandler<T> _consumer;
        private bool _stop = false;
        private int _sleepInterval;

        /// <summary>
        /// 获取当前的消息数
        /// </summary>
        public int Count
        {
            get
            {
                return _messageQueue.Count;
            }
        }

        /// <summary>
        /// 初始化异步队列任务
        /// </summary>
        /// <param name="name">任务名称</param>
        /// <param name="consumer">消费委托</param>
        /// <param name="takeNumber">每次消费的数量</param>
        /// <param name="maxSize">内存中持有的最大消息数，超过该消息数量，调用PutMessage会外抛异常</param>
        /// <param name="sleepInterval">如果队列不饱和的情况下，消费线程的休眠间隔,默认为1000毫秒</param>
        public AsyncQueueTask(string name, ConsumeMessagesHandler<T> consumer, int takeNumber = 100, int maxSize = 10000, int sleepInterval = 1000)
        {
            _messageQueue = new BlockingQueue<T>(name, maxSize);
            _takeNumber = takeNumber;
            _consumer = consumer;
            _sleepInterval = sleepInterval;

            _tdTask = new Thread(new ThreadStart(TaskHandler));
            _tdTask.IsBackground = true;
            _tdTask.Start();
        }

        private void TaskHandler()
        {
            while (!_stop)
            {
                try
                {
                    var lst = _messageQueue.QuickTake(_takeNumber);
                    var interval = lst.Count < _takeNumber ? _sleepInterval : 0;
                    if (lst.Count > 0)
                    {
                        _consumer(lst);
                    }
                    //如果当前队列中的消息数不足，则休眠1秒
                    if (interval > 0)
                    {
                        Thread.Sleep(interval);
                    }
                }
                catch (Exception ex)
                {
                    //消费过程的异常需要业务方在委托中处理，
                    //这里的Catch为保证遗漏的异常引起线程的退出
                    NLogHelper.WriteException("consumer委托引发异常", ex);
                    Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// 如果队列已经满了则直接上抛异常
        /// </summary>
        /// <param name="msg"></param>
        public void PutMessage(T msg)
        {
            _messageQueue.QuickPut(msg);
        }
        /// <summary>
        /// 停止任务
        /// </summary>
        public void Stop()
        {
            _stop = true;
        }
    }
}
