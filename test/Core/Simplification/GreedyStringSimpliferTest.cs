using System;
using FuzzDotNet.Core.Generation;
using FuzzDotNet.Core.Simplification;
using FuzzDotNet.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test.Core.Simplification
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

            bool predicate(object? input) => input is string s && s.Length >= maxLength;
            var simplified = simplifier.SimplifyInstance(new TestFuzzProfile(), input, predicate);

            Assert.AreEqual(simplified.Length, maxLength, $"Input: {input} Simplified: {simplified}");
        }
    }
}
