using System;
using System.Linq;

namespace FuzzDotNet.Core.Utilities
{
    public static class GeneratorBuilder
    {

        /// <summary>
        /// Designates what generator should be used for the annotated argument.
        /// </summary>
        /// <param name="generatorType">The type of the generator</param>
        /// <param name="constructorArguments">The arguments to pass to the object during construction.</param>
        public static IGenerator BuildGenerator(Type generatorType, params object?[] constructorArguments)
        {
            // Should this be done in the attribute, or somewhere else?
            // TODO Handle nulls here
            var constructorArgumentTypes = constructorArguments.Select(a => a.GetType()).ToArray();

            var constructor = generatorType.GetConstructor(constructorArgumentTypes);
            Check.IsNotNull(
                constructor,
                $"Could not construct the generator {generatorType.FullName} with argument types {string.Join(",", constructorArgumentTypes.Select(t => t.FullName))}");

            var generator = constructor.Invoke(constructorArguments) as IGenerator;

            Check.IsNotNull(generator, $"Given generator type is not a subclass of {nameof(IGenerator)}");

            return generator;
        }
    }
}
