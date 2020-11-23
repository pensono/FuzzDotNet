using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzDotNet.Core.Simplification
{
    /// <summary>
    /// Simplification works by performing a greedy depth-first search.
    /// </summary>
    public class GreedyStringSimplifier : GreedySimplifier<string>
    {
        /// <inheritdoc />
        public override IEnumerable<string> SimplificationCandidates(string input)
        {
            // Try removing each individual character
            for (int i = 0; i < input.Length; i++)
            {
                yield return input.Substring(0, i) + input.Substring(i + 1);
            }
        }
    }
}
