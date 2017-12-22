using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues.Service;
using BlastAsia.DigiBook.Domain.Venues;
using BlastAsia.DigiBook.Domain.Venues.VenueExceptions;

namespace BlastAsia.DigiBook.Domain.Test.VenueTest
{
    [TestClass]
    public class VenueServiceTest
    {
        Mock<IVenueRepository> mockVenueRepo;
        VenueService sut;
        Venue venue;

        [TestInitialize]
        public void Initialize() {

            mockVenueRepo = new Mock<IVenueRepository>();
            sut = new VenueService(mockVenueRepo.Object);
            venue = new Venue() { VenueName = "Venue1", Description = "Sample Description for Venue 1." };

        }

        [TestCleanup]
        public void CleanUp() { }

        [TestMethod]
        [TestProperty("Service Test","Venue")]
        public void Save_WithNoVenueName_ThrowsVenueNameException()
        {
            // Arrange
            // Act
            venue.VenueName = string.Empty;
            
            // Assert
            Assert.ThrowsException<VenueNameRequiredException>(() => sut.Save(venue.VenueId, venue));
            mockVenueRepo.Verify(repo => repo.Create(venue), Times.Never);
        }

        [TestMethod]
        [TestProperty("Service Test", "Venue")]
        public void Save_WithVenueNameExceedsLimit_ThrowVenueFieldLimitExceedException()
        {
            // Arrange
            venue.VenueName = "apodiaopdipaoidpaosdipaodiaposdiaposdiapsodiapsdoiaspdoiaspodiapsodiapsodiaspdoiaspdoiaspdoiaspdoaidpaosidapsodiadasdoiasdpaosdiaposdi";

            // Act
            
            // Assert
            Assert.ThrowsException<VenueFieldLimitExceedException>(() => sut.Save(venue.VenueId, venue));
            mockVenueRepo.Verify(repo => repo.Create(venue), Times.Never);
        }

        [TestMethod]
        [TestProperty("Service Test", "Venue")]
        public void Save_WithVenueDecriptionExceedsLimit_ThrowVenueFieldLimitExceedException()
        {
            // Arrange
            venue.Description = "apodiaopdipaoidpaosdipaodiaposdiaposdiapsodiapsdoiaspdoiaspodiapsodiapsodiaspdoiaspdoiaspdoiaspdoaidpaosidapsodiadasdoiasdpaosdiaposdi";

            // Act

            // Assert
            Assert.ThrowsException<VenueFieldLimitExceedException>(() => sut.Save(venue.VenueId, venue));
            mockVenueRepo.Verify(repo => repo.Create(venue), Times.Never);
        }

        [TestMethod]
        [TestProperty("Service Test", "Venue")]
        public void Save_WithValidVenueData_ShouldCallRepositoryCreate()
        {

            // Arrange
            mockVenueRepo.Setup(repo => repo.Create(venue))
                .Callback(() => venue.VenueId = Guid.NewGuid())
                .Returns(venue);
            // Act

            var result = sut.Save(venue.VenueId, venue);


            // Assert
            Assert.AreNotEqual(result.VenueId, Guid.Empty);
            mockVenueRepo.Verify(repo => repo.Retrieve(Guid.Empty), Times.Once);
            mockVenueRepo.Verify(repo => repo.Create(venue), Times.Once);
        }

        [TestMethod]
        [TestProperty("Service Test", "Venue")]
        public void Save_WithExistingData_ShouldCallRepositoryUpdate()
        {
            var existingId = Guid.NewGuid();

            mockVenueRepo.Setup(repo => repo.Retrieve(existingId))
                .Returns(venue);

            mockVenueRepo.Setup(repo => repo.Update(existingId, venue))
                .Callback(()=> venue.VenueId = existingId)
                .Returns(venue);

            var result = sut.Save(existingId, venue);

            Assert.IsNotNull(result);
            mockVenueRepo.Verify(repo => repo.Update(venue.VenueId, venue), Times.Once);
        }

    }
}
