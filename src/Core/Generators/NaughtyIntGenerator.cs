using System.Collections.Generic;

namespace FuzzDotNet.Core.Generators
{
    /// <summary>
    /// Generates potentially problematic integers.
    /// </summary>
    /// <remarks>
    /// It would be cool if this class could be generic
    /// </remarks>
    public class NaughtyIntGenerator : ChoiceGenerator
    {
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
