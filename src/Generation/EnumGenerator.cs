using System;
using System.Collections.Generic;
using System.Text;
using FuzzDotNet.Utilities;

namespace FuzzDotNet.Generation
{
    public class EnumGenerator : Generator
    {
        public override bool CanGenerate(IFuzzProfile profile, Type type)
        {
            return type.IsEnum;
        }

        public override object? Generate(IFuzzProfile profile, Type type, FuzzRandom random)
        {
            Check.IsTrue(type.IsEnum);

            return random.Choice(type.GetEnumValues());
        }
    }
}
