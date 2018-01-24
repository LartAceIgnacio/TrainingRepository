using System;

namespace BlastAsia.DigiBook.Domain.Models.Locations
{
    public class Location
    {
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationMark { get; set; }
    }
}