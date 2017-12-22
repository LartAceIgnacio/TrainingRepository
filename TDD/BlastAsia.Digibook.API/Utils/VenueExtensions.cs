using BlastAsia.Digibook.Domain.Models.Venues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.Digibook.API.Utils
{
    public static class VenueExtensions
    {
        public static Venue ApplyVenueChanges(this Venue destinationVenue, Venue sourceVenue)
        {
            destinationVenue.VenueName = sourceVenue.VenueName;
            destinationVenue.Description = sourceVenue.Description;

            return destinationVenue;
        }
    }
}
