using System.Collections.Concurrent;

namespace HighThroughputQueue
{
    public class HighThroughputQueue_1<T>
    {
        private ConcurrentQueue<T> concurrentQueue = new ConcurrentQueue<T>();
        private SemaphoreSlim semaphore = new SemaphoreSlim(0);
        private int workers;
        private Action<T> action;


        public HighThroughputQueue_1(int numWorkers, Action<T> processAction)
        {
            this.workers = numWorkers;
            this.action = processAction;
        }

        public void Enqueue(T item)
        {
            concurrentQueue.Enqueue(item);
            semaphore.Release();
        }

        public void StartProcessing()
        {
            for (int i = 0; i < workers; i++)
            {
                Task.Run(ProcessItems);
            }
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