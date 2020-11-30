using FuzzDotNet.Test.Common;
using FuzzDotNet.Core.Generation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FuzzDotNet.Core;

namespace FuzzDotNet.Test.Core.Generation
{
    [TestClass]
    public class NaughtyStringGeneratorTest
    {
        [TestMethod]
        public void GeneratesString()
        {
            var generator = new NaughtyStringGenerator();
            var random = new FuzzRandom();

            var generatedValue = generator.Generate(Mock.Of<IFuzzProfile>(), typeof(string), random);

            Assert.That.IsType<string>(generatedValue);
        }
    }
}
