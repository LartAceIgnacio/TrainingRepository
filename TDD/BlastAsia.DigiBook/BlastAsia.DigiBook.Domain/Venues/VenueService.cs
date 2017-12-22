using System;
using BlastAsia.DigiBook.Domain.Models.Venues;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueService : IVenueService
    {
        private IVenueRepository venueRepository;
        private readonly int venueNameMaximumLength = 50;
        private readonly int venueDescriptionMaximumLength = 100;
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
            if (venue.VenueName.Length > venueNameMaximumLength)
            {
                throw new MaximumLengthException("At least 50 characters");
            }
            if (venue.Description.Length > venueDescriptionMaximumLength)
            {
                throw new MaximumLengthException("At least 100 characters");
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