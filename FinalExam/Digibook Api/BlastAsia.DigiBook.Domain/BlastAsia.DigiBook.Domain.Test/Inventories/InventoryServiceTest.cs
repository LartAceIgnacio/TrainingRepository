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
        private Inventory inventory;
        private InventoryService sut;
        private Mock<IInventoryRepository> mockInventoryRepository;
        private Guid existingInventoryId = Guid.NewGuid();
        private Guid nonExistingInventoryId = Guid.Empty;

        [TestInitialize]
        public void Initialize()
        {
            inventory = new Inventory {
                ProductCode = "AAAAAAAA",
                ProductName = "Pencil",
                ProductDescription = "Can write in paper",
                OnHand = 12,
                OnReserved = 15,
                OnOrdered = 50,
                DateCreated = DateTime.Now,
                DateModified = new Nullable<DateTime>(),
                IsActive = true,
                Bin = "01B1A"
            };

            mockInventoryRepository = new Mock<IInventoryRepository>();

            mockInventoryRepository
                .Setup(x => x.Retrieve(existingInventoryId))
                .Returns(inventory);

            mockInventoryRepository
                .Setup(x => x.Retrieve(nonExistingInventoryId))
                .Returns<Inventory>(null);

            sut = new InventoryService(mockInventoryRepository.Object);
        }


        [TestCleanup]
        public void TestCleanup()
        {

        }

        [TestMethod]
        public void Save_NewInventoryWithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange


            //Act
            var result = sut.Save(inventory.ProductId, inventory);

            //Assert
            mockInventoryRepository.Verify(x => x.Create(inventory), Times.Once());
        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewInventoryWithProductId()
        {
            //Arrange
            mockInventoryRepository
                .Setup(x => x.Create(inventory))
                .Callback(() => inventory.ProductId = Guid.NewGuid())
                .Returns(inventory);

            //Act
            var newInventory = sut.Save(inventory.ProductId, inventory);

            //Assert
            Assert.IsTrue(newInventory.ProductId != Guid.Empty);
        }

        [TestMethod]
        public void Save_NewInventoryWithExistingProductCode_ThrowsProductCodeShouldUniqueException()
        {
            //Arrange
            mockInventoryRepository
                .Setup(x => x.Retrieve())
                .Returns(() => new List<Inventory>
                {
                    new Inventory {
                        ProductCode = "AAAAAAAA",
                        ProductName = "Pencil",
                        ProductDescription = "Can write in paper",
                        OnHand = 12,
                        OnReserved = 15,
                        OnOrdered = 50,
                        DateCreated = DateTime.Now,
                        DateModified = new Nullable<DateTime>(),
                        IsActive = true,
                        Bin = "01B1A"
                    }
                });

            //Act

            //Assert
            Assert.ThrowsException<ProductCodeShouldUniqueException>(() => sut.Save(inventory.ProductId, inventory));
            mockInventoryRepository.Verify(x => x.Create(It.IsAny<Inventory>()), Times.Never);
        }

        public void Save_WithValidData_ShouldCallRepositoryUpdate()
        {
            //Arrange
            inventory.ProductId = existingInventoryId;

            //Act
            sut.Save(inventory.ProductId, inventory);

            //Assert
            mockInventoryRepository.Verify(x => x.Update(inventory.ProductId, It.IsAny<Inventory>()), Times.Once());
        }

        [TestMethod]
        public void Save_WithBlankProductCode_ThrowsProductsRequiredException()
        {
            //Arrange
            inventory.ProductCode = "";

            //Act

            //Asset
            Assert.ThrowsException<ProductsRequiredException>(() => sut.Save(inventory.ProductId, inventory));
            mockInventoryRepository.Verify(x => x.Create(It.IsAny<Inventory>()), Times.Never);
        }

        [DataTestMethod]
        [DataRow("0000")]
        [DataRow("A52125aaAA")]
        [TestMethod]
        public void Save_WithProductCodeNotHaveLengthEqualToEight_ThrowsInvalidProductCodeSizeException(string productCode)
        {
            //Arrange
            inventory.ProductCode = productCode;

            //Act

            //Asset
            Assert.ThrowsException<InvalidProductCodeSizeException>(() => sut.Save(inventory.ProductId, inventory));
            mockInventoryRepository.Verify(x => x.Create(It.IsAny<Inventory>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankProductName_ThrowsProductsRequiredException()
        {
            //Arrange
            inventory.ProductName = "";

            //Act

            //Asset
            Assert.ThrowsException<ProductsRequiredException>(() => sut.Save(inventory.ProductId, inventory));
            mockInventoryRepository.Verify(x => x.Create(It.IsAny<Inventory>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithMoreThanFiftyCharsInProductName_ThrowsProductNameMaxLengthException()
        {
            //Arrange
            inventory.ProductName = "           Automatic Raise Credit Limit in Bank....           ";

            //Act


            //Assert
            Assert.ThrowsException<ProductNameMaxLengthException>(() => sut.Save(inventory.ProductId, inventory));
            mockInventoryRepository.Verify(x => x.Create(inventory), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankProductDescription_ThrowsProductsRequiredException()
        {
            //Arrange
            inventory.ProductDescription = "";

            //Act

            //Asset
            Assert.ThrowsException<ProductsRequiredException>(() => sut.Save(inventory.ProductId, inventory));
            mockInventoryRepository.Verify(x => x.Create(It.IsAny<Inventory>()), Times.Never);
        }

        [TestMethod]
        public void Save_WithMoreThanTwoFiftyCharsInProductDescription_ThrowsProductDiscriptionMaxLengthException()
        {
            //Arrange
            inventory.ProductDescription = "      Automatic Raise Credit Limit in Bank....            Automatic Raise Credit Limit in Bank....      " +
                "      Automatic Raise Credit Limit in Bank....            Automatic Raise Credit Limit in Bank....      " +
                "      Automatic Raise Credit Limit in Bank....            Automatic Raise Credit Limit in Bank....      ";

            //Act


            //Assert
            Assert.ThrowsException<ProductDescriptionMaxLengthException>(() => sut.Save(inventory.ProductId, inventory));
            mockInventoryRepository.Verify(x => x.Create(inventory), Times.Never);
        }

        [TestMethod]
        public void Save_WithBlankBin_ThrowsProductsRequiredException()
        {
            //Arrange
            inventory.Bin = "";

            //Act

            //Asset
            Assert.ThrowsException<ProductsRequiredException>(() => sut.Save(inventory.ProductId, inventory));
            mockInventoryRepository.Verify(x => x.Create(It.IsAny<Inventory>()), Times.Never);
        }

        [DataTestMethod]
        [DataRow("00B1A")]
        [DataRow("01B1AA")]
        [DataRow("01B0A")]
        [TestMethod]
        public void Save_WithInvalidBin_ThrowsInvalidBinFormatException(string bin)
        {
            //Arrange
            inventory.Bin = bin;

            //Act

            //Asset
            Assert.ThrowsException<InvalidBinFormatException>(() => sut.Save(inventory.ProductId, inventory));
            mockInventoryRepository.Verify(x => x.Create(It.IsAny<Inventory>()), Times.Never);
        }
    }
}
