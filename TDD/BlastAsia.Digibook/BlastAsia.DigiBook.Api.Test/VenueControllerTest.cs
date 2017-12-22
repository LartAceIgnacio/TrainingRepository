using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Models.Venues;
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
        private Venue venue;
        private JsonPatchDocument patch;
        private Mock<IVenueRepository> mockRepo;
        private Mock<IVenueService> mockService;
        private VenueController sut;

        private Guid existingId = Guid.NewGuid();
        private Guid nonExistingId = Guid.Empty;


        [TestInitialize]
        public void Initialize()
        {
            venue = new Venue
            {
                VenueId = new Guid(),
                VenueName = "Name",
                Description = "Desc"
            };

            patch = new JsonPatchDocument();

            mockRepo = new Mock<IVenueRepository>();
            mockService = new Mock<IVenueService>();

            sut = new VenueController(mockRepo.Object, mockService.Object);

            mockRepo
                .Setup(
                    r => r.Retrieve(existingId)
                )
                .Returns(venue);


            mockRepo
                .Setup(
                    r => r.Retrieve(nonExistingId)
                )
                .Returns<Venue>(null);

        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void GetVenue_WithNullId_ShoulReturnOkObjectResult()
        {
            // arrange
            mockRepo
                .Setup(
                    r => r.Retrieve()
                )
                .Returns(new List<Venue>());
            // act 
            var result = sut.GetContact(null);
            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve()
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetVenue_WithExistingId_ShoulReturnOkObjectResult()
        {
            // arrange

            // act 
            var result = sut.GetContact(existingId);
            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(existingId)
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }


        [TestMethod]
        public void Create_WithNullVenue_ShouldReturnBadRequestObjectResult()
        {
            // arrange
            venue = null;
            // act 
            var result = sut.CreateContact(venue);
            // assert

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockService
                .Verify(
                    r => r.Save(Guid.Empty, venue), Times.Never
                );

        }

        [TestMethod]
        public void Create_WithVenue_ShouldReturnCreatedAtActionResult()
        {
            // arrange
            // act 
            var result = sut.CreateContact(venue);
            // assert

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));

            mockService
                .Verify(
                    r => r.Save(Guid.Empty, venue), Times.Once
                );

        }


        [TestMethod]
        public void Delete_WithNonExistingId_ShouldReturnNotFound()
        {
            // arrange
            // act 
            var result = sut.DeleteContact(nonExistingId);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(nonExistingId), Times.Once
                );

            mockRepo
           .Verify(
               r => r.Delete(nonExistingId), Times.Never
           );

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Delete_WithExistingId_ShouldReturnNoCOntent()
        {
            // arrange
            // act 
            var result = sut.DeleteContact(existingId);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(existingId), Times.Once
                );

            mockRepo
           .Verify(
               r => r.Delete(existingId), Times.Once
           );

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }


        [TestMethod]
        public void Update_WithNullVenue_ShouldReturnBadRequest()
        {
            // arrange
            venue = null;
            // act 
            var result = sut.UpdateVenue(venue, existingId);
            // assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockRepo
                .Verify(
                    r => r.Retrieve(existingId), Times.Never()
                );

            mockRepo
                .Verify(
                    r => r.Update(existingId, venue), Times.Never
                );

        }

        [TestMethod]
        public void Update_WithNonExistingId_ShouldReturnNotFound()
        {
            // arrange
            // act 
            var result = sut.UpdateVenue(venue, nonExistingId);
            // assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockRepo
                .Verify(
                    r => r.Retrieve(nonExistingId), Times.Once()
                );

            mockRepo
                .Verify(
                    r => r.Update(nonExistingId, venue), Times.Never
                );

        }

        [TestMethod]
        public void Update_WithValidData_ShouldReturnOkObjectResult()
        {
            // arrange
            // act 
            var result = sut.UpdateVenue(venue, existingId);
            // assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockRepo
                .Verify(
                    r => r.Retrieve(existingId), Times.Once()
                );

            mockService
                .Verify(
                    r => r.Save(existingId, venue), Times.Once
                );

        }

        [TestMethod]
        public void Patch_WithNullVenue_ShouldReturnBadRequest()
        {
            // arrange
            patch = null;
            // act 
            var result = sut.PatchVenue(patch, existingId);
            // assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockRepo
                .Verify(
                    r => r.Retrieve(existingId), Times.Never()
                );

            mockRepo
                .Verify(
                    r => r.Update(existingId, venue), Times.Never
                );

        }

        [TestMethod]
        public void Patch_WithNonExistingId_ShouldReturnBadRequest()
        {
            // arrange
            // act 
            var result = sut.PatchVenue(patch, nonExistingId);
            // assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockRepo
                .Verify(
                    r => r.Retrieve(nonExistingId), Times.Once()
                );

            mockRepo
                .Verify(
                    r => r.Update(nonExistingId, venue), Times.Never
                );

        }

        [TestMethod]
        public void Patch_WithValidData_ShouldReturnOkObjectResult()
        {
            // arrange
            // act 
            var result = sut.PatchVenue(patch, existingId);
            // assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockRepo
                .Verify(
                    r => r.Retrieve(existingId), Times.Once()
                );

            mockService
                .Verify(
                    r => r.Save(existingId, venue), Times.Once
                );

        }
    }
}
