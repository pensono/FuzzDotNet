using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FuzzDotNet.Generation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet
{
    [AttributeUsage(AttributeTargets.Method)]  
    public class FuzzTestMethodAttribute : TestMethodAttribute
    {
        private IFuzzProfile? _fuzzProfile;

        public IFuzzProfile FuzzProfile {
            get
            {
                if (_fuzzProfile == null)
                {
                    if (FuzzProfileType == null)
                    {
                        _fuzzProfile = CreateFuzzProfile();
                    } 
                    else
                    {
                        var constructor = FuzzProfileType.GetConstructor(Array.Empty<Type>());
                        if (constructor == null)
                        {
                            // Note that this requirement prohibits parameterized fuzz profiles
                            // The best way to do this is to subclass FuzzTestMethodAttribute and forward the parameters from that to the fuzz profile
                            throw new ArgumentException($"Fuzz profile type {FuzzProfileType.FullName} is not default-constructable.");
                        }

                        _fuzzProfile = (IFuzzProfile)constructor.Invoke(Array.Empty<object?>());
                    }
                }

                return _fuzzProfile;
            }

            set 
            {
                _fuzzProfile = value;
            }
        }

        /// <summary>
        /// The type of the fuzz profile to use.
        /// </summary>
        /// <remarks>
        /// Example usage:
        /// <code>
        ///         [FuzzTestMethod(FuzzProfileType = typeof(CustomFuzzProfile)]
        ///         public void FuzzTest(int value)
        ///         {
        ///             // ...
        ///         }
        /// </code>
        /// </remarks>
        public Type? FuzzProfileType { get; set; }

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
                    result.DatarowIndex = seed;
                    results.Add(result);

                    // Parameter name will never be null because this paramater is not a return parameter
                    var fuzzArguments = arguments.Zip(
                        testMethod.MethodInfo.GetParameters(),
                        (a, p) => new Argument(p.Name!, a));

                    var counterexample = new Counterexample(testMethod, fuzzArguments.ToList());

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
