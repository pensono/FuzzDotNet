using System;
using System.Collections.Generic;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test.Generator
{
    [TestClass]
    public class EnumGeneratorTest
    {
        enum TestEnum
        {
            Value1,
            Value2,
        }

        [TestMethod]
        public void GeneratesEnum()
        {
            var generator = new EnumGenerator();
            var random = new FuzzRandom();

            var generatedValue = generator.Generate(typeof(TestEnum), random);

            Assert.IsTrue(generatedValue is TestEnum);
        }
    }
}
