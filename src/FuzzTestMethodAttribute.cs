using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var seedGenerator = new FuzzRandom();
            var results = new List<TestResult>();
            var stopwatch = new Stopwatch();

            var argumentGenerators = testMethod.MethodInfo.GetParameters()
                .Select(parameter => {
                    var generator = GetGenerator(context, parameter);

                    if (!generator.CanGenerate(parameter.ParameterType))
                    {
                        throw new IncompatibleGeneratorException($"The generator of type {generator.GetType()} cannot generate the parameter {parameter.Name} of type {parameter.ParameterType}");
                    }

                    return (Generator: generator, parameter.ParameterType);
                });

            stopwatch.Start();
            for (var iteration = 0; iteration < Iterations; iteration++)
            {
                var seed = seedGenerator.Uniform(int.MinValue, int.MaxValue);
                var random = new FuzzRandom(seed);

                var arguments = argumentGenerators
                    .Select(g => {
                        return g.Generator.Generate(context, g.ParameterType, random);
                    });

                var result = testMethod.Invoke(arguments.ToArray());

                if (result.Outcome != UnitTestOutcome.Passed)
                {
                    result.DatarowIndex = seed;

                    results.Add(result);
                }
            }

            stopwatch.Stop();

            if (!results.Any())
            {
                results.Add(new TestResult
                {
                    Outcome = UnitTestOutcome.Passed,
                    Duration = stopwatch.Elapsed,
                });
            }

            return results.ToArray();
        }

        private static IGenerator GetGenerator(IFuzzContext context, ParameterInfo parameter)
        {
            var generatorAttribute = parameter.GetCustomAttribute<Generator>();

            return generatorAttribute ?? context.GeneratorFor(parameter.ParameterType);
        }
    }
}
