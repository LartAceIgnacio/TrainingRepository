using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace BlastAsia.DigiBook.Api.Test
{
    [TestClass]
    public class AppointmentsControllerTest
    {
        private Mock<IAppointmentRepository> mockAppointmentRepository;
        private Mock<IAppointmentService> mockAppointmentService;
        private AppointmentsController sut;
        private Appointment appointment;
        private Object result;
        private JsonPatchDocument patchedAppointment;
        private Guid existingAppointmentId;
        private Guid nonExistingAppointmentId;

        [TestInitialize]
        public void Initialize()
        {
            mockAppointmentRepository = new Mock<IAppointmentRepository>();
            mockAppointmentService = new Mock<IAppointmentService>();
            sut = new AppointmentsController(mockAppointmentRepository.Object, mockAppointmentService.Object);

            appointment = new Appointment();
            patchedAppointment = new JsonPatchDocument();

            existingAppointmentId = Guid.NewGuid();
            nonExistingAppointmentId = Guid.Empty;

            mockAppointmentRepository
                .Setup(cr => cr.Retrieve(existingAppointmentId))
                .Returns(appointment);

            mockAppointmentRepository
               .Setup(cr => cr.Retrieve(nonExistingAppointmentId))
               .Returns<Appointment>(null);
        }

        [TestMethod]
        public void GetAppointments_WithEmptyAppointmentId_ReturnsOkObjectResult()
        {
            //Arrange
            //Act
            result = sut.GetAppointments(null);

            //Assert
            mockAppointmentRepository
                .Verify(c => c.Retrieve(), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            //Assert.ReferenceEquals

        }

        [TestMethod]
        public void GetAppointments_WithExistingAppointmentId_ReturnsOkObjectResutl()
        {
            //Arrange
            var existingAppointmentId = Guid.NewGuid();
            //Act
            result = sut.GetAppointments(existingAppointmentId, 1, 10);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            mockAppointmentRepository
                .Verify(c => c.Retrieve(existingAppointmentId), Times.Once);

        }

        [TestMethod]
        public void CreateAppointment_WithValidAppointmentData_ReturnsOkObjectResult()
        {
            //Arrange
            appointment = new Appointment();
            //Act
            result = sut.CreateAppointment(appointment);
            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockAppointmentService
                .Verify(c => c.Save(Guid.Empty, appointment), Times.Once);
        }
        [TestMethod]
        public void CreateAppointment_WithNullAppointmentData_ReturnsBadRequestObjectResult()
        {
            //Arrange
            appointment = null;
            //Act
            result = sut.CreateAppointment(appointment);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockAppointmentService
                .Verify(c => c.Save(Guid.Empty, appointment), Times.Never);
        }

        [TestMethod]
        public void DeleteAppointment_WithExistingAppointmentId_ReturnsOkResult()
        {
            //Arrange
            appointment.AppointmentId = existingAppointmentId;
            //Setup AppointmentRepository

            //Act
            result = sut.DeleteAppointment(appointment.AppointmentId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            mockAppointmentRepository
                .Verify(c => c.Delete(appointment.AppointmentId), Times.Once);
        }

        [TestMethod]
        public void DeleteAppointment_WithNonExistingAppointmentId_ReturnsNotFoundResult()
        {
            //Arrange
            appointment.AppointmentId = nonExistingAppointmentId;

            //Act
            result = sut.DeleteAppointment(nonExistingAppointmentId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockAppointmentRepository
                .Verify(c => c.Delete(appointment.AppointmentId), Times.Never);
        }

        [TestMethod]
        public void UpdateAppointment_WithExistingAppointmentId_ReturnsOkResult()
        {
            //Arrange
            appointment.AppointmentId = existingAppointmentId;

            //Act
            result = sut.UpdateAppointment(appointment, appointment.AppointmentId);

            //Assert
            mockAppointmentService
                .Verify(cs => cs.Save(appointment.AppointmentId, appointment), Times.Once);
            mockAppointmentRepository
                .Verify(cr => cr.Retrieve(appointment.AppointmentId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateAppointment_WithNonExistingAppointmentId_ReturnsBadRequestObject()
        {
            //Arrange
            appointment.AppointmentId = nonExistingAppointmentId;

            //Act
            result = sut.UpdateAppointment(appointment, appointment.AppointmentId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockAppointmentRepository
                .Verify(cr => cr.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentService
                .Verify(cs => cs.Save(appointment.AppointmentId, appointment), Times.Never);
        }

        [TestMethod]
        public void PatchAppointment_WithValidPatchAppointmentData_ReturnsOkObjectResult()
        {
            //Arrange
            appointment.AppointmentId = existingAppointmentId;

            //Act
            result = sut.PatchAppointment(patchedAppointment, appointment.AppointmentId);

            //Assert

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockAppointmentService
                .Verify(cs => cs.Save(appointment.AppointmentId, appointment), Times.Once);

        }

        [TestMethod]
        public void PatchAppointment_WithNotValidPatchAppointmentData_ReturnsBadRequestResult()
        {
            //Arrange
            patchedAppointment = null;
            //Act
            result = sut.PatchAppointment(patchedAppointment, appointment.AppointmentId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockAppointmentService
                .Verify(cs => cs.Save(appointment.AppointmentId, appointment), Times.Never);
        }

        [TestMethod]
        public void PatchAppointment_WithNonExistingAppointmentId_ReturnsNotFoundResult()
        {
            //Arrange
            appointment.AppointmentId = nonExistingAppointmentId;

            //Act
            result = sut.PatchAppointment(patchedAppointment, appointment.AppointmentId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockAppointmentService
                .Verify(cs => cs.Save(appointment.AppointmentId, appointment), Times.Never);
        }

    }
}

