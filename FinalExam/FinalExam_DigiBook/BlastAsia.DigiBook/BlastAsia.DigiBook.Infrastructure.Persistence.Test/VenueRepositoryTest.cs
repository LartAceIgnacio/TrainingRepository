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
        private String connectionString = null;
        private DigiBookDbContext dbContext = null;
        private VenueRepository sut = null;

        [TestInitialize]
        public void InitializeTest()
        {
            venue = new Venue
            {
                VenueName = "lalala",
                Description = "lalala"
            };

            connectionString
                = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";

            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            sut = new VenueRepository(dbContext);
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
            // Arrange

            var newVenue = sut.Create(venue);

            // Act



            // Assert

            Assert.IsNotNull(newVenue);
            Assert.IsTrue(newVenue.VenueId != Guid.Empty);

            sut.Delete(newVenue.VenueId);
        }
        [TestMethod]
        public void Retrieve_WithExistingVenueId_RetrieveVenue()
        {
            // Arrange

            var newVenue = sut.Create(venue);

            // Act

            var found = sut.Retrieve(newVenue.VenueId);

            // Assert

            Assert.IsNotNull(found);

            sut.Delete(found.VenueId);
        }

        [TestMethod]
        public void Delete_WithExistingVenue_RemovesVenueFromDatabase()
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
        public void Update_WithValidData_UpdateVenueInDatabase()
        {
            // Arrange

            var updatedDescription = "qweqrqewrqw";
            venue.Description = updatedDescription;
            var newVenue = sut.Create(venue);
            var updatedVenue = sut.Retrieve(newVenue.VenueId);

            // Act

            sut.Update(newVenue.VenueId, venue);

            // Assert

            Assert.AreEqual(updatedDescription, updatedVenue.Description);

            sut.Delete(updatedVenue.VenueId);
        }
    }
}
