using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FuzzDotNet.Core.Utilities;

namespace FuzzDotNet.Core.Generators
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// It would be cool if this class could be generic
    /// </remarks>
    public class ChoiceGenerator : Generator
    {
        private readonly Type _elementType;
        private readonly IList<object?> _items;

        public ChoiceGenerator(Type elementType, params object?[] items)
        {
            _items = items;
            _elementType = elementType;
        }

        public ChoiceGenerator(Type elementType, IEnumerable<object?> items)
        {
            _items = items.ToImmutableList();
            _elementType = elementType;
        }

        public ChoiceGenerator(Type elementType, IEnumerable<int> items)
        {
            _items = items.OfType<object?>().ToImmutableList();
            _elementType = elementType;
        }

        public override object? Generate(Type type, FuzzRandom random)
        {
            return random.Choice(_items);
        }

        public override bool CanGenerate(Type parameterType)
        {
            _elementType.IsCovariantSubtypeOf(parameterType);
        }
    }
}
