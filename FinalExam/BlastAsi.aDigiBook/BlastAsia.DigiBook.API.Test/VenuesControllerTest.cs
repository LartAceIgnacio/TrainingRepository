using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
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
    public class VenuesControllerTest
    {
        private Venue venue;
        private Mock<IVenueRepository> mockVenueRepository;
        private Mock<IVenueService> mockVenueService;
        private VenuesController sut;
        private Guid existingVenueID = Guid.NewGuid();
        private Guid nonExistingVenueID = Guid.Empty;
        private JsonPatchDocument patchedVenue;

        [TestInitialize]
        public void Initialize()
        {
            venue = new Venue
            {
                VenueId = existingVenueID,
                VenueName = "Orange wall",
                Description = "Must be orange"
            };

            mockVenueRepository = new Mock<IVenueRepository>();
            mockVenueService = new Mock<IVenueService>();
            sut = new VenuesController(mockVenueService.Object, mockVenueRepository.Object);

            patchedVenue = new JsonPatchDocument();
            patchedVenue.Replace("VenueName", "Lobby");

            mockVenueRepository
               .Setup(v => v.Retrieve(existingVenueID))
               .Returns(venue);

            mockVenueService
               .Setup(v => v.Save(existingVenueID, venue))
               .Returns(venue);
        }

        [TestMethod]
        public void GetVenue_WithEmptyId_ShouldReturnOkObject()
        {
            // Arrange
            mockVenueRepository
                .Setup(v => v.Retrieve())
                .Returns(() => new List<Venue>()
                {
                    new Venue()
                });

            // Act
            var result = sut.GetVenue(null);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetVenue_WithExistingId_ShouldReturnOkObjectResult()
        {
            // Arrange

            // Act
            var result = sut.GetVenue(venue.VenueId);

            //Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateVenue_WithValidData_ShoulReturnCreatedAtActionResult()
        {
            // Arrange
            venue.VenueId = nonExistingVenueID;

            mockVenueService
                .Setup(v => v.Save(venue.VenueId,venue))
                .Returns(venue);

            // Act
            var result = sut.CreateVenue(venue);

            // Assert
            mockVenueService.Verify(v => v.Save(venue.VenueId, venue), Times.Once());
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void CreateVenue_WithInvalidData_ShoulReturnBadRequestObjectResult()
        {
            // Arrange
            venue.VenueName = "";
            venue.VenueId = nonExistingVenueID;

            // Act
            var result = sut.CreateVenue(venue);

            // Assert
            mockVenueService.Verify(v => v.Save(venue.VenueId, venue), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void CreateVenue_WithBlankData_ShouldReturnBadRequestResult()
        {
            // Arrange
            venue = null;

            // Act
            var result = sut.CreateVenue(venue);

            // Assert
            mockVenueService.Verify(v => v.Save(existingVenueID, venue), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteVenue_WithExistingId_ReturnsNoContentResult()
        {
            // Arrange

            // Act
            var result = sut.Delete(venue.VenueId);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);
            mockVenueRepository.Verify(v => v.Delete(venue.VenueId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }


        [TestMethod]
        public void DeleteVenue_WithNonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            venue.VenueId = nonExistingVenueID;

            // Act
            var result = sut.Delete(venue.VenueId);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);
            mockVenueRepository.Verify(v => v.Delete(venue.VenueId), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


        [TestMethod]
        public void PatchVenue_WithExistingIdAndValidData_ReturnsOkObjectResult()
        {
            // Arrange

            // Act
            var result = sut.PatchVenue(patchedVenue, venue.VenueId);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);
            mockVenueService.Verify(v => v.Save(venue.VenueId, venue), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchVenue_WithBlankData_ReturnsBadRequestResult()
        {
            // Arrange
            patchedVenue = null;

            // Act
            var result = sut.PatchVenue(patchedVenue, venue.VenueId);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Never);
            mockVenueService.Verify(v => v.Save(venue.VenueId, venue), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PatchVenue_WithNonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            venue.VenueId = nonExistingVenueID;

            // Act
            var result = sut.PatchVenue(patchedVenue, venue.VenueId);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);
            mockVenueService.Verify(v => v.Save(venue.VenueId,venue), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


        [TestMethod]
        public void PatchVenue_WithInvalidData_ReturnsBadRequestObjectResult()
        {
            // Arrange
            patchedVenue.Replace("VenueName", "");

            mockVenueService
               .Setup(c => c.Save(venue.VenueId, venue))
               .Throws(new VenueNameRequiredException());

            // Act
            var result = sut.PatchVenue(patchedVenue, venue.VenueId);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);
            mockVenueService.Verify(v => v.Save(venue.VenueId, venue), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }


        [TestMethod]
        public void UpdateVenue_WithExistingIdAndValidData_ReturnsOkObjectResult()
        {
            // Arrange

            ///Act
            var result = sut.UpdateVenue(venue, venue.VenueId);

            //Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);
            mockVenueService.Verify(v => v.Save(venue.VenueId, venue), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }


        [TestMethod]
        public void UpdateVenue_WithNonExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            venue.VenueId = nonExistingVenueID;

            // Act
            var result = sut.UpdateVenue(venue, venue.VenueId);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);
            mockVenueService.Verify(v => v.Save(venue.VenueId, venue), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


        [TestMethod]
        public void UpdateVenue_WithBlankData_ReturnsBadRequestResult()
        {
            // Arrange
            patchedVenue = null;

            // Act
            var result = sut.PatchVenue(patchedVenue, existingVenueID);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(existingVenueID), Times.Never);
            mockVenueService.Verify(v => v.Save(existingVenueID, venue), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }


        [TestMethod]
        public void UpdateVenue_WithInvalidData_ReturnsBadRequestObjectResult()
        {
            // Arrange
            venue.VenueName = "";

            mockVenueService
                .Setup(v => v.Save(venue.VenueId, venue))
                .Throws<VenueNameRequiredException>();

            // Act
            var result = sut.UpdateVenue(venue, venue.VenueId);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);
            mockVenueService.Verify(v => v.Save(venue.VenueId, venue), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}
