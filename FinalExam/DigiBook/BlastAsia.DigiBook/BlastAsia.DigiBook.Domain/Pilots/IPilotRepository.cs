using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Pilots;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public interface IPilotRepository 
        : IRepository<Pilot>
    {
        Pagination<Pilot> Retrieve(int pageNo, int numRec, string filterValue);
    }
}