using System;
using BlastAsia.DigiBook.Domain.Models.Venues;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueService : IVenueService
    {
        private IVenueRepository venueRepository;
        private readonly int nameLength = 50;
        private readonly int descriptionLength = 100;

        public VenueService(IVenueRepository venueRepository)
        {
            this.venueRepository = venueRepository;
        }

        public Venue Save(Guid venueId, Venue venue)
        {
            if (string.IsNullOrEmpty(venue.VenueName))
            {
                throw new VenueNameRequiredException("Venue Name is required");
            }
            else
            {
                if (venue.VenueName.Length > nameLength)
                {
                    throw new VenueNameLengthRequiredException("Venue Name must not over in 50 characters");
                }
            }

            if (venue.Description.Length > descriptionLength)
            {
                throw new DescriptionLengthRequiredException("Description must not over in 100 characters");
            }

            

            var found = venueRepository.Retrieve(venueId);

            Venue result = null;

            if(found == null)
            {
                result = venueRepository.Create(venue);
            }
            else
            {
                result = venueRepository.Update(venue.VenueId,venue);
            }

            return result;
        }
    }
}