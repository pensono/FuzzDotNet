using System;
using System.Collections.Generic;
using System.Linq;
using FuzzDotNet.Generation;
using FuzzDotNet.Notification;

namespace FuzzDotNet
{
    public abstract class FuzzProfile : IFuzzProfile
    {
        public abstract INotifier Notifier { get; set; }

        public abstract IEnumerable<IGenerator> DefaultGenerators { get; }

        public virtual IGenerator? GeneratorFor(Type type) 
        {
            // TODO memoize the results
            return DefaultGenerators.FirstOrDefault(generator => generator.CanGenerate(this, type));
        }
    }
}
