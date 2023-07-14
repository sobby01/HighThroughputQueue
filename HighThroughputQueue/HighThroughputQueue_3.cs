using System.Collections.Concurrent;

namespace HighThroughputQueue
{
    public class HighThroughputQueue_3<T>
    {
        private ConcurrentQueue<T> concurrentQueue = new ConcurrentQueue<T>();
        private SemaphoreSlim semaphore = new SemaphoreSlim(0);
        private int maxQueueSize;
        private Action<T> action;

        public HighThroughputQueue_3(int maxQueueSize, Action<T> processAction)
        {
            this.maxQueueSize = maxQueueSize;
            this.action = processAction;
        }

        public void Enqueue(T item)
        {
            if (concurrentQueue.Count < maxQueueSize)
            {
                concurrentQueue.Enqueue(item);
                semaphore.Release();
            }
            else
            {
                Console.WriteLine($"Queue is full. Unable to enqueue item: {item}");
            }
        }

        public void StartProcessing()
        {
            Task.Run(ProcessItems);
        }

        private async Task ProcessItems()
        {
            while (true)
            {
                await semaphore.WaitAsync();
                if (concurrentQueue.TryDequeue(out T item))
                {
                    action(item);
                }
            }
        }
    }
}