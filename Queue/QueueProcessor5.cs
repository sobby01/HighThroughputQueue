using Core;
using HighThroughputQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueClient
{
    internal class QueueProcessor5
    {
        public void Process()
        {
            int partitionCount = 3;
            int workersPerPartition = 2;

            IMessageQueueV2<string> messageQueue = new MessageQueueV2<string>(partitionCount);

            // Create workers for each partition
            for (int i = 1; i <= partitionCount; i++)
            {
                for (int j = 1; j <= workersPerPartition; j++)
                {
                    string partition = $"Partition{i}";
                    IWorkerV2<string> worker = new WorkerV2<string>(partition);
                    messageQueue.AttachWorker(worker);
                }
            }

            List<IWorkerV2<string>> workers = messageQueue.GetWorkers().ToList();
            foreach (IWorker<string> worker in workers)
            {
                worker.StartWorking();
            }

            messageQueue.Enqueue("Message 1");
            messageQueue.Enqueue("Message 2");
            messageQueue.Enqueue("Message 3");

            // Simulate processing time
            Thread.Sleep(2000);

            foreach (IWorkerV2<string> worker in workers)
            {
                worker.StopWorking();
            }
        }
    }
}
