using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzDotNet.Core.Utilities
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

        public static bool IsCovariantSubtypeOf(this Type subtype, Type supertype)
        {
            if (supertype == subtype || supertype.IsSubclassOf(supertype))
            {
                return true;
            }

            //if (supertype.IsInterface && subtype.ImplementInterface(supertype))
            if (subtype.GetInterfaces().Any(t => t == supertype || t.IsSubclassOf(supertype)))
            {
                return true;
            }

            if (subtype.IsGenericType)
            {
                return subtype.GetGenericTypeDefinition() == supertype ||
                    subtype.IsAssignableFrom(supertype.GetGenericTypeDefinition());
            }

            return false;
        }

        internal static bool ImplementInterface(this Type baseType, Type ifaceType)
        {
            // From https://referencesource.microsoft.com/#mscorlib/system/type.cs,95fa6055bd315eed
            Type? t = baseType;
            while (t != null)
            {
                var interfaces = t.GetInterfaces();
                if (interfaces.Any(inter => inter == ifaceType || inter.ImplementInterface(ifaceType)))
                {
                    return true;
                }

                t = t.BaseType;
            }

            return false;
        }
    }
}
