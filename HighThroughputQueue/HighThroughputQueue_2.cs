using System.Collections.Concurrent;

namespace HighThroughputQueue
{
    public class HighThroughputQueue_2<T>
    {
        private ConcurrentQueue<T> concurrentQueue = new ConcurrentQueue<T>();
        private SemaphoreSlim semaphore = new SemaphoreSlim(0);
        private int workers;
        private Action<T> action;
        private string queueName;

        public string Name
        {
            get
            {
                return queueName;
            }
        }

        public HighThroughputQueue_2(int numWorkers, Action<T> processAction, string queueName)
        {
            this.workers = numWorkers;
            this.action = processAction;
            this.queueName = queueName;
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
                if (concurrentQueue.TryDequeue(result: out T item))
                {
                    action(item);
                }
            }
        }
    }
}