using BlastAsia.Digibook.API.Controllers;
using BlastAsia.Digibook.Domain;
using BlastAsia.Digibook.Domain.Models.Venues;
using BlastAsia.Digibook.Domain.Venues;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.API.Test
{
    [TestClass]
    public class VenueControllerTest
    {
        private Mock<IVenueRepository> mockVenueRepository;
        private Mock<IVenueService> mockVenueService;
        private Venue venue;
        private VenueController sut;
        private readonly Guid existingId = Guid.NewGuid();
        private readonly Guid nonExistingId = Guid.Empty;
        private JsonPatchDocument jsonPatchDocument;

        [TestInitialize]
        public void InitializeData()
        {
            mockVenueRepository = new Mock<IVenueRepository>();
            mockVenueService = new Mock<IVenueService>();
            venue = new Venue();
            sut = new VenueController(mockVenueRepository.Object, mockVenueService.Object);
            jsonPatchDocument = new JsonPatchDocument();
        }

        [TestMethod]
        public void GetVenue_WithEmptyVenueID_ReturnsOkObjectResult()
        {
            mockVenueRepository
                .Setup(vr => vr.Retrieve())
                .Returns(new List<Venue>());

            var result = sut.GetVenue(null);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetVenue_WithVenueID_ReturnsOkObjectResult()
        {
            mockVenueRepository
                .Setup(vr => vr.Retrieve(existingId))
                .Returns(venue);

            var result = sut.GetVenue(null);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateVenue_WithValidVenueData_ShouldReturnCreatedAtActionResult()
        {
            venue.VenueId = nonExistingId;

            mockVenueService
                .Setup(vr => vr.Save(venue.VenueId, venue))
                .Returns(venue);

            var result = sut.CreateVenue(venue);

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void CreateVenue_WithNullVenueData_ShouldReturnBadRequestResult()
        {
            venue = null;

            var result = sut.CreateVenue(venue);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void CreateVenue_WithInvalidData_ShouldReturnBadRequestResult()
        {
            venue.VenueName = "";
            mockVenueService
                .Setup(vr => vr.Save(venue.VenueId, venue))
                .Throws(new InvalidStringLenghtException("Venue name is required"));
            var result = sut.CreateVenue(venue);
            mockVenueService
                .Verify(
                    vr => vr.Save(Guid.Empty, venue), Times.Once()
                );
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

        }

        [TestMethod]
        public void DeleteVenue_WithVenueId_ShouldReturnNoContentResult()
        {
            var result = sut.DeleteVenue(existingId);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            mockVenueService
                .Verify(
                    cs => cs.Save(Guid.Empty, venue), Times.Never()
                );
        }

        [TestMethod]
        public void UpdateVenue_WithValidVenue_ShouldReturnOkObjectResult()
        {
            mockVenueRepository
                .Setup(c => c.Retrieve(existingId))
                .Returns(venue);

            var result = sut.UpdateVenue(venue, existingId);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueService
                .Verify(
                    cs => cs.Save(existingId, venue), Times.Once()
                );
        }

        [TestMethod]
        public void UpdateVenue_WithNullVenue_ShouldReturnBadRequestResult()
        {
            venue = null;

            var result = sut.UpdateVenue(venue, existingId);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockVenueService
                .Verify(
                    cs => cs.Save(existingId, venue), Times.Never()
                );
        }

        [TestMethod]
        public void UpdateVenue_WithNonExistingVenue_ShouldReturnNotFoundResult()
        {
            var result = sut.UpdateVenue(venue, Guid.Empty);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockVenueService
                .Verify(
                    cs => cs.Save(Guid.Empty, venue), Times.Never()
                );
        }

        [TestMethod]
        public void PatchVenue_WithValidJsonPatch_ShouldReturnOkObjectResult()
        {
            mockVenueRepository
                .Setup(
                    r => r.Retrieve(existingId)
                )
                .Returns(venue);

            var result = sut.PatchVenue(jsonPatchDocument, existingId);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueService
                .Verify(
                    cs => cs.Save(existingId, venue), Times.Once()
                );
        }

        [TestMethod]
        public void PatchVenue_WithNullJsonPatch_ShouldReturnBadRequestResult()
        {
            jsonPatchDocument = null;
            var result = sut.PatchVenue(jsonPatchDocument, Guid.NewGuid());
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockVenueService
                .Verify(
                    cs => cs.Save(existingId, venue), Times.Never()
                );
        }

        [TestMethod]
        public void PatchVenue_WithNonExistingContact_ShouldReturnNotFoundResult()
        {
            var result = sut.PatchVenue(jsonPatchDocument, Guid.Empty);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockVenueService
                .Verify(
                    cs => cs.Save(existingId, venue), Times.Never()
                );
        }
    }
}
