using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Models
{
    public class PaginationResult<TEntity>
        where TEntity : class
    {
        public List<TEntity> Results { get; set; }
        public int PageNo { get; set; }
        public int RecordPage { get; set; }
        public int TotalRecords { get; set; }
    }
}
