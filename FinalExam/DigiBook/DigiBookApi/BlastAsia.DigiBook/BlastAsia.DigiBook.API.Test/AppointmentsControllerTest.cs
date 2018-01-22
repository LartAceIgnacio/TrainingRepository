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
        private Mock<IAppointmentRepository> mockAppointmentRepo;
        private Mock<IAppointmentService> mockAppointmentService;
        private AppointmentsController sut;
        private Appointment appointment;
        private Guid emptyAppointmentId;
        private Guid existingAppointmentId;
        private JsonPatchDocument patchedAppointment;


        [TestInitialize]
        public void InitializeTest()
        {
            mockAppointmentRepo = new Mock<IAppointmentRepository>();
            mockAppointmentService = new Mock<IAppointmentService>();
            sut = new AppointmentsController(mockAppointmentService.Object, mockAppointmentRepo.Object);
            appointment = new Appointment();
            existingAppointmentId = Guid.NewGuid();
            emptyAppointmentId = Guid.Empty;
            patchedAppointment = new JsonPatchDocument();

            mockAppointmentRepo
                .Setup(cr => cr.Retrieve(existingAppointmentId))
                .Returns(appointment);
            mockAppointmentRepo
                .Setup(cr => cr.Retrieve(emptyAppointmentId))
                .Returns<Appointment>(null);
        }

        [TestCleanup]
        public void CleanupTest()
        {

        }

        [TestMethod]
        public void GetAppointments_WithEmptyAppointmentId_ReturnsOkObjectValue()
        {
            // Arrange
            mockAppointmentRepo
                .Setup(cr => cr.Retrieve())
                .Returns(new List<Appointment>());

            // Act
            var result = sut.GetAppointments(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetAppointments_WithExistingAppointmentId_ReturnsOkObjectValue()
        {
            // Act
            var result = sut.GetAppointments(existingAppointmentId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateAppointment_WithEmptyAppointment_ReturnsBadRequest()
        {
            // Act
            var result = sut.CreateAppointment(null);

            // Assert
            mockAppointmentService
                .Verify(cs => cs.Save(Guid.Empty, appointment), Times.Never());
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }

        [TestMethod]
        public void CreateAppointment_WithValidAppointment_ReturnsNewAppointmentWithAppointmentId()
        {
            // Arrange
            mockAppointmentService
                .Setup(cs => cs.Save(Guid.Empty, appointment))
                .Returns(appointment);

            // Act
            var result = sut.CreateAppointment(appointment);

            // Assert
            mockAppointmentService
                .Verify(cs => cs.Save(Guid.Empty, appointment), Times.Once);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public void DeleteAppointment_WithEmptyAppointmentId_ReturnsNotFound()
        {
            // Act
            var result = sut.DeleteAppointment(emptyAppointmentId);

            // Assert
            mockAppointmentRepo
                .Verify(cr => cr.Delete(emptyAppointmentId), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteAppointment_WithExistingAppointmentId_ReturnsNoContent()
        {
            // Act
            var result = sut.DeleteAppointment(existingAppointmentId);

            // Assert
            mockAppointmentRepo
                .Verify(cr => cr.Delete(existingAppointmentId), Times.Once);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void UpdateAppointment_WithEmptyAppointment_ReturnsBadRequest()
        {
            // Act
            var result = sut.UpdateAppointment(null, existingAppointmentId);

            // Assert
            mockAppointmentService
                .Verify(cs => cs.Save(existingAppointmentId, null), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void UpdateAppointment_WithEmptyAppointmentId_ReturnsNotFound()
        {
            // Act
            var result = sut.UpdateAppointment(appointment, emptyAppointmentId);

            // Assert
            mockAppointmentService
                .Verify(cs => cs.Save(emptyAppointmentId, appointment), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void UpdateAppointment_WithExistingAppointmentIdAndAppointment_ReturnsOkObjectValue()
        {
            // Act
            var result = sut.UpdateAppointment(appointment, existingAppointmentId);

            // Assert
            mockAppointmentService
                .Verify(cs => cs.Save(existingAppointmentId, appointment), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void PatchAppointment_WithEmptyPatchedAppointment_ReturnsBadRequest()
        {
            // Arrange 
            patchedAppointment = null;

            // Act
            var result = sut.PatchAppointment(patchedAppointment, existingAppointmentId);

            // Assert
            mockAppointmentRepo
                .Verify(cr => cr.Retrieve(existingAppointmentId), Times.Never);
            mockAppointmentService
                .Verify(cs => cs.Save(existingAppointmentId, appointment), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }

        [TestMethod]
        public void PatchAppointment_WithEmptyAppointmentId_ReturnsNotFound()
        {
            // Act
            var result = sut.PatchAppointment(patchedAppointment, emptyAppointmentId);

            // Assert
            mockAppointmentService
                .Verify(cs => cs.Save(emptyAppointmentId, appointment), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PatchAppointment_WithExistingAppointmentId_ReturnsOkObjectValue()
        {
            // Act
            var result = sut.PatchAppointment(patchedAppointment, existingAppointmentId);

            // Assert
            mockAppointmentService
                .Verify(cs => cs.Save(existingAppointmentId, appointment), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
