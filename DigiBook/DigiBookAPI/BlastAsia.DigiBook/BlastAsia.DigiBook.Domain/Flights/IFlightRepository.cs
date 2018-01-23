using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Flights;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public interface IFlightRepository : IRepository<Flight>
    {
        Pagination<Flight> Retrieve(int pageNo, int numRec, string filterValue);
    }
}