using System;
using System.Collections.Generic;
using System.Linq;
using FuzzDotNet.Generation;
using FuzzDotNet.Notification;
using FuzzDotNet.Simplification;

namespace FuzzDotNet
{
    public abstract class FuzzProfile : IFuzzProfile
    {
        public abstract INotifier Notifier { get; set; }

        public abstract IEnumerable<IGenerator> Generators { get; }

        public virtual IEnumerable<ISimplifier> Simplifiers { get => Array.Empty<ISimplifier>(); }

        public virtual IGenerator? GeneratorFor(Type type) 
        {
            // TODO memoize the results
            return Generators.FirstOrDefault(generator => generator.CanGenerate(this, type));
        }

        public ISimplifier SimplifierFor(Type type)
        {
            // TODO memoize the results
            return Simplifiers.FirstOrDefault(simplifier => simplifier.CanSimplify(this, type)) ?? new IdentitySimplifier();
        }
    }
}
