using System;
using System.Collections.Generic;

namespace FuzzDotNet.Generation
{
    /// <summary>
    /// Generates integers uniformly
    /// </summary>
    public class UniformIntGenerator : Generator
    {
        public int Min { get; set; } = int.MinValue;
        public int Max { get; set; } = int.MaxValue;

        public override bool CanGenerate(Type type) =>  type == typeof(int);

        public override object? Generate(IFuzzProfile profile, Type type, FuzzRandom random)
        {
            return random.Uniform(Min, Max);
        }
    }
}
