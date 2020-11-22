using System;
using FuzzDotNet.Generation;
using FuzzDotNet.Simplification;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test.Simplification
{
    [TestClass]
    public class BinarySearchIntegerSimpliferTest
    {
        [Timeout(500)]
        [FuzzTestMethod]
        public void TestSimplify([UniformIntGenerator] int number, [UniformIntGenerator] int cutoff)
        {
            bool predicate(object? input) => input is int i && Math.Abs(i) >= Math.Abs(cutoff);

            if (cutoff == int.MinValue || !predicate(number))
            {
                return;
            }

            var simplifier = new BinarySearchIntegerSimplifier();

            var simplified = simplifier.SimplifyInstance(new TestFuzzProfile(), number, predicate);

            Assert.AreEqual(Math.Abs(cutoff), Math.Abs(simplified));
        }
    }
}
