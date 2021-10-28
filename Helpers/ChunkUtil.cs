using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
    public static class ChunkUtil
    {
        public static IEnumerable<IEnumerable<T>> ChunkList<T>(IEnumerable<T> lists, int chunkSize)
        {
            return lists
                .Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value));
        }
    }
}