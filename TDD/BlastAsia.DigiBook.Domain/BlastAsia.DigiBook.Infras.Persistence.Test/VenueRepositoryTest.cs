﻿using BlastAsia.DigiBook.Domain.Models.Venues;
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
        private string connectionString;
        private DbContextOptions<DigiBookDbContext> dbOptions;
        private DigiBookDbContext dbContext;
        private VenueRepository sut;
        private Venue venue;

        [TestInitialize]
        public void Initialize()
        {
            connectionString = @"Data Source=.;Database=DigiBookDb;Integrated Security=true;";
            dbOptions = new DbContextOptionsBuilder<DigiBookDbContext>()
                 .UseSqlServer(connectionString)
                 .Options;

            dbContext = new DigiBookDbContext(dbOptions); // ORM
            dbContext.Database.EnsureCreated();
            sut = new VenueRepository(dbContext);
            venue = new Venue
            {
                VenueId = new Guid(),
                VenueName = "Venue name",
                Description = ""
            };

        }

        [TestCleanup]
        public void CleanUp()
        {
            dbContext.Dispose();
            dbContext = null;
        }
        [TestMethod]
        public void Create_WithValidData_ShouldSaveRecordsToDb()
        {
            var newVenue = sut.Create(venue);

            Assert.IsNotNull(newVenue);
            Assert.IsTrue(newVenue.VenueId != Guid.Empty);

            //Cleanup
            sut.Delete(newVenue.VenueId);
        }

        [TestMethod]
        public void Retrieve_WithAnExistingVenueId_ShouldRetrieveRecordsFromDb()
        {
            sut.Create(venue);
            var retrieveVenue = sut.Retrieve(venue.VenueId);

            Assert.IsNotNull(retrieveVenue);

            //Cleanup
            sut.Delete(retrieveVenue.VenueId);
        }

        [TestMethod]
        public void Update_WithAnExistingVenueId_ShouldUpdateRecordsFromDb()
        {
            var newVenue = sut.Create(venue);

            var expectedName = "Edited Venue name";
            newVenue.VenueName = expectedName;

            venue = sut.Update(newVenue.VenueId, newVenue);

            Assert.AreEqual(venue.VenueName, expectedName);

            //Cleanup
            sut.Delete(venue.VenueId);
        }

        [TestMethod]
        public void Delete_WithAnExistingVenueId_ShouldDeleteRecordsFromDb()
        {
            var newVenue= sut.Create(venue);
           
            sut.Delete(newVenue.VenueId);

            venue = sut.Retrieve(newVenue.VenueId);
            Assert.IsNull(venue);
        }
    }
}
