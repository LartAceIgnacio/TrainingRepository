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
        private Appointment appointment;
        private AppointmentsController sut;
        private JsonPatchDocument patchedAppointment;

        [TestInitialize]
        public void Initialize()
        {
            mockAppointmentService = new Mock<IAppointmentService>();
            mockAppointmentRepository = new Mock<IAppointmentRepository>();
            sut = new AppointmentsController(mockAppointmentService.Object, mockAppointmentRepository.Object);
            appointment = new Appointment {
                AppointmentId = Guid.NewGuid(),
                AppointmentDate = DateTime.Today,
                GuestId = Guid.NewGuid(),
                HostId = Guid.NewGuid(),
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("01:00:00")),
                IsCancelled = false,
                IsDone = true,
                Notes = "Ongoing"
            };

            patchedAppointment = new JsonPatchDocument();
            patchedAppointment.Replace("Notes", "Not done yet");

            mockAppointmentRepository
               .Setup(a => a.Retrieve(appointment.AppointmentId))
               .Returns(appointment);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void GetAppointments_WithEmptyAppointmentId_ReturnOkObjectResult()
        {
            // Arrange
            mockAppointmentRepository
                .Setup(a => a.Retrieve())
                .Returns(new List<Appointment>());

            // Act
            var result = sut.GetAppointments(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockAppointmentRepository.Verify(a => a.Retrieve(), Times.Once);
        }

        [TestMethod]
        public void GetAppointments_WithValidAppointmentId_ReturnOkObjectResult()
        {
            // Arrange
            mockAppointmentRepository
                .Setup(c => c.Retrieve(Guid.NewGuid()))
                .Returns(appointment);

            // Act
            var result = sut.GetAppointments(appointment.AppointmentId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockAppointmentRepository.Verify(a => a.Retrieve(appointment.AppointmentId), Times.Once);
        }

        [TestMethod]
        public void CreateAppointment_AppointmentWithValidData_ReturnCreatedAtActionResult()
        {
            // Act
            var result = sut.CreateAppointment(appointment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockAppointmentService.Verify(a => a.Save(Guid.Empty, appointment), Times.Once);
        }

        [TestMethod]
        public void CreateAppointment_AppointmentWithNoData_ReturnBadRequestResult()
        {
            // Arrange
            appointment = null;

            // Act
            var result = sut.CreateAppointment(appointment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockAppointmentService.Verify(a => a.Save(Guid.Empty, appointment), Times.Never);
        }

        [TestMethod]
        public void DeleteAppointment_AppointmentDeleted_ReturnNoContentResult()
        {
            // Act
            var result = sut.DeleteAppointment(appointment.AppointmentId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockAppointmentRepository.Verify(a => a.Delete(appointment.AppointmentId), Times.Once);
        }

        [TestMethod]
        public void DeleteAppointment_WithoutAppointmentId_ReturnNotFoundResult()
        {
            // Arrange
            mockAppointmentRepository.
                Setup(a => a.Retrieve(appointment.AppointmentId))
                .Returns<Appointment>(null);

            // Act
            var result = sut.DeleteAppointment(appointment.AppointmentId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockAppointmentRepository.Verify(a => a.Delete(appointment.AppointmentId), Times.Never);
        }

        [TestMethod]
        public void UpdateAppointment_WithExistingAppointmentDataAndId_ReturnOkObjectResult()
        {
            // Act
            var result = sut.UpdateAppointment(appointment, appointment.AppointmentId);

            // Assert
            mockAppointmentRepository.Verify(a => a.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentService.Verify(a => a.Save(appointment.AppointmentId, appointment), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateAppointment_AppointmentWithoutValue_ReturnBadRequestResult()
        {
            // Arrange
            appointment = null;

            // Act
            var result = sut.UpdateAppointment(appointment, Guid.Empty);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockAppointmentRepository.Verify(a => a.Retrieve(Guid.Empty), Times.Never);
            mockAppointmentService.Verify(a => a.Save(Guid.Empty, appointment), Times.Never);
        }

        [TestMethod]
        public void UpdateAppointment_WithNoExistingAppointmentId_ReturnNotFoundResult()
        {
            // Arrange
            appointment.AppointmentId = Guid.Empty;

            // Act
            var result = sut.UpdateAppointment(appointment, appointment.AppointmentId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockAppointmentRepository.Verify(a => a.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentService.Verify(a => a.Save(appointment.AppointmentId, appointment), Times.Never);
        }

        [TestMethod]
        public void PatchAppointment_WithExistingAppointmentDataAndId_ReturnOkObjectResult()
        {
            // Act
            var result = sut.PatchAppointment(patchedAppointment, appointment.AppointmentId);

            // Assert
            mockAppointmentService.Verify(a => a.Save(appointment.AppointmentId, appointment), Times.Once);
            mockAppointmentRepository.Verify(a => a.Retrieve(appointment.AppointmentId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchAppointment_WithoutExistingAppointmentDataAndId_ReturnNotFoundResult()
        {
            // Arrange
            mockAppointmentRepository
              .Setup(a => a.Retrieve(appointment.AppointmentId))
              .Returns<Appointment>(null);

            // Act
            var result = sut.PatchAppointment(patchedAppointment, appointment.AppointmentId);

            // Assert
            mockAppointmentService.Verify(a => a.Save(appointment.AppointmentId, appointment), Times.Never);
            mockAppointmentService.Verify(a => a.Save(appointment.AppointmentId, appointment), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchAppointment_AppointmentWithEmptyPatchDocument_ReturnBadRequestResult()
        {
            // Arrange
            patchedAppointment = null;

            // Act
            var result = sut.PatchAppointment(patchedAppointment, appointment.AppointmentId);

            // Assert
            mockAppointmentRepository.Verify(a => a.Retrieve(appointment.AppointmentId), Times.Never);
            mockAppointmentService.Verify(a => a.Save(appointment.AppointmentId, appointment), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
