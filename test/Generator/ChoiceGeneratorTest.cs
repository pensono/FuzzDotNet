using System;
using System.Collections.Generic;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Generators;
using FuzzDotNet.Test.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuzzDotNet.Test.Generator
{
    [TestClass]
    public class ChoiceGeneratorTest
    {
        [TestMethod]
        public void CollectionGeneratorParams()
        {
            var generator = new ChoiceGenerator(1, 2, 3, 4);
            var random = new FuzzRandom();

            var generatedValue = generator.Generate(Mock.Of<IFuzzContext>(), typeof(int), random);

            Assert.That.IsType<int>(generatedValue);
            Assert.IsTrue((int)generatedValue < 5);
            Assert.IsTrue((int)generatedValue > 0);
        }

        [TestMethod]
        public void CollectionGeneratorList()
        {
            var generator = new ChoiceGenerator(new List<object?>{1, 2, 3, 4});
            var random = new FuzzRandom();

            var generatedValue = generator.Generate(Mock.Of<IFuzzContext>(), typeof(int), random);

            Assert.That.IsType<int>(generatedValue);
            Assert.IsTrue((int)generatedValue < 5);
            Assert.IsTrue((int)generatedValue > 0);
        }
    }
}
