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
        private Mock<IVenueRepository> mockVenueRepository;
        private Mock<IVenueService> mockVenueService;
        private Venue venue;
        private JsonPatchDocument patchedVenue;
        private Guid existingVenue;
        private Guid nonExistingVenue;
        private VenuesController sut;

        [TestInitialize]
        public void Initialize()
        {
            mockVenueRepository = new Mock<IVenueRepository>();
            mockVenueService = new Mock<IVenueService>();
            sut = new VenuesController(
                mockVenueRepository.Object
                , mockVenueService.Object);

            venue = new Venue();
            patchedVenue = new JsonPatchDocument();

            existingVenue = Guid.NewGuid();
            nonExistingVenue = Guid.Empty;

            mockVenueRepository
                .Setup(c => c.Retrieve(existingVenue))
                .Returns(venue);

            mockVenueRepository
               .Setup(c => c.Retrieve(nonExistingVenue))
               .Returns<Venue>(null);

        }

        [TestMethod]
        public void GetVenues_WithEmptyVenueId_ReturnsOkObjectResult()
        {
            // Arrange
            
            // Act

            var result = sut.GetVenues(null);

            // Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockVenueRepository
                .Verify(c => c.Retrieve()
                , Times.Once);

        }

        [TestMethod]
        public void RetrieveVenues_WithExistingVenueId_ReturnsOkResult()
        {
            // Arrange
            
            existingVenue = Guid.NewGuid();

            // Act

            var result = sut.GetVenues(existingVenue);

            // Assert

            Assert.IsNotInstanceOfType(result, typeof(OkResult));

            mockVenueRepository
                .Verify(c => c.Retrieve(existingVenue)
                ,Times.Once);

        }

        [TestMethod]
        public void SaveVenue_WithValidVenueData_ReturnsCreatedAtActionResult()
        {
            // Arrange
            
            // Act

            var result = sut.CreateVenue(venue);

            // Assert

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));

            mockVenueService
                .Verify(c => c.Save(Guid.Empty, venue),
                Times.Once);

        }
        [TestMethod]
        public void SaveVenue_WithNullVenueData_ReturnsBadRequestResult()
        {
            //Arrange
            
            venue = null;

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
            
            venue.VenueId = nonExistingVenue;

            //Act

            var result = sut.DeleteVenue(nonExistingVenue);

            //Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockVenueRepository
                .Verify(c => c.Delete(venue.VenueId)
                , Times.Never);
        }

        [TestMethod]
        public void DeleteVenue_WithNonExistingVenueId_ReturnsNotFoundResult()
        {
            //Arrange

           venue.VenueId = nonExistingVenue;

            //Act

            var result = sut.DeleteVenue(nonExistingVenue);

            //Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockVenueRepository
                .Verify(c => c.Delete(venue.VenueId)
                , Times.Never);
        }

        [TestMethod]
        public void UpdateVenue_WithExistingVenueId_ReturnsOkObjectResult()
        {
            //Arrange
            
            venue.VenueId = existingVenue;

            //Act

            var result = sut.UpdateVenue(venue, venue.VenueId);

            //Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockVenueService
                .Verify(cs => cs.Save(venue.VenueId, venue)
                , Times.Once);
            mockVenueRepository
                .Verify(cr => cr.Retrieve(venue.VenueId)
                , Times.Once);

        }

        [TestMethod]
        public void UpdateVenue_WithNonExistingVenueId_ReturnsBadRequestResult()
        {
            //Arrange
            
            venue.VenueId = nonExistingVenue;

            //Act

            var result = sut.UpdateVenue(venue, venue.VenueId);

            //Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockVenueRepository
                .Verify(c => c.Retrieve(venue.VenueId)
                , Times.Once);
            mockVenueService
                .Verify(c => c.Save(venue.VenueId, venue)
                , Times.Never);
        }

        [TestMethod]
        public void PatchVenue_WithValidPatchVenueData_ReturnsOkObjectResult()
        {
            //Arrange

            venue.VenueId = existingVenue;

            //Act

            var result = sut.PatchVenue(patchedVenue, venue.VenueId);

            //Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockVenueService
                .Verify(cs => cs.Save(venue.VenueId, venue)
                , Times.Once);

        }

        [TestMethod]
        public void PatchVenue_WithNotValidPatchVenueData_ReturnsBadRequestResult()
        {
            //Arrange
            
            patchedVenue = null;

            //Act

            var result = sut.PatchVenue(patchedVenue, venue.VenueId);

            //Assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockVenueService
                .Verify(cs => cs.Save(venue.VenueId, venue)
                , Times.Never);
        }

        [TestMethod]
        public void PatchVenue_WithNonExistingVenueId_ReturnsNotFoundResult()
        {
            //Arrange
                        
            venue.VenueId = nonExistingVenue;

            //Act

            var result = sut.PatchVenue(patchedVenue, venue.VenueId);

            //Assert

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockVenueService
                .Verify(cs => cs.Save(venue.VenueId, venue), Times.Never);
        }
    }
}
