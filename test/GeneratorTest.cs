using System;
using System.Collections;
using System.Collections.Generic;
using FuzzDotNet;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test
{
    [TestClass]
    public class GeneratorTest
    {
        public static int GeneratedValue { get; private set; } = 0;

        private class ParameterGeneratorTestClass {
            [FuzzTest]
            public void Method(
                [Generator(typeof(ConstantGenerator<int>), 42)] int i)
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

        private class GeneratorAttributeWrongTypeClass
        {
            [FuzzTest]
            public void Method(
                [Generator(typeof(int))] int i)
            {
            }
        }

        [TestMethod]
        public void GeneratorAttributeWrongTypeThrows()
        {
            var adapter = new Adapter();
            Assert.ThrowsException<ArgumentException>(() => adapter.RunTests(typeof(GeneratorAttributeWrongTypeClass)));
        }


        private class GeneratorAttributeWrongConstructorArgumentTypeClass
        {
            [FuzzTest]
            public void Method(
                [Generator(typeof(ConstantGenerator<int>), "not an int")] int i)
            {
            }
        }

        [TestMethod]
        public void GeneratorAttributeWrongConstructorArgumentTypeThrows()
        {
            var adapter = new Adapter();
            Assert.ThrowsException<ArgumentException>(() => adapter.RunTests(typeof(GeneratorAttributeWrongConstructorArgumentTypeClass)));
        }

        private class GeneratorAttributeWrongArgumentTypeClass
        {
            [FuzzTest]
            public void Method(
                [Generator(typeof(ConstantGenerator<int>), 42)] string i)
            {
            }
        }

        [TestMethod]
        public void GeneratorAttributeWrongArgumentTypeThrows()
        {
            var adapter = new Adapter();
            Assert.ThrowsException<ArgumentException>(() => adapter.RunTests(typeof(GeneratorAttributeWrongArgumentTypeClass)));
        }
    }
}
