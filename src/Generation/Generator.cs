using FuzzDotNet.Generation;
using System;

namespace FuzzDotNet
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public abstract class Generator : Attribute, IGenerator
    {
        public abstract bool CanGenerate(IFuzzProfile profile, Type type);

        public abstract object? Generate(IFuzzProfile profile, Type type, FuzzRandom random);
    }
}
