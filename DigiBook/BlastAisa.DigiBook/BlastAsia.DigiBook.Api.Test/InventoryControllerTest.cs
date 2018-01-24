using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain;
using BlastAsia.DigiBook.Domain.Inventories;
using BlastAsia.DigiBook.Domain.Models.Inventories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Api.Test
{
    [TestClass]
    public class InventoryControllerTest
    {
        private Mock<IInventoryRepository> mockInventoryRepository;
        private Mock<IInventoryService> mockInventoryService;
        private InventoryController sut;
        private Guid nonExisting;
        private Guid existingId;
        private JsonPatchDocument patch;
        private Inventory inventory;

        [TestInitialize]
        public void TestInitialize()
        {
            mockInventoryRepository = new Mock<IInventoryRepository>();
            mockInventoryService = new Mock<IInventoryService>();
            sut = new InventoryController(mockInventoryRepository.Object, mockInventoryService.Object);
            nonExisting = Guid.Empty;
            existingId = Guid.NewGuid();
            patch = new JsonPatchDocument();
            inventory = new Inventory
            {
                ProductId = existingId,
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

            mockInventoryRepository
                .Setup(i => i.Retreive())
                .Returns(new List<Inventory>());

            mockInventoryRepository
                .Setup(i => i.Retrieve(existingId))
                .Returns(inventory);

            mockInventoryRepository
                .Setup(i => i.Retrieve(nonExisting))
                .Returns<Inventory>(null);
        }

        [TestMethod]
        public void GetInventories_WithEmptyInventoryId_ReturnsOkObjectResult()
        {
            //Arrange
            //Act
            var result = sut.GetInventories(null);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockInventoryRepository
                .Verify(i => i.Retreive(), Times.Once);
        }

        [TestMethod]
        public void GetInventories_WithExistingProductId_ReturnsOkObjectResult()
        {
            //Arrange
            inventory.ProductId = existingId;
            //Act
            var result = sut.GetInventories(inventory.ProductId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockInventoryRepository
                .Verify(i => i.Retrieve(existingId), Times.Once);
        }

        [TestMethod]
        public void PostInventories_WithExistingInventory_RetursCreatedAtActionResult()
        {
            //Arrange
            //Act
            var result = sut.PostInventories(inventory);
            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockInventoryService
                .Verify(i => i.Save(inventory.ProductId, inventory), Times.Once);
        }

        [TestMethod]
        public void PostInventories_WithNullInventory_ReturnsBadRequest()
        {
            //Arrange
            //Act
            var result = sut.PostInventories(null);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockInventoryService
                .Verify(i => i.Save(nonExisting, inventory), Times.Never);
        }

        [TestMethod]
        public void PutInventories_WithExistingInventoryId_ReturnsOkObjectResult()
        {
            //Arrange
            inventory.ProductId = existingId;

            mockInventoryRepository
                .Setup(i => i.Retrieve(existingId))
                .Returns(inventory);
            //Act
            var result = sut.PutInventories(inventory.ProductId, inventory);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockInventoryRepository
                .Verify(i => i.Retrieve(existingId), Times.Once);
            mockInventoryService
                .Verify(i => i.Save(inventory.ProductId, inventory), Times.Once);
        }
        
        [TestMethod]
        public void PutInventories_WithNonExistingProductId_ReturnsNotFound()
        {
            //Arrange
            inventory.ProductId = nonExisting;
            //Act
            var result = sut.PutInventories(inventory.ProductId, inventory);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockInventoryRepository
                .Verify(i => i.Retrieve(nonExisting), Times.Once);
            mockInventoryService
                .Verify(i => i.Save(inventory.ProductId, inventory), Times.Never);
        }

        [TestMethod]
        public void PutInventories_WithNullInventory_ReturnsBadRequest()
        {
            //Arrange
            inventory = null;
            //Act
            var result = sut.PutInventories(nonExisting, inventory);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockInventoryRepository
                .Verify(i => i.Retrieve(nonExisting), Times.Never);
        }

        [TestMethod]
        public void DeleteInventories_WithExistingProductId_ReturnsOkObjectResult()
        {
            //Arrange
            inventory.ProductId = existingId;
            //Act
            var result = sut.DeleteInventories(inventory.ProductId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockInventoryRepository
                .Verify(i => i.Retrieve(existingId), Times.Once);
            mockInventoryRepository
                .Verify(i => i.Delete(inventory.ProductId), Times.Once);
        }

        [TestMethod]
        public void DeleteInventories_WithNonExistingProductId_ReturnsNotFound()
        {
            //Arrange
            inventory.ProductId = nonExisting;
            //Act
            var result = sut.DeleteInventories(inventory.ProductId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockInventoryRepository
                .Verify(i => i.Retrieve(nonExisting), Times.Once);
            mockInventoryRepository
                .Verify(i => i.Delete(inventory.ProductId), Times.Never);
        }

        [TestMethod]
        public void PatchInventories_WithExistingProductId_ReturnsOkObjectResult()
        {
            //Arrange
            inventory.ProductId = existingId;
            //Act
            var result = sut.PatchInventories(inventory.ProductId, patch);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockInventoryRepository
                .Verify(i => i.Retrieve(existingId), Times.Once);
            mockInventoryService
                .Verify(i => i.Save(inventory.ProductId, inventory), Times.Once);
        }

        [TestMethod]
        public void PatchInventories_WithNonExistingProductId_ReturnsNotFound()
        {
            //Arrange
            inventory.ProductId = nonExisting;
            //Act
            var result = sut.PatchInventories(inventory.ProductId, patch);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockInventoryRepository
                .Verify(i => i.Retrieve(nonExisting), Times.Once);
            mockInventoryService
                .Verify(i => i.Save(inventory.ProductId, inventory), Times.Never);
        }

        [TestMethod]
        public void PatchInventories_WithNullPatch_ReturnsBadRequest()
        {
            //Arrange
            patch = null;
            //Act
            var result = sut.PatchInventories(inventory.ProductId, patch);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockInventoryRepository
                .Verify(i => i.Retrieve(nonExisting), Times.Never);
            mockInventoryService
                .Verify(i => i.Save(inventory.ProductId, inventory), Times.Never);
        }
    }
}
