using System;
using System.Threading;
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

        [FuzzTestMethod]
        public void TestIterationsBasic([UniformIntGenerator(Min = 0, Max = 10)] int iterations) {
            var fuzzClassInstance = TestMethodInvocationClass<TestIterationsClass>(iterations);
            Assert.AreEqual(iterations, fuzzClassInstance.Invocations);
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
            var fuzzClassInstance = TestMethodInvocationClass<ParameterGeneratorTestClass>();
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
            Assert.ThrowsException<IncompatibleGeneratorException>(() => TestMethodInvocationClass<GeneratorAttributeWrongConstructorArgumentTypeClass>());
        }

        private class FailingTestClass
        {
            [FuzzTestMethod]
            public void Method([ConstantGenerator(4)] int i)
            {
                Assert.AreEqual(5, i);
            }
        }

        [TestMethod]
        public void CollectAllFailingResults()
        {
            var results = TestMethodInvocationResults<FailingTestClass>();

            Assert.AreEqual(20, results.Length);

            // Results should have a seed associated with them in the DataTestRow field
            foreach (var result in results)
            {
                Assert.AreNotEqual(0, result.DatarowIndex);
            }
        }

        private class PassingTestClass
        {
            [FuzzTestMethod]
            public void Method([ConstantGenerator(4)] int i)
            {
                Assert.AreEqual(4, i);
            }
        }

        [TestMethod]
        public void SinglePassingResult()
        {
            var results = TestMethodInvocationResults<PassingTestClass>();

            Assert.AreEqual(1, results.Length);
        }

        private TestResult[] TestMethodInvocationResults<TTest>()
            where TTest : notnull, new()
        {
            var attribute = new FuzzTestMethodAttribute();
            var fuzzClassInstance = new TTest();
            var method = CreateTestMethodMock(fuzzClassInstance);

            return attribute.Execute(method);
        }

        private TTest TestMethodInvocationClass<TTest>(int iterations = 20)
            where TTest : notnull, new()
        {
            var attribute = new FuzzTestMethodAttribute()
            {
                Iterations = iterations,
            };

            var fuzzClassInstance = new TTest();
            var method = CreateTestMethodMock(fuzzClassInstance);

            attribute.Execute(method);

            return fuzzClassInstance;
        }

        private ITestMethod CreateTestMethodMock<TTest>(TTest instance)
        {
            var method = typeof(TTest).GetMethod("Method")!;

            var fuzzTestMethod = new Mock<ITestMethod>();
            fuzzTestMethod.Setup(m => m.MethodInfo).Returns(method);
            fuzzTestMethod.Setup(m => m.Invoke(It.IsAny<object?[]>()))
                .Returns<object?[]>(args => {
                    try
                    {
                        method.Invoke(instance, args);
                    }
                    catch (Exception)
                    {
                        return new TestResult
                        {
                            Outcome = UnitTestOutcome.Failed,
                        };
                    }

                    return new TestResult
                    {
                        Outcome = UnitTestOutcome.Passed,
                    };
                });

            return fuzzTestMethod.Object;
        }
    }
}
