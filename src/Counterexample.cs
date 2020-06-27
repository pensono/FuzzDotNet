using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzDotNet
{
    public class Counterexample
    {
        public ITestMethod TestMethod { get; }

        public IList<Argument> Arguments { get; }

        public Counterexample(ITestMethod testMethod, IList<Argument> arguments)
        {
            TestMethod = testMethod;
            Arguments = arguments;
        }
    }
}
