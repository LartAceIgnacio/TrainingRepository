using BlastAsia.DigiBook.API.Controllers;
using BlastAsia.DigiBook.Domain.Appointments.Services;
using BlastAsia.DigiBook.Domain.Contacts;
using BlastAsia.DigiBook.Domain.Employees;
using BlastAsia.DigiBook.Domain.Models.Appointments;
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
    }
}
