using FuzzDotNet.Core;
using FuzzDotNet.Core.Generators;
using FuzzDotNet.Test.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test.Generator
{
    [TestClass]
    public class DataObjectGeneratorTest
    {
        private class DataClass
        {
            public string? StringProperty { get; set; }
            public int IntProperty { get; set; }
        }

        [TestMethod]
        public void GeneratesDataClass()
        {
            var generator = new DataObjectGenerator();
            var result = generator.Generate(new TestFuzzContext(), typeof(DataClass), new FuzzRandom());

            Assert.That.IsType<DataClass>(result);

            var obj = (DataClass)result;
            Assert.AreEqual(TestFuzzContext.GeneratedInt, obj.IntProperty);
            Assert.AreEqual(TestFuzzContext.GeneratedString, obj.StringProperty);
        }

        private class HasReadOnlyProperty
        {
            public int MutableProperty { get; set; }
            public int ReadOnlyProperty => 11;
        }

        [TestMethod]
        public void IgnoresReadOnlyProperty()
        {
            var generator = new DataObjectGenerator();
            var result = generator.Generate(new TestFuzzContext(), typeof(HasReadOnlyProperty), new FuzzRandom());

            Assert.That.IsType<HasReadOnlyProperty>(result);

            var obj = (HasReadOnlyProperty)result;
            Assert.AreEqual(TestFuzzContext.GeneratedInt, obj.MutableProperty);
            Assert.AreEqual(11, obj.ReadOnlyProperty);
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
        
        private class Outer
        {
            public string? StringProperty { get; set; }

            public Inner? Inner { get; set; }
        }

        private class Inner 
        {
            public int IntProperty { get; set; }
        }
        
        [TestMethod]
        public void NestedObjects()
        {
            var generator = new DataObjectGenerator();
            var result = generator.Generate(new TestFuzzContext(), typeof(Outer), new FuzzRandom());

            Assert.That.IsType<Outer>(result);

            var obj = (Outer)result;
            Assert.AreEqual(TestFuzzContext.GeneratedString, obj.StringProperty);
            Assert.IsNotNull(obj.Inner);
            Assert.AreEqual(TestFuzzContext.GeneratedInt, obj.Inner!.IntProperty);
        }
    }
}
