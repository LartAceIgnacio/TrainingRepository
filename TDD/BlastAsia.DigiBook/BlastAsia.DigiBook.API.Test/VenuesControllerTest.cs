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
        private Mock<IVenueService> mockVenueService;
        private Mock<IVenueRepository> mockVenueRepository;
        private VenuesController sut;
        private Guid existingId;
        private Guid nonexistingId;
        private JsonPatchDocument patchedVenue;


        [TestInitialize]
        public void initialize()
        {
            venue = new Venue
            {

            };
            mockVenueService = new Mock<IVenueService>();
            mockVenueRepository = new Mock<IVenueRepository>();
            sut = new VenuesController(mockVenueService.Object, mockVenueRepository.Object);
            existingId = Guid.NewGuid();
            nonexistingId = Guid.Empty;
            patchedVenue = new JsonPatchDocument();

            mockVenueRepository
                .Setup(vr => vr.Retrieve())
                .Returns(new List<Venue>());

            mockVenueRepository
                .Setup(vr => vr.Retrieve(existingId))
                .Returns(venue);

            mockVenueRepository
                .Setup(vr => vr.Retrieve(nonexistingId))
                .Returns<Venue>(null);
        }


        [TestMethod]
        public void GetVenues_WithNoId_ShouldOkObjectValue()
        {
            //Arrange
            

            //Act
            var result = sut.GetVenues(null);

            //Assert
            Assert.IsInstanceOfType(result,typeof(OkObjectResult));

        }

        [TestMethod]
        public void GetVenues_WithExistingId_ShouldOkObjectValue()
        {
            //Arrange

            //Act
            var result = sut.GetVenues(existingId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void CreateVenue_WithEmptyVenue_ReturnBadRequestResult()
        {
            //Arrange
            venue = null;

            //Act
            var result = sut.CreateVenue(null);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockVenueService
                .Verify(vs => vs.Save(Guid.Empty, venue), Times.Never());
        }

        [TestMethod]
        public void CreateVenue_WithValidVenue_ReturnCreateAtActionResult()
        {
            //Arrange

            //Act
            var result = sut.CreateVenue(venue);

            //Assert
            Assert.IsInstanceOfType(result,typeof(CreatedAtActionResult));

            mockVenueService
                .Verify(vs => vs.Save(Guid.Empty, venue), Times.Once());
        }

        [TestMethod]
        public void DeleteVenue_WithExistingId_ShouldReturnNoContentResult()
        {
            //Arrange

            //Act
            var result = sut.DeleteVenue(existingId);
            //Assert
            mockVenueRepository
                .Verify(vr => vr.Delete(existingId), Times.Once());

            Assert.IsInstanceOfType(result,typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteVenue_WithNoExistingId_ShouldReturnNotFoundResult()
        {
            //Arrange

            //Act
            var result = sut.DeleteVenue(nonexistingId);
            //Assert
            
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockVenueRepository
                .Verify(vr => vr.Delete(nonexistingId), Times.Never());
        }

        [TestMethod]
        public void UpdateVenue_WithNoExistingVenue_ShouldReturnBadRequestResult()
        {
            //Arrange
            venue = null;

            //Act
            var result = sut.UpdateVenue(venue, existingId);

            //Assert
            Assert.IsInstanceOfType(result,typeof(BadRequestResult));

            mockVenueService
                .Verify(vs => vs.Save(existingId, venue),Times.Never());
        }

        [TestMethod]
        public void UpdateVenue_WithNoExistingId_ShouldReturnNotFoundResult()
        {
 
            //Act
            var result = sut.UpdateVenue(venue, nonexistingId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockVenueService
                .Verify(vs => vs.Save(nonexistingId, venue), Times.Never());
        }

        [TestMethod]
        public void UpdateVenue_WithExistingIdAndVenue_ShouldReturnOkObjectResult()
        {

            //Act
            var result = sut.UpdateVenue(venue, existingId);

            //Assert
            mockVenueService
                .Verify(vs => vs.Save(existingId, venue), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchVenue_WithNoExistingPatchVenue_ShouldReturnBadRequestResult()
        {
            //Arrange
            patchedVenue = null;

            //Act
            var result = sut.PatchVenue(patchedVenue,existingId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockVenueService
                .Verify(vs => vs.Save(existingId, venue), Times.Never());
        }

        [TestMethod]
        public void PatchVenue_WithNoExistingVenueId_ShouldReturnNotFoundResult()
        {

            //Act
            var result = sut.PatchVenue(patchedVenue, nonexistingId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockVenueService
                .Verify(vs => vs.Save(nonexistingId, venue), Times.Never());
        }

        [TestMethod]
        public void PatchVenue_WithExistingVenueIdAndPatchVenue_ShouldReturnOkObjectResult()
        {

            //Act
            var result = sut.PatchVenue(patchedVenue, existingId);
            //Assert
            mockVenueService
                .Verify(vs => vs.Save(existingId, venue), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
