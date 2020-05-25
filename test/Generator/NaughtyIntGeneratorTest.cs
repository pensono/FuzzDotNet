using System;
using System.Collections.Generic;
using FuzzDotNet;
using FuzzDotNet.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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

            var generatedValue = generator.Generate(Mock.Of<IFuzzContext>(), typeof(int), random);

            Assert.IsTrue(generatedValue is int);
        }
    }
}
