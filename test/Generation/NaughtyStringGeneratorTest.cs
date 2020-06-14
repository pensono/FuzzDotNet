using FuzzDotNet;
using FuzzDotNet.Test.TestUtilities;
using FuzzDotNet.Generation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuzzDotNet.Test.Generation
{
    [TestClass]
    public class NaughtyStringGeneratorTest
    {
        [TestMethod]
        public void GeneratesString()
        {
            var generator = new NaughtyStringGenerator();
            var random = new FuzzRandom();

            var generatedValue = generator.Generate(Mock.Of<IFuzzContext>(), typeof(string), random);

            Assert.That.IsType<string>(generatedValue);
        }
    }
}
