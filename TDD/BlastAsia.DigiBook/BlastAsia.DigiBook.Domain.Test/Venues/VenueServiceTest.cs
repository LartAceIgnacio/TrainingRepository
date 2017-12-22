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
        Mock<IVenueRepository> mockVenueRepository;
        VenueService sut;

        [TestInitialize]
        public void Initialize()
        {
            venue = new Venue
            {
                VenueName = "Venue",
                Description = "This is a Venue"
            };

            mockVenueRepository = new Mock<IVenueRepository>();

            sut = new VenueService(mockVenueRepository.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Save_VenueWithValidData_ShouldCallRepositoryCreate()
        {
            // Act
            sut.Save(venue);

            // Assert
            mockVenueRepository.Verify(v => v.Create(venue), Times.Once);
        }

        [TestMethod]
        public void Save_WithValidData_ShouldReturDataWithVenueId()
        {
            mockVenueRepository
                .Setup(v => v.Create(venue))
                .Callback(() => venue.VenueId = Guid.NewGuid())
                .Returns(venue);

            // Act
            sut.Save(venue);

            // Assert
            mockVenueRepository.Verify(v => v.Create(venue), Times.Once);
        }

        [TestMethod]
        public void Save_VenueWithExistingData_ShouldCallRepositoryUpdate()
        {
            // Arrange
            mockVenueRepository
                .Setup(v => v.Retrieve(venue.VenueId))
                .Returns(venue);

            // Act
            sut.Save(venue);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);
            mockVenueRepository.Verify(v => v.Update(venue.VenueId, venue), Times.Once);
        }

        [TestMethod]
        public void Save_WithBlankVenueName_ThrowsVenueNameRequiredException()
        {
            // Arrange
            venue.VenueName = null;

            // Assert
            Assert.ThrowsException<VenueNameRequiredException>(
                ()=> sut.Save(venue));
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Never);
            mockVenueRepository.Verify(v => v.Update(venue.VenueId, venue), Times.Never);
        }
    }
}
