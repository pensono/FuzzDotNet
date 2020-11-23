using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public static TParameter GetDefaultConstructableParameter<TParameter>(ref TParameter? parameterField, Type? specifiedType, Func<TParameter> createDefault)
            where TParameter : class
        {
            if (parameterField == null)
            {
                if (specifiedType == null)
                {
                    parameterField = createDefault();
                }
                else if (!typeof(TParameter).IsAssignableFrom(specifiedType))
                {
                    throw new ArgumentException($"Parameter type {specifiedType.FullName} must be a subclass of {typeof(TParameter)}.");
                }
                else
                {
                    var constructor = specifiedType.GetConstructor(Array.Empty<Type>());
                    if (constructor == null)
                    {
                        throw new ArgumentException($"Parameter type {specifiedType.FullName} is not default-constructable.");
                    }

                    parameterField = (TParameter)constructor.Invoke(Array.Empty<object?>());
                }
            }

            return parameterField;
        }

        public static bool IsDataObject(this Type type)
        {
            // Must be default constructable
            return type.GetConstructor(Array.Empty<Type>()) != null
                && !type.ContainsGenericParameters;
        }

        public static IEnumerable<PropertyInfo> GetDataProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
    }
}
