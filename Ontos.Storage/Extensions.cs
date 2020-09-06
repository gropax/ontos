using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ontos.Storage
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<U> SelectValues<T, U>(this IEnumerable<T> ts, Func<T, U?> selector) where U : struct
        {
            return ts.Select(t => selector(t)).Where(u => u.HasValue).Select(u => u.Value);
        }
    }
}
