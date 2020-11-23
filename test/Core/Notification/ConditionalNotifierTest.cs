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
    public class ConditionalNotifierTest
    {
        [TestMethod]
        [DataRow(true, 1)]
        [DataRow(false, 0)]
        public async Task TestConditionalNotifier(bool shouldNotify, int notifyTimes)
        {
            var innerNotifier = new Mock<INotifier>();

            var notifier = new ConditionalNotifier(innerNotifier.Object, _ => shouldNotify);
            await notifier.NotifyCounterexampleAsync(new Counterexample(Mock.Of<ITestMethod>(), new List<Argument>()));

            innerNotifier.Verify(n => n.NotifyCounterexampleAsync(It.IsAny<Counterexample>()), Times.Exactly(notifyTimes));
        }
    }
}
