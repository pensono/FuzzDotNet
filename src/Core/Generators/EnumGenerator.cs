using System;
using System.Collections.Generic;
using System.Text;
using FuzzDotNet.Core.Utilities;

namespace FuzzDotNet.Core.Generators
{
    public class EnumGenerator : Generator
    {
        public override bool CanGenerate(Type parameterType)
        {
            return parameterType.IsSubclassOf(parameterType);
        }

        public override object? Generate(Type type, FuzzRandom random)
        {
            Check.IsTrue(type.IsEnum);

            return random.Choice(type.GetEnumValues());
        }
    }
}
