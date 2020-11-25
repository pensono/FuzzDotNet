using FuzzDotNet.Core;
using FuzzDotNet.Core.Generation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Reflection;

namespace FuzzDotNet.Test.Core.Generation
{
    [TestClass]
    public class FuzzArgumentGeneratorTest
    {
        [TestMethod]
        public void CurrentBeforeNextThrows()
        {
            var argumentGenerator = new FuzzArgumentGenerator(Mock.Of<IFuzzProfile>(), Mock.Of<MethodInfo>());

            Assert.ThrowsException<InvalidOperationException>(() => argumentGenerator.CurrentArguments);
            Assert.ThrowsException<InvalidOperationException>(() => argumentGenerator.CurrentSeed);
        }

        private class ProfileGeneratorTestClass
        {
            public void Method(int i)
            {
            }
        }

        [TestMethod]
        public void ProfileGenerator()
        {
            var argumentGenerator = new FuzzArgumentGenerator(new TestFuzzProfile(), typeof(ParameterGeneratorTestClass).GetMethod(nameof(ParameterGeneratorTestClass.Method))!); ;

            argumentGenerator.MoveNext();

            CollectionAssert.AreEqual(new object?[] { TestFuzzProfile.GeneratedInt }, argumentGenerator.CurrentArguments);
        }

        private class ParameterGeneratorTestClass
        {
            public void Method([ConstantGenerator(42)] int i)
            {
            }
        }

        [TestMethod]
        public void ParameterGenerator()
        {
            var argumentGenerator = new FuzzArgumentGenerator(Mock.Of<IFuzzProfile>(), typeof(ParameterGeneratorTestClass).GetMethod(nameof(ParameterGeneratorTestClass.Method))!);

            argumentGenerator.MoveNext();

            CollectionAssert.AreEqual(new object?[] { 42 }, argumentGenerator.CurrentArguments);
        }

        private class GeneratorAttributeWrongConstructorArgumentTypeClass
        {
            public void Method([ConstantGenerator("not an int")] int i)
            {
            }
        }

        [TestMethod]
        public void GeneratorAttributeWrongArgumentTypeThrows()
        {
            var method = typeof(GeneratorAttributeWrongConstructorArgumentTypeClass).GetMethod(nameof(GeneratorAttributeWrongConstructorArgumentTypeClass.Method))!;

            Assert.ThrowsException<IncompatibleGeneratorException>(() => new FuzzArgumentGenerator(Mock.Of<IFuzzProfile>(), method));
        }

    }
}
