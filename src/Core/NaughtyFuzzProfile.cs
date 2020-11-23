using System;
using System.Collections.Generic;
using FuzzDotNet.Core.Formatting;
using FuzzDotNet.Core.Generation;
using FuzzDotNet.Core.Notification;
using FuzzDotNet.Core.Simplification;

namespace FuzzDotNet.Core
{
    public class NaughtyFuzzProfile : FuzzProfile
    {
        public override IEnumerable<IGenerator> Generators => new List<IGenerator> {
            new NaughtyStringGenerator(),
            new NaughtyIntGenerator(),
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
