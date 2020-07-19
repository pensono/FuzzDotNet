﻿using System;
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

            var argumentGenerators = testMethod.MethodInfo.GetParameters()
                .Select(parameter => {
                    var generator = GetGenerator(FuzzProfile, parameter);

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
                        return g.Generator.Generate(FuzzProfile, g.ParameterType, random);
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

            return generatorAttribute ?? profile.GeneratorFor(parameter.ParameterType);
        }
    }
}
