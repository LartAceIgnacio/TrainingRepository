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
                throw new MaximumLengthException("50 characters only");
            }
            if (venue.Description.Length > venueDescriptionMaximumLength)
            {
                throw new MaximumLengthException("100 characters only");
            }

            Venue result;
            var found = venueRepository.Retrieve(id);

            if (found == null)
            {
                result = venueRepository.Create(venue);
            }
            else
            {
                result = venueRepository.Update(id, venue);
            }

            return result;
        }
    }
}