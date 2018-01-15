using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Venues;
using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public interface IVenueRepository
        : IRepository<Venue>
    {
        PaginationClass<Venue> Retrieve(int pageNo, int numRec, string filterValue);
    }
}