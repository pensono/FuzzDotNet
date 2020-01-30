using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzDotNet.Core.Utility
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

        public static void IsNotNull(object? obj, string? message = null)
        {
            // TODO Suppress CS8602 by convincing the compiler that obj is not null after this function has been called
            if (obj == null)
            {
                throw new ArgumentException(message);
            }
        }
    }
}
