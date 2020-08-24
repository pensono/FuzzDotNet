using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzDotNet.Simplification
{
    /// <summary>
    /// Simplification works by performing a greedy depth-first search.
    /// </summary>
    /// <typeparam name="T">The type to simplify.</typeparam>
    public abstract class GreedySimplifier<T> : ISimplifier<T>
    {
        /// <summary>
        /// Generates values simpler than the input.
        /// </summary>
        /// <remarks>
        /// The following properties must be true in order to ensure termination of the simplification procedure. 
        /// The return value should start with the simplest case, and work its way to less simple cases.
        /// <list type="bullet">
        /// <item>The return value cannot contain the input item.</item>
        /// <item>The return value cannot be infinite in size.</item>
        /// </list>
        /// 
        /// Note that SimplificationCanidatese may be recursively called on elements of the return value.
        /// </remarks>
        /// <param name="input">The value to generate candidates for.</param>
        /// <returns>Simpler values than the input.</returns>
        public abstract IEnumerable<T> SimplificationCandidates(T input);

        /// <inheritdoc />
        public T Simplify(T input, Func<T, bool> predicate)
        {
            var simplest = input;
            IEnumerable<T> tests = new[] { input };

            while (tests.Any())
            {
                var next = tests.First();
                if (predicate(next))
                {
                    simplest = next;
                    tests = SimplificationCandidates(simplest);
                }
                else
                {
                    tests = tests.Skip(1);
                }
            }

            return simplest;
        }
    }
}
