using System;
using System.Collections.Generic;
using FuzzDotNet.Generation;

namespace FuzzDotNet
{
    public class NaughtyFuzzContext : FuzzContext
    {
        public override IEnumerable<IGenerator> DefaultGenerators => new List<IGenerator> {
            new NaughtyStringGenerator(),
            new NaughtyIntGenerator(),
            new EnumGenerator(),
            new EnumerableGenerator(),
            new DataObjectGenerator(),
            new ConstructedObjectGenerator(),
        };
    }
}
