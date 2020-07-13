using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FuzzDotNet.Generation;
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
            // TODO Use a better way of getting the profile fed in
            IFuzzProfile profile = new NaughtyFuzzProfile();
            var seedGenerator = new FuzzRandom();
            var results = new List<TestResult>();
            var stopwatch = new Stopwatch();

            var argumentGenerators = testMethod.MethodInfo.GetParameters()
                .Select(parameter => {
                    var generator = GetGenerator(profile, parameter);

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
                        return g.Generator.Generate(profile, g.ParameterType, random);
                    });

                var result = testMethod.Invoke(arguments.ToArray());

                if (result.Outcome != UnitTestOutcome.Passed)
                {
                    result.DatarowIndex = seed;

                    results.Add(result);
                }
            }

            stopwatch.Stop();
            var passedIterationCount = Iterations - results.Count;

            var summaryResult = new TestResult
            {
                Outcome = UnitTestOutcome.Passed,
                Duration = stopwatch.Elapsed,
                TestContextMessages = $"{testMethod.TestMethodName} ({passedIterationCount} iterations passed)",
            };

            if (results.Any())
            {
                // If there were any test failures, title the summary with the number of passed iterations.
                // Otherwise, leave it blank and the title will default to the test method's name.
                summaryResult.DisplayName = summaryResult.TestContextMessages;
            }

            results.Insert(0, summaryResult);

            return results.ToArray();
        }

        private static IGenerator GetGenerator(IFuzzProfile profile, ParameterInfo parameter)
        {
            var generatorAttribute = parameter.GetCustomAttribute<Generator>();

            return generatorAttribute ?? profile.GeneratorFor(parameter.ParameterType);
        }
    }
}
