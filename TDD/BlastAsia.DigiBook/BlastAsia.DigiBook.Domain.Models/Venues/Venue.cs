using System;

namespace BlastAsia.DigiBook.Domain.Models.Venues
{
    public class Venue
    {
        public Guid VenueId { get; set; }
        public string VenueName { get; set; }
        public string Description { get; set; }
    }
}