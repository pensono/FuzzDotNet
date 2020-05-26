using System;
using System.Reflection;
using System.Linq;

namespace FuzzDotNet
{
    // TODO figure out how this definition could be used
    //public interface IGenerator<out T>
    public interface IGenerator
    {
        bool CanGenerate(Type type);

        /// <summary>
        /// Generates a random value.
        /// </summary>
        /// <param name="context">Generation context.</param>
        /// <param name="type">The type to generate.</param>
        /// <param name="random">The source of randomness to use during generation.</param>
        /// <returns>A sampled value.</returns>
        object? Generate(IFuzzContext context, Type type, FuzzRandom random);
    }
}
