using BlastAsia.DigiBook.Domain.Models.Inventories;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class InventoryRepositoryTest
    {
        private string connectionString;
        private DbContextOptions<DigiBookDbContext> dbOptions;
        private InventoryRepository sut;
        private Inventory inventory;
        private DigiBookDbContext dbContext;

        [TestInitialize]
        public void TestInitialize()
        {
            connectionString = @"Data Source=.; Database=DigiBookDb;Integrated Security=true";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();
            sut = new InventoryRepository(dbContext);
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
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordToTheDatabase()
        {
            //Arrange
            //Act
            var newInventory = sut.Create(inventory);
            //Assert
            Assert.IsNotNull(newInventory);
            Assert.IsTrue(inventory.ProductId != null);
            //Cleanup
            sut.Delete(inventory.ProductId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithValidData_GetsDataFromTheDatabase()
        {
            //Arrange
            var newInventory = sut.Create(inventory);
            //Act
            var found = sut.Retrieve(newInventory.ProductId);
            //Assert
            Assert.IsNotNull(found);
            //Cleanup
            sut.Delete(inventory.ProductId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_UpdatesDataFromTheDatabase()
        {
            //Arrange
            var newInventory = sut.Create(inventory);

            var expectedProductName = "Asus";

            newInventory.ProductName = expectedProductName;
            //Act
            sut.Update(newInventory.ProductId, inventory);
            //Assert
            var update = sut.Retrieve(newInventory.ProductId);

            Assert.AreEqual(newInventory.ProductName, update.ProductName);
            //Cleanup
            sut.Delete(inventory.ProductId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithValidData_DeletesRecordFromTheDatabase()
        {
            //Arrange
            var newInventory = sut.Create(inventory);
            //Act
            sut.Delete(newInventory.ProductId);
            //Assert
            var found = sut.Retrieve(newInventory.ProductId);
            Assert.IsNull(found);
        }
    }
}
