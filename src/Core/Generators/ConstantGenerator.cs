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

        public override bool CanGenerate(Type type)
        {
            return _value == null || type.IsAssignableFrom(_value.GetType());
        }

        public override object? Generate(IFuzzContext context, Type type, FuzzRandom random)
        {
            return _value;
        }
    }
}
