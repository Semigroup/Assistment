using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assistment.Extensions
{
    public static class CollectionExtender
    {
        public static int Remove<T>(this List<T> list, IEnumerable<T> toRemove)
        {
            return list.RemoveAll(x => toRemove.Contains(x));
        }
    }
}
