using System;
using System.Collections.Generic;
using System.Text;
using FuzzDotNet.Core.Utilities;

namespace FuzzDotNet.Core.Generators
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
