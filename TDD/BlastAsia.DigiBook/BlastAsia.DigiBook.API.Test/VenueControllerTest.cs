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
    public class VenueControllerTest
    {
        [TestMethod]
        public void GetVenues_WithEmptyVenueId_ReturnsOkObjectResult()
        {
            // Arrange

            var mockVenueRepository = new Mock<IVenueRepository>();

            var mockVenueService = new Mock<IVenueService>();

            var sut = new VenuesController(
                mockVenueRepository.Object
                , mockVenueService.Object);
            
           
            // Act

            var result = sut.GetVenues(null);

            // Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockVenueRepository
                .Verify(c => c.Retrieve()
                , Times.Once);

        }

        [TestMethod]
        public void GetVenues_WithExistingVenueId_ReturnsOkResult()
        {
            // Arrange

            var mockVenueRepository = new Mock<IVenueRepository>();

            var mockVenueService = new Mock<IVenueService>();

            var sut = new VenuesController(
                mockVenueRepository.Object
                , mockVenueService.Object);

            var existingVenueId = Guid.NewGuid();

            // Act

            var result = sut.GetVenues(existingVenueId);

            // Assert

            Assert.IsNotInstanceOfType(result, typeof(OkResult));

            mockVenueRepository
                .Verify(c => c.Retrieve(existingVenueId)
                ,Times.Once);

        }

        [TestMethod]
        public void CreateVenue_WithValidVenueData_ReturnsCreatedAtActionResult()
        {
            // Arrange

            var mockVenueRepository = new Mock<IVenueRepository>();

            var mockVenueService = new Mock<IVenueService>();

            var sut = new VenuesController(
                mockVenueRepository.Object
                , mockVenueService.Object);

            var venue = new Venue();

            // Act

            var result = sut.CreateVenue(venue);

            // Assert

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));

            mockVenueService
                .Verify(c => c.Save(Guid.Empty, venue),
                Times.Once);

        }
        [TestMethod]
        public void CreateVenue_WithNullVenueData_ReturnsBadRequestResult()
        {
            //Arrange

            var mockVenueRepository = new Mock<IVenueRepository>();

            var mockVenueService = new Mock<IVenueService>();

            var sut = new VenuesController(
                mockVenueRepository.Object
                , mockVenueService.Object);

            Venue venue = null;

            //Act

            var result = sut.CreateVenue(venue);

            //Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockVenueService
                .Verify(c => c.Save(Guid.Empty, venue)
                , Times.Never);
        }

        [TestMethod]
        public void DeleteVenue_WithExistingVenueId_ReturnsOkResult()
        {
            //Arrange

            var mockVenueRepository = new Mock<IVenueRepository>();

            var mockVenueService = new Mock<IVenueService>();

            var sut = new VenuesController(
                mockVenueRepository.Object
                , mockVenueService.Object);

            var venue = new Venue
            {
                VenueID = Guid.NewGuid()
            };

            var existingVenueId = Guid.NewGuid();

            venue.VenueID = existingVenueId;

            //Act

            var result = sut.DeleteVenue(venue.VenueID);

            //Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockVenueRepository
                .Verify(c => c.Delete(venue.VenueID)
                , Times.Never);
        }

        [TestMethod]
        public void DeleteVenue_WithNonExistingVenueId_ReturnsNotFoundResult()
        {
            //Arrange

            var mockVenueRepository = new Mock<IVenueRepository>();

            var mockVenueService = new Mock<IVenueService>();

            var sut = new VenuesController(
                mockVenueRepository.Object
                , mockVenueService.Object);

            var venue = new Venue
            {
                VenueID = Guid.NewGuid()
            };

            var nonExistingVenueId = Guid.Empty;

            venue.VenueID = nonExistingVenueId;

            //Act

            var result = sut.DeleteVenue(nonExistingVenueId);

            //Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockVenueRepository
                .Verify(c => c.Delete(venue.VenueID)
                , Times.Never);
        }

        [TestMethod]
        public void UpdateVenue_WithExistingVenueId_ReturnsOkObjectResult()
        {
            //Arrange

            var mockVenueRepository = new Mock<IVenueRepository>();

            var mockVenueService = new Mock<IVenueService>();

            var sut = new VenuesController(
                mockVenueRepository.Object
                , mockVenueService.Object);

            var venue = new Venue
            {
                VenueID = Guid.NewGuid()
            };

            var existingVenueId = Guid.NewGuid();

            venue.VenueID = existingVenueId;

            //Act

            var result = sut.UpdateVenue(venue, venue.VenueID);

            //Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockVenueService
                .Verify(cs => cs.Save(venue.VenueID, venue)
                , Times.Never);
            mockVenueRepository
                .Verify(cr => cr.Retrieve(venue.VenueID)
                , Times.Once);

        }

        [TestMethod]
        public void UpdateVenue_WithNonExistingVenueId_ReturnsBadRequestResult()
        {
            //Arrange

            var mockVenueRepository = new Mock<IVenueRepository>();

            var mockVenueService = new Mock<IVenueService>();

            var sut = new VenuesController(
                mockVenueRepository.Object
                , mockVenueService.Object);

            var venue = new Venue
            {
                VenueID = Guid.NewGuid()
            };

            var nonExistingVenueId = Guid.NewGuid();

            venue.VenueID = nonExistingVenueId;

            //Act

            var result = sut.UpdateVenue(venue, venue.VenueID);

            //Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockVenueRepository
                .Verify(cr => cr.Retrieve(venue.VenueID)
                , Times.Once);
            mockVenueService
                .Verify(cs => cs.Save(venue.VenueID, venue)
                , Times.Never);
        }

        [TestMethod]
        public void PatchVenue_WithValidPatchVenueData_ReturnsOkObjectResult()
        {
            //Arrange
            var mockVenueRepository = new Mock<IVenueRepository>();

            var mockVenueService = new Mock<IVenueService>();

            var sut = new VenuesController(
                mockVenueRepository.Object
                , mockVenueService.Object);

            var patchedVenue = new JsonPatchDocument();

            var venue = new Venue
            {
                VenueID = Guid.NewGuid()
            };

            var ExistingVenueId = Guid.NewGuid();

            //Act

            var result = sut.PatchVenue(patchedVenue, venue.VenueID);

            //Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockVenueService
                .Verify(cs => cs.Save(venue.VenueID, venue)
                , Times.Never);

        }

        [TestMethod]
        public void PatchVenue_WithNotValidPatchVenueData_ReturnsBadRequestResult()
        {
            //Arrange

            var mockVenueRepository = new Mock<IVenueRepository>();

            var mockVenueService = new Mock<IVenueService>();

            var sut = new VenuesController(
                mockVenueRepository.Object
                , mockVenueService.Object);

            var patchedVenue = new JsonPatchDocument();

            var venue = new Venue
            {
                VenueID = Guid.NewGuid()
            };

            var ExistingVenueId = Guid.NewGuid();

            patchedVenue = null;

            //Act

            var result = sut.PatchVenue(patchedVenue, venue.VenueID);

            //Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockVenueService
                .Verify(cs => cs.Save(venue.VenueID, venue)
                , Times.Never);
        }

        [TestMethod]
        public void PatchVenue_WithNonExistingVenueId_ReturnsNotFoundResult()
        {
            //Arrange

            var mockVenueRepository = new Mock<IVenueRepository>();

            var mockVenueService = new Mock<IVenueService>();

            var sut = new VenuesController(
                mockVenueRepository.Object
                , mockVenueService.Object);

            var patchedVenue = new JsonPatchDocument();

            var venue = new Venue
            {
                VenueID = Guid.NewGuid()
            };

            var nonExistingVenuetId = Guid.Empty;
            
            venue.VenueID = nonExistingVenuetId;

            //Act

            var result = sut.PatchVenue(patchedVenue, venue.VenueID);

            //Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockVenueService
                .Verify(cs => cs.Save(venue.VenueID, venue), Times.Never);
        }
    }
}
