using System;
using System.Collections.Generic;
using System.Text;
using FuzzDotNet.Formatting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FuzzDotNet.Test.Formatting
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
            var counterexample = new Counterexample(Mock.Of<ITestMethod>(), arguments);

            var formatted = new JsonFormatter().Format(counterexample);

            var expected = @"{
  ""number"": 4,
  ""string"": ""hello""
}";

            Assert.AreEqual(expected, formatted);
        }

        [FuzzTestMethod]
        public void DoesNotEndWithNewline(IList<Argument> arguments)
        {
            if (!arguments.UniqueBy(a => a.Name))
            {
                return;
            }

            var formatter = new JsonFormatter();
            var formatted = formatter.Format(new Counterexample(Mock.Of<ITestMethod>(), arguments));

            Assert.IsFalse(formatted.EndsWith("\n"));
        }
    }
}
