using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzDotNet.Utilities
{
    public static class ReflectionExtensions
    {
        public static Type GetEnumerableElementType(this Type enumerableType)
        {
            // Inspired by https://stackoverflow.com/a/906513/2496050
            if (enumerableType.IsGenericType && enumerableType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return enumerableType.GetGenericArguments()[0];
            }

            return enumerableType
                .GetInterfaces()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(t => t.GetGenericArguments()[0])
                .FirstOrDefault();
        }
    }
}
