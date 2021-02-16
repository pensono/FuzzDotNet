using System;
using System.Collections.Generic;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Generation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuzzDotNet.Test.Core.Generation
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
            var actual = new EnumerableGenerator().CanGenerate(new TestFuzzProfile(), type);

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
            var generator = new EnumerableGenerator(elementGeneratorType: typeof(ConstantGenerator), elementGeneratorConstructorArguments: 42);
            var random = new FuzzRandom();

            var generatedValue = generator.Generate(Mock.Of<IFuzzProfile>(), argumentType, random);

            Assert.IsInstanceOfType(generatedValue, argumentType);
        }

        [TestMethod]
        public void DefaultsToProfileElementGenerator()
        {
            var generator = new EnumerableGenerator();
            var random = new FuzzRandom();

            var profile = new Mock<IFuzzProfile>();
            profile.Setup(c => c.GeneratorFor(typeof(int))).Returns(new ConstantGenerator(42));

            var generatedValue = generator.Generate(profile.Object, typeof(IList<int>), random);

            Assert.IsInstanceOfType(generatedValue, typeof(IList<int>));
        }
    }
}
