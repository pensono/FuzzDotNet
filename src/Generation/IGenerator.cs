using System;

namespace FuzzDotNet.Generation
{
    // TODO figure out how this definition could be used
    //public interface IGenerator<out T>
    public interface IGenerator
    {
        bool CanGenerate(IFuzzProfile profile, Type type);

        /// <summary>
        /// Generates a random value.
        /// </summary>
        /// <param name="profile">Generation profile.</param>
        /// <param name="type">The type to generate.</param>
        /// <param name="random">The source of randomness to use during generation.</param>
        /// <returns>A sampled value.</returns>
        object? Generate(IFuzzProfile profile, Type type, FuzzRandom random);
    }
}
