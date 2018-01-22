using BlastAsia.DigiBook.Domain.Models.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class LocationExtensions
    {
        public static Location ApplyChanges(this Location location,
           Location from)
        {
            location.LocationName = from.LocationName;
            location.Description = from.Description;

            return location;
        }
    }
}
