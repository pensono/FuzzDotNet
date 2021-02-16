using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test.Common
{
    public static class AssertExtensions
    {
        public static void IsType<T>(this Assert assert, [NotNull] object? obj)
        {
            // TODO replace with Assert.IsOfType
            // From https://github.com/Microsoft/testfx-docs/blob/master/RFCs/002-Framework-Extensibility-Custom-Assertions.md
            if (obj is T)
            {
                return;
            }

            throw new AssertFailedException("Type does not match");
        }

        public static void AreEqualIgnoreNewlineStyle(this Assert assert, string expected, string actual)
        {
            string Normalize(string input)
            {
                return input.Replace("\r\n", "\n");
            }

            Assert.AreEqual(Normalize(expected), Normalize(actual));
        }
    }
}
