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
        private Guid existingId;
        private Guid nonExistingId;
        private Location location;

        [TestInitialize]
        public void TestInitialize()
        {
            mockLocationRepository = new Mock<ILocationRepository>();
            sut = new LocationService(mockLocationRepository.Object);
            existingId = Guid.NewGuid();
            nonExistingId = Guid.Empty;

            location = new Location
            {
                LocationId = Guid.NewGuid(),
                LocationName = "Ortigas",
                LocationMark = "MegaMall"
            };

            mockLocationRepository
               .Setup(l => l.Retrieve(existingId))
                .Returns(location);

            mockLocationRepository
                .Setup(l => l.Retrieve(nonExistingId))
                .Returns<Location>(null);
        }
        
        [TestCleanup]
        public void TestCleanUp()
        {

        }

        [TestMethod]
        public void Save_WithValidData_ShouldCallRepositoryCreate()
        {
            //Arrange
            
            //Act
            var result = sut.Save(location.LocationId, location);
            //Assert
            mockLocationRepository
                .Verify(l => l.Create(location), Times.Once);
        }

        [TestMethod]
        public void Save_WithNullLocationName_ThrowsNullLocationNameException()
        {
            //Arrange
            location.LocationName = "";
            //Act

            //Assert
            Assert.ThrowsException<NullLocationNameException>(
                () => sut.Save(location.LocationId, location));

            mockLocationRepository
                .Verify(l => l.Create(location), Times.Never);
        }

        [TestMethod]
        public void Save_WithNullLocationMark_ThrowsNullLocationNameException()
        {
            //Arrange
            location.LocationMark = "";
            //Act

            //Assert
            Assert.ThrowsException<NullLocationMarkException>(
                () => sut.Save(location.LocationId, location));

            mockLocationRepository
                .Verify(l => l.Create(location), Times.Never);
        }

        [TestMethod]
        public void Save_WithExistingLocationId_ShouldCallRepositoryUpdate()
        {
            //Arrange
            location.LocationId = existingId;
            //Act
            var result = sut.Save(location.LocationId, location);
            //Assert
            mockLocationRepository
                .Verify(l => l.Retrieve(existingId), Times.Once);

            mockLocationRepository
                .Verify(l => l.Update(location.LocationId, location), Times.Once);
        }

        [TestMethod]
        public void Save_WithNonExistingLocationId_ShouldCallRepositoryCreate()
        {
            //Arrange
            location.LocationId = nonExistingId;
            //Act
            var result = sut.Save(location.LocationId, location);
            //Assert
            mockLocationRepository
                .Verify(l => l.Retrieve(nonExistingId), Times.Once);

            mockLocationRepository
                .Verify(l => l.Create(location), Times.Once);
        }
    }
}
