using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain;
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
        private Mock<IAppointmentRepository> mockAppointmentRepository;
        private Mock<IAppointmentService> mockAppointmentService;
        private AppointmentsController sut;
        private JsonPatchDocument patchedAppointment;

        [TestInitialize]
        public void Initialize()
        {
            mockAppointmentRepository = new Mock<IAppointmentRepository>();
            mockAppointmentService = new Mock<IAppointmentService>();
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
                Notes = "Sucess"
            };

            patchedAppointment = new JsonPatchDocument();

            mockAppointmentRepository
                .Setup(x => x.Retrieve())
                .Returns(() => new List<Appointment>
                {
                    new Appointment()
                });

            mockAppointmentRepository
                .Setup(x => x.Retrieve(appointment.AppointmentId))
                .Returns(appointment);
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void GetAppointments_WithEmptyAppointmentId_ReturnsOkObjectResult()
        {
            //Arrange

            //Act
            var result = sut.GetAppointments(null);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockAppointmentRepository.Verify(x => x.Retrieve(), Times.Once);
        }

        [TestMethod]
        public void GetAppointments_WithAppointmentId_ReturnsOkObjectResult()
        {
            //Arrange

            //Act
            var result = sut.GetAppointments(appointment.AppointmentId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockAppointmentRepository.Verify(x => x.Retrieve(appointment.AppointmentId), Times.Once);
        }

        [TestMethod]
        public void CreateAppointment_WithEmptyAppointment_ReturnsBadRequestResult()
        {
            //Arrange
            appointment = null;

            //Act
            var result = sut.CreateAppointment(appointment);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockAppointmentService.Verify(x => x.Save(Guid.Empty, appointment), Times.Never);
        }

        [TestMethod]
        public void CreateAppointment_WithAppointment_ReturnsCreatedAtActionResult()
        {
            //Arrange

            //Act
            var result = sut.CreateAppointment(appointment);

            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            mockAppointmentService.Verify(x => x.Save(Guid.Empty, appointment), Times.Once);
        }

        [TestMethod]
        public void CreateAppointment_WithFieldThatThrowsException_ReturnsBadRequestResult()
        {
            //Arrange
            appointment.AppointmentDate = DateTime.Today.AddDays(-5);

            mockAppointmentService
                .Setup(x => x.Save(Guid.Empty, appointment))
                .Throws(new Exception());

            //Act
            var result = sut.CreateAppointment(appointment);

            //Assert
            mockAppointmentService.Verify(x => x.Save(Guid.Empty, appointment), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteAppointment_WithNonExistingAppointmentId_ReturnsNotFoundResult()
        {
            //Arrange
            appointment.AppointmentId = Guid.Empty;

            //Act
            var result = sut.DeleteAppointment(appointment.AppointmentId);

            //Assert
            mockAppointmentRepository.Verify(x => x.Delete(appointment.AppointmentId), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteAppointment_WithExistingAppointmentId_ReturnsNoContentResult()
        {
            //Arrange


            //Act
            var result = sut.DeleteAppointment(appointment.AppointmentId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            mockAppointmentRepository.Verify(x => x.Delete(appointment.AppointmentId), Times.Once);

        }

        [TestMethod]
        public void UpdateAppointment_WithEmptyAppointment_ReturnsBadRequestResult()
        {
            //Arrange
            appointment = null;

            //Act
            var result = sut.UpdateAppointment(appointment, Guid.Empty);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockAppointmentRepository.Verify(x => x.Retrieve(Guid.NewGuid()), Times.Never);
            mockAppointmentService.Verify(x => x.Save(Guid.NewGuid(), appointment), Times.Never);
        }

        [TestMethod]
        public void UpdateAppointment_WithAppointmentButNonExistingAppointmentId_ReturnsNotFoundResult()
        {
            //Arrange
            appointment.AppointmentId = Guid.Empty;

            //Act
            var result = sut.UpdateAppointment(appointment, appointment.AppointmentId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockAppointmentRepository.Verify(x => x.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentService.Verify(x => x.Save(Guid.NewGuid(), appointment), Times.Never);
        }

        [TestMethod]
        public void UpdateAppointment_WithAppointmentAndExistingAppointmentId_ReturnsOkObjectResult()
        {
            //Arrange


            //Act
            var result = sut.UpdateAppointment(appointment, appointment.AppointmentId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockAppointmentRepository.Verify(x => x.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentService.Verify(x => x.Save(appointment.AppointmentId, appointment), Times.Once);
        }

        [TestMethod]
        public void UpdateAppointment_WithFieldThatThrowsException_ReturnsBadRequestResult()
        {
            //Arrange
            appointment.HostId = Guid.Empty;

            mockAppointmentService
                .Setup(x => x.Save(appointment.AppointmentId, appointment))
                .Throws(new Exception());


            //Act
            var result = sut.UpdateAppointment(appointment, appointment.AppointmentId);

            //Assert
            mockAppointmentService.Verify(x => x.Save(appointment.AppointmentId, appointment), Times.Once);
            mockAppointmentRepository.Verify(x => x.Retrieve(appointment.AppointmentId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }


        [TestMethod]
        public void PatchAppointment_WithEmptyPacthedAppointment_ReturnsBadRequestResult()
        {
            //Arrange
            patchedAppointment = null;

            //Act
            var result = sut.PatchAppointment(patchedAppointment, Guid.Empty);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            mockAppointmentRepository.Verify(x => x.Retrieve(Guid.NewGuid()), Times.Never);
            mockAppointmentService.Verify(x => x.Save(Guid.NewGuid(), appointment), Times.Never);
        }

        [TestMethod]
        public void PatchAppointment_WithPatchedAppointmentButNonExistingAppointmentId_ReturnsNotFoundResult()
        {
            //Arrange

            //Act
            var result = sut.PatchAppointment(patchedAppointment, Guid.Empty);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            mockAppointmentRepository.Verify(x => x.Retrieve(Guid.Empty), Times.Once);
            mockAppointmentService.Verify(x => x.Save(Guid.NewGuid(), appointment), Times.Never);
        }

        [TestMethod]
        public void PatchAppointment_WithPatchedAppointmentAndExistingAppointmentId_ReturnsOkObjectResult()
        {
            //Arrange


            //Act
            var result = sut.PatchAppointment(patchedAppointment, appointment.AppointmentId);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockAppointmentRepository.Verify(x => x.Retrieve(appointment.AppointmentId), Times.Once);
            mockAppointmentService.Verify(x => x.Save(appointment.AppointmentId, appointment), Times.Once);
        }

        [TestMethod]
        public void PatchAppointment_WithFieldThatThrowsException_ReturnsBadRequestResult()
        {
            //Arrange
            patchedAppointment.Replace("StartTime", "9:00");
            patchedAppointment.Replace("EndTime", "8:00");

            mockAppointmentService
                .Setup(x => x.Save(appointment.AppointmentId, appointment))
                .Throws(new Exception());


            //Act
            var result = sut.PatchAppointment(patchedAppointment, appointment.AppointmentId);

            //Assert
            mockAppointmentService.Verify(x => x.Save(appointment.AppointmentId, appointment), Times.Once);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
