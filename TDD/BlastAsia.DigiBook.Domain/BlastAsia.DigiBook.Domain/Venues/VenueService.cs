using System;
using BlastAsia.DigiBook.Domain.Models.Venues;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueService : IVenueService
    {
        private IVenueRepository venueRepository;

        public VenueService(IVenueRepository venueRepository)
        {
            this.venueRepository = venueRepository;
        }

        public Venue Save(Guid id, Venue venue)
        {
            if (string.IsNullOrEmpty(venue.VenueName))
            {
                throw new VenueNameRequiredException();
            }
            if(venue.VenueName.Length > 50)
            {
                throw new VenueNameRequiredException();
            }
            if (venue.Description.Length > 100)
            {
                throw new DescriptionException();
            }
                Venue result = null;
            var found = venueRepository.Retrieve(id);

            if(found == null)
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