using Core;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace HighThroughputQueue
{
    public class MessageQueue<T> : IMessageQueue<T>
    {
        private ConcurrentDictionary<string, ConcurrentQueue<T>> concurrentQueues = new ConcurrentDictionary<string, ConcurrentQueue<T>>();
        private ConcurrentDictionary<string, IWorker<T>> workers = new ConcurrentDictionary<string, IWorker<T>>();
        private event Action<string, T> MessageEnqueued;

        public void Enqueue(string queueName, T message)
        {
            if (!concurrentQueues.TryGetValue(queueName, out ConcurrentQueue<T> queue))
            {
                queue = new ConcurrentQueue<T>();
                concurrentQueues.TryAdd(queueName, queue);
            }

            queue.Enqueue(message);
            OnMessageEnqueued(queueName, message);
        }

        public void AttachWorker(string queueName, IWorker<T> worker)
        {
            workers.TryAdd(queueName, worker);
            MessageEnqueued += worker.ProcessMessage;
        }

        private void OnMessageEnqueued(string queueName, T message)
        {
            MessageEnqueued?.Invoke(queueName, message);
        }
    }
}