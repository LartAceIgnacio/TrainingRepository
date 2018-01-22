using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Models.Paginations
{
    public class  Pagination <T> where T : class
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int RecordNumber { get; set; }
        public IEnumerable<T> Result { get; set; }
    }
}
