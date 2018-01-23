using BlastAsia.DigiBook.Domain.Models.Venues;
using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public interface IVenueService
    {
        Venue Save(Guid id, Venue venue);
    }
}