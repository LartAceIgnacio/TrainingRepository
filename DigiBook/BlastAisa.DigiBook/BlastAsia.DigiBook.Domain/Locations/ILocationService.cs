using BlastAsia.DigiBook.Domain.Models.Locations;
using System;

namespace BlastAsia.DigiBook.Domain.Locations
{
    public interface ILocationService
    {
        Location Save(Guid id, Location location);
    }
}