using System;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Utilities;
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

        public class WrongArgumentTypeClass : IGenerator
        {
            public WrongArgumentTypeClass(string argument)
            {
            }

            public Type GeneratedType => throw new NotImplementedException();

            public object? Generate(Type type, FuzzRandom random)
            {
                return null;
            }
        }

        [TestMethod]
        public void WrongArgumentType()
        {
            Assert.ThrowsException<ArgumentException>(() => GeneratorBuilder.BuildGenerator(typeof(WrongArgumentTypeClass), 20));
        }
    }
}
