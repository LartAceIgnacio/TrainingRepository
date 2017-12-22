using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues.Exceptions;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueService : IVenueService
    {
        private VenueRepository venueRepository;
        public VenueService(VenueRepository venueRepository)
        {
            this.venueRepository = venueRepository;
        }
        readonly int VenueName = 50;
        readonly int Description = 100;

        public Venue Save(Guid id, Venue venue)
        {
            if(venue.VenueName.Length > 50)
            {
                throw new VenueNameException("Less than 50");
            }
            if(venue.Description.Length > 100)
            {
                throw new DescriptionException("Less than 100");
            }
            Venue result = null;
            var VenueId = venueRepository.Retrieve(venue.VenueID);

            if(VenueId == null)
            {
                result = venueRepository.Create(venue);
            }
            else
            {
                result = venueRepository.Update(venue.VenueID, venue);
            }
            return result;
        }
    }
}
