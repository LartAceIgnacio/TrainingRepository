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
        private Appointment appointment;
        private Mock<IAppointmentService> mockAppointmentService;
        private Mock<IAppointmentRepository> mockAppointmentRepository;
        private AppointmentsController sut;
        private readonly Guid existingAppointmentId = Guid.NewGuid();
        private readonly Guid nonExistingAppointmentId = Guid.Empty;
        private JsonPatchDocument patchedAppointment;

        [TestInitialize]
        public void Initialize()
        {
            appointment = new Appointment
            {
                AppointmentId = existingAppointmentId,
                GuestId = existingAppointmentId,
                HostId = existingAppointmentId,
                AppointmentDate = DateTime.Now,
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("02:00:00")),
                IsCancelled = false,
                IsDone = false,
                Notes = "Bring coffee."
            };

            mockAppointmentService = new Mock<IAppointmentService>();
            mockAppointmentRepository = new Mock<IAppointmentRepository>();
            sut = new AppointmentsController(mockAppointmentService.Object, mockAppointmentRepository.Object);

            patchedAppointment = new JsonPatchDocument();
            patchedAppointment.Replace("GuestId", Guid.NewGuid());

            mockAppointmentRepository
                .Setup(c => c.Retrieve(existingAppointmentId))
                .Returns(appointment);

            mockAppointmentService
                .Setup(c => c.Save(existingAppointmentId, appointment))
                .Returns(appointment);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }
        [TestMethod]
        public void GetAppointment_WithEmptyAppointmentId_ReturnsOkObjectResult()
        {
            //Arrange
            mockAppointmentRepository
                .Setup(c => c.Retrieve())
                .Returns(() => new List<Appointment>{
                       new Appointment()
                       });
            //Act
            var result = sut.GetAppointment(null);

            //Assert
            mockAppointmentRepository.Verify(c => c.Retrieve(), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetAppointment_WithExistingAppointmentId_ReturnsOkObjectResult()
        {
            //Arrange

            //Act
            var result = sut.GetAppointment(appointment.AppointmentId);

            //Assert
            mockAppointmentRepository.Verify(c => c.Retrieve(appointment.AppointmentId), Times.Once());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateAppointment_WithValidAppointmentData_ReturnsCreatedAtActionResult()
        {
            //Arrange
            appointment.AppointmentId = nonExistingAppointmentId;

            mockAppointmentService
                .Setup(c => c.Save(appointment.AppointmentId, appointment))
                .Returns(appointment);

            //Act
            var result = sut.CreateAppointment(appointment);

            //Assert
            mockAppointmentService.Verify(c => c.Save(appointment.AppointmentId, appointment), Times.Once());
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void CreateAppointment_WithInvalidAppointmentData_ReturnsBadRequestObjectResult()
        {
            //Arrange
            appointment.GuestId = Guid.Empty;
            appointment.AppointmentId = nonExistingAppointmentId;

            mockAppointmentService
                .Setup(c => c.Save(appointment.GuestId, appointment))
                .Throws(new GuestIdDoesNotExistException(""));

            //Act
            var result = sut.CreateAppointment(appointment);

            //Assert
            mockAppointmentService.Verify(c => c.Save(appointment.AppointmentId, appointment), Times.Once());
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void CreateAppointment_WithEmptyAppointmentData_ReturnsBadRequestResult()
        {
            //Arrange
            appointment = null;

            //Act
            var result = sut.CreateAppointment(appointment);

            //Assert
            mockAppointmentService.Verify(c => c.Save(existingAppointmentId, appointment), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Delete_WithExistingAppointmentId_ReturnsNoContentResult()
        {
            //Arrange

            //Act
            var result = sut.Delete(appointment.AppointmentId);

            //Assert
            mockAppointmentRepository.Verify(c => c.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentRepository.Verify(c => c.Delete(appointment.AppointmentId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void Delete_WithNonExistingAppointmentId_ReturnsNotFoundResult()
        {
            //Arrange
            appointment.AppointmentId = nonExistingAppointmentId;

            //Act
            var result = sut.Delete(appointment.AppointmentId);

            //Assert
            mockAppointmentRepository.Verify(c => c.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentRepository.Verify(c => c.Delete(appointment.AppointmentId), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void UpdateAppointment_WithExistingAppointmentIdAndData_ReturnsOkObjectResult()
        {
            //Arrange

            //Act
            var result = sut.UpdateAppointment(appointment, appointment.AppointmentId);

            //Assert
            mockAppointmentRepository.Verify(c => c.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentService.Verify(c => c.Save(appointment.AppointmentId, appointment), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateAppointment_WithEmptyAppointmentData_ReturnsBadRequestResult()
        {
            //Arrange
            appointment = null;

            //Act
            var result = sut.UpdateAppointment(appointment, nonExistingAppointmentId);

            //Assert
            mockAppointmentRepository.Verify(c => c.Retrieve(nonExistingAppointmentId), Times.Never);
            mockAppointmentService.Verify(c => c.Save(nonExistingAppointmentId, appointment), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateAppointment_WithInvalidAppointmentData_ReturnsBadRequestObjectResult()
        {
            //Arrange
            appointment.GuestId = Guid.Empty;

            mockAppointmentService
                .Setup(c => c.Save(appointment.AppointmentId, appointment))
                .Throws(new GuestIdDoesNotExistException(""));

            //Act
            var result = sut.UpdateAppointment(appointment, appointment.AppointmentId);

            //Assert
            mockAppointmentRepository.Verify(c => c.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentService.Verify(c => c.Save(appointment.AppointmentId, appointment), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void UpdateAppointment_WithNonExistingId_ReturnsNotFound()
        {
            //Arrange
            appointment.AppointmentId = nonExistingAppointmentId;

            //Act
            var result = sut.UpdateAppointment(appointment, appointment.AppointmentId);

            //Assert
            mockAppointmentRepository.Verify(c => c.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentService.Verify(c => c.Save(appointment.AppointmentId, appointment), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchAppointment_WithExistingIdAndValidData_ReturnOkObjectResult()
        {
            //Arrange

            //Act
            var result = sut.PatchAppointment(patchedAppointment, appointment.AppointmentId);

            //Assert
            mockAppointmentRepository.Verify(c => c.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentService.Verify(c => c.Save(appointment.AppointmentId, appointment), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchAppointment_WithEmptyAppointmentData_ReturnsBadRequestResult()
        {
            //Arrange
            patchedAppointment = null;

            //Act
            var result = sut.PatchAppointment(patchedAppointment, nonExistingAppointmentId);

            //Assert
            mockAppointmentRepository.Verify(c => c.Retrieve(nonExistingAppointmentId), Times.Never);
            mockAppointmentService.Verify(c => c.Save(nonExistingAppointmentId, appointment), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PatchAppointment_WithInvalidAppointmentData_ReturnsBadRequestObjectResult()
        {
            //Arrange
            patchedAppointment.Replace("GuestId", Guid.Empty);

            mockAppointmentService
                .Setup(c => c.Save(appointment.GuestId, appointment))
                .Throws(new GuestIdDoesNotExistException(""));

            //Act
            var result = sut.PatchAppointment(patchedAppointment, appointment.AppointmentId);

            //Assert
            mockAppointmentRepository.Verify(c => c.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentService.Verify(c => c.Save(appointment.AppointmentId, appointment), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void PatchAppointment_WithNonExistingId_ReturnsNotFound()
        {
            //Arrange
            appointment.AppointmentId = nonExistingAppointmentId;

            //Act
            var result = sut.PatchAppointment(patchedAppointment, appointment.AppointmentId);

            //Assert
            mockAppointmentRepository.Verify(c => c.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentService.Verify(c => c.Save(appointment.AppointmentId, appointment), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
