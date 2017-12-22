using BlastAsia.Digibook.Domain.Models.Venues;
using BlastAsia.Digibook.Infrastracture.Persistence;
using BlastAsia.Digibook.Infrastracture.Persistence.Repositories;
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
        private readonly Guid nonExistingId = Guid.Empty;
        private readonly string connectionString = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
        private DbContextOptions<DigiBookDbContext> dbContextOptions;
        private DigiBookDbContext digibookDbContext;
        private Venue venue;
        private VenueRepository sut;

        [TestInitialize]
        public void InitializeData()
        {
            dbContextOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                                                    .UseSqlServer(connectionString)
                                                    .Options;
            digibookDbContext = new DigiBookDbContext(dbContextOptions);
            venue = new Venue { VenueName = "My Venue", Description = "My Description" };
            sut = new VenueRepository(digibookDbContext);

            digibookDbContext.Database.EnsureDeleted();
            digibookDbContext.Database.EnsureCreated();
        }

        [TestCleanup]
        public void CleanData()
        {
            digibookDbContext.Dispose();
            digibookDbContext = null;
        }

        [TestMethod]
        public void Create_WithValidData_ShouldSaveInTheDatabase()
        {
            var newVenue = sut.Create(venue);

            Assert.IsNotNull(newVenue);
            Assert.IsTrue(newVenue.VenueId != nonExistingId);

            sut.Delete(newVenue.VenueId);
        }

        [TestMethod]
        public void Delete_WithExistingVenue_RemoveRecordFromDatabase()
        {
            var newVenue = sut.Create(venue);

            sut.Delete(newVenue.VenueId);
            var retrievedVenue = sut.Retrieve(newVenue.VenueId);

            Assert.IsNull(retrievedVenue);
        }

        [TestMethod]
        public void Retrieve_WithExistingVenue_ReturnsRecordFromDatabase()
        {
            var newVenue = sut.Create(venue);

            var retrievedVenue = sut.Retrieve(newVenue.VenueId);

            Assert.IsNotNull(retrievedVenue);
            sut.Delete(retrievedVenue.VenueId);
        }

        [TestMethod]
        public void Update_WithExistingVenue_SavesUpdatesInDb()
        {
            var newVenue = sut.Create(venue);

            string expectedVenueName = "My New Event";
            string expectedDescription = "My New Description";

            newVenue.VenueName = expectedVenueName;
            newVenue.Description = expectedDescription;

            sut.Update(newVenue.VenueId, venue);

            var updatedContact = sut.Retrieve(newVenue.VenueId);

            Assert.AreEqual(expectedVenueName, updatedContact.VenueName);
            Assert.AreEqual(expectedDescription, updatedContact.Description);

            sut.Delete(updatedContact.VenueId);
        }
    }
}
