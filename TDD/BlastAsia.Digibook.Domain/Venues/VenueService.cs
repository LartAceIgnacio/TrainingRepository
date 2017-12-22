using BlastAsia.Digibook.Domain.Models.Venues;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Domain.Venues
{
    public class VenueService:IVenueService
    {
        private IVenueRepository venueRepository;
        private IVenueService venueService;

        public VenueService(IVenueRepository venueRepository, IVenueService venueService)
        {
            this.venueRepository = venueRepository;
            this.venueService = venueService;
        }

        public Venue Save(Venue venue)
        {
            if(venue.VenueName.Length == 0)
            {
                throw new InvalidStringLenghtException("Venue name is required");
            }

            if (venue.VenueName.Length > 50)
            {
                throw new InvalidStringLenghtException("Venue name must be less than 50 characters");
            }

            if(venue.Description.Length > 100)
            {
                throw new InvalidStringLenghtException("Description must be less than 100 characters");
            }

            var result = venueRepository.Create(venue);
            return result;
        }
    }
}
