using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RemoteController.Core
{
    public interface IEntityLogger<T>
    {
        Task LogAsync(T entity, IWorkContext context);
    }
}
