using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FuzzDotNet.Formatting;
using FuzzDotNet.Generation;
using FuzzDotNet.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet
{
    [AttributeUsage(AttributeTargets.Method)]  
    public class FuzzTestMethodAttribute : TestMethodAttribute
    {
        private IFuzzProfile? _fuzzProfile;

        public IFuzzProfile FuzzProfile {
            // Note that this requirement prohibits parameterized fuzz profiles
            // The best way to do this is to subclass FuzzTestMethodAttribute and forward the parameters from that to the fuzz profile
            get => ReflectionExtensions.GetDefaultConstructableParameter(ref _fuzzProfile, FuzzProfileType, CreateFuzzProfile);
            set => _fuzzProfile = value;
        }

        /// <summary>
        /// The type of the fuzz profile to use.
        /// </summary>
        /// <remarks>
        /// Example usage:
        /// <code>
        /// [FuzzTestMethod(FuzzProfileType = typeof(CustomFuzzProfile)]
        /// public void FuzzTest(int value)
        /// {
        ///     // ...
        /// }
        /// </code>
        /// </remarks>
        public Type? FuzzProfileType { get; set; }

        private IFormatter? _resultFormatter;

        // Does this property belong here? Or in the FuzzProfile?
        public IFormatter TestResultFormatter {
            get => ReflectionExtensions.GetDefaultConstructableParameter(ref _resultFormatter, TestResultFormatterType, CreateTestResultFormatter);
            set => _resultFormatter = value;
        }

        /// <summary>
        /// The type of the formatter to use when creating TestResults for MSTest.
        /// </summary>
        /// <remarks>
        /// Example usage:
        /// <code>
        /// [FuzzTestMethod(TestResultFormatterType = typeof(JsonFormatter)]
        /// public void FuzzTest(int value)
        /// {
        ///     // ...
        /// }
        /// </code>
        /// </remarks>
        public Type? TestResultFormatterType { get; set; }

        public int Iterations { get; set; } = 20;

        public override TestResult[] Execute(ITestMethod testMethod)
        {
            var seedGenerator = new FuzzRandom();
            var results = new List<TestResult>();
            var stopwatch = new Stopwatch();
            var notifyTasks = new List<Task>();

            var argumentGenerators = testMethod.MethodInfo.GetParameters()
                .Select(parameter => {
                    var generator = GetGenerator(FuzzProfile, parameter);

                    return (Generator: generator, parameter.ParameterType);
                });

            var dataSourceResults = InvokeDataSourceTests(testMethod);
            results.AddRange(dataSourceResults);

            stopwatch.Start();
            for (var iteration = 0; iteration < Iterations; iteration++)
            {
                var seed = seedGenerator.Uniform(int.MinValue, int.MaxValue);
                var random = new FuzzRandom(seed);

                var arguments = argumentGenerators
                    .Select(g => {
                        return g.Generator.Generate(FuzzProfile, g.ParameterType, random);
                    })
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
            var passedIterationCount = Iterations - results.Count;

            Task.WaitAll(notifyTasks.ToArray());

            var summaryResult = new TestResult
            {
                Outcome = UnitTestOutcome.Passed,
                Duration = stopwatch.Elapsed - results.Aggregate(TimeSpan.Zero, (acc, result) => acc + result.Duration),
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

        private IEnumerable<TestResult> InvokeDataSourceTests(ITestMethod testMethod)
        {
            // Unfortunately, MSTest doesn't seem to expose an easy way to get the data rows.
            // Referenced: https://github.com/microsoft/testfx/blob/efa1a6d93497719b51ada27787d7f6fcfdd24afe/src/Adapter/MSTest.CoreAdapter/Execution/TestMethodRunner.cs
            var dataSources = testMethod.MethodInfo.GetCustomAttributes<Attribute>().OfType<ITestDataSource>().ToList();

            foreach (var dataSource in dataSources)
            {
                var rows = dataSource.GetData(testMethod.MethodInfo).ToList();

                for (int i = 0; i < rows.Count; i++)
                {
                    var result = testMethod.Invoke(rows[i]);

                    result.DatarowIndex = i;
                    result.DisplayName = dataSource.GetDisplayName(testMethod.MethodInfo, rows[i]);

                    yield return result;
                }
            }
        }

        /// <summary>
        /// Creates a fuzz profile to be used during fuzz tests.
        /// </summary>
        /// <remarks>
        /// This may be overridden to use a custom fuzz profile anywhere a subclass of this attribute is used.
        /// </remarks>
        /// <returns>The fuzz profile.</returns>
        protected virtual IFuzzProfile CreateFuzzProfile()
        {
            return new NaughtyFuzzProfile();
        }

        /// <summary>
        /// Creates a result formatter to be used when a fuzz test finds a counterexample.
        /// </summary>
        /// <remarks>
        /// This may be overridden to use a custom fuzz profile anywhere a subclass of this attribute is used.
        /// </remarks>
        /// <returns>The formatter.</returns>
        protected virtual IFormatter CreateTestResultFormatter()
        {
            // TODO default this to YAML. It's more human-readable
            return new JsonFormatter();
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
