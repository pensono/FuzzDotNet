using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Notification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuzzDotNet.Test.Core.Notification
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
            await notifier.NotifyCounterexampleAsync(new Counterexample(Mock.Of<MethodInfo>(), new List<Argument>()));

            notifier1.Verify(n => n.NotifyCounterexampleAsync(It.IsAny<Counterexample>()), Times.Once);
            notifier2.Verify(n => n.NotifyCounterexampleAsync(It.IsAny<Counterexample>()), Times.Once);
        }
    }
}
