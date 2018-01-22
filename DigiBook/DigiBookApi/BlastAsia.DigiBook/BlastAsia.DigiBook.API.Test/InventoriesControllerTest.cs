using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Inventories;
using BlastAsia.DigiBook.Domain.Models.Inventories;
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
        [TestMethod]
        public void GetInventories_WithEmptyProductId_ReturnsOkObjectValue()
        {
            // Arrange
            Mock<IInventoryRepository> mockInventoryRepository = new Mock<IInventoryRepository>();
            Mock<IInventoryService> mockInventoryService = new Mock<IInventoryService>();
            InventoriesController sut = new InventoriesController(mockInventoryRepository.Object, mockInventoryService.Object);
            Inventory inventory = new Inventory();

            mockInventoryRepository
                .Setup(ir => ir.Retrieve())
                .Returns(new List<Inventory>());
            // Act
            var result = sut.GetInventories(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
