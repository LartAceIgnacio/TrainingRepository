using System;
using BlastAsia.DigiBook.Domain.Models.Locations;

namespace BlastAsia.DigiBook.Domain.Locations
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository locationRepository;
        private readonly int descriptionMaxLength = 100;

        public LocationService(ILocationRepository locationRepository)
        {
            this.locationRepository = locationRepository;
        }

        public Location Save(Guid id, Location location)
        {
            if (String.IsNullOrEmpty(location.LocationName))
            {
                throw new LocationNameRequiredException("Location name is required");
            }

            if (location.Description.Length > descriptionMaxLength)
            {
                throw new DescriptionTooLongException("Description is too long");
            }

            Location result = null;
            var found = locationRepository.Retrieve(id);
            if(found == null)
            {
                result = locationRepository.Create(location);
            }
            else
            {
                result = locationRepository.Update(id, location);
            }
            return result;
        }
    }
}