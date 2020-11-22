using System;

namespace FuzzDotNet.Simplification
{
    /// <summary>
    /// A simplifier which always returns its input.
    /// </summary>
    public class IdentitySimplifier : ISimplifier
    {
        public bool CanSimplify(IFuzzProfile profile, Type type) => true;

        public object? Simplify(IFuzzProfile profile, object? input, Func<object?, bool> isValid) => input;
    }
}
