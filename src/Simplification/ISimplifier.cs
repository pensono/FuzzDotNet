using System;

namespace FuzzDotNet.Simplification
{
    public interface ISimplifier<T>
    {
        /// <summary>
        /// Simplifies an input. The simplified result must return true when tested with the predicate.
        /// </summary>
        /// <param name="input">The value to simplify. The predicate must evaluate to true on this input.</param>
        /// <param name="isValid">The predicate that the returned value must meet.</param>
        /// <returns></returns>
        public T Simplify(T input, Func<T, bool> isValid);
    }
}
