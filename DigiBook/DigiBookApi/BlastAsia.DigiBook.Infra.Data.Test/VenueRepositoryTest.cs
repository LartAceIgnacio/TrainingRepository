using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Infrastracture.Persistence.Repositories;
using BlastAsia.DigiBook.Insfrastracture.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastracture.Persistence.Test
{

    [TestClass]
    public class VenueRepositoryTest
    {
        private Venue venue;
        private DbContextOptions<DigiBookDbContext> dbOptions;
        private DigiBookDbContext dbContext;
        private VenueRepository sut;
        private string connectionString = @"Data Source=.; Database=DigiBookDb; Integrated Security=true;";

        [TestInitialize]
        public void Initialize()
        {
            venue = new Venue
            {
                VenueId = Guid.NewGuid(),
                VenueName = "Sample Venue",
                Description = "Desc!"
            };

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
        public void Create_WithValiData_ShouldSaveDataToDatabase()
        {
            // arrange

            // act 
            var newVenue = sut.Create(venue);

            // assert
            Assert.IsNotNull(newVenue);
            Assert.IsTrue(newVenue.VenueId != Guid.Empty);

            // cleanup
            sut.Delete(newVenue.VenueId);
        }


        [TestMethod]
        public void Delete_WithAnExistingVenue_ShouldDeleteDataFromDatabase()
        {
            // arrange
            var newVenue = sut.Create(venue);

            // act 
            sut.Delete(venue.VenueId);
            var found = sut.Retrieve(venue.VenueId);

            // assert
            Assert.IsNull(found);

        }


        [TestMethod]
        public void Retrieve_WithExistingVenue_ShouldRetrieveDataFromDatabase()
        {
            // arrange
            var newVenue = sut.Create(venue);

            // act 
            var found = sut.Retrieve(venue.VenueId);
            // assert

            Assert.IsNotNull(found);
            // cleanup
            sut.Delete(venue.VenueId);
        }

        [TestMethod]
        public void Update_WithExisitngVenue_ShouldUpdateDataFromDatabase()
        {
            // arrange
            var newVenue = sut.Create(venue);
            var expectedName = "expectedName";
            var expectedDescription = "expectedDesc";

            // act
            newVenue.VenueName = expectedName;
            newVenue.Description = expectedDescription;

            sut.Update(newVenue.VenueId, venue);
            var updatedVenue = sut.Retrieve(newVenue.VenueId);

            // assert
            Assert.AreEqual(updatedVenue.VenueName , expectedName);
            Assert.AreEqual(updatedVenue.Description, expectedDescription);

            // cleanup
            sut.Delete(updatedVenue.VenueId);

        }
    }
}
