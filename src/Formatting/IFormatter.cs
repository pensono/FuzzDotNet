using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzDotNet.Formatting
{
    public interface IFormatter
    {
        /// <summary>
        /// Formats a counterexample found by fuzzing.
        /// </summary>
        /// <remarks>
        /// The resulting string should not include a newline character at the end of the final line.
        /// </remarks>
        /// <param name="counterexample">The counterexample to format.</param>
        /// <returns>A formatted version of the counterexample.</returns>
        public string Format(Counterexample counterexample);
    }
}
