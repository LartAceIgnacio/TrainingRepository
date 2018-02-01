using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Pilots;
using BlastAsia.DigiBook.Domain.Models.Pilots;
using BlastAsia.DigiBook.Domain.Models.Pagination;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;

namespace BlastAsia.DigiBook.Api.Test
{
    [TestClass]
    public class PilotsControllerTest
    {
        private static Mock<IPilotRepository> mockRepo;
        private static Mock<IPilotService> mockService;

        private PilotsController sut;
        private Pilot pilot;
        private JsonPatchDocument patch;


        private readonly Guid existingId = Guid.NewGuid();
        private readonly Guid nonExistingId = Guid.Empty;


        [TestInitialize]
        public void Initialize()
        {

            pilot = new Pilot
            {
                PilotId = Guid.NewGuid(),
                FirstName = "Emmanuel",
                MiddleName = "Pararuan",
                LastName = "Magadia",
                DateOfBirth = DateTime.Now,
                YearsOfExperience = 12,
                DateActivated = DateTime.Now,
                PilotCode = "",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            patch = new JsonPatchDocument();

            mockService = new Mock<IPilotService>();
            mockRepo = new Mock<IPilotRepository>();


            mockRepo
                .Setup(
                    r => r.Retrieve(existingId)
                )
                .Returns(pilot);

            mockRepo
              .Setup(
                  r => r.Retrieve(nonExistingId)
              )
              .Returns<Pilot>(null);

            mockService
               .Setup(
                   s => s.Save(existingId, pilot)
               )
               .Returns(pilot);


            sut = new PilotsController(mockRepo.Object, mockService.Object);
        }
        [TestCleanup]
        public void Cleanup()
        {

        }
        // Get
        [TestMethod]
        public void GetPilot_WithNoId_ShouldReturnOkObjectValue()
        {
            // arrange

            mockRepo
                .Setup(
                    r => r.Retrieve()
                )
                .Returns(new List<Pilot>());

            // act
            var result = sut.GetPilot(null);

            // assert

            mockRepo
               .Verify(
                   r => r.Retrieve(), Times.Once()
               );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }


        [TestMethod]
        public void GetPilot_WithId_ShouldReturnOkObjectValue()
        {
            // arrange
            var id = Guid.NewGuid();
            mockRepo
                .Setup(
                    r => r.Retrieve(id)
                )
                .Returns(new Pilot());

            // act
            var result = sut.GetPilot(id);
            // assert
            mockRepo
               .Verify(
                   r => r.Retrieve(id), Times.Once()
               );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public void GetPilot_WithPaginationWithValidData_ShouldReturnOkObjectValue()
        {
            // arrange
            var pageNumber = 1;
            var recordNumber = 3;
            var keyWord = "em";

            mockRepo
                .Setup(
                    r => r.Retrieve(pageNumber, recordNumber, keyWord)
                )
                .Returns(new Pagination<Pilot>());

            // act 
            var result = sut.GetPilot(pageNumber, recordNumber, keyWord);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(pageNumber, recordNumber, keyWord), Times.Once
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void GetPilot_WithPaginationWithInvalidPageNumber_ShouldReturnOkObjectValue()
        {
            // arrange
            var pageNumber = -1;
            var recordNumber = 3;
            var keyWord = "em";

            mockRepo
                .Setup(
                    r => r.Retrieve(pageNumber, recordNumber, keyWord)
                )
                .Returns(new Pagination<Pilot>());

            // act 
            var result = sut.GetPilot(pageNumber, recordNumber, keyWord);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(pageNumber, recordNumber, keyWord), Times.Once
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void GetPilot_WithPaginationWIthInvalidRecordNumber_ShouldReturnOkObjectValue()
        {
            // arrange
            var pageNumber = 1;
            var recordNumber = -3;
            var keyWord = "em";

            mockRepo
                .Setup(
                    r => r.Retrieve(pageNumber, recordNumber, keyWord)
                )
                .Returns(new Pagination<Pilot>());

            // act 
            var result = sut.GetPilot(pageNumber, recordNumber, keyWord);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(pageNumber, recordNumber, keyWord), Times.Once
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void GetPilot_WithPaginationWithInvalidKeyWord_ShouldReturnOkObjectValue()
        {
            // arrange
            var pageNumber = 1;
            var recordNumber = 3;
            var keyWord = "";

            mockRepo
                .Setup(
                    r => r.Retrieve(pageNumber, recordNumber, keyWord)
                )
                .Returns(new Pagination<Pilot>());

            // act 
            var result = sut.GetPilot(pageNumber, recordNumber, keyWord);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(pageNumber, recordNumber, keyWord), Times.Once
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }
        // Post
        [TestMethod]
        public void CreatePilot_WithNullPilot_ShouldReturnBadRequest()
        {
            // arrange
            pilot = null;

            // act 
            var result = sut.CreatePilot(pilot);

            // assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockService
                .Verify(
                    r => r.Save(Guid.Empty, pilot), Times.Never()
                );
        }

        [TestMethod]
        public void CreatePilot_WithInvalidPilot_ShouldReturnBadRequest()
        {
            // arrange
            pilot.FirstName = "";

            mockService
                .Setup(
                    s => s.Save(pilot.PilotId, pilot)
                )
                .Returns(() => throw new Exception());

            // act
            var result = sut.CreatePilot(pilot);

            // assert
            mockService
            .Verify(
                r => r.Save(pilot.PilotId, pilot), Times.Once()
            );

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

        }

        [TestMethod]
        public void CreatePilot_WithValidPilot_ShouldReturnOkObjectRsult()
        {
            // arrange
            mockService
               .Setup(
                   s => s.Save(pilot.PilotId, pilot)
               )
               .Returns(new Pilot());
            // act
            var result = sut.CreatePilot(pilot);
            // assert
            mockService
                .Verify(
                    s => s.Save(pilot.PilotId, pilot), Times.Once()
                );

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void DeletePilot_WithNonExistingPilotId_SohouldReturnNotFound()
        {
            // arrange
            // act 
            var result = sut.DeletePilot(nonExistingId);

            // assert

            mockRepo
                .Verify(
                    r => r.Retrieve(nonExistingId), Times.Once()
                );

            mockRepo
              .Verify(
                  r => r.Delete(nonExistingId), Times.Never()
              );

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeletePilot_WithExistingPilotId_SohouldReturnNoContent()
        {
            // arrange
            // act 
            var result = sut.DeletePilot(existingId);

            // assert

            mockRepo
                .Verify(
                    r => r.Retrieve(existingId), Times.Once()
                );

            mockRepo
             .Verify(
                 r => r.Delete(existingId), Times.Once()
             );

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }


        [TestMethod]
        public void UpdatePilot_WithNullPilot_ShouldReturnBadRequest()
        {
            // arrange
            pilot = null;
            var id = existingId;

            // act 
            var result = sut.UpdatePilot(pilot, id);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(id), Times.Never()
                );

            mockService
                .Verify(
                    r => r.Save(id, pilot), Times.Never()
                );

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdatePilot_WithNonExistingPilotid_ShouldReturnNotFoundResult()
        {
            // act
            var result = sut.UpdatePilot(pilot, nonExistingId);
            // assert 
            mockRepo
               .Verify(
                   r => r.Retrieve(nonExistingId), Times.Once()
               );

            mockService
                .Verify(
                    r => r.Save(nonExistingId, pilot), Times.Never()
                );

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void UpdatePilot_WithValidData_ShouldReturnOkResult()
        {
            // act
            var result = sut.UpdatePilot(pilot, existingId);


            // assert 

            mockRepo
               .Verify(
                   r => r.Retrieve(existingId), Times.Once()
            );

            mockService
                .Verify(
                    s => s.Save(existingId, pilot), Times.Once()
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchPilot_WithNullPatchPilot_ShouldReturnBadRequest()
        {
            // arrange
            patch = null;

            var id = existingId;

            // act 
            var result = sut.PatchPilot(patch, id);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(id), Times.Never()
                );

            mockService
                .Verify(
                    r => r.Save(id, pilot), Times.Never()
                );

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }


        [TestMethod]
        public void PatchPilot_WithNonExistingPilotid_ShouldReturnNotFoundResult()
        {
            // act
            var result = sut.PatchPilot(patch, nonExistingId);
            // assert 
            mockRepo
               .Verify(
                   r => r.Retrieve(nonExistingId), Times.Once()
               );

            mockService
                .Verify(
                    r => r.Save(nonExistingId, pilot), Times.Never()
                );

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchPilot_WithValidData_ShouldReturnOkResult()
        {
            // act
            var result = sut.PatchPilot(patch, existingId);
            // assert 

            mockRepo
               .Verify(
                   r => r.Retrieve(existingId), Times.Once()
            );

            mockService
                .Verify(
                    s => s.Save(existingId, pilot), Times.Once()
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void SearchPilot_WithValidData_ShouldReturnOkResult()
        {
            // arrange
            var key = "em";
            // act 
            var result = sut.GetPilot(1,2,key);
            // assert
            mockRepo
              .Verify(
                  r => r.Retrieve(1,2,key), Times.Once()
            );
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}


