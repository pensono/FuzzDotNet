using System;
using NaughtyStrings;

namespace FuzzDotNet.Core.Generators
{
    /// <summary>
    /// 
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
