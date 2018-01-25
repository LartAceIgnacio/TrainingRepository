using BlastAsia.DigiBook.Domain.Models.Venues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class VenueExtension
    {
        public static Venue ApplyChanges(
            this Venue venue,
            Venue form)
        {
            venue.VenueName = form.VenueName;
            venue.Description = form.Description;

            return venue;
        }
    }
}
