using BlastAsia.DigiBook.Domain.Models.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class InventoryExtensions
    {
        public static Inventory ApplyChanges(this Inventory inventory,
            Inventory from)
        {
            inventory.ProductCode = from.ProductCode;
            inventory.ProductName = from.ProductName;
            inventory.ProductDescription = from.ProductDescription;
            inventory.QOH = from.QOH;
            inventory.QOR = from.QOR;
            inventory.QOO = from.QOO;
            inventory.DateModified = from.DateModified;
            inventory.Bin = from.Bin;

            return inventory;
        }
    }
}
