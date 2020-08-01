using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FuzzDotNet.Notification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuzzDotNet.Test.Notification
{
    [TestClass]
    public class CompositeNotifierTest
    {
        [TestMethod]
        public async Task Basic()
        {
            var notifier1 = new Mock<INotifier>();
            var notifier2 = new Mock<INotifier>();

            var notifier = new CompositeNotifier(notifier1.Object, notifier2.Object);
            await notifier.NotifyCounterexampleAsync(new Counterexample(Mock.Of<ITestMethod>(), new List<Argument>()));

            notifier1.Verify(n => n.NotifyCounterexampleAsync(It.IsAny<Counterexample>()), Times.Once);
            notifier2.Verify(n => n.NotifyCounterexampleAsync(It.IsAny<Counterexample>()), Times.Once);
        }
    }
}
