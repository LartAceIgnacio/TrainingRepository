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
        Mock<IVenueRepository> _mockVenueRepo;
        VenueService _sut;
        Venue _venue;

        [TestInitialize]
        public void Initialize() {

            _mockVenueRepo = new Mock<IVenueRepository>();
            _sut = new VenueService(_mockVenueRepo.Object);
            _venue = new Venue() { VenueName = "Venue1", Description = "Sample Description for Venue 1." };

        }

        [TestCleanup]
        public void CleanUp() { }

        [TestMethod]
        [TestProperty("Service Test","Venue")]
        public void Save_WithNoVenueName_ThrowsVenueNameException()
        {
            // Arrange
            // Act
            _venue.VenueName = string.Empty;
            
            // Assert
            Assert.ThrowsException<VenueNameRequiredException>(() => _sut.Save(_venue.VenueId, _venue));
            _mockVenueRepo.Verify(repo => repo.Create(_venue), Times.Never);
        }

        [TestMethod]
        [TestProperty("Service Test", "Venue")]
        public void Save_WithVenueNameExceedsLimit_ThrowVenueFieldLimitExceedException()
        {
            // Arrange
            _venue.VenueName = "apodiaopdipaoidpaosdipaodiaposdiaposdiapsodiapsdoiaspdoiaspodiapsodiapsodiaspdoiaspdoiaspdoiaspdoaidpaosidapsodiadasdoiasdpaosdiaposdi";

            // Act
            
            // Assert
            Assert.ThrowsException<VenueFieldLimitExceedException>(() => _sut.Save(_venue.VenueId, _venue));
            _mockVenueRepo.Verify(repo => repo.Create(_venue), Times.Never);
        }

        [TestMethod]
        [TestProperty("Service Test", "Venue")]
        public void Save_WithVenueDecriptionExceedsLimit_ThrowVenueFieldLimitExceedException()
        {
            // Arrange
            _venue.Description = "apodiaopdipaoidpaosdipaodiaposdiaposdiapsodiapsdoiaspdoiaspodiapsodiapsodiaspdoiaspdoiaspdoiaspdoaidpaosidapsodiadasdoiasdpaosdiaposdi";

            // Act

            // Assert
            Assert.ThrowsException<VenueFieldLimitExceedException>(() => _sut.Save(_venue.VenueId, _venue));
            _mockVenueRepo.Verify(repo => repo.Create(_venue), Times.Never);
        }

        [TestMethod]
        [TestProperty("Service Test", "Venue")]
        public void Save_WithValidVenueData_ShouldCallRepositoryCreate()
        {

            // Arrange
            _mockVenueRepo.Setup(repo => repo.Create(_venue))
                .Callback(() => _venue.VenueId = Guid.NewGuid())
                .Returns(_venue);
            // Act

            var result = _sut.Save(_venue.VenueId, _venue);


            // Assert
            Assert.AreNotEqual(result.VenueId, Guid.Empty);
            _mockVenueRepo.Verify(repo => repo.Retrieve(Guid.Empty), Times.Once);
            _mockVenueRepo.Verify(repo => repo.Create(_venue), Times.Once);
        }

        [TestMethod]
        [TestProperty("Service Test", "Venue")]
        public void Save_WithExistingData_ShouldCallRepositoryUpdate()
        {
            var existingId = Guid.NewGuid();

            _mockVenueRepo.Setup(repo => repo.Retrieve(existingId))
                .Returns(_venue);

            _mockVenueRepo.Setup(repo => repo.Update(existingId, _venue))
                .Callback(()=> _venue.VenueId = existingId)
                .Returns(_venue);

            var result = _sut.Save(existingId, _venue);

            Assert.IsNotNull(result);
            _mockVenueRepo.Verify(repo => repo.Update(_venue.VenueId, _venue), Times.Once);
        }

    }
}
