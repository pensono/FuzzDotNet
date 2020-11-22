using System;

namespace FuzzDotNet.Simplification
{
    /// <summary>
    /// A simplifier that works by performing a binary search between the input and the minimum value in the domain.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BinarySearchSimplifier<T> : SimplifierBase<T>
        where T : IEquatable<T>
    {
        /// <summary>
        /// Gets the simplest value in a domain.
        /// </summary>
        /// <remarks>
        /// The search will tend towards this value.
        /// </remarks>
        public abstract T DomainMinimum { get; }

        /// <summary>
        /// Returns the midpoint between two elements.
        /// </summary>
        /// <remarks>
        /// If the midpoint cannot be exactly represented, then the simplest element should be selected.
        /// 
        /// For example, when searching among integers where the simplest value is zero, the following midpoint 
        /// algorithim could be used. Note that the divison operation always rounds towards zero.
        /// <code>
        /// int Midpoint(int low, int high) => (low + high) / 2;
        /// </code>
        /// 
        /// Implementations must satisfy the following expressions.
        /// <code>
        /// L simpler_than H -> Midpoint(L, H) simpler_than H
        /// L simpler_than H -> Midpoint(L, H) != L
        /// </code>
        /// </remarks>
        /// <param name="low">The lower value.</param>
        /// <param name="high">The upper value.</param>
        /// <returns>The midpoint.</returns>
        public abstract T Midpoint(T low, T high);

        public override T SimplifyInstance(IFuzzProfile profile, T input, Func<object?, bool> isValid)
        {
            if (isValid(DomainMinimum))
            {
                return DomainMinimum;
            }

            // predicate(high) is always true
            var high = input;

            // predicate(low) is always false
            var low = DomainMinimum;

            while (true)
            {
                var midpoint = Midpoint(low, high);

                if (midpoint.Equals(low)) {
                    // Convergence has been achieved
                    return high;
                }
                else if (isValid(midpoint))
                {
                    high = midpoint;
                }
                else { 
                    low = midpoint;
                }
            }
        }
    }
}
