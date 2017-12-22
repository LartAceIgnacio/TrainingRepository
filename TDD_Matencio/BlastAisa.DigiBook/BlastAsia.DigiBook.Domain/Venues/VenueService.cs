using System;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Venues;

namespace BlastAsia.DigiBook.Domain
{
    public class VenueService : IVenueService
    {
        private readonly IVenueRepository venueRepository;
        private readonly int nameLimit = 50;
        private readonly int descriptionLimit = 100;

        public VenueService(IVenueRepository venueRepository)
        {
            this.venueRepository = venueRepository;
        }

        public Venue Save(Guid id, Venue venue)
        {
            if(string.IsNullOrEmpty(venue.VenueName))
            {
                throw new VenueNameRequiredException();
            }
            if(venue.VenueName.Length > nameLimit)
            {
                throw new VenueNameInvalidLengthException();
            }
            if(venue.VenueDescription.Length > descriptionLimit)
            {
                throw new VenueDescriptionInvalidLengthException();
            }
            Venue result = null;
            var found = venueRepository.Retrieve(id);
            if(found != null)
            {
                result = venueRepository.Update(venue.VenueId, venue);
            }
            else
            {
                result = venueRepository.Create(venue);
            }
            return result;
        }
    }
}