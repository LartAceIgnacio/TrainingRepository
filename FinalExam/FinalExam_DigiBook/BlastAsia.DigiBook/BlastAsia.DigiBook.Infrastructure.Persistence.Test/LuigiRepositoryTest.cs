using BlastAsia.DigiBook.Domain.Models.Luigis;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class LuigiRepositoryTest
    {

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_CreateRecordInTheDatabase()
        {
            // Arrange

            var dbOptions = new DbContextOptions<DigiBookDbContext>();

            var connectionString
                = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            var DbContext = new DigiBookDbContext(dbOptions);
            DbContext.Database.EnsureCreated();

            var sut = new LuigiRepository(DbContext);

            var luigi = new Luigi
            {
               FirstName = "Luigi",
               LastName = "Abille"
            };

            var newLuigi = sut.Create(luigi);

            // Assert

            Assert.IsNotNull(newLuigi);
            Assert.IsTrue(newLuigi.LuigiId != Guid.Empty);

            // Cleanup

            sut.Delete(newLuigi.LuigiId);

        }
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithExistingLuigi_RemovesRecordFromDatabase()
        {
            // Arrange

            var dbOptions = new DbContextOptions<DigiBookDbContext>();

            var connectionString
                = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            var DbContext = new DigiBookDbContext(dbOptions);
            DbContext.Database.EnsureCreated();

            var sut = new LuigiRepository(DbContext);

            var luigi = new Luigi
            {
                FirstName = "Luigi",
                LastName = "Abille"
            };

            var newLuigi = sut.Create(luigi);

            // Act

            sut.Delete(newLuigi.LuigiId);

            // Assert

            luigi = sut.Retrieve(newLuigi.LuigiId);
            Assert.IsNull(luigi);
        }
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingLuigiId_ReturnsRecordFromDb()
        {
            // Arrange

            var dbOptions = new DbContextOptions<DigiBookDbContext>();

            var connectionString
                = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            var DbContext = new DigiBookDbContext(dbOptions);
            DbContext.Database.EnsureCreated();

            var sut = new LuigiRepository(DbContext);

            var luigi = new Luigi
            {
                FirstName = "Luigi",
                LastName = "Abille"
            };

            var newLuigi = sut.Create(luigi);

            // Act

            var found = sut.Retrieve(newLuigi.LuigiId);

            // Assert

            Assert.IsNotNull(found);

            // Cleanup

            sut.Delete(found.LuigiId);

        }
        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdatesInDb()
        {
            // Arrange
            var dbOptions = new DbContextOptions<DigiBookDbContext>();

            var connectionString
                = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            var DbContext = new DigiBookDbContext(dbOptions);
            DbContext.Database.EnsureCreated();

            var sut = new LuigiRepository(DbContext);

            var luigi = new Luigi
            {
                FirstName = "Luigi",
                LastName = "Abille"
            };

            var newLuigi = sut.Create(luigi);
            var expectedFirstName = "Antonio";
            var expectedLastName = "Tan";

            newLuigi.FirstName = expectedFirstName;
            newLuigi.LastName = expectedLastName;

            // Act

            sut.Update(newLuigi.LuigiId, newLuigi);

            // Assert

            var updatedLuigi = sut.Retrieve(newLuigi.LuigiId);
            Assert.AreEqual(expectedFirstName, updatedLuigi.FirstName);
            Assert.AreEqual(expectedLastName, updatedLuigi.LastName);

            // Cleanup

            sut.Delete(updatedLuigi.LuigiId);
        }
    }
}
