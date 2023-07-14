using HighThroughputQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueClient
{
    internal class QueueProcessor1
    {
        public void Process()
        {
            HighThroughputQueue_1<string> queue = new HighThroughputQueue_1<string>(4, ProcessItem);
            queue.StartProcessing();

            // Enqueue some items
            queue.Enqueue("Item 1");
            queue.Enqueue("Item 2");
            queue.Enqueue("Item 3");

            // Wait for processing to complete
            Thread.Sleep(2000);

            // Enqueue more items
            queue.Enqueue("Item 4");
            queue.Enqueue("Item 5");

            // Wait for processing to complete
            Thread.Sleep(2000);
            Console.ReadKey();
        }

        public void ProcessItem(string item)
        {
            // Simulate processing time
            Thread.Sleep(1000);

            Console.WriteLine($"Processed: {item}");
        }
    }
}