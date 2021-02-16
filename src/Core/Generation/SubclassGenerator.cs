using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzDotNet.Core.Generation
{
    /// <summary>
    /// A generator which generates subclasses of a type. Only subclasses from the type's assembly
    /// will be loaded.
    /// </summary>
    public class SubclassGenerator : Generator
    {
        private static readonly IDictionary<Type, IList<Type>> _directSubclasses = new Dictionary<Type, IList<Type>>();

        public override bool CanGenerate(IFuzzProfile profile, Type type)
        {
            return type.IsAbstract && DirectSubclassesOf(type).All(t => profile.GeneratorFor(t) != null);
        }

        public override object? Generate(IFuzzProfile profile, Type type, FuzzRandom random)
        {
            // Only find direct subclasses. Indirect subclasses can be found recursively
            var candidateClasses = DirectSubclassesOf(type)
                .ToList();

            var generatedType = random.Choice(candidateClasses);

            return profile.Generate(generatedType, random);
        }

        private IList<Type> DirectSubclassesOf(Type type)
        {
            if (!_directSubclasses.TryGetValue(type, out var subclasses))
            {
                subclasses = type.Assembly.GetTypes().Where(t => t.BaseType == type).ToList();
                _directSubclasses[type] = subclasses;
            }

            return subclasses;
        }
    }
}
