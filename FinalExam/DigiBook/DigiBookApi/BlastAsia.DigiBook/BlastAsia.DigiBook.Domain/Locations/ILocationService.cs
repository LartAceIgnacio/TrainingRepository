using System;
using BlastAsia.DigiBook.Domain.Models.Locations;

namespace BlastAsia.DigiBook.Domain.Locations
{
    public interface ILocationService
    {
        Location Save(Guid id, Location location);
    }
}