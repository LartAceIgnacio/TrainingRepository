using System;

namespace BlastAsia.DigiBook.Domain.Models
{
    public class Venue
    {
        public Guid venueId { get; set; }
        public string venueName { get; set; }
        public string venueDescription { get; set; }
    }
}