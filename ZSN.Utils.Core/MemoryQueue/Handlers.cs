using System.Collections.Generic;

namespace ZSN.Utils.Core.MemoryQueue
{
    /// <summary>
    /// 消息消费委托
    /// </summary>
    /// <param name="messages">本次消费的消息列表</param>
    public delegate void ConsumeMessagesHandler<T>(List<T> messages);
}
