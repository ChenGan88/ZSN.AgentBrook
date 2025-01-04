using System;

namespace ZSN.Utils.Core.MemoryQueue
{
    public class BlockingQueueOverflowException : ApplicationException
    {
        public BlockingQueueOverflowException(string message)
            : base(message)
        {
        }
    }
}
