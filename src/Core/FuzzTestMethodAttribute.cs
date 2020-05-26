﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;
using FuzzDotNet.Core.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FuzzDotNet.Core
{
    [AttributeUsage(AttributeTargets.Method)]  
    public class FuzzTestMethodAttribute : TestMethodAttribute
    {
        private readonly int _iterations;

        public FuzzTestMethodAttribute(int Iterations = 20)
        {
            _iterations = Iterations;
        }

        public override TestResult[] Execute(ITestMethod testMethod)
        {
            for (var iteration = 0; iteration < _iterations; iteration++)
            {
                var arguments = ArgumentGenerator.GenerateArgumentsFor(testMethod.MethodInfo);
                testMethod.Invoke(arguments.ToArray());
            }

            // TODO Some actual logic that uses the test results
            return new[]{ new TestResult{} };
        }
    }
}