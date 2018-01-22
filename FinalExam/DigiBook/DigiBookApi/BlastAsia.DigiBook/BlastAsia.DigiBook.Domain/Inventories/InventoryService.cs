using System;
using BlastAsia.DigiBook.Domain.Models.Inventories;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository inventoryRepository;
        private readonly int ProductCodeRequiredLength = 8;
        private readonly int ProductNameMaxLength = 60;
        private readonly int ProductDescriptionMaxLength = 250;
        private readonly int BinRequiredLength = 5;
        public InventoryService(IInventoryRepository inventoryRepository)
        {
            this.inventoryRepository = inventoryRepository;
        }

        public Inventory Save(Guid id, Inventory inventory)
        {
            if (inventory.ProductCode.Length != ProductCodeRequiredLength)
            {
                throw new ProductCodeInvalidException("Product code invalid");
            }
            if (String.IsNullOrEmpty(inventory.ProductName))
            {
                throw new ProductNameRequiredException("Product name required");
            }
            if (inventory.ProductName.Length > ProductNameMaxLength)
            {
                throw new ProductNameTooLongException("Product name too long");
            }
            if (String.IsNullOrEmpty(inventory.ProductDescription))
            {
                throw new ProductDescriptionRequiredException("Product description required");
            }
            if (inventory.ProductDescription.Length > ProductDescriptionMaxLength)
            {
                throw new ProductDescriptionTooLongException("Product description too long");
            }
            if (inventory.QOH < 0)
            {
                throw new NegativeNumberInvalidException("QOH cannot be negative number");
            }
            if (inventory.QOR < 0)
            {
                throw new NegativeNumberInvalidException("QOR cannot be negative number");
            }
            if (inventory.QOO < 0)
            {
                throw new NegativeNumberInvalidException("QOO cannot be negative number");
            }
            if (inventory.Bin.Length != BinRequiredLength)
            {
                throw new BinInvalidException("Bin invalid");
            }

            Inventory result = null;
            var found = inventoryRepository.Retrieve(id);
            if (found == null)
            {
                result = inventoryRepository.Create(inventory);
            }
            else
            {
                result = inventoryRepository.Update(id, inventory);
            }

            return result;
        }
    }
}