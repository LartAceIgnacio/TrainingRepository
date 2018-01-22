using BlastAsia.DigiBook.Domain.Models.Venues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class VenueExtensions
    {
        public static Venue ApplyChanges(this Venue venue, Venue from)
        {
            venue.VenueName = from.VenueName;
            venue.Description = from.Description;

            return venue;
        }
    }
}
