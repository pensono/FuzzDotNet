using System;
using System.Collections.Generic;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test.Generator
{
    [TestClass]
    public class NaughtyStringGeneratorTest
    {
        [TestMethod]
        public void GeneratesString()
        {
            var generator = new NaughtyStringGenerator();
            var random = new FuzzRandom();

            var generatedValue = generator.Generate(random);

            Assert.IsTrue(generatedValue is string);
        }
    }
}
