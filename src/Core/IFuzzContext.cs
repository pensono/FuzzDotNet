using System;

namespace FuzzDotNet.Core
{
    public interface IFuzzContext
    {
        /// <summary>
        /// Gets a generator for some type.
        /// </summary>
        IGenerator GeneratorFor(Type type);

        object? Generate(Type type, FuzzRandom random) 
        {
            return GeneratorFor(type).Generate(this, type, random);
        }
    }
}
