using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Notification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuzzDotNet.Test.Core.Notification
{
    [TestClass]
    public class ConsoleNotifierTest
    {
        [TestMethod]
        public async Task Basic()
        {
            var writer = new StringWriter();
            Console.SetOut(writer);

            var notifier = new ConsoleNotifier(TestFormatter.Instance);
            await notifier.NotifyCounterexampleAsync(new Counterexample(Mock.Of<ITestMethod>(), new List<Argument>()));

            var output = writer.ToString();
            Assert.IsTrue(output.Contains(TestFormatter.GeneratedString));
        }
    }
}
