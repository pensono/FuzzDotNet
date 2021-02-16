using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FuzzDotNet.Core.Generation
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// It would be cool if this class could be generic
    /// </remarks>
    public class ChoiceGenerator : Generator
    {
        private readonly IList<object?> _items;

        /// <remarks>
        /// Not sure what the right choice here is since the list of items is untyped
        /// It's unlikely this class would be used as a default generator anyways
        /// </remarks>
        protected virtual Type ItemType => typeof(object);

        public ChoiceGenerator(params object?[] items)
        {
            _items = items;
        }

        public ChoiceGenerator(IEnumerable<object?> items)
        {
            _items = items.ToImmutableList();
        }

        public ChoiceGenerator(IEnumerable<int> items)
        {
            _items = items.OfType<object?>().ToImmutableList();
        }

        public override bool CanGenerate(IFuzzProfile profile, Type type)
        {
            return ItemType.IsAssignableFrom(type);
        }

        public override object? Generate(IFuzzProfile profile, Type type, FuzzRandom random)
        {
            return random.Choice(_items);
        }
    }
}
