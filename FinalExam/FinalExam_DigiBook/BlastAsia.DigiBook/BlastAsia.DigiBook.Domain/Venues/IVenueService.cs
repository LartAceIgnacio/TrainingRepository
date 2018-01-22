using BlastAsia.DigiBook.Domain.Models.Venues;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public interface IVenueService
    {
        Venue Save(Guid id, Venue venue);
    }
}
