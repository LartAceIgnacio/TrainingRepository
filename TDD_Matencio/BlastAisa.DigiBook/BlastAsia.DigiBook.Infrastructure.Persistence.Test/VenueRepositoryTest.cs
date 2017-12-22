using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class VenueRepositoryTest
    {
        private Venue venue;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private VenueRepository sut;

        [TestInitialize]
        public void TestInitialize()
        {
            venue = new Venue
            {
                venueId = Guid.NewGuid(),
                venueName = "Training Room",
                venueDescription = "Hello"
            };

            connectionString =
                @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
               .UseSqlServer(connectionString)
               .Options;

            dbContext = new DigiBookDbContext(dbOptions);

            dbContext.Database.EnsureCreated();

            sut = new VenueRepository(dbContext);
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
            var newVenue = sut.Create(venue);
            //Act
            //Assert
            Assert.IsNotNull(newVenue);
            Assert.IsTrue(newVenue.venueId != Guid.Empty);
            //Cleanup
            sut.Delete(venue.venueId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithValidData_GetsRecordFromTheDatabase()
        {
            //Arrange
            var newVenue = sut.Create(venue);
            //Act
            var found = sut.Retrieve(venue.venueId);
            //Assert
            Assert.IsNotNull(found);
            //Cleanup
            sut.Delete(venue.venueId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_UpdatesRecordFromTheDatabase()
        {
            //Arrange
            var newVenue = sut.Create(venue);

            var expectedVenueName = "Board Room A";

            newVenue.venueName = expectedVenueName;
            //Act
            sut.Update(newVenue.venueId, venue);
            //Assert
            var update = sut.Retrieve(newVenue.venueId);

            Assert.AreEqual(newVenue.venueName, update.venueName);
            //Cleanup
            sut.Delete(venue.venueId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithValidData_DeletesRecordFromTheDatabase()
        {
            //Arrange
            var newVenue = sut.Create(venue);
            //Act
            sut.Delete(newVenue.venueId);
            //Assert
            venue = sut.Retrieve(newVenue.venueId);
            Assert.IsNull(venue);
            //Cleanup
        }
    }
}
