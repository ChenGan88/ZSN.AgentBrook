namespace ZSN.Utils.Core.MemoryQueue
{
    public interface IAsyncQueue
    {
        int Count { get; }
        string Name { get; }
        void Stop();
    }
}
