using System.Threading;
using FuzzDotNet.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuzzDotNet.Test
{
    [TestClass]
    public class FuzzTestMethodAnnotationTest
    {
        private class TestIterationsClass
        {
            public int Invocations;

            [FuzzTestMethod]
            public void Method()
            {
                Interlocked.Increment(ref Invocations);
            }
        }

        [TestMethod]
        public void TestIterationsBasic() {
            // TODO Fuzz this test >:D

            var attribute = new FuzzTestMethodAttribute
            {
                Iterations = 20
            };

            var fuzzClassInstance = new TestIterationsClass();

            var fuzzTestMethod = new Mock<ITestMethod>();
            fuzzTestMethod.Setup(m => m.MethodInfo)
                .Returns(typeof(TestIterationsClass).GetMethod("Method")!);
            fuzzTestMethod.Setup(m => m.Invoke(It.IsAny<object?[]>()))
                .Callback(() => fuzzClassInstance.Method());

            attribute.Execute(fuzzTestMethod.Object);

            Assert.AreEqual(20, fuzzClassInstance.Invocations);
        }
    }
}
