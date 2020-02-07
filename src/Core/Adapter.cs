using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using FuzzDotNet.Core.Utilities;

namespace FuzzDotNet.Core
{
    public class Adapter
    {
        // TODO For now, we'll just require that exact type matches are required for each generator. Don't get into inheritance
        private static readonly IDictionary<Type, IGenerator> DefaultGenerators = new Dictionary<Type, IGenerator> {
        };

        public void RunTests(Type testClass) 
        {
            // How much of this can be static?

            var constructor = testClass.GetConstructor(new Type[] {});
            Check.IsNotNull(constructor, $"Type '{testClass}' does not have a no-argument constructor");

            var instance = constructor.Invoke(new object[] {});

            foreach (var method in testClass.GetMethods()) {
                var attribute = method.GetCustomAttribute<FuzzTestMethodAttribute>();
                if (attribute == null) continue;

                var arguments = ArgumentGenerator.GenerateArgumentsFor(method);
                method.Invoke(instance, arguments);
            }
        }
    }
}
