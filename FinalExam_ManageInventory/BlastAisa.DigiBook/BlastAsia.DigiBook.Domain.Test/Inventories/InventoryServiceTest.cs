using BlastAsia.DigiBook.Domain.Inventories;
using BlastAsia.DigiBook.Domain.Models.Inventories;
using BlastAsia.DigiBook.Domain.Test.Inventories.Inventories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        [TestInitialize]
        public void TestInitialize()
        {
            mockInventoryRepository = new Mock<IInventoryRepository>();
            sut = new InventoryService(mockInventoryRepository.Object);
            inventory = new Inventory
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "12345678",
                ProductName = "Lenovo",
                ProductDescription = "Laptop for trainees",
                QonHand = 25,
                QonReserved = 10,
                QonOrdered = 15,
                DateCreated = DateTime.Today,
                DateModified = DateTime.Today,
                IsActive = true,
                Bin = "01B1A"
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange
            
            //Act
            var result = sut.Save(inventory.ProductId, inventory);
            //Assert
            mockInventoryRepository
                .Verify(i => i.Create(inventory), Times.Once);
        }

        [TestMethod]
        public void Save_WithNullProductCode_ThrowsProductCodeRequiredException()
        {

            //Arrange
            inventory.ProductCode = "";
            //Act
            //Assert
            Assert.ThrowsException<ProductCodeRequiredException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithProdCodeLessThanEight_ThrowsProductCodeRequiresEightCharException()
        {
            //Arrange
            inventory.ProductCode = "1234567";
            //Act

            //Assert
            Assert.ThrowsException<ProductCodeRequiresEightCharException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithProdCodeGreaterThanEight_ThrowsProductCodeRequiresEightChars()
        {
            //Arrange
            inventory.ProductCode = "123456789";
            //Act

            //Assert
            Assert.ThrowsException<ProductCodeRequiresEightCharException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithNullProductName_ThrowsProductNameRequiredException()
        {
            //Arrange
            inventory.ProductName = "";
            //Act

            //Assert
            Assert.ThrowsException<ProductNameRequiredException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithProductNameGreaterThanSixtyChars_ThrowsProdNameMaxLengthSixtyException()
        {
            //Arrange
            inventory.ProductName = "1234567890123456789012345678901234567890123456789012345678901";
            //Act

            //Assert
            Assert.ThrowsException <ProdNameMaxLengthSixtyException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithNullProdDescription_ThrowsProdDescriptionRequiredException()
        {
            //Arrange
            inventory.ProductDescription = "";
            //Act

            //Assert
            Assert.ThrowsException<ProdDescriptionRequiredException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithProdDescriptionGreaterThanMaxLength_ThrowsProdDescriptionMaxLengthException()
        {
            //Arrange
           inventory.ProductDescription = "12345678901234567890123456789012345678901234567890" +
                "12345678901234567890123456789012345678901234567890" +
                "12345678901234567890123456789012345678901234567890" +
                "12345678901234567890123456789012345678901234567890" +
                "123456789012345678901234567890123456789012345678901";
            //Act

            //Assert
            Assert.ThrowsException<ProdDescriptionMaxLengthRequiredException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithQuantityOnHandWithNegativeInput_ThrowsQonHandRequiresPostiveInputException()
        {
            //Arrange

            inventory.QonHand = -1;
            //Act

            //Assert
            Assert.ThrowsException<QonHandRequiresPostiveInputException>(
                () => sut.Save(inventory.ProductId, inventory));
        }
        
        [TestMethod]
        public void Save_WithQuantityOnReservedWithNegativeInput_ThrowsQonReservedRequiredPositiveInputException()
        {
            //Arrange
            inventory.QonReserved = -1;
            //Act

            //Assert
            Assert.ThrowsException<QonReservedRequiredPositiveInputException>(
                () => sut.Save(inventory.ProductId, inventory));
        }
        
        [TestMethod]
        public void Save_WithQuantityonOrderedWithNegativeInput_ThrowsQonOrderedRequiredPositiveInputException()
        {
            //Arrange
            inventory.QonOrdered = -1;
            //Act

            //Assert
            Assert.ThrowsException<QonOrderedRequiredPositiveInputException>(
                () => sut.Save(inventory.ProductId, inventory));
        }
        
        [TestMethod]
        public void Save_WithNullBin_ThrowsBinRequiredException()
        {
            //Arrange
            inventory.Bin = "";
            //Act

            //Assert
            Assert.ThrowsException<BinRequiredException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithBinLessThanFiveChars_ThrowsBinRequiresFiveCharException()
        {
            //Arrange
            inventory.Bin = "01B1";
            //Act

            //Assert
            Assert.ThrowsException<BinRequiresFiveCharException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestMethod]
        public void Save_WithBinGreaterThanFiveChars_ThrowsBinRequiresFiveCharException()
        {
            //Arrange
            inventory.Bin = "01B1A1";
            //Act

            //Assert
            Assert.ThrowsException<BinRequiresFiveCharException>(
                () => sut.Save(inventory.ProductId, inventory));
        }
    }
}
