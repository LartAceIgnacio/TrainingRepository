using BlastAsia.DigiBook.Domain.Models;
using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public interface IVenueService
    {
        Venue Save(Guid id, Venue venue);
    }
}