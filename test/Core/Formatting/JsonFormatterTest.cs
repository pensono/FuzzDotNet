using System;
using System.Collections.Generic;
using System.Reflection;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Formatting;
using FuzzDotNet.MSTest;
using FuzzDotNet.Test.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuzzDotNet.Test.Core.Formatting
{
    [TestClass]
    public class JsonFormatterTest
    {
        [TestMethod]
        public void Basic()
        {
            var arguments = new[]
            {
                new Argument("number", 4),
                new Argument("string", "hello"),
            };
            var counterexample = new Counterexample(Mock.Of<MethodInfo>(), arguments);

            var formatted = new JsonFormatter().Format(counterexample);

            var expected = @"{
  ""number"": 4,
  ""string"": ""hello""
}";

            Assert.That.AreEqualIgnoreNewlineStyle(expected, formatted);
        }

        [FuzzTestMethod]
        public void DoesNotEndWithNewline(IList<Argument> arguments)
        {
            if (!arguments.UniqueBy(a => a.Name))
            {
                return;
            }

            var formatter = new JsonFormatter();
            var formatted = formatter.Format(new Counterexample(Mock.Of<MethodInfo>(), arguments));
            Assert.IsFalse(formatted.EndsWith("\n"));
        }
    }
}
