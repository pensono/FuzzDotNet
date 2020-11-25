using FuzzDotNet.Core.Generation;
using FuzzDotNet.Core.Notification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FuzzDotNet.Core
{
    public class FuzzTestDriver
    {
        public IEnumerable<Counterexample> RunTest(Action<object[]> runIteration, int iterations, INotifier? notifier)
        {

            var seedGenerator = new FuzzRandom();
            var results = new List<TestResult>();
            var stopwatch = new Stopwatch();
            var notifyTasks = new List<Task>();

            stopwatch.Start();
            for (var iteration = 0; iteration < iterations; iteration++)
            {
                var seed = seedGenerator.Uniform(int.MinValue, int.MaxValue);
                var random = new FuzzRandom(seed);

                
                    .ToList();

                var result = testMethod.Invoke(arguments.ToArray());

                if (result.Outcome != UnitTestOutcome.Passed)
                {
                    // Parameter name will never be null because this paramater is not a return parameter
                    var fuzzArguments = arguments.Zip(
                        testMethod.MethodInfo.GetParameters(),
                        (a, p) => new Argument(p.Name!, a));

                    var counterexample = new Counterexample(testMethod, fuzzArguments.ToList());

                    result.DatarowIndex = seed;
                    result.TestContextMessages = TestResultFormatter.Format(counterexample);
                    results.Add(result);

                    var notifyTask = FuzzProfile.Notifier.NotifyCounterexampleAsync(counterexample);
                    notifyTasks.Add(notifyTask);
                }
            }

            stopwatch.Stop();

            Task.WaitAll(notifyTasks.ToArray());
        }

        public static IEnumerable<object?[]> GenerateArguments(IFuzzProfile profile, MethodInfo method, int? seed = null)
        {
            var seedGenerator = seed is int s ? new FuzzRandom(s) : new FuzzRandom();

            var argumentGenerators = method.GetParameters()
            .Select(parameter => {
                var generator = GetGenerator(profile, parameter);

                return (Generator: generator, parameter.ParameterType);
            });

            while (true)
            {
                var random = seedGenerator.CreatedNested();

                yield return argumentGenerators
                        .Select(g => g.Generator.Generate(profile, g.ParameterType, random))
                        .ToArray();
            }
        }


        private static IGenerator GetGenerator(IFuzzProfile profile, ParameterInfo parameter)
        {
            var generatorAttribute = parameter.GetCustomAttribute<Generator>();

            if (generatorAttribute != null)
            {
                if (!generatorAttribute.CanGenerate(profile, parameter.ParameterType))
                {
                    throw new IncompatibleGeneratorException($"The generator of type {generatorAttribute.GetType()} cannot generate the parameter {parameter.Name} of type {parameter.ParameterType}");
                }

                return generatorAttribute;
            }
            else
            {
                return profile.GeneratorForOrThrow(parameter.ParameterType);
            }
        }
    }
}
