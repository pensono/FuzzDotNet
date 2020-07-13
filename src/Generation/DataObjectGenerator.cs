using System;
using System.Reflection;

namespace FuzzDotNet.Generation
{
    /// <summary>
    /// Generates objects of classes which are default construtable by generating
    /// each settable property.
    /// </summary>
    public class DataObjectGenerator : Generator
    {
        public override bool CanGenerate(Type type)
        {
            // Must be default constructable
            return type.GetConstructor(Array.Empty<Type>()) != null;
        }

        public override object? Generate(IFuzzProfile profile, Type type, FuzzRandom random)
        {
            var constructor = type.GetConstructor(Array.Empty<Type>())!;
            var instance = constructor.Invoke(Array.Empty<object?>());

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)) 
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
    }
}
