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
        private Mock<IVenueRepository> mockVenueRepo;
        private Mock<IVenueService> mockVenueService;
        private VenuesController sut;
        private Venue venue;
        private Guid emptyVenueId;
        private Guid existingVenueId;
        private JsonPatchDocument patchedVenue;

        [TestInitialize]
        public void InitializeTest()
        {
            mockVenueRepo = new Mock<IVenueRepository>();
            mockVenueService = new Mock<IVenueService>();
            sut = new VenuesController(mockVenueRepo.Object, mockVenueService.Object);
            venue = new Venue();
            existingVenueId = Guid.NewGuid();
            emptyVenueId = Guid.Empty;
            patchedVenue = new JsonPatchDocument();


            mockVenueRepo
                .Setup(v => v.Retrieve(existingVenueId))
                .Returns(venue);
            mockVenueRepo
                .Setup(v => v.Retrieve(emptyVenueId))
                .Returns<Venue>(null);
        }


        [TestCleanup]
        public void CleanupTest()
        {

        }

        [TestMethod]
        public void GetVenues_WithEmptyVenueId_ReturnsOkObjectValue()
        {
            // Arrange
            mockVenueRepo
                .Setup(cr => cr.Retrieve())
                .Returns(new List<Venue>());

            // Act
            var result = sut.GetVenues(emptyVenueId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetVenues_WithExistingVenueId_ReturnsOkObjectValue()
        {
            // Act
            var result = sut.GetVenues(existingVenueId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateVenue_WithEmptyVenue_ReturnsBadRequest()
        {
            // Act
            var result = sut.CreateVenue(null);

            // Assert
            mockVenueService
                .Verify(v => v.Save(Guid.Empty, venue), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void CreateVenue_WithValidData_ReturnsNewVenueWithVenueId()
        {
            // Arrange
            mockVenueService
                .Setup(v => v.Save(Guid.Empty, venue))
                .Returns(venue);

            // Act
            var result = sut.CreateVenue(venue);

            // Assert
            mockVenueService
                .Verify(v => v.Save(Guid.Empty, venue), Times.Once);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void DeleteVenue_WithEmptyVenueId_ReturnsNotFound()
        {
            // Act 
            var result = sut.DeleteVenue(emptyVenueId);

            // Assert
            mockVenueRepo
                .Verify(v => v.Delete(emptyVenueId), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteVenue_WithExistingVenueId_ReturnsNoContent()
        {
            // Act
            var result = sut.DeleteVenue(existingVenueId);

            // Assert
            mockVenueRepo
                .Verify(v => v.Delete(existingVenueId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void UpdateVenue_WithEmptyVenueId_ReturnsNotFound()
        {
            // Act
            var result = sut.UpdateVenue(venue, emptyVenueId);

            // Assert
            mockVenueRepo
                .Verify(v => v.Update(emptyVenueId, venue), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void UpdateVenue_WithEmptyVenue_ReturnsBadRequest()
        {
            // Act
            var result = sut.UpdateVenue(null, existingVenueId);

            // Assert
            mockVenueRepo
                .Verify(v => v.Update(existingVenueId, null), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateVenue_WithExistingVenueIdAndVenue_ReturnsOkObjectValue()
        {
            // Act
            var result = sut.UpdateVenue(venue, existingVenueId);

            // Assert
            mockVenueRepo
                .Verify(v => v.Update(existingVenueId, venue), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void PatchVenue_WithEmptyPatchedVenue_ReturnsBadRequest()
        {
            // Arrange
            patchedVenue = null;

            // Act
            var result = sut.PatchVenue(patchedVenue, existingVenueId);

            // Assert
            mockVenueRepo
                .Verify(v => v.Retrieve(existingVenueId), Times.Never);
            mockVenueService
                .Verify(v => v.Save(existingVenueId, venue), Times.Never);
        }

        [TestMethod]
        public void PatchVenue_WithEmptyVenueId_ReturnsNotFound()
        {
            // Act
            var result = sut.PatchVenue(patchedVenue, emptyVenueId);

            // Assert
            mockVenueService
                .Verify(v => v.Save(emptyVenueId, venue), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchVenue_WithExistingVenueId_ReturnsOkObjectValue()
        {
            // Act
            var result = sut.PatchVenue(patchedVenue, existingVenueId);

            // Assert
            mockVenueService
                .Verify(v => v.Save(existingVenueId, venue), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
