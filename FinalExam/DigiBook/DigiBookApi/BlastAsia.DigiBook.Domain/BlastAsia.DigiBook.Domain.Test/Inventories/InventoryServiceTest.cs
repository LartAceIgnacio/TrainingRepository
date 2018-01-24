using BlastAsia.DigiBook.Domain.Exceptions;
using BlastAsia.DigiBook.Domain.Inventories;
using BlastAsia.DigiBook.Domain.Models.Inventories;
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
        private Mock<IInventoryRepository> mockRepo;
        private InventoryService sut;
        private Inventory inventory;

        [TestInitialize]
        public void InitializeTest()
        {
            mockRepo = new Mock<IInventoryRepository>();
            sut = new InventoryService(mockRepo.Object);
            inventory = new Inventory
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "ABCD1234",
                ProductName = "Ballpen",
                ProductDescription = "Black Ballpen",
                QuantityOnHand = 5,
                QuantityReserved = 2,
                QuantityOrdered = 3,
                DateCreated = DateTime.Today,
                DateModified = new DateTime(),
                IsActive = true,
                Bin = "01B1A"
            };

        }
        [TestProperty("TestType", "Inventory")]
        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositoryCreate()
        {
            //arrange


            //act
            sut.Save(inventory.ProductId, inventory);

            //assert
            mockRepo
                .Verify(r => r.Create(inventory), Times.Once);
        }

        [TestProperty("TestType", "Inventory")]
        [TestMethod]
        public void Save_WithBlankProductCode_ShouldThrowProductCodeIsRequired()
        {
            //arrange
            inventory.ProductCode = "";
            //act
            Assert.ThrowsException<ProductCodeRequiredException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestProperty("TestType", "Inventory")]
        [TestMethod]
        public void Save_ProductCodeWithLessThanTheRequiredLength_ShouldThrowProductCodeRequiredLength()
        {
            //arrange

            inventory.ProductCode = "ABCD123";
            //act
            Assert.ThrowsException<ProductCodeRequiredLengthException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestProperty("TestType", "Inventory")]
        [TestMethod]
        public void Save_WithBlankProductName_ShouldThrowProductNameRequiredException()
        {
            //arrange

            inventory.ProductName = "";
              
            //act
            Assert.ThrowsException<ProductNameRequiredException>(
                () => sut.Save(inventory.ProductId, inventory));
        }


        [TestProperty("TestType", "Inventory")]
        [TestMethod]
        public void Save_ProductNameExceedsTheMaximumLength_ShouldThrowProductNameExceedsMaximumLengthException()
        {
            //arrange
            inventory.ProductName = "1234567890123456789012345678901234567890123456789012345678901";

            //act
            Assert.ThrowsException<ProductNameExceedsMaximumLengthException>(
                () => sut.Save(inventory.ProductId, inventory));
        }


        [TestProperty("TestType", "Inventory")]
        [TestMethod]
        public void Save_BlankProductDescription_ShouldThrowProductDescriptionRequiredException ()
        {
            //arrange
            inventory.ProductDescription = "";

            //act
            Assert.ThrowsException<ProductDescriptionRequiredException>(
                () => sut.Save(inventory.ProductId, inventory));
        }


        [TestProperty("TestType", "Inventory")]
        [TestMethod]
        public void Save_ProductDescriptExceedsMaximiumLength_ShouldThrowProductDescriptionMaximumLengthException()
        {
            //arrange
            inventory.ProductDescription = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";

            //act
            Assert.ThrowsException<ProductDescriptionMaximumLengthException>(
                () => sut.Save(inventory.ProductId, inventory));
        }


        [TestProperty("TestType", "Inventory")]
        [TestMethod]
        public void Save_InvalidQuantityOnHand_ShouldThrowInvalidQuantityOnHandException()
        {
            //arrange
            inventory.QuantityOnHand = -1;

            //act
            Assert.ThrowsException<InvalidQuantityOnHandException>(
                () => sut.Save(inventory.ProductId, inventory));
        }


        [TestProperty("TestType", "Inventory")]
        [TestMethod]
        public void Save_InvalidQuantityReserved_ShouldThrowInvalidQuantityReservedException()
        {
            //arrange
            inventory.QuantityReserved = -1;

            //act
            Assert.ThrowsException<InvalidQuantityReservedException>(
                () => sut.Save(inventory.ProductId, inventory));
        }

        [TestProperty("TestType", "Inventory")]
        [TestMethod]
        public void Save_InvalidQuantityOrder_ShouldThrowInvalidQuantityOrderException()
        {
            //arrange
            inventory.QuantityOrdered = -1;

            //act
            Assert.ThrowsException<InvalidQuantityOrderException>(
                () => sut.Save(inventory.ProductId, inventory));
        }


        [TestProperty("TestType", "Inventory")]
        [TestMethod]
        public void Save_WithBlankBin_ShouldThrowBinRequiredException()
        {
            //arrange
            inventory.Bin = "";

            //act
            Assert.ThrowsException<BinRequiredException>(
                () => sut.Save(inventory.ProductId, inventory));
        }


        [TestProperty("TestType", "Inventory")]
        [TestMethod]
        public void Save_BinExceedsMaximumLength_ShouldThrowBinExceedsMaximumLengthException()
        {
            //arrange
            inventory.Bin = "123456";

            //act
            Assert.ThrowsException<BinExceedsMaximumLengthException>(
                () => sut.Save(inventory.ProductId, inventory));
        }


        [TestProperty("TestType", "Inventory")]
        [TestMethod]
        public void Save_InvalidBinFormat_ShouldThrowInvalidBinFormatException()
        {
            //arrange
            inventory.Bin = "11111";

            //act
            Assert.ThrowsException<InvalidBinFormatException>(
                () => sut.Save(inventory.ProductId, inventory));
        }
    }
}
