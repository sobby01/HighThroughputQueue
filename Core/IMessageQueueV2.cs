namespace Core
{
    public interface IMessageQueueV2<T>
    {
        void Enqueue(T message);
        void AttachWorker(IWorkerV2<T> worker);

        IEnumerable<IWorkerV2<T>> GetWorkers();
    }
}