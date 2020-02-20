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
        private readonly IGenerator _elementGenerator;
        private readonly double _averageSize;

        public EnumerableGenerator(Type generatorType, int averageSize = 10, params object?[] constructorArguments)
        {
            _elementGenerator = GeneratorBuilder.BuildGenerator(generatorType, constructorArguments);
            _averageSize = averageSize;
        }

        public override object? Generate(Type type, FuzzRandom random)
        {
            var elementType = type.GetEnumerableElementType();

            var length = (int)Math.Round(random.Poisson(_averageSize));

            // Can't use the regular constructor because we don't have a generic type
            var listType = typeof(List<>).MakeGenericType(new []{ elementType });
            var result = Activator.CreateInstance(listType);
            Check.IsNotNull(result);

            var add = listType.GetMethod("Add")!;

            for (var i = 0; i < length; i++)
            {
                var element = _elementGenerator.Generate(elementType, random);
                add.Invoke(result, new[]{ element });
            }

            return result;
        }

        public override bool CanGenerate(Type parameterType)
        {
            return typeof(IEnumerable<>).IsSubclassOf(parameterType) && _elementGenerator.CanGenerate(parameterType.GetEnumerableElementType());
        }
    }
}
