using System;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Venues;

namespace BlastAsia.DigiBook.Domain
{
    public class VenueService : IVenueService
    {
        private IVenueRepository venueRepository;
        private int nameLimit = 51;
        private int descriptionLimit = 101;

        public VenueService(IVenueRepository venueRepository)
        {
            this.venueRepository = venueRepository;
        }

        public Venue Save(Guid id, Venue venue)
        {
            if(string.IsNullOrEmpty(venue.venueName))
            {
                throw new VenueNameRequiredException();
            }
            if(venue.venueName.Length >= nameLimit)
            {
                throw new VenueNameInvalidLengthException();
            }
            if(venue.venueDescription.Length >= descriptionLimit)
            {
                throw new VenueDescriptionInvalidLengthException();
            }
            Venue result = null;
            var found = venueRepository.Retrieve(id);
            if(found != null)
            {
                result = venueRepository.Update(venue.venueId, venue);
            }
            else
            {
                result = venueRepository.Create(venue);
            }
            return result;
        }
    }
}