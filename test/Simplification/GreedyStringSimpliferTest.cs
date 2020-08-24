using System;
using FuzzDotNet.Generation;
using FuzzDotNet.Simplification;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test.Simplification
{
    [TestClass]
    public class GreedyStringSimpliferTest
    {
        [FuzzTestMethod]
        public void TestSimplify(string input, [UniformIntGenerator(Min = 0, Max = 10)] int maxLength)
        {
            if (input.Length < maxLength)
            {
                return;
            }

            var simplifier = new GreedyStringSimplifier();

            var simplified = simplifier.Simplify(input, s => s.Length >= maxLength);

            Assert.AreEqual(simplified.Length, maxLength, $"Input: {input} Simplified: {simplified}");
        }
    }
}
