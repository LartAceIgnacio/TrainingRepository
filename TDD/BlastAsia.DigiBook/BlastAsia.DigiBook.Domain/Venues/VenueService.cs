using BlastAsia.DigiBook.Domain.Models.Venues;
using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueService : IVenueService
    {
        private int nameMaxLength = 50;
        private int descriptionMaxLength = 100;
        private IVenueRepository venueRepository;

        public VenueService(IVenueRepository venueRepository)
        {
            this.venueRepository = venueRepository;
        }

        public Venue Save(Guid venueId, Venue venue)
        {
            if (string.IsNullOrEmpty(venue.VenueName))
            {
                throw new VenueNameRequired("Venue name is required");
            }

            if (venue.VenueName.Length > nameMaxLength)
            {
                throw new VenueNameInvalid("Venue name too long");
            }

            if (venue.Description.Length > descriptionMaxLength)
            {
                throw new DescriptionTooLong("Description too long");
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