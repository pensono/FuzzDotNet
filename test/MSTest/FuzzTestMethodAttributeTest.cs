﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Generation;
using FuzzDotNet.Core.Notification;
using FuzzDotNet.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuzzDotNet.Test.MSTest
{
    [TestClass]
    public class FuzzTestMethodAttributeTest
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
        public void TestIterationsBasic([UniformIntGenerator(Min = 0, Max = 10)] int iterations)
        {
            var fuzzClassInstance = TestMethodInvocationClass<TestIterationsClass>(iterations);
            Assert.AreEqual(iterations, fuzzClassInstance.Invocations);
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

            // +1 for the summary item
            Assert.AreEqual(20 + 1, results.Length);

            foreach (var result in results)
            {
                // Results should have a seed associated with them in the DataTestRow field
                Assert.AreNotEqual(0, result.DatarowIndex);

                // Make sure something was logged to the user for debug purposes
                Assert.IsNotNull(result.TestContextMessages);
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

        private class UnitTestFuzzProfile : NaughtyFuzzProfile { }

        [TestMethod]
        public void FuzzProfileByType()
        {
            var annotation = new FuzzTestMethodAttribute
            {
                FuzzProfileType = typeof(UnitTestFuzzProfile),
            };

            Assert.IsInstanceOfType(annotation.FuzzProfile, typeof(UnitTestFuzzProfile));
        }

        [TestMethod]
        public void FuzzProfileByTypeNotAFuzzProfile()
        {
            var annotation = new FuzzTestMethodAttribute
            {
                FuzzProfileType = typeof(object),
            };

            Assert.ThrowsException<ArgumentException>(() => annotation.FuzzProfile);
        }

        [TestMethod]
        public void FuzzProfileByValue()
        {
            var annotation = new FuzzTestMethodAttribute
            {
                FuzzProfile = new UnitTestFuzzProfile(),
            };

            Assert.IsInstanceOfType(annotation.FuzzProfile, typeof(UnitTestFuzzProfile));
        }

        private class UnitTestInheritanceFuzzProfile : NaughtyFuzzProfile { }

        private class FuzzTestMethodWithProfile : FuzzTestMethodAttribute
        {
            protected override IFuzzProfile CreateFuzzProfile()
            {
                return new UnitTestInheritanceFuzzProfile();
            }
        }

        [TestMethod]
        public void FuzzProfileByInheritance()
        {
            var annotation = new FuzzTestMethodWithProfile();

            Assert.IsInstanceOfType(annotation.FuzzProfile, typeof(UnitTestInheritanceFuzzProfile));
        }

        [TestMethod]
        public void FuzzProfileByTypeOverridesInheritance()
        {
            var annotation = new FuzzTestMethodWithProfile
            {
                FuzzProfileType = typeof(UnitTestFuzzProfile),
            };

            Assert.IsInstanceOfType(annotation.FuzzProfile, typeof(UnitTestFuzzProfile));
        }

        [TestMethod]
        public void SendsNotification()
        {
            var notifierMock = new Mock<INotifier>();

            var profile = new NaughtyFuzzProfile
            {
                Notifier = notifierMock.Object,
            };

            TestMethodInvocationClass<FailingTestClass>(profile: profile);

            Func<Counterexample, bool> validCounterexample = (Counterexample c) =>
            {
                if (c.Arguments.Count != 1)
                {
                    return false;
                }

                var argument = c.Arguments[0];
                return argument.Name == "i" && (argument.Value as int?) == 4;
            };

            notifierMock.Verify(n => n.NotifyCounterexampleAsync(It.Is<Counterexample>(c => validCounterexample(c))), Times.AtLeastOnce);
        }

        private class DataDrivenTestClass
        {
            public IList<int> GeneratedValues { get; } = new List<int>();

            [FuzzTestMethod]
            [DataRow(137)]
            public void Method([ConstantGenerator(42)] int i)
            {
                GeneratedValues.Add(i);
            }
        }

        [TestMethod]
        public void UsesDataDrivenTests()
        {
            var fuzzClassInstance = TestMethodInvocationClass<DataDrivenTestClass>(iterations: 1);
            CollectionAssert.AreEqual(new List<int> { 137, 42 }, (ICollection)fuzzClassInstance.GeneratedValues);
        }

        private TestResult[] TestMethodInvocationResults<TTest>()
            where TTest : notnull, new()
        {
            var attribute = new FuzzTestMethodAttribute();
            var fuzzClassInstance = new TTest();
            var method = CreateTestMethodMock(fuzzClassInstance);

            return attribute.Execute(method);
        }

        private TTest TestMethodInvocationClass<TTest>(int iterations = 20, IFuzzProfile? profile = null)
            where TTest : notnull, new()
        {
            var attribute = new FuzzTestMethodAttribute()
            {
                Iterations = iterations,
            };

            if (profile != null)
            {
                attribute.FuzzProfile = profile;
            }

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
                .Returns<object?[]>(args =>
                {
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
