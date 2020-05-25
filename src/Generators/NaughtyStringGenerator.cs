using System;
using NaughtyStrings;

namespace FuzzDotNet.Generators
{
    /// <summary>
    /// 
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
