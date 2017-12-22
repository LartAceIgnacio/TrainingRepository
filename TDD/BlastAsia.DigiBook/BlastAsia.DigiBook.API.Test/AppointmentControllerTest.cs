using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Appointments.Services;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
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
    public class AppointmentControllerTest
    {
        private Mock<IEmployeeRepository> _mockEmployeeRepo;
        private Mock<IContactRepository> _mockContactRepo;
        private Mock<IAppointmentRepository> _mockAppointmentRepo;
        private Mock<IAppointmentService> _mockAppointmentService;
        
        AppointmentController _sut;
        Appointment appointment;

        [TestInitialize]
        public void Initialize() {
            _mockEmployeeRepo = new Mock<IEmployeeRepository>();
            _mockContactRepo = new Mock<IContactRepository>();
            _mockAppointmentRepo = new Mock<IAppointmentRepository>();
            _mockAppointmentService = new Mock<IAppointmentService>();
            _sut = new AppointmentController(_mockAppointmentService.Object, _mockAppointmentRepo.Object
                , _mockEmployeeRepo.Object, _mockContactRepo.Object);
        }

        [TestMethod]
        [TestProperty("API Test", "Appointment")]
        public void GetAppontments_WithoutIdGetAllAppointments_ReturnsOkResult()
        {
            appointment = new Appointment();

            // Act
            var result = _sut.GetAppointments(null);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockAppointmentRepo.Verify(repo => repo.Retrieve(), Times.Once);

        }

        [TestMethod]
        [TestProperty("API Test", "Appointment")]
        public void GetAppointments_WithNonExistingId_ReturnsNoContentResult()
        {
            appointment = new Appointment();

            // Act
            var result = _sut.GetAppointments(Guid.Empty);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            _mockAppointmentService.Verify(service => service.Save(Guid.Empty, appointment), Times.Never);

        }

        [TestMethod]
        [TestProperty("API Test","Appointment")]
        public void PostAppointment_WithValidData_ReturnsOkResult()
        {
            // Arrange
            appointment = new Appointment();

            // Act
            var result = _sut.PostAppointment(appointment);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            _mockAppointmentService.Verify(service => service.Save(Guid.Empty, appointment));

        }

        [TestMethod]
        [TestProperty("API Test", "Appointment")]
        public void PostAppointment_WithNullAppointment_ReturnsBadRequest()
        {
            // Arrange

            // Act
            var result = _sut.PostAppointment(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockAppointmentService.Verify(service => service.Save(Guid.Empty, appointment), Times.Never);
        }

        [TestMethod]
        [TestProperty("API Test", "Appointment")]
        public void DeleteAppointment_WithValidId_ReturnsNoContent()
        {
            var guid = Guid.NewGuid();
            _mockAppointmentRepo.Setup(repo => repo.Retrieve(guid))
                .Returns(new Appointment());

            var result = _sut.DeleteAppoinment(guid);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            _mockAppointmentRepo.Verify(repo => repo.Delete(guid), Times.Once);
        }

        [TestMethod]
        [TestProperty("API Test", "Appointment")]
        public void DeleteAppointment_WithNoValidId_ReturnsNotFound()
        {
            // Assert
            var guid = Guid.Empty;
            _mockAppointmentRepo.Setup(repo => repo.Retrieve(guid))
                .Returns<Appointment>(null);

            // Act
            var result = _sut.DeleteAppoinment(guid);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            _mockAppointmentRepo.Verify(repo => repo.Delete(guid), Times.Never);

        }

        [TestMethod]
        [TestProperty("API Test", "Appointment")]
        public void UpdateAppointment_WithValidPatchDocument_ReturnsOkResult()
        {
            // Arrange
            var patchedDoc = new JsonPatchDocument();
            var apppointment = new Appointment();
            var guid = Guid.NewGuid();
            _mockAppointmentRepo.Setup(repo => repo.Retrieve(guid))
                .Returns(apppointment);

            // Act
            var result = _sut.PatchAppoinment(patchedDoc, guid);

            // Assert 
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockAppointmentService.Verify(service => service.Save(guid, apppointment), Times.Once);
            
        }

        [TestMethod]
        [TestProperty("API Test", "Appointment")]
        public void UpdateAppointment_WithNullPatchDocument_ReturnsBadRequest()
        {
            // Arrange
            JsonPatchDocument patchDoc = null;
            var appointment = new Appointment();
            var guid = Guid.NewGuid();

            // Act
            var result = _sut.PatchAppoinment(patchDoc, guid);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _mockAppointmentRepo.Verify(repo => repo.Retrieve(guid), Times.Never);
            _mockAppointmentService.Verify(service => service.Save(guid, appointment), Times.Never);

        }

        [TestMethod]
        [TestProperty("API Test", "Appointment")]
        public void UpdateAppointment_WithoutRetrievedAppointmentFromDb_ReturnsNotFoundResult()
        {
            // Arrange
            // Act
            var guid = Guid.NewGuid();
            var result = _sut.PatchAppoinment(new JsonPatchDocument(), guid);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            _mockAppointmentRepo.Verify(repo => repo.Retrieve(guid), Times.Once);
            _mockAppointmentService.Verify(service => service.Save(guid, new Appointment()), Times.Never);
        }
    }
}
