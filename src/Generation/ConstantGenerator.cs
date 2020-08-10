using System;
using FuzzDotNet;

namespace FuzzDotNet.Generation
{
    /// <summary>
    /// Always generates the same (user-defined) value.
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

        public override bool CanGenerate(IFuzzProfile profile, Type type)
        {
            return _value == null || type.IsAssignableFrom(_value.GetType());
        }

        public override object? Generate(IFuzzProfile profile, Type type, FuzzRandom random)
        {
            return _value;
        }
    }
}
