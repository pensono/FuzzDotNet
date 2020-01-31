using System;
using System.Linq;
using FuzzDotNet.Core.Utility;

namespace FuzzDotNet
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true)]
    public abstract class Generator : Attribute, IGenerator
    {
        // Is there some way to omit this method so that inheritors don't need to specify override?
        public abstract object? Generate(Random random);
    }
}
