using BlastAsia.DigiBook.Domain.Models.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.Api.Utils
{
    public static class InventoryExtensions
    {
        public static Inventory ApplyChanges(this Inventory inventory, Inventory from)
        {
            inventory.ProductName = from.ProductName;
            inventory.ProductDescription = from.ProductDescription;
            inventory.QonHand = from.QonHand;
            inventory.QonOrdered = from.QonOrdered;
            inventory.QonReserved = from.QonOrdered;
            inventory.Bin = from.Bin;
            inventory.DateModified = DateTime.Now;

            return inventory;
        }
    }
}
