using System;
using BlastAsia.DigiBook.Domain.Models.Inventories;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public interface IInventoryService
    {
        Inventory Save(Guid id, Inventory inventory);
    }
}