using System;
using System.Reflection;
using System.Linq;

namespace FuzzDotNet.Core.Generation
{
    /// <summary>
    /// Generates objects of classes by choosing a public constructor and calling it with generated arguments.
    /// </summary>
    public class ConstructedObjectGenerator : Generator
    {
        public override bool CanGenerate(IFuzzProfile profile, Type type)
        {
            return type.GetConstructors().Length > 0
                && !type.ContainsGenericParameters;
        }

        public override object? Generate(IFuzzProfile profile, Type type, FuzzRandom random)
        {
            var constructor = random.Choice(type.GetConstructors());

            var arguments = constructor.GetParameters()
                .Select(p => profile.Generate(p.ParameterType, random));

            return constructor.Invoke(arguments.ToArray());
        }
    }
}
