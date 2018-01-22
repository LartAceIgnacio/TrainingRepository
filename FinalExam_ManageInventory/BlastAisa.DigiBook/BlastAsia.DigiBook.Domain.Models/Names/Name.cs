using System;

namespace BlastAsia.DigiBook.Domain.Models.Names
{
    public class Name
    {
        public Guid NameId { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
    }
}