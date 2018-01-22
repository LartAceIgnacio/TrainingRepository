using BlastAsia.DigiBook.Domain.Models.Venues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class VenueExtension
    {
        public static Venue ApplyNewChanges(this Venue oldVenue, Venue newVenue)
        {
            oldVenue.VenueName = newVenue.VenueName;
            oldVenue.Description = newVenue.Description;
            return oldVenue;
        }
    }
}
