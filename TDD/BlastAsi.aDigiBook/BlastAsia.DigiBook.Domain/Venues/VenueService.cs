using BlastAsia.DigiBook.Domain.Models.Venues;
using System;
using System.Collections.Generic;
using System.Text;

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

            Venue result = null;
            var found = venueRepository.Retrieve(id);
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
