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
        private Mock<IVenueRepository> mockVenueRepository;
        private Mock<IVenueService> mockVenueService;
        private Venue venue;
        private VenuesController sut;
        private JsonPatchDocument patchedVenue;

        [TestInitialize]
        public void Initialize()
        {
            venue = new Venue {
                VenueId = Guid.NewGuid(),
                VenueName = "Home",
                Description = "Family Meeting"
            };

            patchedVenue = new JsonPatchDocument();

            mockVenueService = new Mock<IVenueService>();
            mockVenueRepository = new Mock<IVenueRepository>();

            sut = new VenuesController(mockVenueRepository.Object, mockVenueService.Object);

            mockVenueRepository.Setup(x => x.Retrieve())
                .Returns(() => new List<Venue>());

            mockVenueRepository.Setup(x => x.Retrieve(venue.VenueId))
                .Returns(venue);
        }


        [TestCleanup]
        public void TestCleanup()
        {

        }

        [TestMethod]
        public void GetVenues_WithEmptyVenueId_ReturnsOkObjectResult()
        {
            //Arrange


            //Act
            var result = sut.GetVenues(null);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueRepository.Verify(x => x.Retrieve(), Times.Once);
        }

        [TestMethod]
        public void GetVenues_WithExistingVenueId_ReturnsOkObjectResult()
        {
            //Arrange


            //Act
            var result = sut.GetVenues(venue.VenueId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueRepository.Verify(x => x.Retrieve(venue.VenueId), Times.Once);
        }
        
        [TestMethod]
        public void CreateVenue_WithEmptyVenue_ReturnsBadRequestResult()
        {
            //Arrange
            venue = null;

            //Act
            var result = sut.CreateVenue(venue);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockVenueService.Verify(x => x.Save(Guid.Empty, venue), Times.Never);
        }

        [TestMethod]
        public void CreateVenue_WithValidVenue_ReturnsCreatedAtActionResult()
        {
            //Arrange


            //Act
            var result = sut.CreateVenue(venue);

            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockVenueService.Verify(x => x.Save(Guid.Empty, venue), Times.Once);

        }


        [TestMethod]
        public void CreateVenue_WithFieldThatThrowsException_ReturnsBadRequestResult()
        {
            //Arrange
            venue.VenueName = "";

            mockVenueService
                .Setup(x => x.Save(Guid.Empty, venue))
                .Throws(new Exception());

            //Act
            var result = sut.CreateVenue(venue);

            //Assert
            mockVenueService.Verify(x => x.Save(Guid.Empty, venue), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteVenue_WithNonExistingVenueId_ReturnsNotFoundResult()
        {
            //Arrange
            venue.VenueId = Guid.Empty;

            //Act
            var result = sut.DeleteVenue(venue.VenueId);

            //Assert
            mockVenueRepository.Verify(x => x.Delete(venue.VenueId), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


        [TestMethod]
        public void DeleteVenue_WithExistingVenueId_ReturnsNoContentResult()
        {
            //Arrange


            //Act
            var result = sut.DeleteVenue(venue.VenueId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockVenueRepository.Verify(x => x.Delete(venue.VenueId), Times.Once);

        }

        [TestMethod]
        public void UpdateVenue_WithEmptyVenue_ReturnsBadRequestResult()
        {
            //Arrange
            venue = null;

            //Act
            var result = sut.UpdateVenue(venue, Guid.Empty);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockVenueRepository.Verify(x => x.Retrieve(Guid.Empty), Times.Never);
            mockVenueService.Verify(x => x.Save(Guid.Empty, venue), Times.Never);
        }

        [TestMethod]
        public void UpdateVenue_WithVenueButNonExistingVenueId_ReturnsNotFoundResult()
        {
            //Arrange
            venue.VenueId = Guid.Empty;

            //Act
            var result = sut.UpdateVenue(venue, venue.VenueId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockVenueRepository.Verify(x => x.Retrieve(venue.VenueId), Times.Once);
            mockVenueService.Verify(x => x.Save(Guid.NewGuid(), venue), Times.Never);
        }

        [TestMethod]
        public void UpdateVenue_WithVenueAndExistingVenueId_ReturnsOkObjectResult()
        {
            //Arrange


            //Act
            var result = sut.UpdateVenue(venue, venue.VenueId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueRepository.Verify(x => x.Retrieve(venue.VenueId), Times.Once);
            mockVenueService.Verify(x => x.Save(venue.VenueId, venue), Times.Once);
        }

        [TestMethod]
        public void UpdateVenue_WithFieldThatThrowsException_ReturnsBadRequestResult()
        {
            //Arrange
            venue.VenueName = "";

            mockVenueService
                .Setup(x => x.Save(venue.VenueId, venue))
                .Throws(new Exception());


            //Act
            var result = sut.UpdateVenue(venue, venue.VenueId);

            //Assert
            mockVenueService.Verify(x => x.Save(venue.VenueId, venue), Times.Once);
            mockVenueRepository.Verify(x => x.Retrieve(venue.VenueId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PatchVenue_WithEmptyPacthedVenue_ReturnsBadRequestResult()
        {
            //Arrange
            patchedVenue = null;

            //Act
            var result = sut.PatchVenue(patchedVenue, Guid.Empty);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockVenueRepository.Verify(x => x.Retrieve(Guid.NewGuid()), Times.Never);
            mockVenueService.Verify(x => x.Save(Guid.NewGuid(), venue), Times.Never);
        }

        [TestMethod]
        public void PatchVenue_WithPatchedVenueButNonExistingVenueId_ReturnsNotFoundResult()
        {
            //Arrange

            //Act
            var result = sut.PatchVenue(patchedVenue, Guid.Empty);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockVenueRepository.Verify(x => x.Retrieve(Guid.Empty), Times.Once);
            mockVenueService.Verify(x => x.Save(Guid.NewGuid(), venue), Times.Never);
        }

        [TestMethod]
        public void PatchVenue_WithPatchedVenueAndExistingVenueId_ReturnsOkObjectResult()
        {
            //Arrange


            //Act
            var result = sut.PatchVenue(patchedVenue, venue.VenueId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockVenueRepository.Verify(x => x.Retrieve(venue.VenueId), Times.Once);
            mockVenueService.Verify(x => x.Save(venue.VenueId, venue), Times.Once);
        }

        [TestMethod]
        public void PatchVenue_WithFieldThatThrowsException_ReturnsBadRequestResult()
        {
            //Arrange
            patchedVenue.Replace("VenueName", "");

            mockVenueService
                .Setup(x => x.Save(venue.VenueId, venue))
                .Throws(new Exception());
            
            //Act
            var result = sut.PatchVenue(patchedVenue, venue.VenueId);

            //Assert
            mockVenueService.Verify(x => x.Save(venue.VenueId, venue), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
