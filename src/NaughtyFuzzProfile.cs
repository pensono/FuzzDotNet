using System;
using System.Collections.Generic;
using FuzzDotNet.Formatting;
using FuzzDotNet.Generation;
using FuzzDotNet.Notification;

namespace FuzzDotNet
{
    public class NaughtyFuzzProfile : FuzzProfile
    {
        public override IEnumerable<IGenerator> DefaultGenerators => new List<IGenerator> {
            new NaughtyStringGenerator(),
            new NaughtyIntGenerator(),
            new EnumGenerator(),
            new EnumerableGenerator(),
            new SubclassGenerator(),
            new DataObjectGenerator(),
            new ConstructedObjectGenerator(),
        };

        public override INotifier Notifier { get; set; } = new ConsoleNotifier(new JsonFormatter());
    }
}
