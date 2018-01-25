using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Luigis;

namespace BlastAsia.DigiBook.Domain.Luigis
{
    public interface ILuigiRepository : IRepository<Luigi>
    {
        Pagination<Luigi> Retrieve(int pageNo, int numRec, string filterValue);
    }
}