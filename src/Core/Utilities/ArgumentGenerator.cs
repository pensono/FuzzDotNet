using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FuzzDotNet.Core.Utilities
{
    public static class ArgumentGenerator
    {
        public static object?[] GenerateArgumentsFor(MethodInfo method)
        {
            var argumentGenerators = method.GetParameters().Select(GetGenerator);

            // We'll have to get more sophisticated with how random numbers are generated for better reproducibility
            // Maybe use one random instance which seeds each generator?
            var random = new FuzzRandom();
            return argumentGenerators.Select(f => f(random)).ToArray();
        }

        private static Func<FuzzRandom, object?> GetGenerator(ParameterInfo parameter)
        {
            var generator = parameter.GetCustomAttribute<Generator>();

            if (generator == null)
            {
                parameter.Member.GetCustomAttributes<Generator>();
            }

            return random => generator.Generate(parameter.ParameterType, random);
        }
    }
}
