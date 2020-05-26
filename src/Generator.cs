using System;

namespace FuzzDotNet
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public abstract class Generator : Attribute, IGenerator
    {
        public abstract bool CanGenerate(Type type);

        public abstract object? Generate(IFuzzContext context, Type type, FuzzRandom random);
    }
}
