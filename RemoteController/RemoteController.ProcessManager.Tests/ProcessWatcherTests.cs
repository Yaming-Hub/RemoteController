using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RemoteController.ProcessManager.Workers;

namespace RemoteController.ProcessManager.Tests
{
    [TestClass]
    public class ProcessWatcherTests
    {
        [TestMethod]
        public void TestGetProcesses()
        {
            foreach (var process in ProcessWatcher.GetProcesses())
            {
                var json = JsonConvert.SerializeObject(process);
                Trace.WriteLine(json);
            }
        }

        [TestMethod]
        public void TestParseWin32ProcessDateTime()
        {
            var expectedDateTime = DateTime.Parse("2020-05-22 10:46:56.299017-07:00");
            var dateTime = ProcessWatcher.ParseWin32ProcessDateTime("20200522104656.299017-420");

            Assert.IsNotNull(dateTime);
            Assert.AreEqual(expectedDateTime, dateTime.Value);
        }
    }
}
