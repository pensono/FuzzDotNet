using FuzzDotNet.Core.Utilities;
using System;
using System.Linq;

namespace FuzzDotNet.Core.Generation
{
    /// <summary>
    /// Generates objects of classes which are default construtable by generating
    /// each settable property.
    /// </summary>
    public class DataObjectGenerator : Generator
    {
        public override bool CanGenerate(IFuzzProfile profile, Type type)
        {
            return type.IsDataObject()
                && type.GetDataProperties().All(p => profile.GeneratorFor(p.PropertyType) != null);
        }

        public override object? Generate(IFuzzProfile profile, Type type, FuzzRandom random)
        {
            var constructor = type.GetConstructor(Array.Empty<Type>())!;
            var instance = constructor.Invoke(Array.Empty<object?>());

            foreach (var property in type.GetDataProperties())
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
