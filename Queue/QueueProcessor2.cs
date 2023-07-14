using Core;
using HighThroughputQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueClient
{
    internal class QueueProcessor2
    {
        public void Process()
        {
            int noOfQueues = 2;
            int noOfWorkers = 4;

            HighThroughputQueue_2<QueueContent>[] queues = new HighThroughputQueue_2<QueueContent>[noOfQueues];
            // Create queues
            for (int i = 0; i < noOfQueues; i++)
            {
                queues[i] = new HighThroughputQueue_2<QueueContent>(noOfWorkers, ProcessItem, $"queue {i}");
                queues[i].StartProcessing();
            }

            // Enqueue items across multiple queues
            //We  can implement another sort of hashign or randomization  to push items in the queues
            for (int i = 0; i < 10; i++)
            {
                string item = $"Item {i}";
                int queueIndex = i % noOfQueues;
                QueueContent qc = new QueueContent()
                {
                    Item = item,
                    Name = queues[queueIndex].Name
                };
                
                queues[queueIndex].Enqueue(qc);
            }

            Thread.Sleep(1000);
            Console.ReadKey();
        }

        public void ProcessItem(QueueContent content)
        {
            // Simulate processing time
            Thread.Sleep(1000);

            Console.WriteLine($"Processed: {content.Item} By: {content.Name}");
        }
    }
}
