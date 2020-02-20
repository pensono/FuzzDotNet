using System;

namespace FuzzDotNet.Core
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public abstract class Generator : Attribute, IGenerator
    {
        // Is there some way to omit this method so that inheritors don't need to specify override?
        public abstract object? Generate(Type type, FuzzRandom random);

        public abstract bool CanGenerate(Type parameterType);
    }
}
