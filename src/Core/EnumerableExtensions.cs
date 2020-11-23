using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzDotNet.Core
{
    public static class EnumerableExtensions
    {
        public static bool UniqueBy<TElement, TProjection>(this IEnumerable<TElement> enumerable, Func<TElement, TProjection> project)
        {
            var originalCount = enumerable.Count();
            var uniqueProjected = enumerable.Select(project).Distinct();

            return uniqueProjected.Count() == originalCount;
        }
    }
}
