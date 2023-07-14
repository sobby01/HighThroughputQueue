using HighThroughputQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueClient
{
    internal class QueueProcessor3
    {
        public void Process()
        {
            HighThroughputQueue_3<string> queue = new HighThroughputQueue_3<string>(100, ProcessItem);
            queue.StartProcessing();

            Task.Run(() => EnqueueItems(queue));

            Thread.Sleep(5000);
        }

        void EnqueueItems(HighThroughputQueue_3<string> queue)
        {
            for (int i = 0; i < 10; i++)
            {
                string item = $"Item {i}";
                queue.Enqueue(item);

                //simulate delay between enqueueing items
                Thread.Sleep(100);
            }
        }

        public void ProcessItem(string item)
        {
            //simulate processing time
            Thread.Sleep(1000);

            Console.WriteLine($"Processed the item: {item}");
        }
    }
}
