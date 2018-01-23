using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Models.Pilots
{
    public class SearchResult
    {
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastNmae { get; set; }
        public string PilotCOde { get; set; }
        public DateTime DateActivated { get; set; }
    }
}
