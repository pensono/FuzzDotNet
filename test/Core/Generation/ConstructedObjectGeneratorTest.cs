using FuzzDotNet.Core;
using FuzzDotNet.Core.Generation;
using FuzzDotNet.Test.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuzzDotNet.Test.Core.Generation
{
    [TestClass]
    public class ConstructedGeneratorTest
    {
        private class ConstructedClass
        {
            public int IntProperty { get; set; }

            public ConstructedClass(int intProperty)
            {
                IntProperty = intProperty;
            }
        }

        [TestMethod]
        public void GeneratesDataClass()
        {
            var generator = new ConstructedObjectGenerator();
            var result = generator.Generate(new TestFuzzProfile(), typeof(ConstructedClass), new FuzzRandom());

            Assert.That.IsType<ConstructedClass>(result);

            var obj = (ConstructedClass)result;
            Assert.AreEqual(TestFuzzProfile.GeneratedInt, obj.IntProperty);
        }

        private class NonConstructable
        {
            public int IntProperty { get; set; }

            private NonConstructable(int intProperty)
            {
                IntProperty = intProperty;
            }
        }

        [TestMethod]
        public void RejectNonConstructable()
        {
            var generator = new ConstructedObjectGenerator();
            Assert.IsFalse(generator.CanGenerate(Mock.Of<IFuzzProfile>(), typeof(NonConstructable)));
        }
    }
}
