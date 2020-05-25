using FuzzDotNet.Core;
using FuzzDotNet.Core.Generators;
using FuzzDotNet.Test.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test.Generator
{
    [TestClass]
    public class DataObjectGeneratorTest
    {
        private class DataClass {
            public string? StringProperty { get; set; }
            public int IntProperty { get; set; }
        }

        [TestMethod]
        public void GeneratesDataClass()
        {
            var generator = new DataObjectGenerator();
            var result = generator.Generate(new TestFuzzContext(), typeof(DataClass), new FuzzRandom());

            Assert.That.IsType<DataClass>(result);

            var dataClass = (DataClass)result;
            Assert.AreEqual(TestFuzzContext.GeneratedInt, dataClass.IntProperty);
            Assert.AreEqual(TestFuzzContext.GeneratedString, dataClass.StringProperty);
        }

        private class NotDefaultConstructable
        {
            public NotDefaultConstructable(int argument)
            {
                // Do nothing
            }
        }

        [TestMethod]
        public void RejectNonDefaultConstructable()
        {
            var generator = new DataObjectGenerator();
            Assert.IsFalse(generator.CanGenerate(typeof(NotDefaultConstructable)));
        }
    }
}
