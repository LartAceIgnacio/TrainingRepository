using BlastAsia.DigiBook.Domain.Models.Venues;
using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueService
    {
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