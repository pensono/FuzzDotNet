using System;
using NaughtyStrings;

namespace FuzzDotNet.Core.Generation
{
    /// <summary>
    /// Generates potentially problematic strings.
    /// </summary>
    /// <remarks>
    /// It would be cool if this class could be generic
    /// </remarks>
    public class NaughtyStringGenerator : ChoiceGenerator
    {
        protected override Type ItemType => typeof(string);

        public NaughtyStringGenerator()
            : base(TheNaughtyStrings.All)
        {
        }
    }
}
