using System;
using System.Collections.Generic;

namespace FuzzDotNet.Generators
{
    /// <summary>
    /// Generates potentially problematic integers
    /// </summary>
    public class NaughtyIntGenerator : ChoiceGenerator
    {
        protected override Type ItemType => typeof(int);

        private static readonly IReadOnlyList<int> NaughtyInts = new List<int>()
        {
            // Arbitrarily chosen values
            1, 0, -1,
            int.MaxValue, int.MinValue,
            int.MinValue + 1, int.MaxValue - 1
        };

        public NaughtyIntGenerator()
            : base(NaughtyInts)
        {
        }
    }
}
