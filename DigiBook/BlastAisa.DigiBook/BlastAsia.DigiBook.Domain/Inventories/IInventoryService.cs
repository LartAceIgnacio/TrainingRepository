using BlastAsia.DigiBook.Domain.Models.Inventories;
using System;

namespace BlastAsia.DigiBook.Domain
{
    public interface IInventoryService
    {
        Inventory Save(Guid id, Inventory inventory);
    }
}