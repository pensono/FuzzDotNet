using System;

namespace FuzzDotNet.Core
{
    public class IncompatibleGeneratorException : Exception
    {
        public IncompatibleGeneratorException()
        {
        }

        public IncompatibleGeneratorException(string message)
            : base(message)
        {
        }

        public IncompatibleGeneratorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
