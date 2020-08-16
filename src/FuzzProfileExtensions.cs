using FuzzDotNet.Generation;
using System;

namespace FuzzDotNet
{
    public static class FuzzProfileExtensions
    {
        public static object? Generate(this IFuzzProfile profile, Type type, FuzzRandom random) 
        {
            var generator = profile.GeneratorForOrThrow(type);

            return generator.Generate(profile, type, random);
        }

        public static IGenerator GeneratorForOrThrow(this IFuzzProfile profile, Type type)
        {
            var generator = profile.GeneratorFor(type);

            if (generator == null)
            {
                throw new NotImplementedException($"{profile.GetType()} does not gave a generator which can generate a {type.FullName}");
            }

            return generator;
        }
    }
}
