using System;

namespace FuzzDotNet.Core
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public abstract class Generator : Attribute, IGenerator
    {
        // Is there some way to omit this method so that inheritors don't need to specify override?
        public abstract object? Generate(FuzzRandom random);
    }
}
