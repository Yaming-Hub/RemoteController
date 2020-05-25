using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteController.ProcessManager.Contracts
{
    public class ProcessInfo
    {
        public uint ProcessId { get; set; }

        public string Name { get; set; }

        public string ExecutablePath { get; set; }

        public string CommandLine { get; set; }

        public DateTime? CreationDate { get; set; }

        public string Owner { get; set; }
    }
}
