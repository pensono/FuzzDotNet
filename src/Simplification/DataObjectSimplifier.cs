using FuzzDotNet.Utilities;
using System;

namespace FuzzDotNet.Simplification
{
    /// <summary>
    /// A simplifier for data objects with getters and setters.
    /// </summary>
    public class DataObjectSimplifier : ISimplifier
    {
        public bool CanSimplify(IFuzzProfile profile, Type type)
        {
            return type.IsDataObject();
        }

        public object? Simplify(IFuzzProfile profile, object? input, Func<object?, bool> isValid)
        {
            if (input is null)
            {
                return null;
            }

            var simplified = CopyDataObject(input);

            // Assume each property is independent, and simplify separately
            foreach (var property in input.GetType().GetDataProperties())
            {
                var initialValue = property.GetValue(input);
                if (initialValue == null)
                {
                    continue;
                }

                // Use the dynamic type
                var simplifier = profile.SimplifierFor(initialValue.GetType());
                var simplifiedProperty = simplifier.Simplify(profile, initialValue, candidate =>
                {
                    // Note that mutating `simplified` is okay, because it is completely hidden from the recursive call
                    property.SetValue(simplified, candidate);
                    return isValid(simplified);
                });

                property.SetValue(simplified, simplifiedProperty);
            }

            return simplified;
        }

        private object CopyDataObject(object input)
        {
            var inputType = input.GetType();
            var result = inputType.GetConstructor(Array.Empty<Type>())!.Invoke(Array.Empty<object?>());

            foreach (var property in inputType.GetDataProperties())
            {
                var propertyValue = property.GetValue(input);
                property.SetValue(result, propertyValue);
            }

            return result;
        }
    }
}
