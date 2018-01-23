using System;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Inventories;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        PaginationClass<Inventory> Retrieve(int pageNo, int numRec, string filterValue);
    }
}