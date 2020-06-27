using System;
using FuzzDotNet.Generation;
using FuzzDotNet.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test.Utilities
{
    [TestClass]
    public class GeneratorBuilderTest
    {
        [TestMethod]
        public void WrongType()
        {
            Assert.ThrowsException<ArgumentException>(() => GeneratorBuilder.BuildGenerator(typeof(int)));
        }

        public class WrongConstructorArgumentTypeClass : IGenerator
        {
            public WrongConstructorArgumentTypeClass(string argument)
            {
            }

            public bool CanGenerate(Type type)
            {
                return true;
            }

            public object? Generate(IFuzzContext context, Type type, FuzzRandom random)
            {
                return null;
            }
        }

        [TestMethod]
        public void WrongConstructorArgumentType()
        {
            Assert.ThrowsException<ArgumentException>(() => GeneratorBuilder.BuildGenerator(typeof(WrongConstructorArgumentTypeClass), 20));
        }
    }
}
