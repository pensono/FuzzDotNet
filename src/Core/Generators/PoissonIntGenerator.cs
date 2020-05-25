using System;
using System.Collections.Generic;

namespace FuzzDotNet.Core.Generators
{
    /// <summary>
    /// Generates integers around a typical value
    /// </summary>
    public class PoissonIntGenerator : Generator
    {
        public int Mean { get; set; }

        public override bool CanGenerate(Type type) =>  type == typeof(int);

        public override object? Generate(IFuzzContext context, Type type, FuzzRandom random)
        {
            return random.Poisson(Mean);
        }
    }
}
