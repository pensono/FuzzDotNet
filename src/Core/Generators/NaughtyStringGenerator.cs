using NaughtyStrings;

namespace FuzzDotNet.Core.Generators
{
    /// <summary>
    /// Generates potentially problematic strings.
    /// </summary>
    /// <remarks>
    /// It would be cool if this class could be generic
    /// </remarks>
    public class NaughtyStringGenerator : ChoiceGenerator
    {
        public NaughtyStringGenerator()
            : base(TheNaughtyStrings.All)
        {
        }
    }
}
