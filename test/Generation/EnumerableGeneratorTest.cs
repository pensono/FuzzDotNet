using System;
using System.Collections.Generic;
using FuzzDotNet.Generation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuzzDotNet.Test.Generation
{
    [TestClass]
    public class EnumerableGeneratorTest
    {
        [DataTestMethod]
        [DataRow(typeof(IEnumerable<int>), true)]
        [DataRow(typeof(IList<int>), true)]
        [DataRow(typeof(ISet<int>), true)]
        [DataRow(typeof(int), false)]
        public void CanGenerate(Type type, bool canGenerate)
        {
            var actual = new EnumerableGenerator().CanGenerate(type);
            
            Assert.AreEqual(canGenerate, actual);
        }

        [TestMethod]
        [DataRow(typeof(IEnumerable<int>))]
        [DataRow(typeof(IList<int>))]
        [DataRow(typeof(ICollection<int>))]
        [DataRow(typeof(ISet<int>))]
        [DataRow(typeof(IReadOnlyCollection<int>))]
        [DataRow(typeof(IReadOnlyList<int>))]
        public void GeneratesCorrectType(Type argumentType)
        {
            var generator = new EnumerableGenerator(elementGeneratorType: typeof(NaughtyIntGenerator));
            var random = new FuzzRandom();

            var generatedValue = generator.Generate(Mock.Of<IFuzzContext>(), argumentType, random);

            Assert.IsInstanceOfType(generatedValue, argumentType);
        }
    }
}
