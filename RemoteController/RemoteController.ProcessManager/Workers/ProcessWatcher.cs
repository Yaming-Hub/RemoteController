using System;
using System.Collections.Generic;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RemoteController.Core;
using RemoteController.ProcessManager.Contracts;

namespace RemoteController.ProcessManager.Workers
{
    public class ProcessWatcher : IWorker
    {
        public Task DoWork(IWorkContext context)
        {
            // Get active processes
            // write active process to log

            return Task.CompletedTask;
        }

        public static IEnumerable<ProcessInfo> GetProcesses(bool includeOwner = false)
        {
            // Query Win32_Process table https://docs.microsoft.com/en-us/windows/win32/wmisdk/wmi-tasks--processes
            // Schema: https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-process
            // ObjectQuery query = new ObjectQuery("SELECT ProcessId, Name, ExecutablePath, CommandLine, CreationDate FROM Win32_Process");
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Process");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject @object in searcher.Get())
            {
                ProcessInfo process = new ProcessInfo
                {
                    ProcessId = (uint)@object["ProcessId"],
                    Name = (string)@object["Name"],
                    ExecutablePath = (string)@object["ExecutablePath"],
                    CommandLine = (string)@object["CommandLine"],
                    CreationDate = ParseWin32ProcessDateTime((string)@object["CreationDate"])
                };

                if (includeOwner)
                {
                    try
                    {
                        // Get owner https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/getowner-method-in-class-win32-process
                        string[] parameters = new String[2];

                        //Invoke the method and populate the o var with the user name and domain
                        @object.InvokeMethod("GetOwner", (object[])parameters);
                        process.Owner = string.IsNullOrEmpty(parameters[1]) ? parameters[0] : $"{parameters[0]}@{parameters[1]}";
                    }
                    catch (InvalidOperationException)
                    {
                    }
                }

                yield return process;
            }
        }

        public static DateTime? ParseWin32ProcessDateTime(string dateTimeString)
        {
            // The Window 32 process time is in "20200522104656.299017-420" format
            // YYYYMMddHHmmss.<milliseconds>-<offset>
            string pattern = @"(?<time>\d+)\.(?<milliseconds>\d+)(?<op>[-|+])(?<offset>\d+)";
            var match = Regex.Match(dateTimeString, pattern);
            if (match.Success)
            {
                // Convert string from "20200522104656.299017-420" to "20200522104656.299017-07:00"
                string time = match.Groups["time"].Value;
                string milliseconds = match.Groups["milliseconds"].Value;
                string op = match.Groups["op"].Value;
                int offsetMinutes = int.Parse(match.Groups["offset"].Value);
                int offsetHour = offsetMinutes / 60;
                int offsetMiute = offsetMinutes % 60;

                string offsetHours = $"{offsetHour:D2}:{offsetMiute:D2}";
                string convertedString = $"{time}.{milliseconds}{op}{offsetHours}";

                return DateTime.ParseExact(convertedString, "yyyyMMddHHmmss.ffffffK", null);
            }

            return null;
        }
    }
}