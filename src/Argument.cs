using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzDotNet
{
    public class Argument
    {
        public string Name { get; }

        public object? Value { get; }

        public Argument(string name, object? value)
        {
            Name = name;
            Value = value;
        }
    }
}
