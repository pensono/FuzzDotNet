using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzDotNet.Core.Simplification
{
    /// <summary>
    /// Simplification works by performing a greedy depth-first search.
    /// </summary>
    /// <typeparam name="T">The type to simplify.</typeparam>
    public abstract class GreedySimplifier<T> : SimplifierBase<T>
    {
        /// <summary>
        /// Generates values simpler than the input.
        /// </summary>
        /// <remarks>
        /// This function should return values which are one step simpler than then input. One of the elements of
        /// the return value will be passed into subsequent calls. The return value should start with the simplest
        /// case, and work its way to less simple cases.
        /// 
        /// The following properties must be true in order to ensure termination of the simplification procedure. 
        /// <list type="bullet">
        /// <item>The return value cannot contain the input item.</item>
        /// <item>The return value cannot be infinite in size.</item>
        /// </list>
        /// 
        /// Note that SimplificationCandidates may be recursively called on elements of the return value.
        /// </remarks>
        /// <param name="input">The value to generate candidates for.</param>
        /// <returns>Simpler values than the input.</returns>
        public abstract IEnumerable<T> SimplificationCandidates(T input);

        public override T SimplifyInstance(IFuzzProfile profile, T input, Func<object?, bool> isValid)
        {

            var simplest = input;
            IEnumerable<T> candidates = new [] { input };

            while (candidates.Any())
            {
                var next = candidates.First();
                if (isValid(next))
                {
                    simplest = next;
                    candidates = SimplificationCandidates(simplest);
                }
                else
                {
                    candidates = candidates.Skip(1);
                }
            }

            return simplest;
        }
    }
}
