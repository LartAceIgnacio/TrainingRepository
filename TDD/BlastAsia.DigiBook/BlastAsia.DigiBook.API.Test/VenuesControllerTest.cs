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
        private JsonPatchDocument patchedVenue;

        [TestInitialize]
        public void Initialize()
        {
            venue = new Venue
            {
                VenueName = "VenueVenueVenueVenueVenueVenueVenueVenueVenueVenue Venue",
                Description = "Description"
            };

            mockVenueRepository = new Mock<IVenueRepository>();
            mockVenueService = new Mock<IVenueService>();

            sut = new VenuesController(mockVenueRepository.Object, mockVenueService.Object);

            patchedVenue = new JsonPatchDocument();
            patchedVenue.Replace("Description", "Say something about the venue");

            mockVenueRepository
                .Setup(v => v.Retrieve(venue.VenueId))
                .Returns(venue);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void GetVenues_WithoutVenueId_ShouldReturnOkObjectResult()
        {
            // Act
            var result = sut.GetVenues(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueRepository.Verify(v => v.Retrieve(), Times.Once);

        }

        [TestMethod]
        public void GetVenues_WithVenueId_ShouldReturnOkObjectResult()
        {
            // Act
            var result = sut.GetVenues(venue.VenueId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);
        }

        [TestMethod]
        public void CreateVenue_VenueWithValidData_ShouldReturnCreatedAtActionResult()
        {
            // Act
            var result = sut.CreateVenue(venue);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockVenueService.Verify(v => v.Save(venue.VenueId, venue), Times.Once);
        }

        [TestMethod]
        public void CreateVenue_VenueWithNoData_ShouldReturnBadRequestResult()
        {
            // Arrange
            venue = null;

            // Act
            var result = sut.CreateVenue(venue);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockVenueService.Verify(v => v.Save(Guid.Empty, venue), Times.Never);
        }

        [TestMethod]
        public void DeleteVenue_WithVenueId_ShouldReturnNoContentResult()
        {
            // Act
            var result = sut.DeleteVenue(venue.VenueId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockVenueRepository.Verify(v => v.Delete(venue.VenueId), Times.Once);
        }

        [TestMethod]
        public void DeleteVenue_WithoutVenueId_ShouldReturnNotFoundResult()
        {
            // Arrange
            mockVenueRepository
                .Setup(v => v.Retrieve(venue.VenueId))
                .Returns<Venue>(null);

            // Act
            var result = sut.DeleteVenue(venue.VenueId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockVenueRepository.Verify(v => v.Delete(venue.VenueId), Times.Never);
        }

        [TestMethod]
        public void UpdateVenue_WithExistingDataAndId_ShouldReturnOkObjectResult()
        {
            // Act
            var result = sut.UpdateVenue(venue, venue.VenueId);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);
            mockVenueService.Verify(v => v.Save(venue.VenueId, venue), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateVenue_VenueWithoutValue_ShouldReturnBadRequestResult()
        {
            // Arrange
            venue = null;

            // Act
            var result = sut.UpdateVenue(venue, Guid.Empty);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(Guid.Empty), Times.Never);
            mockVenueService.Verify(v => v.Save(Guid.Empty, venue), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }

        [TestMethod]
        public void UpdateVenue_WithNoExistingVenueId_ShouldReturnNotFoundResult()
        {
            // Arrange
            venue.VenueId = Guid.Empty;
            mockVenueRepository
                .Setup(v => v.Retrieve(venue.VenueId))
                .Returns<Venue>(null);

            // Act
            var result = sut.UpdateVenue(venue, venue.VenueId);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);
            mockVenueService.Verify(v => v.Save(venue.VenueId, venue), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchAppointment_WithExistingAppointmentDataAndId_ShouldReturnOkObjectResult()
        {
            // Act
            var result = sut.PatchVenue(patchedVenue, venue.VenueId);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);
            mockVenueService.Verify(v => v.Save(venue.VenueId, venue), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchAppointment_WithoutExistingAppointmentDataAndId_ShouldReturnNotFoundResult()
        {
            // Arrange
            mockVenueRepository
                .Setup(v => v.Retrieve(venue.VenueId))
                .Returns<Venue>(null);

            // Act
            var result = sut.PatchVenue(patchedVenue, venue.VenueId);

            // Assert
            mockVenueRepository.Verify(v => v.Retrieve(venue.VenueId), Times.Once);
            mockVenueService.Verify(v => v.Save(venue.VenueId, venue), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod]
        public void PatchAppointment_WithEmptyPatchDocument_ShouldReturnBadRequestResult()
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
    }
}
