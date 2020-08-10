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
        public void CollectionGeneratorParams()
        {
            var profile = new TestFuzzProfile();
            
            var generated = profile.Generate(typeof(AbstractSuperclass), new FuzzRandom());

            Assert.IsInstanceOfType(generated, typeof(ImplementingSubclass));
        }
    }
}
