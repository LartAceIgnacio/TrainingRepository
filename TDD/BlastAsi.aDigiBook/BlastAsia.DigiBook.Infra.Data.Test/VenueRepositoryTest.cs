using BlastAsia.DigiBook.Domain.Models.Venues;
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
        public void Initialize()
        {
            venue = new Venue
            {
                VenueName = "Orange wall",
                Description = "Must be orange"
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
        public void CleanUp()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_WithValidData_ShouldSaveRecordOnDatabase()
        {
            // Arrange

            // Act
            var newVenue = sut.Create(venue);

            // Assert
            Assert.IsNotNull(newVenue);
            Assert.IsTrue(newVenue.VenueId != Guid.Empty);

            //Cleanup
            sut.Delete(venue.VenueId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithExistingId_ShouldReturnRecordFromDatabase()
        {
            // Arrange
            var newVenue = sut.Create(venue);

            // Act
            var foundVenue = sut.Retrieve(newVenue.VenueId);

            // Assert
            Assert.IsNotNull(foundVenue);

            //Cleanup
            sut.Delete(newVenue.VenueId);
        }


        [TestMethod]
        public void Update_WithValidData_ShouldSaveChangesInDatabase()
        {
            // Arrange
            var newVenue = sut.Create(venue);
            var expectedVenueName = "Lobby";
            var expectedVenueDescription = "Wear wizard robe";

            newVenue.VenueName = expectedVenueName;
            newVenue.Description = expectedVenueDescription;

            // Act
            var updatedVenue = sut.Update(newVenue.VenueId,venue);

            // Assert
            Assert.AreEqual(expectedVenueName, updatedVenue.VenueName);
            Assert.AreEqual(expectedVenueDescription, updatedVenue.Description);

            //Cleanup
            sut.Delete(newVenue.VenueId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithExistingId_ShouldRemoveRecordFromDatabase()
        {
            // Arrange
            var newVenue = sut.Create(venue);

            // Act
            sut.Delete(venue.VenueId);

            // Assert
            venue = sut.Retrieve(venue.VenueId);

            Assert.IsNull(venue);
        }
    }
}
