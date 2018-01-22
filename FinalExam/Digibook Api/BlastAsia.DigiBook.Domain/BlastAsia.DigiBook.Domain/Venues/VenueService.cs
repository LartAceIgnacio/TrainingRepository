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

        public Venue Save(Guid venueId, Venue venue)
        {
            Venue result = null;

            if(string.IsNullOrEmpty(venue.VenueName)) {
                throw new VenueNameRequiredException("Venue Name is Required!!!");
            }

            if(venue.VenueName.Length > 50) {
                throw new VenueNameMaxLengthException("Venue Name's Length should have 50 or less characters!!!");
            }

            if (venue.Description.Length > 100) {
                throw new VenueDescriptionMaxLengthException("Venue Description's Length should have 100 or less characters!!!");
            }

            var found = this.venueRepository.Retrieve(venueId);

            if (found == null)
                result = this.venueRepository.Create(venue);
            else
                result = this.venueRepository.Update(venueId, venue);

            return venue;
        }
    }
}