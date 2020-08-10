using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzDotNet
{
    public static class FuzzProfileExtensions
    {
        public static object? Generate(this IFuzzProfile profile, Type type, FuzzRandom random) 
        {
            return profile.GeneratorFor(type).Generate(profile, type, random);
        }
    }
}
