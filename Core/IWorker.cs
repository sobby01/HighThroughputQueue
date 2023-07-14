using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IWorker<T>
    {
        void StartWorking();
        void StopWorking();
        void ProcessMessage(string queueName, T message);
    }
}
