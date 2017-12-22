using BlastAsia.DigiBook.Domain.Models.Venues;
using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueService : IVenueService
    {
        private IVenueRepository venueRepository;
        private readonly int VenueNameMaximumLength = 50;
        private readonly int DescriptionMaximumLength = 100;

        public VenueService(IVenueRepository venueRepository)
        {
            this.venueRepository = venueRepository;
        }
        public Venue Save(Guid id,Venue venue)
        {
            if (string.IsNullOrEmpty(venue.VenueName))
            {
                throw new VenueNameRequiredException("VenueName is required!");
            }
            if(venue.VenueName.Length > VenueNameMaximumLength)
            {
                throw new LessThanMaximumLengthRequiredException("VenueName must be lessthan 50 Characters");
            }
            if (venue.Description.Length > DescriptionMaximumLength)
            {
                throw new LessThanMaximumLengthRequiredException("Description must be lessthan 100 Characters");
            }

            Venue result = null;

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