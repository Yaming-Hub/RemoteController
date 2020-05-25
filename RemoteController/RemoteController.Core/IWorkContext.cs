using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteController.Core
{
    public interface IWorkContext
    {
        bool TryGetVariable<T>(string name, out T value);

        void SetVariable<T>(string name, T value);
    }
}
