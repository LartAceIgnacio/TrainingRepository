using System;
using System.Text.RegularExpressions;
using BlastAsia.DigiBook.Domain.Models.Inventories;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository inventoryRepository;
        private readonly int productCodeRequiredLength = 8;
        private readonly int productNameMaxLength = 60;
        private readonly int productDescriptionMaxLength = 250;
        private readonly int binRequiredLength = 5;
        private readonly string binFormat = @"0[1-5][Bb][1-9][A-Za-z]{1}$";
        public InventoryService(IInventoryRepository inventoryRepository)
        {
            this.inventoryRepository = inventoryRepository;
        }

        public Inventory Save(Guid id, Inventory inventory)
        {
            if (inventory.ProductCode.Length != productCodeRequiredLength)
            {
                throw new ProductCodeInvalidException("Product code invalid");
            }
            if (String.IsNullOrEmpty(inventory.ProductName))
            {
                throw new ProductNameRequiredException("Product name required");
            }
            if (inventory.ProductName.Length > productNameMaxLength)
            {
                throw new ProductNameTooLongException("Product name too long");
            }
            if (String.IsNullOrEmpty(inventory.ProductDescription))
            {
                throw new ProductDescriptionRequiredException("Product description required");
            }
            if (inventory.ProductDescription.Length > productDescriptionMaxLength)
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
            if (inventory.Bin.Length != binRequiredLength)
            {
                throw new BinInvalidException("Bin invalid");
            }
            if (!Regex.IsMatch(inventory.Bin, binFormat, RegexOptions.IgnoreCase))
            {
                throw new BinInvalidException("Bin invalid format");
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