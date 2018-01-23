using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
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
    public class AppointmentsControllerTest
    {
        private Mock<IAppointmentService> mockAppointmentService;
        private Mock<IAppointmentRepository> mockAppointmentRepository;
        AppointmentsController sut;
        private Guid newAppointmentId;
        private Guid noAppointmentId;
        private Appointment appointment;
        private JsonPatchDocument patchedAppointment;

        [TestInitialize]
        public void AppointmentInitialize()
        {
            appointment = new Appointment
            {

            };

            mockAppointmentService = new Mock<IAppointmentService>();
            mockAppointmentRepository = new Mock<IAppointmentRepository>();
            sut = new AppointmentsController(mockAppointmentService.Object, mockAppointmentRepository.Object);
            newAppointmentId = Guid.NewGuid();
            noAppointmentId = Guid.Empty;
            patchedAppointment = new JsonPatchDocument();

            mockAppointmentRepository
                .Setup(cr => cr.Retrieve(newAppointmentId))
                .Returns(appointment);

            mockAppointmentRepository
                .Setup(cr => cr.Retrieve(noAppointmentId))
                .Returns<Appointment>(null);

        }

        [TestMethod]
        public void GetAppointment_WithNoId_ShouldReturnOkObjectValue()
        {
            //Arrange
            mockAppointmentRepository
                .Setup(cr => cr.Retrieve())
                .Returns(new List<Appointment>());


            //Act
            var result = sut.GetAppointments(null);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetAppointment_WithExistingId_ShouldReturnOkObjectValue()
        {
            //Arrange

            mockAppointmentRepository
                .Setup(cr => cr.Retrieve(newAppointmentId))
                .Returns(appointment);


            //Act
            var result = sut.GetAppointments(newAppointmentId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateAppointment_WithEmptyAppointment_ReturnBadRequestResult()
        {
            //Arrange
            Appointment appointment = null;

            //Act
            var result = sut.CreateAppointment(null);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockAppointmentService
                .Verify(cr => cr.Save(Guid.Empty, appointment), Times.Never());
        }

        [TestMethod]
        public void CreateAppointment_WithValidAppointment_ReturnCreatedAtActionResult()
        {
            ;

            //Act
            var result = sut.CreateAppointment(appointment);

            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));

            mockAppointmentService
                .Verify(cr => cr.Save(Guid.Empty, appointment), Times.Once());
        }


        [TestMethod]
        public void DeleteAppointment_WithExistingId_ShouldReturnNoContentResult()
        {
            //Arrange
            mockAppointmentRepository
                .Setup(cr => cr.Retrieve(newAppointmentId))
                .Returns(appointment);

            //Act
            var result = sut.DeleteAppointment(newAppointmentId);


            //Assert
            mockAppointmentRepository
                .Verify(cr => cr.Delete(newAppointmentId), Times.Once());

            Assert.IsInstanceOfType(result, typeof(NoContentResult));


        }

        [TestMethod]
        public void DeleteAppointment_WithNotExistingId_ShouldReturnNotFoundResult()
        {
            //Arrange
            mockAppointmentRepository
                .Setup(cr => cr.Retrieve(noAppointmentId))
                .Returns<Appointment>(null);

            //Act
            var result = sut.DeleteAppointment(noAppointmentId);


            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockAppointmentRepository
                .Verify(cr => cr.Delete(newAppointmentId), Times.Never());




        }

        [TestMethod]
        public void UpdateAppointment_WithNoExistingAppointment_ShouldReturnBadRequestResult()
        {
            //Arrange
            appointment = null;

            //Act
            var result = sut.UpdateAppointment(appointment, newAppointmentId);

            //Arrange
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockAppointmentService
                .Verify(cr => cr.Save(newAppointmentId, appointment), Times.Never());
        }

        [TestMethod]
        public void UpdateAppointment_WithNoExistingId_ShouldReturnNotFoundResult()
        {
            //Arrange


            //Act
            var result = sut.UpdateAppointment(appointment, noAppointmentId);

            //Arrange
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockAppointmentService
                .Verify(cr => cr.Save(noAppointmentId, appointment), Times.Never());
        }

        [TestMethod]
        public void UpdateAppointment_WithExistingAppointmentAndId_ShouldReturnOkObjectResult()
        {


            //Act
            var result = sut.UpdateAppointment(appointment, newAppointmentId);

            //Arrange
            mockAppointmentService
                .Verify(cr => cr.Save(newAppointmentId, appointment), Times.Once());


            Assert.IsInstanceOfType(result, typeof(OkObjectResult));


        }

        [TestMethod]
        public void PatchAppointment_WithNoExistingPatchAppointment_ShouldReturnBadRequestResult()
        {
            //Arrage
            patchedAppointment = null;
            //Act
            var result = sut.PatchAppointment(patchedAppointment, newAppointmentId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

            mockAppointmentService
                .Verify(cr => cr.Save(newAppointmentId, appointment), Times.Never());


        }

        [TestMethod]
        public void PatchAppointment_WithNoExistingId_ShouldReturnNotFoundResult()
        {
            //Arrage

            //Act
            var result = sut.PatchAppointment(patchedAppointment, noAppointmentId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            mockAppointmentService
                .Verify(cr => cr.Save(noAppointmentId, appointment), Times.Never());


        }

        [TestMethod]
        public void PatchAppointment_WithExistingPactchAppointmentAndId_ShouldReturnOkObjectResult()
        {

            //Act
            var result = sut.PatchAppointment(patchedAppointment, newAppointmentId);
            //Assert
            mockAppointmentService
                .Verify(cr => cr.Save(newAppointmentId, appointment), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));




        }
    }
}
