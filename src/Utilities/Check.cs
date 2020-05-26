using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FuzzDotNet.Utilities
{
    internal static class Check
    {
        public static void IsTrue(bool condition, string? message = null)
        {
            if (!condition)
            {
                throw new ArgumentException(message);
            }
        }

        public static void IsType<T>(object obj, string? message = null)
        {
            if (!(obj is T))
            {
                throw new ArgumentException(message);
            }
        }

        public static void IsNotNull([NotNull] object? obj, string? message = null)
        {
            if (obj == null)
            {
                throw new ArgumentException(message);
            }
        }
    }
}
