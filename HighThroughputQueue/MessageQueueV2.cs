
using Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighThroughputQueue
{
    public class MessageQueueV2<T> : IMessageQueueV2<T>
    {
        private ConcurrentDictionary<string, ConcurrentQueue<T>> partitions = new ConcurrentDictionary<string, ConcurrentQueue<T>>();
        private ConcurrentDictionary<IWorkerV2<T>, string> workerPartitions = new ConcurrentDictionary<IWorkerV2<T>, string>();
        private List<string> availablePartitions = new List<string>();
        private object lockObj = new object();
        private event Action<string, T> MessageEnqueued;
        private event Action<string, IWorkerV2<T>> WorkerAttached;
        private int partitionCounter = 1;
        private int partitionCount;

        public MessageQueueV2(int partitionCount)
        {
            if (partitionCount < 1)
                throw new ArgumentException("Partition count must be greater than zero.", nameof(partitionCount));

            this.partitionCount = partitionCount;
            availablePartitions.AddRange(Enumerable.Range(1, partitionCount).Select(n => $"Partition{n}"));
        }

        public void Enqueue(T message)
        {
            string partition = GetNextAvailablePartition();
            if (!partitions.TryGetValue(partition, out ConcurrentQueue<T> queue))
            {
                lock (lockObj)
                {
                    if (!partitions.TryGetValue(partition, out queue))
                    {
                        queue = new ConcurrentQueue<T>();
                        partitions.TryAdd(partition, queue);
                    }
                }
            }

            queue.Enqueue(message);
            OnMessageEnqueued(partition, message);
        }

        public void AttachWorker(IWorkerV2<T> worker)
        {
            string partition = GetNextAvailablePartition();
            workerPartitions.TryAdd(worker, partition);
            OnWorkerAttached(partition, worker);
        }

        public IEnumerable<IWorkerV2<T>> GetWorkers()
        {
            return workerPartitions.Keys;
        }

        private void OnMessageEnqueued(string partition, T message)
        {
            MessageEnqueued?.Invoke(partition, message);
        }

        private void OnWorkerAttached(string partition, IWorkerV2<T> worker)
        {
            WorkerAttached?.Invoke(partition, worker);
        }

        private string GetNextAvailablePartition()
        {
            lock (lockObj)
            {
                partitionCounter = (partitionCounter % partitionCount) + 1;
                string partition = $"Partition{partitionCounter}";
                return partition;
            }
        }
    }
}
