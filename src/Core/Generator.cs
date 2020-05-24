using System;

namespace FuzzDotNet.Core
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public abstract class Generator : Attribute, IGenerator
    {
        public abstract bool CanGenerate(Type type);

        public abstract object? Generate(IFuzzContext context, Type type, FuzzRandom random);
    }
}
