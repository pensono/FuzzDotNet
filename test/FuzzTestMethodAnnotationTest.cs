using System;
using System.Threading;
using FuzzDotNet;
using FuzzDotNet.Generators;
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
            var fuzzClassInstance = InvokeTestMethod<TestIterationsClass>();
            Assert.AreEqual(20, fuzzClassInstance.Invocations);
        }

        private class ParameterGeneratorTestClass {
            public int GeneratedValue { get; private set; } = 0;

            [FuzzTestMethod]
            public void Method([ConstantGenerator(42)] int i)
            {
                GeneratedValue = i;
            }
        }

        [TestMethod]
        public void ParameterGenerator() {
            var fuzzClassInstance = InvokeTestMethod<ParameterGeneratorTestClass>();
            Assert.AreEqual(42, fuzzClassInstance.GeneratedValue);
        }

        private class GeneratorAttributeWrongConstructorArgumentTypeClass
        {
            [FuzzTestMethod]
            public void Method([ConstantGenerator("not an int")] int i)
            {
            }
        }

        [TestMethod]
        public void GeneratorAttributeWrongArgumentTypeThrows()
        {
            Assert.ThrowsException<ArgumentException>(() => InvokeTestMethod<GeneratorAttributeWrongConstructorArgumentTypeClass>());
        }

        private TTest InvokeTestMethod<TTest>() 
            where TTest : notnull, new()
        {
            var attribute = new FuzzTestMethodAttribute();
            var fuzzClassInstance = new TTest();

            var method = typeof(TTest).GetMethod("Method")!;

            var fuzzTestMethod = new Mock<ITestMethod>();
            fuzzTestMethod.Setup(m => m.MethodInfo).Returns(method);
            fuzzTestMethod.Setup(m => m.Invoke(It.IsAny<object?[]>()))
                .Callback<object?[]>(args => method.Invoke(fuzzClassInstance, args));

            attribute.Execute(fuzzTestMethod.Object);

            return fuzzClassInstance;
        }
    }
}
