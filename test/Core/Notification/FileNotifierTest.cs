using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Notification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuzzDotNet.Test.Core.Notification
{
    [TestClass]
    public class FileNotiferTest
    {
        [TestMethod]
        public async Task Basic()
        {
            var path = $"{new FuzzRandom().Uniform(0, int.MaxValue)}.txt";

            var notifier = new FileNotifier(TestFormatter.Instance, _ => path);
            await notifier.NotifyCounterexampleAsync(new Counterexample(Mock.Of<MethodInfo>(), new List<Argument>()));

            var output = await File.ReadAllTextAsync(path, Encoding.UTF8);
            Assert.IsTrue(output.Contains(TestFormatter.GeneratedString));

            File.Delete(path);
        }
    }
}
