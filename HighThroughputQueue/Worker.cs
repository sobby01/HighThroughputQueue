using Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighThroughputQueue
{
    public class Worker<T> : IWorker<T>
    {
        private volatile bool isWorking;
        private ConcurrentDictionary<string, ConcurrentQueue<T>> queues = new ConcurrentDictionary<string, ConcurrentQueue<T>>();

        public void StartWorking()
        {
            isWorking = true;
            Task.Run(() =>
            {
                while (isWorking)
                {
                    foreach (var queue in queues)
                    {
                        if (queue.Value.TryDequeue(out T message))
                        {
                            ProcessMessage(queue.Key, message);
                        }
                        else
                        {
                            // Optionally introduce a delay when the queue is empty before retrying
                            Thread.Sleep(100);
                        }
                    }
                }
            });
        }

        public void StopWorking()
        {
            isWorking = false;
        }

        public void AttachQueue(string queueName, ConcurrentQueue<T> queue)
        {
            queues.TryAdd(queueName, queue);
        }

        public void ProcessMessage(string queueName, T message)
        {
            Console.WriteLine($"Processing message in queue '{queueName}': {message}");
            // Simulate processing time
            Thread.Sleep(1000);
        }
    }
}
