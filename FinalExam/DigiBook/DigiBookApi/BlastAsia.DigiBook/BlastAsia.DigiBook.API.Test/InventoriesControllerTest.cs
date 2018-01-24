using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Inventories;
using BlastAsia.DigiBook.Domain.Models.Inventories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class InventoriesControllerTest
    {
        private Mock<IInventoryRepository> mockInventoryRepository;
        private Mock<IInventoryService> mockInventoryService;
        private InventoriesController sut;
        private Inventory inventory;
        private Guid existingProductId;
        private Guid emptyProductId;
        private JsonPatchDocument patchedInventory;

        [TestInitialize]
        public void InitializeTest()
        {
            mockInventoryRepository = new Mock<IInventoryRepository>();
            mockInventoryService = new Mock<IInventoryService>();
            sut = new InventoriesController(mockInventoryRepository.Object
                , mockInventoryService.Object);
            inventory = new Inventory();

            existingProductId = Guid.NewGuid();
            emptyProductId = Guid.Empty;
            patchedInventory = new JsonPatchDocument();
            mockInventoryRepository
                .Setup(ir => ir.Retrieve(existingProductId))
                .Returns(inventory);
            mockInventoryRepository
                .Setup(ir => ir.Retrieve(emptyProductId))
                .Returns<Inventory>(null);

        }
        [TestMethod]
        public void GetInventories_WithEmptyProductId_ReturnsOkObjectValue()
        {
            // Arrange
            mockInventoryRepository
                .Setup(ir => ir.Retrieve())
                .Returns(new List<Inventory>());

            // Act
            var result = sut.GetInventories(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void GetInventories_WithExistingProductId_OkObjectValue()
        {
            // Arrange
            var result = sut.GetInventories(existingProductId);

            // Assert
            mockInventoryRepository
                .Verify(ir => ir.Retrieve(existingProductId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PostInventory_WithEmptyInventory_ReturnsBadRequest()
        {
            // Act
            var result = sut.PostInventory(null);

            // Assert
            mockInventoryService
                .Verify(i => i.Save(Guid.Empty, inventory), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostInventory_WithValidData_ReturnsNewInventoryWithProductId()
        {
            // Arrange
            mockInventoryService
                .Setup(i => i.Save(Guid.Empty, inventory))
                .Returns(inventory);

            // Act
            var result = sut.PostInventory(inventory);

            // Assert
            mockInventoryService
                .Verify(i => i.Save(Guid.Empty, inventory), Times.Once);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void DeleteInventory_WithEmptyProductId_ReturnsNotFound()
        {
            // Act
            var result = sut.DeleteInventory(emptyProductId);

            // Assert
            mockInventoryRepository
                .Verify(ir => ir.Delete(emptyProductId), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteInventory_WithExistingProductId_ReturnsOkObjectResult()
        {
            // Act
            var result = sut.DeleteInventory(existingProductId);

            // Assert
            mockInventoryRepository
                .Verify(ir => ir.Update(existingProductId, inventory), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PutInventory_WithEmptyInventory_ReturnsBadRequest()
        {
            // Act
            var result = sut.PutInventory(null, existingProductId);

            // Assert
            mockInventoryRepository
                .Verify(ir => ir.Update(existingProductId, null), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PutInventory_WithExistingProductIdAndInventory_ReturnsOkObjectValue()
        {
            // Act
            var result = sut.PutInventory(inventory, existingProductId);

            // Assert
            mockInventoryRepository
                .Verify(ir => ir.Update(existingProductId, inventory), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchInventory_WithEmptyProductId_ReturnsNotFound()
        {
            // Act
            var result = sut.PatchInventory(patchedInventory, emptyProductId);

            // Assert
            mockInventoryService
                .Verify(i => i.Save(emptyProductId, inventory), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchInventory_WithExistingProductId_ReturnsOkObjectValue()
        {
            // Act
            var result = sut.PatchInventory(patchedInventory, existingProductId);

            // Assert
            mockInventoryService
                .Verify(i => i.Save(existingProductId, inventory), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
