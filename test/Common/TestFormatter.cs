using FuzzDotNet.Core;
using FuzzDotNet.Core.Formatting;

namespace FuzzDotNet.Test
{
    /// <summary>
    /// A test fuzz profile to make things predictable.
    /// </summary>
    public class TestFormatter : IFormatter
    {
        public static string GeneratedString { get; } = "I'm a test formatted counterexample!";

        public static TestFormatter Instance = new TestFormatter();

        private TestFormatter() { }

        public string Format(Counterexample counterexample)
        {
            return GeneratedString;
        }
    }
}
