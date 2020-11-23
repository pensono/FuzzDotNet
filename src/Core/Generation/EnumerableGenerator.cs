using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FuzzDotNet.Core.Utilities;

namespace FuzzDotNet.Core.Generation
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// It would be cool if this class could be generic
    /// </remarks>
    public class EnumerableGenerator : Generator
    {
        /// <summary>
        /// The element generator. If null, the generator from the profile should be used.
        /// </summary>
        private readonly IGenerator? _elementGenerator;
        private readonly double _averageSize;

        /// <summary>
        /// Implementations associated with various interfaces. Ordered with the most specific interfaces first
        /// </summary>
        /// <remarks>
        /// I'm not entirely sure if this is the best way to do this.
        /// It might be better to have the generator itself be generic enough to handle different interfaces
        /// </remarks>
        private static readonly IList<(Type Interface, Type Implementation)> Implementations = new []
        {
            (typeof(IList<>), typeof(List<>)),
            (typeof(ISet<>), typeof(HashSet<>)),
            (typeof(IReadOnlyCollection<>), typeof(List<>)),
            (typeof(IReadOnlyList<>), typeof(List<>)),
            (typeof(ICollection<>), typeof(List<>)),
            (typeof(IEnumerable<>), typeof(List<>)),
        };

        public EnumerableGenerator(int averageSize = 10, Type? elementGeneratorType = null, params object?[] elementGeneratorConstructorArguments)
        {
            _elementGenerator = elementGeneratorType == null ? null : GeneratorBuilder.BuildGenerator(elementGeneratorType, elementGeneratorConstructorArguments);
            _averageSize = averageSize;
        }

        public override bool CanGenerate(IFuzzProfile profile, Type type)
        {
            if (GenericImplementationType(type) == null)
            {
                // Non-generic enumerable type
                return false;
            }

            if (_elementGenerator == null) {
                return profile.GeneratorFor(type.GetEnumerableElementType()) != null;
            }
            else 
            {
                return _elementGenerator.CanGenerate(profile, type.GetEnumerableElementType());
            }
        }

        public override object? Generate(IFuzzProfile profile, Type type, FuzzRandom random)
        {
            var elementType = type.GetEnumerableElementType();
            var elementGenerator = _elementGenerator ?? profile.GeneratorForOrThrow(elementType);

            var length = (int)Math.Round(random.Poisson(_averageSize));

            // Can't use the regular constructor because we don't have a generic type
            var genericImplementationType = GenericImplementationType(type)!;
            var implementationType = genericImplementationType.MakeGenericType(new []{ elementType });
            var result = Activator.CreateInstance(implementationType);
            Check.IsNotNull(result);

            var add = implementationType.GetMethod("Add");
            Check.IsNotNull(add);

            for (var i = 0; i < length; i++)
            {
                var element = elementGenerator.Generate(profile, elementType, random);
                add.Invoke(result, new[]{ element });
            }

            return result;
        }

        private Type? GenericImplementationType(Type type)
        {
            if (!type.IsGenericType)
            {
                return null;
            }

            var genericType = type.GetGenericTypeDefinition();
            var interfaces = genericType.GetInterfaces().Prepend(genericType);

            foreach (var (interfaceType, implementation) in Implementations)
            {
                if (interfaces.Contains(interfaceType))
                {
                    return implementation;
                }
            }

            return null;
        }
    }
}
