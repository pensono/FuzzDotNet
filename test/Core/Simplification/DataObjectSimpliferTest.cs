using FuzzDotNet.Core.Simplification;
using FuzzDotNet.MSTest;
using FuzzDotNet.Test.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test.Core.Simplification
{
    [TestClass]
    public class DataObjectSimpliferTest
    {
        public class DataClass
        {
            public string? StringProperty { get; set; }
            public int IntProperty { get; set; }
        }

        [FuzzTestMethod]
        public void TestSimplify(DataClass input)
        {
            // Just check that everything simplifies all the way down
            var simplifier = new DataObjectSimplifier();
            var simplified = simplifier.Simplify(new TestFuzzProfile(), input, s => true);

            Assert.That.IsType<DataClass>(simplified);

            var simplifiedDataClass = (DataClass)simplified;
            Assert.AreEqual(string.Empty, simplifiedDataClass.StringProperty);
            Assert.AreEqual(0, simplifiedDataClass.IntProperty);
        }
    }
}
