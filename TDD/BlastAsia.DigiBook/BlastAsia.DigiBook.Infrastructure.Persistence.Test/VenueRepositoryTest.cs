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
        private Venue _venue;
        private string _connectionString;
        private DbContextOptions<DigiBookDbContext> _dbOptions;
        private DigiBookDbContext _dbContext;
        private VenueRepository _sut;

        [TestInitialize]
        public void Initialize()
        {
            _venue = new Venue()
            {
                VenueName = "Venue1",
                Description = "Sample Description for Venue 1."
            };


            _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DigiBookDb;Trusted_Connection=True;";
            _dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                .UseSqlServer(_connectionString)
                .Options;

            _dbContext = new DigiBookDbContext(_dbOptions);

            _sut = new VenueRepository(_dbContext);
            _dbContext.Database.EnsureCreated();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _dbContext.Dispose();
            _dbContext = null;
        }

        [TestMethod]
        [TestProperty("Integration","Venue")]
        public void Create_WithValidVenueData_SavesRecordToDb()
        {   
            // Arrange
            // Act
            var newVenue = _sut.Create(_venue);

            //Assert
            Assert.IsNotNull(newVenue);
            Assert.AreNotEqual(Guid.Empty, newVenue.VenueId);

            // Cleanup
            _sut.Delete(_venue.VenueId);
        }

        [TestMethod]
        [TestProperty("Integration", "Venue")]
        public void Delete_WithExistingVenue_DeletesRecordFromDb()
        {
            // Arrange
            var newVenue = _sut.Create(_venue);
            
            // Act
            _sut.Delete(_venue.VenueId);

            // Assert
            newVenue = _sut.Retrieve(newVenue.VenueId);
            Assert.IsNull(newVenue);
            
        }

        [TestMethod]
        public void Retrieve_WithExistingVenueId_RetrievesRecordFromDb()
        {
            // Arrange
            var newVenue = _sut.Create(_venue);

            // Act
            var found = _sut.Retrieve(_venue.VenueId);

            // Assert
            Assert.IsNotNull(found);

            //CleanUp
            _sut.Delete(_venue.VenueId);

        }

        [TestMethod]
        public void Update_WithExistingVenue_UpdateDataFromDb()
        {
            // Arrange
            var newVenue = _sut.Create(_venue);
            var expectedVenueName = "Training Room";
            var expectedDescription = "Training room is reserved by Matt.";

            newVenue.VenueName = expectedVenueName;
            newVenue.Description = expectedDescription;

            // Act
            
            _sut.Update(newVenue.VenueId, newVenue);

            // Assert
            var updatedVenue = _sut.Retrieve(newVenue.VenueId);
            Assert.AreEqual(newVenue.VenueName, updatedVenue.VenueName);
            Assert.AreEqual(newVenue.Description, updatedVenue.Description);

            //CleanUp
            _sut.Delete(updatedVenue.VenueId);
        }
    }
}
