using System;
using System.Linq;
using System.Reflection;
using FuzzDotNet.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet
{
    [AttributeUsage(AttributeTargets.Method)]  
    public class FuzzTestMethodAttribute : TestMethodAttribute
    {
        public int Iterations { get; set; } = 20;

        public override TestResult[] Execute(ITestMethod testMethod)
        {
            // TODO Use a better way of getting the context fed in
            IFuzzContext context = new NaughtyFuzzContext();
            var random = new FuzzRandom();
            
            for (var iteration = 0; iteration < Iterations; iteration++)
            {
                var arguments = testMethod.MethodInfo.GetParameters()
                    .Select(parameter => {
                        var generator = GetGenerator(context, parameter);
                        return generator.Generate(context, parameter.ParameterType, random);
                    });

                testMethod.Invoke(arguments.ToArray());
            }

            // TODO Some actual logic that uses the test results
            return new[]{ new TestResult{} };
        }
        
        private static IGenerator GetGenerator(IFuzzContext context, ParameterInfo parameter)
        {
            var generatorAttribute = parameter.GetCustomAttribute<Generator>();

            return generatorAttribute ?? context.GeneratorFor(parameter.ParameterType);
        }
    }
}
