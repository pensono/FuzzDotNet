using System;
using FuzzDotNet.Core;
using FuzzDotNet.Core.Utilities;

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

        public ConstantGenerator(object value)
        {
            // TODO Do a null generator separately
            _value = value;
        }

        public override bool CanGenerate(Type parameterType)
        {
            return parameterType.IsCovariantSubtypeOf(_value.GetType());
        }

        public override object? Generate(Type type, FuzzRandom random)
        {
            return _value;
        }
    }
}
