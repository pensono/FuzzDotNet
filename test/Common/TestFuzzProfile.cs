using System;
using System.Collections.Generic;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Formatting;
using FuzzDotNet.Core.Generation;
using FuzzDotNet.Core.Notification;
using FuzzDotNet.Core.Simplification;

namespace FuzzDotNet.Test
{
    /// <summary>
    /// A test fuzz profile to make things predictable.
    /// </summary>
    public class TestFuzzProfile : FuzzProfile
    {
        public const int GeneratedInt = 42;

        public const string GeneratedString = "The spanish inquisition";

        public override IEnumerable<IGenerator> Generators => new List<IGenerator> {
            new ConstantGenerator(GeneratedInt),
            new ConstantGenerator(GeneratedString),
            new EnumGenerator(),
            new EnumerableGenerator(),
            new SubclassGenerator(),
            new DataObjectGenerator(),
            new ConstructedObjectGenerator(),
        };

        public override IEnumerable<ISimplifier> Simplifiers => new List<ISimplifier>
        {
            new BinarySearchIntegerSimplifier(),
            new GreedyStringSimplifier(),
            new DataObjectSimplifier(),
        };

        public override INotifier Notifier { get; set; } = new ConsoleNotifier(new JsonFormatter());
    }
}
