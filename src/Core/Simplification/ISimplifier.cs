using System;

namespace FuzzDotNet.Core.Simplification
{
    public interface ISimplifier
    {
        public bool CanSimplify(IFuzzProfile profile, Type type);

        /// <summary>
        /// Simplifies an input. The simplified result must return true when tested with the predicate.
        /// </summary>
        /// <param name="input">The value to simplify. The predicate must evaluate to true on this input. This value should not be mutated.</param>
        /// <param name="isValid">The predicate that the returned value must meet.</param>
        /// <returns>The simplified object.</returns>
        public object? Simplify(IFuzzProfile profile, object? input, Func<object?, bool> isValid);
    }
}
