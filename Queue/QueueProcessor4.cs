using Core;
using HighThroughputQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueClient
{
    internal class QueueProcessor4
    {
        public void Process()
        {
            IMessageQueue<string> multiQueue = new MessageQueue<string>();

            // Create workers
            IWorker<string> worker1 = new Worker<string>();
            IWorker<string> worker2 = new Worker<string>();


            // Attach workers to queues
            multiQueue.AttachWorker("Queue1", worker1);
            multiQueue.AttachWorker("Queue2", worker2);

            // Start workers
            worker1.StartWorking();
            worker2.StartWorking();

            // Enqueue messages
            multiQueue.Enqueue("Queue1", "Message 1");
            multiQueue.Enqueue("Queue2", "Message 2");
            multiQueue.Enqueue("Queue1", "Message 3");

            // Simulate processing time
            Thread.Sleep(2000);

            // Stop workers
            worker1.StopWorking();
            worker2.StopWorking();
        }
    }
}
