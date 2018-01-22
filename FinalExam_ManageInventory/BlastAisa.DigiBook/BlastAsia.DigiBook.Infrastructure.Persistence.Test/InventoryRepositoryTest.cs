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
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordToTheDatabase()
        {
            //Arrange
            string connectionString = @"Data Source=.; Database=DigiBookDb;Integrated Security=true";

            DbContextOptions<DigiBookDbContext> dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            DigiBookDbContext dbContext = new DigiBookDbContext(dbOptions);

            dbContext.Database.EnsureCreated();

            InventoryRepository sut = new InventoryRepository(dbContext);

            Inventory inventory = new Inventory
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
            //Act
            var result = sut.Create(inventory);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(inventory.ProductId != null);
            //Cleanup
            sut.Delete(inventory.ProductId);
        }
    }
}
