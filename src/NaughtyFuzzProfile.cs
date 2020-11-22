using System;
using System.Collections.Generic;
using FuzzDotNet.Formatting;
using FuzzDotNet.Generation;
using FuzzDotNet.Notification;
using FuzzDotNet.Simplification;

namespace FuzzDotNet
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
