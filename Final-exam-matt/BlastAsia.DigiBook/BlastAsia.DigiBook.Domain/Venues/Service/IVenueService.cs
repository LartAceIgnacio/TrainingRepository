using BlastAsia.DigiBook.Domain.Models.Venues;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Venues.Service
{
    public interface IVenueService
    {
        Venue Save(Guid guid, Venue venue);
    }
}
