using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FuzzDotNet.Core.Generators
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// It would be cool if this class could be generic
    /// </remarks>
    public class CollectionGenerator : Generator
    {
        private readonly IList<object?> _items;

        public CollectionGenerator(params object?[] items)
        {
            _items = items;
        }

        public CollectionGenerator(IEnumerable<object?> items)
        {
            _items = items.ToImmutableList();
        }

        public CollectionGenerator(IEnumerable<int> items)
        {
            _items = items.OfType<object?>().ToImmutableList();
        }

        public override object? Generate(Type type, FuzzRandom random)
        {
            return random.Choice(_items);
        }
    }
}
