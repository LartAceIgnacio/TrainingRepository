using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Venues;
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
    public class VenueControllerTest
    {
        private Mock<IVenueRepository> mockVenueRepository;
        private Mock<IVenueService> mockVenueService;
        private VenueController sut;
        private Venue venue;
        private Guid nonExisting;
        private JsonPatchDocument patch;

        [TestInitialize]
        public void TestInitialize()
        {
            nonExisting = Guid.Empty;
            mockVenueRepository = new Mock<IVenueRepository>();
            mockVenueService = new Mock<IVenueService>();
            sut = new VenueController(mockVenueRepository.Object, mockVenueService.Object);
            patch = new JsonPatchDocument();

            venue = new Venue
            {
                VenueId = Guid.NewGuid(),
                VenueName = "Training Room",
                VenueDescription = "Hello"
            };


            mockVenueRepository
                .Setup(v => v.Retreive())
                .Returns(new List<Venue>());

            mockVenueRepository
                .Setup(v => v.Retrieve(venue.VenueId))
                .Returns(venue);
            
            mockVenueRepository
                .Setup(v => v.Retrieve(nonExisting))
                .Returns<Venue>(null);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        [TestMethod]
        public void GetVenues_WithEmptyId_ReturnsOkObjectValue()
        {
            //Arrange
            //Act
            var result = sut.GetVenues(null);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueRepository
                .Verify(v => v.Retreive(), Times.Once);
        }

        [TestMethod]
        public void GetVenue_WithExistingId_ReturnsOkObjectValue()
        {
            //Arrange
            //Act
            var result = sut.GetVenues(venue.VenueId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueRepository
                .Verify(v => v.Retrieve(venue.VenueId), Times.Once);

        }

        [TestMethod]
        public void CreateVenue_WithEmptyVenue_ReturnsBadRequest()
        {
            //Arrange
            venue = null;
            //Act
            var result = sut.CreateVenues(venue);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockVenueService
                .Verify(v => v.Save(nonExisting, venue), Times.Never);
        }

        [TestMethod]
        public void CreateVenue_WithExistingVenue_ReturnsCreatedAtAction()
        {
            //Arrange
            //Act
            var result = sut.CreateVenues(venue);
            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockVenueService
                .Verify(v => v.Save(nonExisting, venue), Times.Once);
        }

        [TestMethod]
        public void DeleteVenue_WithEmptyId_ReturnsNotFound()
        {
            //Arrange
            //Act
            var result = sut.DeleteVenues(nonExisting);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockVenueRepository
                .Verify(v => v.Retrieve(nonExisting), Times.Once);
            mockVenueRepository
                .Verify(v => v.Delete(venue.VenueId), Times.Never);
        }

        [TestMethod]
        public void DeleteVenue_WithVenueId_ReturnsNoContent()
        {
            //Arrange
            //Act
            var result = sut.DeleteVenues(venue.VenueId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockVenueRepository
                .Verify(v => v.Retrieve(venue.VenueId), Times.Once);
            mockVenueRepository
                .Verify(v => v.Delete(venue.VenueId), Times.Once);
        }

        [TestMethod]
        public void UpdateVenue_WithEmptyVenue_ReturnsBadRequest()
        {
            //Arrange
            venue = null;
            
            //Act
            var result = sut.UpdateVenues(venue, nonExisting);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockVenueService
                .Verify(v => v.Save(nonExisting, venue), Times.Never);

            mockVenueService
                .Verify(v => v.Save(venue.VenueId, venue), Times.Never);
        }

        [TestMethod]
        public void UpdateVenue_WithEmptyVenueId_ReturnsNotFound()
        {
            //Arrange
            //Act
            var result = sut.UpdateVenues(venue, nonExisting);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockVenueRepository
                .Verify(v => v.Retrieve(nonExisting), Times.Once);
            mockVenueService
                .Verify(v => v.Save(venue.VenueId, venue), Times.Never);
        }

        [TestMethod]
        public void UpdateVenue_WithValidData_ReturnsOkObjectResult()
        {
            //Arrange
            //Act
            var result = sut.UpdateVenues(venue, venue.VenueId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueRepository
                .Verify(v => v.Retrieve(venue.VenueId), Times.Once);

            mockVenueService
                .Verify(v => v.Save(venue.VenueId, venue), Times.Once);
        }

        [TestMethod]
        public void PatchVenue_WithEmptyPatchVenue_ReturnsBadRequest()
        {
            //Arrange
            patch = null;
            //Act
            var result = sut.PatchVenues(patch, venue.VenueId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockVenueService
                .Verify(v => v.Save(venue.VenueId, venue), Times.Never);
        }

        [TestMethod]
        public void PatchVenue_WithEmptyVenueId_ReturnsNotFound()
        {
            //Arrange
            //Act
            var result = sut.PatchVenues(patch, nonExisting);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockVenueRepository
                .Verify(v => v.Retrieve(nonExisting), Times.Once);
            mockVenueService
                .Verify(v => v.Save(venue.VenueId, venue), Times.Never);
        }

        [TestMethod]
        public void PatchVenue_WithValidData_ReturnsOkObjectValue()
        {
            //Arrange
            //Act
            var result = sut.PatchVenues(patch, venue.VenueId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueRepository
                .Verify(v => v.Retrieve(venue.VenueId), Times.Once);

            mockVenueService
                .Verify(v => v.Save(venue.VenueId, venue), Times.Once);
        }
    }
}
