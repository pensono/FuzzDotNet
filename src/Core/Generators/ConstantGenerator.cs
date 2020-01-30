using System;

namespace FuzzDotNet
{
    public class ConstantGenerator<T> : IGenerator
    {
        private readonly T _value;

        public ConstantGenerator(T value) {
            _value = value;
        }

        public object? Generate(Random random)
        {
            return _value;
        }
    }
}
