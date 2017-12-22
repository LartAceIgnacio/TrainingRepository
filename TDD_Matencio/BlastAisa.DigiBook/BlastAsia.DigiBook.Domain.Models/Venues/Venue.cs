using System;

namespace BlastAsia.DigiBook.Domain.Models
{
    public class Venue
    {
        public Guid VenueId { get; set; }
        public string VenueName { get; set; }
        public string VenueDescription { get; set; }
    }
}