using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using BlastAsia.DigiBook.Domain.Models.Pagination;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.Api.Test
{
    [TestClass]
    public class AppointmentsControllerTest
    {
        private static Mock<IAppointmentRepository> mockRepo;
        private static Mock<IAppointmentService> mockService;

        private AppointmentsController sut;
        private Appointment appointment;
        private JsonPatchDocument patch;


        private readonly Guid existingId = Guid.NewGuid();
        private readonly Guid nonExistingId = Guid.Empty;


        [TestInitialize]
        public void Initialize()
        {

            appointment = new Appointment
            {
                    
            };

            patch = new JsonPatchDocument();

            mockService = new Mock<IAppointmentService>();
            mockRepo = new Mock<IAppointmentRepository>();


            mockRepo
                .Setup(
                    r => r.Retrieve(existingId)
                )
                .Returns(appointment);

            mockRepo
              .Setup(
                  r => r.Retrieve(nonExistingId)
              )
              .Returns<Appointment>(null);

            mockService
               .Setup(
                   s => s.Save(existingId, appointment)
               )
               .Returns(appointment);


            sut = new AppointmentsController(mockRepo.Object, mockService.Object);
        }
        [TestCleanup]
        public void Cleanup()
        {

        }
        // Get
        [TestMethod]
        public void GetAppointment_WithNoId_ShouldReturnOkObjectValue()
        {
            // arrange

            mockRepo
                .Setup(
                    r => r.Retrieve()
                )
                .Returns(new List<Appointment>());

            // act
            var result = sut.GetAppointment(null);

            // assert

            mockRepo
               .Verify(
                   r => r.Retrieve(), Times.Once()
               );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }
        [TestMethod]
        public void GetAppointment_WithId_ShouldReturnOkObjectValue()
        {
            // arrange
            var id = Guid.NewGuid();
            mockRepo
                .Setup(
                    r => r.Retrieve(id)
                )
                .Returns(new Appointment());

            // act
            var result = sut.GetAppointment(id);
            // assert
            mockRepo
               .Verify(
                   r => r.Retrieve(id), Times.Once()
               );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetAppointment_WithPaginationWithValidData_ShouldReturnOkObjectValue()
        {
            // arrange
            var pageNumber = 1;
            var recordNumber = 3;
            string key = "emem";

            mockRepo
                .Setup(
                    r => r.Retrieve(pageNumber, recordNumber, key)
                )
                .Returns(new Pagination<Appointment>());

            // act 
            var result = sut.GetAppointment(pageNumber, recordNumber, key);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(pageNumber, recordNumber, key), Times.Once
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void GetAppointment_WithPaginationWithInvalidPageNumber_ShouldReturnOkObjectValue()
        {
            // arrange
            var pageNumber = -1;
            var recordNumber = 3;
            string key = "asd";


            mockRepo
                .Setup(
                    r => r.Retrieve(pageNumber, recordNumber, key)
                )
                .Returns(new Pagination<Appointment>());

            // act 
            var result = sut.GetAppointment(pageNumber, recordNumber, key);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(pageNumber, recordNumber, key), Times.Once
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void GetAppointment_WithPaginationWIthInvalidRecordNumber_ShouldReturnOkObjectValue()
        {
            // arrange
            var pageNumber = 1;
            var recordNumber = -3;
            string key = "asd";

            mockRepo
                .Setup(
                    r => r.Retrieve(pageNumber, recordNumber, key)
                )
                .Returns(new Pagination<Appointment>());

            // act 
            var result = sut.GetAppointment(pageNumber, recordNumber, key);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(pageNumber, recordNumber, key), Times.Once
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void GetAppointment_WithPaginationWithInvalidDate_ShouldReturnOkObjectValue()
        {
            // arrange
            var pageNumber = 1;
            var recordNumber = 3;
            string key = null;

            mockRepo
                .Setup(
                    r => r.Retrieve(pageNumber, recordNumber, key)
                )
                .Returns(new Pagination<Appointment>());

            // act 
            var result = sut.GetAppointment(pageNumber, recordNumber, key);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(pageNumber, recordNumber, key), Times.Once
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }


        // Post
        [TestMethod]
        public void CreateAppointment_WithNullAppointment_ShouldReturnBadRequest()
        {
            // arrange
            appointment = null;

            // act 
            var result = sut.CreateAppointment(appointment);

            // assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockService
                .Verify(
                    r => r.Save(Guid.Empty, appointment), Times.Never()
                );
        }

        [TestMethod]
        public void CreateAppointment_WithInvalidAppointment_ShouldReturnBadRequest()
        {
            // arrange
            appointment.AppointmentDate = DateTime.Now.AddDays(-1);

            mockService
                .Setup(
                    s => s.Save(appointment.AppointmentId, appointment)
                )
                .Returns(() => throw new Exception());

            // act
            var result = sut.CreateAppointment(appointment);

            // assert
            mockService
            .Verify(
                r => r.Save(appointment.AppointmentId, appointment), Times.Once()
            );

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }

        [TestMethod]
        public void CreateAppointment_WithvalidAppointment_ShouldReturnOkObjectRsult()
        {
            // arrange
            mockService
               .Setup(
                   s => s.Save(appointment.AppointmentId, appointment)
               )
               .Returns(new Appointment());
            // act
            var result = sut.CreateAppointment(appointment);
            // assert
            mockService
                .Verify(
                    s => s.Save(Guid.Empty, appointment), Times.Once()
                );

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void DeleteAppointment_WithNonExistingAppointmentId_SohouldReturnNotFound()
        {
            // arrange
            // act 
            var result = sut.DeleteAppointment(nonExistingId);

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
        public void DeleteAppointment_WithExistingAppointmentId_SohouldReturnNoContent()
        {
            // arrange
            // act 
            var result = sut.DeleteAppointment(existingId);

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
        public void UpkeyAppointment_WithNullAppointment_ShouldReturnBadRequest()
        {
            // arrange
            appointment = null;
            var id = existingId;

            // act 
            var result = sut.UpdateAppointment(appointment, id);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(id), Times.Never()
                );

            mockService
                .Verify(
                    r => r.Save(id, appointment), Times.Never()
                );

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpkeyAppointment_WithNonExistingAppointmentid_ShouldReturnNotFoundResult()
        {
            // act
            var result = sut.UpdateAppointment(appointment, nonExistingId);
            // assert 
            mockRepo
               .Verify(
                   r => r.Retrieve(nonExistingId), Times.Once()
               );

            mockService
                .Verify(
                    r => r.Save(nonExistingId, appointment), Times.Never()
                );

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void UpkeyAppointment_WithValidData_ShouldReturnOkResult()
        {
            // act
            var result = sut.UpdateAppointment(appointment, existingId);


            // assert 

            mockRepo
               .Verify(
                   r => r.Retrieve(existingId), Times.Once()
            );

            mockService
                .Verify(
                    s => s.Save(existingId, appointment), Times.Once()
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchAppointment_WithNullPatchAppointment_ShouldReturnBadRequest()
        {
            // arrange
            patch = null;

            var id = existingId;

            // act 
            var result = sut.PatchAppointment(patch, id);

            // assert
            mockRepo
                .Verify(
                    r => r.Retrieve(id), Times.Never()
                );

            mockService
                .Verify(
                    r => r.Save(id, appointment), Times.Never()
                );

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }


        [TestMethod]
        public void PatchAppointment_WithNonExistingAppointmentid_ShouldReturnNotFoundResult()
        {
            // act
            var result = sut.PatchAppointment(patch, nonExistingId);
            // assert 
            mockRepo
               .Verify(
                   r => r.Retrieve(nonExistingId), Times.Once()
               );

            mockService
                .Verify(
                    r => r.Save(nonExistingId, appointment), Times.Never()
                );

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchAppointment_WithValidData_ShouldReturnOkResult()
        {
            // act
            var result = sut.PatchAppointment(patch, existingId);
            // assert 

            mockRepo
               .Verify(
                   r => r.Retrieve(existingId), Times.Once()
            );

            mockService
                .Verify(
                    s => s.Save(existingId, appointment), Times.Once()
                );

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }
    }
}


