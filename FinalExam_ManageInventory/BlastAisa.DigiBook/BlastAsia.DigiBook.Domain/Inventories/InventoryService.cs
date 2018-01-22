using System;
using BlastAsia.DigiBook.Domain.Inventories;
using BlastAsia.DigiBook.Domain.Models.Inventories;

namespace BlastAsia.DigiBook.Domain.Test.Inventories.Inventories
{
    public class InventoryService : IInventoryService
    {
        private IInventoryRepository inventoryRepository;
        private readonly int prodCodeLength = 8;
        private readonly int prodNameLength = 60;
        private readonly int prodDescriptionLength = 250;
        private readonly int binLength = 5;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            this.inventoryRepository = inventoryRepository;
        }

        public Inventory Save(Guid id, Inventory inventory)
        {
            if(string.IsNullOrEmpty(inventory.ProductCode))
            {
                throw new ProductCodeRequiredException();
            }
            if(inventory.ProductCode.Length != prodCodeLength)
            {
                throw new ProductCodeRequiresEightCharException();
            }
            if(string.IsNullOrEmpty(inventory.ProductName))
            {
                throw new ProductNameRequiredException();
            }
            if(inventory.ProductName.Length > prodNameLength)
            {
                throw new ProdNameMaxLengthSixtyException();
            }
            if(string.IsNullOrEmpty(inventory.ProductDescription))
            {
                throw new ProdDescriptionRequiredException();
            }
            if(inventory.ProductDescription.Length > prodDescriptionLength)
            {
                throw new ProdDescriptionMaxLengthRequiredException();
            }
            if(inventory.QonHand < 0)
            {
                throw new QonHandRequiresPostiveInputException();
            }
            if(inventory.QonReserved < 0)
            {
                throw new QonReservedRequiredPositiveInputException();
            }
            if(inventory.QonOrdered < 0)
            {
                throw new QonOrderedRequiredPositiveInputException();
            }
            if(string.IsNullOrEmpty(inventory.Bin))
            {
                throw new BinRequiredException();
            }
            if(inventory.Bin.Length != binLength)
            {
                throw new BinRequiresFiveCharException();
            }
                
            Inventory result = null;
            result = inventoryRepository.Create(inventory);
            return result;
        }
    }
}