using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Models;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public interface IVenueRepository
        : IRepository<Venue>
    {
        Pagination<Venue> Retrieve(int pageNo, int numRec, string filterValue);
    }
}