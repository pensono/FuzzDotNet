using System;
using System.Collections.Generic;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Generators;
using FuzzDotNet.Test.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test.Generator
{
    [TestClass]
    public class CollectionGeneratorTest
    {
        [TestMethod]
        public void CollectionGeneratorParams()
        {
            var generator = new CollectionGenerator(1, 2, 3, 4);
            var random = new FuzzRandom();

            var generatedValue = generator.Generate(typeof(int), random);

            Assert.That.IsType<int>(generatedValue);
            Assert.IsTrue((int)generatedValue < 5);
            Assert.IsTrue((int)generatedValue > 0);
        }

        [TestMethod]
        public void CollectionGeneratorList()
        {
            var generator = new CollectionGenerator(new List<object?>{1, 2, 3, 4});
            var random = new FuzzRandom();

            var generatedValue = generator.Generate(typeof(int), random);

            Assert.That.IsType<int>(generatedValue);
            Assert.IsTrue((int)generatedValue < 5);
            Assert.IsTrue((int)generatedValue > 0);
        }
    }
}
