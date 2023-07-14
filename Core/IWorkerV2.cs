using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IWorkerV2<T>
    {
        void StartWorking();
        void StopWorking();
        void ProcessMessage(T message);
    }
}
