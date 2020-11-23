using FuzzDotNet.Core;
using FuzzDotNet.Core.Generation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuzzDotNet.Test.Core.Generation
{
    [TestClass]
    public class UniformIntGeneratorTest
    {
        [TestMethod]
        public void GeneratesInt()
        {
            var generator = new UniformIntGenerator();
            var random = new FuzzRandom();

            var generatedValue = generator.Generate(Mock.Of<IFuzzProfile>(), typeof(int), random);

            Assert.IsTrue(generatedValue is int);
        }
    }
}
