using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues.VenueExceptions;

namespace BlastAsia.DigiBook.Domain.Venues.Service
{
    public class VenueService : IVenueService
    {
        private IVenueRepository repo;
        public VenueService(IVenueRepository repo)
        {
            this.repo = repo;
        }

        public Venue Save(Guid guid, Venue venue)
        {
            if (string.IsNullOrEmpty(venue.VenueName)) throw new VenueNameRequiredException("Venue name is required.");
            if (venue.VenueName.Length > 50) throw new VenueFieldLimitExceedException("Venue name limit to 50 characters.");
            if (venue.Description.Length > 100) throw new VenueFieldLimitExceedException("Venue description limit to 100 characters.");

            Venue result = null;
            var found = repo.Retrieve(guid);

            if (found == null)
            {
                result = repo.Create(venue);
            }
            else
            {
                result = repo.Update(guid, venue);
            }
            
            return result;
            
        }
    }
}
