using System;
using System.Collections.Generic;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test.Generator
{
    [TestClass]
    public class NaughtyIntGeneratorTest
    {
        [TestMethod]
        public void GeneratesInt()
        {
            var generator = new NaughtyIntGenerator();
            var random = new FuzzRandom();

            var generatedValue = generator.Generate(typeof(int), random);

            Assert.IsTrue(generatedValue is int);
        }
    }
}
