
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
        private Mock<IVenueRepository> mockVenueRepository;
        private VenueService sut;
        private Venue venue;
        private Guid existingVenueId = Guid.NewGuid();
        private Guid nonExistingVenueId = Guid.Empty;

        [TestInitialize]
        public void InitializeTest()
        {
            mockVenueRepository = new Mock<IVenueRepository>();
            sut = new VenueService(mockVenueRepository.Object);
            venue = new Venue
            {
                VenueName = "BlastAsia",
                Description = "Description"
            };

            mockVenueRepository
               .Setup(v => v.Retrieve(nonExistingVenueId))
               .Returns<Venue>(null);

            mockVenueRepository
                .Setup(v => v.Retrieve(existingVenueId))
                .Returns(venue);
        }
        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositoryCreate()
        {

            // Act
            var result = sut.Save(venue.VenueId, venue);

            // Assert
            mockVenueRepository
                .Verify(v => v.Retrieve(nonExistingVenueId), Times.Once);
            mockVenueRepository
                .Verify(v => v.Create(venue), Times.Once);

        }
        [TestMethod]
        public void Save_WithExistingVenueId_ShouldCallRepositoryUpdate()
        {
            // Arrange
            venue.VenueId = existingVenueId;

            // Act
            sut.Save(venue.VenueId, venue);

            // Assert
            mockVenueRepository
                .Verify(c => c.Retrieve(existingVenueId), Times.Once);
            mockVenueRepository
                .Verify(c => c.Update(venue.VenueId, venue), Times.Once);
        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewVenueWithVenueId()
        {
            // Arrange 
            mockVenueRepository
                .Setup(v => v.Create(venue))
                .Callback(() => venue.VenueId = Guid.NewGuid())
                .Returns(venue);

            // Act
            var newVenue = sut.Save(venue.VenueId, venue);

            // Assert
            Assert.IsTrue(newVenue.VenueId != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithBlankVenueName_ThrowsVenueNameRequiredException()
        {
            // Arrange
            venue.VenueName = "";

            // Assert
            mockVenueRepository
                .Verify(v => v.Create(venue), Times.Never);
            Assert.ThrowsException<VenueNameRequired>(
                () => sut.Save(venue.VenueId, venue));
        }

        [TestMethod]
        public void Save_WithVenueNameGreaterThanMaximumLength_ThrowsVenueNameTooLongException()
        {
            // Arrange
            venue.VenueName = "123456789012345dfgdfgdfg67890123456789012345678901234567890";

            // Assert
            mockVenueRepository
                .Verify(v => v.Create(venue), Times.Never);
            Assert.ThrowsException<VenueNameInvalid>(
                () => sut.Save(venue.VenueId, venue));
        }

        [TestMethod]
        public void Save_WithDescriptionGreaterThanMaximumLength_ThrowsDescriptionTooLongException()
        {
            // Arrange
            venue.Description = "123456789012345dfgdfgdfg6789012345678901234123456789012345dfgdfgdfg678901234567890123123456789012345dfgdfgdfg678901234567";

            // Assert
            mockVenueRepository
                .Verify(v => v.Create(venue), Times.Never);
            Assert.ThrowsException<DescriptionTooLong>(
                () => sut.Save(venue.VenueId, venue));
        }
    }
}
