using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models.Inventories;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class InventoryRepositoryTest
    {
        private String connectionString = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private InventoryRepository sut = null;
        private Inventory inventory = null;

        [TestInitialize]
        public void InitializeTest()
        {
            connectionString = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new InventoryRepository(dbContext);
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

        [TestCleanup]
        public void CleanupTest()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Create_WithValidData_SavesRecordInDatabase()
        {
            // Act
            var newInventory = sut.Create(inventory);

            // Assert
            Assert.IsNotNull(newInventory);
            Assert.IsTrue(newInventory.ProductId != Guid.Empty);

            // Cleanup
            sut.Delete(newInventory.ProductId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingProductId_ReturnsRecordFromDatabase()
        {
            // Arrange
            var newInventory = sut.Create(inventory);

            // Act
            var found = sut.Retrieve(newInventory.ProductId);

            // Assert
            Assert.IsNotNull(found);

            // Cleanup
            sut.Delete(newInventory.ProductId);

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SaveUpdatesInDatabase()
        {
            // Arrange
            var newInventory = sut.Create(inventory);
            var expectedProductCode = "00000001";
            var expectedProductName = "product 1.1";
            var expectedProductDescription = "product desc";
            var expectedQOH = 90;
            var expectedQOR = 80;
            var expectedQOO = 10;

            newInventory.ProductCode = expectedProductCode;
            newInventory.ProductName = expectedProductName;
            newInventory.ProductDescription = expectedProductDescription;
            newInventory.QOH = expectedQOH;
            newInventory.QOR = expectedQOR;
            newInventory.QOO = expectedQOO;

            // Act
            sut.Update(newInventory.ProductId, inventory);

            // Assert
            var updatedInventory = sut.Retrieve(newInventory.ProductId);
            Assert.AreEqual(expectedProductCode, updatedInventory.ProductCode);
            Assert.AreEqual(expectedProductName, updatedInventory.ProductName);
            Assert.AreEqual(expectedProductDescription, updatedInventory.ProductDescription);
            Assert.AreEqual(expectedQOH, updatedInventory.QOH);
            Assert.AreEqual(expectedQOO, updatedInventory.QOO);
            Assert.AreEqual(expectedQOR, updatedInventory.QOR);

            // Cleanup
            sut.Delete(newInventory.ProductId);
        }

        [TestMethod]
        public void Delete_WithAnExistingProduct_RemovesRecordFromDatabase()
        {
            // Arrange
            var newInventory = sut.Create(inventory);

            // Act
            sut.Delete(newInventory.ProductId);

            // Assert
            inventory = sut.Retrieve(newInventory.ProductId);
            Assert.IsNull(inventory);
        }
    }
}
