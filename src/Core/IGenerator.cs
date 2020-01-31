using System;
using System.Reflection;
using System.Linq;

namespace FuzzDotNet
{
    // TODO figure out how this definition could be used
    //public interface IGenerator<out T>
    public interface IGenerator
    {
        /// <summary>
        /// Generates a random value.
        /// </summary>
        /// <param name="random">The source of randomness to use during generation.</param>
        /// <returns>A sampled value.</returns>
        public object? Generate(Random random);
    }
}
