using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FuzzDotNet.Core.Utilities;

namespace FuzzDotNet.Core.Generators
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
        /// The element generator. If null, the generator from the context should be used.
        /// </summary>

        private readonly IGenerator? _elementGenerator;
        private readonly double _averageSize;

        public EnumerableGenerator(int averageSize = 10, Type? elementGeneratorType = null, params object?[] constructorArguments)
        {
            _elementGenerator = elementGeneratorType == null ? null : GeneratorBuilder.BuildGenerator(elementGeneratorType, constructorArguments);
            _averageSize = averageSize;
        }

        public override bool CanGenerate(Type type)
        {
            return type.IsAssignableFrom(typeof(IEnumerable<>)) 
                && (_elementGenerator == null || _elementGenerator.CanGenerate(type.GetEnumerableElementType()));
        }

        public override object? Generate(IFuzzContext context, Type type, FuzzRandom random)
        {
            var elementType = type.GetEnumerableElementType();
            var elementGenerator = _elementGenerator ?? context.GeneratorFor(type);

            var length = (int)Math.Round(random.Poisson(_averageSize));

            // Can't use the regular constructor because we don't have a generic type
            var listType = typeof(List<>).MakeGenericType(new []{ elementType });
            var result = Activator.CreateInstance(listType);
            Check.IsNotNull(result);

            var add = listType.GetMethod("Add");
            Check.IsNotNull(add);

            for (var i = 0; i < length; i++)
            {
                var element = elementGenerator.Generate(context, elementType, random);
                add.Invoke(result, new[]{ element });
            }

            return result;
        }
    }
}
