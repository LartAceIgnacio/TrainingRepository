using System;
using BlastAsia.DigiBook.Domain.Models.Locations;

namespace BlastAsia.DigiBook.Domain.Locations
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository locationRepository;
        public LocationService(ILocationRepository locationRepository)
        {
            this.locationRepository = locationRepository;
        }

        public Location Save(Guid id, Location location)
        {
            if(string.IsNullOrEmpty(location.LocationName))
            {
                throw new NullLocationNameException();
            }

            if(string.IsNullOrEmpty(location.LocationMark))
            {
                throw new NullLocationMarkException();
            }

            Location result = null;
            var found = locationRepository.Retrieve(id);

            if (found != null)
            {
                result = locationRepository.Update(location.LocationId, location);
            }
            else
            {
                result = locationRepository.Create(location);
            }
            return result;
        }
    }
}