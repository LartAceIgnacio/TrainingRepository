using System;
using System.Text.RegularExpressions;
using BlastAsia.DigiBook.Domain.Exceptions;
using BlastAsia.DigiBook.Domain.Models.Inventories;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public class InventoryService : IInventoryService
    {
        private readonly string binFormat = @"0[1-5][Bb][1-9][A-Za-z]{1}$";
        private IInventoryRepository inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            this.inventoryRepository = inventoryRepository;
        }

        public Inventory Save(Guid productId, Inventory inventory)
        {
            if (string.IsNullOrWhiteSpace(inventory.ProductCode))
            {
                throw new ProductCodeRequiredException();
            }
            if(inventory.ProductCode.Length != 8)
            {
                throw new ProductCodeRequiredLengthException();
            }
            if (string.IsNullOrWhiteSpace(inventory.ProductName))
            {
                throw new ProductNameRequiredException();
            }
            if (inventory.ProductName.Length > 60){
                throw new ProductNameExceedsMaximumLengthException();
            }
            if (string.IsNullOrWhiteSpace(inventory.ProductDescription))
            {
                throw new ProductDescriptionRequiredException();
            }
            if(inventory.ProductDescription.Length > 250)
            {
                throw new ProductDescriptionMaximumLengthException();
            }
            if (inventory.QuantityOnHand < 0)
            {
                throw new InvalidQuantityOnHandException();
            }

            if(inventory.QuantityReserved < 0)
            {
                throw new InvalidQuantityReservedException();
            }
            if(inventory.QuantityOrdered < 0)
            {
                throw new InvalidQuantityOrderException();
            }
            if (string.IsNullOrWhiteSpace(inventory.Bin))
            {
                throw new BinRequiredException();
            }
            if(inventory.Bin.Length > 5)
            {
                throw new BinExceedsMaximumLengthException();
            }
            if (!Regex.IsMatch(inventory.Bin, binFormat, RegexOptions.IgnoreCase))
            {
                throw new InvalidBinFormatException();
            }

            return inventoryRepository.Create(inventory);
        }
    }
}