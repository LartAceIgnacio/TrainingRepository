using BlastAsia.DigiBook.Domain.Models.Locations;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class LocationRepositoryTest
    {
        private String connectionString = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private LocationRepository sut = null;
        private Location location = null;

        [TestInitialize]
        public void InitializeTest()
        {
            connectionString = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();
            sut = new LocationRepository(dbContext);

            location = new Location
            {
                LocationName = "Orient Square",
                Description = "Square building"
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
            // Arrange
           
            // Act
            var newLocation = sut.Create(location);

            // Assert
            Assert.IsNotNull(newLocation);
            Assert.IsTrue(newLocation.LocationId != Guid.Empty);

            // Cleanup
            sut.Delete(newLocation.LocationId);

        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Retrieve_WithExistingLocationId_ReturnsRecordFromDatabase()
        {
            // Arrange
            var newLocation = sut.Create(location);

            // Act
            var found = sut.Retrieve(newLocation.LocationId);

            // Assert
            Assert.IsNotNull(found);

            // Cleanup
            sut.Delete(found.LocationId);
        }

        [TestMethod]
        [TestProperty("TestType","Integration")]
        public void Update_WithValidData_SaveUpdatesInDatabase()
        {
            // Arrange
            var newLocation = sut.Create(location);
            var expectedLocationName = "El pueblo";
            var expectedDescription = "Mcdo, racks...";

            newLocation.LocationName = expectedLocationName;
            newLocation.Description = expectedDescription;

            // Act
            sut.Update(newLocation.LocationId, location);

            // Assert
            var updatedLocation = sut.Retrieve(newLocation.LocationId);
            Assert.AreEqual(expectedLocationName, updatedLocation.LocationName);
            Assert.AreEqual(expectedDescription, updatedLocation.Description);

            // Cleanup
            sut.Delete(updatedLocation.LocationId);

        }

        [TestMethod]
        public void Delete_WithAnExistingLocation_RemovesRecordFromDatabase()
        {
            // Arrange
            var newLocation = sut.Create(location);

            // Act
            sut.Delete(newLocation.LocationId);

            // Assert
            location = sut.Retrieve(newLocation.LocationId);
            Assert.IsNull(location);

        }
    }
}
