using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlastAsia.Digibook.Domain.Models.Venues
{
    public class Venue
    {
        public Guid VenueId { get; set; }

        [Required]
        public string VenueName { get; set; }

        public string Description { get; set; }
    }
}
