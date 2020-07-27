using System;
using System.Collections.Generic;
using FuzzDotNet.Formatting;
using FuzzDotNet.Generation;
using FuzzDotNet.Notification;

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

        public override INotifier Notifier { get; set; } = new ConsoleNotifier(new JsonFormatter());
    }
}
