using System.Collections.Generic;
using FuzzDotNet.Generation;

namespace FuzzDotNet
{
    /// <summary>
    /// A test fuzz profile to make things predictable.
    /// </summary>
    public class TestFuzzProfile : FuzzProfile
    {
        public const int GeneratedInt = 42;

        public const string GeneratedString = "The spanish inquisition";

        public override IEnumerable<IGenerator> DefaultGenerators => new List<IGenerator> {
            new ConstantGenerator(GeneratedInt),
            new ConstantGenerator(GeneratedString),
            new EnumGenerator(),
            new EnumerableGenerator(),
            new DataObjectGenerator(),
            new ConstructedObjectGenerator(),
        };
    }
}
