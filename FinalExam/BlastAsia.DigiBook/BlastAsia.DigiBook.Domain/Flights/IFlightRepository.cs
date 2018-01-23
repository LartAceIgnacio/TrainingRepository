using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Flights;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public interface IFlightRepository : IRepository<Flight>
    {
        PaginationResult<Flight> Retrieve(int page, int record, string filter);
    }
}