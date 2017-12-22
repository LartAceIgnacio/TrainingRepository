using System;
using BlastAsia.DigiBook.Domain.Models.Venues;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueService : IVenueService
    {
        private IVenueRepository venueRepository;
        private readonly int venueNameMinimumLength = 50;
        public VenueService(IVenueRepository venueRepository)
        {
            this.venueRepository = venueRepository;
        }

        public Venue Save(Guid id, Venue venue)
        {
            if (string.IsNullOrEmpty(venue.VenueName))
            {
                throw new VenueNameRequiredException("Venue name is required.");
            }
            if (venue.VenueName.Length < venueNameMinimumLength)
            {
                throw new MinimumLengthRequiredException("At least 50 characters");
            }

            Venue result;
            var found = venueRepository.Retrieve(venue.VenueId);

            if (found == null)
            {
                result = venueRepository.Create(venue);
            }
            else
            {
                result = venueRepository.Update(venue.VenueId, venue);
            }

            return result;
        }
    }
}