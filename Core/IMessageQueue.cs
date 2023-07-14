namespace Core
{
    public interface IMessageQueue<T>
    {
        void Enqueue(string queueName, T message);
        void AttachWorker(string queueName, IWorker<T> worker);
    }
}