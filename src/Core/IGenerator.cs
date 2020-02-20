using System;
using System.Reflection;
using System.Linq;

namespace FuzzDotNet.Core
{
    public interface IGenerator
    {
        public bool CanGenerate(Type parameterType);

        /// <summary>
        /// Generates a random value.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="random">The source of randomness to use during generation.</param>
        /// <returns>A sampled value.</returns>
        public object? Generate(Type type, FuzzRandom random);
    }
}
