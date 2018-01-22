using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Models.Paginations
{
    public class Pagination<TEntity> where TEntity : class
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int RecordNumber { get; set; }
        public IEnumerable<TEntity> Results { get; set; }
    }
}
