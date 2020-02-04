using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Test.Utilities
{
    public static class AssertExtensions
    {
        public static void IsType<T>(this Assert assert, [NotNull] object? obj)
        {
            // From https://github.com/Microsoft/testfx-docs/blob/master/RFCs/002-Framework-Extensibility-Custom-Assertions.md
            if (obj is T)
            {
                return;
            }
            throw new AssertFailedException("Type does not match");
        }
    }
}
