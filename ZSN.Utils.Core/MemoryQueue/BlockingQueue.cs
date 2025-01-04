using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ZSN.Utils.Core.MemoryQueue
{
    /// <summary>
    /// 实现java中的LinkedBlockingQueue的阻塞队列特性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BlockingQueue<T>
    {
        private readonly Queue<T> _queue = new Queue<T>();
        public int MaxSize { get; private set; }
        public string Name { get; set; }

        /// <summary>
        /// 获取当前队列长度
        /// </summary>
        public int Count
        {
            get { return _queue.Count; }
        }
        /// <summary>
        /// 获取队列中所有元素
        /// </summary>
        public List<T> AllItems
        {
            get
            {
                lock (_queue)
                {
                    return _queue.ToList();
                }
            }
        }

        public BlockingQueue(string name, int maxSize = 1000)
        {
            Name = name;
            MaxSize = maxSize;
        }
        /// <summary>
        /// 入队，如果队列已满，阻塞调用
        /// </summary>
        /// <param name="item"></param>
        public void Put(T item)
        {
            lock (_queue)
            {
                while (_queue.Count >= MaxSize)
                {
                    Monitor.Wait(_queue);
                }
                _queue.Enqueue(item);
                if (_queue.Count == 1)
                {
                    Monitor.PulseAll(_queue);
                }
            }
        }
        /// <summary>
        /// 入队，如果队列已满则直接上抛异常
        /// </summary>
        /// <param name="item"></param>
        public void QuickPut(T item)
        {
            if (_queue.Count >= MaxSize)
            {
                return;
                //throw new BlockingQueueOverflowException(this.Name);
            }
            lock (_queue)
            {
                if (_queue.Count >= MaxSize)
                {
                    return;
                    throw new BlockingQueueOverflowException(Name);
                }
                _queue.Enqueue(item);
                if (_queue.Count == 1)
                {
                    Monitor.PulseAll(_queue);
                }
            }
        }
        /// <summary>
        /// 出队，如果队列为空，阻塞调用
        /// </summary>
        /// <returns></returns>
        public T Take()
        {
            lock (_queue)
            {
                while (_queue.Count == 0)
                {
                    Monitor.Wait(_queue);
                }
                T item = _queue.Dequeue();
                if (_queue.Count == MaxSize - 1)
                {
                    Monitor.PulseAll(_queue);
                }
                return item;
            }
        }
        /// <summary>
        /// 出队指定数量的消息，如果消息不足数量，则有多少返回多少
        /// </summary>
        /// <returns></returns>
        public List<T> QuickTake(int takeNumber = 10)
        {
            List<T> lst = new List<T>();
            if (_queue.Count > 0)
            {
                lock (_queue)
                {
                    while (_queue.Count > 0)
                    {
                        T item = _queue.Dequeue();
                        lst.Add(item);
                        if (lst.Count == takeNumber)
                        {
                            break;
                        }
                    }
                }
            }
            return lst;
        }

        /// <summary>
        /// 获取队首元素，如果超过指定时间队列中没有元素则返回Null
        /// </summary>
        /// <param name="timeout">超时时间，毫秒</param>
        /// <returns></returns>
        public T Poll(int timeout)
        {
            lock (_queue)
            {
                if (_queue.Count == 0)
                {
                    if (Monitor.Wait(_queue, timeout))
                    {
                        T item = _queue.Dequeue();
                        return item;
                    }
                }
                else
                {
                    if (_queue.Count == MaxSize - 1)
                    {
                        Monitor.PulseAll(_queue);
                    }
                    return _queue.Dequeue();
                }
                return default;
            }
        }
        /// <summary>
        /// 重排目标队列，将临时队列中的元素放到队首中
        /// </summary>
        /// <param name="target"></param>
        public void RebuildQueue(List<T> target)
        {
            lock (_queue)
            {
                while (_queue.Count > 0)
                {
                    target.Add(_queue.Dequeue());
                }
                for (int i = 0; i < target.Count; i++)
                {
                    _queue.Enqueue(target[i]);
                }
            }
        }
        public void Clear()
        {
            lock (_queue)
            {
                _queue.Clear();
            }
        }
    }
}
