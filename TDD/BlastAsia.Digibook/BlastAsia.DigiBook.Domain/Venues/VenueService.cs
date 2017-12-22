using System;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues.Exceptions;
using BlastAsia.DigiBook.Domain.Models.Venues.Exceptions;

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
            if (string.IsNullOrEmpty(venue.VenueName))
            {
                throw new InvalidVenueNameException("Venue Name is required");
            }

            if (venue.VenueName.Length > 50)
            {
                throw new InvalidVenueNameException("Venue name must not exceed 50 characters");
            }

            if ((!string.IsNullOrEmpty(venue.Description)) && (venue.Description.Length > 100))
            {
                throw new InvalidDescriptionLengthException("Description must not exceed 100 characters");
            }
            Venue result = null;
            var existing = this.repo.Retrieve(id);

            if (existing == null)
            {
                result = this.repo.Create(venue);
            }
            else
            {
                result = this.repo.Update(id, venue);
            }

            return result;
        }
    }
}