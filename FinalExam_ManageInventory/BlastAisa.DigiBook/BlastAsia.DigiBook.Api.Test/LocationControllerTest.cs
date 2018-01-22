using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Locations;
using BlastAsia.DigiBook.Domain.Models.Locations;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Api.Test
{
    [TestClass]
    public class LocationControllerTest
    {
        private Mock<ILocationRepository> mockLocationRepository;
        private Mock<ILocationService> mockLocationService;
        private LocationController sut;
        private JsonPatchDocument patch;
        private Guid nonExistingId;
        private Guid existingId;
        private Location location;
        

        [TestInitialize]
        public void TestInitialize()
        {
            mockLocationRepository = new Mock<ILocationRepository>();
            mockLocationService = new Mock<ILocationService>();
            sut = new LocationController(mockLocationRepository.Object, mockLocationService.Object);
            patch = new JsonPatchDocument();
            nonExistingId = Guid.Empty;
            existingId = Guid.NewGuid();
            location = new Location
            {
                LocationId = Guid.NewGuid(),
                LocationMark = "MegaMall",
                LocationName = "Ortigas"
            };

            mockLocationRepository
                .Setup(l => l.Retreive())
                .Returns(new List<Location>());

            mockLocationRepository
                .Setup(l => l.Retrieve(existingId))
                .Returns(location);

            mockLocationRepository
                .Setup(l => l.Retrieve(nonExistingId))
                .Returns<Location>(null);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        [TestMethod]
        public void GetLocation_WithLocationId_ReturnsOkObjectResult()
        {
            //Arrange
            //Act
            var result = sut.GetLocations(location.LocationId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockLocationRepository
                .Verify(l => l.Retrieve(location.LocationId), Times.Once);
        }

        [TestMethod]
        public void GetLocations_WithEmptyLocationId_ReturnsOkObjectResult()
        {
            //Arrange
            //Act
            var result = sut.GetLocations(null);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockLocationRepository
                .Verify(l => l.Retreive(), Times.Once);
        }

        [TestMethod]
        public void CreateLocation_WithNonExistingLocationId_ReturnsCreatedAtActionResult()
        {
            //Arrange
            location.LocationId = nonExistingId;
            //Act
            var result = sut.CreateLocations(location);
            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockLocationService
                .Verify(l => l.Save(location.LocationId, location));
        }

        [TestMethod]
        public void CreateLocations_WithNullLocation_ReturnsBadRequest()
        {
            //Arrange
            location = null;
            //Act
            var result = sut.CreateLocations(location);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockLocationService
                .Verify(l => l.Save(nonExistingId, location), Times.Never);
        }

        [TestMethod]
        public void DeleteLocations_WithExistingLocationId_ReturnsNoContent()
        {
            //Arrange
            location.LocationId = existingId;
            //Act
            var result = sut.DeleteLocations(location.LocationId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockLocationRepository
                .Verify(l => l.Retrieve(existingId), Times.Once);
            mockLocationRepository
                .Verify(l => l.Delete(existingId), Times.Once);
        }

        [TestMethod]
        public void DeleteLocations_WithNonExistingLocationId_ReturnsNotFound()
        {
            //Arrange
            location.LocationId = nonExistingId;
            //Act
            var result = sut.DeleteLocations(location.LocationId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockLocationRepository
                .Verify(l => l.Retrieve(nonExistingId), Times.Once);
            mockLocationRepository
                .Verify(l => l.Delete(location.LocationId), Times.Never);
        }

        [TestMethod]
        public void UpdateLocations_WithExistingId_ReturnsOkObjectResult()
        {
            //Arrange
            location.LocationId = existingId;
            //Act
            var result = sut.UpdateLocations(location.LocationId, location);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockLocationRepository
                .Verify(l => l.Retrieve(existingId), Times.Once);
            mockLocationService
                .Verify(l => l.Save(location.LocationId, location), Times.Once);
        }

        [TestMethod]
        public void Update_WithNonExistingLocationId_ReturnsBadRequest()
        {
            //Arrange
            location.LocationId = nonExistingId;
            //Act
            var result = sut.UpdateLocations(location.LocationId, location);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockLocationRepository
                .Verify(l => l.Retrieve(nonExistingId), Times.Once);
            mockLocationService
                .Verify(l => l.Save(location.LocationId, location), Times.Never);
        }

        [TestMethod]
        public void UpdateLocation_WithNullLocation_ReturnsBadRequest()
        {
            //Arrange
            location = null;
            //Act
            var result = sut.UpdateLocations(nonExistingId, location);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockLocationService
                .Verify(l => l.Save(nonExistingId, location), Times.Never);
        }

        [TestMethod]
        public void PatchLocations_WithExistingLocationId_ReturnsOkObjectResult()
        {
            //Arrange
            location.LocationId = existingId;
            //Act
            var result = sut.PatchLocations(location.LocationId, patch);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockLocationRepository
                .Verify(l => l.Retrieve(existingId), Times.Once);
            mockLocationService
                .Verify(l => l.Save(location.LocationId, location), Times.Once);
        }

        [TestMethod]
        public void PatchLocations_WithNonExistingLocationId_ReturnsNotFound()
        {
            //Arrange
            location.LocationId = nonExistingId;
            //Act
            var result = sut.PatchLocations(location.LocationId, patch);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockLocationRepository
                .Verify(l => l.Retrieve(nonExistingId), Times.Once);
            mockLocationService
                .Verify(l => l.Save(location.LocationId, location), Times.Never);
        }

        [TestMethod]
        public void PatchLocations_WithNullLocation_ReturnBadRequest()
        {
            //Arrange

            patch = null;
            //Act
            var result = sut.PatchLocations(location.LocationId, patch);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockLocationRepository
                .Verify(l => l.Retrieve(existingId), Times.Never);
            mockLocationService
                .Verify(l => l.Save(location.LocationId, location), Times.Never);
        }
    }
}
