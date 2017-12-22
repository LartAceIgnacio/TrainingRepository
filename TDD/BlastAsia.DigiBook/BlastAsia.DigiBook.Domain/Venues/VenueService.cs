using BlastAsia.DigiBook.Domain.Models.Venues;
using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueService : IVenueService
    {
        private readonly IVenueRepository venueRepository;
        private readonly int nameMaxLength = 50;
        private readonly int descriptionMaxLength = 100;

        public VenueService(IVenueRepository venueRepository)
        {
            this.venueRepository = venueRepository;
        }

        public Venue Save(Guid venueId, Venue venue)
        {
            if (string.IsNullOrEmpty(venue.VenueName))
            {
                throw new VenueNameRequiredException("Venue name is required");
            }

            if (venue.VenueName.Length > nameMaxLength)
            {
                throw new VenueNameInvalidException("Venue name too long");
            }

            if (venue.Description.Length > descriptionMaxLength)
            {
                throw new DescriptionTooLongException("Description too long");
            }

            Venue result = null;
            var found = venueRepository.Retrieve(venueId);
            if(found == null)
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