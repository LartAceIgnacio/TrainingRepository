using System;
using System.Collections.Generic;
using System.Linq;

namespace BlastAsia.DigiBook.Domain.Models.Records
{
    public class Record<TEntity> 
        where TEntity : class
    {
        public List<TEntity> Result { get; set; }
        public int PageNo { get; set; }
        public int TotalRecord { get; set; }
        public int RecordPage { get; set; }
    }
}
