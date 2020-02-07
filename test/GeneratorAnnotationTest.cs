using System;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test
{
    [TestClass]
    public class GeneratorAnnotationTest
    {
        public static int GeneratedValue { get; private set; } = 0;

        private class ParameterGeneratorTestClass {
            [FuzzTestMethod]
            public void Method(
                [ConstantGenerator(42)] int i)
            {
                GeneratedValue = i;
            }
        }

        [TestMethod]
        public void ParameterGenerator() {
            var adapter = new Adapter();
            adapter.RunTests(typeof(ParameterGeneratorTestClass));

            Assert.AreEqual(42, GeneratedValue);
        }

        private class GeneratorAttributeWrongConstructorArgumentTypeClass
        {
            [FuzzTestMethod]
            public void Method(
                [ConstantGenerator("not an int")] int i)
            {
            }
        }

        [TestMethod]
        public void GeneratorAttributeWrongArgumentTypeThrows()
        {
            var adapter = new Adapter();
            Assert.ThrowsException<ArgumentException>(() => adapter.RunTests(typeof(GeneratorAttributeWrongConstructorArgumentTypeClass)));
        }
    }
}
