using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VintageStoryModManager.Extensions
{
    public static class ListExtensions
    {
        public static List<T> InsertAtStart<T>(this IEnumerable<T> source, T item)
        {
            var result = new List<T> { item };
            result.AddRange(source);
            return result;
        }
    }
}
