using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlastAsia.DigiBook.Domain.Inventories;
using BlastAsia.DigiBook.Domain.Models.Inventories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Inventories
{
    [TestClass]
    public class InventoryServiceTest
    {
        private Mock<IInventoryRepository> mockInventoryRepository;
        private InventoryService sut;
        private Inventory inventory;
        private Guid nonExistingProductId = Guid.Empty;
        private Guid existingProductId = Guid.NewGuid();

        [TestInitialize]
        public void InitializeTest()
        {
            mockInventoryRepository = new Mock<IInventoryRepository>();
            sut = new InventoryService(mockInventoryRepository.Object);
            inventory = new Inventory
            {
                ProductCode = "00000000",
                ProductName = "product 1",
                ProductDescription = "product description",
                QOH = 100,
                QOR = 50,
                QOO = 20,
                DateCreated = new DateTime(),
                DateModified = new DateTime(),
                IsActive = true,
                Bin = "01B1A"
            };
        }

        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositoryCreate()
        {
            // Assert
            mockInventoryRepository
                .Setup(ir => ir.Retrieve(nonExistingProductId))
                .Returns<Inventory>(null);

            // Act
            var result = sut.Save(nonExistingProductId, inventory);

            // Assert
            mockInventoryRepository
                .Verify(ir => ir.Retrieve(nonExistingProductId), Times.Once);
            mockInventoryRepository
                .Verify(ir => ir.Create(inventory), Times.Once);
        }

        [TestMethod]
        public void Save_WithExistingProductId_ShouldCallRepositoryUpdate()
        {
            // Assert
            mockInventoryRepository
                .Setup(ir => ir.Retrieve(existingProductId))
                .Returns(inventory);

            // Act
            var result = sut.Save(existingProductId, inventory);

            // Assert
            mockInventoryRepository
                .Verify(ir => ir.Retrieve(existingProductId), Times.Once);
            mockInventoryRepository
                .Verify(ir => ir.Update(existingProductId, inventory), Times.Once);
        }

        [TestMethod]
        public void Save_WithExisitingProductCode_ThrowsProductCodeAlreadyExistException()
        {
            // Arrange
            var existingProductCode = inventory.ProductCode;
            
            mockInventoryRepository
                .Setup(ir => ir.CheckProductCode(existingProductCode))
                .Returns(inventory);

            // Assert
            Assert.ThrowsException<ProductCodeAlreadyExistException>(
                () => sut.Save(inventory.ProductId, inventory));

        }

        [TestMethod]
        public void Save_WithValidData_ReturnNewInventoryWithProductId()
        {
            // Arrange
            mockInventoryRepository
                .Setup(e => e.Create(inventory))
                .Callback(() => inventory.ProductId = Guid.NewGuid())
                .Returns(inventory);

            // Act
            sut.Save(inventory.ProductId, inventory);

            // Assert
            Assert.IsTrue(inventory.ProductId != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithProductCodeNotEqualToEight_ThrowsProductCodeInvalidException()
        {
            // Arrange
            inventory.ProductCode = "123456789";

            // Assert
            Assert.ThrowsException<ProductCodeInvalidException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithBlankProductName_ThrowsProductNameRequiredException()
        {
            // Arrange
            inventory.ProductName = "";

            // Assert
            Assert.ThrowsException<ProductNameRequiredException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithProductNameGreaterThanMaxLength_ThrowsProductNameTooLongException()
        {
            // Arrange
            inventory.ProductName = "1234567890123456789012345678901234567890123456789012345678901234567890";

            // Assert
            Assert.ThrowsException<ProductNameTooLongException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithBlankProductDescription_ThrowsProductDescriptionRequiredException()
        {
            // Arrange
            inventory.ProductDescription = "";

            // Assert
            Assert.ThrowsException<ProductDescriptionRequiredException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithProductDescriptionGreaterThanMaxLength_ThrowsProductDescriptionTooLongException()
        {
            // Arrange
            inventory.ProductDescription = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";

            // Assert
            Assert.ThrowsException<ProductDescriptionTooLongException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithNegativeQOH_ThrowsNegativeNumberInvalidException()
        {
            // Arrange
            inventory.QOH = -100;

            // Assert
            Assert.ThrowsException<NegativeNumberInvalidException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithNegativeQOR_ThrowsNegativeNumberInvalidException()
        {
            // Arrange
            inventory.QOR = -50;

            // Assert
            Assert.ThrowsException<NegativeNumberInvalidException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithNegativeQOO_ThrowsNegativeNumberInvalidException()
        {
            // Arrange
            inventory.QOR = -20;

            // Assert
            Assert.ThrowsException<NegativeNumberInvalidException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithBinNotEqualToFive_ThrowsBinInvalidException()
        {
            // Arrange
            inventory.Bin = "01B1AZ";

            // Assert
            Assert.ThrowsException<BinInvalidException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [DataTestMethod]
        [DataRow("A01B1")]
        [DataRow("B1A01")]
        [TestMethod]
        public void Save_WithBinIncorrectFormat_ThrowsBinInvalidException(string Bin)
        {
            // Arrange
            inventory.Bin = Bin;

            // Assert
            Assert.ThrowsException<BinInvalidException>(
                () => sut.Save(inventory.ProductId, inventory));
        }
    }
}
