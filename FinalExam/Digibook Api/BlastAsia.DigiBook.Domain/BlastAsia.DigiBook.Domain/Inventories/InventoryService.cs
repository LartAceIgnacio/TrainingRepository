using System;
using System.Linq;
using System.Text.RegularExpressions;
using BlastAsia.DigiBook.Domain.Models.Inventories;

namespace BlastAsia.DigiBook.Domain.Inventories
{
    public class InventoryService
    {
        private IInventoryRepository inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            this.inventoryRepository = inventoryRepository;
        }

        public Inventory Save(Guid productId, Inventory inventory)
        {
            if(string.IsNullOrEmpty(inventory.ProductCode)) {
                throw new ProductsRequiredException("Product Code is Required");
            }

            if(inventory.ProductCode.Length != 8) {
                throw new InvalidProductCodeSizeException("Product Code Length should be equal to 8");
            }

            if (string.IsNullOrEmpty(inventory.ProductName)) {
                throw new ProductsRequiredException("Product Name is Required");
            }

            if (inventory.ProductName.Length > 60) {
                throw new ProductNameMaxLengthException("Product Name should have 60 or less characters!!!");
            }

            if (string.IsNullOrEmpty(inventory.ProductDescription)) {
                throw new ProductsRequiredException("Product Description is Required");
            }

            if (inventory.ProductDescription.Length > 250) {
                throw new ProductDescriptionMaxLengthException("Product Description should have 250 or less characters!!!");
            }

            if (string.IsNullOrEmpty(inventory.Bin)) {
                throw new ProductsRequiredException("Bin is Required");
            }

            if (!Regex.IsMatch(inventory.Bin, "0[1-5][Bb][1-9][A-Za-z]{1}$")) {
                throw new InvalidBinFormatException("Invalid Bin Format");
            }

            var resultList = inventoryRepository.Retrieve();

            if(resultList.Count() != 0) {
                var found = resultList.Where(x => x.ProductCode.Equals(inventory.ProductCode)).FirstOrDefault();

                if(found != null) {
                    throw new ProductCodeShouldUniqueException("Product Code Should Be Unique");
                }
            }

            Inventory result = null;

            if (productId == null || productId == Guid.Empty) {
                result = inventoryRepository.Create(inventory);
            }
            else {
                result = inventoryRepository.Update(productId, inventory);
            }

            return result;
        }
    }
}