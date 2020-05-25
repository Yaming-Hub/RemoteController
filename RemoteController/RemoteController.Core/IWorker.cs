using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RemoteController.Core
{
    public interface IWorker
    {
        Task DoWork(IWorkContext context);
    }
}
