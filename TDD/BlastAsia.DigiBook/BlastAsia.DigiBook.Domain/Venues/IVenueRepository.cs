using System;
using BlastAsia.DigiBook.Domain.Models.Records;
using BlastAsia.DigiBook.Domain.Models.Venues;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public interface IVenueRepository
    : IRepository<Venue>
    {
        Record<Venue> Pagination(int page, int record, string filter);
    }
}