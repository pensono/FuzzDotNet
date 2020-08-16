using System.Collections.Generic;
using FuzzDotNet.Generation;
using FuzzDotNet.Test.TestUtilities;
using FuzzDotNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuzzDotNet.Test.Generation
{
    [TestClass]
    public class SubclassGeneratorTest
    {
        private abstract class AbstractSuperclass {}

        private class ImplementingSubclass : AbstractSuperclass {}

        [FuzzTestMethod]
        public void GeneratesSubclass()
        {
            var profile = new TestFuzzProfile();
            
            var generated = profile.Generate(typeof(AbstractSuperclass), new FuzzRandom());

            Assert.IsInstanceOfType(generated, typeof(ImplementingSubclass));
        }

        /// <summary>
        /// Objects should be handeled by another generator. It's hard to generate random subclasses
        /// of an object because there are so many.
        /// </summary>
        [TestMethod]
        public void DoesNotGenerateObject()
        {
            var profile = new TestFuzzProfile();

            Assert.IsFalse(new SubclassGenerator().CanGenerate(profile, typeof(object)));
        }
    }
}
