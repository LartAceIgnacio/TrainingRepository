using BlastAsia.DigiBook.Api.Controllers;
using BlastAsia.DigiBook.Domain.Appointments;
using BlastAsia.DigiBook.Domain.Models.Appointments;
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
    public class AppointmentsControllerTest
    {
        private Mock<IAppointmentRepository> mockAppointmentRepository;
        private Mock<IAppointmentService> mockAppointmentService;
        private AppointmentsController sut;
        private Appointment appointment;
        private JsonPatchDocument patch;

        [TestInitialize]
        public void TestInitialize()
        {
            mockAppointmentRepository = new Mock<IAppointmentRepository>();
            mockAppointmentService = new Mock<IAppointmentService>();
            sut = new AppointmentsController(mockAppointmentService.Object,
                mockAppointmentRepository.Object);

            patch = new JsonPatchDocument();

            appointment = new Appointment
            {
                AppointmentId = Guid.NewGuid(),
                AppointmentDate = DateTime.Today,
                GuestId = Guid.NewGuid(),
                HostId = Guid.NewGuid(),
                StartTime = DateTime.Now.TimeOfDay,
                EndTime = DateTime.Now.TimeOfDay.Add(TimeSpan.Parse("01:00:00")),
                IsCancelled = false,
                IsDone = true,
                Notes = "Sucess"
            };

            mockAppointmentRepository
               .Setup(c => c.Retreive())
               .Returns(new List<Appointment>());


            mockAppointmentRepository
                .Setup(c => c.Retrieve(appointment.AppointmentId))
                .Returns(appointment);

            mockAppointmentRepository
                .Setup(c => c.Create(appointment))
                .Returns(appointment);

            mockAppointmentRepository
                .Setup(c => c.Retrieve(Guid.Empty))
                .Returns<Appointment>(null);

        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        [TestMethod]
        public void GetAppointments_WithEmptyAppointmentId_ReturnsOkObjectValue()
        {
            //Arrange
            //Act
            var result = sut.GetAppointments(null);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockAppointmentRepository
                .Verify(c => c.Retreive(), Times.Once);
        }

        [TestMethod]
        public void GetAppointments_WithExistingAppointmentId_ReturnsOkObjectValue()
        {
            //Arrange
            //Act
            var result = sut.GetAppointments(appointment.AppointmentId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockAppointmentRepository
                .Verify(c => c.Retrieve(appointment.AppointmentId), Times.Once);
        }

        [TestMethod]
        public void DeleteAppointment_WithEmptyAppointmentId_ReturnsNotFound()
        {
            //Arrange
            appointment.AppointmentId = Guid.Empty;
            //Act
            var result = sut.DeleteAppointment(appointment.AppointmentId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockAppointmentRepository
                .Verify(c => c.Delete(appointment.AppointmentId), Times.Never);
        }

        [TestMethod]
        public void DeleteAppointment_WithExistingAppointmentId_ReturnsNoContent()
        {
            //Arrange
            //Act
            var result = sut.DeleteAppointment(appointment.AppointmentId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockAppointmentRepository
                .Verify(c => c.Delete(appointment.AppointmentId), Times.Once);
        }

        [TestMethod]
        public void CreateAppointment_WithEmptyAppointment_ReturnsBadRequest()
        {
            //Arrange          
            appointment = null;
            //Act
            var result = sut.CreateAppointment(appointment);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockAppointmentService
                .Verify(c => c.Save(Guid.Empty, appointment), Times.Never);
        }

        [TestMethod]
        public void CreateAppointment_WithExistingAppointment_ReturnsCreatedAtActionResult()
        {
            //Arrange
            //Act
            var result = sut.CreateAppointment(appointment);
            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockAppointmentService
                .Verify(c => c.Save(Guid.Empty, appointment), Times.Once);
        }

        [TestMethod]
        public void UpdateAppointment_WithEmptyAppointment_ReturnsBadRequest()
        {
            //Arrange
            appointment = null;
            //Act
            var result = sut.UpdateAppointment(appointment, Guid.Empty);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockAppointmentService
                .Verify(c => c.Save(Guid.Empty, appointment), Times.Never);
        }

        [TestMethod]
        public void UpdateAppointment_WithEmptyAppointmentId_ReturnsNotFound()
        {
            //Arrange
            appointment.AppointmentId = Guid.Empty;
            //Act
            var result = sut.UpdateAppointment(appointment, appointment.AppointmentId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockAppointmentRepository
                .Verify(c => c.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentService
                .Verify(c => c.Save(Guid.NewGuid(), appointment), Times.Never);
        }

        [TestMethod]
        public void UpdateAppointment_WithValidData_ReturnOkObjectResult()
        {
            //Arrange
            //Act
            var result = sut.UpdateAppointment(appointment, appointment.AppointmentId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockAppointmentRepository
                .Verify(c => c.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentService
                .Verify(c => c.Save(appointment.AppointmentId, appointment), Times.Once);
        }

        [TestMethod]
        public void PatchAppointment_WithEmptyPatchedAppointment_ReturnsBadRequest()
        {
            //Arrange
            patch = null;
            //Act
            var result = sut.PatchAppointment(patch, appointment.AppointmentId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockAppointmentService
                .Verify(c => c.Save(appointment.AppointmentId, appointment), Times.Never);
        }

        [TestMethod]
        public void PatchAppointment_WithEmptyAppointmentId_ReturnsNotFound()
        {
            //Arrange
            appointment.AppointmentId = Guid.Empty;
            //Act
            var result = sut.PatchAppointment(patch, appointment.AppointmentId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockAppointmentRepository
                .Verify(c => c.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentService
                .Verify(c => c.Save(appointment.AppointmentId, appointment), Times.Never);
        }

        [TestMethod]
        public void PatchAppointment_WithValidData_ReturnsOkObjectValue()
        {
            //Arrange
            //Act
            var result = sut.PatchAppointment(patch, appointment.AppointmentId);
            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockAppointmentRepository
                .Verify(c => c.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentService
                .Verify(c => c.Save(appointment.AppointmentId, appointment), Times.Once);
        }
    }
}
