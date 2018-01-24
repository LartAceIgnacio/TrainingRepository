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
        private string connectionString;
        private DbContextOptions<DigiBookDbContext> dbOptions;
        private DigiBookDbContext dbContext;
        private VenueRepository sut;

        [TestInitialize]
        public void Initialize()
        {
            venue = new Venue()
            {
                VenueName = "Venue1",
                Description = "Sample Description for Venue 1."
            };


            connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DigiBookDb;Trusted_Connection=True;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            dbContext = new DigiBookDbContext(dbOptions);

            sut = new VenueRepository(dbContext);
            dbContext.Database.EnsureCreated();
        }

        [TestCleanup]
        public void CleanUp()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        [TestProperty("Integration","Venue")]
        public void Create_WithValidVenueData_SavesRecordToDb()
        {   
            // Arrange
            // Act
            var newVenue = sut.Create(venue);

            //Assert
            Assert.IsNotNull(newVenue);
            Assert.AreNotEqual(Guid.Empty, newVenue.VenueId);

            // Cleanup
            sut.Delete(venue.VenueId);
        }

        [TestMethod]
        [TestProperty("Integration", "Venue")]
        public void Delete_WithExistingVenue_DeletesRecordFromDb()
        {
            // Arrange
            var newVenue = sut.Create(venue);
            
            // Act
            sut.Delete(venue.VenueId);

            // Assert
            newVenue = sut.Retrieve(newVenue.VenueId);
            Assert.IsNull(newVenue);
            
        }

        [TestMethod]
        public void Retrieve_WithExistingVenueId_RetrievesRecordFromDb()
        {
            // Arrange
            var newVenue = sut.Create(venue);

            // Act
            var found = sut.Retrieve(venue.VenueId);

            // Assert
            Assert.IsNotNull(found);

            //CleanUp
            sut.Delete(venue.VenueId);

        }

        [TestMethod]
        public void Update_WithExistingVenue_UpdateDataFromDb()
        {
            // Arrange
            var newVenue = sut.Create(venue);
            var expectedVenueName = "Training Room";
            var expectedDescription = "Training room is reserved by Matt.";

            newVenue.VenueName = expectedVenueName;
            newVenue.Description = expectedDescription;

            // Act
            
            sut.Update(newVenue.VenueId, newVenue);

            // Assert
            var updatedVenue = sut.Retrieve(newVenue.VenueId);
            Assert.AreEqual(newVenue.VenueName, updatedVenue.VenueName);
            Assert.AreEqual(newVenue.Description, updatedVenue.Description);

            //CleanUp
            sut.Delete(updatedVenue.VenueId);
        }
    }
}
