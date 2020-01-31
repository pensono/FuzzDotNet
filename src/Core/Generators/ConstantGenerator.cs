using System;

namespace FuzzDotNet
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

        public override object? Generate(Random random)
        {
            return _value;
        }
    }
}
