using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Models.Pilots
{
    public class PilotSearchResult
    {
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastNmae { get; set; }
        public string PilotCode { get; set; }
        public DateTime? DateActivated { get; set; }
    }
}
