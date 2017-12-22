using System;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues.Exceptions;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueService : IVenueService
    {
        private IVenueRepository repo;

        public VenueService(IVenueRepository repo)
        {
            this.repo = repo;
        }

        public Venue Save(Guid id, Venue venue)
        {
            if (string.IsNullOrEmpty(venue.VenueName)) {
                throw new InvalidVenueNameException("Venue Name is required");
            }

            Venue result = null;
            var existing = this.repo.Retrieve(id);

            if (existing == null) {
                result = this.repo.Create(venue);
            } else
            {
                result = this.repo.Update(id, venue);
            }

            return result;
        }
    }
}