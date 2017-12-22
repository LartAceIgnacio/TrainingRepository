using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Venues
{
    [TestClass]
    public class VenueServiceTest
    {
        private Venue venue;
        private Mock<IVenueRepository> mockVenueRepository;
        private VenueService sut;
        private Guid existingVenueID;
        private Guid nonExistingVenueID;

        [TestInitialize]
        public void Initialize()
        {
            venue = new Venue
            {
                VenueName = "Orange wall",
                Description = "Must be orange"
            };

            mockVenueRepository = new Mock<IVenueRepository>();
            sut = new VenueService(mockVenueRepository.Object);

            existingVenueID = Guid.NewGuid();
            nonExistingVenueID = Guid.Empty;
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Save_WithNonExistingIdAndValidData_ShouldCallRepositoryCreate()
        {
            // Arrange
            venue.VenueId = nonExistingVenueID;

            // Act
            sut.Save(venue.VenueId, venue);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once());
            mockVenueRepository.Verify(v => v.Create(venue), Times.Once());
        }


        [TestMethod]
        public void Save_WithExistingIdAndValidData_ShouldCallRepositoryUpdate()
        {
            // Arrange
            venue.VenueId = existingVenueID;

            mockVenueRepository
                .Setup(v => v.Retrieve(existingVenueID))
                .Returns(venue);

            // Act
            sut.Save(venue.VenueId, venue);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once());
            mockVenueRepository.Verify(v => v.Update(venue.VenueId, venue), Times.Once());
            mockVenueRepository.Verify(v => v.Create(venue), Times.Never());
        }


        [TestMethod]
        public void Save_WithBlankVenueName_ThrowsVenueNameRequiredException()
        {
            // Arrange
            venue.VenueName = "";

            // Act

            // Assert
            Assert.ThrowsException<VenueNameRequiredException>(
                () => sut.Save(venue.VenueId, venue));

            mockVenueRepository.Verify(v => v.Create(venue), Times.Never());
        }

        [TestMethod]
      
        public void Save_WithVenueNameGreaterThanMaxLength_ThrowsVenueNameLessThanMaxLengthRequiredException()
        {
            // Arrange
            venue.VenueName = "qwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnm";

            // Act

            // Assert
            Assert.ThrowsException<VenueNameLessThanMaxLengthRequiredException>(
                 () => sut.Save(venue.VenueId, venue));

            mockVenueRepository.Verify(v => v.Create(venue), Times.Never());
        }

        [TestMethod]
        public void Save_WithVenueDescGreaterThanMaxLength_ThrowsVenueDescLessThanMaxLengthRequiredException()
        {
            // Arrange
            venue.Description = "qwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnm";

            // Act

            // Assert
            Assert.ThrowsException<VenueDescLessThanMaxLengthRequiredException>(
                () => sut.Save(venue.VenueId, venue));

            mockVenueRepository.Verify(v => v.Create(venue), Times.Never());
        }
    }
}
