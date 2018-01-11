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
        private IVenueRepository venueRepository;
        public VenueService(IVenueRepository venueRepository)
        {
            this.venueRepository = venueRepository;
        }
        readonly int VenueName = 50;
        readonly int Description = 100;

        public Venue Save(Guid id, Venue venue)
        {
            if(venue.VenueName.Length > VenueName)
            {
                throw new VenueNameException("Less than 50");
            }
            if(venue.Description.Length > Description)
            {
                throw new DescriptionException("Less than 100");
            }
            Venue result = null;
            var VenueId = venueRepository.Retrieve(venue.VenueId);

            if(VenueId == null)
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
