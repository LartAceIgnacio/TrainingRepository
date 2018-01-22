using BlastAsia.DigiBook.Domain.Locations;
using BlastAsia.DigiBook.Domain.Models.Locations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Test.Locations
{
    [TestClass]
    public class LocationServiceTest
    {
        private Mock<ILocationRepository> mockLocationRepository;
        private LocationService sut;
        private Location location;

        [TestInitialize]
        public void InitializeTest()
        {
            mockLocationRepository = new Mock<ILocationRepository>();
            sut = new LocationService(mockLocationRepository.Object);
            location = new Location
            {
                LocationName = "Orient Square",
                Description = "Square building"
            };

        }

        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositoryCreate()
        {
            // Act
            var result = sut.Save(location.LocationId, location);

            // Assert
            mockLocationRepository
                .Verify(l => l.Create(location), Times.Once);
        }

        [TestMethod]
        public void Save_WithExisitingLocationId_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var existingLocationId = Guid.NewGuid();
            location.LocationId = existingLocationId;

            mockLocationRepository
                .Setup(l => l.Retrieve(existingLocationId))
                .Returns(location);

            // Act
            sut.Save(existingLocationId, location);

            // Assert
            mockLocationRepository
                .Verify(l => l.Retrieve(existingLocationId), Times.Once);
            mockLocationRepository
                .Verify(l => l.Update(existingLocationId, location), Times.Once);
        }

        [TestMethod]
        public void Save_WithValidData_ReturnsNewLocationWithLocationId()
        {
            // Arrange
            mockLocationRepository
                .Setup(l => l.Create(location))
                .Callback(() => location.LocationId = Guid.NewGuid())
                .Returns(location);

            // Act
            var newLocation = sut.Save(location.LocationId, location);

            // Assert
            Assert.IsTrue(newLocation.LocationId != Guid.Empty);
        }

        [TestMethod]
        public void Save_WithBlankLocationName_ThrowsLocationNameRequiredException()
        {
            // Arrange
            location.LocationName = "";

            // Assert
            Assert.ThrowsException<LocationNameRequiredException>(
                () => sut.Save(location.LocationId, location));
        }

        [TestMethod]
        public void Save_DescriptionGreaterThanMaxLength_ThrowsDescriptionTooLongException()
        {
            // Arrange
            location.Description = "1234567891012345678910123456789101234567891012345678910123456789101234567891012345678910123456789101234567891012345678910";

            // Assert
            Assert.ThrowsException<DescriptionTooLongException>(
                () => sut.Save(location.LocationId, location));
        }
    }
}
