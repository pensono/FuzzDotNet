using System;
using FuzzDotNet.Core;

namespace FuzzDotNet.Core.Generators
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// It would be cool if this class could be generic
    /// </remarks>
    public class ConstantGenerator : Generator
    {
        private readonly object? _value;

        public ConstantGenerator(object? value)
        {
            _value = value;
        }

        public override object? Generate(Type type, FuzzRandom random)
        {
            return _value;
        }
    }
}
