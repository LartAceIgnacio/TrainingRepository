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
        private Venue venue = null;
        private DbContextOptions<DigiBookDbContext> dbOptions = null;
        private DigiBookDbContext dbContext = null;
        private String connectionString = null;
        private VenueRepository sut = null;

        [TestInitialize]
        public void InitializeTest()
        {
            connectionString = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();
            sut = new VenueRepository(dbContext);
            venue = new Venue
            {
                VenueName = "BlastAsia",
                Description = "description"
            };
        }
        
        [TestCleanup]
        public void CleanupTest()
        {
            dbContext.Dispose();
            dbContext = null;
        }
        [TestMethod]
        public void Create_WithValidData_SavesRecordInDatabase()
        {
            // Act
            var newVenue = sut.Create(venue);

            // Assert
            Assert.IsNotNull(newVenue);
            Assert.IsTrue(newVenue.VenueId != Guid.Empty);

            // Cleanup
            sut.Delete(newVenue.VenueId);
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
            Assert.IsNotNull(found);

            // Cleanup
            sut.Delete(found.VenueId);
        }

        [TestMethod]
        [TestProperty("TestType", "Integration")]
        public void Update_WithValidData_SavesUpdateInDb()
        {
            // Arrange
            var newVenue = sut.Create(venue);
            var expectedVenueName = "AsiaBlast";

            newVenue.VenueName = expectedVenueName;

            // Act
            sut.Update(newVenue.VenueId, newVenue);

            // Assert
            var updatedVenue = sut.Retrieve(newVenue.VenueId, newVenue);

        }
    }
}
