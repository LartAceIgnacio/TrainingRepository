using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Api.Utils;
using BlastAsia.DigiBook.Domain.Models.Venues;
using BlastAsia.DigiBook.Domain.Venues;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.Api.Test
{
    [TestClass]
    public class VenuesControllerTest
    {
        private Mock<IVenueRepository> mockRepo;
        private Mock<IVenueService> mockService;
        private VenuesController sut;
        private List<Venue> venueList = new List<Venue>();
        private Venue venue;
        private Guid existingId = Guid.NewGuid();
        private Guid nonExistingId = Guid.Empty;
        private JsonPatchDocument patchDoc = new JsonPatchDocument();
       
        //[TestMethod]
        //public void CreateVenue_WithValidVenue_ShouldReturnCreatedAtActionResult()
        //{

        //}
        [TestInitialize]
        public void Initialize()
        {
            mockRepo = new Mock<IVenueRepository>();
            mockService = new Mock<IVenueService>();
            sut = new VenuesController(mockService.Object, mockRepo.Object);
            venue = new Venue
            {
                VenueName = "Venue Name",
                Description = ""
            };

            //ExistingId Returns venue
            mockRepo
                .Setup(r => r.Retrieve(existingId))
                .Returns(venue);
            //NoId Returns VenueList
            mockRepo
              .Setup(r => r.Retrieve())
              .Returns(venueList);

            mockRepo
                .Setup(r => r.Retrieve(nonExistingId))
                .Returns<Venue>(null);

        }
        [TestMethod]
        public void GetVenue_WithoutId_ShouldReturnOkObjectValue()
        {


            //Act
            var result = sut.GetVenues(null);

            //Assert
            mockRepo
                .Verify(r => r.Retrieve(), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetVenue_WithExistingId_ShouldReturnOkObjectValue()
        {
            //Act
            var result = sut.GetVenues(venue.VenueId);

            //Arrange
            mockRepo
                .Verify(r => r.Retrieve(venue.VenueId), Times.Once);
        }

        [TestMethod]
        public void CreateVenue_WithValidData_ShouldReturnCreatedActionResult()
        {
            //act
            var result = sut.CreateVenue(venue);

            //assert
            mockService
                .Verify(r => r.Save(venue.VenueId, venue), Times.Once);

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void CreateVenue_WithNullData_ShouldReturnBadRequest()
        {
            //arrange
            venue = null;

            //act
            var result = sut.CreateVenue(venue);

            //assert
            mockService
                .Verify(s => s.Save(Guid.Empty, venue), Times.Never);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateVenue_WithValidExistingId_ShouldReturnOkObjectValue()
        {
            //arrange
            venue.VenueId = existingId;

            //act
            var result = sut.UpdateVenue(venue.VenueId, venue);

            //assert
            mockRepo
                .Verify(r => r.Retrieve(venue.VenueId), Times.Once);

            mockService
                .Verify(s => s.Save(venue.VenueId, venue), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public void UpdateVenue_WithNonExistingId_ShouldReturnNotFound()
        {
            //arrange
            venue.VenueId = nonExistingId;

            //Act
            var result = sut.UpdateVenue(venue.VenueId, venue);

            //Assert
            mockRepo
                .Verify(r => r.Retrieve(venue.VenueId), Times.Once);

            mockService
                .Verify(s => s.Save(venue.VenueId, venue), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));


        }

        [TestMethod]
        public void DeleteVenue_WithExistingId_ShouldReturnNoContent()
        {
            //Arrange
            venue.VenueId = existingId;

            //act
            var result = sut.DeleteVenue(venue.VenueId, venue);

            //Assert
            mockRepo
              .Verify(r => r.Retrieve(venue.VenueId), Times.Once);

            mockRepo
                .Verify(r => r.Delete(venue.VenueId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));

        }

        [TestMethod]
        public void DeleteVenue_WithNonExistingId_ShouldReturnNotFound()
        {
            //Arrange
            venue.VenueId = nonExistingId;

            //act
            var result = sut.DeleteVenue(venue.VenueId, venue);

            //Assert
            mockRepo
                .Verify(r => r.Retrieve(venue.VenueId), Times.Once);

            mockRepo
                .Verify(r => r.Delete(venue.VenueId), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod]
        public void PatchVenue_WithExistingId_ShouldReturnOkObjectValue()
        {
            //Arrange
            venue.VenueId = existingId;

            //aCt
            var result = sut.PatchVenue(patchDoc, venue.VenueId);

            //Assert
            mockRepo
                .Verify(r => r.Retrieve(venue.VenueId), Times.Once);

            mockService
                .Verify(r => r.Save(venue.VenueId, venue), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));


        }
        [TestMethod]
        public void PatchVenue_WithNonExistingId_ShouldReturnNotFound()
        {
            //Arrange
            venue.VenueId = nonExistingId;

            //Act
            var result = sut.PatchVenue(patchDoc, venue.VenueId);

            //ASsert
            mockRepo
                .Verify(r => r.Retrieve(venue.VenueId), Times.Once);
            mockService
                .Verify(s => s.Save(venue.VenueId, venue), Times.Never);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod]
        public void PatchVenue_WithNullPatchDocument_ShouldReturnBadRequestResult()
        {
            //Arrange
            venue.VenueId = existingId;

            patchDoc = null;
            //aCt
            var result = sut.PatchVenue(patchDoc, venue.VenueId);

            //Assert
            mockRepo
                .Verify(r => r.Retrieve(venue.VenueId), Times.Never);

            mockService
                .Verify(s => s.Save(venue.VenueId, venue), Times.Never);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
