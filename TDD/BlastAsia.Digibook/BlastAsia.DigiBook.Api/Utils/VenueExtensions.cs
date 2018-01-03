using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Models.Venues;


namespace BlastAsia.DigiBook.Api.Utils
{
    public static class VenueExtensions
    {
        public static Venue ApplyChages( this Venue venue , Venue from)
        {
            //venue.VenueId = Guid.Empty;
            venue.VenueName = from.VenueName;
            venue.Description = from.Description;
            return venue;

        }
    }
}
