using System;

namespace FuzzDotNet.Simplification
{
    public abstract class SimplifierBase<T> : ISimplifier
    {
        public bool CanSimplify(IFuzzProfile profile, Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }

        /// <inheritdoc/>
        public object? Simplify(IFuzzProfile profile, object? input, Func<object?, bool> isValid)
        {
            // If the input is null, always consider it simplified
            if (input == null)
            {
                return null;
            }

            if (!(input is T inputT))
            {
                throw new ArgumentException();
            }

            return SimplifyInstance(profile, inputT, isValid);
        }

        public abstract T SimplifyInstance(IFuzzProfile profile, T input, Func<object?, bool> isValid);
    }
}
