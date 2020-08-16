using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FuzzDotNet.Generation
{
    /// <summary>
    /// Generates objects of classes which are default construtable by generating
    /// each settable property.
    /// </summary>
    public class DataObjectGenerator : Generator
    {
        public override bool CanGenerate(IFuzzProfile profile, Type type)
        {
            // Must be default constructable
            return type.GetConstructor(Array.Empty<Type>()) != null
                && !type.ContainsGenericParameters
                && GetDataProperties(type).All(p => profile.GeneratorFor(p.PropertyType) != null);
        }

        public override object? Generate(IFuzzProfile profile, Type type, FuzzRandom random)
        {
            var constructor = type.GetConstructor(Array.Empty<Type>())!;
            var instance = constructor.Invoke(Array.Empty<object?>());

            foreach (var property in GetDataProperties(type)) 
            {
                if (!property.CanWrite) 
                {
                    continue;
                }

                var value = profile.Generate(property.PropertyType, random);
                property.SetValue(instance, value);
            }

            return instance;
        }

        private static IEnumerable<PropertyInfo> GetDataProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
    }
}
