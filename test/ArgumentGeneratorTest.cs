using System;
using System.Reflection;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Generators;
using FuzzDotNet.Core.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test
{
    [TestClass]
    public class ArgumentGeneratorTest
    {
        public static int GeneratedValue { get; private set; } = 0;

        private class ParameterGeneratorClass {
            [FuzzTestMethod]
            public void Method(
                [ConstantGenerator(42)] int i)
            {
                GeneratedValue = i;
            }
        }

        [TestMethod]
        public void ParameterGenerator()
        {
            var generatedArguments = ArgumentGenerator.GenerateArgumentsFor(typeof(ParameterGeneratorClass).GetMethod("Method")!);

            CollectionAssert.AreEqual(new[] { 42 }, generatedArguments);
        }

        private class MethodLevelGeneratorClass
        {
            [FuzzTestMethod]
            [ConstantGenerator(42)]
            public void Method(
                int i)
            {
            }
        }

        [TestMethod]
        public void MethodLevelGenerator()
        {

            var generatedArguments = ArgumentGenerator.GenerateArgumentsFor(typeof(MethodLevelGeneratorClass).GetMethod("Method")!);

            CollectionAssert.AreEqual(new[] { 42 }, generatedArguments);
        }
    }
}
