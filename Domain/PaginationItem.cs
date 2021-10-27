using System.Collections.Generic;

namespace Domain
{
    public class PaginationItem<T>
    {
        public List<T> Items { get; set; }
        public string ContinuationToken { get; set; }
    }
}