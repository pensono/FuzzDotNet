using System;
using System.Collections.Generic;
using FuzzDotNet.Core.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test.Utilities
{
    [TestClass]
    public class ReflectionExtensionsTest
    {
        [DataTestMethod]
        [DataRow(typeof(IEnumerable<int>), typeof(int))]
        [DataRow(typeof(List<int>), typeof(int))]
        [DataRow(typeof(IDictionary<string, int>), typeof(KeyValuePair<string, int>))]
        public void EnumerableElementTypeTest(Type listType, Type elementType)
        {
            var type = listType.GetEnumerableElementType();
            Assert.AreEqual(elementType, type);
        }
    }
}
