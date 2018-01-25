using System;
using BlastAsia.DigiBook.Domain.Models.Venues;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public interface IVenueService
    {
        Venue Save(Guid id, Venue venue);
    }
}