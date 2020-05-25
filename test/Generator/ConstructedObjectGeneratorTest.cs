using FuzzDotNet.Core;
using FuzzDotNet.Core.Generators;
using FuzzDotNet.Test.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test.Generator
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
            var result = generator.Generate(new TestFuzzContext(), typeof(ConstructedClass), new FuzzRandom());

            Assert.That.IsType<ConstructedClass>(result);

            var obj = (ConstructedClass)result;
            Assert.AreEqual(TestFuzzContext.GeneratedInt, obj.IntProperty);
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
            Assert.IsFalse(generator.CanGenerate(typeof(NonConstructable)));
        }
    }
}
