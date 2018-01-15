using BlastAsia.DigiBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.Api.Utils
{
    public static class VenueExtensions
    {
        public static Venue ApplyChanges(this Venue venue,
            Venue from)
        {
            venue.VenueId = from.VenueId;
            venue.VenueName = from.VenueName;
            venue.VenueDescription = from.VenueDescription;

            return venue;
        }
    }
}
