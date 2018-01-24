using BlastAsia.DigiBook.Domain.Models.Inventories;
using BlastAsia.DigiBook.Domain.Models.Pagination;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        Pagination<Inventory> Retrieve(int pageNumber, int recordNumber, string query);

        Inventory CheckCode(string code);
    }
}