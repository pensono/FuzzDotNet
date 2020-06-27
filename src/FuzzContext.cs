using FuzzDotNet.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FuzzDotNet
{
    public abstract class FuzzContext : IFuzzContext
    {
        public abstract IEnumerable<IGenerator> DefaultGenerators { get; }

        public virtual IGenerator GeneratorFor(Type type) 
        {
            // TODO memoize the results
            var generator = DefaultGenerators.FirstOrDefault(generator => generator.CanGenerate(type));

            if (generator == null)
            {
                throw new NotImplementedException($"{GetType()} does not gave a generator which can generate a {type.FullName}");
            }

            return generator;
        }
    }
}
