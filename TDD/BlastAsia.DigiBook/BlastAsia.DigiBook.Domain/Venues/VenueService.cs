using System;
using BlastAsia.DigiBook.Domain.Models.Venues;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueService
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
                throw new RequiredException();
            }
            if (venue.VenueName.Length > 50)
            {
                throw new MaximumVenueNameException();
            }
            if (venue.Description.Length > 100)
            {
                throw new MaximumDescriptionLengthException();
            }

            var foundVenue = venueRepository.Retrieve(id);
            Venue result = null;

            if (foundVenue == null)
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