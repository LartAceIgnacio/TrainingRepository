using System;
using BlastAsia.DigiBook.Domain.Models.Inventories;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public interface IInventoryService
    {
        Inventory Save(Guid productId, Inventory inventory);
    }
}