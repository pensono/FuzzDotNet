using System;
using System.Collections.Generic;
using FuzzDotNet;
using FuzzDotNet.Generation;
using FuzzDotNet.Test.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuzzDotNet.Test.Generation
{
    [TestClass]
    public class EnumerableGeneratorTest
    {
        [TestMethod]
        public void GeneratesEnum()
        {
            var generator = new EnumerableGenerator(elementGeneratorType: typeof(NaughtyIntGenerator));
            var random = new FuzzRandom();

            var generatedValue = generator.Generate(Mock.Of<IFuzzContext>(), typeof(IEnumerable<int>), random);

            Assert.That.IsType<IEnumerable<int>>(generatedValue);
        }
    }
}
