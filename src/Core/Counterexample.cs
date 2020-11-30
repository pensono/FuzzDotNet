using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FuzzDotNet.Core
{
    public class Counterexample
    {
        public MethodInfo TestMethod { get; }

        public IList<Argument> Arguments { get; }

        public Counterexample(MethodInfo testMethod, IList<Argument> arguments)
        {
            TestMethod = testMethod;
            Arguments = arguments;
        }
    }
}
