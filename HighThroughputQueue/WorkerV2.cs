using Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighThroughputQueue
{
    public class WorkerV2<T> : IWorkerV2<T>
    {
        private volatile bool isWorking;
        private ConcurrentDictionary<string, ConcurrentQueue<T>> partitions = new ConcurrentDictionary<string, ConcurrentQueue<T>>();
        private string partition;

        public WorkerV2(string partition)
        {
            this.partition = partition;
        }

        public void StartWorking()
        {
            isWorking = true;
            Task.Factory.StartNew(() =>
            {
                while (isWorking && partitions.TryGetValue(partition, out ConcurrentQueue<T> queue))
                {
                    if (queue.TryDequeue(out T message))
                    {
                        ProcessMessage(message);
                    }
                    else
                    {
                        // Optionally introduce a delay when the queue is empty before retrying
                        Thread.Sleep(100);
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void StopWorking()
        {
            isWorking = false;
        }

        public void AttachPartition(string partition, ConcurrentQueue<T> queue)
        {
            partitions.TryAdd(partition, queue);
        }

        public void ProcessMessage(T message)
        {
            Console.WriteLine($"Processing message in partition '{partition}': {message}");
            Thread.Sleep(1000);
        }
    }
}
