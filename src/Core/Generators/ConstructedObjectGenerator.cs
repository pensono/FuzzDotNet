using System;
using System.Reflection;
using System.Linq;

namespace FuzzDotNet.Core.Generators
{
    /// <summary>
    /// Generates objects of classes by choosing a public constructor and calling it with generated arguments.
    /// </summary>
    public class ConstructedObjectGenerator : Generator
    {
        public override bool CanGenerate(Type type)
        {
            return type.GetConstructors().Length > 0;
        }

        public override object? Generate(IFuzzContext context, Type type, FuzzRandom random)
        {
            var constructor = random.Choice(type.GetConstructors());

            var arguments = constructor.GetParameters()
                .Select(p => context.Generate(p.ParameterType, random));

            return constructor.Invoke(arguments.ToArray());
        }
    }
}
