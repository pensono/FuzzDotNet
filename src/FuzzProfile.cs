using System;
using System.Collections.Generic;
using System.Linq;
using FuzzDotNet.Generation;

namespace FuzzDotNet
{
    public abstract class FuzzProfile : IFuzzProfile
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
