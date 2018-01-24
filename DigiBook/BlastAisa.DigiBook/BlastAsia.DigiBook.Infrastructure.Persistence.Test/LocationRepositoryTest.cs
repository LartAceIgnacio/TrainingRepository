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
        private string connectionString;
        private DbContextOptions<DigiBookDbContext> dbOption;
        private DigiBookDbContext dbContext;
        private LocationRepository sut;
        private Location location;

        [TestInitialize]
        public void TestInitialize()
        {
            connectionString = @"Data Source=.; Database = DigiBookDb; Integrated Security = true";
            dbOption = new DbContextOptionsBuilder<DigiBookDbContext>()
                 .UseSqlServer(connectionString)
                 .Options;
            dbContext = new DigiBookDbContext(dbOption);
            sut = new LocationRepository(dbContext);
            location = new Location
            {
                LocationId = Guid.NewGuid(),
                LocationMark = "MegaMall",
                LocationName = "Ortigas"
            };
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_SavesRecordToTheDatabase()
        {
            //Arrange

            //Act
            var newLocation = sut.Create(location);
            //Assert
            Assert.IsNotNull(newLocation);
            Assert.IsTrue(newLocation.LocationId != Guid.Empty);
            //Cleanup
            sut.Delete(location.LocationId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithValidData_GetsRecordFromTheDatabase()
        {
            //Arrange
            var newLocation = sut.Create(location);
            //Act
            var found = sut.Retrieve(newLocation.LocationId);
            //Assert
            Assert.IsNotNull(found);
            //Cleanup
            sut.Delete(location.LocationId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_UpdatesRecordFromTheDatabase()
        {
            //Arrange
            var newLocation = sut.Create(location);

            var expectedLocationName = "Pasig";
            var expectedLocationMark = "Rob";

            newLocation.LocationName = expectedLocationName;
            newLocation.LocationMark = expectedLocationMark;
            //Act
            sut.Update(newLocation.LocationId, location);
            //Assert
            var update = sut.Retrieve(newLocation.LocationId);

            Assert.AreEqual(newLocation.LocationMark, update.LocationMark);
            Assert.AreEqual(newLocation.LocationName, update.LocationName);
            //Cleanup
            sut.Delete(location.LocationId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithValidData_RemovesRecordFromTheDatabase()
        {
            //Arrange
            var newLocation = sut.Create(location);
            //Act
            sut.Delete(newLocation.LocationId);
            //Assert
            location = sut.Retrieve(newLocation.LocationId);
            Assert.IsNull(location);
        }
    }
}
