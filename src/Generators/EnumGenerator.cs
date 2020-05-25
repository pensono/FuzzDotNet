using System;
using System.Collections.Generic;
using System.Text;
using FuzzDotNet.Utilities;

namespace FuzzDotNet.Generators
{
    public class EnumGenerator : Generator
    {
        public override bool CanGenerate(Type type)
        {
            return type.IsEnum;
        }

        public override object? Generate(IFuzzContext context, Type type, FuzzRandom random)
        {
            Check.IsTrue(type.IsEnum);

            return random.Choice(type.GetEnumValues());
        }
    }
}
