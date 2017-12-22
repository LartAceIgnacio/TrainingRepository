using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using BlastAsia.DigiBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Infrastructure.Persistence.Test
{
    [TestClass]
    public class VenueRepositoryTest
    {
        Venue venue;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        VenueRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            venue = new Venue
            {
                VenueName = "VenueVenueVenueVenueVenueVenueVenueVenueVenueVenue Venue",
                Description = "This is a Venue"
            };

            connectionString =
                @"Server=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new VenueRepository(dbContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Create_VenueWithValidData_SavesRecordInTheDatabase()
        {
            // Act
            var newVenue = sut.Create(venue);

            // Assert
            Assert.IsNotNull(newVenue);
            Assert.IsTrue(newVenue.VenueId != Guid.Empty);

            //CleanUp
            sut.Delete(venue.VenueId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Delete_WithAnExistingVenue_RemovesRecordFromDatabase()
        {
            // Arrange
            var newVenue = sut.Create(venue);

            // Act
            sut.Delete(newVenue.VenueId);

            // Assert
            venue = sut.Retrieve(newVenue.VenueId);
            Assert.IsNull(venue);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Retrieve_WithAnExistingVenueId_ReturnsRecordFromDb()
        {
            // Arrange
            var newVenue = sut.Create(venue);

            // Act
            var found = sut.Retrieve(newVenue.VenueId);

            // Assert
            Assert.IsNotNull(newVenue);

            //Cleanup
            sut.Delete(newVenue.VenueId);

        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_VenueWithValidData_SavesUpadatesInDb()
        {
            // Arrange
            var newVenue = sut.Create(venue);
            var expectedVenueName = "VenueVenueVenueVenueVenueVenueVenueVenueVenueVenue Lugar";
            var expectedDescription = "Nothing";

            newVenue.VenueName = expectedVenueName;
            newVenue.Description = expectedDescription;

            // Act
            sut.Update(newVenue.VenueId, newVenue);

            // Assert
            var updatedVenue = sut.Retrieve(newVenue.VenueId);
            Assert.AreEqual(expectedVenueName, updatedVenue.VenueName);
            Assert.AreEqual(expectedDescription, updatedVenue.Description);

            // Cleanup
            sut.Delete(updatedVenue.VenueId);

        }
    }
}
