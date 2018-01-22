using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Locations;
using BlastAsia.DigiBook.Domain.Models.Locations;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.API.Test
{
    [TestClass]
    public class LocationsControllerTest
    {
        private Mock<ILocationRepository> mockLocationRepo;
        private Mock<ILocationService> mockLocationService;
        private LocationsController sut;
        private Location location;
        private Guid existingLocationId;
        private Guid emptyLocationId;
        private JsonPatchDocument patchedLocation;

        [TestInitialize]
        public void InitializeTest()
        {
            
            mockLocationRepo = new Mock<ILocationRepository>();
            mockLocationService = new Mock<ILocationService>();
            sut = new LocationsController(mockLocationRepo.Object,
                mockLocationService.Object);
            location = new Location();

            existingLocationId = Guid.NewGuid();
            emptyLocationId = Guid.Empty;
            patchedLocation = new JsonPatchDocument();

            mockLocationRepo
                .Setup(lr => lr.Retrieve(existingLocationId))
                .Returns(location);
            mockLocationRepo
                .Setup(lr => lr.Retrieve(emptyLocationId))
                .Returns<Location>(null);
        }
        [TestMethod]
        public void GetLocations_WithEmptyLocationId_ReturnsOkObjectValue()
        {
            // Arrange
            mockLocationRepo
                .Setup(lr => lr.Retrieve())
                .Returns(new List<Location>());

            // Act
            var result = sut.GetLocations(null);

            // Assert
            mockLocationRepo
                .Verify(lr => lr.Retrieve(), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetLocations_WithExistingLocationId_ReturnsOkObjectValue()
        {

            // Act
            var result = sut.GetLocations(existingLocationId);

            // Assert
            mockLocationRepo
                .Verify(lr => lr.Retrieve(existingLocationId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PostLocation_WithEmptyLocation_ReturnsBadRequest()
        {
            // Arrange
            
            // Act
            var result = sut.PostLocation(null);

            // Assert
            mockLocationService
                .Verify(l => l.Save(Guid.Empty, location), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostLocation_WithValidData_ReturnsNewLocationWithLocationId()
        {
            // Arrange
            mockLocationService
                .Setup(l => l.Save(Guid.Empty, location))
                .Returns(location);

            // Act
            var result = sut.PostLocation(location);

            // Assert
            mockLocationService
                .Verify(l => l.Save(Guid.Empty, location), Times.Once);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void DeleteLocation_WithEmptyLocationId_ReturnsNotFound()
        {
            // Act
            var result = sut.DeleteLocation(emptyLocationId);

            // Assert
            mockLocationRepo
                .Verify(lr => lr.Delete(emptyLocationId), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteLocation_WithExistingLocationId_ReturnsNoContent()
        {
            // Act
            var result = sut.DeleteLocation(existingLocationId);

            // Assert
            mockLocationRepo
                .Verify(lr => lr.Delete(existingLocationId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void PutLocation_WithEmptyLocation_ReturnsBadRequest()
        {
            // Act
            var result = sut.PutLocation(null, existingLocationId);

            // Assert
            mockLocationRepo
                .Verify(lr => lr.Update(existingLocationId, null), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PutLocation_WithExistingLocationAndId_ReturnsOkObjectValue()
        {
            // Act
            var result = sut.PutLocation(location, existingLocationId);

            // Assert
            mockLocationRepo
                .Verify(lr => lr.Update(existingLocationId, location), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchLocation_WithEmptyLocationId_ReturnNotFound()
        {
            // Act
            var result = sut.PatchLocation(patchedLocation, emptyLocationId);

            // Assert
            mockLocationService
                .Verify(v => v.Save(emptyLocationId, location), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
