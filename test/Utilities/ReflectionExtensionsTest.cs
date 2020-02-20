using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        [DataTestMethod]
        [DataRow(typeof(int), typeof(int), true)]
        [DataRow(typeof(int), typeof(string), false)]
        [DataRow(typeof(IEnumerable<int>), typeof(IEnumerable<int>), true)]
        [DataRow(typeof(IEnumerable<int>), typeof(IEnumerable<string>), false)]
        [DataRow(typeof(IEnumerable<>), typeof(IEnumerable<int>), true)]
        [DataRow(typeof(IEnumerable<>), typeof(IList<int>), true)]
        [DataRow(typeof(IDictionary<string, int>), typeof(Dictionary<string, int>), true)]
        [DataRow(typeof(IDictionary<,>), typeof(Dictionary<string, int>), true)]
        [DataRow(typeof(List<int>), typeof(List<int>), true)]
        [DataRow(typeof(IEnumerable<int>), typeof(List<int>), true)]
        [DataRow(typeof(List<>), typeof(List<int>), true)]
        [DataRow(typeof(List<>), typeof(ImmutableList<int>), true)]
        // If the subtype isn't concrete, then it can't be classified as a subtype
        [DataRow(typeof(List<>), typeof(ImmutableList<>), false)]
        [DataRow(typeof(IEnumerable<>), typeof(IList<>), false)]
        public void CovariantSubtypeOfTest(Type supertype, Type subtype, bool isSubtype)
        {
            // To Fuzz:
            // Forall t, t.IsCovariantSubtypeOf(t)

            Assert.AreEqual(isSubtype, subtype.IsCovariantSubtypeOf(supertype));
        }
    }
}
